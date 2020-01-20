using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Push
{
    internal class ImageFetcher : IImageFetcher
    {
        private readonly ILogger _logger;
        private readonly IOptions _options;
        private readonly IDockerClient _dockerClient;
        private readonly IDockerConverter _dockerConverter;

        public ImageFetcher(
            ILogger logger,
            IOptions options,
            IDockerClient dockerClient,
            IDockerConverter dockerConverter)
        {
            _logger = logger;
            _options = options;
            _dockerClient = dockerClient;
            _dockerConverter = dockerConverter;
        }

        public async Task<Result<IReadOnlyList<ImagesListResponse>>> GetImages()
        {
            using (_logger.CreateBlock("List docker images"))
            {
                var filters = new Dictionary<string, IDictionary<string, bool>> { { "label", new Dictionary<string, bool> { { $"SessionId={_options.SessionId}", true } } } };
                var images = await _dockerClient.Images.ListImagesAsync(new ImagesListParameters { Filters = filters });
                foreach (var image in images)
                {
                    foreach (var repoTag in image.RepoTags)
                    {
                        _logger.Log($"{_dockerConverter.TryConvertConvertHashToImageId(image.ID)} {image.Created} {repoTag}");
                    }
                }

                return new Result<IReadOnlyList<ImagesListResponse>>(new ReadOnlyCollection<ImagesListResponse>(images));
            }
        }
    }
}
