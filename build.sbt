import dslplatform.CompilerPlugin.DslKeys._

projectPropsPath :=  Some(file(System.getProperty("user.home")) / ".config" /  "dccTest" / "project.props")

outputDirectory := Some(file("src/main"))

libraryDependencies ++= Seq(
  "org.slf4j" % "slf4j-simple" % "1.7.5",
  "com.dslplatform" % "dsl-client-http-apache" % "0.4.14"
)

targetSources := Set("Java", "Scala")

packageName := "namespace"

monoDependencyFolder    := file(System.getProperty("user.home")) / "code" / "dsl_compiler_client_user" / "revenj"

//monoServerLocation      := file(System.getProperty("user.home")) / "wwwroot" / "applicationname"
