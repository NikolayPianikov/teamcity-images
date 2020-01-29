using System.Collections.Generic;
using System.Linq;
using CommandLine;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace TeamCity.Docker.Build
{
    [Verb("build", HelpText = "Build docker images for session.")]
    internal class Options: IOptions
    {
        [Option('s', "source", Required = false, HelpText = "Path to configuration directory.")]
        public string SourcePath { get; set; } = string.Empty;

        [Option('i', "id", Required = false, HelpText = "Session Id.")]
        public string SessionId { get; set; } = string.Empty;

        [Option('f', "files", Separator = ';', Required = false, HelpText = "Semicolon separated configuration file.")]
        public IEnumerable<string> ConfigurationFiles { get; set; } = Enumerable.Empty<string>();

        [Option('r', "retries", Required = false, HelpText = "Defines how many retries do if error occurs.")]
        public int Retries { get; } = 3;

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

        [Option('n', "clean", Required = false, HelpText = "Clean session docker images.")]
        public bool Clean { get; set; } = false;
    }
}
