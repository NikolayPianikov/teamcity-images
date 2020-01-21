namespace TeamCity.Docker.Generate
{
    internal struct DockerVariable
    {
        public readonly string Name;
        public readonly string Value;

        public DockerVariable(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
