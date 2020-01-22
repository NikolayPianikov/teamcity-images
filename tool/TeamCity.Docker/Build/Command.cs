// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IoC;

namespace TeamCity.Docker.Build
{
    internal class Command: ICommand<IOptions>
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<Generate.IGenerator> _dockerFileGenerators;
        private readonly IImageBuilder _imageBuilder;

        public Command(
            [NotNull] ILogger logger,
            [NotNull] IEnumerable<Generate.IGenerator> dockerFileGenerators,
            [NotNull] IImageBuilder imageBuilder)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dockerFileGenerators = dockerFileGenerators ?? throw new ArgumentNullException(nameof(dockerFileGenerators));
            _imageBuilder = imageBuilder ?? throw new ArgumentNullException(nameof(imageBuilder));
        }

        public async Task<Result> Run()
        {
            var dockerFiles = new List<Generate.DockerFile>();
            using (_logger.CreateBlock("Generate docker files"))
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

            using (_logger.CreateBlock("Build docker images"))
            {
                return await _imageBuilder.Build(dockerFiles);
            }
        }
    }
}
