#!/bin/bash

main () {
  for f in $(find -iname "*.am" | grep -v "./_ReSharper" | grep -v "/obj/" | grep -v "./External")
  do
    file_update $f
  done

  configure_ac_update
  makefile_am_update
}

configure_ac_update () {
  f=configure.ac
  echo "Update $f"
  find $f -type f -exec sed -i 's/External\/YamlDotNet\/YamlDotNet\/Makefile/External\/YamlDotNet\/YamlDotNet\/Makefile\nPo\/Makefile/g' {} \;
}

makefile_am_update () {
  f=Makefile.am
  echo "Update $f"
  find $f -type f -exec sed -i 's/External\/YamlDotNet\/YamlDotNet /External\/YamlDotNet\/YamlDotNet Po /g' {} \;
}

file_update () {
  f=$1
  echo "Update file: $f"

  # Mono.Posix.dll
  find $f -type f -exec sed -i 's/..\/..\/..\/..\/Mono.Posix/..\/..\/Dependencies\/Mono.Posix.dll/g' {} \;
  find $f -type f -exec sed -i 's/..\/..\/Mono.Posix/Dependencies\/Mono.Posix.dll/g' {} \;

  # Mono.Security.dll
  find $f -type f -exec sed -i 's/..\/..\/Mono.Security/Dependencies\/Mono.Security.dll/g' {} \;

  # IronPython.Modules.dll
  find $f -type f -exec sed -i 's/..\/..\/..\/..\/IronPython.Modules/..\/..\/Dependencies\/IronPython.Modules.dll/g' {} \;
  find $f -type f -exec sed -i 's/..\/..\/IronPython.Modules/Dependencies\/IronPython.Modules.dll/g' {} \;

  # IronPython.dll
  find $f -type f -exec sed -i 's/..\/..\/..\/..\/IronPython/..\/..\/Dependencies\/IronPython.dll/g' {} \;
  find $f -type f -exec sed -i 's/..\/..\/IronPython/Dependencies\/IronPython.dll/g' {} \;

  # Microsoft.Dynamic.dll
  find $f -type f -exec sed -i 's/..\/..\/..\/..\/Microsoft.Dynamic/..\/..\/Dependencies\/Microsoft.Dynamic.dll/g' {} \;
  find $f -type f -exec sed -i 's/..\/..\/Microsoft.Dynamic/Dependencies\/Microsoft.Dynamic.dll/g' {} \;

  # Microsoft.Scripting.Metadata.dll
  find $f -type f -exec sed -i 's/..\/..\/..\/..\/Microsoft.Scripting.Metadata/..\/..\/Dependencies\/Microsoft.Scripting.Metadata.dll/g' {} \;
  find $f -type f -exec sed -i 's/..\/..\/Microsoft.Scripting.Metadata/Dependencies\/Microsoft.Scripting.Metadata.dll/g' {} \;

  # Microsoft.Scripting.dll
  find $f -type f -exec sed -i 's/..\/..\/..\/..\/Microsoft.Scripting/..\/..\/Dependencies\/Microsoft.Scripting.dll/g' {} \;
  find $f -type f -exec sed -i 's/..\/..\/Microsoft.Scripting/Dependencies\/Microsoft.Scripting.dll/g' {} \;
}

main
