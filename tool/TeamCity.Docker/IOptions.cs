using System.Collections.Generic;

namespace TeamCity.Docker
{
    internal interface IOptions
    {
        string SourcePath { get; }

        string DockerEngineEndpoint { get; }

        string SessionId { get; }

        IEnumerable<string> ConfigurationFiles { get; }
    }
}
