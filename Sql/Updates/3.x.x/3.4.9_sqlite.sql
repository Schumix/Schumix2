-- huHU
UPDATE `localized_command` SET Text = "3Verzió: 10{0}\n3Programot írta: {0}\n3Fejlesztők: {0}\n3Parancsok: {0}" WHERE Language = 'huHU' AND Command = 'xbot';
UPDATE `localized_command` SET Text = "3Fejlesztők: {0}.\n3Weboldal: {0}\n3Elérhetőség: [MSN] megax@megaxx.info" WHERE Language = 'huHU' AND Command = 'info';
UPDATE `localized_command` SET Text = "Jelenleg itt van fent: {0}\nJelenleg nincs fent!" WHERE Language = 'huHU' AND Command = 'whois';
UPDATE `localized_command` SET Text = "{0} keres téged itt: {1}" WHERE Language = 'huHU' AND Command = 'warning';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'mantisbt/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'mantisbt/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'mantisbt/start', '{0} már el van indítva!\n{0} sikeresen el lett indítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'mantisbt/stop', '{0} már le van állítva!\n{0} sikeresen le lett állítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'mantisbt/reload', '{0} sikeresen újra lett indítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'mantisbt/reload/all', 'Minden rss újra lett indítva.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'mantisbt/channel/add', 'Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'mantisbt/channel/remove', 'Csatorna sikeresen törölve.\nNem létezik ilyen név!');

-- enUS
UPDATE `localized_command` SET Text = "3Version: 10{0}\n3Programmed by: {0}\n3Developers: {0}\n3Commands: {0}" WHERE Language = 'enUS' AND Command = 'xbot';
UPDATE `localized_command` SET Text = "3Developers: {0}.\n3Website: {0}\n3Contact: [MSN] megax@megaxx.info" WHERE Language = 'enUS' AND Command = 'info';
UPDATE `localized_command` SET Text = "Now online here: {0}\nCurrently offline!" WHERE Language = 'enUS' AND Command = 'whois';
UPDATE `localized_command` SET Text = "{0} is looking for you here: {1}" WHERE Language = 'enUS' AND Command = 'warning';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'mantisbt/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'mantisbt/list', '2List:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'mantisbt/start', '{0} already translated!\n{0} successfully started.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'mantisbt/stop', '{0} already stopped!\n{0} successfully stopped.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'mantisbt/reload', '{0} successfully restarted.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'mantisbt/reload/all', 'All of Rss is restarted.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'mantisbt/channel/add', 'Successfully added channel.\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'mantisbt/channel/remove', 'Successfully deleted channel!\nNo such name!');

-- huHU
UPDATE `localized_command_help` SET Text = "Különböző adatokat jegyezhetünk fel a segítségével.\nJegyzet parancsai: info | user | code\nJegyzet beküldése: {0}notes <egy kód amit megjegyzünk pl: schumix> <amit feljegyeznél>" WHERE Language = 'huHU' AND Command = 'notes';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mantisbt', '1', 'MantisBT rss-ek kezelése.\nMantisBT parancsai: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mantisbt/channel', '1', 'Rss csatornákra való kiírásának kezelése.\nChannel parancsai: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mantisbt/channel/add', '1', 'Új csatorna hozzáadása az rss-hez.\nHasználata: {0}mantisbt channel add <rss neve> <csatorna neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mantisbt/channel/remove', '1', 'Nem használatos csatorna eltávolítása az rss-ből.\nHasználata: {0}mantisbt channel remove <rss neve> <csatorna neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mantisbt/info', '1', 'Kiírja az rss-ek állapotát.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mantisbt/list', '1', 'Választható rss-ek listája.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mantisbt/start', '1', 'Új rss betöltése.\nHasználata: {0}mantisbt start <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mantisbt/stop', '1', 'Rss leállítása.\nHasználata: {0}mantisbt stop <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mantisbt/reload', '1', 'Megadott rss újratöltése.\nMantisBT reload parancsai: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'mantisbt/reload/all', '1', 'Minden rss újratöltése.\nHasználata: {0}mantisbt reload <rss neve>');

-- enUS
UPDATE `localized_command_help` SET Text = "Various data can subscribe to this command.\nNotes commands: info | user | code\nSubmit a note: {0}notes <we note that a code example: schumix> <Includes the text that you want, if you remember the bot.>" WHERE Language = 'enUS' AND Command = 'notes';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'mantisbt', '1', 'Rss mantisbt \'s management.\nMantisBT commands: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'mantisbt/channel', '1', 'RSS feeds on their handling of the announcement.\nChannel commands: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'mantisbt/channel/add', '1', 'New channel added to the rss.\nUse: {0}mantisbt channel add <rss name> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'mantisbt/channel/remove', '1', 'Removed from the RSS Channel.\nUse: {0}mantisbt channel remove <rss name> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'mantisbt/info', '1', 'Prints rss-s condition.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'mantisbt/list', '1', 'Optional list of rss.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'mantisbt/start', '1', 'New RSS feeds.\nUse: {0}mantisbt start <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'mantisbt/stop', '1', 'Rss stop.\nUse: {0}mantisbt stop <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'mantisbt/reload', '1', 'Specify rss reload.\nMantisBT reload command: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'mantisbt/reload/all', '1', 'All RSS reload.\nUse: {0}mantisbt reload <rss name>');

-- ----------------------------
-- Table structure for "mantisbt"
-- ----------------------------
DROP TABLE IF EXISTS "mantisbt";
CREATE TABLE "mantisbt" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(20),
Link VARCHAR(255),
Channel TEXT
);

-- INSERT INTO `mantisbt` VALUES ('1', 'Teszt', 'http://teszt.hu/issues_rss.php?username=Megax&key=KEY-CODE', '#hun_bot,#schumix'); Példa a használatra

INSERT INTO "schumix" VALUES (18, 'mantisbt', 'off');
