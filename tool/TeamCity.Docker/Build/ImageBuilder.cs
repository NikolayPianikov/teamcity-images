using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using IoC;
using TeamCity.Docker.Generate;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Build
{
    internal class ImageBuilder : IImageBuilder
    {
        [NotNull] private readonly IOptions _options;
        [NotNull] private readonly ILogger _logger;
        [NotNull] private readonly IMessageLogger _messageLogger;
        [NotNull] private readonly IStreamService _streamService;
        [NotNull] private readonly IPathService _pathService;
        [NotNull] private readonly IFileSystem _fileSystem;
        [NotNull] private readonly IContextFactory _contextFactory;
        [NotNull] private readonly IImageFetcher _imageFetcher;
        [NotNull] private readonly IImagePublisher _imagePublisher;
        [NotNull] private readonly IImageCleaner _imageCleaner;
        [NotNull] private readonly ITaskRunner<IDockerClient> _taskRunner;

        public ImageBuilder(
            [NotNull] IOptions options,
            [NotNull] ILogger logger,
            [NotNull] IMessageLogger messageLogger,
            [NotNull] IStreamService streamService,
            [NotNull] IPathService pathService,
            [NotNull] IFileSystem fileSystem,
            [NotNull] IContextFactory contextFactory,
            [NotNull] IImageFetcher imageFetcher,
            [NotNull] IImagePublisher imagePublisher,
            [NotNull] IImageCleaner imageCleaner,
            [NotNull] ITaskRunner<IDockerClient> taskRunner)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageLogger = messageLogger ?? throw new ArgumentNullException(nameof(messageLogger));
            _streamService = streamService ?? throw new ArgumentNullException(nameof(streamService));
            _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _imageFetcher = imageFetcher ?? throw new ArgumentNullException(nameof(imageFetcher));
            _imagePublisher = imagePublisher ?? throw new ArgumentNullException(nameof(imagePublisher));
            _imageCleaner = imageCleaner ?? throw new ArgumentNullException(nameof(imageCleaner));
            _taskRunner = taskRunner ?? throw new ArgumentNullException(nameof(taskRunner));
        }

        public async Task<Result> Build(IEnumerable<TreeNode<DockerFile>> dockerNodes)
        {
            if (dockerNodes == null)
            {
                throw new ArgumentNullException(nameof(dockerNodes));
            }

            var nodesToBuild = new List<TreeNode<DockerFile>>();
            var dockerFiles = new HashSet<DockerFile>();
            var dependencies = new Dictionary<string, int>();
            foreach (var node in dockerNodes)
            {
                var allNodes = node.EnumerateNodes().ToList();
                if (allNodes.Any(i => i.Value.Metadata.Repos.Any()))
                {
                    nodesToBuild.Add(node);
                    foreach (var treeNode in allNodes)
                    {
                        dockerFiles.Add(treeNode.Value);
                    }
                }
                else
                {
                    allNodes.ForEach(i => _logger.Log($"Skip {i.Value} because of it has no any repo tag."));
                }

                foreach (var treeNode in allNodes)
                {
                    foreach (var baseImage in treeNode.Value.Metadata.BaseImages)
                    {
                        if (dependencies.TryGetValue(baseImage, out var counter))
                        {
                            dependencies[baseImage] = ++counter;
                        }
                        else
                        {
                            dependencies.Add(baseImage, 1);
                        }
                    }
                }
            }

            if (!nodesToBuild.Any())
            {
                _logger.Log("There are no any images to build.", Result.Warning);
                return Result.Warning;
            }

            await _imageFetcher.GetImages(new Dictionary<string, string>());

            var id = Guid.NewGuid().ToString();
            var labels = new Dictionary<string, string> { { "InternalBuildId", id } };
            if (!string.IsNullOrWhiteSpace(_options.SessionId))
            {
                labels.Add("SessionId", _options.SessionId);
            }

            var dockerFilesRootPath = _fileSystem.UniqueName;
            var contextStreamResult = await _contextFactory.Create(dockerFilesRootPath, dockerFiles);
            if (contextStreamResult.State == Result.Error)
            {
                return Result.Error;
            }

            try
            {
                using (var contextStream = contextStreamResult.Value)
                {
                    foreach (var node in nodesToBuild)
                    {
                        if (await Build(node, contextStream, dockerFilesRootPath, labels, dependencies) == Result.Error)
                        {
                            return Result.Error;
                        }
                    }
                }
            }
            finally
            {
                await _imageFetcher.GetImages(new Dictionary<string, string>());
            }

            return Result.Success;
        }

        private async Task<Result> Build(TreeNode<DockerFile> node, Stream contextStream, string dockerFilesRootPath, IDictionary<string, string> labels, Dictionary<string, int> dependencies)
        {
            var id = Guid.NewGuid().ToString();
            var curLabels = new Dictionary<string, string>(labels) {{"InternalImageId", id}};
            var dockerFile = node.Value;
            var dockerFilePathInContext = _pathService.Normalize(Path.Combine(dockerFilesRootPath, dockerFile.Path));
            var buildParameters = new ImageBuildParameters
            {
                Dockerfile = dockerFilePathInContext,
                Tags = dockerFile.Metadata.Tags.Concat(_options.Tags).Distinct().ToList(),
                Labels = curLabels
            };

            using (_logger.CreateBlock(dockerFile.ToString()))
            {
                try
                {
                    using (_logger.CreateBlock("Build"))
                    {
                        var result = await _taskRunner.Run(client => BuildImage(client, contextStream, buildParameters));
                        if (result == Result.Error)
                        {
                            return Result.Error;
                        }
                    }

                    var imagesResult = await _imageFetcher.GetImages(curLabels);
                    if (imagesResult.State == Result.Error)
                    {
                        return Result.Error;
                    }

                    if (imagesResult.Value.Count == 0)
                    {
                        _logger.Log("There are no any images found after build.", Result.Error);
                        return Result.Error;
                    }

                    // Build children
                    foreach (var child in node.Children)
                    {
                        if (await Build(child, contextStream, dockerFilesRootPath, labels, dependencies) == Result.Error)
                        {
                            return Result.Error;
                        }
                    }

                    // Push produced images
                    if (!string.IsNullOrWhiteSpace(_options.Username) && !string.IsNullOrWhiteSpace(_options.Password))
                    {
                        var pushResult = await _imagePublisher.PushImages(imagesResult.Value);
                        if (pushResult == Result.Error)
                        {
                            return Result.Error;
                        }
                    }
                }
                finally
                {
                    if (_options.Clean)
                    {
                        // Remove produced images
                        var imagesResult = await _imageFetcher.GetImages(curLabels);
                        if (imagesResult.State != Result.Error)
                        {
                            await _imageCleaner.CleanImages(imagesResult.Value);
                        }

                        // Remove dependencies
                        using (_logger.CreateBlock("Dependencies"))
                        {
                            var baseImagesToRemove = new HashSet<string>();
                            foreach (var baseImage in node.Value.Metadata.BaseImages)
                            {
                                if (!dependencies.TryGetValue(baseImage, out var count))
                                {
                                    continue;
                                }

                                count--;
                                if (count == 0)
                                {
                                    dependencies.Remove(baseImage);
                                    baseImagesToRemove.Add(baseImage);
                                }
                            }

                            var images = await _imageFetcher.GetImages(new Dictionary<string, string>());
                            if (images.State != Result.Error)
                            {
                                var imagesToRemove = new List<DockerImage>();
                                foreach (var image in images.Value)
                                {
                                    if (baseImagesToRemove.Contains(image.RepoTag))
                                    {
                                        baseImagesToRemove.Add(image.Info.ID);
                                        imagesToRemove.Add(image);
                                    }
                                }

                                if (imagesToRemove.Count != 0)
                                {
                                    await _imageCleaner.CleanImages(imagesToRemove);
                                }

                                {
                                    _logger.Log("Nothing to clean.");
                                }
                            }
                        }
                    }
                }
            }

            return Result.Success;
        }

        private async Task BuildImage(IDockerClient client, Stream contextStream, ImageBuildParameters buildParameters)
        {
            contextStream.Position = 0;
            using (var buildEventStream = await client.Images.BuildImageFromDockerfileAsync(
                contextStream,
                buildParameters,
                CancellationToken.None))
            {
                _streamService.ProcessLines(buildEventStream, line => { _messageLogger.Log(line); });
            }
        }
    }
}
