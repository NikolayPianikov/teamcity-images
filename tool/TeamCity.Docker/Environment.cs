using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class Environment : IEnvironment
    {
        public bool IsOSPlatform(OSPlatform platform) => RuntimeInformation.IsOSPlatform(platform);

        public bool HasEnvironmentVariable(string name) => !string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable(name));
    }
}
