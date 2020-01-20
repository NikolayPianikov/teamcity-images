using System;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class DockerClientFactory : IDockerClientFactory
    {
        private readonly ILogger _logger;
        private readonly IOptions _options;

        public DockerClientFactory(
            ILogger logger,
            IOptions options)
        {
            _logger = logger;
            _options = options;
        }

        public async Task<IDockerClient> Create()
        {
            var client = new DockerClientConfiguration(new Uri(_options.DockerEngineEndpoint)).CreateClient();
            using (_logger.CreateBlock("Connecting to docker"))
            {
                _logger.Log($"The docker engine endpoint is \"{_options.DockerEngineEndpoint}\".");
                try
                {
                    var info = await client.System.GetSystemInfoAsync(CancellationToken.None);
                    _logger.Log($"Connected to docker engine \"{info.Name}\" {info.OSType} {info.Architecture}.");
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"The docker engine connection error.", ex);
                }
            }

            return client;
        }
    }
}