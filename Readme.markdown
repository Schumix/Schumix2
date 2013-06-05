# Schumix2 [![Build Status](https://travis-ci.org/Schumix/Schumix2.png?branch=master)](https://travis-ci.org/Schumix/Schumix2)

# Ismertetés

Ezen kód hívatott felváltani elõdjét. Új alapokra lett helyezve és több újdonságot is tartalmaz.
Schumixxal szemben gyorsabb és megbízhatobb. A kód c# nyelven írodott.

# Platformok

A bot kompatibilis a windows és linux rendszerrel. Valószínüleg Mac OS X-en is fut de még nem került tesztelésre.
Windows alatt ajálott a `.net 4.0` használata vagy újabb.
Monon jelenleg minimum követelmény a `2.10.8.1` vagy újabb.

# Kód futtatása/Fordítása

## AlModulok

Hiányzó almodulok letöltése: git submodule update --init --recursive

## Windows

Monodevelop használatakor szükséges a nyelvi fájlok lefordításához a `gettext`.
<br/>Töltsük le és telepítsük. `http://gnuwin32.sourceforge.net/packages/gettext.htm`
<br/>A fordítás egyszerû. Nyissuk meg a `Schumix.sln` fájlt.
<br/>Válaszuk ki a nekünk megfelelõ konfigurációt és fordítsuk le vele.

## Linux

Nyissuk meg a `Schumix.sln` fájlt.
<br/>Válaszuk ki a nekünk megfelelõ konfigurációt és fordítsuk le vele.

## Linux terminál

Telepítsük a `mono-xbuild` csomagot vagy forrásból a `mono`-t.
<br/>Ezután inditsuk el a `build.sh`-t és lefordul a kód.

# Kód üzembehelyezése

Navigáljunk a `Run` mappába és azon belül a konfigurációnak megfelelõ mappába.
<br/>Indítusk el az exe-t.
<br/>A program legenerálja önmagának a konfig fájlt.
<br/>Ha bármi gond lenne vele hozzunk létre egy `Configs` nevû mappát és a fõ mappából másoljuk bele a `Schumix.yml` nevû fájlt.

# Telepítés

Csak akkor használjuk ezt az opciót ha úgy szeretnénk telepíteni a botot mint ha be akarjuk telepíteni a rendszerbe.
<br/>Figyelem! Rendszergazadi jog valószinüleg szükséges lesz a telepítés végső szakaszához.

## Archlinux

**Figyelem!!! Root jogra lesz szükség hozzá.**
<br/>Futtassuk `./createarchlinuxpkg.sh` parancsot.
<br/>Ha lefutott megjelenik egy `schumix.pkg.tar.xz` (hasonló lesz a neve) nevű fájl.
<br/>Ezt telepítsük a `sudo pacman -U schumix2.pkg.tar.xz` (csomag fájl neve hasonló lesz) paranccsal és már készen is vagyunk.
<br/>A botot a `schumix` paranccsal futtathatjuk.

## Debian/Ubuntu

**Figyelem!!! Root jogra lesz szükség hozzá.**
<br/>Első lépés hogy átlépünk fakeroot módba.
<br/>Írjuk be hogy `fakeroot`.
<br/>Ezután futtassuk `./createdebianpkg.sh` parancsot.
<br/>Ha lefutott megjelenik egy `schumix.deb` nevű fájl.
<br/>Ezt telepítsük a `sudo dbkg -i schumix.deb` paranccsal és már készen is vagyunk. A botot a `schumix` paranccsal futtathatjuk.

## Windows

Navigáljunk az Installer mappába.
<br/>Futtassuk a `Schumix.iss` nevű fájlt.
<br/>Ha lefutott kapunk egy `Setup.exe` nevű telepíthető állományt.
<br/>Futtasuk és értelemszerüen telepítsük.
<br/>A többit szerintem nem kell részletezni :)

# Konfig beállítása

A konfiguk beállításához menjünk a `Share/doc/Configs` mappába és a `Schumix.md` fájlt nyissuk meg.
<br/>Ez a fő konfig leírása.
<br/>Fontos hogy ezt a leírást mindenképpen olvassuk el mert enélkül nem lehet elindítani a botot normálisan.
<br/>Ha megvagyunk akár a többi leírást is végiglehet nézni de az elindításhoz nem szükséges.

# Adatbázis beüzemelése

## MySql

Ha a jó öreg mysql alapú adatbázist szeretnénk használni állítsuk a konfig fájlban `(lásd: <MySql><Enabled>false</Enabled>)` az engedélyét true értékre.
<br/>Ezután az `Sql` mappából töltsük fel az adatbázisunkat.
<br/>Ha bármiféle javítás jön a kódhoz vagy újítás nem kell az agész adatbázist újra töltenünk.
<br/>Csak az `Updates` mappából frisistsük a megfelelõ verzió szám alapján.

## SQLite

Ha az SQLite alapú adatbázist szeretnénk használni állítsuk a konfig fájlban `(lásd: <SQLite><Enabled>false</Enabled>)` az engedélyét `true` értékre.
<br/>Majd másoljuk az `Sql` mappából a `Schumix.db3` fájlt az exe mellé.
<br/>Ezen fájl neve megváltoztatható de akkor a konfig fájlban is meg kell vátoztatni.
<br/>Természetesen az elérési út is a névvel együtt.

# Figyelmeztetés!

**Csak egy adatbázis lehet aktiv. Olyan nem lehet hogy egyse vagy kettõ. Ezekben az esetekben a kód leáll és nem fut tovább.**

Ha mind ezekkel megvagyunk már csak inditanuk kell és használni a kódot :)

# Apróságok

* Az álltalam készített addonokhoz tartoznak álltalában konfig fájlok. Ezek is szintén a `Configs` mappába generálodnak és ott állíthatóak be.
* A kódban továbbá elhelyezésre került egy bot parancs is. Ez a parancs az elsõdleges névbõl tevõdik össze. Példa rá: `schumix2, help`
  Fontos a parancs szerkezete `<elsõdleges nick>, parancs`
* A kódhoz ki és be kapcsolható funkciók tartoznak. Ezt csak is gizárólag admin vezérelheti.
  Funkciók használatáról leírás: `$help function` (természetesen az az elõjel kell ami megadásra került a konfigban)
* Ha már megemlítésre került. Az admin hozzáadása konzolból történik elõször. `admin add <admin neve>`
  Majd amit kapunk jelszót privát üzenetben el kell küldeni a botnak a jelszót ezen módon: `$admin access <jelszó>`
  Ha másikat szeretnénk akkor: `$admin newpassword <régi> <új>`
* És végül a konzol parancsok. Ha már megemlítettem ;) Szóval a lista a help paranccsal kapható meg.
  Többit pedig úgy kell mint az irc paranccsoknál. A parancs nevét vagy a parancsot és annak alparancsát kell a help parancs mögé írni.
* Bármi lemaradt volna tudok segítséget nyújtani az irc.rizon.net szerveren a `#schumix, #schumix2` vagy `#hun_bot` csatornán.
* Yaml konfignál minden olyan adatot amely különleges karaktert tartalmaz (pl: #) azt idézőjelek közé kell helyezni "" mert a program máskülönben megpróbálná értelmezni és az hibát okozna.
* Remélem meg fog tetszeni a bot :)
