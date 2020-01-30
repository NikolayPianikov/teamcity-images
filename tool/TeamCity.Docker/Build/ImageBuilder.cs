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
                        if (await Build(node, contextStream, dockerFilesRootPath, labels) == Result.Error)
                        {
                            return Result.Error;
                        }
                    }
                }
            }
            finally
            {
                var imagesResult = await _imageFetcher.GetImages(labels);
                if (_options.Clean && imagesResult.State != Result.Error)
                {
                    await _imageCleaner.CleanImages(imagesResult.Value);
                }

                await _imageFetcher.GetImages(new Dictionary<string, string>());
            }

            return Result.Success;
        }

        private async Task<Result> Build(TreeNode<DockerFile> node, Stream contextStream, string dockerFilesRootPath, IDictionary<string, string> labels)
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

                if (!string.IsNullOrWhiteSpace(_options.Username) && !string.IsNullOrWhiteSpace(_options.Password))
                {
                    var pushResult = await _imagePublisher.PushImages(imagesResult.Value);
                    if (pushResult == Result.Error)
                    {
                        return Result.Error;
                    }
                }

                foreach (var child in node.Children)
                {
                    if (await Build(child, contextStream, dockerFilesRootPath, labels) == Result.Error)
                    {
                        return Result.Error;
                    }
                }

                if ( _options.Clean)
                {
                    await _imageCleaner.CleanImages(imagesResult.Value);
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
