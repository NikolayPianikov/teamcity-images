// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoC;
using TeamCity.Docker.Generic;
using TeamCity.Docker.Model;

namespace TeamCity.Docker
{
    internal class BuildCommand: ICommand<IBuildOptions>
    {
        [NotNull] private readonly ILogger _logger;
        [NotNull] private readonly IFileSystem _fileSystem;
        [NotNull] private readonly IBuildOptions _options;
        [NotNull] private readonly IConfigurationExplorer _configurationExplorer;
        [NotNull] private readonly IFactory<IGraph<IArtifact, Dependency>, IEnumerable<Template>> _buildGraphFactory;
        [NotNull] private readonly IContextFactory _contextFactory;

        public BuildCommand(
            [NotNull] ILogger logger,
            [NotNull] IFileSystem fileSystem,
            [NotNull] IBuildOptions options,
            [NotNull] IConfigurationExplorer configurationExplorer,
            [NotNull] IFactory<IGraph<IArtifact, Dependency>, IEnumerable<Template>> buildGraphFactory,
            [NotNull] IContextFactory contextFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _configurationExplorer = configurationExplorer ?? throw new ArgumentNullException(nameof(configurationExplorer));
            _buildGraphFactory = buildGraphFactory ?? throw new ArgumentNullException(nameof(buildGraphFactory));
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task<Result> Run()
        {
            var templates = _configurationExplorer.Explore(_options.SourcePath, _options.ConfigurationFiles);
            if (templates.State == Result.Error)
            {
                return Result.Error;
            }

            var graph = _buildGraphFactory.Create(templates.Value);
            if (graph.State == Result.Error)
            {
                return Result.Error;
            }

            var imageNodes = graph.Value.Nodes.Where(i => i.Value is Image).ToList();
            var dockerFilesRootPath = _fileSystem.UniqueName;
            var contextStream = await _contextFactory.Create(dockerFilesRootPath, imageNodes.Select(i => i.Value).OfType<Image>().Select(i => i.File));
            if (contextStream.State == Result.Error)
            {
                return Result.Error;
            }

            using (contextStream.Value)
            {
            }

            return Result.Success;
        }
    }
}
