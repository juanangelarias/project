/**
* JetBrains Space Automation
* This Kotlin-script file lets you automate build activities
* For more info, see https://www.jetbrains.com/help/space/automation.html
*/

job("Build and publish") {
    container(displayName = "Build and notify", image = "gradle"){
        kotlinScript { api ->
            api.gradlew("build")
            api.space().projects.planing.issues.createIssue()
        }
    }
}
