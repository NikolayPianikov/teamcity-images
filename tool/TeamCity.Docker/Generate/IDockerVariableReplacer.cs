using System.Collections.Generic;

namespace TeamCity.Docker.Generate
{
    internal interface IDockerVariableReplacer
    {
        string Replace(string text, IReadOnlyDictionary<string, string> values);
    }
}