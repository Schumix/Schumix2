-- ----------------------------
-- Table structure for localized_console_command
-- ----------------------------
DROP TABLE IF EXISTS `localized_console_command`;
CREATE TABLE `localized_console_command` (
  `Id` int(8) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- huHU
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'consolelog', 'Console logol�s bekapcsolva.\nConsole logol�s kikapcsolva.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'sys', 'Verzi�: {0}\nPlatform: {0}\nOSVerzi�: {0}\nProgramnyelv: c#\nMem�ria haszn�lat: {0} MB\nFut� sz�lak: {0}\nM�k�d�si id�: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'csatorna', '�j csatorna ahova mostant�l lehet �rni: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/info', 'Jelenleg F�l Oper�tor vagy.\nJelenleg Oper�tor vagy.\nJelenleg Adminisztr�tor vagy.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/list', '2Adminok: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/add', 'A n�v m�r szerepel az admin list�n!\nAdmin hozz�adva: {0}\nJelenlegi jelsz�:');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/remove', 'Ilyen n�v nem l�tezik!\nAdmin t�r�lve: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/rank', 'Rang sikeresen m�dos�tva.\nHib�s rang!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin', 'Parancsok: help | list | add | remove');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel/info', 'Bekapcsolva: {0}\nKikapcsolva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update', 'Sikeresen friss�tve {0} csatorn�n a funkci�k.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update/all', 'Sikeresen friss�tve minden csatorn�n a funkci�k.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/info', 'Bekapcsolva: {0}\nKikapcsolva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel', 'Parancsok: add | remove | info | update | language');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/add', 'A n�v m�r szerepel a csatorna list�n!\nCsatorna hozz�adva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/remove', 'A mester csatorna nem t�r�lhet�!\nIlyen csatorna nem l�tezik!\nCsatorna elt�vol�tva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/update', 'A csatorna inform�ci�k friss�t�sre ker�ltek.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/info', 'Akt�v: {0}\nAkt�v: Nincs inform�ci�.\nInakt�v: {0}\nInakt�v: Nincs inform�ci�.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/language', 'Csatorna nyelvezete sikeresen meg lett v�ltoztatva erre: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Nick megv�ltoztat�sa erre: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'join', 'Kapcsol�d�s ehhez a csatorn�hoz: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'left', 'Lel�p�s err�l a csatorn�r�l: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reload', '{0} �jra lett ind�tva.\nA programban nincs ilyen r�sz!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'quit', 'Viszl�t :(\nConsole: Program le�ll�t�sa.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'help', 'Ha a parancs m�g� �rod a megadott parancs nev�t vagy a nevet �s alparancs�t inform�ci�t ad a haszn�lat�r�l.\nParancsok: {0}');

-- enUS
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'consolelog', 'Console logging on.\nConsole logging off.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'sys', 'Version: {0}\nPlatform: {0}\nOSVersion: {0}\nProgramming language: c#\nMemory allocation: {0} MB\nThread count: {0}\nUptime: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'csatorna', 'The new channel to write to now: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/info', 'You are half operator now.\nYou are operator now.\nYou are administrator now.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/list', '2Admins: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/add', 'The name is already in the admin list!\nAdmin added to the list: {0}\nPassword:');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/remove', 'No such nick!\nAdmin was deleted: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/rank', 'Successfully changed the rank!\nRank error!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin', 'Commands: help | list | add | remove');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/channel/info', 'On: {0}\nOff: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/channel', '{0}: On\n{0}: Off');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/update', 'Successfully updated the channel functions in: {0}.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/update/all', 'Successfully updated the information of all channels.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/info', 'On: {0}\nOff: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function', '{0}: are on.\n{0}: are off.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel', 'Commands: add | remove | info | update | language');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/add', 'The name is already exists in the channel list.\nAdded channel: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/remove', 'The master channel cannot delete!\nNo such channel!\nDeleted channel: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/update', 'The channel informations are updated.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/info', 'Active: {0}\nActive: Nothing information.\nInactive: {0}\nInactive: Nothing information.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/language', 'Successfully changed the channel language to: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'nick', 'Nick changes to: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'join', 'Join to this channel: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'left', 'Part of this channel: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'reload', '{0} reloaded.\nThe program does not contains that part!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'quit', 'Bye :(\nConsole: Program shut down.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'help', 'If you wrote behind the command the command or the name or co-command then gets information about usage.\nCommands: {0}');

-- ----------------------------
-- Table structure for localized_console_command_help
-- ----------------------------
DROP TABLE IF EXISTS `localized_console_command_help`;
CREATE TABLE `localized_console_command_help` (
  `Id` int(8) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- huHU
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin', 'Ki�rja az oper�torok vagy adminisztr�torok �ltal haszn�lhat� parancsokat.\nAdmin parancsai: info | list | add | remove | rank | access | newpassword');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/add', '�j admin hozz�ad�sa.\nHaszn�lata: {0}admin add <admin neve>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/remove', 'Admin elt�vol�t�sa.\nHaszn�lata: {0}admin remove <admin neve>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/rank', 'Admin rangj�nak megv�ltoztat�sa.\nHaszn�lata: {0}admin rank <admin neve> <�j rang pl halfoperator: 0, operator: 1, administrator: 2>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/info', 'Kiirja �ppen milyen rangod van.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/list', 'Kiirja az �sszes admin nev�t aki az adatb�zisban szerepel.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function', 'Funkci�k vez�rl�s�re szolg�l� parancs.\nFunkci� parancsai: channel | update | info\nHaszn�lata glob�lisan:\nGlobalis funkci� kezel�se: {0}function <on vagy off> <funkci� n�v>\nGlob�lis funkci�k kezel�se: {0}function <on vagy off> <funkci� n�v1> <funkci� n�v2> ... stb');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel', 'Megadott channelen �ll�that�k ezzel a parancsal a funkci�k.\nFunkci� channel parancsai: info\nHaszn�lata:\nChannel funkci� kezel�se: {0}function channel <on vagy off> <funkci� n�v>\nChannel funkci�k kezel�se: {0}function channel <on vagy off> <funkci� n�v1> <funkci� n�v2> ... stb');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel/info', 'Ki�rja a funkci�k �llapot�t.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update', 'Friss�ti a funkci�kat vagy alap�rtelmez�sre �ll�tja.\nFunkci� update parancsai: all\nHaszn�lata:\nM�s channel: {0}function update <channel neve>\nAhol tartozkodsz channel: {0}function update');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update/all', 'Friss�ti az �sszes funkci�t vagy alap�rtelmez�sre �ll�tja.\Haszn�lata: {0}function update all');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/info', 'Ki�rja a funkci�k �llapot�t.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel', 'Channel parancsai: add | remove | info | update | language');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/add', '�j channel hozz�ad�sa.\nHaszn�lata: {0}channel add <channel> <ha van jelsz� akkor az>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/remove', 'Nem haszn�latos channel elt�vol�t�sa.\nHaszn�lata: {0}channel remove <channel>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/info', '�sszes channel kiir�sa ami az adatb�zisban van �s a hozz�juk tartoz� inform�ciok.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/update', 'Channelekhez tartoz� �sszes inform�ci� friss�t�se, alap�rtelmez�sre �ll�t�sa.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/language', 'Friss�ti a csatorna nyelvezet�t.\nHaszn�lata: {0}channel language <csatorna> <nyelvezet>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Bot nick nev�nek cser�je.\nHaszn�lata: {0}nick <n�v>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'join', 'Kapcsol�d�s megadot csatorn�ra.\nHaszn�lata:\nJelsz� n�lk�li csatorna: {0}join <csatorna>\nJelsz�val ell�tott csatorna: {0}join <csatorna> <jelsz�>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'left', 'Lel�p�s megadot csaton�r�l.\nHaszn�lata: {0}left <csatona>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'quit', 'Bot le�ll�t�s�ra haszn�lhat� parancs.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reload', '�jraind�tja a megadott programr�szt.\nHaszn�lata: {0}reload <n�v>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'consolelog', 'Az irc adatok konzolra �r�s�t enged�lyezi vagy tiltja. Alap�rtelmez�sben ki van kapcsolva.\nHaszn�lata: consolelog <on vagy off>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'sys', 'Ki�rja a botr�l a rendszer inform�ci�kat.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'csatorna', 'A bot csatorn�ra �r�s�t �ll�thatjuk vele.\nHaszn�lata: csatorna <csatorna neve>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'connect', 'Kapcsolod�s az irc szerverhez.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'disconnect', 'Kapcsolat bont�sa.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reconnect', '�jrakapcsolod�s az irc szerverhez.');

-- ----------------------------
-- Table structure for localized_console_warning
-- ----------------------------
DROP TABLE IF EXISTS `localized_console_warning`;
CREATE TABLE `localized_console_warning` (
  `Id` int(8) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- huHU
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoName', 'A n�v nincs megadva!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoValue', 'Nincs param�ter!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'No1Value', 'Nincs megadva egy param�ter!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoChannelName', 'Nincs megadva a csatorna neve!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'FaultyQuery', 'Hib�s lek�rdez�s!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoRank', 'Nincs megadva a rang!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoFunctionName', 'Nincs megadva a funkci� neve!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoChannelLanguage', 'Nincs megadva a csatorna nyelvezete!');

-- enUS
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoName', 'The name is not specified!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoValue', 'The parameters are not specified!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'No1Value', 'A parameter was not specified!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoChannelName', 'The channel name is not specified!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'FaultyQuery', 'Syntax error!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoRank', 'The rank is not specified!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoFunctionName', 'The function name is not specified!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoChannelLanguage', 'The channel language is not specified!');

-- ----------------------------
-- Table structure for localized_command
-- ----------------------------
DROP TABLE IF EXISTS `localized_command`;
CREATE TABLE `localized_command` (
  `Id` int(8) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- huHU
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/sys', '3Verzi�: 10{0}\n3Platform: {0}\n3OSVerzi�: {0}\n3Programnyelv: c#\n3Mem�ria haszn�lat:5 {0} MB\n3Mem�ria haszn�lat:8 {0} MB\n3Mem�ria haszn�lat:3 {0} MB\n3M�k�d�si id�: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/help', '3Parancsok: nick | sys\n3Parancsok: ghost | nick | sys\n3Parancsok: ghost | nick | sys | clean\n3Parancsok: sys');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/ghost', 'Ghost paranccsal els�dleges nick visszaszerz�se.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/nick', 'N�v megv�ltoztat�sa erre: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/nick/identify', 'Azonos�t� jelsz� k�ld�se a kiszolg�l�nak.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/clean', 'Lefoglalt mem�ria felszabad�t�sra ker�l.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'help', 'Ha a parancs m�g� �rod a megadott parancs nev�t vagy a nevet �s alparancs�t inform�ci�t ad a haszn�lat�r�l.\nF� parancsom: {0}xbot');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'xbot', '3Verzi�: 10{0}\n3Parancsok: {0}\nProgrammed by: 3Csaba');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'info', 'Programoz�m: Csaba, Jackneill.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'time', 'Helyi id�: {0}:0{1}\nHelyi id�: {0}:{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'date', 'Ma {0}. 0{1}. 0{2}. {3} napja van.\nMa {0}. 0{1}. {2}. {3} napja van.\nMa {0}. {1}. 0{2}. {3} napja van.\nMa {0}. {1}. {2}. {3} napja van.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'roll', 'Sz�zal�kos ar�nya {0}%');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'whois', 'Jelenleg itt van fent: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'warning', 'Keresnek t�ged itt: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'google', '2Title: Nincs Title.\n2Link: Nincs Link.\n2Title: {0}\n2Link: 3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'translate', 'Nincs f�rd�tott sz�veg.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'prime', 'Nem csak sz�mot tartalmaz!\n{0} nem pr�msz�m.\n{0} primsz�m.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/access', 'Hozz�f�r�s enged�lyezve.\nHozz�f�r�s megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/password', 'Jelsz� sikeresen meg lett v�ltoztatva erre: {0}\nA mostani jelsz� nem egyezik, mod�sit�s megtagadva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/info', 'Jelenleg F�l Oper�tor vagy.\nJelenleg Oper�tor vagy.\nJelenleg Adminisztr�tor vagy.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/list', '2Adminok: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/add', 'A n�v m�r szerepel az admin list�n!\nAdmin hozz�adva: {0}\nMostant�l Schumix adminja vagy. A mostani jelszavad: {0}\nHa megszeretn�d v�ltoztatni haszn�ld az {0}admin newpassword parancsot. Haszn�lata: {0}admin newpassword <r�gi> <�j>\nAdmin nick �les�t�se: {0}admin access <jelsz�>');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/remove', 'Ilyen n�v nem l�tezik!\nAdmin t�r�lve: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/rank', 'Rang sikeresen m�dos�tva.\nHib�s rang!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin', '3F�l Oper�tor parancsok!\n3Parancsok: {0}\n3Oper�tor parancsok!\n3Parancsok: {0}\n3Adminisztr�tor parancsok!\n3Parancsok: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'colors', '1teszt1 2teszt2 3teszt3 4teszt4 5teszt5 6teszt6 7teszt7 8teszt8 9teszt9 10teszt10 11teszt11 12teszt12 13teszt13 14teszt14 15teszt15');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Nick megv�ltoztat�sa erre: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'join', 'Kapcsol�d�s ehhez a csatorn�hoz: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'left', 'Lel�p�s err�l a csatorn�r�l: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/all/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/all', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update', 'Sikeresen friss�tve {0} csatorn�n a funkci�k.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update/all', 'Sikeresen friss�tve minden csatorn�n a funkci�k.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel', '3Parancsok: add | remove | info | update | language');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/add', 'A n�v m�r szerepel a csatorna list�n!\nCsatorna hozz�adva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/remove', 'A mester csatorna nem t�r�lhet�!\nIlyen csatorna nem l�tezik!\nCsatorna elt�vol�tva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/update', 'A csatorna inform�ci�k friss�t�sre ker�ltek.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/info', '3Akt�v: {0}\n3Akt�v: Nincs inform�ci�.\n3Inakt�v: {0}\n3Inakt�v: Nincs inform�ci�.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/language', 'Csatorna nyelvezete sikeresen meg lett v�ltoztatva erre: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'plugin/load', '2[Bet�lt�s]: �sszes plugin bet�lt�se 3sikeres.\n2[Bet�lt�s]: �sszes plugin bet�lt�se 5sikertelen.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'plugin/unload', '2[Lev�laszt�s]: �sszes plugin lev�laszt�sa 3sikeres.\n2[Lev�laszt�s]: �sszes plugin lev�laszt�sa 5sikertelen.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'plugin', '{0}: 3loaded.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'quit', 'Viszl�t :(\n{0} le�ll�tott paranccsal.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/channel/add', 'Csatorna sikeresen hozz�adva.\nNem l�tezik ilyen n�v!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/channel/remove', 'Csatorna sikeresen t�r�lve.\nNem l�tezik ilyen n�v!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/channel/add', 'Csatorna sikeresen hozz�adva.\nNem l�tezik ilyen n�v!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/channel/remove', 'Csatorna sikeresen t�r�lve.\nNem l�tezik ilyen n�v!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/info', '3{0} 7{1} Channel: 2{2}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/channel/add', 'Csatorna sikeresen hozz�adva.\nNem l�tezik ilyen n�v!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/channel/remove', 'Csatorna sikeresen t�r�lve.\nNem l�tezik ilyen n�v!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/memory', 'Jelenleg t�l sok mem�ri�t fogyaszt a bot ez�rt ezen funkci� nem el�rhet�!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/warning', 'A k�dban olyan r�szek vannak melyek vesz�lyeztetik a programot. Ez�rt le�llt a ford�t�s!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler', 'Nincs megadva a f� fv! (Schumix)\nNincs megadva a f� class!\nA kimeneti sz�veg t�l hossz� ez�rt nem ker�lt kiir�sra!\nA k�d sikeresen lefordult csak nincs kimen� �zenet!\nH�tramaradt m�g {0} kiir�s!');
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
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/info', 'Kick list�n l�v�k: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/add', 'A n�v m�r szerepel a kick list�n!\nKick list�hoz a n�v hozz�adva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/remove', 'Ilyen n�v nem l�tezik!\nKick list�b�l a n�v elt�v�l�t�sra ker�lt: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/info', 'Kick list�n l�v�k: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/add', 'A n�v m�r szerepel a mode list�n!\nMode list�hoz a n�v hozz�adva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/remove', 'Ilyen n�v nem l�tezik!\nMode list�b�l a n�v elt�v�l�t�sra ker�lt: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/info', 'Mode list�n l�v�k: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/add', 'A n�v m�r szerepel a mode list�n!\nMode list�hoz a n�v hozz�adva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/remove', 'Ilyen n�v nem l�tezik!\nMode list�b�l a n�v elt�v�l�t�sra ker�lt: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/info', 'Mode list�n l�v�k: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'message/channel', 'Az �zenet sikeresen feljegyz�sre ker�lt.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'message', 'Az �zenet sikeresen feljegyz�sre ker�lt.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/info', 'Jegyzetek k�djai: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/access', 'Hozz�f�r�s enged�lyezve.\nHozz�f�r�s megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/newpassword', 'Jelsz� sikeresen meg lett v�ltoztatva erre: {0}\nA mostani jelsz� nem egyezik, m�dos�t�s megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/register', 'M�r szerepelsz a felhaszn�l�i list�n!\nSikeresen hozz� vagy adva a felhaszn�l�i list�hoz.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/remove', 'Nincs megadva a jelsz� a t�rl�s meger�s�t�s�hez!\nNem szerepelsz a felhaszn�l�i list�n!\nA jelsz� nem egyezik meg az adatb�zisban t�roltal!\nT�rl�s meg lett szak�tva!\nSikeresen t�r�lve lett a felhaszn�l�d.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/code/remove', 'Ilyen k�d nem szerepel a list�n!\nA jegyzet sikeresen t�rl�sre ker�lt.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/code', 'Jegyzet: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes', 'Nincs megadva jegyzetnek semmi se!\nA jegyzet k�dneve m�r szerepel az adatb�zisban!\nJegyzet k�dja: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/warning', 'Jelenleg nem szerepelsz a jegyzetek felhaszn�l�i list�j�n!\nAhoz hogy hozz�add magad nem kell m�st tenned mint az al�bbi parancsot v�grehajtani. (Lehet�leg priv�t �zenetk�nt nehogy m�s megtudja.)\n{0}jegyzet user register <jelsz�>\nFelhaszn�l�i adatok friss�t�se (ha nem fogadn� el adataidat) pedig: {0}jegyzet user hozzaferes <jelsz�>');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'message2', '�zenetet hagyta neked: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reload', '{0} �jra lett ind�tva.\nA programban nincs ilyen r�sz!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'weather', '12Id�j�r�s otthon: {0}, minimum H�m�rs�klet: {1}�, maximum H�m�rs�klet: {2}�, Sz�lsebess�g: {3}\n5{0} 12id�j�r�sa: {1}, minimum H�m�rs�klet: {2}�, maximum H�m�rs�klet: {3}�, Sz�lsebess�g: {4}\nNem szerepel ilyen v�ros a list�n!');

-- enUS
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/sys', '3Version: 10{0}\n3Platform: {0}\n3OSVersion: {0}\n3Programming language: c#\n3Memory allocation:5 {0} MB\n3Memory allocation:8 {0} MB\n3Memory allocation:3 {0} MB\n3Uptime: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/help', '3Commands: nick | sys\n3Commands: ghost | nick | sys\n3Commands: ghost | nick | sys | clean\n3Commands: sys');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/ghost', 'Return to the primary nick.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/nick', 'Change nick to: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/nick/identify', 'Send identifcation password to the server.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/clean', 'Allocated memory will be freed.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'help', 'If you wrote behind the command the command or the name or co-command then gets information about usage.\nMain command: {0}xbot');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'xbot', '3Version: 10{0}\n3Commands: {0}\nProgrammed by: 3Csaba');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'info', 'My programmer(s): Csaba, Jackneill.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'time', 'Local time: {0}:0{1}\nLocal time: {0}:{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'date', 'Today is {0}. 0{1}. 0{2}. and {3}\'s day.\nToday is {0}. 0{1}. {2}. and {3}\'s day.\nToday is {0}. {1}. 0{2}. and {3}\'s day.\nToday is {0}. {1}. {2}. and {3}\'s day.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'roll', 'Pencentage rate: {0}%');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'whois', 'Now online here: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'warning', 'Looking for you here: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'google', '2Title: Nothing Title.\n2Link: Nothing Link.\n2Title: {0}\n2Link: 3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'translate', 'Nothing translated text.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'prime', 'This is not a numeric text!\n{0} is not a prime number.\n{0} is a prime number.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/access', 'Access granted.\nAccess denied.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/password', 'Successfully changed to password to: {0}\nThe current password does not match, modification denied.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/info', 'You are half operator now.\nYou are operator now.\nYou are administrator now.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/list', '2Admins: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/add', 'The name is already in the admin list!\nAdmin added to the list: {0}\nYou are schumix\'s admin now. Your current password is: {0}\nIf you want to change it, use this command: {0}admin newpassword. Usage: {0}admin newpassword <Old> <New>\nAdmin nick confirmation: {0}admin access <Password>');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/remove', 'No such nick!\nAdmin was deleted: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/rank', 'Successfully changed the rank!\nRank error!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin', '3half operator commands!\n3Commands: {0}\n3Operator commands!\n3Commands: {0}\n3Administrator commands!\n3Commands: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'colors', '1test1 2test2 3test3 4test4 5test5 6test6 7test7 8test8 9test9 10test10 11test11 12test12 13test13 14test14 15test15');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'nick', 'Nick changes to: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'join', 'Join to this channel: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'left', 'Part of this channel: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/info', '2On: {0}\n2Off: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/all/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/all', '{0}: are on.\n{0}: are off.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/channel/info', '2On: {0}\n2Off: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/channel', '{0}: On\n{0}: Off');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/update', 'Successfully updated the channel functions in: {0}.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/update/all', 'Successfully updated the information of all channels.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function', '{0}: are on.\n{0}: are off.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel', '3Commands: add | remove | info | update | language');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/add', 'The name is already exists in the channel list.\nAdded channel: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/remove', 'The master channel cannot delete!\nNo such channel!\nDeleted channel: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/update', 'The channel informations are updated.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/info', '3Active: {0}\n3Active: Nothing information.\n3Inactive: {0}\n3Inactive: Nothing information.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/language', 'Successfully changed the channel language to: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'plugin/load', '2[Load]: All plugins 3done.\n2[Load]: All plugins 5failed.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'plugin/unload', '2[Unload]: All plugins 3done.\n2[Unload]: All plugins 5failed.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'plugin', '{0}: 3loaded.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'quit', 'Bye :(\n{0} shutted down me with command.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/list', '2List:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/channel/add', 'Successfully added channel.\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/channel/remove', 'Successfully deleted channel!\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/list', '2List:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/channel/add', 'Sccessfully added channel!\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/channel/remove', 'Successfully deleted channel!.\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/info', '3{0} 7{1} Channel: 2{2}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/list', '2List:3{0}');
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
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/info', 'On the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/add', 'The name is already on the kick list!A n�v m�r szerepel a kick list�n!\nKick list�hoz a n�v hozz�adva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/remove', 'No such name!\nThe name has been deleted from the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/info', 'On the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/add', 'The name is already on the mode list!\nThe name added to the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/remove', 'No such name!\The name has been deleted from the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/info', 'On the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/add', 'The name is already on the mode list!\nThe name added to the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/remove', 'No such name!\nThe name has been deleted from the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/info', 'On the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'message/channel', 'Successfully recorded the message.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'message', 'Successfully recorded the message.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/info', 'Recorded message (quotes) codes: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/user/access', 'Access granted.\nAccess denied!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/user/newpassword', 'Successfully changed the password to: {0}\nThe password does not match! Modification is denied!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/user/register', 'You are already on the user list!\nSuccessfully added you to the user list!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/user/remove', 'The password for confirmation is not specified!The password for delete confirmation is not specified!\nYou are not in user list!\nYour password does not match which is stored in database!\nThe password does not match which is stored in database!\nThe deleting is aborted!\nSuccessfully deleted your account.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/code/remove', 'This code is not in the list!\nSuccessfully deleted the note.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/code', 'Note: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes', 'No text found for note!\nNote\'s codename is alreadyin the database!\nNote\'s code: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/warning', 'You are not in the note\'s user list!\nIf you want to add yourself, you have to do the following command. (Must be a private message, do not gather info someone else.)\n{0}jegyzet user register <password>\nUpdating user data (If do not accept your datas) Do: {0}jegyzet user hozzaferes <password>');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'message2', 'Left the note for you: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'reload', '{0} reloaded.\nThe program does not contains that part!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'weather', '12Local weather: {0}, minimum temperature: {1}�, maximum temperature {2}�, wind speed: {3}\n5{0} 12weather: {1}, minimum temperature {2}�, maximum temperature: {3}�, wind speed. {4}\nNo such city in the list!');

-- ----------------------------
-- Table structure for localized_command_help
-- ----------------------------
DROP TABLE IF EXISTS `localized_command_help`;
CREATE TABLE `localized_command_help` (
  `Id` int(8) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Rank` int(1) NOT NULL default '0',
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- huHU
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'xbot', '9', 'Felhaszn�l�k sz�m�ra haszn�lhat� parancslista.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'info', '9', 'Kis le�r�s a botr�l.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'whois', '9', 'A parancs seg�ts�g�vel megtudhatjuk hogy egy nick milyen channelon van fent.\nHaszn�lata: {0}whois <nick>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'roll', '9', 'Cs�pp szorakoz�s a wowb�l, m�r ha valaki felismeri :P');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'date', '9', 'Az aktu�lis d�tumot �rja ki �s a hozz� tartoz� n�vnapot.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'time', '9', 'Az aktu�lis id�t �rja ki.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'google', '9', 'Ha sz�ks�ged lenne valamire a google-b�l nem kell hozz� weboldal csak ez a parancs.\nHaszn�lata: {0}google <ide j�n a keresett sz�veg>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'translate', '9', 'Ha r�gt�n k�ne ford�tani m�sik nyelvre vagy -r�l valamit, akkor megteheted ezzel a parancsal.\nHaszn�lata: {0}translate <kiindul�si nyelv|c�l nyelv> <sz�veg>\nP�ld�ul: {0}translate hu|en Sz�p sz�veg.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'irc', '9', 'N�h�ny parancs haszn�lata az IRC-n.\nHaszn�lata: {0}irc <parancs neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'calc', '9', 'T�bb funkci�s sz�mol�g�p.\nHaszn�lata: {0}calc <sz�m>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'warning', '9', 'Figyelmeztet� �zenet k�ld�se, hogy keresik ezen a csatorn�n vagy egy tetsz�leges �zenet k�ld�se.\nHaszn�lata: {0}warning <ide j�n a szem�ly> <ha nem felh�v�t k�lden�l hanem saj�t �zenetet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'sha1', '9', 'Sha1 k�dol�ss� �talak�t� parancs.\nHaszn�lata: {0}sha1 <�talak�tand� sz�veg>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'md5', '9', 'Md5 k�dol�ss� �talak�t� parancs.\nHaszn�lata: {0}md5 <�talak�tand� sz�veg>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'prime', '9', 'Meg�lap�tja hogy a sz�m pr�msz�m-e. Csak eg�sz sz�mmal tud sz�molni!\nHaszn�lata: {0}prime <sz�m>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin', '0', 'Ki�rja az oper�torok vagy adminisztr�torok �ltal haszn�lhat� parancsokat.\nAdmin parancsai: info | list | add | remove | rank | access | newpassword');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/add', '0', '�j admin hozz�ad�sa.\nHaszn�lata: {0}admin add <admin neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/remove', '0', 'Admin elt�vol�t�sa.\nHaszn�lata: {0}admin remove <admin neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/rank', '0', 'Admin rangj�nak megv�ltoztat�sa.\nHaszn�lata: {0}admin rank <admin neve> <�j rang pl halfoperator: 0, operator: 1, administrator: 2>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/info', '0', 'Kiirja �ppen milyen rangod van.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/list', '0', 'Kiirja az �sszes admin nev�t aki az adatb�zisban szerepel.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/access', '0', 'Az admin parancsok haszn�lat�hoz sz�ks�ges jelsz� ellen�rz� �s vhost aktiv�l�.\nHaszn�lata: {0}admin access <jelsz�>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/newpassword', '0', 'Az admin jelszav�nak cser�je ha �j k�ne a r�gi helyett.\nHaszn�lata: {0}admin newpassword <r�gi jelsz�> <�j jelsz�>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'colors', '0', 'Adott sk�l�j� sz�nek ki�r�sa amit lehet haszn�lni IRC-n.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'nick', '0', 'Bot nick nev�nek cser�je.\nHaszn�lata: {0}nick <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'join', '0', 'Kapcsol�d�s megadot csatorn�ra.\nHaszn�lata:\nJelsz� n�lk�li csatorna: {0}join <csatorna>\nJelsz�val ell�tott csatorna: {0}join <csatorna> <jelsz�>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'left', '0', 'Lel�p�s megadot csaton�r�l.\nHaszn�lata: {0}left <csatona>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel', '1', 'Channel parancsai: add | remove | info | update | language');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/add', '1', '�j channel hozz�ad�sa.\nHaszn�lata: {0}channel add <channel> <ha van jelsz� akkor az>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/remove', '1', 'Nem haszn�latos channel elt�vol�t�sa.\nHaszn�lata: {0}channel remove <channel>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/info', '1', '�sszes channel ki�r�sa ami az adatb�zisban van �s a hozz�juk tartoz� inform�ciok.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/update', '1', 'Channelekhez tartoz� �sszes inform�ci� friss�t�se, alap�rtelmez�sre �ll�t�sa.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/language', '1', 'Friss�ti a csatorna nyelvezet�t.\nHaszn�lata: {0}channel language <csatorna> <nyelvezet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function', '1', 'Funkci�k vez�rl�s�re szolg�l� parancs.\nFunkci� parancsai: channel | all | update | info\nHaszn�lata ahol tart�zkodsz:\nChannel funkci� kezel�se: {0}function <on vagy off> <funkci� n�v>\nChannel funkci�k kezel�se: {0}function <on vagy off> <funkci� n�v1> <funkci� n�v2> ... stb');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/channel', '1', 'Megadott channelen �llithat�k ezzel a parancsal a funkci�k.\nFunkci� channel parancsai: info\nHaszn�lata:\nChannel funkci� kezel�se: {0}function channel <on vagy off> <funkci� n�v>\nChannel funkci�k kezel�se: {0}function channel <on vagy off> <funkci� n�v1> <funkci� n�v2> ... stb');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/channel/info', '1', 'Ki�rja a funkci�k �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/all', '1', 'Glob�lis funkci�k kezel�se.\nFunkci� all parancsai: info\nEgy�ttes kezel�s: {0}function all <on vagy off> <funkci� n�v>\nEgy�ttes funkci�k kezel�se: {0}function all <on vagy off> <funkci� n�v1> <funkci� n�v2> ... stb');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/all/info', '1', 'Ki�rja a funkci�k �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/update', '1', 'Friss�ti a funkci�kat vagy alap�rtelmez�sre �ll�tja.\nFunkci� update parancsai: all\nHaszn�lata:\nM�s channel: {0}function update <channel neve>\nAhol tart�zkodsz channel: {0}function update');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/update/all', '1', 'Friss�ti az �sszes funkci�t vagy alap�rtelmez�sre �ll�tja.\Haszn�lata: {0}function update all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/info', '1', 'Ki�rja a funkci�k �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'kick', '1', 'Kir�gja a nick-et a megadott channelr�l.\nHaszn�lata:\nCsak kir�g�s: {0}kick <channel> <n�v>\nKir�g�s okkal: {0}kick <channel> <n�v> <oka>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mode', '1', 'Megv�ltoztatja a nick rangj�t megadott channelen.\nHaszn�lata: {0}mode <rang> <n�v vagy nevek>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'plugin', '2', 'Ki�rja milyen pluginok vannak bet�ltve.\nPlugin parancsok: load | unload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'plugin/load', '2', 'Bet�lt minden plugint.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'plugin/unload', '2', 'Elt�vol�t minden plugint.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'quit', '2', 'Bot le�ll�t�s�ra haszn�lhat� parancs.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2', '9', 'Parancsok: nick | sys\nParancsok: ghost | nick | sys\nParancsok: ghost | nick | sys | clean\nParancsok: sys');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/sys', '9', 'Ki�rja a program inform�ci�it.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/ghost', '1', 'Kil�pteti a f� nick-et ha regisztr�lva van.\nHaszn�lata: {0} ghost');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/nick', '0', 'Bot nick nev�nek cser�je.\n"Haszn�lata: {0} nick <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/nick/identify', '0', 'Aktiv�lja a f� nick jelszav�t.\nHaszn�lata: {0} nick identify');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/clean', '2', 'Felszabad�tja a lefoglalt mem�ri�t.\nHaszn�lata: {0} clean');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn', '1', 'Svn rss-ek kezel�se.\nSvn parancsai: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/channel', '1', 'Rss csatorn�kra val� kiir�s�nak kezel�se.\nChannel parancsai: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/channel/add', '1', '�j csatorna hozz�ad�sa az rss-hez.\nHaszn�lata: {0}svn channel add <rss neve> <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/channel/remove', '1', 'Nem haszn�latos csatorna elt�vol�t�sa az rss-b�l.\nHaszn�lata: {0}svn channel remove <rss neve> <csatorna>');
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
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/info', '1', 'Kiirja az rss-ek �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/list', '1', 'V�laszthat� rss-ek list�ja.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/start', '1', '�j rss bet�lt�se.\nHaszn�lata: {0}hg start <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/stop', '1', 'Rss le�ll�t�sa.\nHaszn�lata: {0}hg stop <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/reload', '1', 'Megadott rss �jrat�lt�se.\nHg reload parancsai: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/reload/all', '1', 'Minden rss �jrat�lt�se.\nHaszn�lata: {0}hg reload <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git', '1', 'Git rss-ek kezel�se.\nGit parancsai: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/channel', '1', 'Rss csatorn�kra val� kiir�s�nak kezel�se.\nChannel parancsai: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/channel/add', '1', '�j csatorna hozz�ad�sa az rss-hez.\nHaszn�lata: {0}git channel add <rss neve> <tipus> <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/channel/remove', '1', 'Nem haszn�latos csatorna elt�vol�t�sa az rss-b�l.\nHaszn�lata: {0}git channel remove <rss neve> <tipus> <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/info', '1', 'Kiirja az rss-ek �llapot�t.');
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
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/user/newpassword', '9', 'Felhaszn�l� jelszav�nak cser�je ha �j k�ne a r�gi helyet.\nHaszn�lata: {0}notes user newpassword <r�gi jelsz�> <�j jelsz�>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/code', '9', 'Jegyzet kiolvas�s�hoz sz�ks�ges k�d.\nHaszn�lata: {0}notes code <jegyzet k�dja>\nK�d parancsai: remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/code/remove', '9', 'T�rli a jegyzetet k�d alapj�n.\nHaszn�lata: {0}notes code remove <jegyzet k�dja>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'message', '9', 'Ezzel a paranccsal �zenetet lehet hagyni b�rkinek a megadott csatorn�n.\nHaszn�lata: {0}message <n�v> <�zenet>\n�zenet parancsai: channel');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'message/channel', '9', 'Ezzel a paranccsal �zenetet lehet hagyni b�rkinek a kiv�lasztott csatorn�n.\nHaszn�lata: {0}message channel <csatorna> <n�v> <�zenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction', '0', 'Aut�matikusan m�k�d� k�dr�szek kezel�se.\nAutofunkcio parancsai: hlmessage\nAut�matikusan m�k�d� k�dr�szek kezel�se.\nAutofunkcio parancsai: kick | mode | hlmessage');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/hlmessage', '0', 'Aut�matikusan hl-t kap� nick-ek kezel�se.\nHl �zenet parancsai: function | update | info\nHaszn�lata: {0}autofunction hlmessage <�zenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/function', '0', 'Ezzel a parancsal �ll�that� a hl �llapota.\nHaszn�lata: {0}autofunction hlmessage function <�llapot>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/update', '0', 'Friss�ti az adatb�zisban szerepl� hl list�t!');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/info', '0', 'Ki�rja a hl-ek �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick', '1', 'Automatikusan kirug�sra ker�l� nick-ek kezel�se.\nKick parancsai: add | remove | info | channel');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/add', '1', 'Kirugand� nev�nek hozz�ad�sa ahol tart�zkodsz.\nHaszn�lata: {0}autofunction kick add <n�v> <oka>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/remove', '1', 'Kirugand� nev�nek elt�vol�t�sa ahol tart�zkodsz.\nHaszn�lata: {0}autofunction kick remove <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/info', '1', 'Ki�rja a kirugandok �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/channel', '1', 'Automatikusan kirug�sra ker�l� nick-ek kezel�se megadot channelen.\nKick channel parancsai: add | remove | info');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/add', '1', 'Kirugand� nev�nek hozz�ad�sa megadott channelen.\nHaszn�lata: {0}autofunction kick channel add <n�v> <csatorna> <oka>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/remove', '1', 'Kirugand� nev�nek elt�vol�t�sa megadott channelen.\nHaszn�lata: {0}autofunction kick channel remove <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/info', '1', 'Ki�rja a kirugandok �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode', '1', 'Automatikusan rangot kap� nick-ek kezel�se.\nMode parancsai: add | remove | info | channel');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/add', '1', 'Rangot kap� nev�nek hozz�ad�sa ahol tart�zkodsz.\nHaszn�lata: {0}autofunction mode add <n�v> <rang>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/remove', '1', 'Rangot kap� nev�nek elt�vol�t�sa ahol tart�zkodsz.\nHaszn�lata: {0}autofunction mode remove <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/info', '1', 'Ki�rja a rangot kap�k �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel', '1', 'Automatikusan rangot kap� nick-ek kezel�se megadot channelen.\nMode channel parancsai: add | remove | info');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/add', '1', 'Rangot kap� nev�nek hozz�ad�sa megadott channelen.\nsSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Haszn�lata: {0}autofunction mode channel add <n�v> <csatorna> <rang>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/remove', '1', 'Rangot kap� nev�nek elt�vol�t�sa megadott channelen.\nHaszn�lata: {0}autofunction mode channel remove <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/info', '1', 'Ki�rja a rangot kap�k �llapot�t.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'reload', '2', '�jraind�tja a megadott programr�szt.\nHaszn�lata: {0}reload <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'weather', '9', 'Megmondja az id�j�r�st a megadott v�rosban.\nHaszn�lata: {0}weather <v�ros>');

-- ----------------------------
-- Table structure for localized_warning
-- ----------------------------
DROP TABLE IF EXISTS `localized_warning`;
CREATE TABLE `localized_warning` (
  `Id` int(8) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- huHU
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoName', 'A n�v nincs megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoValue', 'Nincs param�ter!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'No1Value', 'Nincs megadva egy param�ter!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'FaultyQuery', 'Hib�s lek�rdez�s!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoIrcCommandName', 'Nincs megadva a parancs neve!');
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
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTypeName', 'Nincs a t�pus neve megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'CapsLockOff', 'Kapcsold ki a caps lock-ot!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTime', 'Nincs megadva az id�!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoBanNameOrVhost', 'Nincs megadva a kitiltand� neve vagy a vhost!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoUnbanNameOrVhost', 'Nincs megadva a kitiltott neve vagy a vhost!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'BanList', 'M�r szerepel a t�lt� list�n!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'BanList1', 'Sikeresen hozz� lett adva a t�lt� list�hoz.');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'UnbanList', 'Nem szerepel a t�lt� list�n!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'UnbanList1', 'Sikeresen t�r�lve lett a t�lt� list�hoz.');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'RecurrentFlooding', 'Ism�tl�d� flooding!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'StopFlooding', '�llj le a floodinggal!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoMessage', '�zenet nincs megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoCode', 'A k�d nincs megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoReason', 'Nincs ok megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoChannelLanguage', 'Nincs megadva a csatorna nyelvezete!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoDataNoCommand', 'Az adataid nem megfelel�ek ez�rt nem folytathat� a parancs!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoCityName', 'Nem adott meg egy v�rosnevet sem!');

-- enUS
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoName', 'The name is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoValue', 'The parameters are not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'No1Value', 'A parameter was not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'FaultyQuery', 'Syntax error!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoIrcCommandName', 'The name of the command is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoWhoisName', 'The searching person\'s name are not specified!');
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
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoBanNameOrVhost', 'The banning person\'s name or his/her vhost is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoUnbanNameOrVhost', 'The unbanning person\'s name or his/her vhost is not specified!');
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

ALTER TABLE uptime CHANGE column `datum` `Date` text NOT NULL;
ALTER TABLE uptime CHANGE column `uptime` `Uptime` text NOT NULL;
ALTER TABLE uptime CHANGE column `memory` `Memory` text NOT NULL;