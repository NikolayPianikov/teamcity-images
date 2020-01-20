﻿using System;
using CommandLine;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Push
{
    [Verb("push", HelpText = "Push docker images for session.")]
    internal class Options: IOptions
    {
        [Option('i', "id", Required = true, HelpText = "Session Id.")]
        public string SessionId { get; set; } = "";

        [Option('a', "address", Required = false, HelpText = "Docker server address (default: docker.io).")]
        public string ServerAddress { get; set; } = "docker.io";

        [Option('u', "username", Required = true, HelpText = "Docker server username.")]
        public string Username { get; set; } = "";

        [Option('p', "password", Required = false, HelpText = "Docker server password.")]
        public string Password { get; set; } = "";

        [Option('c', "clean", Required = false, HelpText = "Clean session docker images.")]
        public bool Clean { get; set; } = false;

        [Option('d', "docker", Required = false, HelpText = "The Docker Engine endpoint like tcp://localhost:2375 (default: npipe://./pipe/docker_engine).")]
        public string DockerEngineEndpoint { get; set; } = "npipe://./pipe/docker_engine";

        public string SourcePath => throw new NotImplementedException();
    }
}
