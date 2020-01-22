using IoC;

namespace TeamCity.Docker.Build
{
    internal interface IPathService
    {
        [NotNull] string Normalize([NotNull] string path);
    }
}