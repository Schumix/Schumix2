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

Dependencies="../../../../Dependencies/SGit"

git submodule update --init --recursive

cd External/SGit
./build.sh
cd Run/Release

if [ ! -e $Dependencies ]; then
	mkdir $Dependencies
fi

for file in *.dll
do
	echo $file;
	cp $file $Dependencies/$file
done

echo "SGit.exe";
cp SGit.exe $Dependencies/SGit.exe
echo "Done";
