// ReSharper disable ClassNeverInstantiated.Global

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamCity.Docker.Build
{
    internal class Command: ICommand<IOptions>
    {
        private readonly ILogger _logger;
        private readonly IEnumerable<Generate.IGenerator> _dockerFileGenerators;
        private readonly IImageBuilder _imageBuilder;

        public Command(
            ILogger logger,
            IEnumerable<Generate.IGenerator> dockerFileGenerators,
            IImageBuilder imageBuilder)
        {
            _logger = logger;
            _dockerFileGenerators = dockerFileGenerators;
            _imageBuilder = imageBuilder;
        }

        public async Task<Result> Run()
        {
            List<Generate.DockerFile> dockerFiles;
            using (_logger.CreateBlock("Generate docker files"))
            {
                dockerFiles = _dockerFileGenerators.SelectMany(generator => generator.Generate()).ToList();
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
