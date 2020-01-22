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

        public DockerFileGenerator(
            IDockerFileContentParser fileContentParser,
            IDockerMetadataProvider metadataProvider)
        {
            _fileContentParser = fileContentParser;
            _metadataProvider = metadataProvider;
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