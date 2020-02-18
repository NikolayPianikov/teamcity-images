package patches.buildTypes

import jetbrains.buildServer.configs.kotlin.v2019_2.*
import jetbrains.buildServer.configs.kotlin.v2019_2.ui.*

/*
This patch script was generated by TeamCity on settings change in UI.
To apply the patch, change the buildType with id = 'build_3'
accordingly, and delete the patch script.
*/
changeBuildType(RelativeId("build_3")) {
    dependencies {
        expect(AbsoluteId("Docker_TeamCityDist")) {
            snapshot {
            }

            artifacts {
                artifactRules = "TeamCity-*.tar.gz!=>context"
            }
        }
        update(AbsoluteId("Docker_TeamCityDist")) {
            snapshot {
            }

            artifacts {
                artifactRules = "TeamCity-*.tar.gz!/**=>context/"
            }
        }

    }
}
