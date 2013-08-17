# Schumix2 [![Build Status](https://travis-ci.org/Schumix/Schumix2.png?branch=stable)](https://travis-ci.org/Schumix/Schumix2)

# Statement

This code is intended to relay its predecessor. It is lay new basis and got many newness.
More faster and reliable than Schumix. The code is written in c#.

# Platforms

The bot has compatibility with windows and linux systems.
It might be run on Mac OS X but it's haven't been tested yet.
Under Windows the `.NET Framework 4.0` or higher is recommended.
Under Mono at least the `2.10.8.1` or higher is needed.

# Run source/Compile

## SubModule

Download missing submodules: git submodule update --init --recursive

## Windows

### Monodevelop / Xamarin Studio
When using Monodevelop / Xamarin Studio the `gettext` is needed for translating the language files.
<br/>Download and install it. `http://gnuwin32.sourceforge.net/packages/gettext.htm`
<br/>The compiling is simple. Open the `Schumix.sln` file.
<br/>Choose the configuration that fits for you and compile it.

### Visual Studio
When using Visual Studio you only need to open `SchumixVS.sln`.
<br/>Choose the configuration that fits for you and compile it.

## Linux

Open the `Schumix.sln` file.
<br/>Choose the configuration that fits for you and compile it.

## Linux terminal

Install the `mono-xbuild` package or from source `mono`.
<br/>Than run the `build.sh` command and the code is compiled.

# Code commissioning

Navigate to the `Run` folder and than to the appropriate folder for the configuration file.
<br/>Run the exe.
<br/>The program will generate its own config file.
<br/>If any problem occurs then create a `Configs` named folder and copy the `Schumix.yml` file from the `Configs` folder into it.

# Install

Use this option only if you want to innstal the bot like a program.
<br/>Attention! Administrative permissions probably needed at the final stage of install.

## Archlinux

**Attention!!! Root permission needed for that.**
<br/>Run the `./createarchlinuxpkg.sh` command.
<br/>When it's done a `schumix.pkg.tar.xz` (the name will be similar) named file will be created.
<br/>Install it with the `sudo pacman -U schumix2.pkg.tar.xz` command (the packet name will be similar) and it's all ready.
<br/>Run the bot with the `schumix` command.

## Debian/Ubuntu

**Attention!!! Root permission needed for that.**
<br/>For the first step, switch to fakeroot mode.
<br/>Type the `fakeroot` command for that.
<br/>After that run the `./createdebianpkg.sh` command.
<br/>When it's done a `schumix.deb` named file will be created.
<br/>Install it with the `sudo dpkg -i schumix.deb` command and it's all ready.
<br/>Run the bot with the `schumix` command.

## Windows

Navigate to the Installer folder.
<br/>Run the `Schumix.iss` named file.
<br/>When it's done a `Setup.exe` file will be created.
<br/>Run this and complete the install.
<br/>I guess I don't need to explain more. :)

# Config settings

To adjust the configs navigate to the `Share/doc/Configs` folder and open the `Schumix.md` file.
<br/>This is the description for the main config.
<br/>Reading the description is necessary because the bot can't be started correctly without it.
<br/>After that you can take a look at the other descriptions but they aren't required for starting the bot.

# Database configuration

## MySql

If you want to use a mysql based database set the `<MySql><Enabled>false</Enabled>` permission to true.
<br/>After that, fill the database from the `Sql` folder.
<br/>If any kind of correction or update will come you don't need to refill the whole database.
<br/>Only update it from the `Updates` folder with the appropriate version number.

## SQLite

If you want to use an SQLite based database set the `<SQLite><Enabled>false</Enabled>` permission to true.
<br/>Then copy the `Schumix.db3` file from the `Sql` folder next to the exe.
<br/>You can rename or move it but you have to change the name or the path in the config file.
<br/>If any kind of correction or update will come you don't need to refill the whole database.
<br/>Only update it from the `Updates` folder with the appropriate version number.

# Attention!

**Only one database can be active. If there is'n an active database or there are two active database the code will crash and stop running.**

If you are all done with it than you just need to launch and use the code. :)

# Trivia

* Usually configuration files belongs to the addons made by me. These are also generated in the `Configs` folder and you can modify them there.
* Furthermore, a bot command was placed in the code. That command composed by the primary name. For example: `schumix2, help`
  The structure of the command is important: `<primary nick>, command`
* There are functions belongs to the code which can be turned on and off. These are controllable by the administrator only.
  Description about the functions: `$help function` (With the configured prefix, of course)
* If it has been already mentioned, you can add admin with the console for first.
  `admin add <admin name>`
  Then you will get a password which have to be sent to the bot in a private message like this: `$admin access <password>`
  If you want to change it than: `$admin newpassword <old> <new>`
* And finally the console commands. You can get the list with te `help` command.
  The rest can be obtained as like the irc commands. Type the name of the command than the subcommand after the help command.
* If anything missed I can help on the `irc.rizon.net` server on the `#schumix, #schumix2` or `#hun_bot` channels.
* In Yaml configs, all the data which contains special characters (#,$) have to be placed between quotation marks "" otherwise the program try to    
  interpreting them and cause an error.
* I hope you will like the bot. :)
