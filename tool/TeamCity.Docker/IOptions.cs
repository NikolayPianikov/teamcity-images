using System.Collections.Generic;
using IoC;

namespace TeamCity.Docker
{
    internal interface IOptions
    {
        [NotNull] string SourcePath { get; }

        [NotNull] string DockerEngineEndpoint { get; }

        [NotNull] string SessionId { get; }

        [NotNull] IEnumerable<string> ConfigurationFiles { get; }

        int Retries { get; }

        bool VerboseMode { get; }
    }
}
