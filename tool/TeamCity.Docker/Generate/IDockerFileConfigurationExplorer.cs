using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal interface IDockerFileConfigurationExplorer
    {
        Result<IEnumerable<DockerFileConfiguration>> Explore([NotNull] string sourcePath, [NotNull] IEnumerable<string> configurationFiles);
    }
}