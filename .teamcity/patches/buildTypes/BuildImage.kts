package patches.buildTypes

import jetbrains.buildServer.configs.kotlin.v2019_2.*
import jetbrains.buildServer.configs.kotlin.v2019_2.buildSteps.dockerCommand
import jetbrains.buildServer.configs.kotlin.v2019_2.ui.*

/*
This patch script was generated by TeamCity on settings change in UI.
To apply the patch, change the buildType with id = 'BuildImage'
accordingly, and delete the patch script.
*/
changeBuildType(RelativeId("BuildImage")) {
    check(description == "") {
        "Unexpected description: '$description'"
    }
    description = "dasdas fdfef"

    expectSteps {
    }
    steps {
        insert(0) {
            dockerCommand {
                name = "step 1"
                commandType = build {
                    source = content {
                        content = """
                            aaaa
                            cdscdsc
                        """.trimIndent()
                    }
                    commandArgs = "--pull"
                }
                param("dockerImage.platform", "windows")
            }
        }
        insert(1) {
            dockerCommand {
                name = "step 2"
                commandType = build {
                    source = content {
                        content = "aaaa"
                    }
                    commandArgs = "--pull"
                }
                param("dockerImage.platform", "windows")
            }
        }
    }
}
