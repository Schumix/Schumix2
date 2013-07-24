# Schumix config file

## Server

* **Enabled:** `true` or `false`. Should the program connect to a server. Default: `false`
* **Host:** Server's domain name or ip address.
* **Port:** Server port. Default: `35220`
* **Password:** Server's identify password. Default: `schumix`

## Listener

* **Enabled:** `true` or `false`. Should others be able to connect to the server in the bot. Default: `false`
* **Port:** Server port. Default: `36200`
* **Password:** Server's identify password. Default: `schumix`

## Irc

With Yaml config:<br/>
If you want to connect to more irc server you need to copy the whole irc part (Irc: ...) and you need to configure there the settings and you need to name it as Irc(number). eg.: Irc2: ... (after here is the same as in Irc).<br/>
With Xml config:<br/>
If you want to connect to more irc server you need to copy the while irc part (`<Irc>...</Irc>`) and you need to configure there the settings.
* **ServerName:** The Server's name. With more servers connected you can set each server's name. ATTENTION: The server's name can't be the same. (Little and big letters isn't different)!
* **Server:** Server's name where to connect.
* **Password:** A szerver jelszavát lehet vele beállítani. Ha üresen van hagyva akkor úgy veszi mintha nem kellene jelszó a szerverhez.
* **Port:** The server's port Default: `6667`
* **ModeMask:** Irc Mask. Default: `8`
* **Ssl:** `true` or `false`. Ezzel aktiválható a kapcsolódás olyan irc szerverre ahol ssl protokol van használva. Default: `false`
* **NickName:** Primary name.
* **NickName2:** Secondary name.
* **NickName3:** Tertiary name.
* **UserName:** Username.
* **UserInfo:** Information about the user.
* **MasterChannel:**
    * **Name:** Elsõdleges csatorna ahova csatlakozik minden esetben a bot. Ennek a neve itt változtatható meg. Az adatbázisból nem törölhetõ. Yaml konfig esetén "" jelek közé kell rakni a csatornát. Pl: Name: "#schumix2"
    * **Password:** Az elsődleges csatornához tartozó jelszó.
                    Default: (semmi)[Ez azt jelenti hogy nem add meg jelszót az elsődleges csatornához.]
* **IgnoreChannels:** Letilthatók a nem kívánatos csatornák vele. Ami itt szerepel oda nem megy fel a bot. Ezen rész letiltja a bot rendszerében szereplõket is.
                      Tehát ha abból nem akarunk valahova felmenni akkor is használhatjuk ezt törlés helyett. Vesszõvel elválasztva kell egymás útán írni öket.
                      `pl: #teszt,#teszt2 vagy szimplán #teszt`
* **IgnoreNames:** Letilthatóak vele a nem kívánatos személyek. Így csak az használhatja a botot aki megérdemli.
                   `pl: schumix,schumix2 vagy szimplán schumix`
* **NickServ:**
    * **Enabled:** `true` or `false`. Ezen rész határozza meg hogy a nickhez tartozó jelszó el legyen-e küldve. true = igen, false = nem.
                   Default: false
    * **Password:** Nickhez tartozó jelszó.
* **HostServ:**
    * **Enabled:** `true` or `false`. Ezen rész határózza meg hogy ha van a nickhez vhost akkor bekapcsolodjon-e. Default: `false`
                   Mert ha nincs akkor megjelenitödhet az ip ezért olyankor ajánlott false értékre tenni.
    * **Vhost:** `true` or `false`. Ezen rész határózza meg hogy a nickhez tartotó vhost aktiválásra kerüljön-e. Default: `false`
* **Wait:**
    * **MessageSending:** Üzenet küldésének késleltetése. Legföbbként flood ellen van.
* **Command:**
    * **Prefix:** A parancsok elõjele. Yaml konfig esetén "" jelek közé kell rakni a parancsot. Pl: Prefix: "$". Default: `$` (Fõ parancs xbot. Ezzel a parancselõjelel így néz ki: `$xbot`)
* **MessageType:** Értéke `Privmsg` vagy `Notice` lehet. Meghatározza hogy milyen formában küldje az üzeneteket a szerver felé. Default: `Privmsg`

## Log

* **FileName:** Meghatározza hova mentődjenek el a log információk. Default: `Schumix.log`
* **DateFileName:** Ha ez a beállítás bekapcsolásra került akkor a log fájl nevéből létrehoz egy mappát a program és abba az indítás dátumával menti el a logot. Így áttekinthetőbbé válik.
                    Default: `False`
* **MaxFileSize:** Meghatározza a log fájlt maximális méretét. Ha eléri azt a fájl akkor törlődik és a program csinál helyette egy újat.
                    Default: `100` (mb-ban értendő)
* **LogLevel:** Meghatározza hogy a konzolba milyen üzenetek kerülnek kiírásra. Default: `2`
    * **Szintjei:** <br/>
                    `0` (Normális üzenetek és a sikeresek)<br/>
                    `1` (Figyelmeztetések)<br/>
                    `2` (Hibák)<br/>
                    `3` (Hibakeresõ üzenetek)
* **LogDirectory:** A log üzenetek mentése abba a mappába ami megvan adva. Default: `Logs`
* **IrcLogDirectory:** Az irc csatornák és egyéb üzenetének mentése abba a mappába ami megvan adva. Default: `Csatornak`
* **IrcLog:** `true` or `false`. Meghatározza hogy a konzolba kiirásra kerülhetnek-e az irc-tõl jövõ üzenetek. Default: `false`

## MySql

* **Enabled:** `true` or `false`. Meghatározza hogy mysql alapú-e az adatbázis. Default: `false`
* **Host:** A mysql szerver címe.
* **User:** A szerver felhasználó neve.
* **Password:** A szerver jelszava.
* **Database:** Az adatbázis amiben megtalálhatók a bothoz tartozó táblák.
* **Charset:** Az adatbázisba menõ adatok kódolását és olvasását határozza meg.
               Default: `utf8`

## SQLite

* **Enabled:** `true` or `false`. Meghatározza hogy sqlite alapú-e az adatbázis. Default: `false`
* **FileName:** Az sqlite fájl neve.

## Addons

* **Enabled:** `true` or `false`. Engedélyezi az addonok betöltését. Default: `true`
* **Ignore:** Azon addonok melyeket nem szeretnénk inditáskor betölteni. Vesszõvel elválasztva kell egymás útán írni öket. `(pl: TestAddon,Test2Addon vagy szimplán TestAddon)`
              Default: `MantisBTRssAddon,SvnRssAddon,GitRssAddon,HgRssAddon,WordPressRssAddon,TestAddon`
* **Directory:** Az addonok mappája ahol tárolva vannak és ahonnét betöltésre kerülnek. Default: `Addons`

## Scripts

* **Lua:** `true` or `false`. Engedélyezi a lua fájlok betöltését. Default: `false`
* **Python:** `true` or `false`. Engedélyezi a python fájlok betöltését. Default: `false`
* **Directory:** A script-ek mappája ahol tárolva vannak és ahonnét betöltésre kerülnek. Default: `Scripts`

## Crash

* **Directory:** Meghatározza az összeomláskor keletkező mappa nevét. Default: `Dumps`

## Localization

* **Locale:** Meghatározza hogy a kód milyen nyelven fusson. (csak az irc és konzol parancsokra vonatkozik)
              Default: `enUS`

## Update

* **Enabled:** `true` or `false`. Engedélyezi az automatikus frissítést. Default: `false`
* **Version:** Meghatározza melyik verzióra szeretnénk frissíteni. Current vagy stable lehet. A current az utolsó verzó ami a tárolóban van a stable pedig az utolsó stabil verzió.
               Default: `stable`
* **Branch:** Beállítható vele az ág (branch). Ez csak a current verziók esetében érdekes. Default: `master`
* **WebPage:** A megadott weboldalcímről tölti le a frissítéseket. Default: `https://github.com/Schumix/Schumix2`

## Shutdown

* **MaxMemory:** Meghatározza a program leállítását ha eléri a megadott memória nagyságot. Ha több szerverre is csatlakozik a bot akkor annyival fog tovább nőni ez a korlát ahány irc szerver be van állítva a konfigba.
                 Default: `100` (mb-ban értendő)

## Flooding

* **Seconds:** Meghatározza mennyi időnként fusson le a flood elemzése. Default: `4` (másodpercben)
* **NumberOfCommands:** Meghatározza hányszor használhatja a parancsot adott személy a megadott indőn belül. Ha többet add meg akkor egy percre letiltja a program a parancsainak használatát annak a személynek. Default: `2`

## Clean

* **Config:** `true` or `false`. Engedélyezi a konfig mappában a régi fájlok takarítását/törlését. Default: `false`
* **Database:** `true` or `false`. Engedélyezi az adatbázis takarítását. Default: `false`

## ShortUrl

A bit.ly url röviditő apihoz való adatokat kell itt megadni.
* **Name:** A felhasználói nevet kell itt megadni.
* **ApiKey:** A felhasználóhoz tartozó api kulcsot kell megadni itt.
