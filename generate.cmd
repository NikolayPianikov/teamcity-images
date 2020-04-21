rmdir "generated" /s /q
rmdir "context/generated" /s /q

dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -f %1 -- generate -s configs -f "configs/common.config;configs/windows.config;configs/windows-internal.config;configs/linux.config;configs/linux-internal.config" -c context -t context/generated -d .teamcity -b "TC2019_2_BuildDist:2019.2.4;TC_Trunk_BuildDist:2020.1.EAP3" -r "%%docker.registryId%%"
dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -f %1 -- generate -s configs -f "configs/common.config;configs/windows.config;configs/linux.config" -c context -t generated