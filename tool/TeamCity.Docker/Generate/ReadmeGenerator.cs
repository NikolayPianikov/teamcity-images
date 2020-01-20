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
                sb.AppendLine("| Tags | Base images | Components | Dockerfile |");
                sb.AppendLine("| ---- | ----------- | ---------- | ---------- |");

                var files = dockerFileGroup.ToList();
                foreach (var dockerFile in files)
                {
                    sb.Append('|');
                    sb.Append(string.Join(":", dockerFile.Metadata.Tags.Select(tag => _dockerConverter.TryConvertRepoTagToTag(tag))));
                    sb.Append('|');
                    sb.Append(string.Join(", ", dockerFile.Metadata.BaseImages));
                    sb.Append('|');
                    sb.Append(string.Join(", ", dockerFile.Metadata.Components));
                    sb.Append('|');
                    sb.Append(_pathService.Normalize(dockerFile.Path));
                    sb.Append('|');
                    sb.AppendLine();
                }

                _logger.Log($"The readme \"{readmeFilePath}\" file was generated.");
                yield return new ReadmeFile(readmeFilePath, sb.ToString(), files);
            }
        }
    }
}
