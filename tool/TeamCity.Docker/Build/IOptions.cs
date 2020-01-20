namespace TeamCity.Docker.Build
{
    internal interface IOptions: Docker.IOptions
    {
        string ContextPath { get; }
    }
}
