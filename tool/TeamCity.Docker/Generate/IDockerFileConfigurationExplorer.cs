using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal interface IDockerFileConfigurationExplorer
    {
        IEnumerable<DockerFileConfiguration> Explore(string sourcePath);
    }
}