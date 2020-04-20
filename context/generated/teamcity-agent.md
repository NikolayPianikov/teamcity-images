### Tags

- [18.04 or linux](#whale-1804-or-linux)
- [latest-nanoserver-1809](#whale-latest-nanoserver-1809)
- [latest-nanoserver-1903](#whale-latest-nanoserver-1903)
- [latest-windowsservercore-1809](#whale-latest-windowsservercore-1809)
- [latest-windowsservercore-1903](#whale-latest-windowsservercore-1903)
- [latest-nanoserver-1803](#whale-latest-nanoserver-1803)
- [latest-windowsservercore-1803](#whale-latest-windowsservercore-1803)

### :whale: 18.04 or linux

[Dockerfile](linux/Agent/Ubuntu/18.04/Dockerfile)

<img align="center" height="64" src="/logo/ubuntu.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) build agent image.

The docker image is available on:

- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:

- Git
- Mercurial
- [<img align="center" height="18" src="/logo/dotnetcore.png"> SDK x64 v.3.1.100](https://dotnetcli.blob.core.windows.net/dotnet/Sdk/3.1.100/dotnet-sdk-3.1.100-linux-x64.tar.gz)

Container Platform: linux

Build commands:

```
docker build -f "context/generated/linux/MinimalAgent/Ubuntu/18.04/Dockerfile" -t teamcity-minimal-agent:18.04 -t teamcity-minimal-agent:linux "context"
docker build -f "context/generated/linux/Agent/Ubuntu/18.04/Dockerfile" -t teamcity-agent:18.04 -t teamcity-agent:linux "context"
```

Base images:

```
docker pull ubuntu:18.04
```

_The required free space to generate image(s) is about **2 GB**._
### :whale: latest-nanoserver-1809

[Dockerfile](windows/Agent/nanoserver/1809/Dockerfile)

<img align="center" height="64" src="/logo/windows_nano.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) build agent image.

The docker image is available on:

- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:

- [.NET Core SDK x64 v.3.1.100](https://dotnetcli.blob.core.windows.net/dotnet/Sdk/3.1.100/dotnet-sdk-3.1.100-win-x64.zip)
- [<img src="/logo/powershell.png" height="18" align="center"> PowerShell](https://github.com/PowerShell/PowerShell#get-powershell)

Container Platform: windows

Build commands:

```
docker build -f "context/generated/windows/MinimalAgent/nanoserver/1809/Dockerfile" -t teamcity-minimal-agent:latest-nanoserver-1809 "context"
docker build -f "context/generated/windows/Agent/windowsservercore/1809/Dockerfile" -t teamcity-agent:latest-windowsservercore-1809 "context"
docker build -f "context/generated/windows/Agent/nanoserver/1809/Dockerfile" -t teamcity-agent:latest-nanoserver-1809 "context"
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

<img align="center" height="64" src="/logo/windows_nano.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) build agent image.

The docker image is available on:

- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:

- [.NET Core SDK x64 v.3.1.100](https://dotnetcli.blob.core.windows.net/dotnet/Sdk/3.1.100/dotnet-sdk-3.1.100-win-x64.zip)
- [<img src="/logo/powershell.png" height="18" align="center"> PowerShell](https://github.com/PowerShell/PowerShell#get-powershell)

Container Platform: windows

Build commands:

```
docker build -f "context/generated/windows/MinimalAgent/nanoserver/1903/Dockerfile" -t teamcity-minimal-agent:latest-nanoserver-1903 "context"
docker build -f "context/generated/windows/Agent/windowsservercore/1903/Dockerfile" -t teamcity-agent:latest-windowsservercore-1903 "context"
docker build -f "context/generated/windows/Agent/nanoserver/1903/Dockerfile" -t teamcity-agent:latest-nanoserver-1903 "context"
```

Base images:

```
docker pull mcr.microsoft.com/windows/nanoserver:1903
docker pull mcr.microsoft.com/powershell:nanoserver-1903
docker pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1903
```

_The required free space to generate image(s) is about **25 GB**._
### :whale: latest-windowsservercore-1809

[Dockerfile](windows/Agent/windowsservercore/1809/Dockerfile)

<img align="center" height="64" src="/logo/windows.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) build agent image.

The docker image is available on:

- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:

- [<img src="/logo/powershell.png" height="18" align="center"> PowerShell](https://github.com/PowerShell/PowerShell#get-powershell)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://repo.labs.intellij.net/cache/https/d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](http://repo.labs.intellij.net/thirdparty/vm-templates/MinGit-2.19.1-64-bit.zip)
- [Mercurial x64 v.4.7.2](http://repo.labs.intellij.net/thirdparty/vm-templates/mercurial-4.7.2-x64.msi)

Container Platform: windows

Build commands:

```
docker build -f "context/generated/windows/MinimalAgent/nanoserver/1809/Dockerfile" -t teamcity-minimal-agent:latest-nanoserver-1809 "context"
docker build -f "context/generated/windows/Agent/windowsservercore/1809/Dockerfile" -t teamcity-agent:latest-windowsservercore-1809 "context"
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

<img align="center" height="64" src="/logo/windows.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) build agent image.

The docker image is available on:

- [https://hub.docker.com/r/jetbrains/teamcity-agent](https://hub.docker.com/r/jetbrains/teamcity-agent)

Installed components:

- [<img src="/logo/powershell.png" height="18" align="center"> PowerShell](https://github.com/PowerShell/PowerShell#get-powershell)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://repo.labs.intellij.net/cache/https/d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](http://repo.labs.intellij.net/thirdparty/vm-templates/MinGit-2.19.1-64-bit.zip)
- [Mercurial x64 v.4.7.2](http://repo.labs.intellij.net/thirdparty/vm-templates/mercurial-4.7.2-x64.msi)

Container Platform: windows

Build commands:

```
docker build -f "context/generated/windows/MinimalAgent/nanoserver/1903/Dockerfile" -t teamcity-minimal-agent:latest-nanoserver-1903 "context"
docker build -f "context/generated/windows/Agent/windowsservercore/1903/Dockerfile" -t teamcity-agent:latest-windowsservercore-1903 "context"
```

Base images:

```
docker pull mcr.microsoft.com/windows/nanoserver:1903
docker pull mcr.microsoft.com/powershell:nanoserver-1903
docker pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1903
```

_The required free space to generate image(s) is about **24 GB**._
### :whale: latest-nanoserver-1803

[Dockerfile](windows/Agent/nanoserver/1803/Dockerfile)

<img align="center" height="64" src="/logo/windows_nano.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) build agent image.
The docker image is not available and may be created manually.

Installed components:

- [<img src="/logo/powershell.png" height="18" align="center"> PowerShell](https://github.com/PowerShell/PowerShell#get-powershell)
- [.NET Core SDK x64 v.3.1.100](https://dotnetcli.blob.core.windows.net/dotnet/Sdk/3.1.100/dotnet-sdk-3.1.100-win-x64.zip)

Container Platform: windows

Build commands:

```
docker build -f "context/generated/windows/MinimalAgent/nanoserver/1803/Dockerfile" -t teamcity-minimal-agent:latest-nanoserver-1803 "context"
docker build -f "context/generated/windows/Agent/windowsservercore/1803/Dockerfile" -t teamcity-agent:latest-windowsservercore-1803 "context"
docker build -f "context/generated/windows/Agent/nanoserver/1803/Dockerfile" -t teamcity-agent:latest-nanoserver-1803 "context"
```

Base images:

```
docker pull mcr.microsoft.com/powershell:nanoserver-1803
docker pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1803
```

_The required free space to generate image(s) is about **24 GB**._
### :whale: latest-windowsservercore-1803

[Dockerfile](windows/Agent/windowsservercore/1803/Dockerfile)

<img align="center" height="64" src="/logo/windows.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) build agent image.
The docker image is not available and may be created manually.

Installed components:

- [<img src="/logo/powershell.png" height="18" align="center"> PowerShell](https://github.com/PowerShell/PowerShell#get-powershell)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://repo.labs.intellij.net/cache/https/d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](http://repo.labs.intellij.net/thirdparty/vm-templates/MinGit-2.19.1-64-bit.zip)
- [Mercurial x64 v.4.7.2](http://repo.labs.intellij.net/thirdparty/vm-templates/mercurial-4.7.2-x64.msi)

Container Platform: windows

Build commands:

```
docker build -f "context/generated/windows/MinimalAgent/nanoserver/1803/Dockerfile" -t teamcity-minimal-agent:latest-nanoserver-1803 "context"
docker build -f "context/generated/windows/Agent/windowsservercore/1803/Dockerfile" -t teamcity-agent:latest-windowsservercore-1803 "context"
```

Base images:

```
docker pull mcr.microsoft.com/powershell:nanoserver-1803
docker pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1803
```

_The required free space to generate image(s) is about **23 GB**._
