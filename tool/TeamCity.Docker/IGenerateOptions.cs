using IoC;

namespace TeamCity.Docker
{
    internal interface IGenerateOptions: Docker.IOptions
    {
        [NotNull] string ContextPath { get; }

        [NotNull] string TargetPath { get; }
    }
}
