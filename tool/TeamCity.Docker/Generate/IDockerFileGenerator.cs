using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal interface IDockerFileGenerator
    {
        DockerFile Generate([NotNull] string buildPath, [NotNull] string template, [NotNull] IReadOnlyDictionary<string, string> values);
    }
}
