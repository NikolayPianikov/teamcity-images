import jetbrains.buildServer.configs.kotlin.v2019_2.*
import jetbrains.buildServer.configs.kotlin.v2019_2.vcs.GitVcsRoot
import jetbrains.buildServer.configs.kotlin.v2019_2.buildSteps.dockerCommand
import jetbrains.buildServer.configs.kotlin.v2019_2.buildFeatures.dockerSupport
import jetbrains.buildServer.configs.kotlin.v2019_2.buildFeatures.freeDiskSpace
version = "2019.2"

object TC2019_2_BuildDist_latest_nanoserver_1903 : BuildType({
name = "TC2019_2 latest-nanoserver-1903"
description  = "teamcity-server:latest-nanoserver-1903 teamcity-minimal-agent:latest-nanoserver-1903 teamcity-agent:latest-windowsservercore-1903:latest-nanoserver-1903"
vcs {root(RemoteTeamcityImages)}
steps {
dockerCommand {
name = "pull mcr.microsoft.com/powershell:nanoserver-1903"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/powershell:nanoserver-1903"
}
}

dockerCommand {
name = "pull mcr.microsoft.com/windows/nanoserver:1903"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/windows/nanoserver:1903"
}
}

dockerCommand {
name = "pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1903"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1903"
}
}

dockerCommand {
name = "build teamcity-server:latest-nanoserver-1903"
commandType = build {
source = file {
path = """context/generated/windows/Server/nanoserver/1903/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-server:latest-nanoserver-1903
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-minimal-agent:latest-nanoserver-1903"
commandType = build {
source = file {
path = """context/generated/windows/MinimalAgent/nanoserver/1903/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-minimal-agent:latest-nanoserver-1903
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-windowsservercore-1903"
commandType = build {
source = file {
path = """context/generated/windows/Agent/windowsservercore/1903/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-windowsservercore-1903
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-nanoserver-1903"
commandType = build {
source = file {
path = """context/generated/windows/Agent/nanoserver/1903/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-nanoserver-1903
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "change tag from teamcity-server:latest-nanoserver-1903 to latest-nanoserver-1903"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-server:latest-nanoserver-1903 %docker.pushRepository%teamcity-server:latest-nanoserver-1903"
}
}

dockerCommand {
name = "change tag from teamcity-minimal-agent:latest-nanoserver-1903 to latest-nanoserver-1903"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-minimal-agent:latest-nanoserver-1903 %docker.pushRepository%teamcity-minimal-agent:latest-nanoserver-1903"
}
}

dockerCommand {
name = "change tag from teamcity-agent:latest-windowsservercore-1903 to latest-windowsservercore-1903"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:latest-windowsservercore-1903 %docker.pushRepository%teamcity-agent:latest-windowsservercore-1903"
}
}

dockerCommand {
name = "change tag from teamcity-agent:latest-nanoserver-1903 to latest-nanoserver-1903"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:latest-nanoserver-1903 %docker.pushRepository%teamcity-agent:latest-nanoserver-1903"
}
}

dockerCommand {
name = "push teamcity-server:latest-nanoserver-1903"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-server:latest-nanoserver-1903
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-minimal-agent:latest-nanoserver-1903"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-minimal-agent:latest-nanoserver-1903
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:latest-windowsservercore-1903"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:latest-windowsservercore-1903
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:latest-nanoserver-1903"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:latest-nanoserver-1903
""".trimIndent()
}
}

}
features {
freeDiskSpace {
requiredSpace = "27gb"
failBuild = true
}
dockerSupport {
loginToRegistry = on {
dockerRegistryId = "PROJECT_EXT_2307"
}
}
swabra { }
}
dependencies {
dependency(AbsoluteId("TC2019_2_BuildDist")) {
snapshot { onDependencyFailure = FailureAction.IGNORE }
artifacts {
artifactRules = "TeamCity-*.tar.gz!/**=>context"
}
}
}
})

object TC_Trunk_BuildDist_latest_nanoserver_1903 : BuildType({
name = "TC_Trunk latest-nanoserver-1903"
description  = "teamcity-server:latest-nanoserver-1903 teamcity-minimal-agent:latest-nanoserver-1903 teamcity-agent:latest-windowsservercore-1903:latest-nanoserver-1903"
vcs {root(RemoteTeamcityImages)}
steps {
dockerCommand {
name = "pull mcr.microsoft.com/powershell:nanoserver-1903"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/powershell:nanoserver-1903"
}
}

dockerCommand {
name = "pull mcr.microsoft.com/windows/nanoserver:1903"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/windows/nanoserver:1903"
}
}

dockerCommand {
name = "pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1903"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1903"
}
}

dockerCommand {
name = "build teamcity-server:latest-nanoserver-1903"
commandType = build {
source = file {
path = """context/generated/windows/Server/nanoserver/1903/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-server:latest-nanoserver-1903
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-minimal-agent:latest-nanoserver-1903"
commandType = build {
source = file {
path = """context/generated/windows/MinimalAgent/nanoserver/1903/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-minimal-agent:latest-nanoserver-1903
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-windowsservercore-1903"
commandType = build {
source = file {
path = """context/generated/windows/Agent/windowsservercore/1903/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-windowsservercore-1903
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-nanoserver-1903"
commandType = build {
source = file {
path = """context/generated/windows/Agent/nanoserver/1903/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-nanoserver-1903
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "change tag from teamcity-server:latest-nanoserver-1903 to eap-latest-nanoserver-1903"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-server:latest-nanoserver-1903 %docker.pushRepository%teamcity-server:eap-latest-nanoserver-1903"
}
}

dockerCommand {
name = "change tag from teamcity-minimal-agent:latest-nanoserver-1903 to eap-latest-nanoserver-1903"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-minimal-agent:latest-nanoserver-1903 %docker.pushRepository%teamcity-minimal-agent:eap-latest-nanoserver-1903"
}
}

dockerCommand {
name = "change tag from teamcity-agent:latest-windowsservercore-1903 to eap-latest-windowsservercore-1903"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:latest-windowsservercore-1903 %docker.pushRepository%teamcity-agent:eap-latest-windowsservercore-1903"
}
}

dockerCommand {
name = "change tag from teamcity-agent:latest-nanoserver-1903 to eap-latest-nanoserver-1903"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:latest-nanoserver-1903 %docker.pushRepository%teamcity-agent:eap-latest-nanoserver-1903"
}
}

dockerCommand {
name = "push teamcity-server:eap-latest-nanoserver-1903"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-server:eap-latest-nanoserver-1903
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-minimal-agent:eap-latest-nanoserver-1903"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-minimal-agent:eap-latest-nanoserver-1903
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:eap-latest-windowsservercore-1903"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:eap-latest-windowsservercore-1903
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:eap-latest-nanoserver-1903"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:eap-latest-nanoserver-1903
""".trimIndent()
}
}

}
features {
freeDiskSpace {
requiredSpace = "27gb"
failBuild = true
}
dockerSupport {
loginToRegistry = on {
dockerRegistryId = "PROJECT_EXT_2307"
}
}
swabra { }
}
dependencies {
dependency(AbsoluteId("TC_Trunk_BuildDist")) {
snapshot { onDependencyFailure = FailureAction.IGNORE }
artifacts {
artifactRules = "TeamCity-*.tar.gz!/**=>context"
}
}
}
})


object TC2019_2_BuildDist_latest_nanoserver_1809 : BuildType({
name = "TC2019_2 latest-nanoserver-1809"
description  = "teamcity-server:latest-nanoserver-1809 teamcity-minimal-agent:latest-nanoserver-1809 teamcity-agent:latest-windowsservercore-1809:latest-nanoserver-1809"
vcs {root(RemoteTeamcityImages)}
steps {
dockerCommand {
name = "pull mcr.microsoft.com/powershell:nanoserver-1809"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/powershell:nanoserver-1809"
}
}

dockerCommand {
name = "pull mcr.microsoft.com/windows/nanoserver:1809"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/windows/nanoserver:1809"
}
}

dockerCommand {
name = "pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019"
}
}

dockerCommand {
name = "build teamcity-server:latest-nanoserver-1809"
commandType = build {
source = file {
path = """context/generated/windows/Server/nanoserver/1809/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-server:latest-nanoserver-1809
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-minimal-agent:latest-nanoserver-1809"
commandType = build {
source = file {
path = """context/generated/windows/MinimalAgent/nanoserver/1809/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-minimal-agent:latest-nanoserver-1809
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-windowsservercore-1809"
commandType = build {
source = file {
path = """context/generated/windows/Agent/windowsservercore/1809/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-windowsservercore-1809
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-nanoserver-1809"
commandType = build {
source = file {
path = """context/generated/windows/Agent/nanoserver/1809/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-nanoserver-1809
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "change tag from teamcity-server:latest-nanoserver-1809 to latest-nanoserver-1809"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-server:latest-nanoserver-1809 %docker.pushRepository%teamcity-server:latest-nanoserver-1809"
}
}

dockerCommand {
name = "change tag from teamcity-minimal-agent:latest-nanoserver-1809 to latest-nanoserver-1809"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-minimal-agent:latest-nanoserver-1809 %docker.pushRepository%teamcity-minimal-agent:latest-nanoserver-1809"
}
}

dockerCommand {
name = "change tag from teamcity-agent:latest-windowsservercore-1809 to latest-windowsservercore-1809"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:latest-windowsservercore-1809 %docker.pushRepository%teamcity-agent:latest-windowsservercore-1809"
}
}

dockerCommand {
name = "change tag from teamcity-agent:latest-nanoserver-1809 to latest-nanoserver-1809"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:latest-nanoserver-1809 %docker.pushRepository%teamcity-agent:latest-nanoserver-1809"
}
}

dockerCommand {
name = "push teamcity-server:latest-nanoserver-1809"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-server:latest-nanoserver-1809
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-minimal-agent:latest-nanoserver-1809"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-minimal-agent:latest-nanoserver-1809
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:latest-windowsservercore-1809"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:latest-windowsservercore-1809
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:latest-nanoserver-1809"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:latest-nanoserver-1809
""".trimIndent()
}
}

}
features {
freeDiskSpace {
requiredSpace = "27gb"
failBuild = true
}
dockerSupport {
loginToRegistry = on {
dockerRegistryId = "PROJECT_EXT_2307"
}
}
swabra { }
}
dependencies {
dependency(AbsoluteId("TC2019_2_BuildDist")) {
snapshot { onDependencyFailure = FailureAction.IGNORE }
artifacts {
artifactRules = "TeamCity-*.tar.gz!/**=>context"
}
}
}
})

object TC_Trunk_BuildDist_latest_nanoserver_1809 : BuildType({
name = "TC_Trunk latest-nanoserver-1809"
description  = "teamcity-server:latest-nanoserver-1809 teamcity-minimal-agent:latest-nanoserver-1809 teamcity-agent:latest-windowsservercore-1809:latest-nanoserver-1809"
vcs {root(RemoteTeamcityImages)}
steps {
dockerCommand {
name = "pull mcr.microsoft.com/powershell:nanoserver-1809"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/powershell:nanoserver-1809"
}
}

dockerCommand {
name = "pull mcr.microsoft.com/windows/nanoserver:1809"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/windows/nanoserver:1809"
}
}

dockerCommand {
name = "pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019"
}
}

dockerCommand {
name = "build teamcity-server:latest-nanoserver-1809"
commandType = build {
source = file {
path = """context/generated/windows/Server/nanoserver/1809/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-server:latest-nanoserver-1809
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-minimal-agent:latest-nanoserver-1809"
commandType = build {
source = file {
path = """context/generated/windows/MinimalAgent/nanoserver/1809/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-minimal-agent:latest-nanoserver-1809
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-windowsservercore-1809"
commandType = build {
source = file {
path = """context/generated/windows/Agent/windowsservercore/1809/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-windowsservercore-1809
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-nanoserver-1809"
commandType = build {
source = file {
path = """context/generated/windows/Agent/nanoserver/1809/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-nanoserver-1809
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "change tag from teamcity-server:latest-nanoserver-1809 to eap-latest-nanoserver-1809"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-server:latest-nanoserver-1809 %docker.pushRepository%teamcity-server:eap-latest-nanoserver-1809"
}
}

dockerCommand {
name = "change tag from teamcity-minimal-agent:latest-nanoserver-1809 to eap-latest-nanoserver-1809"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-minimal-agent:latest-nanoserver-1809 %docker.pushRepository%teamcity-minimal-agent:eap-latest-nanoserver-1809"
}
}

dockerCommand {
name = "change tag from teamcity-agent:latest-windowsservercore-1809 to eap-latest-windowsservercore-1809"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:latest-windowsservercore-1809 %docker.pushRepository%teamcity-agent:eap-latest-windowsservercore-1809"
}
}

dockerCommand {
name = "change tag from teamcity-agent:latest-nanoserver-1809 to eap-latest-nanoserver-1809"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:latest-nanoserver-1809 %docker.pushRepository%teamcity-agent:eap-latest-nanoserver-1809"
}
}

dockerCommand {
name = "push teamcity-server:eap-latest-nanoserver-1809"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-server:eap-latest-nanoserver-1809
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-minimal-agent:eap-latest-nanoserver-1809"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-minimal-agent:eap-latest-nanoserver-1809
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:eap-latest-windowsservercore-1809"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:eap-latest-windowsservercore-1809
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:eap-latest-nanoserver-1809"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:eap-latest-nanoserver-1809
""".trimIndent()
}
}

}
features {
freeDiskSpace {
requiredSpace = "27gb"
failBuild = true
}
dockerSupport {
loginToRegistry = on {
dockerRegistryId = "PROJECT_EXT_2307"
}
}
swabra { }
}
dependencies {
dependency(AbsoluteId("TC_Trunk_BuildDist")) {
snapshot { onDependencyFailure = FailureAction.IGNORE }
artifacts {
artifactRules = "TeamCity-*.tar.gz!/**=>context"
}
}
}
})


object TC2019_2_BuildDist_18_04_linux : BuildType({
name = "TC2019_2 18.04 linux"
description  = "teamcity-server:18.04,linux teamcity-minimal-agent:18.04,linux teamcity-agent:18.04,linux"
vcs {root(RemoteTeamcityImages)}
steps {
dockerCommand {
name = "pull ubuntu:18.04"
commandType = other {
subCommand = "pull"
commandArgs = "ubuntu:18.04"
}
}

dockerCommand {
name = "build teamcity-server:18.04,linux"
commandType = build {
source = file {
path = """context/generated/linux/Server/Ubuntu/18.04/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-server:18.04
teamcity-server:linux
""".trimIndent()
}
param("dockerImage.platform", "linux")
}

dockerCommand {
name = "build teamcity-minimal-agent:18.04,linux"
commandType = build {
source = file {
path = """context/generated/linux/MinimalAgent/Ubuntu/18.04/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-minimal-agent:18.04
teamcity-minimal-agent:linux
""".trimIndent()
}
param("dockerImage.platform", "linux")
}

dockerCommand {
name = "build teamcity-agent:18.04,linux"
commandType = build {
source = file {
path = """context/generated/linux/Agent/Ubuntu/18.04/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:18.04
teamcity-agent:linux
""".trimIndent()
}
param("dockerImage.platform", "linux")
}

dockerCommand {
name = "change tag from teamcity-server:18.04 to 18.04"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-server:18.04 %docker.pushRepository%teamcity-server:18.04"
}
}

dockerCommand {
name = "change tag from teamcity-server:linux to linux"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-server:linux %docker.pushRepository%teamcity-server:linux"
}
}

dockerCommand {
name = "change tag from teamcity-minimal-agent:18.04 to 18.04"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-minimal-agent:18.04 %docker.pushRepository%teamcity-minimal-agent:18.04"
}
}

dockerCommand {
name = "change tag from teamcity-minimal-agent:linux to linux"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-minimal-agent:linux %docker.pushRepository%teamcity-minimal-agent:linux"
}
}

dockerCommand {
name = "change tag from teamcity-agent:18.04 to 18.04"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:18.04 %docker.pushRepository%teamcity-agent:18.04"
}
}

dockerCommand {
name = "change tag from teamcity-agent:linux to linux"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:linux %docker.pushRepository%teamcity-agent:linux"
}
}

dockerCommand {
name = "push teamcity-server:18.04,linux"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-server:18.04
%docker.pushRepository%teamcity-server:linux
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-minimal-agent:18.04,linux"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-minimal-agent:18.04
%docker.pushRepository%teamcity-minimal-agent:linux
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:18.04,linux"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:18.04
%docker.pushRepository%teamcity-agent:linux
""".trimIndent()
}
}

}
features {
freeDiskSpace {
requiredSpace = "3gb"
failBuild = true
}
dockerSupport {
loginToRegistry = on {
dockerRegistryId = "PROJECT_EXT_2307"
}
}
swabra { }
}
dependencies {
dependency(AbsoluteId("TC2019_2_BuildDist")) {
snapshot { onDependencyFailure = FailureAction.IGNORE }
artifacts {
artifactRules = "TeamCity-*.tar.gz!/**=>context"
}
}
}
})

object TC_Trunk_BuildDist_18_04_linux : BuildType({
name = "TC_Trunk 18.04 linux"
description  = "teamcity-server:18.04,linux teamcity-minimal-agent:18.04,linux teamcity-agent:18.04,linux"
vcs {root(RemoteTeamcityImages)}
steps {
dockerCommand {
name = "pull ubuntu:18.04"
commandType = other {
subCommand = "pull"
commandArgs = "ubuntu:18.04"
}
}

dockerCommand {
name = "build teamcity-server:18.04,linux"
commandType = build {
source = file {
path = """context/generated/linux/Server/Ubuntu/18.04/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-server:18.04
teamcity-server:linux
""".trimIndent()
}
param("dockerImage.platform", "linux")
}

dockerCommand {
name = "build teamcity-minimal-agent:18.04,linux"
commandType = build {
source = file {
path = """context/generated/linux/MinimalAgent/Ubuntu/18.04/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-minimal-agent:18.04
teamcity-minimal-agent:linux
""".trimIndent()
}
param("dockerImage.platform", "linux")
}

dockerCommand {
name = "build teamcity-agent:18.04,linux"
commandType = build {
source = file {
path = """context/generated/linux/Agent/Ubuntu/18.04/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:18.04
teamcity-agent:linux
""".trimIndent()
}
param("dockerImage.platform", "linux")
}

dockerCommand {
name = "change tag from teamcity-server:18.04 to eap-18.04"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-server:18.04 %docker.pushRepository%teamcity-server:eap-18.04"
}
}

dockerCommand {
name = "change tag from teamcity-server:linux to eap-linux"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-server:linux %docker.pushRepository%teamcity-server:eap-linux"
}
}

dockerCommand {
name = "change tag from teamcity-minimal-agent:18.04 to eap-18.04"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-minimal-agent:18.04 %docker.pushRepository%teamcity-minimal-agent:eap-18.04"
}
}

dockerCommand {
name = "change tag from teamcity-minimal-agent:linux to eap-linux"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-minimal-agent:linux %docker.pushRepository%teamcity-minimal-agent:eap-linux"
}
}

dockerCommand {
name = "change tag from teamcity-agent:18.04 to eap-18.04"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:18.04 %docker.pushRepository%teamcity-agent:eap-18.04"
}
}

dockerCommand {
name = "change tag from teamcity-agent:linux to eap-linux"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:linux %docker.pushRepository%teamcity-agent:eap-linux"
}
}

dockerCommand {
name = "push teamcity-server:eap-18.04,eap-linux"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-server:eap-18.04
%docker.pushRepository%teamcity-server:eap-linux
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-minimal-agent:eap-18.04,eap-linux"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-minimal-agent:eap-18.04
%docker.pushRepository%teamcity-minimal-agent:eap-linux
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:eap-18.04,eap-linux"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:eap-18.04
%docker.pushRepository%teamcity-agent:eap-linux
""".trimIndent()
}
}

}
features {
freeDiskSpace {
requiredSpace = "3gb"
failBuild = true
}
dockerSupport {
loginToRegistry = on {
dockerRegistryId = "PROJECT_EXT_2307"
}
}
swabra { }
}
dependencies {
dependency(AbsoluteId("TC_Trunk_BuildDist")) {
snapshot { onDependencyFailure = FailureAction.IGNORE }
artifacts {
artifactRules = "TeamCity-*.tar.gz!/**=>context"
}
}
}
})


object TC2019_2_BuildDist_latest_nanoserver_1803 : BuildType({
name = "TC2019_2 latest-nanoserver-1803"
description  = "teamcity-server:latest-nanoserver-1803 teamcity-minimal-agent:latest-nanoserver-1803 teamcity-agent:latest-windowsservercore-1803:latest-nanoserver-1803"
vcs {root(RemoteTeamcityImages)}
steps {
dockerCommand {
name = "pull mcr.microsoft.com/powershell:nanoserver-1803"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/powershell:nanoserver-1803"
}
}

dockerCommand {
name = "pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1803"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1803"
}
}

dockerCommand {
name = "build teamcity-server:latest-nanoserver-1803"
commandType = build {
source = file {
path = """context/generated/windows/Server/nanoserver/1803/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-server:latest-nanoserver-1803
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-minimal-agent:latest-nanoserver-1803"
commandType = build {
source = file {
path = """context/generated/windows/MinimalAgent/nanoserver/1803/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-minimal-agent:latest-nanoserver-1803
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-windowsservercore-1803"
commandType = build {
source = file {
path = """context/generated/windows/Agent/windowsservercore/1803/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-windowsservercore-1803
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-nanoserver-1803"
commandType = build {
source = file {
path = """context/generated/windows/Agent/nanoserver/1803/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-nanoserver-1803
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "change tag from teamcity-server:latest-nanoserver-1803 to latest-nanoserver-1803"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-server:latest-nanoserver-1803 %docker.pushRepository%teamcity-server:latest-nanoserver-1803"
}
}

dockerCommand {
name = "change tag from teamcity-minimal-agent:latest-nanoserver-1803 to latest-nanoserver-1803"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-minimal-agent:latest-nanoserver-1803 %docker.pushRepository%teamcity-minimal-agent:latest-nanoserver-1803"
}
}

dockerCommand {
name = "change tag from teamcity-agent:latest-windowsservercore-1803 to latest-windowsservercore-1803"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:latest-windowsservercore-1803 %docker.pushRepository%teamcity-agent:latest-windowsservercore-1803"
}
}

dockerCommand {
name = "change tag from teamcity-agent:latest-nanoserver-1803 to latest-nanoserver-1803"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:latest-nanoserver-1803 %docker.pushRepository%teamcity-agent:latest-nanoserver-1803"
}
}

dockerCommand {
name = "push teamcity-server:latest-nanoserver-1803"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-server:latest-nanoserver-1803
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-minimal-agent:latest-nanoserver-1803"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-minimal-agent:latest-nanoserver-1803
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:latest-windowsservercore-1803"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:latest-windowsservercore-1803
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:latest-nanoserver-1803"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:latest-nanoserver-1803
""".trimIndent()
}
}

}
features {
freeDiskSpace {
requiredSpace = "26gb"
failBuild = true
}
dockerSupport {
loginToRegistry = on {
dockerRegistryId = "PROJECT_EXT_2307"
}
}
swabra { }
}
dependencies {
dependency(AbsoluteId("TC2019_2_BuildDist")) {
snapshot { onDependencyFailure = FailureAction.IGNORE }
artifacts {
artifactRules = "TeamCity-*.tar.gz!/**=>context"
}
}
}
})

object TC_Trunk_BuildDist_latest_nanoserver_1803 : BuildType({
name = "TC_Trunk latest-nanoserver-1803"
description  = "teamcity-server:latest-nanoserver-1803 teamcity-minimal-agent:latest-nanoserver-1803 teamcity-agent:latest-windowsservercore-1803:latest-nanoserver-1803"
vcs {root(RemoteTeamcityImages)}
steps {
dockerCommand {
name = "pull mcr.microsoft.com/powershell:nanoserver-1803"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/powershell:nanoserver-1803"
}
}

dockerCommand {
name = "pull mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1803"
commandType = other {
subCommand = "pull"
commandArgs = "mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-1803"
}
}

dockerCommand {
name = "build teamcity-server:latest-nanoserver-1803"
commandType = build {
source = file {
path = """context/generated/windows/Server/nanoserver/1803/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-server:latest-nanoserver-1803
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-minimal-agent:latest-nanoserver-1803"
commandType = build {
source = file {
path = """context/generated/windows/MinimalAgent/nanoserver/1803/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-minimal-agent:latest-nanoserver-1803
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-windowsservercore-1803"
commandType = build {
source = file {
path = """context/generated/windows/Agent/windowsservercore/1803/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-windowsservercore-1803
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-nanoserver-1803"
commandType = build {
source = file {
path = """context/generated/windows/Agent/nanoserver/1803/Dockerfile"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-nanoserver-1803
""".trimIndent()
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "change tag from teamcity-server:latest-nanoserver-1803 to eap-latest-nanoserver-1803"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-server:latest-nanoserver-1803 %docker.pushRepository%teamcity-server:eap-latest-nanoserver-1803"
}
}

dockerCommand {
name = "change tag from teamcity-minimal-agent:latest-nanoserver-1803 to eap-latest-nanoserver-1803"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-minimal-agent:latest-nanoserver-1803 %docker.pushRepository%teamcity-minimal-agent:eap-latest-nanoserver-1803"
}
}

dockerCommand {
name = "change tag from teamcity-agent:latest-windowsservercore-1803 to eap-latest-windowsservercore-1803"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:latest-windowsservercore-1803 %docker.pushRepository%teamcity-agent:eap-latest-windowsservercore-1803"
}
}

dockerCommand {
name = "change tag from teamcity-agent:latest-nanoserver-1803 to eap-latest-nanoserver-1803"
commandType = other {
subCommand = "tag"
commandArgs = "teamcity-agent:latest-nanoserver-1803 %docker.pushRepository%teamcity-agent:eap-latest-nanoserver-1803"
}
}

dockerCommand {
name = "push teamcity-server:eap-latest-nanoserver-1803"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-server:eap-latest-nanoserver-1803
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-minimal-agent:eap-latest-nanoserver-1803"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-minimal-agent:eap-latest-nanoserver-1803
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:eap-latest-windowsservercore-1803"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:eap-latest-windowsservercore-1803
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:eap-latest-nanoserver-1803"
commandType = push {
namesAndTags = """
%docker.pushRepository%teamcity-agent:eap-latest-nanoserver-1803
""".trimIndent()
}
}

}
features {
freeDiskSpace {
requiredSpace = "26gb"
failBuild = true
}
dockerSupport {
loginToRegistry = on {
dockerRegistryId = "PROJECT_EXT_2307"
}
}
swabra { }
}
dependencies {
dependency(AbsoluteId("TC_Trunk_BuildDist")) {
snapshot { onDependencyFailure = FailureAction.IGNORE }
artifacts {
artifactRules = "TeamCity-*.tar.gz!/**=>context"
}
}
}
})


object TC2019_2_BuildDist_root : BuildType(
{
name = "TC2019_2 Build All Docker Images"
dependencies {
snapshot(AbsoluteId("TC2019_2_BuildDist"))
{ onDependencyFailure = FailureAction.IGNORE }
snapshot(TC2019_2_BuildDist_latest_nanoserver_1903)
{ onDependencyFailure = FailureAction.IGNORE }
snapshot(TC2019_2_BuildDist_latest_nanoserver_1809)
{ onDependencyFailure = FailureAction.IGNORE }
snapshot(TC2019_2_BuildDist_18_04_linux)
{ onDependencyFailure = FailureAction.IGNORE }
snapshot(TC2019_2_BuildDist_latest_nanoserver_1803)
{ onDependencyFailure = FailureAction.IGNORE }
}
})

object TC_Trunk_BuildDist_root : BuildType(
{
name = "TC_Trunk Build All Docker Images"
dependencies {
snapshot(AbsoluteId("TC_Trunk_BuildDist"))
{ onDependencyFailure = FailureAction.IGNORE }
snapshot(TC_Trunk_BuildDist_latest_nanoserver_1903)
{ onDependencyFailure = FailureAction.IGNORE }
snapshot(TC_Trunk_BuildDist_latest_nanoserver_1809)
{ onDependencyFailure = FailureAction.IGNORE }
snapshot(TC_Trunk_BuildDist_18_04_linux)
{ onDependencyFailure = FailureAction.IGNORE }
snapshot(TC_Trunk_BuildDist_latest_nanoserver_1803)
{ onDependencyFailure = FailureAction.IGNORE }
}
})

project {
vcsRoot(RemoteTeamcityImages)
buildType(TC2019_2_BuildDist_latest_nanoserver_1903)
buildType(TC2019_2_BuildDist_latest_nanoserver_1809)
buildType(TC2019_2_BuildDist_18_04_linux)
buildType(TC2019_2_BuildDist_latest_nanoserver_1803)
buildType(TC2019_2_BuildDist_root)
buildType(TC_Trunk_BuildDist_latest_nanoserver_1903)
buildType(TC_Trunk_BuildDist_latest_nanoserver_1809)
buildType(TC_Trunk_BuildDist_18_04_linux)
buildType(TC_Trunk_BuildDist_latest_nanoserver_1803)
buildType(TC_Trunk_BuildDist_root)
}

object RemoteTeamcityImages : GitVcsRoot({
name = "remote teamcity images"
url = "https://github.com/NikolayPianikov/teamcity-images.git"
})
