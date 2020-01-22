using System;
using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal struct DockerFile
    {
        [NotNull] public readonly string Path;
        public readonly Metadata Metadata;
        [NotNull] public readonly IReadOnlyCollection<DockerLine> Content;

        public DockerFile([NotNull] string path, Metadata metadata, [NotNull] IReadOnlyCollection<DockerLine> content)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Metadata = metadata;
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }

        public override string ToString() => $"{string.Join(", ", Metadata.Tags)}";
    }
}
