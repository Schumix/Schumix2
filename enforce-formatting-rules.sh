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

licence=`ls -d $PWD`/Share/doc/License.cs

main () {
  for f in $(find -iname "*.cs" | grep -v "./_ReSharper" | grep -v "/obj/" | grep -v "./External")
  do
    license $f
    license_update $f
  done

  for f in $(find -iname "*.sh" | grep -v "./_ReSharper" | grep -v "/obj/" | grep -v "./External")
  do
    license_sh_update $f
  done
}

license () {
  f=$1
  if (! grep -q "Copyright" $f) | (! grep -q "* Copyright" $f);
  then
    echo "Adding licence to $f"
    cat $licence $f > $f.new
    mv $f.new $f
  fi
}

license_update () {
  f=$1
  echo "Update licence to $f"
  find $f -type f -exec sed -i 's/* Copyright (C) 2013-2014 Schumix Team <http:\/\/schumix.eu\/>/* Copyright (C) 2013-2015 Schumix Team <http:\/\/schumix.eu\/>/g' {} \;
}

license_sh_update () {
  f=$1
  echo "Update licence to $f"
  find $f -type f -exec sed -i 's/# Copyright (C) 2013-2014 Schumix Team <http:\/\/schumix.eu\/>/# Copyright (C) 2013-2015 Schumix Team <http:\/\/schumix.eu\/>/g' {} \;
}

main
