# Server konfig fájl

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

## Server

* **Listener:**
    * **Port:** A szerver portja. Default: `35220`
* **Password:** A szerver azonosító jelszava. Default: `schumix`

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

## Clean

* **Config:** `true` or `false`. Engedélyezi a konfig mappában a régi fájlok takarítását/törlését. Default: `false`
* **Database:** `true` or `false`. Engedélyezi az adatbázis takarítását. Default: `false`

## Schumixs

* **Enabled:** `true` or `false`. Engedélyezi a Schumix-ok indítását. Default: `false`

With Yaml config:<br/>
Ha több Schumix-ot szeretnék indítani akkor az egész Schumix részt (Schumix: ...) le kell másolni még egyszer és ott külön be kell állítani az adatokat valamint Schumix(szám) ként kell megadni. Pl: Schumix2: .... (ide pedig minde úgy jön utána ahogy volt csak az Schumix-nél kell átírni az újat).<br/>
With Xml config:<br/>
Ha több Schumix-ot szeretnék indítani akkor az egész Schumix részt (`<Schumix> ... </Schumix>`) le kell másolni még egyszer és ott külön be kell állítani az adatokat.
* **Schumix:**
    * **Config:**
        * **File:** Schumix konfig fájl neve. Default: `Schumix.yml`
        * **Directory:** Schumix konfig mappa neve/elérhetősége. Default: `Configs`
        * **ConsoleEncoding:** Schumix konzoljának karakterkódolása. Default: `utf-8`
        * **Locale:** Schumix nyelvezete. Default: `enUS`
