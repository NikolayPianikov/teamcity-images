### 18.04, linux

Dockerfile: [Dockerfile](linux/Server/Ubuntu/18.04/Dockerfile)

Docker build commands:
```
docker build -f "generated/linux/Server/Ubuntu/18.04/Dockerfile" -t teamcity-server:18.04 -t teamcity-server:linux "context"
```

Installed components:
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-linux-x64.tar.gz)

Base images:
- ubuntu:18.04

### latest-nanoserver-1803

Dockerfile: [Dockerfile](windows/Server/nanoserver/1803/Dockerfile)

Docker build commands:
```
docker build -f "generated/windows/Server/nanoserver/1803/Dockerfile" -t teamcity-server:latest-nanoserver-1803 "context"
```

Installed components:
- PowerShell
- [JRE Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](https://github.com/git-for-windows/git/releases/download/v2.19.1.windows.1/MinGit-2.19.1-64-bit.zip)

Base images:
- mcr.microsoft.com/powershell:nanoserver-1803

### latest-nanoserver-1809

Dockerfile: [Dockerfile](windows/Server/nanoserver/1809/Dockerfile)

Docker build commands:
```
docker build -f "generated/windows/Server/nanoserver/1809/Dockerfile" -t teamcity-server:latest-nanoserver-1809 "context"
```

Installed components:
- PowerShell
- [JRE Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](https://github.com/git-for-windows/git/releases/download/v2.19.1.windows.1/MinGit-2.19.1-64-bit.zip)

Base images:
- mcr.microsoft.com/powershell:nanoserver-1809

### latest-nanoserver-1903

Dockerfile: [Dockerfile](windows/Server/nanoserver/1903/Dockerfile)

Docker build commands:
```
docker build -f "generated/windows/Server/nanoserver/1903/Dockerfile" -t teamcity-server:latest-nanoserver-1903 "context"
```

Installed components:
- PowerShell
- [JRE Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jre.zip)
- [JDK Amazon Corretto x64 v.8.232.09.1](https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip)
- [Git x64 v.2.19.1](https://github.com/git-for-windows/git/releases/download/v2.19.1.windows.1/MinGit-2.19.1-64-bit.zip)

Base images:
- mcr.microsoft.com/powershell:nanoserver-1903

