#!/bin/bash

dotnet run -p tool/TeamCity.Docker/TeamCity.Docker.csproj -f $1 -- build -s "configs/linux" -f "configs/common.config;configs/linux.config$3" -c context