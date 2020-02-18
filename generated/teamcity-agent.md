### Tags
- [18.04, linux](#whale-1804-linux)
- [latest-nanoserver-1803](#whale-latest-nanoserver-1803)
- [latest-nanoserver-1809](#whale-latest-nanoserver-1809)
- [latest-nanoserver-1903](#whale-latest-nanoserver-1903)
- [latest-windowsservercore-1803](#whale-latest-windowsservercore-1803)
- [latest-windowsservercore-1809](#whale-latest-windowsservercore-1809)
- [latest-windowsservercore-1903](#whale-latest-windowsservercore-1903)

### :whale: 18.04, linux

[Dockerfile](linux/Agent/Ubuntu/18.04/Dockerfile)

Platform: 

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:
- Git
- Mercurial
- [.NET Core SDK x64 v.3.1.100](https://dotnetcli.blob.core.windows.net/dotnet/Sdk/3.1.100/dotnet-sdk-3.1.100-linux-x64.tar.gz)

Docker commands:
```
docker build -f "generated/linux/MinimalAgent/Ubuntu/18.04" -t 18.04 -t linux "context"
docker build -f "generated/linux/Agent/Ubuntu/18.04" -t 18.04 -t linux "context"
```
Base images:
```
docker pull ubuntu:18.04
```
_The required free space to generate image(s) is about **2 GB**._

### :whale: latest-nanoserver-1803

[Dockerfile](windows/Agent/nanoserver/1803/Dockerfile)

Platform: 

Installed components:
- [.NET Core SDK x64 v.3.1.100](https://dotnetcli.blob.core.windows.net/dotnet/Sdk/3.1.100/dotnet-sdk-3.1.100-win-x64.zip)

Docker commands:
```
docker build -f "generated/windows/MinimalAgent/nanoserver/1803" -t latest-nanoserver-1803 "context"
docker build -f "generated/windows/Agent/windowsservercore/1803" -t latest-windowsservercore-1803 "context"
docker build -f "generated/windows/Agent/nanoserver/1803" -t latest-nanoserver-1803 "context"
```
Base images:
```
docker pull mcr.microsoft.com/powershell:nanoserver-1803
docker pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1803
```
_The required free space to generate image(s) is about **24 GB**._

### :whale: latest-nanoserver-1809

[Dockerfile](windows/Agent/nanoserver/1809/Dockerfile)

Platform: 

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:
- [.NET Core SDK x64 v.3.1.100](https://dotnetcli.blob.core.windows.net/dotnet/Sdk/3.1.100/dotnet-sdk-3.1.100-win-x64.zip)

Docker commands:
```
docker build -f "generated/windows/MinimalAgent/nanoserver/1809" -t latest-nanoserver-1809 "context"
docker build -f "generated/windows/Agent/windowsservercore/1809" -t latest-windowsservercore-1809 "context"
docker build -f "generated/windows/Agent/nanoserver/1809" -t latest-nanoserver-1809 "context"
```
Base images:
```
docker pull mcr.microsoft.com/windows/nanoserver:1809
docker pull mcr.microsoft.com/powershell:nanoserver-1809
docker pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019
```
_The required free space to generate image(s) is about **25 GB**._

### :whale: latest-nanoserver-1903

[Dockerfile](windows/Agent/nanoserver/1903/Dockerfile)

Platform: 

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:
- [.NET Core SDK x64 v.3.1.100](https://dotnetcli.blob.core.windows.net/dotnet/Sdk/3.1.100/dotnet-sdk-3.1.100-win-x64.zip)

Docker commands:
```
docker build -f "generated/windows/MinimalAgent/nanoserver/1903" -t latest-nanoserver-1903 "context"
docker build -f "generated/windows/Agent/windowsservercore/1903" -t latest-windowsservercore-1903 "context"
docker build -f "generated/windows/Agent/nanoserver/1903" -t latest-nanoserver-1903 "context"
```
Base images:
```
docker pull mcr.microsoft.com/windows/nanoserver:1903
docker pull mcr.microsoft.com/powershell:nanoserver-1903
docker pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1903
```
_The required free space to generate image(s) is about **25 GB**._

### :whale: latest-windowsservercore-1803

[Dockerfile](windows/Agent/windowsservercore/1803/Dockerfile)

Platform: 

Installed components:
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](https://github.com/git-for-windows/git/releases/download/v2.19.1.windows.1/MinGit-2.19.1-64-bit.zip)
- [Mercurial x64 v.4.7.2](https://bitbucket.org/tortoisehg/files/downloads/mercurial-4.7.2-x64.msi)

Docker commands:
```
docker build -f "generated/windows/MinimalAgent/nanoserver/1803" -t latest-nanoserver-1803 "context"
docker build -f "generated/windows/Agent/windowsservercore/1803" -t latest-windowsservercore-1803 "context"
```
Base images:
```
docker pull mcr.microsoft.com/powershell:nanoserver-1803
docker pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1803
```
_The required free space to generate image(s) is about **23 GB**._

### :whale: latest-windowsservercore-1809

[Dockerfile](windows/Agent/windowsservercore/1809/Dockerfile)

Platform: 

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](https://github.com/git-for-windows/git/releases/download/v2.19.1.windows.1/MinGit-2.19.1-64-bit.zip)
- [Mercurial x64 v.4.7.2](https://bitbucket.org/tortoisehg/files/downloads/mercurial-4.7.2-x64.msi)

Docker commands:
```
docker build -f "generated/windows/MinimalAgent/nanoserver/1809" -t latest-nanoserver-1809 "context"
docker build -f "generated/windows/Agent/windowsservercore/1809" -t latest-windowsservercore-1809 "context"
```
Base images:
```
docker pull mcr.microsoft.com/windows/nanoserver:1809
docker pull mcr.microsoft.com/powershell:nanoserver-1809
docker pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019
```
_The required free space to generate image(s) is about **24 GB**._

### :whale: latest-windowsservercore-1903

[Dockerfile](windows/Agent/windowsservercore/1903/Dockerfile)

Platform: 

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](https://github.com/git-for-windows/git/releases/download/v2.19.1.windows.1/MinGit-2.19.1-64-bit.zip)
- [Mercurial x64 v.4.7.2](https://bitbucket.org/tortoisehg/files/downloads/mercurial-4.7.2-x64.msi)

Docker commands:
```
docker build -f "generated/windows/MinimalAgent/nanoserver/1903" -t latest-nanoserver-1903 "context"
docker build -f "generated/windows/Agent/windowsservercore/1903" -t latest-windowsservercore-1903 "context"
```
Base images:
```
docker pull mcr.microsoft.com/windows/nanoserver:1903
docker pull mcr.microsoft.com/powershell:nanoserver-1903
docker pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1903
```
_The required free space to generate image(s) is about **24 GB**._

