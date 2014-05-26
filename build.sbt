
import dslplatform.CompilerPlugin.DslKeys._

val dslConfig = com.typesafe.config.ConfigFactory.parseFile(file(System.getProperty("user.home")) / ".config" /  "some.txt")

username := dslConfig.getString("dsl.username")

password := dslConfig.getString("dsl.password")

outputDirectory := Some(file("src/main"))

libraryDependencies ++= Seq(
  "org.slf4j" % "slf4j-simple" % "1.7.5",
  "com.dslplatform" % "dsl-client-http-apache" % "0.4.14"
)

resolvers in ThisBuild += "NGS Snapshots" at "http://ngs.hr/nexus/content/repositories/snapshots/"

credentials in ThisBuild += Credentials(Path.userHome / ".config" / "dsl-compiler-client" / "nexus.config")

targetSources := Set("Java")

packageName := "namespace"

monoDependencyFolder    := file(System.getProperty("user.home")) / "code" / "dsl_compiler_client_user" / "revenj"

monoServerLocation      := file(System.getProperty("user.home")) / "wwwroot" / "applicationname"

databaseConnection := Map(
    "ServerName" -> dslConfig.getString("db.ServerName"),
    "Port" -> dslConfig.getString("db.Port"),
    "DatabaseName" -> dslConfig.getString("db.DatabaseName"),
    "User" -> dslConfig.getString("db.User"),
    "Password" -> dslConfig.getString("db.Password"))
