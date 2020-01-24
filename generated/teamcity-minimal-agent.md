### Tags
- [18.04, linux](#1804-linux)
- [latest-nanoserver-1803](#latest-nanoserver-1803)
- [latest-nanoserver-1809](#latest-nanoserver-1809)
- [latest-nanoserver-1903](#latest-nanoserver-1903)

### 18.04, linux

[Dockerfile](linux/MinimalAgent/Ubuntu/18.04/Dockerfile)

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-minimal-agent](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)

Docker build commands:
```
docker build -f "generated/linux/MinimalAgent/Ubuntu/18.04/Dockerfile" -t teamcity-minimal-agent:18.04 -t teamcity-minimal-agent:linux "context"
```

Installed components:
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-linux-x64.tar.gz)

Base images:
- ubuntu:18.04

### latest-nanoserver-1803

[Dockerfile](windows/MinimalAgent/nanoserver/1803/Dockerfile)

Docker build commands:
```
docker build -f "generated/windows/MinimalAgent/nanoserver/1803/Dockerfile" -t teamcity-minimal-agent:latest-nanoserver-1803 "context"
```

Installed components:
- PowerShell
- [JRE Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)

Base images:
- mcr.microsoft.com/powershell:nanoserver-1803

### latest-nanoserver-1809

[Dockerfile](windows/MinimalAgent/nanoserver/1809/Dockerfile)

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-minimal-agent](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)

Docker build commands:
```
docker build -f "generated/windows/MinimalAgent/nanoserver/1809/Dockerfile" -t teamcity-minimal-agent:latest-nanoserver-1809 "context"
```

Installed components:
- [JRE Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- PowerShell

Base images:
- mcr.microsoft.com/powershell:nanoserver-1809

### latest-nanoserver-1903

[Dockerfile](windows/MinimalAgent/nanoserver/1903/Dockerfile)

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-minimal-agent](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)

Docker build commands:
```
docker build -f "generated/windows/MinimalAgent/nanoserver/1903/Dockerfile" -t teamcity-minimal-agent:latest-nanoserver-1903 "context"
```

Installed components:
- [JRE Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- PowerShell

Base images:
- mcr.microsoft.com/powershell:nanoserver-1903

