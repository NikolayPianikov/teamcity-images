﻿using System.Collections.Generic;
using System.Linq;
using CommandLine;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace TeamCity.Docker.Build
{
    [Verb("build", HelpText = "Build docker images to session.")]
    internal class Options: IOptions
    {
        [Option('s', "source", Required = false, HelpText = "Path to configuration directory.")]
        public string SourcePath { get; set; } = "";

        [Option('i', "id", Required = false, HelpText = "Session Id.")]
        public string SessionId { get; set; } = "";

        [Option('f', "files", Separator = ';', Required = false, HelpText = "Semicolon separated configuration file.")]
        public IEnumerable<string> ConfigurationFiles { get; set; } = Enumerable.Empty<string>();

        [Option('c', "context", Required = false, HelpText = "Path to the context directory.")]
        public string ContextPath { get; set; } = "";

        [Option('d', "docker", Required = false, HelpText = "The Docker Engine endpoint like tcp://localhost:2375 (default: npipe://./pipe/docker_engine).")]
        public string DockerEngineEndpoint { get; set; } = "";

        [Option('t', "tags", Separator = ';', Required = false, HelpText = "Semicolon separated docker image tags.")]
        public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();
    }
}
