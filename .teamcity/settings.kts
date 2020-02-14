import jetbrains.buildServer.configs.kotlin.v2019_2.*
import jetbrains.buildServer.configs.kotlin.v2019_2.vcs.GitVcsRoot

version = "2019.2"

project {
    vcsRoot(RemoteTeamcityImages)
    buildType(BuildImage)
}

object BuildImage : BuildType({
    name = "build image 2"
})

object RemoteTeamcityImages : GitVcsRoot({
    name = "remote teamcity images"
    url = "https://github.com/NikolayPianikov/teamcity-images.git"
})
