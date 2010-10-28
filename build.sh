#!/bin/sh
chmod +x ./extract_svn_revision.sh && ./extract_svn_revision.sh
mv ./svn_revision.cs ./schumix/svn_revision.cs
xbuild schumix.sln
