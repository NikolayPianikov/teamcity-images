### Tags
- [18.04, linux](#1804-linux)
- [latest-nanoserver-1803](#latest-nanoserver-1803)
- [latest-nanoserver-1809](#latest-nanoserver-1809)
- [latest-nanoserver-1903](#latest-nanoserver-1903)

### 18.04, linux

[Dockerfile](linux/MinimalAgent/Ubuntu/18.04/Dockerfile)

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-minimal-agent](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)

Installed components:
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-linux-x64.tar.gz)

Docker build commands:
```
docker build -f "generated/linux/MinimalAgent/Ubuntu/18.04/Dockerfile" -t teamcity-minimal-agent:18.04 -t teamcity-minimal-agent:linux "context"
```

Base images:
- [Build teamcity-minimal-agent:18.04](teamcity-minimal-agent.md#1804-linux)


Docker build commands:
```
docker build -f "generated/linux/MinimalAgent/Ubuntu/18.04/Dockerfile" -t teamcity-minimal-agent:18.04 -t teamcity-minimal-agent:linux "context"
```

Base images:
- [Build teamcity-minimal-agent:linux](teamcity-minimal-agent.md#1804-linux)

### latest-nanoserver-1803

[Dockerfile](windows/MinimalAgent/nanoserver/1803/Dockerfile)

Installed components:
- PowerShell
- [JRE Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)

Docker build commands:
```
docker build -f "generated/windows/MinimalAgent/nanoserver/1803/Dockerfile" -t teamcity-minimal-agent:latest-nanoserver-1803 "context"
```

Base images:
- [Build teamcity-minimal-agent:latest-nanoserver-1803](teamcity-minimal-agent.md#latest-nanoserver-1803)

### latest-nanoserver-1809

[Dockerfile](windows/MinimalAgent/nanoserver/1809/Dockerfile)

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-minimal-agent](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)

Installed components:
- [JRE Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- PowerShell

Docker build commands:
```
docker build -f "generated/windows/MinimalAgent/nanoserver/1809/Dockerfile" -t teamcity-minimal-agent:latest-nanoserver-1809 "context"
```

Base images:
- [Build teamcity-minimal-agent:latest-nanoserver-1809](teamcity-minimal-agent.md#latest-nanoserver-1809)

### latest-nanoserver-1903

[Dockerfile](windows/MinimalAgent/nanoserver/1903/Dockerfile)

The docker image is available on:
- [https://hub.docker.com/r/jetbrains/teamcity-minimal-agent](https://hub.docker.com/r/jetbrains/teamcity-minimal-agent)

Installed components:
- [JRE Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- PowerShell

Docker build commands:
```
docker build -f "generated/windows/MinimalAgent/nanoserver/1903/Dockerfile" -t teamcity-minimal-agent:latest-nanoserver-1903 "context"
```

Base images:
- [Build teamcity-minimal-agent:latest-nanoserver-1903](teamcity-minimal-agent.md#latest-nanoserver-1903)

