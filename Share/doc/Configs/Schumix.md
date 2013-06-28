# Schumix konfig fájl

## Server

* **Enabled:** Értéke `true` vagy `false` lehet. Meghatározza hogy kapcsolodjon-e a szerverhez a program. Alapértelmezés: `false`
* **Host:** Ide kell beírni a szerver nevét/címét ahova csatlakozni szeretnénk.
* **Port:** A szerver portja. Alapértelmezés: `35220`
* **Password:** A szerver azonosító jelszava. Alapértelmezés: `schumix`

## Irc

Yaml konfignál:<br/>
Ha több szerverre szeretnék felkapcsolódni vagy egyre többször akkor az egész irc részt (Irc: ...) le kell másolni még egyszer és ott külön be kell állítani az adatokat valamint Irc(szám) ként kell megadni. Pl: Irc2: .... (ide pedig minde úgy jön utána ahogy volt csak az Irc-nél kell átírni az újat).
Xml konfignál:<br/>
Ha több szerverre szeretnék felkapcsolódni vagy egyre többször akkor az egész irc részt (`<Irc> ... </Irc>`) le kell másolni még egyszer és ott külön be kell állítani az adatokat.
* **ServerName:** A szerver neve. Ezzel lehet beállítani hogy többszerveres módban hogy mi legyen az egyes szervereket megkülönböztető név. FIGYELEM: Nem egyezhet meg a többi szerver nevével (kis és nagybetüt nem különbőzteti meg)!
* **Server:** Ide kell beírni a szerver nevét ahova csatlakozni szeretnénk.
* **Password:** A szerver jelszavát lehet vele beállítani. Ha üresen van hagyva akkor úgy veszi mintha nem kellene jelszó a szerverhez.
* **Port:** A szerver portja. Alapértelmezés: `6667`
* **ModeMask:** A maskot lehet vele beállítani. Alapértelmezés: `8`
* **Ssl:** Értéke `true` vagy `false` lehet. Ezzel aktiválható a kapcsolódás olyan irc szerverre ahol ssl protokol van használva. Alapértelmezés: `false`
* **NickName:** Elsõdleges név.
* **NickName2:** Másodlagos név.
* **NickName3:** Harmadlagos név.
* **UserName:** Felhasználó név.
* **UserInfo:** Információ a felhasználóról.
* **MasterChannel:**
    * **Name:** Elsõdleges csatorna ahova csatlakozik minden esetben a bot. Ennek a neve itt változtatható meg. Az adatbázisból nem törölhetõ. Yaml konfig esetén "" jelek közé kell rakni a csatornát. Pl: Name: "#schumix2"
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
    * **Prefix:** A parancsok elõjele. Yaml konfig esetén "" jelek közé kell rakni a parancsot. Pl: Prefix: "$". Alapértelmezés: `$` (Fõ parancs xbot. Ezzel a parancselõjelel így néz ki: `$xbot`)
* **MessageType:** Értéke `Privmsg` vagy `Notice` lehet. Meghatározza hogy milyen formában küldje az üzeneteket a szerver felé. Alapértelmezés: `Privmsg`

## Log

* **FileName:** Meghatározza hova mentődjenek el a log információk. Alapértelmezés: `Schumix.log`
* **DateFileName:** Ha ez a beállítás bekapcsolásra került akkor a log fájl nevéből létrehoz egy mappát a program és abba az indítás dátumával menti el a logot. Így áttekinthetőbbé válik.
                    Alapértelmezés: `False`
* **MaxFileSize:** Meghatározza a log fájlt maximális méretét. Ha eléri azt a fájl akkor törlődik és a program csinál helyette egy újat.
                    Alapértelmezés: `100` (mb-ban értendő)
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
* **Ignore:** Azon addonok melyeket nem szeretnénk inditáskor betölteni. Vesszõvel elválasztva kell egymás útán írni öket. `(pl: TestAddon,Test2Addon vagy szimplán TestAddon)`
              Alapértelmezés: `MantisBTRssAddon,SvnRssAddon,GitRssAddon,HgRssAddon,WordPressRssAddon,TestAddon`
* **Directory:** Az addonok mappája ahol tárolva vannak és ahonnét betöltésre kerülnek. Alapértelmezés: `Addons`

## Scripts

* **Lua:** Értéke `true` vagy `false` lehet. Engedélyezi a lua fájlok betöltését. Alapértelmezés: `false`
* **Python:** Értéke `true` vagy `false` lehet. Engedélyezi a python fájlok betöltését. Alapértelmezés: `false`
* **Directory:** A script-ek mappája ahol tárolva vannak és ahonnét betöltésre kerülnek. Alapértelmezés: `Scripts`

## Crash

* **Directory:** Meghatározza az összeomláskor keletkező mappa nevét. Alapértelmezés: `Dumps`

## Localization

* **Locale:** Meghatározza hogy a kód milyen nyelven fusson. (csak az irc és konzol parancsokra vonatkozik)
              Alapértelmezés: `enUS`

## Update

* **Enabled:** Értéke `true` vagy `false` lehet. Engedélyezi az automatikus frissítést. Alapértelmezés: `false`
* **Version:** Meghatározza melyik verzióra szeretnénk frissíteni. Current vagy stable lehet. A current az utolsó verzó ami a tárolóban van a stable pedig az utolsó stabil verzió.
               Alapértelmezés: `stable`
* **Branch:** Beállítható vele az ág (branch). Ez csak a current verziók esetében érdekes. Alapértelmezés: `master`
* **WebPage:** A megadott weboldalcímről tölti le a frissítéseket. Alapértelmezés: `https://github.com/Schumix/Schumix2`

## Shutdown

* **MaxMemory:** Meghatározza a program leállítását ha eléri a megadott memória nagyságot. Ha több szerverre is csatlakozik a bot akkor annyival fog tovább nőni ez a korlát ahány irc szerver be van állítva a konfigba.
                 Alapértelmezés: `100` (mb-ban értendő)

## Flooding

* **Seconds:** Meghatározza mennyi időnként fusson le a flood elemzése. Alapértelmezés: `4` (másodpercben)
* **NumberOfCommands:** Meghatározza hányszor használhatja a parancsot adott személy a megadott indőn belül. Ha többet add meg akkor egy percre letiltja a program a parancsainak használatát annak a személynek. Alapértelmezés: `2`

## Clean

* **Config:** Értéke `true` vagy `false` lehet. Engedélyezi a konfig mappában a régi fájlok takarítását/törlését. Alapértelmezés: `false`
* **Database:** Értéke `true` vagy `false` lehet. Engedélyezi az adatbázis takarítását. Alapértelmezés: `false`

## ShortUrl

A bit.ly url röviditő apihoz való adatokat kell itt megadni.
* **Name:** A felhasználói nevet kell itt megadni.
* **ApiKey:** A felhasználóhoz tartozó api kulcsot kell megadni itt.
