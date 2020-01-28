using System.Collections.Generic;
using System.Threading.Tasks;
using IoC;

namespace TeamCity.Docker.Push
{
    internal interface IImageCleaner
    {
        [NotNull] Task<Result> CleanImages([NotNull] IEnumerable<DockerImage> images);
    }
}