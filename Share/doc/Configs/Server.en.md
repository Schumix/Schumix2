# Server konfig fájl

## Log

* **FileName:** The filename where the log will be saved. Default: `Schumix.log`
* **DateFileName:** If you enable this setting it will create a folder with the Filename given below and save here the log files with the name of the date of the launch so it becomes more manageable.
                    Default: `False`
* **MaxFileSize:** Specifies the log file's maximal size. If the log reaches this value the bot will delete it and creates a new one.
                    Default: `100` (In Mb)
* **LogLevel:** Specifies the messages which will be displayed in the console. Default: `2`
    * **Levels:** <br/>
                    `0` (Normal and successful messages)<br/>
                    `1` (Warnings)<br/>
                    `2` (Errors)<br/>
                    `3` (Troubleshooter messages)
* **LogDirectory:** The folder where the log file will be saved. Default: `Logs`

## Server

* **Listener:**
    * **Port:** A szerver portja. Default: `35220`
* **Password:** A szerver azonosító jelszava. Default: `schumix`

## Crash

* **Directory:** The name of the folder which will be created when the bot crashes. Default: `Dumps`

## Localization

* **Locale:** The code will run on that language. (Refers only for Irc and Console commands)
              Default: `enUS`

## Update

* **Enabled:** `true` or `false`. Enables the automatic updates. Default: `false`
* **Version:** The bot will be updated to this version. It can be Current or Stable. The Current is the latest version in the storage and the Stable is the latest stable version.
               Default: `stable`
* **Branch:** You can set the branch here. Only matter if you use Current versions. Default: `master`
* **WebPage:** The bot will get the updates from the given URL. Default: `https://github.com/Schumix/Schumix2`

## Shutdown

* **MaxMemory:** The bot will be stop running if it reaches the given memory size. If the bot connects to multiple servers It multiplies the size by how many connections the bot have.
                 Default: `100` (mb-ban értendő)

## Clean

* **Config:** `true` or `false`. Enables the deletion of the old files. Default: `false`
* **Database:** `true` or `false`. Enables the cleaning of the Database. Default: `false`

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
