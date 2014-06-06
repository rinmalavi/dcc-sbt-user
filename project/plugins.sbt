val NGSNexus = "NGS Nexus" at "http://ngs.hr/nexus/content/groups/public/"
val NGSReleases = "NGS Releases" at "http://ngs.hr/nexus/content/repositories/releases/"
val NGSSnapshots = "NGS Snapshots" at "http://ngs.hr/nexus/content/repositories/snapshots/"
val NGSPrivateReleases = "NGS Private Releases" at "http://ngs.hr/nexus/content/repositories/releases-private/"
val NGSPrivateSnapshots = "NGS Private Snapshots" at "http://ngs.hr/nexus/content/repositories/snapshots-private/"

resolvers in ThisBuild += "NGS Nexus" at "http://ngs.hr/nexus/content/groups/public/"


addSbtPlugin("com.dslplatform" % "dsl-compiler-client-sbt" % "0.20.0-SNAPSHOT" )
