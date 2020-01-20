namespace TeamCity.Docker.Build
{
    internal interface IPathService
    {
        string Normalize(string path);
    }
}