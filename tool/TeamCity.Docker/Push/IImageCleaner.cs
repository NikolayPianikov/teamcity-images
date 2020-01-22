using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeamCity.Docker.Push
{
    internal interface IImageCleaner
    {
        Task<Result> CleanImages(IEnumerable<DockerImage> images);
    }
}