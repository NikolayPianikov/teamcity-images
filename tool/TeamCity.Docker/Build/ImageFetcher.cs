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

                Dictionary<string, IDictionary<string, bool>> dockerFilters = null;
                if (filters.Count > 0)
                {
                    dockerFilters = new Dictionary<string, IDictionary<string, bool>>
                    {
                        {"label", filters.ToDictionary(filter => $"{filter.Key}={filter.Value}", filter => true)}
                    };
                }

                var dockerImages = await _taskRunner.Run(client => client.Images.ListImagesAsync(new ImagesListParameters {Filters = dockerFilters}));
                if (dockerImages.State == Result.Error)
                {
                    return new Result<IReadOnlyList<DockerImage>>(new List<DockerImage>(), Result.Error);
                }

                if (dockerImages.Value.Count == 0)
                {
                    _logger.Log("Nothing to list.");
                    return new Result<IReadOnlyList<DockerImage>>(new List<DockerImage>());
                }

                long count = 0;
                long size = 0;
                var ids = new HashSet<string>();
                foreach (var image in dockerImages.Value)
                {
                    if (ids.Add(image.ID))
                    {
                        count++;
                        size += image.Size;
                    }

                    if (image.RepoTags != null)
                    {
                        foreach (var imageRepoTag in image.RepoTags)
                        {
                            _logger.Log($"{_dockerConverter.TryConvertConvertHashToImageId(image.ID)} {image.Created} {imageRepoTag} {_dockerConverter.ConvertToSize(image.Size, 1)}");
                        }
                    }
                    else
                    {
                        _logger.Log($"{_dockerConverter.TryConvertConvertHashToImageId(image.ID)} {image.Created} {_dockerConverter.ConvertToSize(image.Size, 1)}");
                    }
                }

                _logger.Log($"Totals {count} images {_dockerConverter.ConvertToSize(size, 1)}");

                var images = (
                        from image in (
                            from image in dockerImages.Value
                            where image.RepoTags != null
                            from tag in image.RepoTags
                            where !tag.Contains("<none>")
                            select new DockerImage(image, tag))
                        select image)
                    .ToList();

                using (_logger.CreateBlock("Result"))
                {
                    count = 0;
                    size = 0;
                    ids.Clear();

                    if (images.Count > 0)
                    {
                        foreach (var image in images)
                        {
                            if (ids.Add(image.Info.ID))
                            {
                                count++;
                                size += image.Info.Size;
                            }

                            _logger.Log($"{_dockerConverter.TryConvertConvertHashToImageId(image.Info.ID)} {image.Info.Created} {image.RepoTag} {_dockerConverter.ConvertToSize(image.Info.Size, 1)}");
                        }

                        _logger.Log($"Totals {count} images {_dockerConverter.ConvertToSize(size, 1)}");
                    }
                    else
                    {
                        _logger.Log("Nothing to list.");
                    }
                }

                return new Result<IReadOnlyList<DockerImage>>(images);
            }
        }
    }
}
