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
        [NotNull] private readonly ITaskRunner<IDockerClient> _taskRunner;

        public ImageCleaner(
            [NotNull] ILogger logger,
            [NotNull] ITaskRunner<IDockerClient> taskRunner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _taskRunner = taskRunner ?? throw new ArgumentNullException(nameof(taskRunner));
        }

        public async Task<Result> CleanImages(IEnumerable<DockerImage> images)
        {
            if (images == null)
            {
                throw new ArgumentNullException(nameof(images));
            }

            using (_logger.CreateBlock("Clean"))
            {
                foreach (var image in images)
                {
                    await _taskRunner.Run(client => client.Images.DeleteImageAsync(image.RepoTag, new ImageDeleteParameters {Force = true, PruneChildren = true}));
                }
            }

            return Result.Success;
        }
    }
}
