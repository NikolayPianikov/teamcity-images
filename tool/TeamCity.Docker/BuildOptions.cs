using System.Collections.Generic;
using System.Linq;
using CommandLine;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace TeamCity.Docker
{
    [Verb("build", HelpText = "Build docker images for session.")]
    internal class BuildOptions: IBuildOptions
    {
        [Option('s', "source", Required = false, HelpText = "Path to configuration directory.")]
        public string SourcePath { get; set; } = string.Empty;

        [Option('f', "files", Separator = ';', Required = false, HelpText = "Semicolon separated configuration file.")]
        public IEnumerable<string> ConfigurationFiles { get; set; } = Enumerable.Empty<string>();

        [Option('c', "context", Required = false, HelpText = "Path to the context directory.")]
        public string ContextPath { get; set; } = "";

        [Option('d', "docker", Required = false, HelpText = "The Docker Engine endpoint like tcp://localhost:2375 (defaults: npipe://./pipe/docker_engine fo windows and unix:///var/run/docker.sock for others).")]
        public string DockerEngineEndpoint { get; set; } = string.Empty;

        [Option('t', "tags", Separator = ';', Required = false, HelpText = "Semicolon separated docker image tags.")]
        public IEnumerable<string> Tags { get; set; } = Enumerable.Empty<string>();

        [Option('a', "address", Required = false, HelpText = "Docker server address (default: docker.io).")]
        public string ServerAddress { get; set; } = "docker.io";

        [Option('u', "username", Required = false, HelpText = "Docker server username.")]
        public string Username { get; set; } = "";

        [Option('p', "password", Required = false, HelpText = "Docker server password.")]
        public string Password { get; set; } = "";

        [Option('v', "verbose", Required = false, HelpText = "Add it for detailed output.")]
        public bool VerboseMode { get; set; } = false;
    }
}
