using IoC;

namespace TeamCity.Docker.Generate
{
    internal interface IOptions: Docker.IOptions
    {
        [NotNull] string ContextPath { get; }

        [NotNull] string TargetPath { get; }
    }
}
