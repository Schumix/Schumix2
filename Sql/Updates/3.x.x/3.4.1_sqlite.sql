-- ----------------------------
-- Table structure for "irc_commands"
-- ----------------------------
DROP TABLE IF EXISTS "irc_commands";
CREATE TABLE "irc_commands" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4)    DEFAULT 'enUS',
Command VARCHAR(30),
Text TEXT
);

-- huHU
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'mode', 'Mode használata: /mode <csatorna> <rang> <név>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'fixrank', 'Rang mentése: /chanserv <rang (sop, aop, hop, vop)> <csatorna> ADD <név>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Nick név megváltoztatása: /nick <új név>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'kick', 'Kick használata: /kick <csatorna> <név> (<oka> nem feltétlen kell)');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'owner', 'Owner mód bekapcsolása a csatornán: /msg chanserv SET <csatorna> ownermode on');

-- enUS
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'mode', 'Mode usage: /mode <channel> <rank> <name>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'fixrank', 'Save rank: /chanserv <rank (sop, aop, hop, vop)> <channel> ADD <name>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'nick', 'Nick change usage: /nick <new nick>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'kick', 'Kick usage: /kick <channel> <name> (<reason>)');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'owner', 'Turn on owner mode: /msg chanserv SET <channel> ownermode on');

DELETE FROM `localized_warning` WHERE Command = 'NoIrcCommandName';

-- huHU
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'irc', '3Parancsok: {0}\nNem létezik ilyen parancs!');

-- enUS
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'irc', '3Commands: {0}\nNo such command!');

-- huHU
UPDATE `localized_console_command` SET Text = "Adminok: {0}" WHERE Language = 'huHU' AND Command = 'admin/list';

-- enUS
UPDATE `localized_console_command` SET Text = "Admins: {0}" WHERE Language = 'enUS' AND Command = 'admin/list';

-- huHU
UPDATE `localized_console_command_help` SET Text = "Kiírja, hogy éppen milyen rangja van.\nHasználata: admin info <admin neve>" WHERE Language = 'huHU' AND Command = 'admin/info';
UPDATE `localized_console_command_help` SET Text = "A megadott csatornán ezzel a paranccsal állíthatók a funkciók.\nFunkció channel parancsai: info\nHasználata:\nCsatorna funkció kezelése: function channel <csatorna neve> <on vagy off> <funkció név>\nChannel funkciók kezelése: function channel <csatorna neve> <on vagy off> <funkció név1> <funkció név2> ... stb" WHERE Language = 'huHU' AND Command = 'function/channel';
UPDATE `localized_console_command_help` SET Text = "Frissíti a funkciókat vagy alapértelmezésre állítja.\nFunkció update parancsai: all\nHasználata:\nMás channel: function update <csatorna neve>\nJelenlegi csatorna: function update" WHERE Language = 'huHU' AND Command = 'function/update';
UPDATE `localized_console_command_help` SET Text = "Frissíti az összes funkciót vagy alapértelmezésre állítja.\nHasználata: function update all" WHERE Language = 'huHU' AND Command = 'function/update/all';
UPDATE `localized_console_command_help` SET Text = "Új csatorna hozzáadása.\nHasználata: channel add <csatorna neve> <ha van jelszó akkor az>" WHERE Language = 'huHU' AND Command = 'channel/add';
UPDATE `localized_console_command_help` SET Text = "Nem használatos csatorna eltávolítása.\nHasználata: channel remove <csatorna neve>" WHERE Language = 'huHU' AND Command = 'channel/remove';
UPDATE `localized_console_command_help` SET Text = "Frissíti a csatornákhoz tartozó összes információkat és alapértelmezettre állítja" WHERE Language = 'huHU' AND Command = 'channel/update';
UPDATE `localized_console_command_help` SET Text = "Frissíti a csatorna nyelvezetét.\nHasználata: channel language <csatorna neve> <nyelvezet>" WHERE Language = 'huHU' AND Command = 'channel/language';
UPDATE `localized_console_command_help` SET Text = "Kapcsolódás a megadott csatornára.\nHasználata:\nJelszó nélküli csatorna: join <csatorna neve>\nJelszóval ellátott csatorna: join <csatorna neve> <jelszó>" WHERE Language = 'huHU' AND Command = 'join';
UPDATE `localized_console_command_help` SET Text = "Lelépés a megadott csatornáról.\nHasználata: leave <csatona neve>" WHERE Language = 'huHU' AND Command = 'leave';

-- enUS
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin', 'Print Operators or Administrators can use commands.\nAdmin commands: info | list | add | remove | rank | access | newpassword');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/add', 'Add new admin.\nUse: admin add <admin name>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/remove', 'Removing admin.\nUse: admin remove <admin name>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/rank', 'Admin rank change.\nUse: admin rank <admin name> <new rank e.g. halfoperator: 0, operator: 1, administrator: 2>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/info', 'Show the admin\'s rank.\nUse: admin info <admin name>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/list', 'Show the names of all the admin, who is included in the database.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function', 'Function control command.\nFunction commands: channel | update | info\nUse globally:\nGlobal management function: function <on or off> <function name>\nGlobal management functions: function <on or off> <function name1> <function name2> ... e.g.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/channel', 'The specified channel, use this command to set functions.\nFunction channel commands: info\nUse:\nChannel management function: function channel <channel> <on or off> <function name>\nChannel management functions: function channel <channel> <on or off> <function name1> <function name2> ... e.g.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/channel/info', 'Shows the functions status.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/update', 'Updates the function or set defaults.\nFunction update command: all\nUse:\nOther channel: function update <channel name>\nCurrent channel: function update');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/update/all', 'Updates all the features or set defaults.\nUse: function update all');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/info', 'Shows the functions status.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel', 'Channel commands: add | remove | info | update | language');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/add', 'Add new channel.\nUse: channel add <channel name> <password if you have>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/remove', 'Removing channel.\nUse: channel remove <channel name>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/info', 'Shows all the channels, which are in the database and associated information.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/update', 'Updates on all channels of information and their default values??.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/language', 'Updates the language of the channel.\nUse: channel language <channel name> <language>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'nick', 'Bot nick change.\nUse: nick <name>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'join', 'Connect to the specified channel.\nUse:\nNon-password protected channels: join <channel name>\nPassword protected channels: join <channel name> <password>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'leave', 'Part a given channel.\Use: leave <channel name>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'quit', 'Bot shutdown command.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'reload', 'Reloads the specified program section.\nUse: reload <name>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'consolelog', 'Enables or disables display of the IRC logs to the console. The default is off.\nUse: consolelog <on or off>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'sys', 'Show the system information of the bot.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'csatorna', 'You can select which channel to send the robot.\nUse: csatorna <channel name>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'connect', 'Connect to the IRC server.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'disconnect', 'Disconnect.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'reconnect', 'Trying to reconnect to the IRC server.');

-- huHU
UPDATE `localized_command_help` SET Text = "Megállapítja hogy a szám prímszám-e. Csak egész számmal tud számolni!\nHasználata: {0}prime <szám>" WHERE Language = 'huHU' AND Command = 'prime';
UPDATE `localized_command_help` SET Text = "Kapcsolódás a megadott csatornára.\nHasználata:\nJelszó nélküli csatorna: {0}join <csatorna neve>\nJelszóval ellátott csatorna: {0}join <csatorna neve> <jelszó>" WHERE Language = 'huHU' AND Command = 'join';
UPDATE `localized_command_help` SET Text = "Lelépés a megadott csatonáról.\nHasználata: {0}leave <csatona neve>" WHERE Language = 'huHU' AND Command = 'leave';
UPDATE `localized_command_help` SET Text = "Új csatorna hozzáadása.\nHasználata: {0}channel add <csatorna neve> <ha van jelszó akkor az>" WHERE Language = 'huHU' AND Command = 'channel/add';
UPDATE `localized_command_help` SET Text = "Nem használatos channel eltávolítása.\nHasználata: {0}channel remove <csatorna neve>" WHERE Language = 'huHU' AND Command = 'channel/remove';
UPDATE `localized_command_help` SET Text = "Összes channel kiírása ami az adatbázisban van és a hozzájuk tartozó információk." WHERE Language = 'huHU' AND Command = 'channel/info';
UPDATE `localized_command_help` SET Text = "Frissíti a csatorna nyelvezetét.\nHasználata: {0}channel language <csatorna neve> <nyelvezet>" WHERE Language = 'huHU' AND Command = 'channel/language';
UPDATE `localized_command_help` SET Text = "Megadott csatornán állithatók ezzel a parancsal a funkciók.\nFunkció channel parancsai: info\nHasználata:\nChannel funkció kezelése: {0}function channel <csatorna neve> <on vagy off> <funkció név>\nChannel funkciók kezelése: {0}function channel <csatorna neve> <on vagy off> <funkció név1> <funkció név2> ... stb" WHERE Language = 'huHU' AND Command = 'function/channel';
UPDATE `localized_command_help` SET Text = "Frissíti a funkciókat vagy alapértelmezésre állítja.\nFunkció update parancsai: all\nHasználata:\nMás channel: {0}function update <csatorna neve>\nAhol tartózkodsz channel: {0}function update" WHERE Language = 'huHU' AND Command = 'function/update';
UPDATE `localized_command_help` SET Text = "Kirúgja a nick-et a megadott csatornáról.\nHasználata:\nCsak kirúgás: {0}kick <csatorna neve> <név>\nKirúgás okkal: {0}kick <csatorna neve> <név> <oka>" WHERE Language = 'huHU' AND Command = 'kick';
UPDATE `localized_command_help` SET Text = "Rss csatornákra való kiírásának kezelése.\nChannel parancsai: add | remove" WHERE Language = 'huHU' AND Command = 'svn/channel';
UPDATE `localized_command_help` SET Text = "Új csatorna hozzáadása az rss-hez.\nHasználata: {0}svn channel add <rss neve> <csatorna neve>" WHERE Language = 'huHU' AND Command = 'svn/channel/add';
UPDATE `localized_command_help` SET Text = "Nem használatos csatorna eltávolítása az rss-bõl.\nHasználata: {0}svn channel remove <rss neve> <csatorna neve>" WHERE Language = 'huHU' AND Command = 'svn/channel/remove';
UPDATE `localized_command_help` SET Text = "Nem használatos csatorna eltávolítása az rss-bõl.\nHasználata: {0}git channel remove <rss neve> <tipus> <csatorna neve>" WHERE Language = 'huHU' AND Command = 'git/channel/remove';
UPDATE `localized_command_help` SET Text = "Felhasználó jelszavának cseréje ha új kéne a régi helyett.\nHasználata: {0}notes user newpassword <régi jelszó> <új jelszó>" WHERE Language = 'huHU' AND Command = 'notes/user/newpassword';
UPDATE `localized_command_help` SET Text = "Jegyzet kiolvasásához szükséges kód.\nHasználata: {0}notes code <jegyzet kódja>\nKód parancsai: remove" WHERE Language = 'huHU' AND Command = 'notes/code';
UPDATE `localized_command_help` SET Text = "Törli a jegyzetet kód alapján.\nHasználata: {0}notes code remove <jegyzet kódja>" WHERE Language = 'huHU' AND Command = 'notes/code/remove';
UPDATE `localized_command_help` SET Text = "Ezzel a paranccsal üzenetet lehet hagyni bárkinek a kiválasztott csatornán.\nHasználata: {0}message channel <csatorna neve> <név> <üzenet>" WHERE Language = 'huHU' AND Command = 'message/channel';
UPDATE `localized_command_help` SET Text = "Ezzel a paranccsal állítható a hl állapota.\nHasználata: {0}autofunction hlmessage function <állapot>" WHERE Language = 'huHU' AND Command = 'autofunction/hlmessage/function';
UPDATE `localized_command_help` SET Text = "Automatikusan kirúgásra kerülõ nick-ek kezelése.\nKick parancsai: add | remove | list | channel" WHERE Language = 'huHU' AND Command = 'autofunction/kick';
UPDATE `localized_command_help` SET Text = "Kiírja a kirugandók állapotát." WHERE Language = 'huHU' AND Command = 'autofunction/kick/list';
UPDATE `localized_command_help` SET Text = "Automatikusan kirugásra kerülõ nick-ek kezelése megadott csatornán.\nKick channel parancsai: add | remove | list" WHERE Language = 'huHU' AND Command = 'autofunction/kick/channel';
UPDATE `localized_command_help` SET Text = "Kirugandó nevének hozzáadása megadott csatornán.\nHasználata: {0}autofunction kick channel <csatorna neve> add <név> <oka>" WHERE Language = 'huHU' AND Command = 'autofunction/kick/channel/add';
UPDATE `localized_command_help` SET Text = "Kirugandó nevének eltávolítása megadott csatornán.\nHasználata: {0}autofunction kick channel <csatorna neve> remove <név>" WHERE Language = 'huHU' AND Command = 'autofunction/kick/channel/remove';
UPDATE `localized_command_help` SET Text = "Kiírja a kirugandók állapotát a megadott csatornán.\nHasználata: {0}autofunction kick channel <csatorna neve> list" WHERE Language = 'huHU' AND Command = 'autofunction/kick/channel/list';
UPDATE `localized_command_help` SET Text = "Rangot kapó nevének hozzáadása megadott csatornán.\nHasználata: {0}autofunction mode channel <csatorna neve> add <név> <rang>" WHERE Language = 'huHU' AND Command = 'autofunction/mode/channel/add';
UPDATE `localized_command_help` SET Text = "Rang megváltoztatása a megadott csatornán.\nHasználata: {0}autofunction mode channel <csatorna neve> change <név>" WHERE Language = 'huHU' AND Command = 'autofunction/mode/channel/change';
UPDATE `localized_command_help` SET Text = "Rangot kapó nevének eltávolítása megadott csatornán.\nHasználata: {0}autofunction mode channel <csatorna neve> remove <név>" WHERE Language = 'huHU' AND Command = 'autofunction/mode/channel/remove';
UPDATE `localized_command_help` SET Text = "Kiírja a rangot kapók állapotát a megadott csatornán.\nHasználata: {0}autofunction mode channel <csatorna neve> info" WHERE Language = 'huHU' AND Command = 'autofunction/mode/channel/list';
UPDATE `localized_command_help` SET Text = "Játék indítására szolgáló parancs.\nHasználata: {0}game start <játék neve>" WHERE Language = 'huHU' AND Command = 'game/start';

-- enUS
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'xbot', '9', 'Users to use the command list.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'info', '9', 'Small description of the bot.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'whois', '9', 'This command tells you which channel is that a nick above.\nHasználata: {0}whois <nick>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'roll', '9', 'Tiny fun from the World of Warcraft, if you recognize anyone: P');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'date', '9', 'Writes the current date and name day.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'time', '9', 'The current time is printed.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'google', '9', 'If you need something from Google without a website, you just type this command.\nUse: {0}google <Your text goes here>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'translate', '9', 'If you want to translate immediately from one language to another, you just type this command.\nUse: {0}translate <base language|target language> <text>\ne.g.: {0}translate hu|en Szép szöveg.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'irc', '9', 'Some commands use the IRC.\nUse: {0}irc <command name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'calc', '9', 'Multi-function calculator.\nUse: {0}calc <number>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'warning', '9', 'A warning message to look for that channel, or an arbitrary message.\nUse: {0}warning <name> <an arbitrary message>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'sha1', '9', 'Sha1 encoding conversion scripts.\nUse: {0}sha1 <convert text>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'md5', '9', 'Md5 encryption conversion scripts.\nUse: {0}md5 <convert text>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'prime', '9', 'It states that the number is prime or not. Only whole numbers can count!\nUse: {0}prime <number>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'admin', '0', 'Shows Operators or Administrators can use commands.\nAdmin commands: info | list | add | remove | rank | access | newpassword');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'admin/add', '0', 'Add new admin.\nUse: {0}admin add <admin name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'admin/remove', '0', 'Admin removed.\nUse: {0}admin remove <admin name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'admin/rank', '0', 'Admin rank change.\nUse: {0}admin rank <admin name> <new rank e.g. halfoperator: 0, operator: 1, administrator: 2>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'admin/info', '0', 'It show you admin level.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'admin/list', '0', 'Show the names of all the admin, who is included in the database.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'admin/access', '0', 'The admin password is required to command control, and activation vhost.\nUse: {0}admin access <password>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'admin/newpassword', '0', 'The admin password replacement, should a new for old.\nUse: {0}admin newpassword <old password> <new password>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'colors', '0', 'A specific range of colors that can be used to dump on IRC.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'nick', '0', 'Bot nick change.\nUse: {0}nick <name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'join', '0', 'Connect to the specified channel.\nUse:\nNon-password protected channels: {0}join <channel name>\nPassword protected channels: {0}join <channel name> <password>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'leave', '0', 'Part a given channel.\nUse: {0}leave <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'channel', '1', 'Channel commands: add | remove | info | update | language');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'channel/add', '1', 'Add new channel.\nUse: {0}channel add <channel name> <password if you have>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'channel/remove', '1', 'Removing channel.\nUse: {0}channel remove <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'channel/info', '1', 'Shows all the channels, which are in the database and associated information.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'channel/update', '1', 'Updates on all channels of information and their default values??.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'channel/language', '1', 'Updates the language of the channel.\nUse: {0}channel language <channel name> <language>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'function', '1', 'Function control command.\nFunction commands: channel | all | update | info\nUse current channel:\nChannel management function: {0}function <on or off> <function name>\nChannel management functions: {0}function <on or off> <function name1> <function name2> ... e.g.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'function/channel', '1', 'The specified channel, use this command to set functions.\nFunction channel commands: info\nUse:\nChannel management function: {0}function channel <channel name> <on or off> <function name>\nChannel management functions: {0}function channel <channel name> <on or off> <function name1> <function name2> ... e.g.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'function/channel/info', '1', 'Shows the functions status.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'function/all', '1', 'Global management functions.\nFunction all commands: info\nGlobal management function: {0}function all <on or off> <function name>\nGlobal management functions: {0}function all <on or off> <function name1> <function name2> ... e.g.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'function/all/info', '1', 'Shows the functions status.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'function/update', '1', 'Updates the function or set defaults.\nFunction update command: all\nUse:\nOther channel: {0}function update <channel name>\nCurrent channel: {0}function update');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'function/update/all', '1', 'Updates all the features or set defaults.\Use: {0}function update all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'function/info', '1', 'Shows the functions status.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'kick', '1', 'Removes the given nick from the specified channel.\nUse:\nKick without reason: {0}kick <channel name> <name>\nKick with reason: {0}kick <channel name> <name> <reason>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'mode', '1', 'Changes to the rank of the given nickname to the specified channel.\nUse: {0}mode <rank> <name or names>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'plugin', '2', 'Shows what plugins are loaded.\nPlugin commands: load | unload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'plugin/load', '2', 'Loads all the plugin.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'plugin/unload', '2', 'Remove all plugin.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'quit', '2', 'Bot shutdown command.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'schumix2', '9', 'Commands: nick | sys\nCommands: ghost | nick | sys\nCommands: ghost | nick | sys | clean\nCommands: sys');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'schumix2/sys', '9', 'Displays the program information.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'schumix2/ghost', '1', 'Exits to the main nick, if registered.\nUse: {0} ghost');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'schumix2/nick', '0', 'Bot nick change.\nCommands: identify\nUse: {0} nick <name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'schumix2/nick/identify', '0', 'Turn on the main nick password.\nNick: {0} nick identify');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'schumix2/clean', '2', 'Frees allocated memory.\nUse: {0} clean');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn', '1', 'Rss svn \'s management.\nSvn commands: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/channel', '1', 'RSS feeds on their handling of the announcement.\nChannel commands: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/channel/add', '1', 'New channel added to the rss.\nUse: {0}svn channel add <rss name> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/channel/remove', '1', 'Removed from the RSS Channel.\nUse: {0}svn channel remove <rss name> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/info', '1', 'Prints rss-s condition.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/list', '1', 'Optional list of rss.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/start', '1', 'New RSS feeds.\nUse: {0}svn start <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/stop', '1', 'Rss stop.\nUse: {0}svn stop <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/reload', '1', 'Specify rss reload.\nSvn reload command: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/reload/all', '1', 'All RSS reload.\nUse: {0}svn reload <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg', '1', 'Rss hg \'s management.\nHg commands: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/channel', '1', 'RSS feeds on their handling of the announcement.\nChannel commands: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/channel/add', '1', 'New channel added to the rss.\nUse: {0}hg channel add <rss name> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/channel/remove', '1', 'Removed from the RSS Channel.\nUse: {0}hg channel remove <rss name> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/info', '1', 'Prints rss-s condition.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/list', '1', 'Optional list of rss.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/start', '1', 'New RSS feeds.\nUse: {0}hg start <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/stop', '1', 'Rss stop.\nUse: {0}hg stop <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/reload', '1', 'Specify rss reload.\nHg reload command: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/reload/all', '1', 'All RSS reload.\nUse: {0}hg reload <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'git', '1', 'Rss git \'s management.\nGit commands: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'git/channel', '1', 'RSS feeds on their handling of the announcement.\nChannel commands: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'git/channel/add', '1', 'New channel added to the rss.\nUse: {0}git channel add <rss name> <type> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'git/channel/remove', '1', 'Removed from the RSS Channel.\nUse: {0}git channel remove <rss name> <type> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'git/info', '1', 'Prints rss-s condition.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'git/list', '1', 'Optional list of rss.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'git/start', '1', 'New RSS feeds.\nUse: {0}git start <rss name> <type>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'git/stop', '1', 'Rss stop.\nUse: {0}git stop <rss name> <type>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'git/reload', '1', 'Specify rss reload.\nGit reload command: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'git/reload/all', '1', 'All RSS reload.\nUse: {0}git reload <rss name> <type>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'ban', '1', 'Bans the given name or a vhost.\nUse:\nHour and minute: {0}ban <name> <hh:mm> <reason>\nDate, hour and minute: {0}ban <name> <yyyy.mm.dd> <hh:mm> <reason>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'unban', '1', 'Removes a ban from the given name or vhost.\nUse: {0}unban <name or vhost>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'notes', '9', 'Various data can subscribe to this command.\nNotes commands: user | code\nSubmit a note: {0}notes <we note that a code example: schumix> <Includes the text that you want, if you remember the bot.>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'notes/user', '9', 'Notes user management.\nUser commands: register | remove | access | newpassword');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'notes/user/register', '9', 'Add a new user.\nUse: {0}notes user register <password>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'notes/user/remove', '9', 'Remove a user from.\nUse: {0}notes user remove <password>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'notes/user/access', '9', 'The commands needed to use notes password vhost control and activation.\nUse: {0}notes user access <password>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'notes/user/newpassword', '9', 'User password replacement, should a new for old.\nUse: {0}notes user newpassword <old password> <new password>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'notes/code', '9', 'Note retrieve the necessary code.\nUse: {0}notes code <code memo>\nCode command: remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'notes/code/remove', '9', 'Delete the note code.\nUse: {0}notes code remove <notes code>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'message', '9', 'This command message can be left to anyone on the specified channel.\nUse: {0}message <name> <message>\nMessage command: channel');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'message/channel', '9', 'This command message can be left to anyone in the selected channel.\nUse: {0}message channel <channel name> <name> <message>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction', '0', 'Automatically works of code management.\nAutofunction command: hlmessage\nAutomatically works of code management.\nAutofunction commands: kick | mode | hlmessage');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/hlmessage', '0', 'Nicks automatically receiving hl.\nHl message commands: function | update | info\nUse: {0}autofunction hlmessage <message>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/hlmessage/function', '0', 'Use this command to set the state of hl.\nUse: {0}autofunction hlmessage function <status>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/hlmessage/update', '0', 'Update the database list of hl!');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/hlmessage/info', '0', 'Displays the status of hl.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/kick', '1', 'Nicks will be automatically kick.\nKick commands: add | remove | list | channel');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/kick/add', '1', 'Name will automatically kick you out of the current channel.\nUse: {0}autofunction kick add <name> <reason>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/kick/remove', '1', 'Name that kick off automatically cancel the current channel.\nUse: {0}autofunction kick remove <name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/kick/list', '1', 'Names, which will automatically kick on the channel.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/kick/channel', '1', 'Nicks will automatically kick into the specified channel.\nKick channel commands: add | remove | list');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/add', '1', 'Name will automatically kick you out of the specified channel.\nUse: {0}autofunction kick channel <channel name> add <name> <reason>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/remove', '1', 'Name that kick off automatically cancel the specified channel.\nUse: {0}autofunction kick channel <channel name> remove <name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/list', '1', 'Names, which will automatically kick the specified channel.\nUse: {0}autofunction kick channel <channel name> list');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/mode', '1', 'Nicks are automatically given rank.\nMode commands: add | change | remove | list | channel');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/mode/add', '1', 'Add a name to those who automatically receive rank the current channel.\nUse: {0}autofunction mode add <name> <rank>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/mode/change', '1', 'Rank change.\nUse: {0}autofunction mode change <name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/mode/remove', '1', 'Name removed from those who automatically receive rank the current channel.\nUse: {0}autofunction mode remove <name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/mode/list', '1', 'Names that are automatically given rank the current channel.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/mode/channel', '1', 'Names are automatically given rank the specified channel.\nMode channel commands: add | change | remove | list');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/add', '1', 'Add a name to those who automatically receive rank the specified channel.\nUse: {0}autofunction mode channel <channel name> add <name> <rank>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/change', '1', 'Rank Change on the specified channel.\nUse: {0}autofunction mode channel <channel name> change <name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/remove', '1', 'Name removed from those who automatically receive rank the specified channel.\nUse: {0}autofunction mode channel <channel name> remove <name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/list', '1', 'Names that are automatically given rank the specified channel.\nUse: {0}autofunction mode channel <channel name> info');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'reload', '2', 'Reloads the specified program section.\nUse: {0}reload <név>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'weather', '9', 'Displays of the canal, what is the weather in the town.\nUse: {0}weather <name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'game', '9', 'Games start on IRC.\nGame command: start');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'game/start', '9', 'Game launching commands.\nUse: {0}game start <game name>');
