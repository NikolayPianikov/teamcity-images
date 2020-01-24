using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IoC;
using TeamCity.Docker.Build;

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

        public IEnumerable<ReadmeFile> Generate(IEnumerable<DockerFile> dockerFiles)
        {
            if (dockerFiles == null)
            {
                throw new ArgumentNullException(nameof(dockerFiles));
            }

            var allDockerFiles = dockerFiles.ToList();

            var repoTags = (
                from file in allDockerFiles
                from tag in file.Metadata.Tags
                select new { file, tag });

            var dockerFileDictionary = new Dictionary<string, DockerFile>();
            foreach (var repoTag in repoTags)
            {
                dockerFileDictionary[repoTag.tag] = repoTag.file;
            }

            var dockerFileGroups =
                from dockerFile in allDockerFiles
                group dockerFile by new {dockerFile.Metadata.ImageId};

            foreach (var dockerFileGroup in dockerFileGroups)
            {
                var imageId = dockerFileGroup.Key.ImageId;
                var readmeFilePath = imageId + ".md";

                var sb = new StringBuilder();
                var files = dockerFileGroup.ToList();
                var counter = 0;
                foreach (var dockerFile in files)
                {
                    var tags = string.Join(", ", dockerFile.Metadata.Tags.Select(tag => _dockerConverter.TryConvertRepoTagToTag(tag)));
                    sb.AppendLine($"### {tags}");

                    sb.AppendLine();
                    sb.AppendLine($"[{Path.GetFileName(dockerFile.Path)}]({_pathService.Normalize(dockerFile.Path)})");
                    
                    sb.AppendLine();
                    var dependencies = new Queue<DockerFile>();
                    var targetDockerFiles = new List<DockerFile>();
                    dependencies.Enqueue(dockerFile);
                    while (dependencies.Count > 0)
                    {
                        var dependency = dependencies.Dequeue();
                        if (targetDockerFiles.Contains(dependency))
                        {
                            continue;
                        }

                        targetDockerFiles.Add(dependency);
                        foreach (var baseImage in dependency.Metadata.BaseImages)
                        {
                            if (dockerFileDictionary.TryGetValue(baseImage, out var newDockerFile))
                            {
                                dependencies.Enqueue(newDockerFile);
                            }
                        }
                    }

                    targetDockerFiles.Reverse();
                    sb.AppendLine("Docker build commands:");
                    sb.AppendLine("```");
                    foreach (var dependency in targetDockerFiles.Distinct())
                    {
                        sb.AppendLine(GeneratedDockerBuildCommand(dependency));
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
                            var dockerFilePath = _pathService.Normalize(imageDockerFile.Path);
                            sb.AppendLine($"- [{image}]({dockerFilePath})");
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
                yield return new ReadmeFile(readmeFilePath, sb.ToString(), files);
            }
        }

        private string GeneratedDockerBuildCommand(DockerFile dockerFile)
        {
            var dockerFilePath = _pathService.Normalize(Path.Combine(_options.TargetPath, dockerFile.Path));
            var tags = string.Join(" ", dockerFile.Metadata.Tags.Select(tag => $"-t {tag}"));
            return $"docker build -f \"{dockerFilePath}\" {tags} \"{_options.ContextPath}\"";
        }
    }
}
