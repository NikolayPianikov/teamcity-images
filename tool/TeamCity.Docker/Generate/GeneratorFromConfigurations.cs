using System;
using System.Collections.Generic;
using System.Linq;
using IoC;

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
            [NotNull] Docker.IOptions options,
            [NotNull] ILogger logger,
            [NotNull] IDockerFileConfigurationExplorer dockerFileConfigurationExplorer,
            [NotNull] IDockerFileGenerator dockerFileGenerator)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dockerFileConfigurationExplorer = dockerFileConfigurationExplorer ?? throw new ArgumentNullException(nameof(dockerFileConfigurationExplorer));
            _dockerFileGenerator = dockerFileGenerator ?? throw new ArgumentNullException(nameof(dockerFileGenerator));
        }

        public Result<IEnumerable<DockerFile>> Generate()
        {
            if (string.IsNullOrWhiteSpace(_options.SourcePath))
            {
                _logger.Log("Path to configuration directory is empty.", Result.Error);
                return new Result<IEnumerable<DockerFile>>(Enumerable.Empty<DockerFile>(), Result.Error);
            }

            var configurationsResult = _dockerFileConfigurationExplorer.Explore(_options.SourcePath, _options.ConfigurationFiles);
            if (configurationsResult.State == Result.Error)
            {
                return new Result<IEnumerable<DockerFile>>(Enumerable.Empty<DockerFile>(), Result.Error);
            }

            return new Result<IEnumerable<DockerFile>>(GetDockerFiles(configurationsResult.Value));
        }

        private IEnumerable<DockerFile> GetDockerFiles([NotNull] IEnumerable<DockerFileConfiguration> configurations)
        {
            if (configurations == null)
            {
                throw new ArgumentNullException(nameof(configurations));
            }

            return from dockerFileConfiguration in configurations
                from dockerFileVariant in dockerFileConfiguration.Variants
                select _dockerFileGenerator.Generate(dockerFileVariant.BuildPath, dockerFileConfiguration.DockerfileTemplateContent, dockerFileVariant.Variables);
        }
    }
}
