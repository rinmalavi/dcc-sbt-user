##Revenj prototype deploy Help

version `700`

install mono

log in as user `mono`

    su - mono -s $SHELL
    
cd to your projects directory and run:

    sbt upgradeCSharpServer

restart mono server

    sudo /etc/init.d/mono restart

- check at `http://<hostname>/Domain.svc/search/<packagename.some_root_name>`

customize config at: `Revenj.Http.exe.config`
