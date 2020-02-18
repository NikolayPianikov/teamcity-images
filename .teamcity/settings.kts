import jetbrains.buildServer.configs.kotlin.v2019_2.*
import jetbrains.buildServer.configs.kotlin.v2019_2.vcs.GitVcsRoot
import jetbrains.buildServer.configs.kotlin.v2019_2.buildSteps.dockerCommand
version = "2019.2"

object teamcity-agent:latest-nanoserver-1903:latest-windowsservercore-1903_teamcity-minimal-agent:latest-nanoserver-1903_teamcity-server:latest-nanoserver-1903 : BuildType({
name = "build docker image teamcity-agent:latest-nanoserver-1903:latest-windowsservercore-1903_teamcity-minimal-agent:latest-nanoserver-1903_teamcity-server:latest-nanoserver-1903"
steps {
dockerCommand {
name = "build teamcity-server:latest-nanoserver-1903"
commandType = build {
source = file {
path = """context/generated/windows/Server/nanoserver/1903"""
}
contextDir = "context"
namesAndTags = """
teamcity-server:latest-nanoserver-1903
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-minimal-agent:latest-nanoserver-1903"
commandType = build {
source = file {
path = """context/generated/windows/MinimalAgent/nanoserver/1903"""
}
contextDir = "context"
namesAndTags = """
teamcity-minimal-agent:latest-nanoserver-1903
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-windowsservercore-1903"
commandType = build {
source = file {
path = """context/generated/windows/Agent/windowsservercore/1903"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-windowsservercore-1903
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-nanoserver-1903"
commandType = build {
source = file {
path = """context/generated/windows/Agent/nanoserver/1903"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-nanoserver-1903
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

}
})


object teamcity-agent:latest-nanoserver-1809:latest-windowsservercore-1809_teamcity-minimal-agent:latest-nanoserver-1809_teamcity-server:latest-nanoserver-1809 : BuildType({
name = "build docker image teamcity-agent:latest-nanoserver-1809:latest-windowsservercore-1809_teamcity-minimal-agent:latest-nanoserver-1809_teamcity-server:latest-nanoserver-1809"
steps {
dockerCommand {
name = "build teamcity-server:latest-nanoserver-1809"
commandType = build {
source = file {
path = """context/generated/windows/Server/nanoserver/1809"""
}
contextDir = "context"
namesAndTags = """
teamcity-server:latest-nanoserver-1809
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-minimal-agent:latest-nanoserver-1809"
commandType = build {
source = file {
path = """context/generated/windows/MinimalAgent/nanoserver/1809"""
}
contextDir = "context"
namesAndTags = """
teamcity-minimal-agent:latest-nanoserver-1809
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-windowsservercore-1809"
commandType = build {
source = file {
path = """context/generated/windows/Agent/windowsservercore/1809"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-windowsservercore-1809
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-nanoserver-1809"
commandType = build {
source = file {
path = """context/generated/windows/Agent/nanoserver/1809"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-nanoserver-1809
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

}
})


object teamcity-agent:18.04,linux_teamcity-minimal-agent:18.04,linux_teamcity-server:18.04,linux : BuildType({
name = "build docker image teamcity-agent:18.04,linux_teamcity-minimal-agent:18.04,linux_teamcity-server:18.04,linux"
steps {
dockerCommand {
name = "build teamcity-server:18.04,linux"
commandType = build {
source = file {
path = """context/generated/linux/Server/Ubuntu/18.04"""
}
contextDir = "context"
namesAndTags = """
teamcity-server:18.04
teamcity-server:linux
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "linux")
}

dockerCommand {
name = "build teamcity-minimal-agent:18.04,linux"
commandType = build {
source = file {
path = """context/generated/linux/MinimalAgent/Ubuntu/18.04"""
}
contextDir = "context"
namesAndTags = """
teamcity-minimal-agent:18.04
teamcity-minimal-agent:linux
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "linux")
}

dockerCommand {
name = "build teamcity-agent:18.04,linux"
commandType = build {
source = file {
path = """context/generated/linux/Agent/Ubuntu/18.04"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:18.04
teamcity-agent:linux
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "linux")
}

}
})


object teamcity-agent:latest-nanoserver-1803:latest-windowsservercore-1803_teamcity-minimal-agent:latest-nanoserver-1803_teamcity-server:latest-nanoserver-1803 : BuildType({
name = "build docker image teamcity-agent:latest-nanoserver-1803:latest-windowsservercore-1803_teamcity-minimal-agent:latest-nanoserver-1803_teamcity-server:latest-nanoserver-1803"
steps {
dockerCommand {
name = "build teamcity-server:latest-nanoserver-1803"
commandType = build {
source = file {
path = """context/generated/windows/Server/nanoserver/1803"""
}
contextDir = "context"
namesAndTags = """
teamcity-server:latest-nanoserver-1803
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-minimal-agent:latest-nanoserver-1803"
commandType = build {
source = file {
path = """context/generated/windows/MinimalAgent/nanoserver/1803"""
}
contextDir = "context"
namesAndTags = """
teamcity-minimal-agent:latest-nanoserver-1803
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-windowsservercore-1803"
commandType = build {
source = file {
path = """context/generated/windows/Agent/windowsservercore/1803"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-windowsservercore-1803
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:latest-nanoserver-1803"
commandType = build {
source = file {
path = """context/generated/windows/Agent/nanoserver/1803"""
}
contextDir = "context"
namesAndTags = """
teamcity-agent:latest-nanoserver-1803
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

}
})


project {
vcsRoot(RemoteTeamcityImages)
buildType(teamcity-agent:latest-nanoserver-1903:latest-windowsservercore-1903_teamcity-minimal-agent:latest-nanoserver-1903_teamcity-server:latest-nanoserver-1903)
buildType(teamcity-agent:latest-nanoserver-1809:latest-windowsservercore-1809_teamcity-minimal-agent:latest-nanoserver-1809_teamcity-server:latest-nanoserver-1809)
buildType(teamcity-agent:18.04,linux_teamcity-minimal-agent:18.04,linux_teamcity-server:18.04,linux)
buildType(teamcity-agent:latest-nanoserver-1803:latest-windowsservercore-1803_teamcity-minimal-agent:latest-nanoserver-1803_teamcity-server:latest-nanoserver-1803)
}

object RemoteTeamcityImages : GitVcsRoot({
name = "remote teamcity images"
url = "https://github.com/NikolayPianikov/teamcity-images.git"
})
