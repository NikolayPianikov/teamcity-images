namespace TeamCity.Docker.Generate
{
    internal struct TreeDependency
    {
        public readonly DockerFile File;
        public readonly Dependency Dependency;

        public TreeDependency(DockerFile file, Dependency dependency)
        {
            File = file;
            Dependency = dependency;
        }

        public override string ToString()
        {
            return Dependency.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is TreeDependency other && (File.Equals(other.File) && Dependency.Equals(other.Dependency));
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (File.GetHashCode() * 397) ^ Dependency.GetHashCode();
            }
        }
    }
}
