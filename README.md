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

### Build

To build docker images for Windows run the command line:

```
dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -- build -s configs/windows -f configs/windows.config -c context -i <sessionId>
```

where _sessionId_ is a docker label _SessionId_ to combine all created docker images for the [command](#push).

To build docker images for Linux run the command line:

```
dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -- build -s configs/linux -f configs/linux.config -c context -i <sessionId>
```
### Push

To push docker images for the specific _sessionId_ run the command line:

```
dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -- push -u <username> -p <password> -i <sessionId>
```

where _username_ and _password_ - credentials to [hub.docker.com](https://hub.docker.com/). Also you can specify your custom docker repo by the optional argument `-a`.

For more information run `dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -- --help`.