#!/bin/bash

git submodule update --init --recursive

build_conf="$(echo "$1" | tr [:upper:] [:lower:])"

if [ "$2" = "" ]; then
	build_platform="AnyCPU"
else
	build_platform="$(echo "$2" | tr [:upper:] [:lower:])"
fi

if [ "$1" = "" ]; then
	xbuild /p:Configuration="Debug" /p:Platform=$build_platform Schumix.sln
else
	if [ $build_conf = "release" ]; then
		xbuild /p:Configuration="Release" /p:Platform=$build_platform Schumix.sln
	else
		xbuild /p:Configuration="Debug" /p:Platform=$build_platform Schumix.sln
	fi
fi
