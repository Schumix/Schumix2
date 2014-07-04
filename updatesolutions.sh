#!/bin/bash

main () {
  schumixvs_sln_update
  po_csproj_update
}

schumixvs_sln_update () {
  f=SchumixVS.sln
  echo "Update $f"

  if [ -e $f ]; then
    rm $f
  fi

  cp Schumix.sln $f

  find $f -type f -exec sed -i 's/Po.mdproj/Po.csproj/g' {} \;
}

po_csproj_update () {
  f=Po/Po.csproj
  echo "Update $f"

  if [ -e $f ]; then
    rm $f
  fi

  cp Po/Po.mdproj $f

  find $f -type f -exec sed -i 's/Po.mdproj/Po.csproj/g' {} \;
}

main
