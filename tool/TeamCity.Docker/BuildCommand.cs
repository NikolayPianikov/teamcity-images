﻿// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using Docker.DotNet.Models;
using IoC;
using TeamCity.Docker.Generic;
using TeamCity.Docker.Model;

namespace TeamCity.Docker
{
    internal class BuildCommand: ICommand<IBuildOptions>
    {
        [NotNull] private readonly ILogger _logger;
        [NotNull] private readonly IMessageLogger _messageLogger;
        [NotNull] private readonly CancellationTokenSource _cancellationTokenSource;
        [NotNull] private readonly IFileSystem _fileSystem;
        [NotNull] private readonly IPathService _pathService;
        [NotNull] private readonly IBuildOptions _options;
        [NotNull] private readonly IConfigurationExplorer _configurationExplorer;
        [NotNull] private readonly IFactory<IGraph<IArtifact, Dependency>, IEnumerable<Template>> _buildGraphFactory;
        [NotNull] private readonly IFactory<IEnumerable<IGraph<IArtifact, Dependency>>, IGraph<IArtifact, Dependency>> _buildGraphsFactory;
        [NotNull] private readonly IBuildPathProvider _buildPathProvider;
        [NotNull] private readonly IContextFactory _contextFactory;
        [NotNull] private readonly IStreamService _streamService;
        [NotNull] private readonly IDockerClient _dockerClient;

        public BuildCommand(
            [NotNull] ILogger logger,
            [NotNull] IMessageLogger messageLogger,
            [NotNull] CancellationTokenSource cancellationTokenSource,
            [NotNull] IFileSystem fileSystem,
            [NotNull] IPathService pathService,
            [NotNull] IBuildOptions options,
            [NotNull] IConfigurationExplorer configurationExplorer,
            [NotNull] IFactory<IGraph<IArtifact, Dependency>, IEnumerable<Template>> buildGraphFactory,
            [NotNull] IFactory<IEnumerable<IGraph<IArtifact, Dependency>>, IGraph<IArtifact, Dependency>> buildGraphsFactory,
            [NotNull] IBuildPathProvider buildPathProvider,
            [NotNull] IContextFactory contextFactory,
            [NotNull] IStreamService streamService,
            [NotNull] IDockerClient dockerClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageLogger = messageLogger ?? throw new ArgumentNullException(nameof(messageLogger));
            _cancellationTokenSource = cancellationTokenSource ?? throw new ArgumentNullException(nameof(cancellationTokenSource));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _configurationExplorer = configurationExplorer ?? throw new ArgumentNullException(nameof(configurationExplorer));
            _buildGraphFactory = buildGraphFactory ?? throw new ArgumentNullException(nameof(buildGraphFactory));
            _buildGraphsFactory = buildGraphsFactory ?? throw new ArgumentNullException(nameof(buildGraphsFactory));
            _buildPathProvider = buildPathProvider ?? throw new ArgumentNullException(nameof(buildPathProvider));
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _streamService = streamService ?? throw new ArgumentNullException(nameof(streamService));
            _dockerClient = dockerClient ?? throw new ArgumentNullException(nameof(dockerClient));
        }

        public async Task<Result> Run()
        {
            var templates = _configurationExplorer.Explore(_options.SourcePath, _options.ConfigurationFiles);
            if (templates.State == Result.Error)
            {
                return Result.Error;
            }

            var buildGraphResult = _buildGraphFactory.Create(templates.Value);
            if (buildGraphResult.State == Result.Error)
            {
                return Result.Error;
            }

            var buildGraphsResult = _buildGraphsFactory.Create(buildGraphResult.Value);
            if (buildGraphsResult.State == Result.Error)
            {
                return Result.Error;
            }

            var buildGraphs = buildGraphsResult.Value.ToList();
            if (!buildGraphs.Any())
            {
                _logger.Log("Nothing to build.", Result.Error);
                return Result.Error;
            }

            var dockerFilesRootPath = _fileSystem.UniqueName;
            var contextStreamResult = await _contextFactory.Create(dockerFilesRootPath, buildGraphResult.Value.Nodes.Select(i => i.Value).OfType<Image>().Select(i => i.File));
            if (contextStreamResult.State == Result.Error)
            {
                return Result.Error;
            }

            var contextStream = contextStreamResult.Value;
            using (contextStream)
            using (_logger.CreateBlock("Build"))
            {
                var labels = new Dictionary<string, string>();
                foreach (var graph in buildGraphs)
                {
                    var buildPath = _buildPathProvider.GetPath(graph).ToList();
                    foreach (var buildNode in buildPath)
                    {
                        switch (buildNode.Value)
                        {
                            case Image image:
                                var dockerFile = image.File;
                                using (_logger.CreateBlock(dockerFile.ToString()))
                                {
                                    contextStream.Position = 0;
                                    var dockerFilePathInContext = _pathService.Normalize(Path.Combine(dockerFilesRootPath, dockerFile.Path));
                                    var buildParameters = new ImageBuildParameters
                                    {
                                        Dockerfile = dockerFilePathInContext,
                                        Tags = dockerFile.Tags.Distinct().ToList(),
                                        Labels = labels
                                    };

                                    using (var buildEventStream = await _dockerClient.Images.BuildImageFromDockerfileAsync(
                                        contextStream,
                                        buildParameters,
                                        _cancellationTokenSource.Token))
                                    {
                                        _streamService.ProcessLines(buildEventStream, line => { _messageLogger.Log(line); });
                                    }
                                }

                                break;
                        }
                    }
                }
            }

            return Result.Success;
        }
    }
}
