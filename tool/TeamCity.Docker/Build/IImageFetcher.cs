using System.Collections.Generic;
using System.Threading.Tasks;
using IoC;

namespace TeamCity.Docker.Build
{
    internal interface IImageFetcher
    {
        [NotNull] Task<Result<IReadOnlyList<DockerImage>>> GetImages([NotNull] IReadOnlyDictionary<string, string> filters);
    }
}