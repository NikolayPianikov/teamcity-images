using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal interface IDockerFileGenerator
    {
        DockerFile Generate(string buildPath, string template, IReadOnlyDictionary<string, string> values);
    }
}
