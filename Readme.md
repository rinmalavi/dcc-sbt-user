### Revenj prototype deploy Help

install mono

log in as user `mono`

    su - mono -s $SHELL
    
cd to your projects directory and run:

    sbt upgradeCSharpServer

restart mono server

    sudo /etc/init.d/mono restart

check at `http://<hostname>/Domain.svc/search/<packagename.some_root_name>`

customize config at: `Revenj.Http.exe.config`

#### Minimal build settings contain
1.1 `projectPropsPath` set to an option of a file looking something like this:

    dsl {
      username=<your username @ dsl-platform.com>,
      password=<your password for this username,
      projectId=<optional projectId for some tests>
      package-name=<namespace of target sources>
    }
    db {
      ServerName=x,
      Port=x,
      DatabaseName=x,
      User=x,
      Password=x
    }
    
db part is and projectId are optional depending on a type of a project.

1.2 If `projectPropsPath` is not set then `username`, `password` and optional `projectId` can be set like this (in build.sbt):

    val credentials = com.typesafe.config.ConfigFactory.parseFile(file(System.getProperty("user.home")) / ".config" / "dsl-compiler-client" / "test.credentials")
    
    username := credentials.getString("dsl.username")
   
    password := credentials.getString("dsl.password")
   
    packageName := "namespace"  // if you wouldn't like to have the default namespace of "model".
    
    databaseConnection := Map( 
        "ServerName"-><ServerNameStringValue>, 
        "Port"-><PortStringValue>, 
        "DatabaseName"-><DatabaseNameStringValue>, 
        "User"-><UserStringValue>, 
        "Password"=<PasswordStringValue>)

2.1 Unmanaged projects need `revenj` in case you decide to compile or deploy with a plugin. Following keys must be set:

    monoDependencyFolder    := file(System.getProperty("user.home")) / "code" / "dsl_compiler_client_user" / "revenj"
   
    performDatabaseMigration  := false // or true if you would like a database to be upgraded
    
    performServerDeploy := true

this will probably change!

3.1 `targetSources` are by default sent to Set(Scala). Java, PHP, C# can also be added as client code.

## Use
