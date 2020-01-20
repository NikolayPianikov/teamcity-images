using System.Collections.Generic;
using System.IO;
using System.Linq;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class DockerFileGenerator : IDockerFileGenerator
    {
        private readonly IDockerVariableReplacer _variableReplacer;
        private readonly IDockerMetadataProvider _metadataProvider;
        private readonly ILogger _logger;

        public DockerFileGenerator(
            IDockerVariableReplacer variableReplacer,
            IDockerMetadataProvider metadataProvider,
            ILogger logger)
        {
            _variableReplacer = variableReplacer;
            _metadataProvider = metadataProvider;
            _logger = logger;
        }

        public DockerFile Generate(string buildPath, string template, IReadOnlyDictionary<string, string> values)
        {
            var dockerFileContent = _variableReplacer.Replace(template, values);
            var dockerFilePath = Path.Combine(buildPath, "Dockerfile");
            var metadata = _metadataProvider.GetMetadata(dockerFileContent);
            _logger.Log($"The docker file \"{dockerFilePath}\" with values {string.Join(", ", values.Select(i => $"{i.Key}={i.Value}"))} was generated.");
            return new DockerFile(dockerFilePath, metadata, dockerFileContent);
        }
    }
}