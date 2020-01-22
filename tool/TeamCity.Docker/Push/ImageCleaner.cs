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
        private readonly IDockerClient _dockerClient;

        public ImageCleaner(
            [NotNull] ILogger logger,
            [NotNull] IDockerClient dockerClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dockerClient = dockerClient ?? throw new ArgumentNullException(nameof(dockerClient));
        }

        public async Task<Result> CleanImages(IEnumerable<DockerImage> images)
        {
            if (images == null)
            {
                throw new ArgumentNullException(nameof(images));
            }

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
