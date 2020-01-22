using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal interface IDockerFileContentParser
    {
        [NotNull] IEnumerable<DockerLine> Parse([NotNull] string text, [NotNull] IReadOnlyDictionary<string, string> values);
    }
}