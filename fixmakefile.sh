#!/bin/bash
# 
# This file is part of Schumix.
# 
# Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
# Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
# 
# Schumix is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
# 
# Schumix is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
# 
# You should have received a copy of the GNU General Public License
# along with Schumix.  If not, see <http://www.gnu.org/licenses/>.
# 

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
