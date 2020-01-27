using System;
using System.Runtime.InteropServices;
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
        private readonly IEnvironment _environment;

        public DockerClientFactory(
            [NotNull] ILogger logger,
            [NotNull] IOptions options,
            [NotNull] IEnvironment environment)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public async Task<IDockerClient> Create()
        {
            var engineEndpoint = _options.DockerEngineEndpoint;
            if (string.IsNullOrWhiteSpace(engineEndpoint))
            {
                if (_environment.IsOSPlatform(OSPlatform.Windows))
                {
                    engineEndpoint = "npipe://./pipe/docker_engine";
                }
                else
                {
                    engineEndpoint = "unix:///var/run/docker.sock";
                }
            }

            var client = new DockerClientConfiguration(new Uri(engineEndpoint)).CreateClient();
            using (_logger.CreateBlock("Connect to docker"))
            {
                _logger.Log($"The docker engine endpoint is \"{engineEndpoint}\".");
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