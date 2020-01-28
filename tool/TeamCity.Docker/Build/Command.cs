// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using IoC;

namespace TeamCity.Docker.Build
{
    internal class Command: ICommand<IOptions>
    {
        [NotNull] private readonly ILogger _logger;
        [NotNull] private readonly IEnumerable<Generate.IGenerator> _dockerFileGenerators;
        [NotNull] private readonly IFileSystem _fileSystem;
        [NotNull] private readonly IGate<IDockerClient> _gate;
        [NotNull] private readonly IContextFactory _contextFactory;
        [NotNull] private readonly IImageBuilder _imageBuilder;
        [NotNull] private readonly Push.IImageFetcher _imageFetcher;

        public Command(
            [NotNull] ILogger logger,
            [NotNull] IEnumerable<Generate.IGenerator> dockerFileGenerators,
            [NotNull] IFileSystem fileSystem,
            [NotNull] IGate<IDockerClient> gate,
            [NotNull] IContextFactory contextFactory,
            [NotNull] IImageBuilder imageBuilder,
            [NotNull] Push.IImageFetcher imageFetcher)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dockerFileGenerators = dockerFileGenerators ?? throw new ArgumentNullException(nameof(dockerFileGenerators));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _gate = gate ?? throw new ArgumentNullException(nameof(gate));
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _imageBuilder = imageBuilder ?? throw new ArgumentNullException(nameof(imageBuilder));
            _imageFetcher = imageFetcher ?? throw new ArgumentNullException(nameof(imageFetcher));
        }

        public async Task<Result> Run()
        {
            var dockerFiles = new List<Generate.DockerFile>();
            using (_logger.CreateBlock("Generate"))
            {
                foreach (var generator in _dockerFileGenerators)
                {
                    var newDockerFiles = generator.Generate();
                    if (newDockerFiles.State == Result.Error)
                    {
                        return Result.Error;
                    }

                    dockerFiles.AddRange(newDockerFiles.Value);
                }

                if (dockerFiles.Count == 0)
                {
                    _logger.Log("Docker files were not generated.", Result.Error);
                    return Result.Error;
                }
            }

            var dockerFilesRootPath = _fileSystem.UniqueName;
            var contextStreamResult = await _contextFactory.Create(dockerFilesRootPath, dockerFiles);
            if (contextStreamResult.State == Result.Error)
            {
                return Result.Error;
            }

            Result result;
            using (_logger.CreateBlock("Build"))
            using (contextStreamResult.Value)
            {
                result = await _gate.Run(client => _imageBuilder.Build(client, dockerFiles, dockerFilesRootPath, contextStreamResult.Value));
            }

            using (_logger.CreateBlock("List"))
            {
                await _gate.Run(client => _imageFetcher.GetImages(client));
            }

            return result;
        }
    }
}
