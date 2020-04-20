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
    internal class TeamCityKotlinSettingsGenerator : IGenerator
    {
        [NotNull] private readonly IGenerateOptions _options;
        [NotNull] private readonly IFactory<IEnumerable<IGraph<IArtifact, Dependency>>, IGraph<IArtifact, Dependency>> _buildGraphsFactory;
        [NotNull] private readonly IPathService _pathService;
        [NotNull] private readonly IBuildPathProvider _buildPathProvider;
        [NotNull] private readonly IFactory<string, IGraph<IArtifact, Dependency>> _graphNameFactory;

        public TeamCityKotlinSettingsGenerator(
            [NotNull] IGenerateOptions options,
            [NotNull] IFactory<IEnumerable<IGraph<IArtifact, Dependency>>, IGraph<IArtifact, Dependency>> buildGraphsFactory,
            [NotNull] IPathService pathService,
            [NotNull] IBuildPathProvider buildPathProvider,
            [NotNull] IFactory<string, IGraph<IArtifact, Dependency>> graphNameFactory)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _buildGraphsFactory = buildGraphsFactory ?? throw new ArgumentNullException(nameof(buildGraphsFactory));
            _pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
            _buildPathProvider = buildPathProvider ?? throw new ArgumentNullException(nameof(buildPathProvider));
            _graphNameFactory = graphNameFactory ?? throw new ArgumentNullException(nameof(graphNameFactory));
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
            lines.Add("import jetbrains.buildServer.configs.kotlin.v2019_2.buildFeatures.dockerSupport");
            lines.Add("import jetbrains.buildServer.configs.kotlin.v2019_2.buildFeatures.freeDiskSpace");
            lines.Add("version = \"2019.2\"");
            lines.Add(string.Empty);

            var buildTypes = new List<string>();
            var buildGraphResult = _buildGraphsFactory.Create(graph);
            if (buildGraphResult.State == Result.Error)
            {
                return;
            }

            var buildGraphs = buildGraphResult.Value.ToList();
            var counter = 0;
            var names = new HashSet<string>();
            foreach (var buildGraph in buildGraphs)
            {
                var path = _buildPathProvider.GetPath(buildGraph).ToList();

                var weight = buildGraph.Nodes
                    .Select(i => i.Value.Weight.Value)
                    .Sum();

                var name = _graphNameFactory.Create(buildGraph).Value;
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = "Build Docker Images";
                }
                
                if (!names.Add(name))
                {
                    name = $"{name} {++counter}";
                }

                var id = _options.TeamCityBuildConfigurationId + "_" + name.Replace(' ', '_').Replace('-', '_').Replace('.', '_');
                lines.AddRange(GenerateBuildType(id, name, path.Select(i => i.Value).OfType<Image>().ToList(), weight));
                buildTypes.Add(id);
                lines.Add(string.Empty);
            }

            lines.Add("object root : BuildType({");
            lines.Add("name = \"Build All Docker Images\"");
            lines.Add("dependencies {");
            
            lines.Add($"snapshot(AbsoluteId(\"{_options.TeamCityBuildConfigurationId}\"))");
            lines.Add("{}");

            foreach (var buildType in buildTypes)
            {
                lines.Add($"snapshot({buildType})");
                lines.Add("{}");
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

            lines.Add("buildType(root)");

            lines.Add("}"); // project

            lines.Add(string.Empty);

            lines.Add("object RemoteTeamcityImages : GitVcsRoot({");
            lines.Add("name = \"remote teamcity images\"");
            lines.Add("url = \"https://github.com/NikolayPianikov/teamcity-images.git\"");
            lines.Add("})");

            graph.TryAddNode(new FileArtifact(_pathService.Normalize(Path.Combine(_options.TeamCityDslPath, "settings.kts")), lines), out var dslNode);
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

            yield return $"object {id} : BuildType({{";
            yield return $"name = \"{name}\"";
            yield return $"description  = \"{description}\"";
            yield return "vcs {root(RemoteTeamcityImages)}";
            yield return "steps {";
            // docker build
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

            // docker image tag
            foreach (var image in images)
            {
                if (image.File.Tags.Any())
                {
                    foreach (var tag in image.File.Tags)
                    {
                        yield return "dockerCommand {";
                        yield return $"name = \"image tag {image.File.ImageId}:{tag}\"";
                        yield return "commandType = other {";

                        yield return "subCommand = \"tag\"";
                        yield return $"commandArgs = \"{image.File.ImageId}:{tag} %repository%{image.File.ImageId}:{tag}\"";

                        yield return "}";
                        yield return "}";

                        yield return string.Empty;
                    }
                }
            }

            // docker push
            foreach (var image in images)
            {
                yield return "dockerCommand {";
                yield return $"name = \"push {image.File.ImageId}:{string.Join(",", image.File.Tags)}\"";
                yield return "commandType = push {";

                yield return "namesAndTags = \"\"\"";
                foreach (var tag in image.File.Tags)
                {
                    yield return $"%repository%{image.File.ImageId}:{tag}";
                }

                yield return "\"\"\".trimIndent()";

                yield return "}";
                yield return "}";

                yield return string.Empty;
            }

            yield return "}";

            yield return "features {";

            if (weight > 0)
            {
                yield return "freeDiskSpace {";
                yield return $"requiredSpace = \"{weight}gb\"";
                yield return "failBuild = true";
                yield return "}";
            }

            if (!string.IsNullOrWhiteSpace(_options.TeamCityDockerRegistryId))
            {
                yield return "dockerSupport {";
                yield return "loginToRegistry = on {";
                yield return $"dockerRegistryId = \"{_options.TeamCityDockerRegistryId}\"";
                yield return "}";
                yield return "}";
            }

            yield return "}";

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
