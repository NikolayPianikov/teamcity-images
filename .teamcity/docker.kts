import jetbrains.buildServer.configs.kotlin.v2019_2.*
import jetbrains.buildServer.configs.kotlin.v2019_2.vcs.GitVcsRoot
import jetbrains.buildServer.configs.kotlin.v2019_2.buildSteps.dockerCommand
version = "2019.2"

object build_1 : BuildType({
name = "build docker image build_1"
steps {
dockerCommand {
name = "build teamcity-server:latest-nanoserver-1903"
commandType = build {
source = file {
path = """windows/Server/nanoserver/1903"""
}
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
path = """windows/MinimalAgent/nanoserver/1903"""
}
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
path = """windows/Agent/windowsservercore/1903"""
}
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
path = """windows/Agent/nanoserver/1903"""
}
namesAndTags = """
teamcity-agent:latest-nanoserver-1903
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

}
})


object build_2 : BuildType({
name = "build docker image build_2"
steps {
dockerCommand {
name = "build teamcity-server:latest-nanoserver-1809"
commandType = build {
source = file {
path = """windows/Server/nanoserver/1809"""
}
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
path = """windows/MinimalAgent/nanoserver/1809"""
}
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
path = """windows/Agent/windowsservercore/1809"""
}
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
path = """windows/Agent/nanoserver/1809"""
}
namesAndTags = """
teamcity-agent:latest-nanoserver-1809
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

}
})


object build_3 : BuildType({
name = "build docker image build_3"
steps {
dockerCommand {
name = "build teamcity-server:18.04,linux"
commandType = build {
source = file {
path = """linux/Server/Ubuntu/18.04"""
}
namesAndTags = """
teamcity-server:18.04
teamcity-server:linux
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-minimal-agent:18.04,linux"
commandType = build {
source = file {
path = """linux/MinimalAgent/Ubuntu/18.04"""
}
namesAndTags = """
teamcity-minimal-agent:18.04
teamcity-minimal-agent:linux
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

dockerCommand {
name = "build teamcity-agent:18.04,linux"
commandType = build {
source = file {
path = """linux/Agent/Ubuntu/18.04"""
}
namesAndTags = """
teamcity-agent:18.04
teamcity-agent:linux
""".trimIndent()
commandArgs = "--pull"
}
param("dockerImage.platform", "windows")
}

}
})


object build_4 : BuildType({
name = "build docker image build_4"
steps {
dockerCommand {
name = "build teamcity-server:latest-nanoserver-1803"
commandType = build {
source = file {
path = """windows/Server/nanoserver/1803"""
}
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
path = """windows/MinimalAgent/nanoserver/1803"""
}
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
path = """windows/Agent/windowsservercore/1803"""
}
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
path = """windows/Agent/nanoserver/1803"""
}
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
buildType(build_1)
buildType(build_2)
buildType(build_3)
buildType(build_4)
}

object RemoteTeamcityImages : GitVcsRoot({
name = "remote teamcity images"
url = "https://github.com/NikolayPianikov/teamcity-images.git"
})
