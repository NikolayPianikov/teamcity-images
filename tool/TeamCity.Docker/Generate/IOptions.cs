namespace TeamCity.Docker.Generate
{
    internal interface IOptions: Docker.IOptions
    {
        string TargetPath { get; }
    }
}
