using System;
using Docker.DotNet.Models;
using IoC;

namespace TeamCity.Docker.Build
{
    internal struct DockerImage
    {
        [NotNull] public readonly ImagesListResponse Info;
        [NotNull] public readonly string RepoTag;
        private readonly string _id;

        public DockerImage([NotNull] ImagesListResponse info, [NotNull] string repoTag)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            RepoTag = repoTag ?? throw new ArgumentNullException(nameof(repoTag));
            _id = string.IsNullOrWhiteSpace(RepoTag) ? Info.ID : RepoTag;
        }

        public override string ToString() => RepoTag;

        public override bool Equals(object obj)
        {
            return obj is DockerImage other && _id == other._id;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}
