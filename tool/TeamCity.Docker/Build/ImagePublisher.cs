using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using IoC;

// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Build
{
    internal class ImagePublisher : IImagePublisher
    {
        private readonly ILogger _logger;
        private readonly IMessageLogger _messageLogger;
        private readonly IOptions _options;
        private readonly IDockerConverter _dockerConverter;
        [NotNull] private readonly IImageFetcher _imageFetcher;
        [NotNull] private readonly ITaskRunner<IDockerClient> _taskRunner;

        public ImagePublisher(
            [NotNull] ILogger logger,
            [NotNull] IMessageLogger messageLogger,
            [NotNull] IOptions options,
            [NotNull] IDockerConverter dockerConverter,
            [NotNull] IImageFetcher imageFetcher,
            [NotNull] ITaskRunner<IDockerClient> taskRunner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageLogger = messageLogger ?? throw new ArgumentNullException(nameof(messageLogger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _dockerConverter = dockerConverter ?? throw new ArgumentNullException(nameof(dockerConverter));
            _imageFetcher = imageFetcher ?? throw new ArgumentNullException(nameof(imageFetcher));
            _taskRunner = taskRunner ?? throw new ArgumentNullException(nameof(taskRunner));
        }

        public async Task<Result<IEnumerable<DockerImage>>> PushImages(IEnumerable<DockerImage> images)
        {
            if (images == null)
            {
                throw new ArgumentNullException(nameof(images));
            }

            var result = new List<DockerImage>();
            var authConfig = new AuthConfig {ServerAddress = _options.ServerAddress, Username = _options.Username, Password = _options.Password};
            foreach (var image in images)
            {
                if (string.IsNullOrWhiteSpace(image.RepoTag))
                {
                    continue;
                }

                var tag = _dockerConverter.TryConvertRepoTagToTag(image.RepoTag);
                var repositoryName = _options.Username + "/" + _dockerConverter.TryConvertRepoTagToRepositoryName(image.RepoTag);
                var newRepoTag = $"{repositoryName}:{tag}";
                _logger.Log($"Add tag {newRepoTag} for {image.RepoTag}.");
                await _taskRunner.Run(client => client.Images.TagImageAsync(
                    image.RepoTag,
                    new ImageTagParameters {RepositoryName = repositoryName, Tag = tag, Force = true}));

                _logger.Log($"Push {newRepoTag}.");
                var imagePushParameters = new ImagePushParameters {ImageID = image.Info.ID};
                var pushResult = await _taskRunner.Run(client => PushImage(client, newRepoTag, imagePushParameters, authConfig));

                var imagesAfter = await _imageFetcher.GetImages(new Dictionary<string, string>(), false);

                _logger.Log($"Delete tag {newRepoTag}.");
                await _taskRunner.Run(client => client.Images.DeleteImageAsync(newRepoTag, new ImageDeleteParameters {Force = true, PruneChildren = false}));

                if (pushResult == Result.Error || imagesAfter.State == Result.Error)
                {
                    return new Result<IEnumerable<DockerImage>>(Enumerable.Empty<DockerImage>(), Result.Error);
                }

                var publishedImages = imagesAfter.Value.Where(i => i.Info.ID == image.Info.ID && i.RepoTag == newRepoTag);
                result.AddRange(publishedImages);
            }

            return new Result<IEnumerable<DockerImage>>(result);
        }

        private async Task PushImage(IDockerClient client, string repoTag, ImagePushParameters imagePushParameters, AuthConfig authConfig) =>
            await client.Images.PushImageAsync(
                repoTag,
                imagePushParameters,
                authConfig,
                new Progress<JSONMessage>(message =>
                {
                    _messageLogger.Log(message);
                }));
        }
    }
