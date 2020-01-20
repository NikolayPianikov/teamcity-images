using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using TeamCity.Docker.Generate;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Build
{
    internal class ImageBuilder : IImageBuilder
    {
        private readonly IOptions _options;
        private readonly ILogger _logger;
        private readonly IDockerClient _dockerClient;
        private readonly IMessageLogger _messageLogger;
        private readonly IFileSystem _fileSystem;
        private readonly IStreamService _streamService;
        private readonly IPathService _pathService;
        private readonly IContextFactory _contextFactory;

        public ImageBuilder(
            IOptions options,
            ILogger logger,
            IDockerClient dockerClient,
            IMessageLogger messageLogger,
            IFileSystem fileSystem,
            IStreamService streamService,
            IPathService pathService,
            IContextFactory contextFactory)
        {
            _options = options;
            _logger = logger;
            _dockerClient = dockerClient;
            _messageLogger = messageLogger;
            _fileSystem = fileSystem;
            _streamService = streamService;
            _pathService = pathService;
            _contextFactory = contextFactory;
        }

        public async Task<Result> Build(IReadOnlyCollection<DockerFile> dockerFiles)
        {
            var dockerFilesRootPath = _fileSystem.UniqueName;
            await using var contextStream = await _contextFactory.Create(dockerFilesRootPath, dockerFiles);
            var labels = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(_options.SessionId))
            {
                labels.Add("SessionId", _options.SessionId);
            }

            var hasError = false;
            foreach (var dockerFile in dockerFiles.OrderBy(dockerFile => dockerFile.Metadata.Priority))
            {
                using (_logger.CreateBlock(dockerFile.ToString()))
                {
                    _logger.Log($"The dockerfile is \"{dockerFile.Path}\"");
                    contextStream.Position = 0;
                    var dockerFilePathInContext = _pathService.Normalize(Path.Combine(dockerFilesRootPath, dockerFile.Path));
                    try
                    {
                        await using var buildEventStream = await _dockerClient.Images.BuildImageFromDockerfileAsync(
                            contextStream,
                            new ImageBuildParameters
                            {
                                Dockerfile = dockerFilePathInContext,
                                Tags = dockerFile.Metadata.Tags.ToList(),
                                Labels = labels
                            },
                            CancellationToken.None);

                        _streamService.ProcessLines(
                            buildEventStream,
                            line =>
                            {
                                if (_messageLogger.Log(line) == Result.Error)
                                {
                                    hasError = true;
                                }
                            });

                        if (hasError)
                        {
                            return Result.Error;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Log($"Error: {ex.Message}", Result.Error);
                        return Result.Error;
                    }
                }
            }

            return Result.Success;
        }
    }
}
