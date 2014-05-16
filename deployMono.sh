#!/bin/bash

rm -rf dll

mkdir  dll

./makeMonoBinaries.sh /home/oke/code/dsl_compiler_client_user/revenj mono_src_tmp dll/generatedModel.dll

echo Copying files to mono server.

install -g mono -m 750 dll/generatedModel.dll /var/mono/wwwroot/myFirstMono/bin

find revenj -type f -exec install -g mono -m 750 '{}' /var/mono/wwwroot/myFirstMono/bin/ \;
