﻿namespace TeamCity.Docker
{
    internal interface IOptions
    {
        string SourcePath { get; }

        string DockerEngineEndpoint { get; }

        string SessionId { get; }

        string ConfigurationFiles { get; }
    }
}
