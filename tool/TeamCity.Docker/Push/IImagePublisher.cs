using System.Collections.Generic;
using System.Threading.Tasks;
using IoC;

namespace TeamCity.Docker.Push
{
    internal interface IImagePublisher
    {
        [NotNull] Task<Result> PushImages([NotNull] IEnumerable<DockerImage> images);
    }
}