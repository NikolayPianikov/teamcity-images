// ReSharper disable UnusedMember.Global

using IoC;

namespace TeamCity.Docker.Generate
{
    internal interface IResources
    {
        [CanBeNull] string GetResource([NotNull] string resourceName);
    }
}