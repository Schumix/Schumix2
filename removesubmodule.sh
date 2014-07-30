#!/bin/sh
# 
# This file is part of Schumix.
# 
# Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
# Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
# 
# Schumix is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
# 
# Schumix is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
# 
# You should have received a copy of the GNU General Public License
# along with Schumix.  If not, see <http://www.gnu.org/licenses/>.
# 

path_to_submodule=$1
git config -f .gitmodules --remove-section submodule.$path_to_submodule
git config -f .git/config --remove-section submodule.$path_to_submodule
git rm --cached $path_to_submodule
git commit -m "Remove submodule $path_to_submodule"
rm -rf $path_to_submodule
rm -rf .git/modules/$path_to_submodule
