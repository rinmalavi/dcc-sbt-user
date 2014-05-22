resolvers in ThisBuild += "NGS Snapshots" at "http://ngs.hr/nexus/content/repositories/snapshots/"

addSbtPlugin("com.dslplatform" % "dsl-compiler-client-sbt" % "0.20.0-SNAPSHOT" classifier "tests" classifier "")
