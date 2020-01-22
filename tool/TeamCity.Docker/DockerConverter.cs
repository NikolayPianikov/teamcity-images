using System;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class DockerConverter : IDockerConverter
    {
        public string TryConvertRepoTagToTag([NotNull] string repoTag)
        {
            if (repoTag == null)
            {
                throw new ArgumentNullException(nameof(repoTag));
            }

            var ind = repoTag.IndexOf(":", StringComparison.Ordinal);
            if (ind > 2)
            {
                return repoTag.Substring(ind + 1);
            }

            return null;
        }

        public string TryConvertRepoTagToRepositoryName([NotNull] string repoTag)
        {
            if (repoTag == null)
            {
                throw new ArgumentNullException(nameof(repoTag));
            }

            var ind = repoTag.IndexOf(":", StringComparison.Ordinal);
            if (ind > 2)
            {
                return repoTag.Substring(0, ind);
            }

            return null;
        }

        public string TryConvertConvertHashToImageId([NotNull] string hash)
        {
            if (hash == null)
            {
                throw new ArgumentNullException(nameof(hash));
            }

            var ind = hash.IndexOf(":", StringComparison.Ordinal);
            if (ind > 2)
            {
                return hash.Substring(ind + 1, 12);
            }

            return null;
        }
    }
}
