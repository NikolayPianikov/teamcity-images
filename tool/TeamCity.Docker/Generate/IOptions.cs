using IoC;

namespace TeamCity.Docker.Generate
{
    internal interface IOptions: Docker.IOptions
    {
        [NotNull] string TargetPath { get; }
    }
}
