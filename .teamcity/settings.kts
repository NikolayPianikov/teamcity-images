import jetbrains.buildServer.configs.kotlin.v2019_2.*
import jetbrains.buildServer.configs.kotlin.v2019_2.vcs.GitVcsRoot
import jetbrains.buildServer.configs.kotlin.v2019_2.buildSteps.dockerCommand
version = "2019.2"

object teamcity_agent_latest_nanoserver_1903_latest_windowsservercore_1903_teamcity_minimal_agent_latest_nanoserver_1903_teamcity_server_latest_nanoserver_1903 : BuildType({
name = "build docker image teamcity_agent_latest_nanoserver_1903_latest_windowsservercore_1903_teamcity_minimal_agent_latest_nanoserver_1903_teamcity_server_latest_nanoserver_1903"
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


object teamcity_agent_latest_nanoserver_1809_latest_windowsservercore_1809_teamcity_minimal_agent_latest_nanoserver_1809_teamcity_server_latest_nanoserver_1809 : BuildType({
name = "build docker image teamcity_agent_latest_nanoserver_1809_latest_windowsservercore_1809_teamcity_minimal_agent_latest_nanoserver_1809_teamcity_server_latest_nanoserver_1809"
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


object teamcity_agent_18.04,linux_teamcity_minimal_agent_18.04,linux_teamcity_server_18.04,linux : BuildType({
name = "build docker image teamcity_agent_18.04,linux_teamcity_minimal_agent_18.04,linux_teamcity_server_18.04,linux"
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


object teamcity_agent_latest_nanoserver_1803_latest_windowsservercore_1803_teamcity_minimal_agent_latest_nanoserver_1803_teamcity_server_latest_nanoserver_1803 : BuildType({
name = "build docker image teamcity_agent_latest_nanoserver_1803_latest_windowsservercore_1803_teamcity_minimal_agent_latest_nanoserver_1803_teamcity_server_latest_nanoserver_1803"
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
buildType(teamcity_agent_latest_nanoserver_1903_latest_windowsservercore_1903_teamcity_minimal_agent_latest_nanoserver_1903_teamcity_server_latest_nanoserver_1903)
buildType(teamcity_agent_latest_nanoserver_1809_latest_windowsservercore_1809_teamcity_minimal_agent_latest_nanoserver_1809_teamcity_server_latest_nanoserver_1809)
buildType(teamcity_agent_18.04,linux_teamcity_minimal_agent_18.04,linux_teamcity_server_18.04,linux)
buildType(teamcity_agent_latest_nanoserver_1803_latest_windowsservercore_1803_teamcity_minimal_agent_latest_nanoserver_1803_teamcity_server_latest_nanoserver_1803)
}

object RemoteTeamcityImages : GitVcsRoot({
name = "remote teamcity images"
url = "https://github.com/NikolayPianikov/teamcity-images.git"
})
