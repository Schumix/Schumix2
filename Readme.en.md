# Schumix2

# Statement

This code is intended to relay its predecessor. It is lay new basis and got many newness.
More faster and reliable than Schumix. The code is written in c#.

# Platforms

The bot has compatibility with the windows and linux systems. It might be run on Mac OS X but it is not tested.
Under Windows the `.NET Framework 4.0` or higher is recommended.
Under Mono at least the `2.10` or higher is needed.

# Run source/Compile

## Windows

Monodevelop használatakor szükséges a nyelvi fájlok lefordításához a `gettext`. Töltsük le és telepítsük. `http://gnuwin32.sourceforge.net/packages/gettext.htm`
The compiling is simple. Open the `Schumix.sln` file. Choose the configuration that fits for you and compile it.

## Linux

Open the `Schumix.sln` file. Choose the configuration that fits for you and compile it.

## Linux terminal

Install the `mono-xbuild` package or from source. After run the `build.sh` command and the code is compiled.

# Code commissioning

Navigate to the `Run` folder and in that proper folder for the configuration. Run the exe. The program is generates its config file. If there will be some problem then create a `Configs` named folder and from the root folder copy in that folder the `Schumix.yml` named file.

# Install

Csak akkor használjuk ezt az opciót ha úgy szeretnénk telepíteni a botot mint ha be akarjuk telepíteni a rendszerbe. Figyelem! Rendszergazadi jog valószinüleg szükséges lesz a telepítés végső szakaszához.

## Archlinux

**Figyelem!!! Root jogra lesz szükség hozzá.**
Futtassuk `./createarchlinuxpkg.sh` parancsot. Ha lefutott megjelenik egy `schumix.pkg.tar.xz` (hasonló lesz a neve) nevű fájl. Ezt telepítsük a `sudo pacman -U schumix2.pkg.tar.xz` (csomag fájl neve hasonló lesz) paranccsl és már készen is vagyunk. A botot a `schumix` paranccsal futtathatjuk.

## Debian/Ubuntu

**Figyelem!!! Root jogra lesz szükség hozzá.**
Futtassuk `./createdebianpkg.sh` parancsot. Ha lefutott megjelenik egy `schumix.deb` nevű fájl. Ezt telepítsük a `sudo dbkg -i schumix.deb` paranccsl és már készen is vagyunk. A botot a `schumix` paranccsal futtathatjuk.

## Windows

Navigáljunk az Installer mappába. Futtassuk a `Schumix.iss` nevű fájlt. Ha lefutott kapunk egy `Setup.exe` nevű telepíthető állományt. Futtasuk és értelemszerüen telepítsük. A többit szerintem nem kell részletezni :)

# Config settings

A konfiguk beállításához menjünk a `Share/doc/Configs` mappába és a `Schumix.en.md` fájlt nyissuk meg. Ez a fő konfig leírása. Fontos hogy ezt a leírást mindenképpen olvassuk el mert enélkül nem lehet elindítani a botot normálisan. Ha megvagyunk akár a többi leírást is végiglehet nézni de az elindításhoz nem szükséges.

# Adatbázis beüzemelése

## MySql

Ha a jó öreg mysql alapú adatbázist szeretnénk használni állítsuk a konfig fájlban `(lásd: <MySql><Enabled>false</Enabled>)` az engedélyét true értékre.
Ezután az `Sql` mappából töltsük fel az adatbázisunkat. Ha bármiféle javítás jön a kódhoz vagy újítás nem kell az agész adatbázist újra töltenünk.
Csak az `Updates` mappából frisistsük a megfelelõ verzió szám alapján.

## SQLite

Ha az SQLite alapú adatbázist szeretnénk használni állítsuk a konfig fájlban `(lásd: <SQLite><Enabled>false</Enabled>)` az engedélyét `true` értékre.
Majd másoljuk az `Sql` mappából a `Schumix.db3` fájlt az exe mellé. Ezen fájl neve megváltoztatható de akkor a konfig fájlban is meg kell vátoztatni.
Természetesen az elérési út is a névvel együtt.

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
