using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal struct DockerFileVariant
    {
        public readonly string BuildPath;
        public readonly IReadOnlyDictionary<string, string> Variables;

        public DockerFileVariant(string buildPath, IReadOnlyDictionary<string, string> variables)
        {
            BuildPath = buildPath;
            Variables = variables;
        }
    }
}
