#!/bin/bash
# 
# This file is part of Schumix.
# 
# Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
# Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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

consts_cs="Core/Schumix.Framework/Config/Consts.cs"

if [ "$1" = "" ]; then
  newversion="$(echo $(cat $consts_cs) | grep -o 'SchumixVersion = ".*"; public const string SchumixFileVersion' | awk '{print substr($3, 2, length($3)-3)}')"
else
  newversion="$1"
  f=$consts_cs
  echo "Update Consts.cs file: $f"
  find $f -type f -exec sed -i 's/public const string SchumixVersion = ".*"/public const string SchumixVersion = "'$newversion'"/g' {} \;
  find $f -type f -exec sed -i 's/public const string SchumixFileVersion = ".*"/public const string SchumixFileVersion = "'$newversion'.0"/g' {} \;
fi

main () {
  echo "New version: $newversion"

  fv="Version"
  echo "Update Version file."
  find $fv -type f -exec sed -i 's/.*/'$newversion'/g' {} \;

  for f in $(find -iname "*.sln" | grep -v "./_ReSharper" | grep -v "/obj/" | grep -v "./External")
  do
    sln_update $f
  done

  for f in $(find -iname "*.csproj" | grep -v "./_ReSharper" | grep -v "/obj/" | grep -v "./External")
  do
    csproj_update $f
  done

  for f in $(find -iname "*.mdproj" | grep -v "./_ReSharper" | grep -v "/obj/" | grep -v "./External")
  do
    mdproj_update $f
  done

  for f in $(find -iname "configure.ac" | grep -v "./_ReSharper" | grep -v "/obj/" | grep -v "./External")
  do
    configure_ac_update $f
  done

  for f in $(find -iname "*.pc.in" | grep -v "./_ReSharper" | grep -v "/obj/" | grep -v "./External")
  do
    pc_in_update $f
  done

  for f in $(find -iname "PKGBUILD" | grep -v "./_ReSharper" | grep -v "/obj/" | grep -v "./External")
  do
    PKGBUILD_update $f
  done

  for f in $(find -iname "*.iss" | grep -v "./_ReSharper" | grep -v "/obj/" | grep -v "./External")
  do
    iss_update $f
  done
}

sln_update () {
  f=$1
  echo "Update sln file: $f"
  find $f -type f -exec sed -i 's/version = .*/version = '$newversion'/g' {} \;
}

csproj_update () {
  f=$1
  echo "Update csproj file: $f"
  find $f -type f -exec sed -i 's/<ReleaseVersion>.*<\/ReleaseVersion>/<ReleaseVersion>'$newversion'<\/ReleaseVersion>/g' {} \;
}

mdproj_update () {
  f=$1
  echo "Update mdproj file: $f"
  find $f -type f -exec sed -i 's/<ReleaseVersion>.*<\/ReleaseVersion>/<ReleaseVersion>'$newversion'<\/ReleaseVersion>/g' {} \;
}

configure_ac_update () {
  f=$1
  echo "Update configure.ac file: $f"
  find $f -type f -exec sed -i 's/AC_INIT(\[Schumix\], \[.*\])/AC_INIT(\[Schumix\], \['$newversion'\])/g' {} \;
}

pc_in_update () {
  f=$1
  echo "Update pc.in file: $f"
  find $f -type f -exec sed -i 's/Version: .*/Version: '$newversion'/g' {} \;
}

PKGBUILD_update () {
  f=$1
  echo "Update PKGBUILD file: $f"
  find $f -type f -exec sed -i 's/pkgver=".*"/pkgver="'$newversion'"/g' {} \;
  find $f -type f -exec sed -i 's/_pkgverclassic=".*"/_pkgverclassic="'$newversion'"/g' {} \;
}

iss_update () {
  f=$1
  echo "Update iss file: $f"
  find $f -type f -exec sed -i 's/MyAppVersion ".*"/MyAppVersion "'$newversion'"/g' {} \;
}

main
