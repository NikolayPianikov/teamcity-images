using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal interface IDockerFileContentParser
    {
        IEnumerable<DockerLine> Parse(string text, IReadOnlyDictionary<string, string> values);
    }
}