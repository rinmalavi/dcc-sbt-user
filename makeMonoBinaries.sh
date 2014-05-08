#!/bin/bash

test "x$#" = "x3" || {
	echo "Usage: $0 dependencies_folder sources_folder assembly_name"
	exit 255
}

dependencies_folder="$1"
sources_folder="$2"
assembly_name="$3"

dependencies="-r:System.ComponentModel.Composition -r:System -r:System.Data -r:System.Xml -r:System.Runtime.Serialization -r:System.Configuration -r:System.Drawing"
while IFS='' read -d $'\0' -r path; do
	dependencies="$dependencies -r:${path#./}"
done < <(cd "$dependencies_folder" && find "." -iname '*.dll' -type f -print0)

sources="-recurse:$sources_folder/*.cs"

cmd="mcs -out:$assembly_name -target:library -lib:$dependencies_folder $dependencies $sources"
$cmd
