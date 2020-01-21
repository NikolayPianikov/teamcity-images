using System.Collections.Generic;
using System.Linq;
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Generate
{
    internal class DockerVariableReplacer : IDockerVariableReplacer
    {
        private readonly ILogger _logger;

        public DockerVariableReplacer(ILogger logger) => _logger = logger;

        public string Replace(string text, IReadOnlyDictionary<string, string> values)
        {
            var result = values.Aggregate(text, (current, value) => current.Replace("${" + value.Key + "}", value.Value));
            foreach (var line in result.Split('\n'))
            {
                if (line.Contains("${") && line.Contains("}"))
                {
                    _logger.Log($"The line may still contain unresolved variables: \"{line.Trim()}\"", Result.Warning);
                }
            }
            return result;
        }
    }
}
