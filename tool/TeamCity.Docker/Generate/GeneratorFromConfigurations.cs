using System.Collections.Generic;
using System.Linq;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class GeneratorFromConfigurations: IGenerator
    {
        private readonly Docker.IOptions _options;
        private readonly ILogger _logger;
        private readonly IDockerFileConfigurationExplorer _dockerFileConfigurationExplorer;
        private readonly IDockerFileGenerator _dockerFileGenerator;

        public GeneratorFromConfigurations(
            Docker.IOptions options,
            ILogger logger,
            IDockerFileConfigurationExplorer dockerFileConfigurationExplorer,
            IDockerFileGenerator dockerFileGenerator)
        {
            _options = options;
            _logger = logger;
            _dockerFileConfigurationExplorer = dockerFileConfigurationExplorer;
            _dockerFileGenerator = dockerFileGenerator;
        }

        public Result<IEnumerable<DockerFile>> Generate()
        {
            if (string.IsNullOrWhiteSpace(_options.SourcePath))
            {
                _logger.Log("Path to configuration directory is empty.", Result.Error);
                return new Result<IEnumerable<DockerFile>>(Enumerable.Empty<DockerFile>(), Result.Error);
            }

            var configurationFiles = _options.ConfigurationFiles.Split(',');
            var configurationsResult = _dockerFileConfigurationExplorer.Explore(_options.SourcePath, configurationFiles);
            if (configurationsResult.State == Result.Error)
            {
                return new Result<IEnumerable<DockerFile>>(Enumerable.Empty<DockerFile>(), Result.Error);
            }

            return new Result<IEnumerable<DockerFile>>(GetDockerFiles(configurationsResult.Value));
        }

        private IEnumerable<DockerFile> GetDockerFiles(IEnumerable<DockerFileConfiguration> configurations)
        {
            return
                from dockerFileConfiguration in configurations
                from dockerFileVariant in dockerFileConfiguration.Variants
                select _dockerFileGenerator.Generate(dockerFileVariant.BuildPath, dockerFileConfiguration.DockerfileTemplateContent, dockerFileVariant.Variables);
        }
    }
}
