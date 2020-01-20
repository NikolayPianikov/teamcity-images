using System.Runtime.InteropServices;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Build
{
    internal class PathService : IPathService
    {
        private readonly IEnvironment _environment;

        public PathService(IEnvironment environment) => _environment = environment;

        public string Normalize(string path) => 
            _environment.IsOSPlatform(OSPlatform.Windows) ? path.Replace('\\', '/') : path;
    }
}
