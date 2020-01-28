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
        private readonly IMessageLogger _messageLogger;
        private readonly IStreamService _streamService;
        private readonly IPathService _pathService;

        public ImageBuilder(
            [NotNull] IOptions options,
            [NotNull] ILogger logger,
            [NotNull] IMessageLogger messageLogger,
            [NotNull] IStreamService streamService,
            [NotNull] IPathService pathService)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageLogger = messageLogger ?? throw new ArgumentNullException(nameof(messageLogger));
            _streamService = streamService ?? throw new ArgumentNullException(nameof(streamService));
            _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
        }

        public async Task<Result> Build(
            IDockerClient dockerClient,
            IReadOnlyCollection<DockerFile> dockerFiles,
            string dockerFilesRootPath,
            Stream contextStream)
        {
            if (dockerClient == null)
            {
                throw new ArgumentNullException(nameof(dockerClient));
            }

            if (dockerFiles == null)
            {
                throw new ArgumentNullException(nameof(dockerFiles));
            }

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
                    var dockerFilePathInContext = _pathService.Normalize(Path.Combine(dockerFilesRootPath, dockerFile.Path));
                    try
                    {
                        contextStream.Position = 0;
                        using (var buildEventStream = await dockerClient.Images.BuildImageFromDockerfileAsync(
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
