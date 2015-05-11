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
# Autobuilder

git submodule update --init --recursive

build_conf="$(echo "$1" | tr [:upper:] [:lower:])"

if [ "$2" = "" ]; then
	build_platform=""
else
	build_platform="$(echo "$2" | tr [:upper:] [:lower:])"
fi

if [ "$1" = "" ]; then
	xbuild /p:Configuration="Release" Schumix.sln /flp:LogFile=xbuild.log;Verbosity=Detailed
else
	if [ "$build_platform" = "" ]; then
		if [ $build_conf = "debug" ]; then
			xbuild /p:Configuration="Debug" Schumix.sln /flp:LogFile=xbuild.log;Verbosity=Detailed
		else
			xbuild /p:Configuration="Release" Schumix.sln /flp:LogFile=xbuild.log;Verbosity=Detailed
		fi
	else
		if [ $build_conf = "debug" ]; then
			xbuild /p:Configuration="Debug" /p:PlatformTarget=$build_platform Schumix.sln /flp:LogFile=xbuild.log;Verbosity=Detailed
		else
			xbuild /p:Configuration="Release" /p:PlatformTarget=$build_platform Schumix.sln /flp:LogFile=xbuild.log;Verbosity=Detailed
		fi
	fi
fi
