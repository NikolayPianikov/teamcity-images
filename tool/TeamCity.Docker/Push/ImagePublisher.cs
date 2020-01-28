using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using IoC;

// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Push
{
    internal class ImagePublisher : IImagePublisher
    {
        private readonly ILogger _logger;
        private readonly IMessageLogger _messageLogger;
        private readonly IOptions _options;
        private readonly IDockerConverter _dockerConverter;

        public ImagePublisher(
            [NotNull] ILogger logger,
            [NotNull] IMessageLogger messageLogger,
            [NotNull] IOptions options,
            [NotNull] IDockerConverter dockerConverter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageLogger = messageLogger ?? throw new ArgumentNullException(nameof(messageLogger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _dockerConverter = dockerConverter ?? throw new ArgumentNullException(nameof(dockerConverter));
        }

        public async Task<Result> PushImages(IDockerClient dockerClient, IEnumerable<DockerImage> images)
        {
            if (dockerClient == null)
            {
                throw new ArgumentNullException(nameof(dockerClient));
            }

            if (images == null)
            {
                throw new ArgumentNullException(nameof(images));
            }

            var authConfig = new AuthConfig {ServerAddress = _options.ServerAddress, Username = _options.Username, Password = _options.Password};
            foreach (var image in images)
            {
                var tag = _dockerConverter.TryConvertRepoTagToTag(image.RepoTag);
                var repositoryName = _options.Username + "/" + _dockerConverter.TryConvertRepoTagToRepositoryName(image.RepoTag);
                var newRepoTag = $"{repositoryName}:{tag}";
                _logger.Log($"Add tag {newRepoTag} for {image.RepoTag}.");
                await dockerClient.Images.TagImageAsync(
                    image.RepoTag,
                    new ImageTagParameters {RepositoryName = repositoryName, Tag = tag, Force = true});

                _logger.Log($"Push {newRepoTag}.");
                var hasError = false;
                await dockerClient.Images.PushImageAsync(
                    newRepoTag,
                    new ImagePushParameters {ImageID = image.Info.ID},
                    authConfig,
                    new Progress<JSONMessage>(message =>
                    {
                        if (_messageLogger.Log(message) == Result.Error)
                        {
                            hasError = true;
                        }
                    }));

                _logger.Log($"Delete tag {newRepoTag}.");
                await dockerClient.Images.DeleteImageAsync(newRepoTag, new ImageDeleteParameters {Force = true, PruneChildren = false});

                if (hasError)
                {
                    return Result.Error;
                }
            }

            return Result.Success;
        }
    }
}
