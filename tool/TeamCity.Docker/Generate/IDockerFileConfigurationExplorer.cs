using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal interface IDockerFileConfigurationExplorer
    {
        Result<IEnumerable<DockerFileConfiguration>> Explore(string sourcePath, IEnumerable<string> configurationFiles);
    }
}