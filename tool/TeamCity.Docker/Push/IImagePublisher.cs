using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamCity.Docker.Push
{
    internal interface IImagePublisher
    {
        Task<Result> PushImages(IEnumerable<DockerImage> images);
    }
}