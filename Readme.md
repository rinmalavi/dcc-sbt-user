##Revenj Mono Prototype

- publishLocal your version of plugin

- set apps configuration in `Revenj.Http.exe.config`

- set user to mono group

    usermod -a -G mono <username>

- or

    setfacl -Rdm 'u:<username>:rwx' /var/mono
    setfacl -Rm 'u:<username>:rwx' /var/mono

run `upgradeMonoServer`

restart mono if the target path was `/var/mono/wwwroot/<app_name>` run `/etc/init.d/mono restart`

- or form `bin/` (where ever that might have been placed)

    mono Revenj.Http.exe |& less


####Alternativly
#####to generate dll

    cd csproj
    xbuild generatedModel.csproj
    install -g mono -m 750 ../revenj/Revenj.Http.exe.config /var/mono/wwwroot/<app_name>/bin/
    find bin/Debug -type f -exec sudo install -g mono -m 750 '{}' /var/mono/wwwroot/<app_name>/bin/ \;

- or if runScript.sh was left behind, but the rest failed, use that one

####To start the server

- start server

    /var/mono/wwwroot/<app_name>/start.sh

    #!/bin/sh
    cd "$(dirname "$0")"/bin
    exec mono Revenj.Http.exe "$@" > ../logs/mono.log 2>&1
    bash /var/mono/wwwroot/<app_name>/start.sh


- check to see `localhost:8999/Domain.svc/search/myModule.A`

- to be continued ...
