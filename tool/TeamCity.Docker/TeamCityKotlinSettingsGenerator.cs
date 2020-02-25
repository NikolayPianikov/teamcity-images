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
            lines.Add($"import jetbrains.buildServer.configs.kotlin.v2019_2.*");
            lines.Add("import jetbrains.buildServer.configs.kotlin.v2019_2.vcs.GitVcsRoot");
            lines.Add("import jetbrains.buildServer.configs.kotlin.v2019_2.buildSteps.dockerCommand");
            lines.Add("import jetbrains.buildServer.configs.kotlin.v2019_2.buildFeatures.freeDiskSpace");
            lines.Add("version = \"2019.2\"");
            lines.Add(string.Empty);

            var buildTypes = new List<string>();

            var buildGraphs = _buildGraphsFactory.Create(graph).ToList();
            var counter = 0;
            var names = new HashSet<string>();
            foreach (var buildGraph in buildGraphs)
            {
                var path = new List<INode<IArtifact>>();
                var leaves = buildGraph.Nodes.Except(buildGraph.Links.Select(i => i.To)).ToList();
                foreach (var leaf in leaves)
                {
                    path.AddRange(GetPath(buildGraph, leaf));
                }

                path.Reverse();

                var weight = buildGraph.Nodes
                    .Select(i => i.Value.Weight.Value)
                    .Sum();

                var generalTags = buildGraph.Nodes
                    .Select(i => i.Value)
                    .OfType<Image>()
                    .SelectMany(i => i.File.Tags)
                    .GroupBy(i => i)
                    .Select(i =>new { tag = i.Key , count = i.Count() })
                    .Where(i => i.count > 1)
                    .OrderByDescending(i => i.count)
                    .Select(i => i.tag)
                    .ToList();

                var name = generalTags.Any() ? string.Join(" ", generalTags) : "Build Docker Images";
                var id = name.Replace('-', '_').Replace('.', '_');
                if (!names.Add(name))
                {
                    name = $"{name} {++counter}";
                    id = $"{id}_{++counter}";
                }

                lines.AddRange(GenerateBuildType(id, name, path.Select(i => i.Value).OfType<Image>().ToList(), weight));
                buildTypes.Add(id);
                lines.Add(string.Empty);
            }

            lines.Add($"object root : BuildType({{");
            lines.Add("name = \"Build All Docker Images\"");
            lines.Add($"artifactRules = \"{_pathService.Normalize(_options.TargetPath)} => \"");
            lines.Add("dependencies {");
            foreach (var buildType in buildTypes)
            {
                lines.Add($"dependency({buildType}) {{");
                lines.Add("snapshot {}");
                lines.Add("}");
            }
            lines.Add("}");
            lines.Add("})");

            lines.Add(string.Empty);

            lines.Add("project {");
            lines.Add("vcsRoot(RemoteTeamcityImages)");
            foreach (var buildType in buildTypes)
            {
                lines.Add($"buildType({buildType})");
            }
            
            lines.Add($"buildType(root)");
            
            lines.Add("}"); // project

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

        private IEnumerable<string> GenerateBuildType(string id, string name, ICollection<Image> images, int weight)
        {
            var groups =
                from image in images
                group image by image.File.ImageId
                into groupsByImageId
                from groupByImageId in
                    from image in groupsByImageId
                    group image by image.File
                group groupByImageId by groupsByImageId.Key;

            var description = new StringBuilder();
            foreach (var grp in groups)
            {
                if (description.Length > 0)
                {
                    description.Append(" ");
                }

                description.Append(grp.Key);
                foreach (var aa in grp)
                {
                    var tags = string.Join(",", aa.Key.Tags);
                    if (!string.IsNullOrWhiteSpace(tags))
                    {
                        description.Append(':');
                        description.Append(tags);
                    }
                }
            }

            yield return $"object {name.Replace(' ', '_')} : BuildType({{";
            yield return $"name = \"{name}\"";
            yield return $"description  = \"{description}\"";
            yield return "vcs {root(RemoteTeamcityImages)}";
            yield return "steps {";
            foreach (var image in images)
            {
                yield return "dockerCommand {";
                yield return $"name = \"build {image.File.ImageId}:{string.Join(",", image.File.Tags)}\"";
                yield return "commandType = build {";

                yield return "source = file {";
                yield return $"path = \"\"\"{_pathService.Normalize(Path.Combine(_options.TargetPath, image.File.Path, "Dockerfile"))}\"\"\"";
                yield return "}";

                yield return $"contextDir = \"{_pathService.Normalize(_options.ContextPath)}\"";

                yield return "namesAndTags = \"\"\"";
                foreach (var tag in image.File.Tags)
                {
                    yield return $"{image.File.ImageId}:{tag}";
                }
                yield return "\"\"\".trimIndent()";

                yield return "}";
                yield return $"param(\"dockerImage.platform\", \"{image.File.Platform}\")";
                yield return "}";

                yield return string.Empty;
            }
            yield return "}";

            if (weight > 0)
            {
                yield return "features {";
                yield return "freeDiskSpace {";
                yield return $"requiredSpace = \"{weight}gb\"";
                yield return "failBuild = true";
                yield return "}";
                yield return "}";
            }

            if (!string.IsNullOrWhiteSpace(_options.TeamCityBuildConfigurationId))
            {
                yield return "dependencies {";
                yield return $"dependency(AbsoluteId(\"{_options.TeamCityBuildConfigurationId}\")) {{";
                yield return "snapshot { }";
                yield return "artifacts {";
                yield return $"artifactRules = \"TeamCity-*.tar.gz!/**=>{_pathService.Normalize(_options.ContextPath)}\"";
                yield return "}";
                yield return "}";
                yield return "}";
            }

            yield return "})";
            yield return string.Empty;
        }
    }
}
