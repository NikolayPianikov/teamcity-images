using System;
using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal struct DockerFileVariant
    {
        [NotNull] public readonly string BuildPath;
        [NotNull] public readonly IReadOnlyDictionary<string, string> Variables;

        public DockerFileVariant([NotNull] string buildPath, [NotNull] IReadOnlyDictionary<string, string> variables)
        {
            BuildPath = buildPath ?? throw new ArgumentNullException(nameof(buildPath));
            Variables = variables ?? throw new ArgumentNullException(nameof(variables));
        }
    }
}
