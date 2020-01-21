using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal interface IGenerator
    {
        Result<IEnumerable<DockerFile>> Generate();
    }
}
