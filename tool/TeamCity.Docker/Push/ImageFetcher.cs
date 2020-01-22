using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.VisualBasic;

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
                    _logger.Log($"{_dockerConverter.TryConvertConvertHashToImageId(image.Info.ID)} {image.Info.Created} {image.RepoTag}");
                }

                return new Result<IReadOnlyList<DockerImage>>(images);
            }
        }
    }
}
