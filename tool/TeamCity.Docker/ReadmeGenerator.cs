using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IoC;
using TeamCity.Docker.Generic;
using TeamCity.Docker.Model;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class ReadmeGenerator : IGenerator
    {
        private static readonly Dependency GenerateDependency = new Dependency(DependencyType.Generate);
        [NotNull] private readonly IGenerateOptions _options;
        [NotNull] private readonly IPathService _pathService;

        public ReadmeGenerator(
            [NotNull] IGenerateOptions options,
            [NotNull] IPathService pathService)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
        }

        public void Generate([NotNull] IGraph<IArtifact, Dependency> graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));

            var groups =
                from node in graph.Nodes
                let image = node.Value as Image
                where image != null
                group node by image.File.ImageId
                into groupsByImageId
                from groupByImageId in
                    from groupByImageId in groupsByImageId
                    let image = groupByImageId.Value as Image
                    where image != null
                    group groupByImageId by image.File
                group groupByImageId by groupsByImageId.Key;

            foreach (var groupByImageId in groups)
            {
                var imageId = groupByImageId.Key;
                var lines = new List<string>();
                graph.TryAddNode(new FileArtifact(GetReadmeFilePath(imageId), lines), out var readmeNode);
                var groupByImage = groupByImageId.ToList();

                lines.Add("### Tags");
                foreach (var groupByFile in groupByImage)
                {
                    var dockerFile = groupByFile.Key;
                    lines.Add($"- [{GetReadmeTagName(dockerFile)}](#whale-{GetTagLink(dockerFile)})");
                }

                lines.Add(string.Empty);

                foreach (var groupByFile in groupByImage)
                {
                    var dockerFile = groupByFile.Key;
                    lines.Add($"### :whale: {GetReadmeTagName(dockerFile)}");

                    lines.Add(string.Empty);
                    lines.Add($"[Dockerfile]({_pathService.Normalize(Path.Combine(dockerFile.Path, "Dockerfile"))})");

                    if (dockerFile.Repositories.Any())
                    {
                        lines.Add(string.Empty);
                        lines.Add("The docker image is available on:");
                        foreach (var repo in dockerFile.Repositories)
                        {
                            lines.Add($"- [{repo}{dockerFile.ImageId}]({repo}{dockerFile.ImageId})");
                        }
                    }

                    lines.Add(string.Empty);
                    lines.Add("Installed components:");
                    foreach (var component in dockerFile.Components)
                    {
                        lines.Add($"- {component}");
                    }

                    foreach (var node in groupByFile)
                    {
                        var artifacts = GetArtifacts(graph, node).Reverse().Distinct().ToList();

                        lines.Add(string.Empty);
                        lines.Add("Docker commands:");

                        lines.Add("```");
                        var weight = 0;
                        foreach (var image in artifacts.OfType<Image>())
                        {
                            lines.Add(GenerateCommand(image));
                            weight += image.Weight.Value;
                        }

                        lines.Add("```");

                        lines.Add("Base images:");

                        lines.Add("```");
                        foreach (var reference in artifacts.OfType<Reference>())
                        {
                            lines.Add(GeneratePullCommand(reference));
                            weight += reference.Weight.Value;
                        }

                        lines.Add("```");

                        lines.Add($"_The required free space to generate image(s) is about **{weight} GB**._");
                        lines.Add(string.Empty);
                    }

                    foreach (var node in groupByFile)
                    {
                        graph.TryAddLink(node, GenerateDependency, readmeNode, out _);
                    }
                }
            }
        }

        private static IEnumerable<IArtifact> GetArtifacts(IGraph<IArtifact, Dependency> graph, INode<IArtifact> node)
        {
            yield return node.Value;

            var dependencies = (
                from dependencyLink in graph.Links
                where dependencyLink.From.Equals(node)
                select dependencyLink.To)
                .ToList();

            var images =
                from dependency in dependencies
                let image = dependency.Value as Image
                select new {dependency, image};

            foreach (var image in images)
            {
                foreach (var nestedCommand in GetArtifacts(graph, image.dependency))
                {
                    yield return nestedCommand;
                }
            }
        }

        private static string GeneratePullCommand(Reference reference) => 
            $"docker pull {reference.RepoTag}";

        private string GenerateCommand(Image image)
        {
            var dockerFilePath = _pathService.Normalize(Path.Combine(_options.TargetPath, image.File.Path));
            var tags = string.Join(" ", image.File.Tags.Select(tag => $"-t {tag}"));
            return $"docker build -f \"{dockerFilePath}\" {tags} \"{_options.ContextPath}\"";
        }

        private static string GetReadmeFilePath(string imageId) =>
            imageId + ".md";

        private static string GetTagLink(Dockerfile dockerFile) =>
            string.Join("-", dockerFile.Tags.Select(tag => tag.Replace(".", string.Empty)));

        private static string GetReadmeTagName(Dockerfile dockerFile) =>
            string.Join(", ", dockerFile.Tags);
    }
}
