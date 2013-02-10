#!/bin/sh

echo "Init SubModule"
git submodule update --init --recursive

sdir=`ls -d $PWD`
echo "Running autogen.sh"
sh autogen.sh
echo "Running configure"
./configure --prefix=/usr
echo "Running make"
make

echo "Locale make and install"
cd Po
make DESTDIR=$sdir/Run install
cd ..

fakeroot

rm -rf pkg
mkdir pkg
mkdir pkg/Share
cp -rf Share/share pkg/Share/
cp Share/changelog pkg/Share/
cp Share/postinst pkg/Share/
cp Share/postrm pkg/Share/
cp Share/schumix pkg/Share/
cp Share/schumix-server pkg/Share/
mkdir pkg/usr
mkdir pkg/usr/bin
mv pkg/Share/schumix pkg/usr/bin/
mv pkg/Share/schumix-server pkg/usr/bin/
mv pkg/Share/share pkg/usr/
mkdir pkg/usr/share/doc
mkdir pkg/usr/share/doc/schumix
cp License pkg/usr/share/doc/schumix/
mkdir pkg/usr/lib
mkdir pkg/usr/lib/pkgconfig
mkdir pkg/usr/lib/schumix
cp -rf Scripts pkg/usr/lib/schumix/Scripts
cd Run/Release/Addons

for file in *.pc
do
	echo $file;
	mv $file ../../../pkg/usr/lib/pkgconfig/$file
done

cd ..

for file in *.pc
do
	echo $file;
	mv $file ../../pkg/usr/lib/pkgconfig/$file
done

rm Config.exe Installer.exe Addons/Schumix.db3 Addons/sqlite3.dll Addons/System.Data.SQLite.dll Addons/MySql.Data.dll Addons/Schumix.Irc.dll Addons/Schumix.Api.dll Addons/Schumix.Framework.dll schumix.config schumix.installer schumix schumix.server
cp -rf ./ ../../pkg/usr/lib/schumix
cd ../../
cp -rf ./Run/usr/lib/schumix/locale pkg/usr/share/locale
cd pkg
#control file
cd Share
revision="$(echo $(cat ../../Core/Schumix.Framework/Config/Consts.cs) | grep -o 'SchumixVersion = ".*"; public const string SchumixFileVersion' | awk '{print substr($3, 2, length($3)-3)}')"
echo "Version: $revision"
echo "Package: schumix" >> control
echo "Version: $revision" >> control
echo "Architecture: all" >> control
echo "Maintainer: Csaba Jakosa <megax@yeahunter.hu>" >> control
echo "Depends: mono-devel (>= 2.10.8)" >> control
echo "Section: Schumix" >> control
echo "Priority: optional" >> control
echo "Description: Schumix IRC Bot and Framework" >> control

cd ..
cd usr
find . -exec md5sum '{}' \; > ../Share/md5sums
cd ..
mv Share DEBIAN
cd ..
#sudo chown -R root:root pkg
dpkg-deb --build pkg
#sudo rm -rf pkg
echo "mv pkg.deb schumix.deb"
mv pkg.deb schumix.deb
exit
echo "Success :)"
