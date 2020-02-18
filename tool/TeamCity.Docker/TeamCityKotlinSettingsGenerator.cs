using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IoC;
using TeamCity.Docker.Generic;
using TeamCity.Docker.Model;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class TeamCityKotlinSettingsGenerator: IGenerator
    {
        [NotNull] private readonly IGenerateOptions _options;
        [NotNull] private readonly IBuildGraphsFactory _buildGraphsFactory;
        [NotNull] private readonly IPathService _pathService;

        public TeamCityKotlinSettingsGenerator(
            [NotNull] IGenerateOptions options,
            [NotNull] IBuildGraphsFactory buildGraphsFactory,
            [NotNull] IPathService pathService)
        {
            _options = options;
            _buildGraphsFactory = buildGraphsFactory ?? throw new ArgumentNullException(nameof(buildGraphsFactory));
            _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
        }

        public void Generate([NotNull] IGraph<IArtifact, Dependency> graph)
        {
            if (graph == null) throw new ArgumentNullException(nameof(graph));
            if (string.IsNullOrWhiteSpace(_options.TeamCityDslPath))
            {
                return;
            }

            var lines = new List<string>();
            lines.Add("import jetbrains.buildServer.configs.kotlin.v2019_2.*");
            lines.Add("import jetbrains.buildServer.configs.kotlin.v2019_2.vcs.GitVcsRoot");
            lines.Add("import jetbrains.buildServer.configs.kotlin.v2019_2.buildSteps.dockerCommand");
            lines.Add("version = \"2019.2\"");
            lines.Add(string.Empty);

            var buildTypes = new HashSet<string>();

            var buildGraphs = _buildGraphsFactory.Create(graph).ToList();
            foreach (var buildGraph in buildGraphs)
            {
                var path = new List<INode<IArtifact>>();
                var leaves = buildGraph.Nodes.Except(buildGraph.Links.Select(i => i.To)).ToList();
                foreach (var leaf in leaves)
                {
                    path.AddRange(GetPath(buildGraph, leaf));
                }

                path.Reverse();

                var groups =
                    from node in buildGraph.Nodes
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

                var nameSb = new StringBuilder();
                foreach (var grp in groups)
                {
                    if (nameSb.Length > 0)
                    {
                        nameSb.Append("_");
                    }

                    nameSb.Append(grp.Key);
                    foreach (var aa in grp)
                    {
                        var tags = string.Join(",", aa.Key.Tags);
                        if (!string.IsNullOrWhiteSpace(tags))
                        {
                            nameSb.Append(':');
                            nameSb.Append(tags);
                        }
                    }
                }

                var name = nameSb.ToString();
                buildTypes.Add(name);

                lines.AddRange(GenerateBuildType(name, path.Select(i => i.Value).OfType<Image>()));
                buildTypes.Add(name);
                lines.Add(string.Empty);
            }

            lines.Add("project {");
            lines.Add("vcsRoot(RemoteTeamcityImages)");
            foreach (var buildType in buildTypes)
            {
                lines.Add($"buildType({buildType})");
            }
            lines.Add("}");
            lines.Add(string.Empty);

            lines.Add("object RemoteTeamcityImages : GitVcsRoot({");
            lines.Add("name = \"remote teamcity images\"");
            lines.Add("url = \"https://github.com/NikolayPianikov/teamcity-images.git\"");
            lines.Add("})");

            graph.TryAddNode(new FileArtifact(Path.Combine(_options.TeamCityDslPath, "settings.kts"), lines), out var dslNode);
        }

        private static IEnumerable<INode<IArtifact>> GetPath(IGraph<IArtifact, Dependency> graph, INode<IArtifact> node)
        {
            yield return node;

            var dependencies = (
                    from dependencyLink in graph.Links
                    where dependencyLink.From.Equals(node)
                    select dependencyLink.To)
                .ToList();

            var images =
                from dependency in dependencies
                let image = dependency.Value as Image
                select new { dependency, image };

            foreach (var image in images)
            {
                foreach (var nestedCommand in GetPath(graph, image.dependency))
                {
                    yield return nestedCommand;
                }
            }
        }

        private IEnumerable<string> GenerateBuildType(string name, IEnumerable<Image> images)
        {
            yield return $"object {name} : BuildType({{";
            yield return $"name = \"build docker image {name}\"";
            yield return "steps {";
            foreach (var image in images)
            {
                yield return "dockerCommand {";
                yield return $"name = \"build {image.File.ImageId}:{string.Join(",", image.File.Tags)}\"";
                yield return "commandType = build {";

                yield return "source = file {";
                yield return $"path = \"\"\"{_pathService.Normalize(Path.Combine(_options.TargetPath, image.File.Path))}\"\"\"";
                yield return "}";

                yield return $"contextDir = \"{_pathService.Normalize(_options.ContextPath)}\"";

                yield return "namesAndTags = \"\"\"";
                foreach (var tag in image.File.Tags)
                {
                    yield return $"{image.File.ImageId}:{tag}";
                }
                yield return "\"\"\".trimIndent()";

                yield return "commandArgs = \"--pull\"";

                yield return "}";
                yield return $"param(\"dockerImage.platform\", \"{image.File.Platform}\")";
                yield return "}";

                yield return string.Empty;
            }
            yield return "}";
            yield return "})";
            yield return string.Empty;
        }
    }
}
