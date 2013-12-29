#!/bin/bash

licence=`ls -d $PWD`/Share/doc/License.cs

main () {
  for f in $(find -iname "*.cs" | grep -v "./_ReSharper" | grep -v "/obj/" | grep -v "./External")
  do
    license $f
    license_update $f
  done
}

license () {
  f=$1
  if (! grep -q "Copyright" $f) | (! grep -q "* Copyright" $f);
  then
    echo "Adding licence to $f"
    cat $licence $f > $f.new
    mv $f.new $f
  fi
}

license_update () {
  f=$1
  echo "Update licence to $f"
  find $f -type f -exec sed -i 's/* Copyright (C) 2013 Schumix Team <http:\/\/schumix.eu\/>/* Copyright (C) 2013-2014 Schumix Team <http:\/\/schumix.eu\/>/g' {} \;
}

main
