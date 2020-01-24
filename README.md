# TeamCity docker images

* [Minimal agent](generated/teamcity-minimal-agent.md)
* [Agent](generated/teamcity-agent.md)
* [Server](generated/teamcity-server.md)

## Building of TeamCity docker images

TeamCity team uses `TeamCity.Docker` tool for TeamCity docker images. It requires [.NET Core 2.0+ runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1) installed.

### Generate

You can generate docker and readme files to directory `generated` running the command `dotnet run -p tool\TeamCity.Docker\TeamCity.Docker.csproj -- generate -s configs -f configs\windows.config;configs\linux.config -t generated`

### Build

To build docker images run commands
- for Windows `dotnet run -p tool\TeamCity.Docker\TeamCity.Docker.csproj -- build -s configs\windows -f configs\windows.config -c context -i <sessionId>`
- for Linux `dotnet run -p tool\TeamCity.Docker\TeamCity.Docker.csproj -- build -s configs\linux -f configs\linux.config -c context -i <sessionId>`,
where <sessionId> is a docker label `SessionId` to combine all created docker images for the next command:

### Push

To push docker images for the specific sessionId run commands `dotnet run -p tool\TeamCity.Docker\TeamCity.Docker.csproj -- push -u <username> -p <password> -i <sessionId>`,
where <username> and <password> - credentials to [docker.com](https://hub.docker.com/). You can specify your custom docker repo by the optional argument `-a`.

For more information run `dotnet run -p tool\TeamCity.Docker\TeamCity.Docker.csproj -- --help`.