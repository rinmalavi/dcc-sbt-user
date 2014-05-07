
import sbt._
import sbt.Keys._

object ClientSbtTest extends Build {
  lazy val clientSbt = ProjectRef(file("../../dsl-compiler-client/client-api"), "sbt")

  lazy val root = Project(
    id = "someProject",
    base = file("."),
    settings = Project.defaultSettings
  ).dependsOn( clientSbt)
}

