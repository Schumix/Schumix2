#!/bin/bash

git submodule update --init --recursive
xbuild /p:Configuration="Debug" Schumix.sln /flp:LogFile=xbuild.log;Verbosity=Detailed

cd Run/Tests/Debug
nunit-console Schumix.Framework.Test.dll
nunit-console Schumix.Irc.Test.dll
