dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -- generate -s configs -f configs/windows.config;configs/windows-internal.config;configs/linux.config;configs/linux-internal.config -c context -t context/generated -d .teamcity -b TC2019_2_BuildDist
dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -- generate -s configs -f configs/windows.config;configs/linux.config -c context -t generated