#!/bin/bash

rm -rf dll

mkdir  dll

./makeMonoBinaries.sh revenj monotemp dll/generatedModel.dll

echo Copying files to mono server.

install -o mono -g mono -m 750 dll/generatedModel.dll /var/mono/wwwroot/myFirstMono/bin

find revenj -type f -exec sudo install -o mono -g mono -m 750 '{}' /var/mono/wwwroot/myFirstMono/bin/ \;



