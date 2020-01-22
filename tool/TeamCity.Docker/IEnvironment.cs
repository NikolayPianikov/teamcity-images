using System.Runtime.InteropServices;
using IoC;

// ReSharper disable InconsistentNaming

namespace TeamCity.Docker
{
    internal interface IEnvironment
    {
        bool IsOSPlatform(OSPlatform platform);

        bool HasEnvironmentVariable([NotNull] string name);
    }
}