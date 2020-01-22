using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class DockerFileGenerator : IDockerFileGenerator
    {
        private readonly IDockerFileContentParser _fileContentParser;
        private readonly IDockerMetadataProvider _metadataProvider;

        public DockerFileGenerator(
            [NotNull] IDockerFileContentParser fileContentParser,
            [NotNull] IDockerMetadataProvider metadataProvider)
        {
            _fileContentParser = fileContentParser ?? throw new ArgumentNullException(nameof(fileContentParser));
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
        }

        public DockerFile Generate(string buildPath, string template, IReadOnlyDictionary<string, string> values)
        {
            if (buildPath == null)
            {
                throw new ArgumentNullException(nameof(buildPath));
            }

            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var dockerFileContent = _fileContentParser.Parse(template, values).ToList();
            var dockerFilePath = Path.Combine(buildPath, "Dockerfile");
            var metadata = _metadataProvider.GetMetadata(dockerFileContent);
            return new DockerFile(dockerFilePath, metadata, dockerFileContent);
        }
    }
}