﻿using System;
using System.Collections.Generic;
using System.Linq;
using CommandLine;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace TeamCity.Docker.Generate
{
    [Verb("generate", HelpText = "Generate docker and readme files.")]
    internal class Options: IOptions
    {
        [Option('s', "source", Required = false, HelpText = "Path to configuration directory.")]
        public string SourcePath { get; set; } = string.Empty;

        [Option('i', "id", Required = false, HelpText = "Session Id.")]
        public string SessionId { get; set; } = string.Empty;

        [Option('c', "context", Required = false, HelpText = "Path to the context directory.")]
        public string ContextPath { get; set; } = string.Empty;

        [Option('f', "files", Separator = ';', Required = false, HelpText = "Semicolon separated configuration file.")]
        public IEnumerable<string> ConfigurationFiles { get; set; } = Enumerable.Empty<string>();

        public int Retries => 1;

        [Option('t', "target", Required = true, HelpText = "Path to directory for generating docker files.")]
        public string TargetPath { get; set; } = string.Empty;

        public string DockerEngineEndpoint => throw new NotImplementedException();
    }
}
