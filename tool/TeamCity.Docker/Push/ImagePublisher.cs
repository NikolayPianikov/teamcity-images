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
        [NotNull] private readonly ITaskRunner<IDockerClient> _taskRunner;

        public ImagePublisher(
            [NotNull] ILogger logger,
            [NotNull] IMessageLogger messageLogger,
            [NotNull] IOptions options,
            [NotNull] IDockerConverter dockerConverter,
            [NotNull] ITaskRunner<IDockerClient> taskRunner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageLogger = messageLogger ?? throw new ArgumentNullException(nameof(messageLogger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _dockerConverter = dockerConverter ?? throw new ArgumentNullException(nameof(dockerConverter));
            _taskRunner = taskRunner ?? throw new ArgumentNullException(nameof(taskRunner));
        }

        public async Task<Result> PushImages(IEnumerable<DockerImage> images)
        {
            if (images == null)
            {
                throw new ArgumentNullException(nameof(images));
            }

            using (_logger.CreateBlock("Push"))
            {
                var authConfig = new AuthConfig {ServerAddress = _options.ServerAddress, Username = _options.Username, Password = _options.Password};
                foreach (var image in images)
                {
                    var tag = _dockerConverter.TryConvertRepoTagToTag(image.RepoTag);
                    var repositoryName = _options.Username + "/" + _dockerConverter.TryConvertRepoTagToRepositoryName(image.RepoTag);
                    var newRepoTag = $"{repositoryName}:{tag}";
                    _logger.Log($"Add tag {newRepoTag} for {image.RepoTag}.");
                    await _taskRunner.Run(client => client.Images.TagImageAsync(
                        image.RepoTag,
                        new ImageTagParameters {RepositoryName = repositoryName, Tag = tag, Force = true}));

                    _logger.Log($"Push {newRepoTag}.");
                    var imagePushParameters = new ImagePushParameters {ImageID = image.Info.ID};
                    var result = await _taskRunner.Run(client => PushImage(client, newRepoTag, imagePushParameters, authConfig));
                    
                    _logger.Log($"Delete tag {newRepoTag}.");
                    await _taskRunner.Run(client => client.Images.DeleteImageAsync(newRepoTag, new ImageDeleteParameters {Force = true, PruneChildren = false}));

                    if (result == Result.Error)
                    {
                        return Result.Error;
                    }
                }
            }

            return Result.Success;
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
