using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Build
{
    internal class ImageCleaner : IImageCleaner
    {
        private static readonly ImageDeleteParameters DeleteParameters = new ImageDeleteParameters {Force = true, PruneChildren = false};
        private readonly ILogger _logger;
        [NotNull] private readonly IDockerConverter _dockerConverter;
        [NotNull] private readonly ITaskRunner<IDockerClient> _taskRunner;

        public ImageCleaner(
            [NotNull] ILogger logger,
            [NotNull] IDockerConverter dockerConverter,
            [NotNull] ITaskRunner<IDockerClient> taskRunner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dockerConverter = dockerConverter;
            _taskRunner = taskRunner ?? throw new ArgumentNullException(nameof(taskRunner));
        }

        public async Task<Result> CleanImages(IEnumerable<DockerImage> images)
        {
            if (images == null)
            {
                throw new ArgumentNullException(nameof(images));
            }

            var cleared = false;
            foreach (var image in images.OrderByDescending(i => i.RepoTag))
            {
                cleared = true;
                var name = string.IsNullOrWhiteSpace(image.RepoTag) ? image.Info.ID : image.RepoTag;
                _logger.Log($"Delete {_dockerConverter.TryConvertConvertHashToImageId(image.Info.ID)} {image.RepoTag}");
                await _taskRunner.Run(client => client.Images.DeleteImageAsync(name, DeleteParameters));
            }

            if (!cleared)
            {
                _logger.Log("Nothing to clean.");
            }

            return Result.Success;
        }
    }
}
