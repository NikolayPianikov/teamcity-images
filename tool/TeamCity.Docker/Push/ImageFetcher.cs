using System;
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
        private readonly IDockerClient _dockerClient;
        private readonly IDockerConverter _dockerConverter;

        public ImageFetcher(
            [NotNull] ILogger logger,
            [NotNull] Docker.IOptions options,
            [NotNull] IDockerClient dockerClient,
            [NotNull] IDockerConverter dockerConverter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _dockerClient = dockerClient ?? throw new ArgumentNullException(nameof(dockerClient));
            _dockerConverter = dockerConverter ?? throw new ArgumentNullException(nameof(dockerConverter));
        }

        public async Task<Result<IReadOnlyList<DockerImage>>> GetImages()
        {
            using (_logger.CreateBlock("List docker images"))
            {
                var filters = new Dictionary<string, IDictionary<string, bool>> { { "label", new Dictionary<string, bool> { { $"SessionId={_options.SessionId}", true } } } };
                var dockerImages = await _dockerClient.Images.ListImagesAsync(new ImagesListParameters { Filters = filters });

                var images = (
                    from image in (
                        from image in dockerImages
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
                        var historyEntries = await _dockerClient.Images.GetImageHistoryAsync(image.RepoTag, CancellationToken.None);
                        foreach (var historyEntry in historyEntries)
                        {
                            _logger.Log($"{historyEntry.ID} {historyEntry.Created} {historyEntry.Size:D10} {historyEntry.CreatedBy}");
                        }
                    }
                }

                return new Result<IReadOnlyList<DockerImage>>(images);
            }
        }
    }
}
