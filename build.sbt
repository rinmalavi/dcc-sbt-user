import com.dslplatform.compiler.client.api.core.impl.UnmanagedDSLImpl
import dslplatform.CompilerPlugin.DslKeys._

username := "rinmalavi@gmail.com"

password := "qwe321"

dslProjectId := "4b875a05-fc95-4591-a76d-b73210dbcb31"

outputDirectory := Some(file("src/main"))

libraryDependencies ++= Seq(
  "org.slf4j" % "slf4j-simple" % "1.7.5",
  "com.dslplatform" % "dsl-client-http-apache" % "0.4.14"
)

targetSources := Set("Java")

packageName := "namespace"

monoDependencyFolder    := file(System.getProperty("user.home")) / "code" / "dsl_compiler_client_user" / "revenj"

api := new com.dslplatform.compiler.client.ApiImpl(new com.dslplatform.compiler.client.api.core.impl.HttpRequestBuilderImpl(), new com.dslplatform.compiler.client.api.core.mock.HttpTransportMock(), new UnmanagedDSLImpl())
//api := new com.dslplatform.compiler.client.ApiImpl(new com.dslplatform.compiler.client.api.core.impl.HttpRequestBuilderImpl(), new com.dslplatform.compiler.client.api.core.mock.HttpTransportMock(), com.dslplatform.compiler.client.api.core.mock.UnmanagedDSLMock.mock_single_integrated)

databaseConnection := Map("ServerName" -> "localhost", "Port" -> "5434", "DatabaseName" -> "dccTest", "User" -> "dccTest", "Password" -> "dccTest")
