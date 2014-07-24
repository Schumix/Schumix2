#!/bin/sh
path_to_submodule=$1
git config -f .gitmodules --remove-section submodule.$path_to_submodule
git config -f .git/config --remove-section submodule.$path_to_submodule
git rm --cached $path_to_submodule
git commit -m "Remove submodule $path_to_submodule"
rm -rf $path_to_submodule
rm -rf .git/modules/$path_to_submodule
