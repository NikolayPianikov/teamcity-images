using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeamCity.Docker.Build;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class ReadmeGenerator : IReadmeGenerator
    {
        private readonly ILogger _logger;
        private readonly IPathService _pathService;
        private readonly IDockerConverter _dockerConverter;

        public ReadmeGenerator(
            ILogger logger,
            IPathService pathService,
            IDockerConverter dockerConverter)
        {
            _logger = logger;
            _pathService = pathService;
            _dockerConverter = dockerConverter;
        }

        public IEnumerable<ReadmeFile> Generate(IEnumerable<DockerFile> dockerFiles)
        {
            var dockerFileGroups =
                from dockerFile in dockerFiles
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
                    sb.AppendLine($"Dockerfile: {_pathService.Normalize(dockerFile.Path)}");
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
                        sb.AppendLine($"- {image}");
                    }

                    sb.AppendLine();
                    counter++;
                }

                _logger.Log($"The readme \"{readmeFilePath}\" file was generated for {counter} docker files.");
                yield return new ReadmeFile(readmeFilePath, sb.ToString(), files);
            }
        }
    }
}
