# Schumix config file

## Server

* **Enabled:** `true` or `false`. Enables the program to connect to the Server. Default: `false`
* **Host:** Server's Domain name or Ip address.
* **Port:** Server port. Default: `35220`
* **Password:** Server's identification password. Default: `schumix`

## Listener

* **Enabled:** `true` or `false`. Specifies that others will be able to connect to the bot's built in Server. Default: `false`
* **Port:** Server port. Default: `36200`
* **Password:** Server's identification password. Default: `schumix`

## Irc

With Yaml config:<br/>
If you want to connect to more Irc Servers or create multiple connections to the same Irc Server you need to copy the whole Irc part (Irc: ...) and change the settings there and name it as Irc(number). e.g.: Irc2: ... (The rest is the same, only the Irc section have to be changed.).<br/>
With Xml config:<br/>
If you want to connect to more Irc Servers or create multiple connections to the same Irc Server you need to copy the whole Irc part (`<Irc>...</Irc>`) and you need to change the settings there.
* **ServerName:** The Server's name. With more Servers connected you can set each Server's name. ATTENTION: The Server's name can't be the same. (Upper-case and lower-case letters aren't different)!
* **Server:** The Server's name which you want to connect.
* **Password:** The Server's password. If left blank it means that password is not required.
* **Port:** The Server's port Default: `6667`
* **ModeMask:** Irc Mask. Default: `8`
* **Ssl:** `true` or `false`. Activates connection to a Server which uses SSL protocol. Default: `false`
* **NickName:** Primary name.
* **NickName2:** Secondary name.
* **NickName3:** Tertiary name.
* **UserName:** User name.
* **UserInfo:** Information about the user.
* **MasterChannel:**
    * **Name:** The name of the Primary Channel where the bot connects every time. It can't deleted from the database. If you using Yaml configuration you have to put the name between "" marks. e.g.: 'Name: "#schumix2"'
    * **Password:** The password of the Primary Channel.
                    Default: (nothing)[It means that password won't be set for the Primary Channel.]
* **IgnoreChannels:** The bot ignores the channels listed here and won't join to them. That part affect the Channels in the bot's system too, so if you don't want to join to any channel from it use this setting instead of delete. Separate them with commas sequentially.
                      `e.g.: #test,#test2 or simply #test`
* **IgnoreNames:** The bot ignores the users listed here so they can't use it.
   				`e.g.: schumix,schumix2 or simply schumix`
* **NickServ:**
    * **Enabled:** `true` or `false`. Specifies that the password of the Nickname can be sent. true = yes, false = no.
                   Default: false
    * **Password:** Password of the Nickname.
* **HostServ:**
    * **Enabled:** `true` or `false`. Specifies that the vhost will be enabled if the Nick has one. Default: `false`
                   If the Nick hasn't got a vhost the Ip address can displayed therefore false is recommended.
    * **Vhost:** `true` or `false`. Specifies that the vhost will be activated. Default: `false`
* **Wait:**
    * **MessageSending:** Delay between the messages. Mainly against flooding.
* **Command:**
    * **Prefix:** The omen of the commands. If you using Yaml configuration you have to put the command between "" marks. e.g.: Prefix: "$". Default: `$` (Main command is 'xbot'. With this prefix it's look like this: `$xbot`)
* **MessageType:** This only can be `Privmsg` or `Notice` . Specifies the format of the message to the server. Default: `Privmsg`

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
* **IrcLogDirectory:** The IRC Chanells' messages and others will be saved in the given folder here. Default: `Channels`
* **IrcLog:** `true` or `false`. Specifies that the Console can display the messages from the IRC. Default: `false`

## MySql

* **Enabled:** `true` or `false`. Specifies that the Database is MySql based. Default: `false`
* **Host:** Address of the MySql server.
* **Port:** Port of the MySql server. Default: `3306`
* **User:** Username of the server.
* **Password:** The server's password.
* **Database:** The Database that contains the tables for the Bot.
* **Charset:** Specifies the encoding and the reading of the Data which goes to the Database.
               Default: `utf8`

## SQLite

* **Enabled:** `true` or `false`. Specifies that the Database is SQLite based. Default: `false`
* **FileName:** The name of the SQLite file.

## Addons

* **Enabled:** `true` or `false`. Enables the loading of addons. Default: `true`
* **Ignore:** These addons won't load at start-up. Separate them with commas sequentially.`(pl: TestAddon,Test2Addon or simply TestAddon)`
              Default: `MantisBTRssAddon,SvnRssAddon,GitRssAddon,HgRssAddon,WordPressRssAddon,TestAddon`
* **Directory:** The addons' folder where they stored and loaded from there. Default: `Addons`

## Scripts

* **Lua:** `true` or `false`. Enables the loading of the Lua files. Default: `false`
* **Python:** `true` or `false`. Enables the loading of the Python files. Default: `false`
* **Directory:** The scripts' folder where they stored and loaded from there. Default: `Scripts`

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

## Flooding

* **Seconds:** The period of the flood analysing. Default: `4` (In seconds)
* **NumberOfCommands:** Specifies that how many times can a user use a command inside the given time. If a user violate the limit the bot will suspend the user for one minute. Default: `2`

## Clean

* **Config:** `true` or `false`. Enables the deletion of the old files. Default: `false`
* **Database:** `true` or `false`. Enables the cleaning of the Database. Default: `false`

## ShortUrl

The details for the bit.ly url shorting API.
* **Name:** The u ser name.
* **ApiKey:** The API key for the user name.
