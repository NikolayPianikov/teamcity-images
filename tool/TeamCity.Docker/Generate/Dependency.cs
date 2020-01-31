using System;
using IoC;

namespace TeamCity.Docker.Generate
{
    internal struct Dependency
    {
        [NotNull] public readonly string RepoTag;
        public readonly DependencyType DependencyType;

        public Dependency([NotNull] string repoTag, DependencyType dependencyType)
        {
            RepoTag = repoTag ?? throw new ArgumentNullException(nameof(repoTag));
            DependencyType = dependencyType;
        }

        public override bool Equals(object obj)
        {
            return obj is Dependency other && RepoTag == other.RepoTag && DependencyType == other.DependencyType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (RepoTag.GetHashCode() * 397) ^ (int) DependencyType;
            }
        }

        public override string ToString()
        {
            return $"{DependencyType} {RepoTag}";
        }
    }
}
