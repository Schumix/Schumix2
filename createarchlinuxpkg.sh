#!/bin/sh
cd Archlinux
makepkg -s -f
cp *pkg.tar.xz ../
