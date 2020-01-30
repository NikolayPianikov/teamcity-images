using System.Collections.Generic;
using System.Threading.Tasks;
using IoC;

namespace TeamCity.Docker.Build
{
    internal interface IImagePublisher
    {
        [NotNull] Task<Result<IEnumerable<DockerImage>>> PushImages([NotNull] IEnumerable<DockerImage> images);
    }
}