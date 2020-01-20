# Buildung of TeamCity docker images

TeamCity team uses `TeamCity.Docker` tool for TeamCity docker images. It requires [.NET Core 3.1 runtime](https://dotnet.microsoft.com/download/dotnet-core/3.1) installed.

## Generate

You can generate docker and readme files running the command `dotnet run -p tool\TeamCity.Docker\TeamCity.Docker.csproj -- generate -s configs -t generated`

## Build

To build docker images run commands
- for Windows `dotnet run -p tool\TeamCity.Docker\TeamCity.Docker.csproj -- build -s configs\Windows -c context -i <sessionId>`
- for Unix `dotnet run -p tool\TeamCity.Docker\TeamCity.Docker.csproj -- build -s configs\Unix -c context -i <sessionId>`,
where <sessionId> - is some tag to combine all created docker images for the next command:

## Push
To push docker images for the specific sessionId run commands `dotnet run -p tool\TeamCity.Docker\TeamCity.Docker.csproj -- push -u <username> -p <password> -i <sessionId>`,
where <username> and <password> - credentials to [docker.com](https://hub.docker.com/).

For more information see help by the command `dotnet run -p tool\TeamCity.Docker\TeamCity.Docker.csproj -- --help`.