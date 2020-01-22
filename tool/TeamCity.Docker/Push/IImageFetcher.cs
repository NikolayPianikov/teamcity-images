using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamCity.Docker.Push
{
    internal interface IImageFetcher
    {
        Task<Result<IReadOnlyList<DockerImage>>> GetImages();
    }
}