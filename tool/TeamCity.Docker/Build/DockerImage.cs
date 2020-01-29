using System;
using Docker.DotNet.Models;
using IoC;

namespace TeamCity.Docker.Build
{
    internal struct DockerImage
    {
        [NotNull] public readonly ImagesListResponse Info;
        [NotNull] public readonly string RepoTag;

        public DockerImage([NotNull] ImagesListResponse info, [NotNull] string repoTag)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            RepoTag = repoTag ?? throw new ArgumentNullException(nameof(repoTag));
        }
    }
}
