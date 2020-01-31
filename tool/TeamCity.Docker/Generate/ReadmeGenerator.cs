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

            var dockerFileDictionary = new Dictionary<Dependency, DockerFile>();
            foreach (var dependency 
                in from node in allNodes
                from dependency in node.Value.File.Metadata.Dependencies
                select new { node.Value.File, dependency })
            {
                dockerFileDictionary[dependency.dependency] = dependency.File;
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
                            foreach (var dependency in path.Metadata.Dependencies.Where(i => i.DependencyType == DependencyType.Build))
                            {
                                if (dockerFileDictionary.TryGetValue(dependency, out var dependencyDockerFile))
                                {
                                    sb.AppendLine(GeneratedDockerBuildCommand(dependencyDockerFile));
                                }
                            }
                        }

                        sb.AppendLine("```");

                        sb.AppendLine();
                        sb.AppendLine("Base images:");
                        foreach (var path in buildPath)
                        {
                            foreach (var dependency in path.Metadata.Dependencies.Where(i => i.DependencyType == DependencyType.Build || i.DependencyType == DependencyType.Pull))
                            {
                                if (dockerFileDictionary.TryGetValue(dependency, out var imageDockerFile))
                                {
                                    sb.AppendLine($"- [{dependency.RepoTag}]({GetReadmeFilePath(imageDockerFile.Metadata.ImageId)}#{GetTagLink(imageDockerFile)})");
                                }
                                else
                                {
                                    sb.AppendLine($"- {dependency.RepoTag}");
                                }
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

        private static IEnumerable<DockerFile> GetBuildPath(TreeNode<TreeDependency> targetNode)
        {
            do
            {
                var parent = targetNode.Parent;
                yield return targetNode.Value.File;
                targetNode = parent;
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
