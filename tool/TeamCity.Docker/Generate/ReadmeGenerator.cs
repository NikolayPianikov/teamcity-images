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

        public IEnumerable<ReadmeFile> Generate(IEnumerable<TreeNode<TreeDependency>> dockerNodes)
        {
            if (dockerNodes == null)
            {
                throw new ArgumentNullException(nameof(dockerNodes));
            }

            var allNodes = dockerNodes.EnumerateNodes().ToList();
            var repoTags =
                from node in allNodes
                from tag in node.Value.File.Metadata.Tags
                select new { node.Value, tag };

            var dockerFileDictionary = new Dictionary<string, TreeDependency>();
            foreach (var repoTag in repoTags)
            {
                dockerFileDictionary[repoTag.tag] = repoTag.Value;
            }

            var groups =
                from node in allNodes
                group node by node.Value.File.Metadata.ImageId
                into groupsByImageId
                from groupByImageId in 
                    from groupByImageId in groupsByImageId
                    group groupByImageId by groupByImageId.Value.File
                group groupByImageId by groupsByImageId.Key;

            foreach (var groupByImageId in groups)
            {
                var imageId = groupByImageId.Key;
                var readmeFilePath = GetReadmeFilePath(imageId);

                var groupByImage = groupByImageId.ToList();

                var sb = new StringBuilder();
                sb.AppendLine("### Tags");
                foreach (var groupByFile in groupByImage)
                {
                    var dockerFile = groupByFile.Key;
                    sb.AppendLine($"- [{GetReadmeTagName(dockerFile)}](#{GetTagLink(dockerFile)})");
                }

                //var files = dockerNodeGroup.Select(i => i.Value.File).ToList();

                sb.AppendLine();

                var counter = 0;
                foreach (var groupByFile in groupByImage)
                {
                    var dockerFile = groupByFile.Key;
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
                    sb.AppendLine("Installed components:");
                    foreach (var component in dockerFile.Metadata.Components)
                    {
                        sb.AppendLine($"- {component}");
                    }

                    foreach (var node in groupByFile)
                    {
                        var buildPath = GetBuildPath(node).ToList();
                        buildPath.Reverse();

                        sb.AppendLine();
                        sb.AppendLine("Docker build commands:");
                        sb.AppendLine("```");
                        foreach (var path in buildPath)
                        {
                            if (path.Value.Dependency.DependencyType != DependencyType.Build)
                            {
                                continue;
                            }

                            if (dockerFileDictionary.TryGetValue(path.Value.Dependency.RepoTag, out var dependencyDockerFile))
                            {
                                sb.AppendLine(GeneratedDockerBuildCommand(dependencyDockerFile.File));
                            }
                        }

                        sb.AppendLine("```");

                        sb.AppendLine();
                        sb.AppendLine("Base images:");
                        foreach (var dependency in buildPath.Select(i => i.Value))
                        {
                            if (dependency.Dependency.DependencyType == DependencyType.Logical)
                            {
                                continue;
                            }

                            if (dockerFileDictionary.TryGetValue(dependency.Dependency.RepoTag, out var imageDockerFile))
                            {
                                sb.AppendLine($"- [{dependency}]({GetReadmeFilePath(imageDockerFile.File.Metadata.ImageId)}#{GetTagLink(imageDockerFile.File)})");
                            }
                            else
                            {
                                sb.AppendLine($"- {dependency}");
                            }
                        }

                        sb.AppendLine();
                    }

                    counter++;
                }

                _logger.Log($"The readme \"{readmeFilePath}\" file was generated for {counter} docker files.");
                yield return new ReadmeFile(readmeFilePath, sb.ToString());
            }
        }

        private static IEnumerable<TreeNode<TreeDependency>> GetBuildPath(TreeNode<TreeDependency> targetNode)
        {
            do
            {
                yield return targetNode;
                targetNode = targetNode.Parent;
            } while (targetNode != null);
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
