using System;
using IoC;

// ReSharper disable ParameterTypeCanBeEnumerable.Local
// ReSharper disable MemberCanBePrivate.Global

namespace TeamCity.Docker.Generate
{
    internal struct ReadmeFile
    {
        [NotNull] public readonly string Path;
        [NotNull] public readonly string Content;

        public ReadmeFile([NotNull] string path, [NotNull] string content)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
    }
}
