// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal class Command: ICommand<IOptions>
    {
        private readonly IOptions _options;
        private readonly ILogger _logger;
        private readonly IFileSystem _fileSystem;
        private readonly IEnumerable<IGenerator> _dockerFileGenerators;
        private readonly IReadmeGenerator _readmeGenerator;
        [NotNull] private readonly IDependencyTreeFactory _dependencyTreeFactory;

        public Command(
            [NotNull] IOptions options,
            [NotNull] ILogger logger,
            [NotNull] IFileSystem fileSystem,
            [NotNull] IEnumerable<IGenerator> dockerFileGenerators,
            [NotNull] IReadmeGenerator readmeGenerator,
            [NotNull] IDependencyTreeFactory dependencyTreeFactory)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _dockerFileGenerators = dockerFileGenerators ?? throw new ArgumentNullException(nameof(dockerFileGenerators));
            _readmeGenerator = readmeGenerator ?? throw new ArgumentNullException(nameof(readmeGenerator));
            _dependencyTreeFactory = dependencyTreeFactory ?? throw new ArgumentNullException(nameof(dependencyTreeFactory));
        }

        public Task<Result> Run()
        {
            var dockerFiles = new List<DockerFile>();
            using (_logger.CreateBlock("Generate"))
            {
                foreach (var generator in _dockerFileGenerators)
                {
                    var newDockerFiles = generator.Generate();
                    if (newDockerFiles.State == Result.Error)
                    {
                        return Task.FromResult(Result.Error);
                    }

                    dockerFiles.AddRange(newDockerFiles.Value);
                }

                if (dockerFiles.Count == 0)
                {
                    _logger.Log("Docker files were not generated.", Result.Error);
                    return Task.FromResult(Result.Error);
                }

                foreach (var dockerFile in dockerFiles)
                {
                    var content = string.Join(System.Environment.NewLine, dockerFile.Content.Select(line => line.Text));
                    _fileSystem.WriteFile(Path.Combine(_options.TargetPath, dockerFile.Path), content);
                }

                var dockerNodes = _dependencyTreeFactory.Create(dockerFiles).ToList();
                using (_logger.CreateBlock("Tree"))
                {
                    foreach (var node in dockerNodes)
                    {
                        ShowNode(node);
                    }
                }

                var readmeFiles = _readmeGenerator.Generate(dockerNodes).ToList();
                if (readmeFiles.Count == 0)
                {
                    _logger.Log("Readme was not generated.", Result.Warning);
                }

                foreach (var readmeFile in readmeFiles)
                {
                    _fileSystem.WriteFile(Path.Combine(_options.TargetPath, readmeFile.Path), readmeFile.Content);
                }
            }

            return Task.FromResult(Result.Success);
        }

        private void ShowNode(TreeNode<DockerFile> node)
        {
            if (!node.Children.Any())
            {
                _logger.Log(node.Value.ToString());
                return;
            }

            using (_logger.CreateBlock(node.Value.ToString()))
            {
                foreach (var treeNode in node.Children)
                {
                    ShowNode(treeNode);
                }
            }
        }
    }
}
