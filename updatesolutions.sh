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

main () {
  schumixvs_sln_update
  po_csproj_update
}

schumixvs_sln_update () {
  f=SchumixVS.sln
  echo "Update $f"

  if [ -e $f ]; then
    rm $f
  fi

  cp Schumix.sln $f

  find $f -type f -exec sed -i 's/Po.mdproj/Po.csproj/g' {} \;
}

po_csproj_update () {
  f=Po/Po.csproj
  echo "Update $f"

  if [ -e $f ]; then
    rm $f
  fi

  cp Po/Po.mdproj $f
}

main
