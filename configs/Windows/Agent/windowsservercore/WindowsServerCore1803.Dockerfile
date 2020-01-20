# Priority 2
# Id teamcity-agent
# Tag ${tag}

# Based on ${windowsservercoreImage}
FROM ${windowsservercoreImage} AS tools

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Install JDK Amazon Corretto v.8.232.09.1
RUN [Net.ServicePointManager]::SecurityProtocol = 'tls12, tls11, tls' ; \
    Invoke-WebRequest https://d3pxv6yz143wms.cloudfront.net/8.232.09.1/amazon-corretto-8.232.09.1-windows-x64-jdk.zip -OutFile jdk.zip; \
    Expand-Archive jdk.zip -DestinationPath $Env:ProgramFiles\Java ; \
    Get-ChildItem $Env:ProgramFiles\Java | Rename-Item -NewName "OpenJDK" ; \
    Remove-Item $Env:ProgramFiles\Java\OpenJDK\demo -Force -Recurse ; \
    Remove-Item $Env:ProgramFiles\Java\OpenJDK\sample -Force -Recurse ; \
    Remove-Item $Env:ProgramFiles\Java\OpenJDK\src.zip -Force ; \
    Remove-Item -Force jdk.zip

# Install Git v.2.19.1
RUN [Net.ServicePointManager]::SecurityProtocol = 'tls12, tls11, tls' ; \
    Invoke-WebRequest https://github.com/git-for-windows/git/releases/download/v2.19.1.windows.1/MinGit-2.19.1-64-bit.zip -OutFile git.zip; \
    Expand-Archive git.zip -DestinationPath $Env:ProgramFiles\Git ; \
    Remove-Item -Force git.zip

# Install Mercurial v.4.7.2
RUN [Net.ServicePointManager]::SecurityProtocol = 'tls12, tls11, tls' ; \
    Invoke-WebRequest https://bitbucket.org/tortoisehg/files/downloads/mercurial-4.7.2-x64.msi -OutFile hg.msi; \
    Start-Process msiexec -Wait -ArgumentList /q, /i, hg.msi ; \
    Remove-Item -Force hg.msi

# Based on ${teamcityMinimalAgentImage}
FROM ${teamcityMinimalAgentImage} AS buildagent

FROM ${windowsservercoreImage}

COPY --from=tools ["C:/Program Files/Java/OpenJDK", "C:/Program Files/Java/OpenJDK"]
COPY --from=tools ["C:/Program Files/Git", "C:/Program Files/Git"]
COPY --from=tools ["C:/Program Files/Mercurial", "C:/Program Files/Mercurial"]
COPY --from=buildagent /BuildAgent /BuildAgent

EXPOSE 9090

VOLUME C:/BuildAgent/conf

CMD ./BuildAgent/run-agent.ps1

    # Configuration file for TeamCity agent
ENV CONFIG_FILE="C:/BuildAgent/conf/buildAgent.properties" \
    # Java home directory
    JAVA_HOME="C:\Program Files\Java\OpenJDK" \
    # Opt out of the telemetry feature
    DOTNET_CLI_TELEMETRY_OPTOUT=true \
    # Disable first time experience
    DOTNET_SKIP_FIRST_TIME_EXPERIENCE=true \
    # Configure Kestrel web server to bind to port 80 when present
    ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Enable correct mode for dotnet watch (only mode supported in a container)
    DOTNET_USE_POLLING_FILE_WATCHER=true \
    # Skip extraction of XML docs - generally not useful within an image/container - helps perfomance
    NUGET_XMLDOC_MODE=skip

RUN setx /M PATH ('{0};{1}\bin;C:\Program Files\Git\cmd;C:\Program Files\Mercurial' -f $env:PATH, $env:JAVA_HOME)