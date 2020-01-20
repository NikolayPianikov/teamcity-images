using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace TeamCity.Docker
{
    internal interface IEnvironment
    {
        bool IsOSPlatform(OSPlatform platform);

        bool HasEnvironmentVariable(string name);
    }
}