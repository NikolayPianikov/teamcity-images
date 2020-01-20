using System.Collections.Generic;
// ReSharper disable ParameterTypeCanBeEnumerable.Local

namespace TeamCity.Docker.Generate
{
    internal struct ReadmeFile
    {
        public readonly string Path;
        public readonly string Content;
        public readonly IEnumerable<DockerFile> DockerFiles;

        public ReadmeFile(string path, string content, IReadOnlyCollection<DockerFile> dockerFiles)
        {
            Path = path;
            Content = content;
            DockerFiles = dockerFiles;
        }
    }
}
