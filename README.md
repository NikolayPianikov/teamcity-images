# TeamCity docker images

## Docker images on [hub.docker.com](https://hub.docker.com/)

* [Minimal agent](generated/teamcity-minimal-agent.md) on [hub.docker.com](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)
* [Agent](generated/teamcity-agent.md) on [hub.docker.com](https://hub.docker.com/r/jetbrains/teamcity-agent)
* [Server](generated/teamcity-server.md) on [hub.docker.com](https://hub.docker.com/r/jetbrains/teamcity-server)

## Docker images

- Make sure you have got <img align="center" height="18" src="/logo/dotnetcore.png"> SDK installed.

- Download the [TeamCity distributive](https://www.jetbrains.com/teamcity/download/download-thanks.html?platform=linux) like a _TeamCity-*.tar.gz_ and unpack it to directory _context_, thus all TeamCity files should be in the directory _/context/TeamCity_.

- Run a script like _net30_windows_build.cmd_ depending on <img align="center" height="18" src="/logo/dotnetcore.png"> SDK version installed and your operating system. For Linux use _*.sh_ scripts. For instance the version <img align="center" height="18" src="/logo/dotnetcore.png"> SDK v.3 scripts are:
  - to build windows docker images run the script _net30_windows_build.cmd_
  - to build linux docker images run the script _net30_linux_build.cmd_

- These script builds a set of required docker images by localy. You can add the command line argument - a regular expression to filter images to build, for instance _net30_linux_build.cmd 18.04_ will build a set of images with the tag _18.04_ only.

## Generate docker files

- Make sure you have got <img align="center" height="18" src="/logo/dotnetcore.png"> SDK installed.

- Run a script like _net30_generate.cmd_ depending on <img align="center" height="18" src="/logo/dotnetcore.png"> SDK version installed and your operating system. _net30_generate.cmd_ script generates docker files, readme and Kotlin script for TeamCity build configurations.

