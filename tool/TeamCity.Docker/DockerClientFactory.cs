using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IList<Uri> _endpoints = new List<Uri>();

        public DockerClientFactory(
            [NotNull] ILogger logger,
            [NotNull] IOptions options,
            [NotNull] IEnvironment environment)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            var engineEndpoint = (options ?? throw new ArgumentNullException(nameof(options))).DockerEngineEndpoint;
            if (string.IsNullOrWhiteSpace(engineEndpoint))
            {
                if ((environment ?? throw new ArgumentNullException(nameof(environment))).IsOSPlatform(OSPlatform.Windows))
                {
                    _endpoints.Add(new Uri("npipe://./pipe/docker_engine"));
                }
                else
                {
                    _endpoints.Add(new Uri("unix:///var/run/docker.sock"));
                }

                _endpoints.Add(new Uri("tcp://127.0.0.1:2375"));
            }
            else
            {
                _endpoints.Add(new Uri(engineEndpoint));
            }
        }

        public async Task<IDockerClient> Create()
        {
            DockerClient client = null;
            using (_logger.CreateBlock("Connect"))
            {
                var errors = new List<Exception>();
                var endpoints = _endpoints.ToList();
                foreach (var endpoint in endpoints)
                {
                    try
                    {
                        client = new DockerClientConfiguration(endpoint).CreateClient();
                        _logger.Log($"Connecting to \"{endpoint}\".");
                        var info = await client.System.GetSystemInfoAsync(CancellationToken.None);
                        _logger.Log($"Connected to \"{info.Name}\" {info.OSType} {info.Architecture}.");
                        break;
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex);
                    }
                }

                if (client == null)
                {
                    foreach (var error in errors)
                    {
                        _logger.Log(error);
                    }

                    throw new InvalidOperationException("The docker engine connection error.");
                }
            }

            return client;
        }
    }
}