using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Push
{
    internal class ImageCleaner : IImageCleaner
    {
        private readonly ILogger _logger;
        private readonly IDockerClient _dockerClient;

        public ImageCleaner(
            ILogger logger,
            IDockerClient dockerClient)
        {
            _logger = logger;
            _dockerClient = dockerClient;
        }

        public async Task<Result> CleanImages(IEnumerable<DockerImage> images)
        {
            using (_logger.CreateBlock("Clean docker images"))
            {
                foreach (var image in images)
                {
                    await _dockerClient.Images.DeleteImageAsync(image.RepoTag, new ImageDeleteParameters { Force = true, PruneChildren = true });
                }
            }

            return Result.Success;
        }
    }
}
