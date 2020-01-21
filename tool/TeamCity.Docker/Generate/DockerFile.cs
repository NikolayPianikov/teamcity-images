using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal struct DockerFile
    {
        public readonly string Path;
        public readonly Metadata Metadata;
        public readonly IReadOnlyCollection<DockerLine> Content;

        public DockerFile(string path, Metadata metadata, IReadOnlyCollection<DockerLine> content)
        {
            Path = path;
            Metadata = metadata;
            Content = content;
        }

        public override string ToString() => $"{string.Join(", ", Metadata.Tags)}";
    }
}
