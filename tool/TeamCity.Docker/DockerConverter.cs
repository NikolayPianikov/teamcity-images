using System;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class DockerConverter : IDockerConverter
    {
        public string? TryConvertRepoTagToTag(string repoTag)
        {
            var ind = repoTag.IndexOf(":", StringComparison.Ordinal);
            if (ind > 2)
            {
                return repoTag.Substring(ind + 1);
            }

            return null;
        }

        public string? TryConvertRepoTagToRepositoryName(string repoTag)
        {
            var ind = repoTag.IndexOf(":", StringComparison.Ordinal);
            if (ind > 2)
            {
                return repoTag.Substring(0, ind);
            }

            return null;
        }

        public string? TryConvertConvertHashToImageId(string hash)
        {
            var ind = hash.IndexOf(":", StringComparison.Ordinal);
            if (ind > 2)
            {
                return hash.Substring(ind + 1, 12);
            }

            return null;
        }
    }
}
