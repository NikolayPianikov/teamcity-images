using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Push
{
    internal class ImagePublisher : IImagePublisher
    {
        private readonly ILogger _logger;
        private readonly IMessageLogger _messageLogger;
        private readonly IOptions _options;
        private readonly IDockerClient _dockerClient;
        private readonly IDockerConverter _dockerConverter;

        public ImagePublisher(
            ILogger logger,
            IMessageLogger messageLogger,
            IOptions options,
            IDockerClient dockerClient,
            IDockerConverter dockerConverter)
        {
            _logger = logger;
            _messageLogger = messageLogger;
            _options = options;
            _dockerClient = dockerClient;
            _dockerConverter = dockerConverter;
        }

        public async Task<Result> PushImages(IEnumerable<ImagesListResponse> images)
        {
            using (_logger.CreateBlock("Push docker images"))
            {
                var authConfig = new AuthConfig { ServerAddress = _options.ServerAddress, Username = _options.Username, Password = _options.Password };
                foreach (var image in images)
                {
                    foreach (var repoTag in image.RepoTags)
                    {
                        var tag = _dockerConverter.TryConvertRepoTagToTag(repoTag);
                        var repositoryName = _options.Username + "/" + _dockerConverter.TryConvertRepoTagToRepositoryName(repoTag);
                        var newRepoTag = $"{repositoryName}:{tag}";
                        _logger.Log($"Add tag {newRepoTag} for {repoTag}.");
                        await _dockerClient.Images.TagImageAsync(
                            repoTag,
                            new ImageTagParameters { RepositoryName = repositoryName, Tag = tag, Force = true });

                        _logger.Log($"Push {newRepoTag}.");
                        var hasError = false;
                        await _dockerClient.Images.PushImageAsync(
                            newRepoTag,
                            new ImagePushParameters { ImageID = image.ID },
                            authConfig,
                            new Progress<JSONMessage>(message =>
                            {
                                if (_messageLogger.Log(message) == Result.Error)
                                {
                                    hasError = true;
                                }
                            }));

                        _logger.Log($"Delete tag {newRepoTag}.");
                        await _dockerClient.Images.DeleteImageAsync(newRepoTag, new ImageDeleteParameters { Force = true, PruneChildren = false });

                        if (hasError)
                        {
                            return Result.Error;
                        }
                    }
                }
            }

            return Result.Success;
        }
    }
}
