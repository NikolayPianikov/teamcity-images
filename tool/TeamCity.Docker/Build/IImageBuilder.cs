using System.Collections.Generic;
using System.Threading.Tasks;
using IoC;
using TeamCity.Docker.Generate;

namespace TeamCity.Docker.Build
{
    internal interface IImageBuilder
    {
        [NotNull] Task<Result> Build([NotNull] IReadOnlyCollection<DockerFile> dockerFiles);
    }
}