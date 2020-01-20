using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal interface IGenerator
    {
        IEnumerable<DockerFile> Generate();
    }
}
