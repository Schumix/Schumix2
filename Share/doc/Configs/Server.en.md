# Server konfig fájl

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

## Server

* **Listener:**
    * **Port:** A szerver portja. Alapértelmezés: `35220`
* **Password:** A szerver azonosító jelszava. Alapértelmezés: `schumix`

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

## Clean

* **Config:** Értéke `true` vagy `false` lehet. Engedélyezi a konfig mappában a régi fájlok takarítását/törlését. Alapértelmezés: `false`
* **Database:** Értéke `true` vagy `false` lehet. Engedélyezi az adatbázis takarítását. Alapértelmezés: `false`

## Schumixs

* **Enabled:** Értéke `true` vagy `false` lehet. Engedélyezi a Schumix-ok indítását. Alapértelmezés: `false`

Yaml konfignál:<br/>
Ha több Schumix-ot szeretnék indítani akkor az egész Schumix részt (Schumix: ...) le kell másolni még egyszer és ott külön be kell állítani az adatokat valamint Schumix(szám) ként kell megadni. Pl: Schumix2: .... (ide pedig minde úgy jön utána ahogy volt csak az Schumix-nél kell átírni az újat).
Xml konfignál:<br/>
Ha több Schumix-ot szeretnék indítani akkor az egész Schumix részt (`<Schumix> ... </Schumix>`) le kell másolni még egyszer és ott külön be kell állítani az adatokat.
* **Schumix:**
    * **Config:**
        * **File:** Schumix konfig fájl neve. Alapértelmezés: `Schumix.yml`
        * **Directory:** Schumix konfig mappa neve/elérhetősége. Alapértelmezés: `Configs`
        * **ConsoleEncoding:** Schumix konzoljának karakterkódolása. Alapértelmezés: `utf-8`
        * **Locale:** Schumix nyelvezete. Alapértelmezés: `enUS`
