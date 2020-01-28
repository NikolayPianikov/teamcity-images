using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using IoC;

namespace TeamCity.Docker.Push
{
    internal interface IImageCleaner
    {
        [NotNull] Task<Result> CleanImages([NotNull] IDockerClient dockerClient, [NotNull] IEnumerable<DockerImage> images);
    }
}