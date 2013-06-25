#!/bin/bash

git submodule update --init --recursive

build_conf="$(echo "$1" | tr [:upper:] [:lower:])"

if [ "$2" = "" ]; then
	build_platform=""
else
	build_platform="$(echo "$2" | tr [:upper:] [:lower:])"
fi

if [ "$1" = "" ]; then
	xbuild /p:Configuration="Debug" Schumix.sln
else
	if [ "$build_platform" = "" ]; then
		if [ $build_conf = "release" ]; then
			xbuild /p:Configuration="Release" Schumix.sln /flp:LogFile=xbuild.log
		else
			xbuild /p:Configuration="Debug" Schumix.sln /flp:LogFile=xbuild.log
		fi
	else
		if [ $build_conf = "release" ]; then
			xbuild /p:Configuration="Release" /p:PlatformTarget=$build_platform Schumix.sln /flp:LogFile=xbuild.log
		else
			xbuild /p:Configuration="Debug" /p:PlatformTarget=$build_platform Schumix.sln /flp:LogFile=xbuild.log
		fi
	fi
fi
