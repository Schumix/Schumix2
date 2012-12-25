#!/bin/sh
cd ArchLinux
makepkg -s -f
cp *pkg.tar.xz ../
