using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Docker.DotNet;
using IoC;
using TeamCity.Docker.Generate;

namespace TeamCity.Docker.Build
{
    internal interface IImageBuilder
    {
        [NotNull] Task<Result> Build(
            [NotNull] IDockerClient dockerClient,
            [NotNull] IReadOnlyCollection<DockerFile> dockerFiles,
            [NotNull] string dockerFilesRootPath,
            [NotNull] Stream contextStream);
    }
}