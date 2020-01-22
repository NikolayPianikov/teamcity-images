using System;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class DockerClientFactory : IDockerClientFactory
    {
        private readonly ILogger _logger;
        private readonly IOptions _options;

        public DockerClientFactory(
            [NotNull] ILogger logger,
            [NotNull] IOptions options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<IDockerClient> Create()
        {
            var client = new DockerClientConfiguration(new Uri(_options.DockerEngineEndpoint)).CreateClient();
            using (_logger.CreateBlock("Connect to docker"))
            {
                _logger.Log($"The docker engine endpoint is \"{_options.DockerEngineEndpoint}\".");
                try
                {
                    var info = await client.System.GetSystemInfoAsync(CancellationToken.None);
                    _logger.Log($"Connected to docker engine \"{info.Name}\" {info.OSType} {info.Architecture}.");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The docker engine connection error.", ex);
                }
            }

            return client;
        }
    }
}