using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal interface IDockerMetadataProvider
    {
        Metadata GetMetadata(IEnumerable<DockerLine> dockerFileContent);
    }
}