using System;
using System.Collections.Generic;
using IoC;

// ReSharper disable ParameterTypeCanBeEnumerable.Local
// ReSharper disable MemberCanBePrivate.Global

namespace TeamCity.Docker.Generate
{
    internal struct ReadmeFile
    {
        [NotNull] public readonly string Path;
        [NotNull] public readonly string Content;
        [NotNull] public readonly IEnumerable<DockerFile> DockerFiles;

        public ReadmeFile([NotNull] string path, [NotNull] string content, [NotNull] IReadOnlyCollection<DockerFile> dockerFiles)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Content = content ?? throw new ArgumentNullException(nameof(content));
            DockerFiles = dockerFiles ?? throw new ArgumentNullException(nameof(dockerFiles));
        }
    }
}
