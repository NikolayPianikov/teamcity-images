| Tags | Base images | Components | Dockerfile |
| ---- | ----------- | ---------- | ---------- |
|18.04:linux|teamcity-minimal-agent:18.04|.NET Core SDK v.3.1.100, Git, Mercurial|Unix/Agent/Ubuntu/18.04/Dockerfile|
|latest-nanoserver-1803|mcr.microsoft.com/powershell:nanoserver-1803, teamcity-agent:latest-windowsservercore-1803|.NET Core SDK v.3.1.100|Windows/Agent/nanoserver/1803/Dockerfile|
|latest-nanoserver-1809|mcr.microsoft.com/powershell:nanoserver-1809, teamcity-agent:latest-windowsservercore-1809|.NET Core SDK v.3.1.100|Windows/Agent/nanoserver/1809/Dockerfile|
|latest-nanoserver-1903|mcr.microsoft.com/powershell:nanoserver-1903, teamcity-agent:latest-windowsservercore-1903|.NET Core SDK v.3.1.100|Windows/Agent/nanoserver/1903/Dockerfile|
|latest-windowsservercore-1803|mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1803, teamcity-minimal-agent:latest-nanoserver-1803|JDK Amazon Corretto v.8.232.09.1, Git v.2.19.1, Mercurial v.4.7.2|Windows/Agent/windowsservercore/1803/Dockerfile|
|latest-windowsservercore-1809|mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019, teamcity-minimal-agent:latest-nanoserver-1809|JDK Amazon Corretto v.8.232.09.1, Git v.2.19.1, Mercurial v.4.7.2|Windows/Agent/windowsservercore/1809/Dockerfile|
|latest-windowsservercore-1903|mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1903, teamcity-minimal-agent:latest-nanoserver-1903|JDK Amazon Corretto v.8.232.09.1, Git v.2.19.1, Mercurial v.4.7.2|Windows/Agent/windowsservercore/1903/Dockerfile|
