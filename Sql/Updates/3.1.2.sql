INSERT INTO `localized_command` (Language, Command, Text) VALUES ('huHU', 'reload', '{0} �jra lett ind�tva.\nA programban nincs ilyen r�sz!');
INSERT INTO `localized_command` (Language, Command, Text) VALUES ('enUS', 'reload', '{0} reloaded.\nThe program does not contains that part!');
INSERT INTO `localized_command` (Language, Command, Text) VALUES ('huHU', 'weather', '12Id�j�r�s otthon: {0}, minimum H�m�rs�klet: {1}�, maximum H�m�rs�klet: {2}�, Sz�lsebess�g: {3}\n5{0} 12id�j�r�sa: {1}, minimum H�m�rs�klet: {2}�, maximum H�m�rs�klet: {3}�, Sz�lsebess�g: {4}\nNem szerepel ilyen v�ros a list�n!');
INSERT INTO `localized_command` (Language, Command, Text) VALUES ('enUS', 'weather', '12Local weather: {0}, minimum temperature: {1}�, maximum temperature {2}�, wind speed: {3}\n5{0} 12weather: {1}, minimum temperature {2}�, maximum temperature: {3}�, wind speed. {4}\nNo such city in the list!');
INSERT INTO `localized_warning` (Language, Command, Text) VALUES ('huHU', 'NoCityName', 'Nem adott meg egy v�rosnevet sem!');
INSERT INTO `localized_warning` (Language, Command, Text) VALUES ('enUS', 'NoCityName', 'No such city name!');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'reload', '2', '�jraind�tja a megadott programr�szt.\nHaszn�lata: {0}reload <n�v>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'weather', '9', 'Megmondja az id�j�r�st a megadott v�rosban.\nHaszn�lata: {0}weather <v�ros>');

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
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'sys', 'Verzi�: {0}\nPlatform: {0}\nOSVerzi�: {0}\nProgramnyelv: c#\nMemoria haszn�lat: {0} MB\nFut� sz�lak: {0}\nM�k�d�si id�: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'csatorna', '�j csatorna ahova mostant�l lehet �rni: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/info', 'Jelenleg F�l Oper�tor vagy.\nJelenleg Oper�tor vagy.\nJelenleg Adminisztr�tor vagy.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/list', '2Adminok: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/add', 'A n�v m�r szerepel az admin list�n!\nAdmin hozz�adva: {0}\nJelenlegi jelsz�:');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/remove', 'Ilyen n�v nem l�tezik!\nAdmin t�r�lve: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/rank', 'Rang sikeresen m�dos�tva.\nHib�s rang!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin', 'Parancsok: help | list | add | remove');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel/info', 'Bekapcsolva: {0}\nKikapcsolva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update', 'Sikeresen frissitve {0} csatorn�n a funkci�k.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update/all', '"Sikeresen frissitve minden csatorn�n a funkci�k.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/info', 'Bekapcsolva: {0}\nKikapcsolva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel', 'Parancsok: add | remove | info | update | language');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/add', 'A n�v m�r szerepel a csatorna list�n!\nCsatorna hozz�adva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/remove', 'A mester csatorna nem t�r�lhet�!\nIlyen csatorna nem l�tezik!\nCsatorna elt�vol�tva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/update', 'A csatorna inform�ci�k frissit�sre ker�ltek.');
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
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function', 'Funkci�k vez�rl�s�re szolg�l� parancs.\nFunkci� parancsai: channel | update | info\nHaszn�lata glob�lisan:\nGlobalis funkci� kezel�se: {0}function <on vagy off> <funkci� n�v>\nGlobalis funkci�k kezel�se: {0}function <on vagy off> <funkci� n�v1> <funkci� n�v2> ... stb');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel', 'Megadott channelen �llithat�k ezzel a parancsal a funkci�k.\nFunkci� channel parancsai: info\nHaszn�lata:\nChannel funkci� kezel�se: {0}function channel <on vagy off> <funkci� n�v>\nChannel funkci�k kezel�se: {0}function channel <on vagy off> <funkci� n�v1> <funkci� n�v2> ... stb');
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
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'join', 'Kapcsolod�s megadot csatorn�ra.\nHaszn�lata:\nJelsz� n�lk�li csatorna: {0}join <csatorna>\nJelsz�val ell�tott csatorna: {0}join <csatorna> <jelsz�>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'left', 'Lel�p�s megadot csaton�r�l.\nHaszn�lata: {0}left <csatona>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'quit', 'Bot le�ll�t�s�ra haszn�lhat� parancs.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reload', '�jraind�tja a megadott programr�szt.\nHaszn�lata: {0}reload <n�v>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'consolelog', 'Az irc adatok konzolra �r�s�t enged�lyezi vagy tiltja. Alap�rtelmez�sben ki van kapcsolva.\nHaszn�lata: consolelog <on vagy off>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'sys', 'K�irja a botr�l a rendszer inform�ci�kat.');
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
