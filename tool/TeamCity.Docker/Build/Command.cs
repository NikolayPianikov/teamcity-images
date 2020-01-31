// ReSharper disable ClassNeverInstantiated.Global

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IoC;
using TeamCity.Docker.Generate;

namespace TeamCity.Docker.Build
{
    internal class Command: ICommand<IOptions>
    {
        [NotNull] private readonly ILogger _logger;
        [NotNull] private readonly IEnumerable<Generate.IGenerator> _dockerFileGenerators;
        [NotNull] private readonly Generate.IDependencyTreeFactory _dependencyTreeFactory;
        [NotNull] private readonly IImageBuilder _imageBuilder;
        [NotNull] private readonly IDockerConverter _dockerConverter;

        public Command(
            [NotNull] ILogger logger,
            [NotNull] IEnumerable<Generate.IGenerator> dockerFileGenerators,
            [NotNull] Generate.IDependencyTreeFactory dependencyTreeFactory,
            [NotNull] IImageBuilder imageBuilder,
            [NotNull] IDockerConverter dockerConverter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dockerFileGenerators = dockerFileGenerators ?? throw new ArgumentNullException(nameof(dockerFileGenerators));
            _dependencyTreeFactory = dependencyTreeFactory ?? throw new ArgumentNullException(nameof(dependencyTreeFactory));
            _imageBuilder = imageBuilder ?? throw new ArgumentNullException(nameof(imageBuilder));
            _dockerConverter = dockerConverter ?? throw new ArgumentNullException(nameof(dockerConverter));
        }

        public async Task<Result> Run()
        {
            var dockerFiles = new List<Generate.DockerFile>();
            using (_logger.CreateBlock("Generate"))
            {
                foreach (var generator in _dockerFileGenerators)
                {
                    var newDockerFiles = generator.Generate();
                    if (newDockerFiles.State == Result.Error)
                    {
                        return Result.Error;
                    }

                    dockerFiles.AddRange(newDockerFiles.Value);
                }

                if (dockerFiles.Count == 0)
                {
                    _logger.Log("Docker files were not generated.", Result.Error);
                    return Result.Error;
                }
            }

            var dockerNodes = _dependencyTreeFactory.Create(dockerFiles).ToList();
            using (_logger.CreateBlock("Build Tree"))
            {
                foreach (var node in dockerNodes)
                {
                    ShowNode(node);
                }
            }

            var result = await _imageBuilder.Build(dockerNodes);
            if (result.State == Result.Error)
            {
                return Result.Error;
            }

            using (_logger.CreateBlock("Result"))
            {
                long count = 0;
                long size = 0;
                var ids = new HashSet<string>();

                foreach (var image in result.Value)
                {
                    if (ids.Add(image.Info.ID))
                    {
                        count++;
                        size += image.Info.Size;
                    }

                    _logger.Log($"{_dockerConverter.TryConvertConvertHashToImageId(image.Info.ID)} {image.Info.Created} {image.RepoTag} {_dockerConverter.ConvertToSize(image.Info.Size, 1)}");
                }

                _logger.Log($"Totals {count} images {_dockerConverter.ConvertToSize(size, 1)}");
            }

            return Result.Success;
        }

        private void ShowNode(TreeNode<TreeDependency> node)
        {
            var toBuild = node.Value.File.Metadata.Repos.Any();
            if (!toBuild)
            {
                return;
            }

            var description = $"{node.Value.Dependency.RepoTag}";

            if (!node.Children.Any())
            {
                _logger.Log(description);
                return;
            }

            using (_logger.CreateBlock(description))
            {
                foreach (var treeNode in node.Children)
                {
                    ShowNode(treeNode);
                }
            }
        }
    }
}
