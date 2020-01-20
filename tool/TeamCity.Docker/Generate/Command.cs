// ReSharper disable ClassNeverInstantiated.Global

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TeamCity.Docker.Generate
{
    internal class Command: ICommand<IOptions>
    {
        private readonly IOptions _options;
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;
        private readonly IEnumerable<IGenerator> _dockerFileGenerators;
        private readonly IReadmeGenerator _readmeGenerator;

        public Command(
            IOptions options,
            ILogger logger,
            IFileSystem fileSystem,
            IEnumerable<IGenerator> dockerFileGenerators,
            IReadmeGenerator readmeGenerator)
        {
            _options = options;
            _logger = logger;
            _fileSystem = fileSystem;
            _dockerFileGenerators = dockerFileGenerators;
            _readmeGenerator = readmeGenerator;
        }

        public Task<Result> Run()
        {
            var dockerFiles = new List<DockerFile>();
            using (_logger.CreateBlock("Generate docker files"))
            {
                foreach (var generator in _dockerFileGenerators)
                {
                    dockerFiles.AddRange(generator.Generate());
                }

                if (dockerFiles.Count == 0)
                {
                    _logger.Log("Docker files were not generated.", Result.Error);
                    return Task.FromResult(Result.Error);
                }
            }

            List<ReadmeFile> readmeFiles;
            using (_logger.CreateBlock("Generate readme files"))
            {
                readmeFiles = _readmeGenerator.Generate(dockerFiles).ToList();
                if (readmeFiles.Count == 0)
                {
                    _logger.Log("Docker files were not generated.", Result.Warning);
                }
            }

            using (_logger.CreateBlock("Save docker files"))
            {
                foreach (var dockerFile in dockerFiles)
                {
                    _fileSystem.WriteFile(Path.Combine(_options.TargetPath, dockerFile.Path), dockerFile.Content);
                }
            }

            using (_logger.CreateBlock("Save readme files"))
            {
                foreach (var readmeFile in readmeFiles)
                {
                    _fileSystem.WriteFile(Path.Combine(_options.TargetPath, readmeFile.Path), readmeFile.Content);
                }
            }

            return Task.FromResult(Result.Success);
        }
    }
}
