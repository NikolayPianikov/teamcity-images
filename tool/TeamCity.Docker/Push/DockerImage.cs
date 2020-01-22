using Docker.DotNet.Models;

namespace TeamCity.Docker.Push
{
    internal struct DockerImage
    {
        public readonly ImagesListResponse Info;
        public readonly string RepoTag;

        public DockerImage(ImagesListResponse info, string repoTag)
        {
            Info = info;
            RepoTag = repoTag;
        }
    }
}
