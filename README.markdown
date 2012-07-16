# Schumix2

# Ismertetés

Ezen kód hívatott felváltani elõdjét. Új alapokra lett helyezve és több újdonságot is tartalmaz.
Schumixxal szemben gyorsabb és megbízhatobb. A kód c# nyelven írodott.

# Platformok

A bot kompatibilis a windows és linux rendszerrel. Valószínüleg Mac OS X-en is fut de még nem került tesztelésre.
Windows alatt ajálott a `.net 4.0` használata vagy újabb.
Monon jelenleg minimum követelmény a `2.10` vagy újabb.

# Fordítása

## Windows
A fordítás egyszerû. Nyissuk meg a `Schumix.sln` fájlt. Válaszuk ki a nekünk megfelelõ konfigurációt és fordítsuk le vele.

## Linux
Nyissuk meg a `Schumix.sln` fájlt. Válaszuk ki a nekünk megfelelõ konfigurációt és fordítsuk le vele.

## Linux terminál
Telepítsük a `mono-xbuild` csomagot vagy forrásból. Ezután inditsuk el a `build.sh`-t és lefordul a kód.

# Kód üzembehelyezése

Navigáljunk a `Run` mappába és azon belül a konfigurációnak megfelelõ mappába. Inditusk el az exe-t. A program legenerálja
önmagának a konfig fájlt. Ha bármi gond lenne vele hozzunk létre egy `Configs` nevû mappát és a fõ mappából másoljuk bele
a `Schumix.xml` nevû fájlt.

# Konfig beállítása

## Server

* **Enabled:** Értéke `true` vagy `false` lehet. Meghatározza hogy kapcsolodjon-e a szerverhez a program. Alapértelmezés: `false`
* **Host:** Ide kell beírni a szerver nevét/címét ahova csatlakozni szeretnénk.
* **Port:** A szerver portja. Alapértelmezés: `35220`
* **Password:** A szerver azonosító jelszava.

## Irc

Ha több szerverre szeretnék felkapcsolódni vagy egyre többször akkor az egész irc részt (<Irc> ... </Irc>) le kell másolni még egyszer és ott külön be kell állítani az adatokat.
* **ServerName:** A szerver neve. Ezzel lehet beállítani hogy többszerveres módban hogy mi legyen az egyes szervereket megkülönböztető név. FIGYELEM: Nem egyezhet meg a többi szerver nevével (kis és nagybetüt nem különbőzteti meg)!
* **Server:** Ide kell beírni a szerver nevét ahova csatlakozni szeretnénk.
* **Port:** A szerver portja. Alapértelmezés: `6667`
* **Ssl:** Értéke `true` vagy `false` lehet. Ezzel aktiválható a kapcsolódás olyan irc szerverre ahol ssl protokol van használva. Alapértelmezés: `false`
* **NickName:** Elsõdleges név.
* **NickName2:** Másodlagos név.
* **NickName3:** Harmadlagos név.
* **UserName:** Felhasználó név.
* **UserInfo:** Információ a felhasználóról.
* **MasterChannel:**
    * **Name:** Elsõdleges csatorna ahova csatlakozik minden esetben a bot. Ennek a neve itt változtatható meg. Az adatbázisból nem törölhetõ.
    * **Password:** Az elsődleges csatornához tartozó jelszó.
                    Alapértelmezés: (semmi)[Ez azt jelenti hogy nem add meg jelszót az elsődleges csatornához.]
* **IgnoreChannels:** Letilthatók a nem kívánatos csatornák vele. Ami itt szerepel oda nem megy fel a bot. Ezen rész letiltja a bot rendszerében szereplõket is.
                      Tehát ha abból nem akarunk valahova felmenni akkor is használhatjuk ezt törlés helyett. Vesszõvel elválasztva kell egymás útán írni öket.
                      `pl: #teszt,#teszt2 vagy szimplán #teszt`
* **IgnoreNames:** Letilthatóak vele a nem kívánatos személyek. Így csak az használhatja a botot aki megérdemli.
                   `pl: schumix,schumix2 vagy szimplán schumix`
* **NickServ:**
    * **Enabled:** Értéke `true` vagy `false` lehet. Ezen rész határozza meg hogy a nickhez tartozó jelszó el legyen-e küldve. true = igen, false = nem.
                   Alapértelmezés: false
    * **Password:** Nickhez tartozó jelszó.
* **HostServ:**
    * **Enabled:** Értéke `true` vagy `false` lehet. Ezen rész határózza meg hogy ha van a nickhez vhost akkor bekapcsolodjon-e. Alapértelmezés: `false`
                   Mert ha nincs akkor megjelenitödhet az ip ezért olyankor ajánlott false értékre tenni.
    * **Vhost:** Értéke `true` vagy `false` lehet. Ezen rész határózza meg hogy a nickhez tartotó vhost aktiválásra kerüljön-e. Alapértelmezés: `false`
* **Wait:**
    * **MessageSending:** Üzenet küldésének késleltetése. Legföbbként flood ellen van.
* **Command:**
    * **Prefix:** A parancsok elõjele. Alapértelmezés: `$` (Fõ parancs xbot. Ezzel a parancselõjelel így néz ki: `$xbot`)
* **MessageType:** Értéke `Privmsg` vagy `Notice` lehet. Meghatározza hogy milyen formában küldje az üzeneteket a szerver felé. Alapértelmezés: `Privmsg`

## Log

* **FileName:** Meghatározza hova mentődjenek el a log információk. Alapértelmezés: `Schumix.log`
* **LogLevel:** Meghatározza hogy a konzolba milyen üzenetek kerülnek kiírásra. Alapértelmezés: `2`
    * **Szintjei:** `0` (Normális üzenetek és a sikeresek)
                    `1` (Figyelmeztetések)
                    `2` (Hibák)
                    `3` (Hibakeresõ üzenetek)
* **LogDirectory:** A log üzenetek mentése abba a mappába ami megvan adva. Alapértelmezés: `Logs`
* **IrcLogDirectory:** Az irc csatornák és egyéb üzenetének mentése abba a mappába ami megvan adva. Alapértelmezés: `Csatornak`
* **IrcLog:** Értéke `true` vagy `false` lehet. Meghatározza hogy a konzolba kiirásra kerülhetnek-e az irc-tõl jövõ üzenetek. Alapértelmezés: `false`

## MySql

* **Enabled:** Értéke `true` vagy `false` lehet. Meghatározza hogy mysql alapú-e az adatbázis. Alapértelmezés: `false`
* **Host:** A mysql szerver címe.
* **User:** A szerver felhasználó neve.
* **Password:** A szerver jelszava.
* **Database:** Az adatbázis amiben megtalálhatók a bothoz tartozó táblák.
* **Charset:** Az adatbázisba menõ adatok kódolását és olvasását határozza meg.
               Alapértelmezés: `utf8`

## SQLite

* **Enabled:** Értéke `true` vagy `false` lehet. Meghatározza hogy sqlite alapú-e az adatbázis. Alapértelmezés: `false`
* **FileName:** Az sqlite fájl neve.

## Addons

* **Enabled:** Értéke `true` vagy `false` lehet. Engedélyezi az addonok betöltését. Alapértelmezés: `true`
* **Ignore:** Azon addonok melyeket nem szeretnénk inditáskor betölteni. Vesszõvel elválasztva kell egymás útán írni öket. `(pl: TesztAddon,Teszt2Addon vagy szimplán TesztAddon)`
              Alapértelmezés: `MantisBTRssAddon,SvnRssAddon,GitRssAddon,HgRssAddon,WordPressRssAddon,TesztAddon`
* **Directory:** Az addonok mappája ahol tárolva vannak és ahonnét betöltésre kerülnek. Alapértelmezés: `Addons`

## Scripts

* **Lua:** Értéke `true` vagy `false` lehet. Engedélyezi a lua fájlok betöltését. Alapértelmezés: `true`
* **Directory:** A script-ek mappája ahol tárolva vannak és ahonnét betöltésre kerülnek. Alapértelmezés: `Scripts`

## Localization

* **Locale:** Meghatározza hogy a kód milyen nyelven fusson. (csak az irc és konzol parancsokra vonatkozik)
              Alapértelmezés: `enUS`

## Update

* **Enabled:** Értéke `true` vagy `false` lehet. Engedélyezi az automatikus frissítést. Alapértelmezés: `false`
* **Version:** Meghatározza melyik verzióra szeretnénk frissíteni. Current vagy stable lehet. A current az utolsó verzó ami a tárolóban van a stable pedig az utolsó stabil verzió.
               Alapértelmezés: `stable`
* **Branch:** Beállítható vele az ág (branch). Ez csak a current verziók esetében érdekes. Alapértelmezés: `master`
* **WebPage:** A megadott weboldalcímről tölti le a frissítéseket. Alapértelmezés: `http://megax.uw.hu/Schumix2/`

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
* És végül a konzol parancsok. Ha már megemlítettem ;) Szóval a lista a help parancsal kapható meg.
  Többit ki kell tapasztalni mert egyenlõre nincs hozzá help.
* Bármi lemaradt volna tudok segítséget nyújtani az irc.rizon.net szerveren a `#schumix, #schumix2` vagy `#hun_bot` csatornán.
* Remélem meg fog tetszeni a bot :)
