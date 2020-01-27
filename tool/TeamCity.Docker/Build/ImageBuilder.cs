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
        private readonly IOptions _options;
        private readonly ILogger _logger;
        private readonly IDockerClient _dockerClient;
        private readonly IMessageLogger _messageLogger;
        private readonly IFileSystem _fileSystem;
        private readonly IStreamService _streamService;
        private readonly IPathService _pathService;
        private readonly IContextFactory _contextFactory;

        public ImageBuilder(
            [NotNull] IOptions options,
            [NotNull] ILogger logger,
            [NotNull] IDockerClient dockerClient,
            [NotNull] IMessageLogger messageLogger,
            [NotNull] IFileSystem fileSystem,
            [NotNull] IStreamService streamService,
            [NotNull] IPathService pathService,
            [NotNull] IContextFactory contextFactory)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dockerClient = dockerClient ?? throw new ArgumentNullException(nameof(dockerClient));
            _messageLogger = messageLogger ?? throw new ArgumentNullException(nameof(messageLogger));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _streamService = streamService ?? throw new ArgumentNullException(nameof(streamService));
            _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
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
                    }

                    using (_logger.CreateBlock(dockerFile.ToString()))
                    {
                        _logger.Log($"The dockerfile is \"{dockerFile.Path}\"");
                        contextStream.Position = 0;
                        var dockerFilePathInContext = _pathService.Normalize(Path.Combine(dockerFilesRootPath, dockerFile.Path));
                        try
                        {
                            using (var buildEventStream = await _dockerClient.Images.BuildImageFromDockerfileAsync(
                                contextStream,
                                new ImageBuildParameters
                                {
                                    Dockerfile = dockerFilePathInContext,
                                    Tags = dockerFile.Metadata.Tags.Concat(_options.Tags).Distinct().ToList(),
                                    Labels = labels
                                },
                                CancellationToken.None))
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

                return Result.Success;
            }
        }
    }
}
