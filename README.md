# TeamCity docker images

* [Minimal agent](generated/teamcity-minimal-agent.md) on [hub.docker.com](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)
* [Agent](generated/teamcity-agent.md) on [hub.docker.com](https://hub.docker.com/r/jetbrains/teamcity-agent)
* [Server](generated/teamcity-server.md) on [hub.docker.com](https://hub.docker.com/r/jetbrains/teamcity-server)

## Building of TeamCity docker images

TeamCity team uses `TeamCity.Docker` tool for TeamCity docker images. It requires [.NET Core 2.0+ runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1) installed.

### Generate

You can generate docker and readme files to `generated` directory running the command line:
```
dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -- generate -s configs -f configs/windows.config;configs/linux.config -c context -t generated
```

### Prepare a docker build context

Download [TeamCity-*.tar.gz file](https://www.jetbrains.com/teamcity/download/#section=section-get) and unpack it into the build context directory `context` to have `context\TeamCity`

### Build & Push

To build docker image based on the specific dockerfile see the list of command lines for required docker tag.

To build latest TeamCity docker images for Windows run the command line:

```
dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -- build -s configs/windows -f configs/windows.config -c context -u <username> -p <password>
```

To build build latest TeamCity docker images for Linux run the command line:

```
dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -- build -s configs/linux -f configs/linux.config -c context -u <username> -p <password>
```
where _username_ and _password_ - credentials to [hub.docker.com](https://hub.docker.com/). Also you can specify your custom docker repo by the optional argument `-a`.

For more information run `dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -- --help`.