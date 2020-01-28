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
        [NotNull] private readonly ITaskRunner<IDockerClient> _taskRunner;

        public ImageBuilder(
            [NotNull] IOptions options,
            [NotNull] ILogger logger,
            [NotNull] IMessageLogger messageLogger,
            [NotNull] IStreamService streamService,
            [NotNull] IPathService pathService,
            [NotNull] IFileSystem fileSystem,
            [NotNull] IContextFactory contextFactory,
            [NotNull] ITaskRunner<IDockerClient> taskRunner)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageLogger = messageLogger ?? throw new ArgumentNullException(nameof(messageLogger));
            _streamService = streamService ?? throw new ArgumentNullException(nameof(streamService));
            _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _taskRunner = taskRunner ?? throw new ArgumentNullException(nameof(taskRunner));
        }

        public async Task<Result> Build(IReadOnlyCollection<DockerFile> dockerFiles)
        {
            if (dockerFiles == null)
            {
                throw new ArgumentNullException(nameof(dockerFiles));
            }

            var dockerFilesRootPath = _fileSystem.UniqueName;
            var contextStreamResult = await _contextFactory.Create(dockerFilesRootPath, dockerFiles);
            if (contextStreamResult.State == Result.Error)
            {
                return Result.Error;
            }

            using (var contextStream = contextStreamResult.Value)
            {
                using (_logger.CreateBlock("Build"))
                {
                    var labels = new Dictionary<string, string>();
                    if (!string.IsNullOrWhiteSpace(_options.SessionId))
                    {
                        labels.Add("SessionId", _options.SessionId);
                    }

                    var hasError = false;
                    foreach (var dockerFile in dockerFiles.OrderBy(dockerFile => dockerFile.Metadata.Priority))
                    {
                        if (!dockerFile.Metadata.Repos.Any())
                        {
                            _logger.Log($"Skip {dockerFile} because of it has no any repo tag.");
                            continue;
                        }

                        using (_logger.CreateBlock(dockerFile.ToString()))
                        {
                            _logger.Log($"The dockerfile is \"{dockerFile.Path}\"");
                            var dockerFilePathInContext = _pathService.Normalize(Path.Combine(dockerFilesRootPath, dockerFile.Path));
                            try
                            {
                                contextStream.Position = 0;
                                using (var buildEventStream = await _taskRunner.Run(client => client.Images.BuildImageFromDockerfileAsync(
                                    contextStream,
                                    new ImageBuildParameters
                                    {
                                        Dockerfile = dockerFilePathInContext,
                                        Tags = dockerFile.Metadata.Tags.Concat(_options.Tags).Distinct().ToList(),
                                        Labels = labels
                                    },
                                    CancellationToken.None)))
                                {
                                    _streamService.ProcessLines(
                                        buildEventStream,
                                        line =>
                                        {
                                            if (_messageLogger.Log(line) == Result.Error)
                                            {
                                                hasError = true;
                                            }
                                        });
                                }

                                if (hasError)
                                {
                                    return Result.Error;
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.Log(ex);
                                return Result.Error;
                            }
                        }
                    }
                }
            }

            return Result.Success;
        }
    }
}
