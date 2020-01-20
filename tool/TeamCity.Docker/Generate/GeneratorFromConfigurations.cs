using System.Collections.Generic;
using System.Linq;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class GeneratorFromConfigurations: IGenerator
    {
        private readonly Docker.IOptions _options;
        private readonly IDockerFileConfigurationExplorer _dockerFileConfigurationExplorer;
        private readonly IDockerFileGenerator _dockerFileGenerator;

        public GeneratorFromConfigurations(
            Docker.IOptions options,
            IDockerFileConfigurationExplorer dockerFileConfigurationExplorer,
            IDockerFileGenerator dockerFileGenerator)
        {
            _options = options;
            _dockerFileConfigurationExplorer = dockerFileConfigurationExplorer;
            _dockerFileGenerator = dockerFileGenerator;
        }

        public IEnumerable<DockerFile> Generate()
        {
            if (string.IsNullOrWhiteSpace(_options.SourcePath))
            {
                return Enumerable.Empty<DockerFile>();
            }

            return 
                from dockerFileConfiguration in _dockerFileConfigurationExplorer.Explore(_options.SourcePath)
                from dockerFileVariant in dockerFileConfiguration.Variants
                select _dockerFileGenerator.Generate(dockerFileVariant.BuildPath, dockerFileConfiguration.DockerfileTemplateContent, dockerFileVariant.Variables);
            
        }
    }
}
