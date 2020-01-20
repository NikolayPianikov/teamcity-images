using System.Collections.Generic;
using System.Threading.Tasks;
using TeamCity.Docker.Generate;

namespace TeamCity.Docker.Build
{
    internal interface IImageBuilder
    {
        Task<Result> Build(IReadOnlyCollection<DockerFile> dockerFiles);
    }
}