#!/bin/bash
xbuild \
  /p:DocumentationFile="" \
  /p:DefineConstants="DEBUG,MONO" \
  /p:Configuration="Mono-Debug" \
  /p:Platform="x86" \
  Schumix.sln
