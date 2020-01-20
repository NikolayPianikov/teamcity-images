using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal interface IReadmeGenerator
    {
        IEnumerable<ReadmeFile> Generate(IEnumerable<DockerFile> dockerFiles);
    }
}