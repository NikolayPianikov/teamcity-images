// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IoC;
using TeamCity.Docker.Generic;
using TeamCity.Docker.Model;

namespace TeamCity.Docker
{
    internal class GenerateCommand: ICommand<IGenerateOptions>
    {
        private readonly ILogger _logger;
        [NotNull] private readonly IFileSystem _fileSystem;
        [NotNull] private readonly IGenerateOptions _options;
        [NotNull] private readonly IConfigurationExplorer _configurationExplorer;
        [NotNull] private readonly IFactory<IGraph<IArtifact, Dependency>, IEnumerable<Template>> _buildGraphFactory;
        private readonly IReadmeGenerator _readmeGenerator;
        
        public GenerateCommand(
            [NotNull] ILogger logger,
            [NotNull] IFileSystem fileSystem,
            [NotNull] IGenerateOptions options,
            [NotNull] IConfigurationExplorer configurationExplorer,
            [NotNull] IFactory<IGraph<IArtifact, Dependency>, IEnumerable<Template>> buildGraphFactory,
            [NotNull] IReadmeGenerator readmeGenerator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _configurationExplorer = configurationExplorer ?? throw new ArgumentNullException(nameof(configurationExplorer));
            _buildGraphFactory = buildGraphFactory ?? throw new ArgumentNullException(nameof(buildGraphFactory));
            _readmeGenerator = readmeGenerator ?? throw new ArgumentNullException(nameof(readmeGenerator));
        }

        public Task<Result> Run()
        {
            var templates = _configurationExplorer.Explore(_options.SourcePath, _options.ConfigurationFiles);
            if (templates.State == Result.Error)
            {
                return Task.FromResult(Result.Error);
            }

            var graph = _buildGraphFactory.Create(templates.Value);
            if (graph.State == Result.Error)
            {
                return Task.FromResult(Result.Error);
            }

            _readmeGenerator.Generate(graph.Value);
            var dockerFiles = graph.Value.Nodes.Select(i => i.Value).OfType<GeneratedDockerfile>();
            foreach (var dockerfile in dockerFiles)
            {
                var path = Path.Combine(_options.TargetPath, dockerfile.Path);
                _fileSystem.WriteLines(path, dockerfile.Lines.Select(i => i.Text));
            }

            var readmeFiles = graph.Value.Nodes.Select(i => i.Value).OfType<Readme>();
            foreach (var readmeFile in readmeFiles)
            {
                var path = Path.Combine(_options.TargetPath, readmeFile.Path);
                _fileSystem.WriteLines(path, readmeFile.Lines);
            }

            return Task.FromResult(graph.State);
        }
    }
}
