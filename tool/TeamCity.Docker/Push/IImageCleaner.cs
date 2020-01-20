using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace TeamCity.Docker.Push
{
    internal interface IImageCleaner
    {
        Task<Result> CleanImages(IEnumerable<ImagesListResponse> images);
    }
}