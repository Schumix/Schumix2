#!/bin/bash

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
