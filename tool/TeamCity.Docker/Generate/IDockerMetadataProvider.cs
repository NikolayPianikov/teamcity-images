using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal interface IDockerMetadataProvider
    {
        Metadata GetMetadata([NotNull] IEnumerable<DockerLine> dockerFileContent);
    }
}