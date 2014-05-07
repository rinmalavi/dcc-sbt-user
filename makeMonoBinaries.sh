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
	dependencies="$dependencies -r:$path"
done < <(find "$dependencies_folder" -iname '*.dll' -type f -print0)

sources=""
while IFS='' read -d $'\0' -r path; do
	sources="$sources $path"
done < <(find "$sources_folder" -iname '*.cs' -type f -print0)

cmd="mcs -out:$assembly_name -target:library $dependencies $sources"
$cmd
