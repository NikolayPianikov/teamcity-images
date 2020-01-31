﻿using System;
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
        private static readonly Result<IEnumerable<DockerImage>> Error = new Result<IEnumerable<DockerImage>>(Enumerable.Empty<DockerImage>(), Result.Error);
        private static readonly Result<IEnumerable<DockerImage>> Warning = new Result<IEnumerable<DockerImage>>(Enumerable.Empty<DockerImage>(), Result.Warning);

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

        public async Task<Result<IEnumerable<DockerImage>>> Build(IEnumerable<TreeNode<TreeDependency>> dockerNodes)
        {
            if (dockerNodes == null)
            {
                throw new ArgumentNullException(nameof(dockerNodes));
            }

            var nodesToBuild = new List<TreeNode<TreeDependency>>();
            var dockerFiles = new HashSet<DockerFile>();
            var dependencies = new Dictionary<string, int>();
            foreach (var node in dockerNodes)
            {
                var allNodes = node.EnumerateNodes().ToList();
                if (allNodes.Any(i => i.Value.File.Metadata.Repos.Any()))
                {
                    nodesToBuild.Add(node);
                    foreach (var treeNode in allNodes)
                    {
                        dockerFiles.Add(treeNode.Value.File);
                    }
                }
                else
                {
                    allNodes.ForEach(i => _logger.Log($"Skip {i.Value} because of it has no any repo tag."));
                }

                FillDependencies(allNodes, dependencies);
            }

            if (!nodesToBuild.Any())
            {
                _logger.Log("There are no any images to build.", Result.Warning);
                return Warning;
            }

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
                return Error;
            }

            var initialState = await GetImages(new Dictionary<string, string>(), true, "Initial state");

            var result = new List<DockerImage>();
            using (var contextStream = contextStreamResult.Value)
            {
                foreach (var node in nodesToBuild)
                {
                    var images = await Build(node, contextStream, dockerFilesRootPath, labels, initialState, dependencies);
                    if (images.State == Result.Error)
                    {
                        return images;
                    }

                    result.AddRange(images.Value);
                }
            }

            return new Result<IEnumerable<DockerImage>>(result);
        }

        private async Task<HashSet<DockerImage>> GetImages([NotNull] IReadOnlyDictionary<string, string> filters, bool details = true, string blockName = "")
        {
            var verbose = (_options.VerboseMode && details || !details) && !string.IsNullOrWhiteSpace(blockName);
            IDisposable blockToken = null;
            if (verbose)
            {
                blockToken = _logger.CreateBlock(blockName);
            }

            try
            {
                var images = await _imageFetcher.GetImages(filters, verbose);
                return images.State == Result.Error ? new HashSet<DockerImage>() : images.Value.ToHashSet();
            }
            finally
            {
                blockToken?.Dispose();
            }
        }

        private async Task<Result<IEnumerable<DockerImage>>> Build(TreeNode<TreeDependency> node, Stream contextStream, string dockerFilesRootPath, IReadOnlyDictionary<string, string> labels, HashSet<DockerImage> initialState, IDictionary<string, int> dependencies)
        {
            var id = Guid.NewGuid().ToString();
            var curLabels = new Dictionary<string, string>(labels) {{"InternalImageId", id}};
            var dockerFile = node.Value.File;
            var dockerFilePathInContext = _pathService.Normalize(Path.Combine(dockerFilesRootPath, dockerFile.Path));
            var buildParameters = new ImageBuildParameters
            {
                Dockerfile = dockerFilePathInContext,
                Tags = dockerFile.Metadata.Tags.Concat(_options.Tags).Distinct().ToList(),
                Labels = curLabels
            };
            
            var result = new List<DockerImage>();
            var pushed = false;
            using (_logger.CreateBlock(dockerFile.ToString()))
            {
                try
                {
                    await GetImages(new Dictionary<string, string>(), true, "State");

                    using (_logger.CreateBlock("Build"))
                    {
                        var buildResult = await _taskRunner.Run(client => BuildImage(client, contextStream, buildParameters));
                        if (buildResult == Result.Error)
                        {
                            return Error;
                        }
                    }

                    var buildState = await GetImages(curLabels, false, "Images");
                    var afterBuildState = await GetImages(new Dictionary<string, string>());

                    ExcludeServedDependencies(node.Value.File, dependencies);

                    var difState = afterBuildState.Except(initialState).ToHashSet();
                    var toRemove = difState.Where(i => !dependencies.ContainsKey(i.RepoTag)).ToHashSet();
                    var toPush = buildState.Where(i => !string.IsNullOrWhiteSpace(i.RepoTag)).ToHashSet();

                    if (toPush.Count == 0)
                    {
                        _logger.Log("There are no any produced images found after build.", Result.Error);
                        return Error;
                    }

                    // Push produced images
                    if (!string.IsNullOrWhiteSpace(_options.Username) && !string.IsNullOrWhiteSpace(_options.Password))
                    {
                        using (_logger.CreateBlock("Push"))
                        {
                            var pushResult = await _imagePublisher.PushImages(toPush);
                            if (pushResult.State == Result.Error)
                            {
                                return Error;
                            }

                            result.AddRange(pushResult.Value);
                            pushed = true;
                        }
                    }
                    else
                    {
                        result.AddRange(toPush);
                    }

                    // Clean
                    if (_options.Clean)
                    {
                        using (_logger.CreateBlock("Clean"))
                        {
                            await _imageCleaner.CleanImages(toRemove);
                        }
                    }

                    // Build children
                    foreach (var child in node.Children)
                    {
                        var images = await Build(child, contextStream, dockerFilesRootPath, labels, initialState, dependencies);
                        if (images.State == Result.Error)
                        {
                            return Error;
                        }

                        result.AddRange(images.Value);
                    }
                }
                finally
                {
                    if (pushed && _options.Clean)
                    {
                        var buildState = await GetImages(curLabels);
                        var toRemove = buildState.Where(i => !dependencies.ContainsKey(i.RepoTag)).ToHashSet();

                        using (_logger.CreateBlock("Clean"))
                        {
                            await _imageCleaner.CleanImages(toRemove);
                        }
                    }

                    await GetImages(new Dictionary<string, string>(), true, "State");
                }
            }

            return new Result<IEnumerable<DockerImage>>(result);
        }

        private static void FillDependencies(IEnumerable<TreeNode<TreeDependency>> nodes, IDictionary<string, int> dependencies)
        {
            foreach (var treeNode in nodes)
            {
                foreach (var dependency in treeNode.Value.File.Metadata.Dependencies)
                {
                    if (dependencies.TryGetValue(dependency.RepoTag, out var counter))
                    {
                        dependencies[dependency.RepoTag] = counter + 1;
                    }
                    else
                    {
                        dependencies.Add(dependency.RepoTag, 1);
                    }
                }
            }
        }

        private static void ExcludeServedDependencies(DockerFile dockerFile, IDictionary<string, int> dependencies)
        {
            foreach (var dependency in dockerFile.Metadata.Dependencies)
            {
                if (!dependencies.TryGetValue(dependency.RepoTag, out var count))
                {
                    continue;
                }

                count--;
                if (count == 0)
                {
                    dependencies.Remove(dependency.RepoTag);
                }
                else
                {
                    dependencies[dependency.RepoTag] = count - 1;
                }
            }
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
