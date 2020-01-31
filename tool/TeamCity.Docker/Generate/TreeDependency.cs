namespace TeamCity.Docker.Generate
{
    internal struct TreeDependency
    {
        public readonly DockerFile File;
        
        public TreeDependency(DockerFile file)
        {
            File = file;
        }

        public override string ToString() => File.ToString();

        public override bool Equals(object obj) => obj is TreeDependency other && File.Equals(other.File);

        public override int GetHashCode() => File.GetHashCode();
    }
}
