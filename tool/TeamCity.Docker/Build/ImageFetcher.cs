using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Build
{
    internal class ImageFetcher : IImageFetcher
    {
        private readonly ILogger _logger;
        private readonly IDockerConverter _dockerConverter;
        [NotNull] private readonly ITaskRunner<IDockerClient> _taskRunner;

        public ImageFetcher(
            [NotNull] ILogger logger,
            [NotNull] IDockerConverter dockerConverter,
            [NotNull] ITaskRunner<IDockerClient> taskRunner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dockerConverter = dockerConverter ?? throw new ArgumentNullException(nameof(dockerConverter));
            _taskRunner = taskRunner ?? throw new ArgumentNullException(nameof(taskRunner));
        }

        public async Task<Result<IReadOnlyList<DockerImage>>> GetImages(IReadOnlyDictionary<string, string> filters)
        {
            using (_logger.CreateBlock("List"))
            {
                foreach (var (key, value) in filters)
                {
                    _logger.Log($"where {key}={value}");
                }

                var dockerFilters = new Dictionary<string, IDictionary<string, bool>>
                {
                    { "label", filters.ToDictionary(filter => $"{filter.Key}={filter.Value}", filter => true) }
                };

                var dockerImages = await _taskRunner.Run(client => client.Images.ListImagesAsync(new ImagesListParameters {Filters = dockerFilters}));
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
