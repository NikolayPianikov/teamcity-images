# TeamCity docker images

* [Minimal agent](generated/teamcity-minimal-agent.md) on [hub.docker.com](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)
* [Agent](generated/teamcity-agent.md) on [hub.docker.com](https://hub.docker.com/r/jetbrains/teamcity-agent)
* [Server](generated/teamcity-server.md) on [hub.docker.com](https://hub.docker.com/r/jetbrains/teamcity-server)

### Building docker images

Run a script like _net30_windows_build.cmd_. For instance .net core 3 scripts are:
- to build windows docker images run the script _net30_windows_build.cmd_
- to build linux docker images run the script _net30_linux_build.cmd_

These script builds a set of required docker images by local docker. You can add the command line argument - a regular expression to filter images to build, for instance _net30_linux_build.cmd 18.04_ will build images with tag _18.04_ only.

### Generate docker files

Run a script like _net30_generate.cmd_. _net30_generate.cmd_ script generates docker files, readme and Kotlin script for TeamCity build configurations.

