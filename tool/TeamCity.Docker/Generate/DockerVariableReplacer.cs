using System.Collections.Generic;
using System.Linq;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class DockerVariableReplacer : IDockerVariableReplacer
    {
        public string Replace(string text, IReadOnlyDictionary<string, string> values) => 
            values.Aggregate(text, (current, value) => current.Replace("${" + value.Key + "}", value.Value));
    }
}
