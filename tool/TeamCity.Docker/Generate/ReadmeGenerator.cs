using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class ReadmeGenerator : IReadmeGenerator
    {
        private readonly ILogger _logger;
        private readonly IPathService _pathService;
        private readonly IOptions _options;
        private readonly IDockerConverter _dockerConverter;

        public ReadmeGenerator(
            [NotNull] ILogger logger,
            [NotNull] IPathService pathService,
            [NotNull] IOptions options,
            [NotNull] IDockerConverter dockerConverter)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _dockerConverter = dockerConverter ?? throw new ArgumentNullException(nameof(dockerConverter));
        }

        public IEnumerable<ReadmeFile> Generate(IEnumerable<TreeNode<DockerFile>> dockerNodes)
        {
            if (dockerNodes == null)
            {
                throw new ArgumentNullException(nameof(dockerNodes));
            }

            var allNodes = EnumerateNodes(dockerNodes).Distinct().ToList();

            var repoTags =
                from node in allNodes
                from tag in node.Value.Metadata.Tags
                select new { node.Value, tag };

            var dockerFileDictionary = new Dictionary<string, DockerFile>();
            foreach (var repoTag in repoTags)
            {
                dockerFileDictionary[repoTag.tag] = repoTag.Value;
            }

            var dockerNodeGroups =
                from node in EnumerateNodes(allNodes)
                group node by new { node.Value.Metadata.ImageId };

            foreach (var dockerNodeGroup in dockerNodeGroups)
            {
                var imageId = dockerNodeGroup.Key.ImageId;
                var readmeFilePath = GetReadmeFilePath(imageId);
                var nodes = dockerNodeGroup.ToList();

                var sb = new StringBuilder();
                sb.AppendLine("### Tags");
                foreach (var node in nodes)
                {
                    var dockerFile = node.Value;
                    sb.AppendLine($"- [{GetReadmeTagName(dockerFile)}](#{GetTagLink(dockerFile)})");
                }

                sb.AppendLine();

                var counter = 0;
                foreach (var node in nodes)
                {
                    var dockerFile = node.Value;
                    sb.AppendLine($"### {GetReadmeTagName(dockerFile)}");

                    sb.AppendLine();
                    sb.AppendLine($"[{Path.GetFileName(dockerFile.Path)}]({_pathService.Normalize(dockerFile.Path)})");

                    if (dockerFile.Metadata.Repos.Any())
                    {
                        sb.AppendLine();
                        sb.AppendLine("The docker image is available on:");
                        foreach (var repo in dockerFile.Metadata.Repos)
                        {
                            sb.AppendLine($"- [{repo}{dockerFile.Metadata.ImageId}]({repo}{dockerFile.Metadata.ImageId})");
                        }
                    }

                    sb.AppendLine();
                    sb.AppendLine("Docker build commands:");
                    sb.AppendLine("```");
                    foreach (var dependency in GetParents(allNodes, node).Concat(Enumerable.Repeat(node, 1)))
                    {
                        sb.AppendLine(GeneratedDockerBuildCommand(dependency.Value));
                    }

                    sb.AppendLine("```");

                    sb.AppendLine();
                    sb.AppendLine("Installed components:");
                    foreach (var component in dockerFile.Metadata.Components)
                    {
                        sb.AppendLine($"- {component}");
                    }

                    sb.AppendLine();
                    sb.AppendLine("Base images:");
                    foreach (var image in dockerFile.Metadata.BaseImages)
                    {
                        if (dockerFileDictionary.TryGetValue(image, out var imageDockerFile))
                        {
                            sb.AppendLine($"- [{image}]({GetReadmeFilePath(imageDockerFile.Metadata.ImageId)}#{GetTagLink(imageDockerFile)})");
                        }
                        else
                        {
                            sb.AppendLine($"- {image}");
                        }
                    }

                    sb.AppendLine();
                    counter++;
                }

                _logger.Log($"The readme \"{readmeFilePath}\" file was generated for {counter} docker files.");
                yield return new ReadmeFile(readmeFilePath, sb.ToString());
            }
        }

        private static IEnumerable<TreeNode<DockerFile>> EnumerateNodes(IEnumerable<TreeNode<DockerFile>> nodes)
        {
            foreach (var node in nodes)
            {
                yield return node;
                foreach (var child in EnumerateNodes(node.Children))
                {
                    yield return child;
                }
            }
        }

        private static IEnumerable<TreeNode<DockerFile>> GetParents(IReadOnlyCollection<TreeNode<DockerFile>> nodes, TreeNode<DockerFile> targetNode)
        {
            foreach (var treeNode in nodes)
            {
                if (treeNode.Children.Contains(targetNode))
                {
                    foreach (var parent in GetParents(nodes, treeNode))
                    {
                        yield return parent;
                    }

                    yield return treeNode;
                }
            }
        }

        private string GetTagLink(DockerFile dockerFile) =>
            string.Join("-", dockerFile.Metadata.Tags.Select(tag => (_dockerConverter.TryConvertRepoTagToTag(tag) ?? string.Empty).Replace(".", string.Empty)));

        private string GetReadmeTagName(DockerFile dockerFile) =>
            string.Join(", ", dockerFile.Metadata.Tags.Select(tag => _dockerConverter.TryConvertRepoTagToTag(tag) ?? string.Empty));

        private static string GetReadmeFilePath(string imageId) => imageId + ".md";

        private string GeneratedDockerBuildCommand(DockerFile dockerFile)
        {
            var dockerFilePath = _pathService.Normalize(Path.Combine(_options.TargetPath, dockerFile.Path));
            var tags = string.Join(" ", dockerFile.Metadata.Tags.Select(tag => $"-t {tag}"));
            return $"docker build -f \"{dockerFilePath}\" {tags} \"{_options.ContextPath}\"";
        }
    }
}
