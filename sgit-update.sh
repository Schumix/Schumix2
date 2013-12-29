#!/bin/bash

git submodule update --init --recursive

cd External/SGit
./build.sh
cd Run/Release

if [ ! -e ../../../../Dependencies/SGit ]; then
	mkdir ../../../../Dependencies/SGit
fi

for file in *.dll
do
	echo $file;
	cp $file ../../../../Dependencies/SGit/$file
done

echo "SGit.exe";
cp SGit.exe ../../../../Dependencies/SGit/SGit.exe
echo "Done";
