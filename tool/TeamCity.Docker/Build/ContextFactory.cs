using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Tar;
using TeamCity.Docker.Generate;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Build
{
    internal class ContextFactory : IContextFactory
    {
        private readonly IOptions _options;
        private readonly ILogger _logger;
        private readonly IPathService _pathService;
        private readonly IFileSystem _fileSystem;
        private readonly IStreamService _streamService;
        private static readonly int Chmod = Convert.ToInt32("100755", 8);

        public ContextFactory(
            IOptions options,
            ILogger logger,
            IPathService pathService,
            IFileSystem fileSystem,
            IStreamService streamService)
        {
            _options = options;
            _logger = logger;
            _pathService = pathService;
            _fileSystem = fileSystem;
            _streamService = streamService;
        }

        public async Task<Result<Stream>> Create(string dockerFilesRootPath, IEnumerable<DockerFile> dockerFiles)
        {
            using (_logger.CreateBlock("Create docker context"))
            {
                var context = new MemoryStream();
                await using var archive = new TarOutputStream(context) { IsStreamOwner = false };
                var number = 0;

                if (!string.IsNullOrWhiteSpace(_options.ContextPath))
                {
                    var path = Path.GetFullPath(_options.ContextPath);
                    _logger.Log($"The context path is \"{path}\" (\"{_options.ContextPath}\")");
                    if (!_fileSystem.IsDirectoryExist(path))
                    {
                        throw new InvalidOperationException($"The context directory \"{path}\" does not exist.");
                    }

                    _logger.Log($"The docker files root path in the context is \"{dockerFilesRootPath}\"");

                    foreach (var file in _fileSystem.EnumerateFileSystemEntries(path, "*.*"))
                    {
                        if (!_fileSystem.IsFileExist(file))
                        {
                            continue;
                        }

                        await using var fileStream = _fileSystem.OpenRead(file);
                        var filePathInArchive = _pathService.Normalize(Path.GetRelativePath(path, file));
                        var result = await AddEntry(++number, archive, filePathInArchive, fileStream);
                        if (result == Result.Error)
                        {
                            return new Result<Stream>(new MemoryStream(), Result.Error);
                        }
                    }
                }
                else
                {
                    _logger.Log("The context was not defined.", Result.Warning);
                }

                foreach (var dockerFile in dockerFiles)
                {
                    var content = string.Join(System.Environment.NewLine, dockerFile.Content.Select(line => line.Text));
                    var contentBytes=Encoding.UTF8.GetBytes(content);
                    await using var dockerFileStream = new MemoryStream(contentBytes);
                    var dockerFilePathInArchive = _pathService.Normalize(Path.Combine(dockerFilesRootPath, dockerFile.Path));
                    var result = await AddEntry(++number, archive, dockerFilePathInArchive, dockerFileStream);
                    if (result == Result.Error)
                    {
                        return new Result<Stream>(new MemoryStream(), Result.Error);
                    }
                }

                archive.Close();
                context.Position = 0;
                return new Result<Stream>(context);
            }
        }

        private async Task<Result> AddEntry(int number, TarOutputStream archive, string filePathInArchive, Stream contentStream)
        {
            var entry = TarEntry.CreateTarEntry(filePathInArchive);
            entry.Size = contentStream.Length;
            entry.TarHeader.Mode = Chmod; //chmod 755
            archive.PutNextEntry(entry);
            var result = await _streamService.Copy(contentStream, archive, $"Adding {filePathInArchive}");
            archive.CloseEntry();
            _logger.Log($"{number:000000} \"{filePathInArchive}\" was added ({contentStream.Length} bytes).");
            return result;
        }
    }
}
