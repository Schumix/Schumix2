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
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'mode', 'Mode haszn�lata: /mode <csatorna> <rang> <n�v>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'fixrank', 'Rang ment�se: /chanserv <rang (sop, aop, hop, vop)> <csatorna> ADD <n�v>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Nick n�v megv�ltoztat�sa: /nick <�j n�v>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'kick', 'Kick haszn�lata: /kick <csatorna> <n�v> (<oka> nem felt�tlen kell)');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'owner', 'Owner m�d bekapcsol�sa a csatorn�n: /msg chanserv SET <csatorna> ownermode on');

-- enUS
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'mode', 'Mode usage: /mode <channel> <rank> <name>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'fixrank', 'Save rank: /chanserv <rank (sop, aop, hop, vop)> <channel> ADD <name>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'nick', 'Nick change usage: /nick <new nick>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'kick', 'Kick usage: /kick <channel> <name> (<reason>)');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'owner', 'Turn on owner mode: /msg chanserv SET <channel> ownermode on');

-- ----------------------------
-- Table structure for "localized_console_command"
-- ----------------------------
DROP TABLE IF EXISTS "localized_console_command";
CREATE TABLE "localized_console_command" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4)    DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- huHU
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'consolelog', 'Console logol�s bekapcsolva.\nConsole logol�s kikapcsolva.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'sys', 'Verzi�: {0}\nPlatform: {0}\nOSVerzi�: {0}\nProgramnyelv: c#\nMem�ria haszn�lat: {0} MB\nFut� sz�lak: {0}\nM�k�d�si id�: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'csatorna', '�j csatorna ahova mostant�l lehet �rni: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/info', 'Jelenleg F�l Oper�tor vagy.\nJelenleg Oper�tor vagy.\nJelenleg Adminisztr�tor vagy.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/list', 'Adminok: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/add', 'A n�v m�r szerepel az admin list�n!\nAdmin hozz�adva: {0}\nJelenlegi jelsz�: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/remove', 'Ilyen n�v nem l�tezik!\nAdmin t�r�lve: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/rank', 'Rang sikeresen m�dos�tva.\nHib�s rang!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin', 'Parancsok: help | list | add | remove');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel/info', 'Bekapcsolva: {0}\nKikapcsolva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel', '{0}: bekapcsolva\n{0}: kikapcsolva\nIlyen csatorna nem l�tezik!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update', 'Sikeresen friss�tve {0} csatorn�n a funkci�k.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update/all', 'Sikeresen friss�tve minden csatorn�n a funkci�k.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/info', 'Bekapcsolva: {0}\nKikapcsolva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel', 'Parancsok: add | remove | info | update | language');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/add', 'A n�v m�r szerepel a csatorna list�n!\nCsatorna hozz�adva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/remove', 'A mester csatorna nem t�r�lhet�!\nIlyen csatorna nem l�tezik!\nCsatorna elt�vol�tva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/update', 'A csatorna inform�ci�k friss�t�sre ker�ltek.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/info', 'Akt�v: {0}\nAkt�v: Nincs inform�ci�.\nInakt�v: {0}\nInakt�v: Nincs inform�ci�.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/language', 'Csatorna nyelvezete sikeresen meg lett v�ltoztatva erre: {0}\nIlyen csatorna nem l�tezik!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Nick megv�ltoztat�sa erre: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'join', 'Kapcsol�d�s ehhez a csatorn�hoz: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'leave', 'Lel�p�s err�l a csatorn�r�l: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reload', '{0} �jra lett ind�tva.\nA programban nincs ilyen r�sz!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'quit', 'Viszl�t :(\nConsole: Program le�ll�t�sa.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'help', 'Ha a parancs m�g� �rod a megadott parancs nev�t vagy a nevet �s alparancs�t inform�ci�t ad a haszn�lat�r�l.\nParancsok: {0}');

-- enUS
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'consolelog', 'Console logging on.\nConsole logging off.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'sys', 'Version: {0}\nPlatform: {0}\nOSVersion: {0}\nProgramming language: c#\nMemory allocation: {0} MB\nThread count: {0}\nUptime: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'csatorna', 'The new channel to write to now: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/info', 'You are half operator now.\nYou are operator now.\nYou are administrator now.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/list', 'Admins: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/add', 'The name is already in the admin list!\nAdmin added to the list: {0}\nPassword: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/remove', 'No such nick!\nAdmin was deleted: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/rank', 'Successfully changed the rank!\nRank error!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin', 'Commands: help | list | add | remove');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/channel/info', 'On: {0}\nOff: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/channel', '{0}: On\n{0}: Off\nNo such channel!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/update', 'Successfully updated the channel functions in: {0}.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/update/all', 'Successfully updated the information of all channels.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/info', 'On: {0}\nOff: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function', '{0}: are on.\n{0}: are off.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel', 'Commands: add | remove | info | update | language');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/add', 'The name is already exists in the channel list.\nAdded channel: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/remove', 'The master channel cannot delete!\nNo such channel!\nDeleted channel: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/update', 'The channel informations are updated.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/info', 'Active: {0}\nActive: Nothing information.\nInactive: {0}\nInactive: Nothing information.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/language', 'Successfully changed the channel language to: {0}\nNo such channel!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'nick', 'Nick changes to: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'join', 'Join to this channel: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'left', 'Part of this channel: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'reload', '{0} reloaded.\nThe program does not contains that part!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'quit', 'Bye :(\nConsole: Program shut down.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'help', 'If you wrote behind the command the command or the name or co-command then gets information about usage.\nCommands: {0}');

-- ----------------------------
-- Table structure for localized_console_command_help
-- ----------------------------
DROP TABLE IF EXISTS "localized_console_command_help";
CREATE TABLE "localized_console_command_help" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4)    DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- huHU
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin', 'Ki�rja az oper�torok vagy adminisztr�torok �ltal haszn�lhat� parancsokat.\nAdmin parancsai: info | list | add | remove | rank | access | newpassword');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/add', '�j admin hozz�ad�sa.\nHaszn�lata: admin add <admin neve>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/remove', 'Admin elt�vol�t�sa.\nHaszn�lata: admin remove <admin neve>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/rank', 'Admin rangj�nak megv�ltoztat�sa.\nHaszn�lata: admin rank <admin neve> <�j rang pl halfoperator: 0, operator: 1, administrator: 2>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/info', 'Ki�rja, hogy �ppen milyen rangja van.\nHaszn�lata: admin info <admin neve>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/list', 'Ki�rja az �sszes admin nev�t aki az adatb�zisban szerepel.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function', 'Funkci�k vez�rl�s�re szolg�l� parancs.\nFunkci� parancsai: channel | update | info\nHaszn�lata glob�lisan:\nGlobalis funkci� kezel�se: function <on vagy off> <funkci� n�v>\nGlob�lis funkci�k kezel�se: function <on vagy off> <funkci� n�v1> <funkci� n�v2> ... stb');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel', 'A megadott csatorn�n ezzel a paranccsal �ll�that�k a funkci�k.\nFunkci� channel parancsai: info\nHaszn�lata:\nCsatorna funkci� kezel�se: function channel <csatorna neve> <on vagy off> <funkci� n�v>\nChannel funkci�k kezel�se: function channel <csatorna neve> <on vagy off> <funkci� n�v1> <funkci� n�v2> ... stb');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel/info', 'Ki�rja a funkci�k �llapot�t.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update', 'Friss�ti a funkci�kat vagy alap�rtelmez�sre �ll�tja.\nFunkci� update parancsai: all\nHaszn�lata:\nM�s channel: function update <csatorna neve>\nJelenlegi csatorna: function update');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update/all', 'Friss�ti az �sszes funkci�t vagy alap�rtelmez�sre �ll�tja.\nHaszn�lata: function update all');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/info', 'Ki�rja a funkci�k �llapot�t.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel', 'Channel parancsai: add | remove | info | update | language');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/add', '�j csatorna hozz�ad�sa.\nHaszn�lata: channel add <csatorna neve> <ha van jelsz� akkor az>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/remove', 'Nem haszn�latos csatorna elt�vol�t�sa.\nHaszn�lata: channel remove <csatorna neve>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/info', 'Az �sszes csatorna ki�r�sa, ami az adatb�zisban van �s a hozz�juk tartoz� inform�ci�k.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/update', 'Friss�ti a csatorn�khoz tartoz� �sszes inform�ci�kat �s alap�rtelmezettre �ll�tja.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/language', 'Friss�ti a csatorna nyelvezet�t.\nHaszn�lata: channel language <csatorna neve> <nyelvezet>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Bot nick nev�nek cser�je.\nHaszn�lata: nick <n�v>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'join', 'Kapcsol�d�s a megadott csatorn�ra.\nHaszn�lata:\nJelsz� n�lk�li csatorna: join <csatorna neve>\nJelsz�val ell�tott csatorna: join <csatorna neve> <jelsz�>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'leave', 'Lel�p�s a megadott csatorn�r�l.\nHaszn�lata: leave <csatona neve>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'quit', 'Bot le�ll�t�s�ra haszn�lhat� parancs.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reload', '�jraind�tja a megadott programr�szt.\nHaszn�lata: reload <n�v>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'consolelog', 'Az irc adatok konzolra �r�s�t enged�lyezi vagy tiltja. Alap�rtelmez�sben ki van kapcsolva.\nHaszn�lata: consolelog <on vagy off>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'sys', 'Ki�rja a botr�l a rendszer inform�ci�kat.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'csatorna', 'A bot csatorn�ra �r�s�t �ll�thatjuk vele.\nHaszn�lata: csatorna <csatorna neve>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'connect', 'Kapcsolod�s az irc szerverhez.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'disconnect', 'Kapcsolat bont�sa.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reconnect', '�jrakapcsolod�s az irc szerverhez.');

-- enUS
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin', 'Print Operators or Administrators can use commands.\nAdmin commands: info | list | add | remove | rank | access | newpassword');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/add', 'Add new admin.\nUse: admin add <admin name>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/remove', 'Removing admin.\nUse: admin remove <admin name>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/rank', 'Admin rank change.\nUse: admin rank <admin name> <new rank e.g. halfoperator: 0, operator: 1, administrator: 2>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/info', "Show the admin's rank.\nUse: admin info <admin name>");
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

-- ----------------------------
-- Table structure for "localized_command"
-- ----------------------------
DROP TABLE IF EXISTS "localized_command";
CREATE TABLE "localized_command" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4)    DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- huHU
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/sys', '3Verzi�: 10{0}\n3Platform: {0}\n3OSVerzi�: {0}\n3Programnyelv: c#\n3Mem�ria haszn�lat:5 {0} MB\n3Mem�ria haszn�lat:8 {0} MB\n3Mem�ria haszn�lat:3 {0} MB\n3M�k�d�si id�: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/help', '3Parancsok: nick | sys\n3Parancsok: ghost | nick | sys\n3Parancsok: ghost | nick | sys | clean\n3Parancsok: sys');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/ghost', 'Ghost paranccsal az els�dleges nick visszaszerz�se.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/nick', 'N�v megv�ltoztat�sa erre: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/nick/identify', 'Azonos�t� jelsz� k�ld�se a kiszolg�l�nak.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/clean', 'Lefoglalt mem�ria felszabad�t�sra ker�l.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'help', 'Ha a parancs m�g� �rod a megadott parancs nev�t vagy a nevet �s alparancs�t inform�ci�t ad a haszn�lat�r�l.\nF� parancsom: {0}xbot');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'xbot', '3Verzi�: 10{0}\n3Parancsok: {0}\nProgrammed by: 3Csaba');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'info', '3Programoz�m: Csaba, Jackneill.\n3Weboldal: https://github.com/megax/Schumix2\n3El�rhet�s�g: [MSN] megax@megaxx.info');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'time', 'Helyi id�: {0}:0{1}\nHelyi id�: {0}:{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'date', 'Ma {0}. 0{1}. 0{2}. {3} napja van.\nMa {0}. 0{1}. {2}. {3} napja van.\nMa {0}. {1}. 0{2}. {3} napja van.\nMa {0}. {1}. {2}. {3} napja van.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'roll', 'Sz�zal�kos ar�nya {0}%');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'whois', 'Jelenleg itt van fent: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'warning', 'Keresnek t�ged itt: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'google', '2Title: Nincs Title.\n2Link: Nincs Link.\n2Title: {0}\n2Link: 3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'translate', 'Rossz nyelvezeti adatok lettek megadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'prime', 'Nem csak sz�mot tartalmaz!\n{0} nem pr�msz�m.\n{0} primsz�m.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/access', 'Hozz�f�r�s enged�lyezve.\nHozz�f�r�s megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/newpassword', 'Jelsz� sikeresen meg lett v�ltoztatva erre: {0}\nA mostani jelsz� nem egyezik, mod�s�t�s megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/info', 'Jelenleg F�l Oper�tor vagy.\nJelenleg Oper�tor vagy.\nJelenleg Adminisztr�tor vagy.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/list', '2Adminok: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/add', 'A n�v m�r szerepel az admin list�n!\nAdmin hozz�adva: {0}\nMostant�l Schumix adminja vagy. A mostani jelszavad: {0}\nHa megszeretn�d v�ltoztatni haszn�ld az {0}admin newpassword parancsot. Haszn�lata: {0}admin newpassword <r�gi> <�j>\nAdmin nick �les�t�se: {0}admin access <jelsz�>');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/remove', 'Ilyen n�v nem l�tezik!\nAdmin t�r�lve: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/rank', 'Rang sikeresen m�dos�tva.\nHib�s rang!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin', '3F�l Oper�tor parancsok!\n3Parancsok: {0}\n3Oper�tor parancsok!\n3Parancsok: {0}\n3Adminisztr�tor parancsok!\n3Parancsok: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'colors', '1teszt1 2teszt2 3teszt3 4teszt4 5teszt5 6teszt6 7teszt7 8teszt8 9teszt9 10teszt10 11teszt11 12teszt12 13teszt13 14teszt14 15teszt15');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Nick megv�ltoztat�sa erre: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'join', 'Kapcsol�d�s ehhez a csatorn�hoz: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'leave', 'Lel�p�s err�l a csatorn�r�l: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/all/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/all', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel', '{0}: bekapcsolva\n{0}: kikapcsolva\nIlyen csatorna nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update', 'Sikeresen friss�tve {0} csatorn�n a funkci�k.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update/all', 'Sikeresen friss�tve minden csatorn�n a funkci�k.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel', '3Parancsok: add | remove | info | update | language');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/add', 'A n�v m�r szerepel a csatorna list�n!\nCsatorna hozz�adva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/remove', 'A mester csatorna nem t�r�lhet�!\nIlyen csatorna nem l�tezik!\nCsatorna elt�vol�tva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/update', 'A csatorna inform�ci�k friss�t�sre ker�ltek.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/info', '3Akt�v: {0}\n3Akt�v: Nincs inform�ci�.\n3Inakt�v: {0}\n3Inakt�v: Nincs inform�ci�.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/language', 'Csatorna nyelvezete sikeresen meg lett v�ltoztatva erre: {0}\nIlyen csatorna nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'plugin/load', '2[Bet�lt�s]: �sszes plugin bet�lt�se 3sikeres.\n2[Bet�lt�s]: �sszes plugin bet�lt�se 5sikertelen.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'plugin/unload', '2[Lev�laszt�s]: �sszes plugin lev�laszt�sa 3sikeres.\n2[Lev�laszt�s]: �sszes plugin lev�laszt�sa 5sikertelen.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'plugin', '{0}: 3loaded.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'quit', 'Viszl�t :(\n{0} le�ll�tott paranccsal.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/start', '{0} m�r el van ind�tva!\n{0} sikeresen el lett ind�tva.\n{0} nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/stop', '{0} m�r le van �ll�tva!\n{0} sikeresen le lett �ll�tva.\n{0} nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/reload', '{0} sikeresen �jra lett ind�tva.\n{0} nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/reload/all', 'Minden rss �jra lett ind�tva.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/channel/add', 'Csatorna sikeresen hozz�adva.\nNem l�tezik ilyen n�v!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/channel/remove', 'Csatorna sikeresen t�r�lve.\nNem l�tezik ilyen n�v!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/start', '{0} m�r el van ind�tva!\n{0} sikeresen el lett ind�tva.\n{0} nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/stop', '{0} m�r le van �ll�tva!\n{0} sikeresen le lett �ll�tva.\n{0} nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/reload', '{0} sikeresen �jra lett ind�tva.\n{0} nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/reload/all', 'Minden rss �jra lett ind�tva.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/channel/add', 'Csatorna sikeresen hozz�adva.\nNem l�tezik ilyen n�v!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/channel/remove', 'Csatorna sikeresen t�r�lve.\nNem l�tezik ilyen n�v!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/info', '3{0} 7{1} Channel: 2{2}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/start', '{0} {1} m�r el van ind�tva!\n{0} {1} sikeresen el lett ind�tva.\n{0} {1} nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/stop', '{0} {1} m�r le van �ll�tva!\n{0} {1} sikeresen le lett �ll�tva.\n{0} {1} nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/reload', '{0} {1} sikeresen �jra lett ind�tva.\n{0} {1} nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/reload/all', 'Minden rss �jra lett ind�tva.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/channel/add', 'Csatorna sikeresen hozz�adva.\nNem l�tezik ilyen n�v!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/channel/remove', 'Csatorna sikeresen t�r�lve.\nNem l�tezik ilyen n�v!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/memory', 'Jelenleg t�l sok mem�ri�t fogyaszt a bot ez�rt ezen funkci� nem el�rhet�!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/warning', 'A k�dban olyan r�szek vannak melyek vesz�lyeztetik a programot. Ez�rt le�llt a ford�t�s!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler', 'Nincs megadva a f� fv! (Schumix)\nNincs megadva a f� class!\nA kimeneti sz�veg t�l hossz� ez�rt nem ker�lt ki�r�sra!\nA k�d sikeresen lefordult csak nincs kimen� �zenet!\nH�tramaradt m�g {0} ki�r�s!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/code', 'Hib�k: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/kill', 'Sz�l kil�ve!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handlekick', '{0} kir�gta a k�vetkez� felhaszn�l�t: {1} oka: {2}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'ban', 'Helytelen d�tum form�tum!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction', '3Parancsok: hlmessage\n3Parancsok: kick | mode | hlmessage');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/info', '3L�tez� nickek: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/update', 'Az adatb�zis sikeresen friss�t�sre ker�lt.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/hlmessage', 'Az �zenet m�dos�t�sra ker�lt.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/add', 'A n�v m�r szerepel a kick list�n!\nKick list�hoz a n�v hozz�adva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/remove', 'Ilyen n�v nem l�tezik!\nKick list�b�l a n�v elt�v�l�t�sra ker�lt: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/list', '3Kick list�n l�v�k: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/add', 'A n�v m�r szerepel a kick list�n!\nKick list�hoz a n�v hozz�adva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/remove', 'Ilyen n�v nem l�tezik!\nKick list�b�l a n�v elt�v�l�t�sra ker�lt: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/list', '3Kick list�n l�v�k: {0}\nIlyen csatorna nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/add', 'A n�v m�r szerepel a mode list�n!\nMode list�hoz a n�v hozz�adva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/change', 'Ilyen n�v nem l�tezik!\n{0} �j rangja: {1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/remove', 'Ilyen n�v nem l�tezik!\nMode list�b�l a n�v elt�v�l�t�sra ker�lt: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/list', '3Voice list�n l�v�k: {0}\n3F�l Oper�tor list�n l�v�k: {0}\n3Oper�tor list�n l�v�k: {0}\n3Adminisztr�tor list�n l�v�k: {0}\n3Tulajdonos list�n l�v�k: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/add', 'A n�v m�r szerepel a mode list�n!\nMode list�hoz a n�v hozz�adva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/change', 'Ilyen n�v nem l�tezik!\n{0} �j rangja: {1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/remove', 'Ilyen n�v nem l�tezik!\nMode list�b�l a n�v elt�v�l�t�sra ker�lt: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/list', '3Voice list�n l�v�k: {0}\n3F�l Oper�tor list�n l�v�k: {0}\n3Oper�tor list�n l�v�k: {0}\n3Adminisztr�tor list�n l�v�k: {0}\n3Tulajdonos list�n l�v�k: {0}\nIlyen csatorna nem l�tezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'message/channel', 'Az �zenet sikeresen feljegyz�sre ker�lt.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'message', 'Az �zenet sikeresen feljegyz�sre ker�lt.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/info', '3Jegyzetek k�djai: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/access', 'Hozz�f�r�s enged�lyezve.\nHozz�f�r�s megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/newpassword', 'Jelsz� sikeresen meg lett v�ltoztatva erre: {0}\nA mostani jelsz� nem egyezik, m�dos�t�s megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/register', 'M�r szerepelsz a felhaszn�l�i list�n!\nSikeresen hozz� vagy adva a felhaszn�l�i list�hoz.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/remove', 'Nincs megadva a jelsz� a t�rl�s meger�s�t�s�hez!\nNem szerepelsz a felhaszn�l�i list�n!\nA jelsz� nem egyezik meg az adatb�zisban t�roltal!\nT�rl�s meg lett szak�tva!\nSikeresen t�r�lve lett a felhaszn�l�d.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/code/remove', 'Ilyen k�d nem szerepel a list�n!\nA jegyzet sikeresen t�rl�sre ker�lt.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/code', '3Jegyzet: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes', 'Nincs megadva jegyzetnek semmi se!\nA jegyzet k�dneve m�r szerepel az adatb�zisban!\nJegyzet k�dja: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/warning', 'Jelenleg nem szerepelsz a jegyzetek felhaszn�l�i list�j�n!\nAhoz hogy hozz�add magad nem kell m�st tenned mint az al�bbi parancsot v�grehajtani. (Lehet�leg priv�t �zenetk�nt nehogy m�s megtudja.)\n{0}notes user register <jelsz�>\nFelhaszn�l�i adatok friss�t�se (ha nem fogadn� el adataidat) pedig: {0}notes user access <jelsz�>');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'message2', '�zenetet hagyta neked: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reload', '{0} �jra lett ind�tva.\nA programban nincs ilyen r�sz!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'weather', '12Id�j�r�s otthon!\n5{0} 12id�j�r�sa!\n3Nappal: {0}\n3�jszaka: {0}\nNem szerepel ilyen v�ros a list�n!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handlejoin/random', 'Hello\nCs��\nHy\nSzevasz\n�dv\nSzervusz\nAloha\nJ� napot\nSzia\nHi\nSzerbusz\nHali\nSzeva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handlejoin', 'J� reggelt {0}\nJ� est�t {0}\n�dv f�n�k');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handleleft/random', 'Viszl�t\nBye');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handleleft', 'J��t {0}');

-- enUS
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/sys', '3Version: 10{0}\n3Platform: {0}\n3OSVersion: {0}\n3Programming language: c#\n3Memory allocation:5 {0} MB\n3Memory allocation:8 {0} MB\n3Memory allocation:3 {0} MB\n3Uptime: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/help', '3Commands: nick | sys\n3Commands: ghost | nick | sys\n3Commands: ghost | nick | sys | clean\n3Commands: sys');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/ghost', 'Return to the primary nick.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/nick', 'Change nick to: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/nick/identify', 'Send identifcation password to the server.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/clean', 'Allocated memory will be freed.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'help', 'If you wrote behind the command the command or the name or co-command then gets information about usage.\nMain command: {0}xbot');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'xbot', '3Version: 10{0}\n3Commands: {0}\nProgrammed by: 3Csaba');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'info', '3My programmer(s): Csaba, Jackneill.\n3Website: https://github.com/megax/Schumix2\n3Contact: [MSN] megax@megaxx.info');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'time', 'Local time: {0}:0{1}\nLocal time: {0}:{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'date', "Today is {0}. 0{1}. 0{2}. and {3}'s day.\nToday is {0}. 0{1}. {2}. and {3}'s day.\nToday is {0}. {1}. 0{2}. and {3}'s day.\nToday is {0}. {1}. {2}. and {3}'s day.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'roll', 'Pencentage rate: {0}%');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'whois', 'Now online here: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'warning', 'Looking for you here: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'google', '2Title: Nothing Title.\n2Link: Nothing Link.\n2Title: {0}\n2Link: 3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'translate', 'Nothing translated text.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'prime', 'This is not a numeric text!\n{0} is not a prime number.\n{0} is a prime number.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/access', 'Access granted.\nAccess denied.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/newpassword', 'Successfully changed to password to: {0}\nThe current password does not match, modification denied!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/info', 'You are half operator now.\nYou are operator now.\nYou are administrator now.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/list', '2Admins: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/add', "The name is already in the admin list!\nAdmin added to the list: {0}\nYou are schumix's admin now. Your current password is: {0}\nIf you want to change it, use this command: {0}admin newpassword. Usage: {0}admin newpassword <Old> <New>\nAdmin nick confirmation: {0}admin access <Password>");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/remove', 'No such nick!\nAdmin was deleted: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/rank', 'Successfully changed the rank!\nRank error!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin', '3half operator commands!\n3Commands: {0}\n3Operator commands!\n3Commands: {0}\n3Administrator commands!\n3Commands: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'colors', '1test1 2test2 3test3 4test4 5test5 6test6 7test7 8test8 9test9 10test10 11test11 12test12 13test13 14test14 15test15');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'nick', 'Nick changes to: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'join', 'Join to this channel: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'leave', 'Part of this channel: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/info', '2On: {0}\n2Off: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/all/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/all', '{0}: are on.\n{0}: are off.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/channel/info', '2On: {0}\n2Off: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/channel', '{0}: On\n{0}: Off\nNo such channel!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/update', 'Successfully updated the channel functions in: {0}.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/update/all', 'Successfully updated the information of all channels.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function', '{0}: are on.\n{0}: are off.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel', '3Commands: add | remove | info | update | language');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/add', 'The name is already exists in the channel list.\nAdded channel: {0}\nNo such channel!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/remove', 'The master channel cannot delete!\nNo such channel!\nDeleted channel: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/update', 'The channel informations are updated.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/info', '3Active: {0}\n3Active: Nothing information.\n3Inactive: {0}\n3Inactive: Nothing information.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/language', 'Successfully changed the channel language to: {0}\nNo such channel!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'plugin/load', '2[Load]: All plugins 3done.\n2[Load]: All plugins 5failed.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'plugin/unload', '2[Unload]: All plugins 3done.\n2[Unload]: All plugins 5failed.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'plugin', '{0}: 3loaded.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'quit', 'Bye :(\n{0} shutted down me with command.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/list', '2List:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/start', '{0} already translated!\n{0} successfully started.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/stop', '{0} already stopped!\n{0} successfully stopped.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/reload', '{0} successfully restarted.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/reload/all', 'All of Rss is restarted.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/channel/add', 'Successfully added channel.\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/channel/remove', 'Successfully deleted channel!\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/list', '2List:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/start', '{0} already translated!\n{0} successfully started.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/stop', '{0} already stopped!\n{0} successfully stopped.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/reload', '{0} successfully restarted.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/reload/all', 'All of Rss is restarted.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/channel/add', 'Sccessfully added channel!\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/channel/remove', 'Successfully deleted channel!.\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/info', '3{0} 7{1} Channel: 2{2}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/list', '2List:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/start', '{0} {1} already translated!\n{0} {1} successfully started.\n{0} {1} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/stop', '{0} {1} already stopped!\n{0} {1} successfully stopped.\n{0} {1} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/reload', '{0} {1} successfully restarted.\n{0} {1} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/reload/all', 'All of Rss is restarted.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/channel/add', 'Successfully added channel!\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/channel/remove', 'Successfully deleted channel!\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'compiler/memory', 'Currently too many memory is allocated so this function is disabled!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'compiler/warning', 'This code contains dangerous parts. Compiling stopped!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'compiler', 'The main function is not specified! (Schumix)\nThe main class is not specified!\nThe output text is too long so did not written out!\nSuccessfully compiled the code, only nothing output text!\nLeft: {0} line!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'compiler/code', 'Errors: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'compiler/kill', 'Killed thread!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handlekick', '{0} kicked that user: {1} reason: {2}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'ban', 'Date format error!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction', '3Commands: hlmessage\n3Commands: kick | mode | hlmessage');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/hlmessage/info', '3Exciting nicks: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/hlmessage/update', 'Successfully updated the database.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/hlmessage/function', '{0}: are on\n{0}: are off.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/hlmessage', 'The message has been modified!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/add', 'The name is already on the kick list!\nThe name has been added to the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/remove', 'No such name!\nThe name has been deleted from the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/list', '3On the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/add', 'The name is already on the kick list!A n�v m�r szerepel a kick list�n!\nKick list�hoz a n�v hozz�adva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/remove', 'No such name!\nThe name has been deleted from the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/list', '3On the kick list: {0}\nNo such channel!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/add', 'The name is already on the mode list!\nThe name added to the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/change', "No such name!\n{0}'s new rank: {1}");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/remove', 'No such name!\The name has been deleted from the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/list', '3Voice list: {0}\n3Half Operator list: {0}\n3Operator list: {0}\n3Administrator list: {0}\n3Owner list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/add', 'The name is already on the mode list!\nThe name added to the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/change', "No such name!\n{0}'s new rank: {1}");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/remove', 'No such name!\nThe name has been deleted from the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/list', '3Voice list: {0}\n3Half Operator list: {0}\n3Operator list: {0}\n3Administrator list: {0}\n3Owner list: {0}\nNo such channel!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'message/channel', 'Successfully recorded the message.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'message', 'Successfully recorded the message.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/info', '3Recorded message (quotes) codes: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/user/access', 'Access granted.\nAccess denied!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/user/newpassword', 'Successfully changed the password to: {0}\nThe password does not match! Modification is denied!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/user/register', 'You are already on the user list!\nSuccessfully added you to the user list!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/user/remove', 'The password for confirmation is not specified!The password for delete confirmation is not specified!\nYou are not in user list!\nYour password does not match which is stored in database!\nThe password does not match which is stored in database!\nThe deleting is aborted!\nSuccessfully deleted your account.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/code/remove', 'This code is not in the list!\nSuccessfully deleted the note.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/code', '3Note: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes', "No text found for note!\nNote's codename is alreadyin the database!\nNote's code: {0}");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/warning', "You are not in the note's user list!\nIf you want to add yourself, you have to do the following command. (Must be a private message, do not gather info someone else.)\n{0}notes user register <password>\nUpdating user data (If do not accept your datas) Do: {0}notes user access <password>");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'message2', 'Left the note for you: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'reload', '{0} reloaded.\nThe program does not contains that part!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'weather', '12Local weather!\n5{0} 12weather!\n3Day: {0}\n3Night: {0}\nNo such city in the list!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handlejoin/random', 'Hello\nHy\nHi\nGood afternoon');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handlejoin', 'Good Morning {0}\nGood Night {0}\nWelcome boss!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handleleft/random', 'ByeBye\nBye');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handleleft', 'Goodbye {0}');

-- ----------------------------
-- Table structure for "localized_command_help"
-- ----------------------------
DROP TABLE IF EXISTS "localized_command_help";
CREATE TABLE "localized_command_help" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4)    DEFAULT 'enUS',
Command TEXT,
Rank INTEGER DEFAULT 0,
Text TEXT
);

-- huHU
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'xbot', '9', 'Felhaszn�l�k sz�m�ra haszn�lhat� parancslista.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'info', '9', 'Kis le�r�s a botr�l.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'whois', '9', 'A parancs seg�ts�g�vel megtudhatjuk hogy egy nick milyen csatorn�n van fent.\nHaszn�lata: {0}whois <nick>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'roll', '9', 'Cs�pp szorakoz�s a wowb�l, m�r ha valaki felismeri :P');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'date', '9', 'Az aktu�lis d�tumot �rja ki �s a hozz� tartoz� n�vnapot.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'time', '9', 'Az aktu�lis id�t �rja ki.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'google', '9', 'Ha sz�ks�ged lenne valamire a google-b�l nem kell hozz� weboldal csak ez a parancs.\nHaszn�lata: {0}google <ide j�n a keresett sz�veg>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'translate', '9', 'Ha r�gt�n k�ne ford�tani m�sik nyelvre vagy -r�l valamit, akkor megteheted ezzel a parancsal.\nHaszn�lata: {0}translate <kiindul�si nyelv|c�l nyelv> <sz�veg>\nP�ld�ul: {0}translate hu|en Sz�p sz�veg.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'irc', '9', 'N�h�ny parancs haszn�lata az IRC-n.\nHaszn�lata: {0}irc <parancs neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'calc', '9', 'T�bb funkci�s sz�mol�g�p.\nHaszn�lata: {0}calc <sz�m>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'warning', '9', 'Figyelmeztet� �zenet k�ld�se, hogy keresik ezen a csatorn�n vagy egy tetsz�leges �zenet k�ld�se.\nHaszn�lata: {0}warning <ide j�n a szem�ly> <ha nem csak felh�v�st k�lden�l, hanem saj�t �zenetet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'sha1', '9', 'Sha1 k�dol�ss� �talak�t� parancs.\nHaszn�lata: {0}sha1 <�talak�tand� sz�veg>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'md5', '9', 'Md5 k�dol�ss� �talak�t� parancs.\nHaszn�lata: {0}md5 <�talak�tand� sz�veg>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'prime', '9', 'Meg�llap�tja hogy a sz�m pr�msz�m-e. Csak eg�sz sz�mmal tud sz�molni!\nHaszn�lata: {0}prime <sz�m>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin', '0', 'Ki�rja az oper�torok vagy adminisztr�torok �ltal haszn�lhat� parancsokat.\nAdmin parancsai: info | list | add | remove | rank | access | newpassword');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/add', '0', '�j admin hozz�ad�sa.\nHaszn�lata: {0}admin add <admin neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/remove', '0', 'Admin elt�vol�t�sa.\nHaszn�lata: {0}admin remove <admin neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/rank', '0', 'Admin rangj�nak megv�ltoztat�sa.\nHaszn�lata: {0}admin rank <admin neve> <�j rang pl halfoperator: 0, operator: 1, administrator: 2>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/info', '0', 'Ki�rja �ppen milyen rangod van.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/list', '0', 'Ki�rja az �sszes admin nev�t aki az adatb�zisban szerepel.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/access', '0', 'Az admin parancsok haszn�lat�hoz sz�ks�ges jelsz� ellen�rz� �s vhost aktiv�l�.\nHaszn�lata: {0}admin access <jelsz�>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/newpassword', '0', 'Az admin jelszav�nak cser�je ha �j k�ne a r�gi helyett.\nHaszn�lata: {0}admin newpassword <r�gi jelsz�> <�j jelsz�>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'colors', '0', 'Adott sk�l�j� sz�nek ki�r�sa amit lehet haszn�lni IRC-n.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'nick', '0', 'Bot nick nev�nek cser�je.\nHaszn�lata: {0}nick <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'join', '0', 'Kapcsol�d�s a megadott csatorn�ra.\nHaszn�lata:\nJelsz� n�lk�li csatorna: {0}join <csatorna neve>\nJelsz�val ell�tott csatorna: {0}join <csatorna neve> <jelsz�>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'leave', '0', 'Lel�p�s a megadott csaton�r�l.\nHaszn�lata: {0}leave <csatona neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel', '1', 'Channel parancsai: add | remove | info | update | language');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/add', '1', '�j csatorna hozz�ad�sa.\nHaszn�lata: {0}channel add <csatorna neve> <ha van jelsz� akkor az>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/remove', '1', 'Nem haszn�latos channel elt�vol�t�sa.\nHaszn�lata: {0}channel remove <csatorna neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/info', '1', '�sszes channel ki�r�sa ami az adatb�zisban van �s a hozz�juk tartoz� inform�ci�k.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/update', '1', 'A csatorn�khoz tartoz� �sszes inform�ci� friss�t�se, alap�rtelmez�sre �ll�t�sa.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/language', '1', 'Friss�ti a csatorna nyelvezet�t.\nHaszn�lata: {0}channel language <csatorna neve> <nyelvezet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function', '1', 'Funkci�k vez�rl�s�re szolg�l� parancs.\nFunkci� parancsai: channel | all | update | info\nHaszn�lata ahol tart�zkodsz:\nChannel funkci� kezel�se: {0}function <on vagy off> <funkci� n�v>\nChannel funkci�k kezel�se: {0}function <on vagy off> <funkci� n�v1> <funkci� n�v2> ... stb');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/channel', '1', 'Megadott csatorn�n �llithat�k ezzel a parancsal a funkci�k.\nFunkci� channel parancsai: info\nHaszn�lata:\nChannel funkci� kezel�se: {0}function channel <csatorna neve> <on vagy off> <funkci� n�v>\nChannel funkci�k kezel�se: {0}function channel <csatorna neve> <on vagy off> <funkci� n�v1> <funkci� n�v2> ... stb');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/channel/info', '1', 'Ki�rja a funkci�k �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/all', '1', 'Glob�lis funkci�k kezel�se.\nFunkci� all parancsai: info\nGlob�lis funkci� kezel�se: {0}function all <on vagy off> <funkci� n�v>\nGlob�lis funkci�k kezel�se: {0}function all <on vagy off> <funkci� n�v1> <funkci� n�v2> ... stb');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/all/info', '1', 'Ki�rja a funkci�k �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/update', '1', 'Friss�ti a funkci�kat vagy alap�rtelmez�sre �ll�tja.\nFunkci� update parancsai: all\nHaszn�lata:\nM�s channel: {0}function update <csatorna neve>\nAhol tart�zkodsz channel: {0}function update');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/update/all', '1', 'Friss�ti az �sszes funkci�t vagy alap�rtelmez�sre �ll�tja.\Haszn�lata: {0}function update all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/info', '1', 'Ki�rja a funkci�k �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'kick', '1', 'Kir�gja a nick-et a megadott csatorn�r�l.\nHaszn�lata:\nCsak kir�g�s: {0}kick <csatorna neve> <n�v>\nKir�g�s okkal: {0}kick <csatorna neve> <n�v> <oka>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mode', '1', 'Megv�ltoztatja a nick rangj�t megadott csatorn�n.\nHaszn�lata: {0}mode <rang> <n�v vagy nevek>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'plugin', '2', 'Ki�rja milyen pluginok vannak bet�ltve.\nPlugin parancsok: load | unload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'plugin/load', '2', 'Bet�lt minden plugint.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'plugin/unload', '2', 'Elt�vol�t minden plugint.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'quit', '2', 'Bot le�ll�t�s�ra haszn�lhat� parancs.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2', '9', 'Parancsok: nick | sys\nParancsok: ghost | nick | sys\nParancsok: ghost | nick | sys | clean\nParancsok: sys');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/sys', '9', 'Ki�rja a program inform�ci�it.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/ghost', '1', 'Kil�pteti a f� nick-et ha regisztr�lva van.\nHaszn�lata: {0} ghost');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/nick', '0', 'Bot nick nev�nek cser�je.\nParancsok: identify\nHaszn�lata: {0} nick <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/nick/identify', '0', 'Aktiv�lja a f� nick jelszav�t.\nHaszn�lata: {0} nick identify');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/clean', '2', 'Felszabad�tja a lefoglalt mem�ri�t.\nHaszn�lata: {0} clean');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn', '1', 'Svn rss-ek kezel�se.\nSvn parancsai: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/channel', '1', 'Rss csatorn�kra val� ki�r�s�nak kezel�se.\nChannel parancsai: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/channel/add', '1', '�j csatorna hozz�ad�sa az rss-hez.\nHaszn�lata: {0}svn channel add <rss neve> <csatorna neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/channel/remove', '1', 'Nem haszn�latos csatorna elt�vol�t�sa az rss-b�l.\nHaszn�lata: {0}svn channel remove <rss neve> <csatorna neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/info', '1', 'Ki�rja az rss-ek �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/list', '1', 'V�laszthat� rss-ek list�ja.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/start', '1', '�j rss bet�lt�se.\nHaszn�lata: {0}svn start <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/stop', '1', 'Rss le�ll�t�sa.\nHaszn�lata: {0}svn stop <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/reload', '1', 'Megadott rss �jrat�lt�se.\nSvn reload parancsai: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/reload/all', '1', 'Minden rss �jrat�lt�se.\nHaszn�lata: {0}svn reload <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg', '1', 'Hg rss-ek kezel�se.\nHg parancsai: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/channel', '1', 'Rss csatorn�kra val� ki�r�s�nak kezel�se.\nChannel parancsai: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/channel/add', '1', '�j csatorna hozz�ad�sa az rss-hez.\nHaszn�lata: {0}hg channel add <rss neve> <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/channel/remove', '1', 'Nem haszn�latos csatorna elt�vol�t�sa az rss-b�l.\nHaszn�lata: {0}hg channel remove <rss neve> <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/info', '1', 'Ki�rja az rss-ek �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/list', '1', 'V�laszthat� rss-ek list�ja.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/start', '1', '�j rss bet�lt�se.\nHaszn�lata: {0}hg start <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/stop', '1', 'Rss le�ll�t�sa.\nHaszn�lata: {0}hg stop <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/reload', '1', 'Megadott rss �jrat�lt�se.\nHg reload parancsai: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/reload/all', '1', 'Minden rss �jrat�lt�se.\nHaszn�lata: {0}hg reload <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git', '1', 'Git rss-ek kezel�se.\nGit parancsai: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/channel', '1', 'Rss csatorn�kra val� kiir�s�nak kezel�se.\nChannel parancsai: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/channel/add', '1', '�j csatorna hozz�ad�sa az rss-hez.\nHaszn�lata: {0}git channel add <rss neve> <tipus> <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/channel/remove', '1', 'Nem haszn�latos csatorna elt�vol�t�sa az rss-b�l.\nHaszn�lata: {0}git channel remove <rss neve> <tipus> <csatorna neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/info', '1', 'Ki�rja az rss-ek �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/list', '1', 'V�laszthat� rss-ek list�ja.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/start', '1', '�j rss bet�lt�se.\nHaszn�lata: {0}git start <rss neve> <tipus>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/stop', '1', 'Rss le�ll�t�sa.\nHaszn�lata: {0}git stop <rss neve> <tipus>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/reload', '1', 'Megadott rss �jrat�lt�se.\nGit reload parancsai: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/reload/all', '1', 'Minden rss �jrat�lt�se.\nHaszn�lata: {0}git reload <rss neve> <tipus>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'ban', '1', 'Tilt�st rak a megadott n�vre vagy vhost-ra.\nHaszn�lata:\n�ra �s perc: {0}ban <n�v> <��:pp> <oka>\nD�tum, �ra �s perc: {0}ban <n�v> <����.hh.nn> <��:pp> <oka>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'unban', '1', 'Feloldja a tilt�st a n�vr�l vagy vhost-r�l ha szerepel a bot rendszer�ben.\nHaszn�lata: {0}unban <n�v vagy vhost>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes', '9', 'K�l�nb�z� adatokat jegyezhet�nk fel a seg�ts�g�vel.\nJegyzet parancsai: user | code\nJegyzet bek�ld�se: {0}notes <egy k�d amit megjegyz�nk pl: schumix> <amit feljegyezn�l>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/user', '9', 'Jegyzet felhaszn�l� kezel�se.\nUser parancsai: register | remove | access | newpassword');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/user/register', '9', '�j felhaszn�l� hozz�ad�sa.\nHaszn�lata: {0}notes user register <jelsz�>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/user/remove', '9', 'Felhaszn�l� elt�vol�t�sa.\nHaszn�lata: {0}notes user remove <jelsz�>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/user/access', '9', 'Az jegyzet parancsok haszn�lat�hoz sz�ks�ges jelsz� ellen�rz� �s vhost aktiv�l�.\nHaszn�lata: {0}notes user access <jelsz�>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/user/newpassword', '9', 'Felhaszn�l� jelszav�nak cser�je ha �j k�ne a r�gi helyett.\nHaszn�lata: {0}notes user newpassword <r�gi jelsz�> <�j jelsz�>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/code', '9', 'Jegyzet kiolvas�s�hoz sz�ks�ges k�d.\nHaszn�lata: {0}notes code <jegyzet k�dja>\nK�d parancsai: remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/code/remove', '9', 'T�rli a jegyzetet k�d alapj�n.\nHaszn�lata: {0}notes code remove <jegyzet k�dja>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'message', '9', 'Ezzel a paranccsal �zenetet lehet hagyni b�rkinek a megadott csatorn�n.\nHaszn�lata: {0}message <n�v> <�zenet>\n�zenet parancsai: channel');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'message/channel', '9', 'Ezzel a paranccsal �zenetet lehet hagyni b�rkinek a kiv�lasztott csatorn�n.\nHaszn�lata: {0}message channel <csatorna neve> <n�v> <�zenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction', '0', 'Aut�matikusan m�k�d� k�dr�szek kezel�se.\nAutofunkcio parancsai: hlmessage\nAut�matikusan m�k�d� k�dr�szek kezel�se.\nAutofunkcio parancsai: kick | mode | hlmessage');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/hlmessage', '0', 'Aut�matikusan hl-t kap� nick-ek kezel�se.\nHl �zenet parancsai: function | update | info\nHaszn�lata: {0}autofunction hlmessage <�zenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/function', '0', 'Ezzel a paranccsal �ll�that� a hl �llapota.\nHaszn�lata: {0}autofunction hlmessage function <�llapot>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/update', '0', 'Friss�ti az adatb�zisban szerepl� hl list�t!');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/info', '0', 'Ki�rja a hl-ek �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick', '1', 'Automatikusan kir�g�sra ker�l� nick-ek kezel�se.\nKick parancsai: add | remove | list | channel');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/add', '1', 'Kirugand� nev�nek hozz�ad�sa ahol tart�zkodsz.\nHaszn�lata: {0}autofunction kick add <n�v> <oka>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/remove', '1', 'Kirugand� nev�nek elt�vol�t�sa ahol tart�zkodsz.\nHaszn�lata: {0}autofunction kick remove <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/list', '1', 'Ki�rja a kirugand�k �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/channel', '1', 'Automatikusan kirug�sra ker�l� nick-ek kezel�se megadott csatorn�n.\nKick channel parancsai: add | remove | list');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/add', '1', 'Kirugand� nev�nek hozz�ad�sa megadott csatorn�n.\nHaszn�lata: {0}autofunction kick channel <csatorna neve> add <n�v> <oka>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/remove', '1', 'Kirugand� nev�nek elt�vol�t�sa megadott csatorn�n.\nHaszn�lata: {0}autofunction kick channel <csatorna neve> remove <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/list', '1', 'Ki�rja a kirugand�k �llapot�t a megadott csatorn�n.\nHaszn�lata: {0}autofunction kick channel <csatorna neve> list');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode', '1', 'Automatikusan rangot kap� nick-ek kezel�se.\nMode parancsai: add | change | remove | list | channel');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/add', '1', 'Rangot kap� nev�nek hozz�ad�sa ahol tart�zkodsz.\nHaszn�lata: {0}autofunction mode add <n�v> <rang>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/change', '1', 'Rang megv�ltoztat�sa.\nHaszn�lata: {0}autofunction mode change <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/remove', '1', 'Rangot kap� nev�nek elt�vol�t�sa ahol tart�zkodsz.\nHaszn�lata: {0}autofunction mode remove <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/list', '1', 'Ki�rja a rangot kap�k �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel', '1', 'Automatikusan rangot kap� nick-ek kezel�se megadot csatorn�n.\nMode channel parancsai: add | change | remove | list');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/add', '1', 'Rangot kap� nev�nek hozz�ad�sa megadott csatorn�n.\nHaszn�lata: {0}autofunction mode channel <csatorna neve> add <n�v> <rang>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/change', '1', 'Rang megv�ltoztat�sa a megadott csatorn�n.\nHaszn�lata: {0}autofunction mode channel <csatorna neve> change <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/remove', '1', 'Rangot kap� nev�nek elt�vol�t�sa megadott csatorn�n.\nHaszn�lata: {0}autofunction mode channel <csatorna neve> remove <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/list', '1', 'Ki�rja a rangot kap�k �llapot�t a megadott csatorn�n.\nHaszn�lata: {0}autofunction mode channel <csatorna neve> info');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'reload', '2', '�jraind�tja a megadott programr�szt.\nHaszn�lata: {0}reload <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'weather', '9', 'Megmondja az id�j�r�st a megadott v�rosban.\nHaszn�lata: {0}weather <v�ros>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'game', '9', 'J�t�kok ind�t�sa irc-n.\nJ�t�k parancsai: start');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'game/start', '9', 'J�t�k ind�t�s�ra szolg�l� parancs.\nHaszn�lata: {0}game start <j�t�k neve>');

-- enUS
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'xbot', '9', 'Users to use the command list.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'info', '9', 'Small description of the bot.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'whois', '9', 'This command tells you which channel is that a nick above.\nHaszn�lata: {0}whois <nick>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'roll', '9', 'Tiny fun from the World of Warcraft, if you recognize anyone: P');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'date', '9', 'Writes the current date and name day.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'time', '9', 'The current time is printed.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'google', '9', 'If you need something from Google without a website, you just type this command.\nUse: {0}google <Your text goes here>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'translate', '9', 'If you want to translate immediately from one language to another, you just type this command.\nUse: {0}translate <base language|target language> <text>\ne.g.: {0}translate hu|en Sz�p sz�veg.');
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
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn', '1', "Rss svn 's management.\nSvn commands: channel | info | list | start | stop | reload");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/channel', '1', 'RSS feeds on their handling of the announcement.\nChannel commands: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/channel/add', '1', 'New channel added to the rss.\nUse: {0}svn channel add <rss name> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/channel/remove', '1', 'Removed from the RSS Channel.\nUse: {0}svn channel remove <rss name> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/info', '1', 'Prints rss-s condition.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/list', '1', 'Optional list of rss.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/start', '1', 'New RSS feeds.\nUse: {0}svn start <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/stop', '1', 'Rss stop.\nUse: {0}svn stop <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/reload', '1', 'Specify rss reload.\nSvn reload command: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'svn/reload/all', '1', 'All RSS reload.\nUse: {0}svn reload <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg', '1', "Rss hg 's management.\nHg commands: channel | info | list | start | stop | reload");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/channel', '1', 'RSS feeds on their handling of the announcement.\nChannel commands: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/channel/add', '1', 'New channel added to the rss.\nUse: {0}hg channel add <rss name> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/channel/remove', '1', 'Removed from the RSS Channel.\nUse: {0}hg channel remove <rss name> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/info', '1', 'Prints rss-s condition.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/list', '1', 'Optional list of rss.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/start', '1', 'New RSS feeds.\nUse: {0}hg start <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/stop', '1', 'Rss stop.\nUse: {0}hg stop <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/reload', '1', 'Specify rss reload.\nHg reload command: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'hg/reload/all', '1', 'All RSS reload.\nUse: {0}hg reload <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'git', '1', "Rss git 's management.\nGit commands: channel | info | list | start | stop | reload");
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
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'reload', '2', 'Reloads the specified program section.\nUse: {0}reload <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'weather', '9', 'Displays of the canal, what is the weather in the town.\nUse: {0}weather <name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'game', '9', 'Games start on IRC.\nGame command: start');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'game/start', '9', 'Game launching commands.\nUse: {0}game start <game name>');

-- ----------------------------
-- Table structure for "localized_warning"
-- ----------------------------
DROP TABLE IF EXISTS "localized_warning";
CREATE TABLE "localized_warning" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4)    DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- huHU
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoName', 'A n�v nincs megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoValue', 'Nincs param�ter!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'No1Value', 'Nincs megadva egy param�ter!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'FaultyQuery', 'Hib�s lek�rdez�s!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoWhoisName', 'Nincs megadva a keresend� szem�ly neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoGoogleText', 'Nincs megadva a keresend� sz�veg!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTranslateText', 'Nincs megadva a ford�tand� sz�veg!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTranslateLanguage', 'Nincs megadva melyik nyelvr�l melyikre ford�tsa le!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoNumber', 'Nincs megadva sz�m!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoPassword', 'Nincs megadva a jelsz�!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoOldPassword', 'Nincs megadva a r�gi jelsz�!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoNewPassword', 'Nincs megadva az �j jelsz�!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoOperator', 'Nem vagy Oper�tor!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoAdministrator', 'Nem vagy Adminisztr�tor!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoChannelName', 'Nincs megadva a csatorna neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoRank', 'Nincs megadva a rang!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoFunctionName', 'Nincs megadva a funkci� neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoFunctionStatus', 'Nincs megadva a funkci� �llapota!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoCommand', 'Nincs megadva a parancs!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTypeName', 'Nincs megadva a t�pus neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'CapsLockOff', 'Kapcsold ki a caps lock-ot!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTime', 'Nincs megadva az id�!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoBanNameOrVhost', 'Nincs megadva a kitiltand� neve vagy a vhost!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoUnbanNameOrVhost', 'Nincs megadva a kitiltott neve vagy a vhost!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'BanList', 'M�r szerepel a tilt� list�n!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'BanList1', 'Sikeresen hozz� lett adva a tilt� list�hoz.');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'UnbanList', 'Nem szerepel a tilt� list�n!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'UnbanList1', 'Sikeresen t�r�lve lett a tilt� list�hoz.');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'RecurrentFlooding', 'Ism�tl�d� flooddal!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'StopFlooding', '�llj le a floodol�ssal!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoMessage', '�zenet nincs megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoCode', 'A k�d nincs megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoReason', 'Nincs megadva az ok!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoChannelLanguage', 'Nincs megadva a csatorna nyelvezete!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoDataNoCommand', 'Az adataid nem megfelel�ek ez�rt nem folytathat� a parancs!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoCityName', 'Nem adott meg egy v�rosnevet sem!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoMessageFunction', 'A funkci� jelenleg nem �zemel!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NotaChannelHasBeenSet', 'Nem csatorna lett megadva!');

-- enUS
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoName', 'The name is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoValue', 'The parameters are not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'No1Value', 'A parameter was not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'FaultyQuery', 'Syntax error!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoWhoisName', "The searching person's name are not specified!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoGoogleText', 'The searching text is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoTranslateText', 'The text to be translated is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoTranslateLanguage', 'Which language to other language text is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoNumber', 'The number is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoPassword', 'The password is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoOldPassword', 'The old password is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoNewPassword', 'The new password is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoOperator', 'You are not an operator!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoAdministrator', 'You are not an administrator!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoChannelName', 'The channel name is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoRank', 'The rank is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoFunctionName', 'The function name is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoFunctionStatus', 'The function status is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoCommand', 'The command is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoTypeName', 'The type is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'CapsLockOff', 'Turn caps lock OFF!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoTime', 'The time is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoBanNameOrVhost', "The banning person's name or his/her vhost is not specified!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoUnbanNameOrVhost', "The unbanning person's name or his/her vhost is not specified!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'BanList', 'Already on the bann list!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'BanList1', 'Successfully added to the bann list!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'UnbanList', 'He/She is not on the bann list!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'UnbanList1', 'Successfully deleted from the bann list!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'RecurrentFlooding', 'Recurrent flooding!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'StopFlooding', 'Stop flooding!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoMessage', 'Message is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoCode', 'The code is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoReason', 'Reason is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoChannelLanguage', 'The channel language is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoDataNoCommand', 'Your datas are bad, so aborted the command!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoCityName', 'No such city name!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoMessageFunction', 'This function is currently not operating!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NotaChannelHasBeenSet', 'Not a channel has been set!');
