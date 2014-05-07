##Revenj Mono Prototype

All included (prolly)

- publishLocal your version of plugin

- to regenerate the model go to the sbt console and write upgradeServerUnmanaged

- to compile the generatedModel.dll call deplyMono.sh

- set user to mono group
    ````sh
    usermod -a -G mono username
    ````

- or
    ````sh
    setfacl -Rdm 'u:marin:rwx' /var/mono
    setfacl -Rm 'u:marin:rwx' /var/mono
    ````

- cp dependencies
    ````sh
    install -o mono -g mono -m 750 dll/generatedModel.dll /var/mono/wwwroot/myFirstMono/
    find revenj -type f -exec sudo install -o mono -g mono -m 750 '{}' /var/mono/wwwroot/myFirstMono/bin/ \;
    ````

- start server
    ````sh
    /var/mono/wwwroot/myFirstMono/start.sh

    #!/bin/sh
    cd "$(dirname "$0")"/bin
    exec mono Revenj.Http.exe "$@" > ../logs/mono.log 2>&1
    bash /var/mono/wwwroot/myFirstMono/start.sh
    ````

- to be continued ...

