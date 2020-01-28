using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using IoC;

namespace TeamCity.Docker.Push
{
    internal interface IImagePublisher
    {
        [NotNull] Task<Result> PushImages([NotNull] IDockerClient dockerClient, [NotNull] IEnumerable<DockerImage> images);
    }
}