# TeamCity docker images

* [Minimal agent](generated/teamcity-minimal-agent.md) on [hub.docker.com](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)
* [Agent](generated/teamcity-agent.md) on [hub.docker.com](https://hub.docker.com/r/jetbrains/teamcity-agent)
* [Server](generated/teamcity-server.md) on [hub.docker.com](https://hub.docker.com/r/jetbrains/teamcity-server)

### Building docker images

Run a script like _net30_windows_build.cmd_. For instance .net core 3 scripts are:
- to build windows docker images run the script _net30_windows_build.cmd_
- to build linux docker images run the script _net30_windows_build.cmd_

These script builds a set of required docker images by local docker.

### Generate docker files

Run a script like _net30_generate.cmd_. _net30_generate.cmd_ script generates docker files, readme and Kotlin script for TeamCity build configurations.

