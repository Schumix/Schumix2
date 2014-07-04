#!/bin/bash

main () {
  schumixvs_sln_update
}

schumixvs_sln_update () {
  f=SchumixVS.sln
  echo "Update $f"

  if [ -e $f ]; then
    rm SchumixVS.sln
  fi

  cp Schumix.sln $f

  find $f -type f -exec sed -i 's/Po.mdproj/Po.csproj/g' {} \;
}

main
