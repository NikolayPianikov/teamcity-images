﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Push
{
    internal class ImageFetcher : IImageFetcher
    {
        private readonly ILogger _logger;
        private readonly Docker.IOptions _options;
        private readonly IDockerConverter _dockerConverter;
        [NotNull] private readonly ITaskRunner<IDockerClient> _taskRunner;

        public ImageFetcher(
            [NotNull] ILogger logger,
            [NotNull] Docker.IOptions options,
            [NotNull] IDockerConverter dockerConverter,
            [NotNull] ITaskRunner<IDockerClient> taskRunner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _dockerConverter = dockerConverter ?? throw new ArgumentNullException(nameof(dockerConverter));
            _taskRunner = taskRunner ?? throw new ArgumentNullException(nameof(taskRunner));
        }

        public async Task<Result<IReadOnlyList<DockerImage>>> GetImages()
        {
            using (_logger.CreateBlock("List"))
            {
                var filters = new Dictionary<string, IDictionary<string, bool>> {{"label", new Dictionary<string, bool> {{$"SessionId={_options.SessionId}", true}}}};
                var dockerImages = await _taskRunner.Run(client => client.Images.ListImagesAsync(new ImagesListParameters {Filters = filters}));

                var images = (
                        from image in (
                            from image in dockerImages.Value
                            where image.RepoTags != null
                            from tag in image.RepoTags
                            where !tag.Contains("<none>")
                            select new DockerImage(image, tag))
                        select image)
                    .ToList();

                foreach (var image in images)
                {
                    using (_logger.CreateBlock(image.RepoTag))
                    {
                        _logger.Log($"{_dockerConverter.TryConvertConvertHashToImageId(image.Info.ID)} {image.Info.Created}");
                        var historyEntries = await _taskRunner.Run(client => client.Images.GetImageHistoryAsync(image.RepoTag, CancellationToken.None));
                        foreach (var historyEntry in historyEntries.Value)
                        {
                            _logger.Log($"{_dockerConverter.TryConvertConvertHashToImageId(historyEntry.ID)} {historyEntry.Created} {historyEntry.Size:D10} {historyEntry.CreatedBy}");
                        }
                    }
                }

                return new Result<IReadOnlyList<DockerImage>>(images);
            }
        }
    }
}
