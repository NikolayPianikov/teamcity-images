### Tags

- [18.04 or linux](#whale-1804-or-linux)
- [latest-nanoserver-1809](#whale-latest-nanoserver-1809)
- [latest-nanoserver-1903](#whale-latest-nanoserver-1903)
- [latest-nanoserver-1803](#whale-latest-nanoserver-1803)

### :whale: 18.04 or linux

[Dockerfile](linux/Server/Ubuntu/18.04/Dockerfile)

<img align="center" height="64" src="/logo/ubuntu.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) server image. The image is suitable for production use and evaluation purposes.

The docker image is available on:

- [https://hub.docker.com/r/jetbrains/teamcity-server](https://hub.docker.com/r/jetbrains/teamcity-server)

Installed components:

- [<img align="center" height="18" src="/logo/corretto.png"> JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-linux-x64.tar.gz)

Container Platform: linux

Build commands:

```
docker build -f "context/generated/linux/Server/Ubuntu/18.04/Dockerfile" -t 18.04 -t linux "context"
```

Base images:

```
docker pull ubuntu:18.04
```

_The required free space to generate image(s) is about **1 GB**._
### :whale: latest-nanoserver-1809

[Dockerfile](windows/Server/nanoserver/1809/Dockerfile)

<img align="center" height="64" src="/logo/windows_nano.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) server image. The image is suitable for production use and evaluation purposes.

The docker image is available on:

- [https://hub.docker.com/r/jetbrains/teamcity-server](https://hub.docker.com/r/jetbrains/teamcity-server)

Installed components:

- [<img src="/logo/powershell.png" height="18" align="center"> PowerShell](https://github.com/PowerShell/PowerShell#get-powershell)
- [JRE Amazon Corretto x64 v.8.232.09.1](https://repo.labs.intellij.net/cache/https/d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://repo.labs.intellij.net/cache/https/d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](http://repo.labs.intellij.net/thirdparty/vm-templates/MinGit-2.19.1-64-bit.zip)

Container Platform: windows

Build commands:

```
docker build -f "context/generated/windows/Server/nanoserver/1809/Dockerfile" -t latest-nanoserver-1809 "context"
```

Base images:

```
docker pull mcr.microsoft.com/powershell:nanoserver-1809
```

_The required free space to generate image(s) is about **3 GB**._
### :whale: latest-nanoserver-1903

[Dockerfile](windows/Server/nanoserver/1903/Dockerfile)

<img align="center" height="64" src="/logo/windows_nano.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) server image. The image is suitable for production use and evaluation purposes.

The docker image is available on:

- [https://hub.docker.com/r/jetbrains/teamcity-server](https://hub.docker.com/r/jetbrains/teamcity-server)

Installed components:

- [<img src="/logo/powershell.png" height="18" align="center"> PowerShell](https://github.com/PowerShell/PowerShell#get-powershell)
- [JRE Amazon Corretto x64 v.8.232.09.1](https://repo.labs.intellij.net/cache/https/d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://repo.labs.intellij.net/cache/https/d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](http://repo.labs.intellij.net/thirdparty/vm-templates/MinGit-2.19.1-64-bit.zip)

Container Platform: windows

Build commands:

```
docker build -f "context/generated/windows/Server/nanoserver/1903/Dockerfile" -t latest-nanoserver-1903 "context"
```

Base images:

```
docker pull mcr.microsoft.com/powershell:nanoserver-1903
```

_The required free space to generate image(s) is about **3 GB**._
### :whale: latest-nanoserver-1803

[Dockerfile](windows/Server/nanoserver/1803/Dockerfile)

<img align="center" height="64" src="/logo/windows_nano.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) server image. The image is suitable for production use and evaluation purposes.
The docker image is not available and may be created manually.

Installed components:

- [<img src="/logo/powershell.png" height="18" align="center"> PowerShell](https://github.com/PowerShell/PowerShell#get-powershell)
- [JRE Amazon Corretto x64 v.8.232.09.1](https://repo.labs.intellij.net/cache/https/d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://repo.labs.intellij.net/cache/https/d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](http://repo.labs.intellij.net/thirdparty/vm-templates/MinGit-2.19.1-64-bit.zip)

Container Platform: windows

Build commands:

```
docker build -f "context/generated/windows/Server/nanoserver/1803/Dockerfile" -t latest-nanoserver-1803 "context"
```

Base images:

```
docker pull mcr.microsoft.com/powershell:nanoserver-1803
```

_The required free space to generate image(s) is about **3 GB**._
