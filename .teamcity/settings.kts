import jetbrains.buildServer.configs.kotlin.v2019_2.*
import jetbrains.buildServer.configs.kotlin.v2019_2.vcs.GitVcsRoot
import jetbrains.buildServer.configs.kotlin.v2019_2.buildSteps.dockerCommand
import jetbrains.buildServer.configs.kotlin.v2019_2.buildFeatures.freeDiskSpace
version = "2019.2"

object TC2019_2_BuildDist_latest_nanoserver_1903 : BuildType({
name = "latest-nanoserver-1903"
description  = "teamcity-server:latest-nanoserver-1903 teamcity-minimal-agent:latest-nanoserver-1903 teamcity-agent:latest-windowsservercore-1903:latest-nanoserver-1903"
vcs {root(RemoteTeamcityImages)}
steps {
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
name = "image tag teamcity-server:latest-nanoserver-1903"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-server:latest-nanoserver-1903 %repository%teamcity-server:latest-nanoserver-1903"
}
}

dockerCommand {
name = "image tag teamcity-minimal-agent:latest-nanoserver-1903"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-minimal-agent:latest-nanoserver-1903 %repository%teamcity-minimal-agent:latest-nanoserver-1903"
}
}

dockerCommand {
name = "image tag teamcity-agent:latest-windowsservercore-1903"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-agent:latest-windowsservercore-1903 %repository%teamcity-agent:latest-windowsservercore-1903"
}
}

dockerCommand {
name = "image tag teamcity-agent:latest-nanoserver-1903"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-agent:latest-nanoserver-1903 %repository%teamcity-agent:latest-nanoserver-1903"
}
}

dockerCommand {
name = "push teamcity-server:latest-nanoserver-1903"
commandType = push {
namesAndTags = """
%repository%teamcity-server:latest-nanoserver-1903
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-minimal-agent:latest-nanoserver-1903"
commandType = push {
namesAndTags = """
%repository%teamcity-minimal-agent:latest-nanoserver-1903
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:latest-windowsservercore-1903"
commandType = push {
namesAndTags = """
%repository%teamcity-agent:latest-windowsservercore-1903
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:latest-nanoserver-1903"
commandType = push {
namesAndTags = """
%repository%teamcity-agent:latest-nanoserver-1903
""".trimIndent()
}
}

}
features {
freeDiskSpace {
requiredSpace = "27gb"
failBuild = true
}
}
dependencies {
dependency(AbsoluteId("TC2019_2_BuildDist")) {
snapshot { }
artifacts {
artifactRules = "TeamCity-*.tar.gz!/**=>context"
}
}
}
})


object TC2019_2_BuildDist_latest_nanoserver_1809 : BuildType({
name = "latest-nanoserver-1809"
description  = "teamcity-server:latest-nanoserver-1809 teamcity-minimal-agent:latest-nanoserver-1809 teamcity-agent:latest-windowsservercore-1809:latest-nanoserver-1809"
vcs {root(RemoteTeamcityImages)}
steps {
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
name = "image tag teamcity-server:latest-nanoserver-1809"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-server:latest-nanoserver-1809 %repository%teamcity-server:latest-nanoserver-1809"
}
}

dockerCommand {
name = "image tag teamcity-minimal-agent:latest-nanoserver-1809"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-minimal-agent:latest-nanoserver-1809 %repository%teamcity-minimal-agent:latest-nanoserver-1809"
}
}

dockerCommand {
name = "image tag teamcity-agent:latest-windowsservercore-1809"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-agent:latest-windowsservercore-1809 %repository%teamcity-agent:latest-windowsservercore-1809"
}
}

dockerCommand {
name = "image tag teamcity-agent:latest-nanoserver-1809"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-agent:latest-nanoserver-1809 %repository%teamcity-agent:latest-nanoserver-1809"
}
}

dockerCommand {
name = "push teamcity-server:latest-nanoserver-1809"
commandType = push {
namesAndTags = """
%repository%teamcity-server:latest-nanoserver-1809
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-minimal-agent:latest-nanoserver-1809"
commandType = push {
namesAndTags = """
%repository%teamcity-minimal-agent:latest-nanoserver-1809
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:latest-windowsservercore-1809"
commandType = push {
namesAndTags = """
%repository%teamcity-agent:latest-windowsservercore-1809
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:latest-nanoserver-1809"
commandType = push {
namesAndTags = """
%repository%teamcity-agent:latest-nanoserver-1809
""".trimIndent()
}
}

}
features {
freeDiskSpace {
requiredSpace = "27gb"
failBuild = true
}
}
dependencies {
dependency(AbsoluteId("TC2019_2_BuildDist")) {
snapshot { }
artifacts {
artifactRules = "TeamCity-*.tar.gz!/**=>context"
}
}
}
})


object TC2019_2_BuildDist_18_04_linux : BuildType({
name = "18.04 linux"
description  = "teamcity-server:18.04,linux teamcity-minimal-agent:18.04,linux teamcity-agent:18.04,linux"
vcs {root(RemoteTeamcityImages)}
steps {
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
name = "image tag teamcity-server:18.04"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-server:18.04 %repository%teamcity-server:18.04"
}
}

dockerCommand {
name = "image tag teamcity-server:linux"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-server:linux %repository%teamcity-server:linux"
}
}

dockerCommand {
name = "image tag teamcity-minimal-agent:18.04"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-minimal-agent:18.04 %repository%teamcity-minimal-agent:18.04"
}
}

dockerCommand {
name = "image tag teamcity-minimal-agent:linux"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-minimal-agent:linux %repository%teamcity-minimal-agent:linux"
}
}

dockerCommand {
name = "image tag teamcity-agent:18.04"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-agent:18.04 %repository%teamcity-agent:18.04"
}
}

dockerCommand {
name = "image tag teamcity-agent:linux"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-agent:linux %repository%teamcity-agent:linux"
}
}

dockerCommand {
name = "push teamcity-server:18.04,linux"
commandType = push {
namesAndTags = """
%repository%teamcity-server:18.04
%repository%teamcity-server:linux
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-minimal-agent:18.04,linux"
commandType = push {
namesAndTags = """
%repository%teamcity-minimal-agent:18.04
%repository%teamcity-minimal-agent:linux
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:18.04,linux"
commandType = push {
namesAndTags = """
%repository%teamcity-agent:18.04
%repository%teamcity-agent:linux
""".trimIndent()
}
}

}
features {
freeDiskSpace {
requiredSpace = "3gb"
failBuild = true
}
}
dependencies {
dependency(AbsoluteId("TC2019_2_BuildDist")) {
snapshot { }
artifacts {
artifactRules = "TeamCity-*.tar.gz!/**=>context"
}
}
}
})


object TC2019_2_BuildDist_latest_nanoserver_1803 : BuildType({
name = "latest-nanoserver-1803"
description  = "teamcity-server:latest-nanoserver-1803 teamcity-minimal-agent:latest-nanoserver-1803 teamcity-agent:latest-windowsservercore-1803:latest-nanoserver-1803"
vcs {root(RemoteTeamcityImages)}
steps {
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
name = "image tag teamcity-server:latest-nanoserver-1803"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-server:latest-nanoserver-1803 %repository%teamcity-server:latest-nanoserver-1803"
}
}

dockerCommand {
name = "image tag teamcity-minimal-agent:latest-nanoserver-1803"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-minimal-agent:latest-nanoserver-1803 %repository%teamcity-minimal-agent:latest-nanoserver-1803"
}
}

dockerCommand {
name = "image tag teamcity-agent:latest-windowsservercore-1803"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-agent:latest-windowsservercore-1803 %repository%teamcity-agent:latest-windowsservercore-1803"
}
}

dockerCommand {
name = "image tag teamcity-agent:latest-nanoserver-1803"
commandType = other {
subCommand = "image tag"
commandArgs = "teamcity-agent:latest-nanoserver-1803 %repository%teamcity-agent:latest-nanoserver-1803"
}
}

dockerCommand {
name = "push teamcity-server:latest-nanoserver-1803"
commandType = push {
namesAndTags = """
%repository%teamcity-server:latest-nanoserver-1803
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-minimal-agent:latest-nanoserver-1803"
commandType = push {
namesAndTags = """
%repository%teamcity-minimal-agent:latest-nanoserver-1803
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:latest-windowsservercore-1803"
commandType = push {
namesAndTags = """
%repository%teamcity-agent:latest-windowsservercore-1803
""".trimIndent()
}
}

dockerCommand {
name = "push teamcity-agent:latest-nanoserver-1803"
commandType = push {
namesAndTags = """
%repository%teamcity-agent:latest-nanoserver-1803
""".trimIndent()
}
}

}
features {
freeDiskSpace {
requiredSpace = "26gb"
failBuild = true
}
}
dependencies {
dependency(AbsoluteId("TC2019_2_BuildDist")) {
snapshot { }
artifacts {
artifactRules = "TeamCity-*.tar.gz!/**=>context"
}
}
}
})


object root : BuildType({
name = "Build All Docker Images"
artifactRules = "context/generated => "
dependencies {
dependency(TC2019_2_BuildDist_latest_nanoserver_1903) {
snapshot {}
}
dependency(TC2019_2_BuildDist_latest_nanoserver_1809) {
snapshot {}
}
dependency(TC2019_2_BuildDist_18_04_linux) {
snapshot {}
}
dependency(TC2019_2_BuildDist_latest_nanoserver_1803) {
snapshot {}
}
}
})

project {
vcsRoot(RemoteTeamcityImages)
buildType(TC2019_2_BuildDist_latest_nanoserver_1903)
buildType(TC2019_2_BuildDist_latest_nanoserver_1809)
buildType(TC2019_2_BuildDist_18_04_linux)
buildType(TC2019_2_BuildDist_latest_nanoserver_1803)
buildType(root)
}

object RemoteTeamcityImages : GitVcsRoot({
name = "remote teamcity images"
url = "https://github.com/NikolayPianikov/teamcity-images.git"
})
