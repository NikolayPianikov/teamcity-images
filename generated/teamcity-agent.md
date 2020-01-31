### Tags
- [18.04, linux](#1804-linux)
- [latest-windowsservercore-1803](#latest-windowsservercore-1803)
- [latest-nanoserver-1803](#latest-nanoserver-1803)
- [latest-windowsservercore-1809](#latest-windowsservercore-1809)
- [latest-nanoserver-1809](#latest-nanoserver-1809)
- [latest-windowsservercore-1903](#latest-windowsservercore-1903)
- [latest-nanoserver-1903](#latest-nanoserver-1903)

### 18.04, linux

[Dockerfile](linux/Agent/Ubuntu/18.04/Dockerfile)

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:
- Git
- Mercurial
- [.NET Core SDK x64 v.3.1.100](https://dotnetcli.blob.core.windows.net/dotnet/Sdk/3.1.100/dotnet-sdk-3.1.100-linux-x64.tar.gz)

Docker build commands:
```
docker build -f "generated/linux/MinimalAgent/Ubuntu/18.04/Dockerfile" -t teamcity-minimal-agent:18.04 -t teamcity-minimal-agent:linux "context"
docker build -f "generated/linux/Agent/Ubuntu/18.04/Dockerfile" -t teamcity-agent:18.04 -t teamcity-agent:linux "context"
```

Base images:
- [ubuntu:18.04](teamcity-minimal-agent.md#1804-linux)
- [teamcity-minimal-agent:18.04](teamcity-agent.md#1804-linux)

### latest-windowsservercore-1803

[Dockerfile](windows/Agent/windowsservercore/1803/Dockerfile)

Installed components:
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](https://github.com/git-for-windows/git/releases/download/v2.19.1.windows.1/MinGit-2.19.1-64-bit.zip)
- [Mercurial x64 v.4.7.2](https://bitbucket.org/tortoisehg/files/downloads/mercurial-4.7.2-x64.msi)

Docker build commands:
```
docker build -f "generated/windows/Server/nanoserver/1803/Dockerfile" -t teamcity-server:latest-nanoserver-1803 "context"
docker build -f "generated/windows/Agent/windowsservercore/1803/Dockerfile" -t teamcity-agent:latest-windowsservercore-1803 "context"
docker build -f "generated/windows/Agent/windowsservercore/1803/Dockerfile" -t teamcity-agent:latest-windowsservercore-1803 "context"
```

Base images:
- [mcr.microsoft.com/powershell:nanoserver-1803](teamcity-server.md#latest-nanoserver-1803)
- [mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1803](teamcity-agent.md#latest-windowsservercore-1803)
- [teamcity-minimal-agent:latest-nanoserver-1803](teamcity-agent.md#latest-windowsservercore-1803)

### latest-nanoserver-1803

[Dockerfile](windows/Agent/nanoserver/1803/Dockerfile)

Installed components:
- [.NET Core SDK x64 v.3.1.100](https://dotnetcli.blob.core.windows.net/dotnet/Sdk/3.1.100/dotnet-sdk-3.1.100-win-x64.zip)

Docker build commands:
```
docker build -f "generated/windows/Server/nanoserver/1803/Dockerfile" -t teamcity-server:latest-nanoserver-1803 "context"
docker build -f "generated/windows/Agent/windowsservercore/1803/Dockerfile" -t teamcity-agent:latest-windowsservercore-1803 "context"
docker build -f "generated/windows/Agent/windowsservercore/1803/Dockerfile" -t teamcity-agent:latest-windowsservercore-1803 "context"
docker build -f "generated/windows/Server/nanoserver/1803/Dockerfile" -t teamcity-server:latest-nanoserver-1803 "context"
docker build -f "generated/windows/Agent/nanoserver/1803/Dockerfile" -t teamcity-agent:latest-nanoserver-1803 "context"
```

Base images:
- [mcr.microsoft.com/powershell:nanoserver-1803](teamcity-server.md#latest-nanoserver-1803)
- [mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1803](teamcity-agent.md#latest-windowsservercore-1803)
- [teamcity-minimal-agent:latest-nanoserver-1803](teamcity-agent.md#latest-windowsservercore-1803)
- [mcr.microsoft.com/powershell:nanoserver-1803](teamcity-server.md#latest-nanoserver-1803)
- [teamcity-agent:latest-windowsservercore-1803](teamcity-agent.md#latest-nanoserver-1803)

### latest-windowsservercore-1809

[Dockerfile](windows/Agent/windowsservercore/1809/Dockerfile)

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](https://github.com/git-for-windows/git/releases/download/v2.19.1.windows.1/MinGit-2.19.1-64-bit.zip)
- [Mercurial x64 v.4.7.2](https://bitbucket.org/tortoisehg/files/downloads/mercurial-4.7.2-x64.msi)

Docker build commands:
```
docker build -f "generated/windows/Server/nanoserver/1809/Dockerfile" -t teamcity-server:latest-nanoserver-1809 "context"
docker build -f "generated/windows/Agent/windowsservercore/1809/Dockerfile" -t teamcity-agent:latest-windowsservercore-1809 "context"
docker build -f "generated/windows/Agent/windowsservercore/1809/Dockerfile" -t teamcity-agent:latest-windowsservercore-1809 "context"
```

Base images:
- [mcr.microsoft.com/powershell:nanoserver-1809](teamcity-server.md#latest-nanoserver-1809)
- [mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019](teamcity-agent.md#latest-windowsservercore-1809)
- [teamcity-minimal-agent:latest-nanoserver-1809](teamcity-agent.md#latest-windowsservercore-1809)

### latest-nanoserver-1809

[Dockerfile](windows/Agent/nanoserver/1809/Dockerfile)

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:
- [.NET Core SDK x64 v.3.1.100](https://dotnetcli.blob.core.windows.net/dotnet/Sdk/3.1.100/dotnet-sdk-3.1.100-win-x64.zip)

Docker build commands:
```
docker build -f "generated/windows/Server/nanoserver/1809/Dockerfile" -t teamcity-server:latest-nanoserver-1809 "context"
docker build -f "generated/windows/Agent/windowsservercore/1809/Dockerfile" -t teamcity-agent:latest-windowsservercore-1809 "context"
docker build -f "generated/windows/Agent/windowsservercore/1809/Dockerfile" -t teamcity-agent:latest-windowsservercore-1809 "context"
docker build -f "generated/windows/Server/nanoserver/1809/Dockerfile" -t teamcity-server:latest-nanoserver-1809 "context"
docker build -f "generated/windows/Agent/nanoserver/1809/Dockerfile" -t teamcity-agent:latest-nanoserver-1809 "context"
```

Base images:
- [mcr.microsoft.com/powershell:nanoserver-1809](teamcity-server.md#latest-nanoserver-1809)
- [mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019](teamcity-agent.md#latest-windowsservercore-1809)
- [teamcity-minimal-agent:latest-nanoserver-1809](teamcity-agent.md#latest-windowsservercore-1809)
- [mcr.microsoft.com/powershell:nanoserver-1809](teamcity-server.md#latest-nanoserver-1809)
- [teamcity-agent:latest-windowsservercore-1809](teamcity-agent.md#latest-nanoserver-1809)

### latest-windowsservercore-1903

[Dockerfile](windows/Agent/windowsservercore/1903/Dockerfile)

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](https://github.com/git-for-windows/git/releases/download/v2.19.1.windows.1/MinGit-2.19.1-64-bit.zip)
- [Mercurial x64 v.4.7.2](https://bitbucket.org/tortoisehg/files/downloads/mercurial-4.7.2-x64.msi)

Docker build commands:
```
docker build -f "generated/windows/Server/nanoserver/1903/Dockerfile" -t teamcity-server:latest-nanoserver-1903 "context"
docker build -f "generated/windows/Agent/windowsservercore/1903/Dockerfile" -t teamcity-agent:latest-windowsservercore-1903 "context"
docker build -f "generated/windows/Agent/windowsservercore/1903/Dockerfile" -t teamcity-agent:latest-windowsservercore-1903 "context"
```

Base images:
- [mcr.microsoft.com/powershell:nanoserver-1903](teamcity-server.md#latest-nanoserver-1903)
- [mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1903](teamcity-agent.md#latest-windowsservercore-1903)
- [teamcity-minimal-agent:latest-nanoserver-1903](teamcity-agent.md#latest-windowsservercore-1903)

### latest-nanoserver-1903

[Dockerfile](windows/Agent/nanoserver/1903/Dockerfile)

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:
- [.NET Core SDK x64 v.3.1.100](https://dotnetcli.blob.core.windows.net/dotnet/Sdk/3.1.100/dotnet-sdk-3.1.100-win-x64.zip)

Docker build commands:
```
docker build -f "generated/windows/Server/nanoserver/1903/Dockerfile" -t teamcity-server:latest-nanoserver-1903 "context"
docker build -f "generated/windows/Agent/windowsservercore/1903/Dockerfile" -t teamcity-agent:latest-windowsservercore-1903 "context"
docker build -f "generated/windows/Agent/windowsservercore/1903/Dockerfile" -t teamcity-agent:latest-windowsservercore-1903 "context"
docker build -f "generated/windows/Server/nanoserver/1903/Dockerfile" -t teamcity-server:latest-nanoserver-1903 "context"
docker build -f "generated/windows/Agent/nanoserver/1903/Dockerfile" -t teamcity-agent:latest-nanoserver-1903 "context"
```

Base images:
- [mcr.microsoft.com/powershell:nanoserver-1903](teamcity-server.md#latest-nanoserver-1903)
- [mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1903](teamcity-agent.md#latest-windowsservercore-1903)
- [teamcity-minimal-agent:latest-nanoserver-1903](teamcity-agent.md#latest-windowsservercore-1903)
- [mcr.microsoft.com/powershell:nanoserver-1903](teamcity-server.md#latest-nanoserver-1903)
- [teamcity-agent:latest-windowsservercore-1903](teamcity-agent.md#latest-nanoserver-1903)

