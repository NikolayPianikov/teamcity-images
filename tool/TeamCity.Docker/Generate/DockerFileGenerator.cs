using System.Collections.Generic;
using System.IO;
using System.Linq;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class DockerFileGenerator : IDockerFileGenerator
    {
        private readonly IDockerFileContentParser _fileContentParser;
        private readonly IDockerMetadataProvider _metadataProvider;
        private readonly ILogger _logger;

        public DockerFileGenerator(
            IDockerFileContentParser fileContentParser,
            IDockerMetadataProvider metadataProvider,
            ILogger logger)
        {
            _fileContentParser = fileContentParser;
            _metadataProvider = metadataProvider;
            _logger = logger;
        }

        public DockerFile Generate(string buildPath, string template, IReadOnlyDictionary<string, string> values)
        {
            var dockerFileContent = _fileContentParser.Parse(template, values).ToList();
            var dockerFilePath = Path.Combine(buildPath, "Dockerfile");
            var metadata = _metadataProvider.GetMetadata(dockerFileContent);
            return new DockerFile(dockerFilePath, metadata, dockerFileContent);
        }
    }
}