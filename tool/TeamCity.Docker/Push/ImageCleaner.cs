using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Push
{
    internal class ImageCleaner : IImageCleaner
    {
        private readonly ILogger _logger;

        public ImageCleaner(
            [NotNull] ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result> CleanImages(IDockerClient dockerClient, IEnumerable<DockerImage> images)
        {
            if (dockerClient == null)
            {
                throw new ArgumentNullException(nameof(dockerClient));
            }

            if (images == null)
            {
                throw new ArgumentNullException(nameof(images));
            }

            using (_logger.CreateBlock("Clean docker images"))
            {
                foreach (var image in images)
                {
                    await dockerClient.Images.DeleteImageAsync(image.RepoTag, new ImageDeleteParameters { Force = true, PruneChildren = true });
                }
            }

            return Result.Success;
        }
    }
}
