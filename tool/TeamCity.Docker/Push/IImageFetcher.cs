using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using IoC;

namespace TeamCity.Docker.Push
{
    internal interface IImageFetcher
    {
        [NotNull] Task<Result<IReadOnlyList<DockerImage>>> GetImages([NotNull] IDockerClient dockerClient);
    }
}