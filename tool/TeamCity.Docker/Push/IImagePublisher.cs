using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace TeamCity.Docker.Push
{
    internal interface IImagePublisher
    {
        Task<Result> PushImages(IEnumerable<ImagesListResponse> images);
    }
}