### Tags
- [18.04 or linux](#whale-whale-1804-or-linux)
- [latest-nanoserver-1803](#whale-whale-latest-nanoserver-1803)
- [latest-nanoserver-1809](#whale-whale-latest-nanoserver-1809)
- [latest-nanoserver-1903](#whale-whale-latest-nanoserver-1903)

### :whale: 18.04 or linux

[Dockerfile](linux/MinimalAgent/Ubuntu/18.04/Dockerfile)

<img align="center" height="64" src="/logo/ubuntu.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) build agent image.

The docker image is available on:

- [https://hub.docker.com/r/jetbrains/teamcity-minimal-agent](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)


Installed components:

- [<img align="center" height="18" src="/logo/corretto.png"> JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-linux-x64.tar.gz)

Container Platform: linux

Build commands:

```
docker build -f "generated/linux/MinimalAgent/Ubuntu/18.04" -t 18.04 -t linux "context"
```

Base images:

```
docker pull ubuntu:18.04
```

_The required free space to generate image(s) is about **1 GB**._
### :whale: latest-nanoserver-1803

[Dockerfile](windows/MinimalAgent/nanoserver/1803/Dockerfile)

<img align="center" height="64" src="/logo/windows_nano.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) build agent image.

The docker image is available on:

- [teamcity-minimal-agent](teamcity-minimal-agent)


Installed components:

- <img src="/logo/powershell.png" height="18" align="center"> [PowerShell](https://github.com/PowerShell/PowerShell#get-powershell)
- [<img align="center" height="18" src="/logo/corretto.png"> JRE Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [<img align="center" height="18" src="/logo/corretto.png"> JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)

Container Platform: windows

Build commands:

```
docker build -f "generated/windows/MinimalAgent/nanoserver/1803" -t latest-nanoserver-1803 "context"
```

Base images:

```
docker pull mcr.microsoft.com/powershell:nanoserver-1803
```

_The required free space to generate image(s) is about **2 GB**._
### :whale: latest-nanoserver-1809

[Dockerfile](windows/MinimalAgent/nanoserver/1809/Dockerfile)

<img align="center" height="64" src="/logo/windows_nano.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) build agent image.

The docker image is available on:

- [https://hub.docker.com/r/jetbrains/teamcity-minimal-agent](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)


Installed components:

- [<img align="center" height="18" src="/logo/corretto.png"> JRE Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [<img align="center" height="18" src="/logo/corretto.png"> JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- <img src="/logo/powershell.png" height="18" align="center"> [PowerShell](https://github.com/PowerShell/PowerShell#get-powershell)

Container Platform: windows

Build commands:

```
docker build -f "generated/windows/MinimalAgent/nanoserver/1809" -t latest-nanoserver-1809 "context"
```

Base images:

```
docker pull mcr.microsoft.com/windows/nanoserver:1809
docker pull mcr.microsoft.com/powershell:nanoserver-1809
```

_The required free space to generate image(s) is about **3 GB**._
### :whale: latest-nanoserver-1903

[Dockerfile](windows/MinimalAgent/nanoserver/1903/Dockerfile)

<img align="center" height="64" src="/logo/windows_nano.png">

This is an official [JetBrains TeamCity](https://www.jetbrains.com/teamcity/) build agent image.

The docker image is available on:

- [https://hub.docker.com/r/jetbrains/teamcity-minimal-agent](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)


Installed components:

- [<img align="center" height="18" src="/logo/corretto.png"> JRE Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [<img align="center" height="18" src="/logo/corretto.png"> JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- <img src="/logo/powershell.png" height="18" align="center"> [PowerShell](https://github.com/PowerShell/PowerShell#get-powershell)

Container Platform: windows

Build commands:

```
docker build -f "generated/windows/MinimalAgent/nanoserver/1903" -t latest-nanoserver-1903 "context"
```

Base images:

```
docker pull mcr.microsoft.com/windows/nanoserver:1903
docker pull mcr.microsoft.com/powershell:nanoserver-1903
```

_The required free space to generate image(s) is about **3 GB**._
