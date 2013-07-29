# Server configuration file

## Log

* **FileName:** The filename where the log will be saved. Default: `Server.log`
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
    * **Port:** The server's port. Default: `35220`
* **Password:** The server's identify password. Default: `schumix`

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
                 Default: `100` (in MB)

## Clean

* **Config:** `true` or `false`. Enables the deletion of the old files. Default: `false`
* **Database:** `true` or `false`. Enables the cleaning of the Database. Default: `false`

## Schumixs

* **Enabled:** `true` or `false`. Enables the Schumixs' start-up. Default: `false`

With Yaml config:<br/>
If you want to start more Schumix you need to copy the whole Schumix part (Schumix: ...) and change the settings there and name it as Schumix(number). e.g.: Schumix2: ... (The rest is the same, only the Schumix section have to be changed.).<br/>
With Xml config:<br/>
If you want to start more Schumix you need to copy the whole Schumix part (`<Schumix>...</Schumix>`) and you need to change the settings there.
* **Schumix:**
    * **Config:**
        * **File:** The name of the Schumix configuration file. Default: `Schumix.yml`
        * **Directory:** The nme of the folder which contains the Schumix's configuration. Default: `Configs`
        * **ConsoleEncoding:** The character encoding of the Schumix's console. Default: `utf-8`
        * **Locale:** Schumix's language. Default: `enUS`
