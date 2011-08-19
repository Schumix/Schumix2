-- ----------------------------
-- Table structure for irc_commands
-- ----------------------------
DROP TABLE IF EXISTS `irc_commands`;
CREATE TABLE `irc_commands` (
  `Id` int(4) unsigned NOT NULL auto_increment,
  `Command` varchar(30) collate utf8_hungarian_ci NOT NULL default '',
  `Message` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

INSERT INTO `irc_commands` VALUES ('1', 'rang', 'Rang használata: /mode <csatorna> <rang> <név>');
INSERT INTO `irc_commands` VALUES ('2', 'rang1', 'Rang mentése: /chanserv <rang (sop, aop, hop, vop)> <csatorna> ADD <név>');
INSERT INTO `irc_commands` VALUES ('3', 'nick', 'Nick csere használata: /nick <új név>');
INSERT INTO `irc_commands` VALUES ('4', 'kick', 'Kick használata: /kick <csatorna> <név> (<oka> nem feltétlen kell)');
INSERT INTO `irc_commands` VALUES ('5', 'owner', 'Ownermód használata: /msg chanserv SET <csatorna> ownermode on');

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
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'consolelog', 'Console logolás bekapcsolva.\nConsole logolás kikapcsolva.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'sys', 'Verzió: {0}\nPlatform: {0}\nOSVerzió: {0}\nProgramnyelv: c#\nMemória használat: {0} MB\nFutó szálak: {0}\nMűködési idő: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'csatorna', 'Új csatorna ahova mostantól lehet írni: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/info', 'Jelenleg Fél Operátor vagy.\nJelenleg Operátor vagy.\nJelenleg Adminisztrátor vagy.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/list', '2Adminok: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/add', 'A név már szerepel az admin listán!\nAdmin hozzáadva: {0}\nJelenlegi jelszó: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/remove', 'Ilyen név nem létezik!\nAdmin törölve: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/rank', 'Rang sikeresen módosítva.\nHibás rang!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin', 'Parancsok: help | list | add | remove');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel/info', 'Bekapcsolva: {0}\nKikapcsolva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update', 'Sikeresen frissítve {0} csatornán a funkciók.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update/all', 'Sikeresen frissítve minden csatornán a funkciók.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/info', 'Bekapcsolva: {0}\nKikapcsolva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel', 'Parancsok: add | remove | info | update | language');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/add', 'A név már szerepel a csatorna listán!\nCsatorna hozzáadva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/remove', 'A mester csatorna nem törölhető!\nIlyen csatorna nem létezik!\nCsatorna eltávolítva: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/update', 'A csatorna információk frissítésre kerültek.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/info', 'Aktív: {0}\nAktív: Nincs információ.\nInaktív: {0}\nInaktív: Nincs információ.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/language', 'Csatorna nyelvezete sikeresen meg lett változtatva erre: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Nick megváltoztatása erre: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'join', 'Kapcsolódás ehhez a csatornához: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'leave', 'Lelépés erről a csatornáról: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reload', '{0} újra lett indítva.\nA programban nincs ilyen rész!');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'quit', 'Viszlát :(\nConsole: Program leállítása.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'help', 'Ha a parancs mögé írod a megadott parancs nevét vagy a nevet és alparancsát információt ad a használatáról.\nParancsok: {0}');

-- enUS
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'consolelog', 'Console logging on.\nConsole logging off.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'sys', 'Version: {0}\nPlatform: {0}\nOSVersion: {0}\nProgramming language: c#\nMemory allocation: {0} MB\nThread count: {0}\nUptime: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'csatorna', 'The new channel to write to now: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/info', 'You are half operator now.\nYou are operator now.\nYou are administrator now.');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/list', '2Admins: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/add', 'The name is already in the admin list!\nAdmin added to the list: {0}\nPassword: {0}');
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
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin', 'Kiírja az operátorok vagy adminisztrátorok által használható parancsokat.\nAdmin parancsai: info | list | add | remove | rank | access | newpassword');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/add', 'Új admin hozzáadása.\nHasználata: admin add <admin neve>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/remove', 'Admin eltávolítása.\nHasználata: admin remove <admin neve>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/rank', 'Admin rangjának megváltoztatása.\nHasználata: admin rank <admin neve> <új rang pl halfoperator: 0, operator: 1, administrator: 2>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/info', 'Kiírja éppen milyen rangod van.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/list', 'Kiírja az összes admin nevét aki az adatbázisban szerepel.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function', 'Funkciók vezérlésére szolgáló parancs.\nFunkció parancsai: channel | update | info\nHasználata globálisan:\nGlobalis funkció kezelése: function <on vagy off> <funkció név>\nGlobális funkciók kezelése: function <on vagy off> <funkció név1> <funkció név2> ... stb');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel', 'A megadott csatornán ezzel a paranccsal állíthatók a funkciók.\nFunkció channel parancsai: info\nHasználata:\nChannel funkció kezelése: function channel <csatorna> <on vagy off> <funkció név>\nChannel funkciók kezelése: function channel <csatorna> <on vagy off> <funkció név1> <funkció név2> ... stb');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel/info', 'Kiírja a funkciók állapotát.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update', 'Frissíti a funkciókat vagy alapértelmezésre állítja.\nFunkció update parancsai: all\nHasználata:\nMás channel: function update <channel neve>\nAhol tartózkodsz channel: function update');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update/all', 'Frissíti az összes funkciót vagy alapértelmezésre állítja.\Használata: function update all');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/info', 'Kiírja a funkciók állapotát.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel', 'Channel parancsai: add | remove | info | update | language');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/add', 'Új csatorna hozzáadása.\nHasználata: channel add <csatorna> <ha van jelszó akkor az>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/remove', 'Nem használatos csatorna eltávolítása.\nHasználata: channel remove <csatorna>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/info', 'Az összes csatorna kiírása, ami az adatbázisban van és a hozzájuk tartozó információk.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/update', 'A csatornákhoz tartozó összes információ frissítése, alapértelmezésre állítása.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/language', 'Frissíti a csatorna nyelvezetét.\nHasználata: channel language <csatorna> <nyelvezet>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Bot nick nevének cseréje.\nHasználata: nick <név>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'join', 'Kapcsolódás a megadott csatornára.\nHasználata:\nJelszó nélküli csatorna: join <csatorna>\nJelszóval ellátott csatorna: join <csatorna> <jelszó>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'leave', 'Lelépés a megadott csatornáról.\nHasználata: leave <csatona>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'quit', 'Bot leállítására használható parancs.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reload', 'Újraindítja a megadott programrészt.\nHasználata: reload <név>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'consolelog', 'Az irc adatok konzolra írását engedélyezi vagy tiltja. Alapértelmezésben ki van kapcsolva.\nHasználata: consolelog <on vagy off>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'sys', 'Kiírja a botról a rendszer információkat.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'csatorna', 'A bot csatornára írását állíthatjuk vele.\nHasználata: csatorna <csatorna neve>');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'connect', 'Kapcsolodás az irc szerverhez.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'disconnect', 'Kapcsolat bontása.');
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reconnect', 'Újrakapcsolodás az irc szerverhez.');

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
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoName', 'A név nincs megadva!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoValue', 'Nincs paraméter!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'No1Value', 'Nincs megadva egy paraméter!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoChannelName', 'Nincs megadva a csatorna neve!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'FaultyQuery', 'Hibás lekérdezés!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoRank', 'Nincs megadva a rang!');
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoFunctionName', 'Nincs megadva a funkció neve!');
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
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/sys', '3Verzió: 10{0}\n3Platform: {0}\n3OSVerzió: {0}\n3Programnyelv: c#\n3Memória használat:5 {0} MB\n3Memória használat:8 {0} MB\n3Memória használat:3 {0} MB\n3Működési idő: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/help', '3Parancsok: nick | sys\n3Parancsok: ghost | nick | sys\n3Parancsok: ghost | nick | sys | clean\n3Parancsok: sys');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/ghost', 'Ghost paranccsal az elsődleges nick visszaszerzése.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/nick', 'Név megváltoztatása erre: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/nick/identify', 'Azonosító jelszó küldése a kiszolgálónak.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/clean', 'Lefoglalt memória felszabadításra kerül.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'help', 'Ha a parancs mögé írod a megadott parancs nevét vagy a nevet és alparancsát információt ad a használatáról.\nFő parancsom: {0}xbot');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'xbot', '3Verzió: 10{0}\n3Parancsok: {0}\nProgrammed by: 3Csaba');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'info', 'Programozóm: Csaba, Jackneill.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'time', 'Helyi idő: {0}:0{1}\nHelyi idő: {0}:{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'date', 'Ma {0}. 0{1}. 0{2}. {3} napja van.\nMa {0}. 0{1}. {2}. {3} napja van.\nMa {0}. {1}. 0{2}. {3} napja van.\nMa {0}. {1}. {2}. {3} napja van.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'roll', 'Százalékos aránya {0}%');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'whois', 'Jelenleg itt van fent: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'warning', 'Keresnek téged itt: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'google', '2Title: Nincs Title.\n2Link: Nincs Link.\n2Title: {0}\n2Link: 3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'translate', 'Nincs fórdított szöveg.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'prime', 'Nem csak számot tartalmaz!\n{0} nem prímszám.\n{0} primszám.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/access', 'Hozzáférés engedélyezve.\nHozzáférés megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/newpassword', 'Jelszó sikeresen meg lett változtatva erre: {0}\nA mostani jelszó nem egyezik, modósítás megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/info', 'Jelenleg Fél Operátor vagy.\nJelenleg Operátor vagy.\nJelenleg Adminisztrátor vagy.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/list', '2Adminok: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/add', 'A név már szerepel az admin listán!\nAdmin hozzáadva: {0}\nMostantól Schumix adminja vagy. A mostani jelszavad: {0}\nHa megszeretnéd változtatni használd az {0}admin newpassword parancsot. Használata: {0}admin newpassword <régi> <új>\nAdmin nick élesítése: {0}admin access <jelszó>');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/remove', 'Ilyen név nem létezik!\nAdmin törölve: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/rank', 'Rang sikeresen módosítva.\nHibás rang!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin', '3Fél Operátor parancsok!\n3Parancsok: {0}\n3Operátor parancsok!\n3Parancsok: {0}\n3Adminisztrátor parancsok!\n3Parancsok: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'colors', '1teszt1 2teszt2 3teszt3 4teszt4 5teszt5 6teszt6 7teszt7 8teszt8 9teszt9 10teszt10 11teszt11 12teszt12 13teszt13 14teszt14 15teszt15');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Nick megváltoztatása erre: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'join', 'Kapcsolódás ehhez a csatornához: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'leave', 'Lelépés erről a csatornáról: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/all/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/all', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update', 'Sikeresen frissítve {0} csatornán a funkciók.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update/all', 'Sikeresen frissítve minden csatornán a funkciók.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel', '3Parancsok: add | remove | info | update | language');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/add', 'A név már szerepel a csatorna listán!\nCsatorna hozzáadva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/remove', 'A mester csatorna nem törölhető!\nIlyen csatorna nem létezik!\nCsatorna eltávolítva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/update', 'A csatorna információk frissítésre kerültek.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/info', '3Aktív: {0}\n3Aktív: Nincs információ.\n3Inaktív: {0}\n3Inaktív: Nincs információ.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/language', 'Csatorna nyelvezete sikeresen meg lett változtatva erre: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'plugin/load', '2[Betöltés]: Összes plugin betöltése 3sikeres.\n2[Betöltés]: Összes plugin betöltése 5sikertelen.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'plugin/unload', '2[Leválasztás]: Összes plugin leválasztása 3sikeres.\n2[Leválasztás]: Összes plugin leválasztása 5sikertelen.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'plugin', '{0}: 3loaded.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'quit', 'Viszlát :(\n{0} leállított paranccsal.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/start', '{0} már el van indítva!\n{0} sikeresen el lett indítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/stop', '{0} már le van állítva!\n{0} sikeresen le lett állítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/reload', '{0} sikeresen újra lett indítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/reload/all', 'Minden rss újra lett indítva.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/channel/add', 'Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/channel/remove', 'Csatorna sikeresen törölve.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/start', '{0} már el van indítva!\n{0} sikeresen el lett indítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/stop', '{0} már le van állítva!\n{0} sikeresen le lett állítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/reload', '{0} sikeresen újra lett indítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/reload/all', 'Minden rss újra lett indítva.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/channel/add', 'Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/channel/remove', 'Csatorna sikeresen törölve.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/info', '3{0} 7{1} Channel: 2{2}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/start', '{0} {1} már el van indítva!\n{0} {1} sikeresen el lett indítva.\n{0} {1} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/stop', '{0} {1} már le van állítva!\n{0} {1} sikeresen le lett állítva.\n{0} {1} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/reload', '{0} {1} sikeresen újra lett indítva.\n{0} {1} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/reload/all', 'Minden rss újra lett indítva.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/channel/add', 'Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/channel/remove', 'Csatorna sikeresen törölve.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/memory', 'Jelenleg túl sok memóriát fogyaszt a bot ezért ezen funkció nem elérhető!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/warning', 'A kódban olyan részek vannak melyek veszélyeztetik a programot. Ezért leállt a fordítás!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler', 'Nincs megadva a fő fv! (Schumix)\nNincs megadva a fő class!\nA kimeneti szöveg túl hosszú ezért nem került kiírásra!\nA kód sikeresen lefordult csak nincs kimenő üzenet!\nHátramaradt még {0} kiírás!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/code', 'Hibák: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/kill', 'Szál kilőve!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handlekick', '{0} kirúgta a következő felhasználót: {1} oka: {2}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'ban', 'Helytelen dátum formátum!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction', '3Parancsok: hlmessage\n3Parancsok: kick | mode | hlmessage');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/info', '3Létező nickek: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/update', 'Az adatbázis sikeresen frissítésre került.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/hlmessage', 'Az üzenet módosításra került.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/add', 'A név már szerepel a kick listán!\nKick listához a név hozzáadva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/remove', 'Ilyen név nem létezik!\nKick listából a név eltávólításra került: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/list', '3Kick listán lévők: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/add', 'A név már szerepel a kick listán!\nKick listához a név hozzáadva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/remove', 'Ilyen név nem létezik!\nKick listából a név eltávólításra került: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/list', '3Kick listán lévők: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/add', 'A név már szerepel a mode listán!\nMode listához a név hozzáadva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/change', 'Ilyen név nem létezik!\n{0} új rangja: {1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/remove', 'Ilyen név nem létezik!\nMode listából a név eltávólításra került: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/list', '3Voice listán lévők: {0}\n3Fél Operátor listán lévők: {0}\n3Operátor listán lévők: {0}\n3Adminisztrátor listán lévők: {0}\n3Tulajdonos listán lévők: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/add', 'A név már szerepel a mode listán!\nMode listához a név hozzáadva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/change', 'Ilyen név nem létezik!\n{0} új rangja: {1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/remove', 'Ilyen név nem létezik!\nMode listából a név eltávólításra került: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/list', '3Voice listán lévők: {0}\n3Fél Operátor listán lévők: {0}\n3Operátor listán lévők: {0}\n3Adminisztrátor listán lévők: {0}\n3Tulajdonos listán lévők: {0}\nIlyen csatorna nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'message/channel', 'Az üzenet sikeresen feljegyzésre került.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'message', 'Az üzenet sikeresen feljegyzésre került.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/info', 'Jegyzetek kódjai: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/access', 'Hozzáférés engedélyezve.\nHozzáférés megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/newpassword', 'Jelszó sikeresen meg lett változtatva erre: {0}\nA mostani jelszó nem egyezik, módosítás megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/register', 'Már szerepelsz a felhasználói listán!\nSikeresen hozzá vagy adva a felhasználói listához.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/remove', 'Nincs megadva a jelszó a törlés megerősítéséhez!\nNem szerepelsz a felhasználói listán!\nA jelszó nem egyezik meg az adatbázisban tároltal!\nTörlés meg lett szakítva!\nSikeresen törölve lett a felhasználód.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/code/remove', 'Ilyen kód nem szerepel a listán!\nA jegyzet sikeresen törlésre került.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/code', 'Jegyzet: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes', 'Nincs megadva jegyzetnek semmi se!\nA jegyzet kódneve már szerepel az adatbázisban!\nJegyzet kódja: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/warning', 'Jelenleg nem szerepelsz a jegyzetek felhasználói listáján!\nAhoz hogy hozzáadd magad nem kell mást tenned mint az alábbi parancsot végrehajtani. (Lehetőleg privát üzenetként nehogy más megtudja.)\n{0}notes user register <jelszó>\nFelhasználói adatok frissítése (ha nem fogadná el adataidat) pedig: {0}notes user access <jelszó>');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'message2', 'Üzenetet hagyta neked: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reload', '{0} újra lett indítva.\nA programban nincs ilyen rész!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'weather', '12Időjárás otthon!\n5{0} 12időjárása!\n3Nappal: {0}\n3Éjszaka: {0}\nNem szerepel ilyen város a listán!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handlejoin/random', 'Hello\nCsáó\nHy\nSzevasz\nÜdv\nSzervusz\nAloha\nJó napot\nSzia\nHi\nSzerbusz\nHali\nSzeva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handlejoin', 'Jó reggelt {0}\nJó estét {0}\nÜdv főnök');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handleleft/random', 'Viszlát\nBye');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handleleft', 'Jóét {0}');

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
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/newpassword', 'Successfully changed to password to: {0}\nThe current password does not match, modification denied!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/info', 'You are half operator now.\nYou are operator now.\nYou are administrator now.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/list', '2Admins: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/add', 'The name is already in the admin list!\nAdmin added to the list: {0}\nYou are schumix\'s admin now. Your current password is: {0}\nIf you want to change it, use this command: {0}admin newpassword. Usage: {0}admin newpassword <Old> <New>\nAdmin nick confirmation: {0}admin access <Password>');
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
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/add', 'The name is already on the kick list!A név már szerepel a kick listán!\nKick listához a név hozzáadva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/remove', 'No such name!\nThe name has been deleted from the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/list', '3On the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/add', 'The name is already on the mode list!\nThe name added to the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/change', 'No such name!\n{0}\'s new rank: {1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/remove', 'No such name!\The name has been deleted from the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/list', '3Voice list: {0}\n3Half Operator list: {0}\n3Operator list: {0}\n3Administrator list: {0}\n3Owner list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/add', 'The name is already on the mode list!\nThe name added to the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/change', 'No such name!\n{0}\'s new rank: {1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/remove', 'No such name!\nThe name has been deleted from the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/list', '3Voice list: {0}\n3Half Operator list: {0}\n3Operator list: {0}\n3Administrator list: {0}\n3Owner list: {0}\nNo such channel!');
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
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/warning', 'You are not in the note\'s user list!\nIf you want to add yourself, you have to do the following command. (Must be a private message, do not gather info someone else.)\n{0}notes user register <password>\nUpdating user data (If do not accept your datas) Do: {0}notes user access <password>');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'message2', 'Left the note for you: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'reload', '{0} reloaded.\nThe program does not contains that part!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'weather', '12Local weather!\n5{0} 12weather!\n3Day: {0}\n3Night: {0}\nNo such city in the list!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handlejoin/random', 'Hello\nHy\nHi\nGood afternoon');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handlejoin', 'Good Morning {0}\nGood Night {0}\nWelcome boss!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handleleft/random', 'ByeBye\nBye');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handleleft', 'Goodbye {0}');

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
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'xbot', '9', 'Felhasználók számára használható parancslista.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'info', '9', 'Kis leírás a botról.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'whois', '9', 'A parancs segítségével megtudhatjuk hogy egy nick milyen csatornán van fent.\nHasználata: {0}whois <nick>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'roll', '9', 'Csöpp szorakozás a wowból, már ha valaki felismeri :P');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'date', '9', 'Az aktuális dátumot írja ki és a hozzá tartozó névnapot.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'time', '9', 'Az aktuális időt írja ki.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'google', '9', 'Ha szükséged lenne valamire a google-ből nem kell hozzá weboldal csak ez a parancs.\nHasználata: {0}google <ide jön a keresett szöveg>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'translate', '9', 'Ha rögtön kéne fordítani másik nyelvre vagy -ről valamit, akkor megteheted ezzel a parancsal.\nHasználata: {0}translate <kiindulási nyelv|cél nyelv> <szöveg>\nPéldául: {0}translate hu|en Szép szöveg.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'irc', '9', 'Néhány parancs használata az IRC-n.\nHasználata: {0}irc <parancs neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'calc', '9', 'Több funkciós számológép.\nHasználata: {0}calc <szám>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'warning', '9', 'Figyelmeztető üzenet küldése, hogy keresik ezen a csatornán vagy egy tetszőleges üzenet küldése.\nHasználata: {0}warning <ide jön a személy> <ha nem csak felhívást küldenél, hanem saját üzenetet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'sha1', '9', 'Sha1 kódolássá átalakító parancs.\nHasználata: {0}sha1 <átalakítandó szöveg>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'md5', '9', 'Md5 kódolássá átalakító parancs.\nHasználata: {0}md5 <átalakítandó szöveg>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'prime', '9', 'Megálapítja hogy a szám prímszám-e. Csak egész számmal tud számolni!\nHasználata: {0}prime <szám>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin', '0', 'Kiírja az operátorok vagy adminisztrátorok által használható parancsokat.\nAdmin parancsai: info | list | add | remove | rank | access | newpassword');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/add', '0', 'Új admin hozzáadása.\nHasználata: {0}admin add <admin neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/remove', '0', 'Admin eltávolítása.\nHasználata: {0}admin remove <admin neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/rank', '0', 'Admin rangjának megváltoztatása.\nHasználata: {0}admin rank <admin neve> <új rang pl halfoperator: 0, operator: 1, administrator: 2>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/info', '0', 'Kiírja éppen milyen rangod van.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/list', '0', 'Kiírja az összes admin nevét aki az adatbázisban szerepel.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/access', '0', 'Az admin parancsok használatához szükséges jelszó ellenörző és vhost aktiváló.\nHasználata: {0}admin access <jelszó>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'admin/newpassword', '0', 'Az admin jelszavának cseréje ha új kéne a régi helyett.\nHasználata: {0}admin newpassword <régi jelszó> <új jelszó>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'colors', '0', 'Adott skálájú színek kiírása amit lehet használni IRC-n.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'nick', '0', 'Bot nick nevének cseréje.\nHasználata: {0}nick <név>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'join', '0', 'Kapcsolódás a megadott csatornára.\nHasználata:\nJelszó nélküli csatorna: {0}join <csatorna>\nJelszóval ellátott csatorna: {0}join <csatorna> <jelszó>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'leave', '0', 'Lelépés a megadott csatonáról.\nHasználata: {0}leave <csatona>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel', '1', 'Channel parancsai: add | remove | info | update | language');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/add', '1', 'Új csatorna hozzáadása.\nHasználata: {0}channel add <csatorna> <ha van jelszó akkor az>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/remove', '1', 'Nem használatos channel eltávolítása.\nHasználata: {0}channel remove <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/info', '1', 'Összes channel kiírása ami az adatbázisban van és a hozzájuk tartozó informáciok.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/update', '1', 'A csatornákhoz tartozó összes információ frissítése, alapértelmezésre állítása.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'channel/language', '1', 'Frissíti a csatorna nyelvezetét.\nHasználata: {0}channel language <csatorna> <nyelvezet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function', '1', 'Funkciók vezérlésére szolgáló parancs.\nFunkció parancsai: channel | all | update | info\nHasználata ahol tartózkodsz:\nChannel funkció kezelése: {0}function <on vagy off> <funkció név>\nChannel funkciók kezelése: {0}function <on vagy off> <funkció név1> <funkció név2> ... stb');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/channel', '1', 'Megadott csatornán állithatók ezzel a parancsal a funkciók.\nFunkció channel parancsai: info\nHasználata:\nChannel funkció kezelése: {0}function channel <csatorna> <on vagy off> <funkció név>\nChannel funkciók kezelése: {0}function channel <csatorna> <on vagy off> <funkció név1> <funkció név2> ... stb');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/channel/info', '1', 'Kiírja a funkciók állapotát.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/all', '1', 'Globális funkciók kezelése.\nFunkció all parancsai: info\nGlobális funkció kezelése: {0}function all <on vagy off> <funkció név>\nGlobális funkciók kezelése: {0}function all <on vagy off> <funkció név1> <funkció név2> ... stb');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/all/info', '1', 'Kiírja a funkciók állapotát.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/update', '1', 'Frissíti a funkciókat vagy alapértelmezésre állítja.\nFunkció update parancsai: all\nHasználata:\nMás channel: {0}function update <csatorna>\nAhol tartózkodsz channel: {0}function update');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/update/all', '1', 'Frissíti az összes funkciót vagy alapértelmezésre állítja.\Használata: {0}function update all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'function/info', '1', 'Kiírja a funkciók állapotát.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'kick', '1', 'Kirúgja a nick-et a megadott csatornáról.\nHasználata:\nCsak kirúgás: {0}kick <csatorna> <név>\nKirúgás okkal: {0}kick <csatorna> <név> <oka>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mode', '1', 'Megváltoztatja a nick rangját megadott csatornán.\nHasználata: {0}mode <rang> <név vagy nevek>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'plugin', '2', 'Kiírja milyen pluginok vannak betöltve.\nPlugin parancsok: load | unload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'plugin/load', '2', 'Betölt minden plugint.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'plugin/unload', '2', 'Eltávolít minden plugint.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'quit', '2', 'Bot leállítására használható parancs.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2', '9', 'Parancsok: nick | sys\nParancsok: ghost | nick | sys\nParancsok: ghost | nick | sys | clean\nParancsok: sys');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/sys', '9', 'Kiírja a program információit.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/ghost', '1', 'Kilépteti a fő nick-et ha regisztrálva van.\nHasználata: {0} ghost');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/nick', '0', 'Bot nick nevének cseréje.\nParancsok: identify\nHasználata: {0} nick <név>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/nick/identify', '0', 'Aktiválja a fő nick jelszavát.\nHasználata: {0} nick identify');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'schumix2/clean', '2', 'Felszabadítja a lefoglalt memóriát.\nHasználata: {0} clean');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn', '1', 'Svn rss-ek kezelése.\nSvn parancsai: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/channel', '1', 'Rss csatornákra való kiirásának kezelése.\nChannel parancsai: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/channel/add', '1', 'Új csatorna hozzáadása az rss-hez.\nHasználata: {0}svn channel add <rss neve> <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/channel/remove', '1', 'Nem használatos csatorna eltávolítása az rss-ből.\nHasználata: {0}svn channel remove <rss neve> <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/info', '1', 'Kiírja az rss-ek állapotát.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/list', '1', 'Választható rss-ek listája.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/start', '1', 'Új rss betöltése.\nHasználata: {0}svn start <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/stop', '1', 'Rss leállítása.\nHasználata: {0}svn stop <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/reload', '1', 'Megadott rss újratöltése.\nSvn reload parancsai: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'svn/reload/all', '1', 'Minden rss újratöltése.\nHasználata: {0}svn reload <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg', '1', 'Hg rss-ek kezelése.\nHg parancsai: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/channel', '1', 'Rss csatornákra való kiírásának kezelése.\nChannel parancsai: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/channel/add', '1', 'Új csatorna hozzáadása az rss-hez.\nHasználata: {0}hg channel add <rss neve> <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/channel/remove', '1', 'Nem használatos csatorna eltávolítása az rss-ből.\nHasználata: {0}hg channel remove <rss neve> <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/info', '1', 'Kiírja az rss-ek állapotát.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/list', '1', 'Választható rss-ek listája.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/start', '1', 'Új rss betöltése.\nHasználata: {0}hg start <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/stop', '1', 'Rss leállítása.\nHasználata: {0}hg stop <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/reload', '1', 'Megadott rss újratöltése.\nHg reload parancsai: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'hg/reload/all', '1', 'Minden rss újratöltése.\nHasználata: {0}hg reload <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git', '1', 'Git rss-ek kezelése.\nGit parancsai: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/channel', '1', 'Rss csatornákra való kiirásának kezelése.\nChannel parancsai: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/channel/add', '1', 'Új csatorna hozzáadása az rss-hez.\nHasználata: {0}git channel add <rss neve> <tipus> <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/channel/remove', '1', 'Nem használatos csatorna eltávolítása az rss-ből.\nHasználata: {0}git channel remove <rss neve> <tipus> <csatorna>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/info', '1', 'Kiírja az rss-ek állapotát.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/list', '1', 'Választható rss-ek listája.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/start', '1', 'Új rss betöltése.\nHasználata: {0}git start <rss neve> <tipus>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/stop', '1', 'Rss leállítása.\nHasználata: {0}git stop <rss neve> <tipus>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/reload', '1', 'Megadott rss újratöltése.\nGit reload parancsai: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'git/reload/all', '1', 'Minden rss újratöltése.\nHasználata: {0}git reload <rss neve> <tipus>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'ban', '1', 'Tiltást rak a megadott névre vagy vhost-ra.\nHasználata:\nÓra és perc: {0}ban <név> <óó:pp> <oka>\nDátum, Óra és perc: {0}ban <név> <éééé.hh.nn> <óó:pp> <oka>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'unban', '1', 'Feloldja a tiltást a névről vagy vhost-ról ha szerepel a bot rendszerében.\nHasználata: {0}unban <név vagy vhost>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes', '9', 'Különböző adatokat jegyezhetünk fel a segítségével.\nJegyzet parancsai: user | code\nJegyzet beküldése: {0}notes <egy kód amit megjegyzünk pl: schumix> <amit feljegyeznél>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/user', '9', 'Jegyzet felhasználó kezelése.\nUser parancsai: register | remove | access | newpassword');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/user/register', '9', 'Új felhasználó hozzáadása.\nHasználata: {0}notes user register <jelszó>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/user/remove', '9', 'Felhasználó eltávolítása.\nHasználata: {0}notes user remove <jelszó>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/user/access', '9', 'Az jegyzet parancsok használatához szükséges jelszó ellenörző és vhost aktiváló.\nHasználata: {0}notes user access <jelszó>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/user/newpassword', '9', 'Felhasználó jelszavának cseréje ha új kéne a régi helyet.\nHasználata: {0}notes user newpassword <régi jelszó> <új jelszó>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/code', '9', 'Jegyzet kiolvasásához szükséges kód.\nHasználata: {0}notes code <jegyzet kódja>\nKód parancsai: remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'notes/code/remove', '9', 'Törli a jegyzetet kód alapján.\nHasználata: {0}notes code remove <jegyzet kódja>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'message', '9', 'Ezzel a paranccsal üzenetet lehet hagyni bárkinek a megadott csatornán.\nHasználata: {0}message <név> <üzenet>\nÜzenet parancsai: channel');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'message/channel', '9', 'Ezzel a paranccsal üzenetet lehet hagyni bárkinek a kiválasztott csatornán.\nHasználata: {0}message channel <csatorna> <név> <üzenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction', '0', 'Autómatikusan müködő kódrészek kezelése.\nAutofunkcio parancsai: hlmessage\nAutómatikusan müködő kódrészek kezelése.\nAutofunkcio parancsai: kick | mode | hlmessage');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/hlmessage', '0', 'Autómatikusan hl-t kapó nick-ek kezelése.\nHl üzenet parancsai: function | update | info\nHasználata: {0}autofunction hlmessage <üzenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/function', '0', 'Ezzel a parancsal állítható a hl állapota.\nHasználata: {0}autofunction hlmessage function <állapot>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/update', '0', 'Frissíti az adatbázisban szereplő hl listát!');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/info', '0', 'Kiírja a hl-ek állapotát.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick', '1', 'Automatikusan kirugásra kerülő nick-ek kezelése.\nKick parancsai: add | remove | list | channel');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/add', '1', 'Kirugandó nevének hozzáadása ahol tartózkodsz.\nHasználata: {0}autofunction kick add <név> <oka>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/remove', '1', 'Kirugandó nevének eltávolítása ahol tartózkodsz.\nHasználata: {0}autofunction kick remove <név>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/list', '1', 'Kiírja a kirugandok állapotát.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/channel', '1', 'Automatikusan kirugásra kerülő nick-ek kezelése megadot csatornán.\nKick channel parancsai: add | remove | list');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/add', '1', 'Kirugandó nevének hozzáadása megadott csatornán.\nHasználata: {0}autofunction kick channel <csatorna> add <név> <oka>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/remove', '1', 'Kirugandó nevének eltávolítása megadott csatornán.\nHasználata: {0}autofunction kick channel <csatorna> remove <név>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/list', '1', 'Kiírja a kirugandok állapotát a megadott csatornán.\nHasználata: {0}autofunction kick channel <csatorna> info');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode', '1', 'Automatikusan rangot kapó nick-ek kezelése.\nMode parancsai: add | change | remove | list | channel');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/add', '1', 'Rangot kapó nevének hozzáadása ahol tartózkodsz.\nHasználata: {0}autofunction mode add <név> <rang>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/change', '1', 'Rang megváltoztatása.\nHasználata: {0}autofunction mode change <név>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/remove', '1', 'Rangot kapó nevének eltávolítása ahol tartózkodsz.\nHasználata: {0}autofunction mode remove <név>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/list', '1', 'Kiírja a rangot kapók állapotát.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel', '1', 'Automatikusan rangot kapó nick-ek kezelése megadot csatornán.\nMode channel parancsai: add | change | remove | list');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/add', '1', 'Rangot kapó nevének hozzáadása megadott csatornán.\nHasználata: {0}autofunction mode channel <csatorna> add <név> <rang>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/change', '1', 'Rang megváltoztatása a megadott csatornán.\nHasználata: {0}autofunction mode channel <csatorna> change <név>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/remove', '1', 'Rangot kapó nevének eltávolítása megadott csatornán.\nHasználata: {0}autofunction mode channel <csatorna> remove <név>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/list', '1', 'Kiírja a rangot kapók állapotát a megadott csatornán.\nHasználata: {0}autofunction mode channel <csatorna> info');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'reload', '2', 'Újraindítja a megadott programrészt.\nHasználata: {0}reload <név>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'weather', '9', 'Megmondja az időjárást a megadott városban.\nHasználata: {0}weather <város>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'game', '9', 'Játékok indítása irc-n.\nJáték parancsai: start');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'game/start', '9', 'Játék indítására szolgáló parancs.\Hanszálata: {0}game start <játék neve>');

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
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoName', 'A név nincs megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoValue', 'Nincs paraméter!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'No1Value', 'Nincs megadva egy paraméter!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'FaultyQuery', 'Hibás lekérdezés!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoIrcCommandName', 'Nincs megadva a parancs neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoWhoisName', 'Nincs megadva a keresendő személy neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoGoogleText', 'Nincs megadva a keresendő szöveg!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTranslateText', 'Nincs megadva a fordítandó szöveg!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTranslateLanguage', 'Nincs megadva melyik nyelvről melyikre fordítsa le!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoNumber', 'Nincs megadva szám!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoPassword', 'Nincs megadva a jelszó!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoOldPassword', 'Nincs megadva a régi jelszó!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoNewPassword', 'Nincs megadva az új jelszó!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoOperator', 'Nem vagy Operátor!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoAdministrator', 'Nem vagy Adminisztrátor!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoChannelName', 'Nincs megadva a csatorna neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoRank', 'Nincs megadva a rang!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoFunctionName', 'Nincs megadva a funkció neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoFunctionStatus', 'Nincs megadva a funkció állapota!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoCommand', 'Nincs megadva a parancs!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTypeName', 'Nincs megadva a típus neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'CapsLockOff', 'Kapcsold ki a caps lock-ot!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTime', 'Nincs megadva az idő!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoBanNameOrVhost', 'Nincs megadva a kitiltandó neve vagy a vhost!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoUnbanNameOrVhost', 'Nincs megadva a kitiltott neve vagy a vhost!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'BanList', 'Már szerepel a tiltó listán!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'BanList1', 'Sikeresen hozzá lett adva a tiltó listához.');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'UnbanList', 'Nem szerepel a tiltó listán!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'UnbanList1', 'Sikeresen törölve lett a tiltó listához.');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'RecurrentFlooding', 'Ismétlődő flooddal!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'StopFlooding', 'Állj le a floodolással!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoMessage', 'Üzenet nincs megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoCode', 'A kód nincs megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoReason', 'Nincs megadva az ok!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoChannelLanguage', 'Nincs megadva a csatorna nyelvezete!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoDataNoCommand', 'Az adataid nem megfelelőek ezért nem folytatható a parancs!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoCityName', 'Nem adott meg egy városnevet sem!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoMessageFunction', 'A funkció jelenleg nem üzemel!');

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
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoMessageFunction', 'This function is currently not operating!');

