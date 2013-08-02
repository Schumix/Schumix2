INSERT INTO `schumix` VALUES ('19', 'wordpress', 'off');
INSERT INTO `schumix` VALUES ('20', 'chatterbot', 'off');

ALTER TABLE channel CHANGE column `Functions` `Functions` varchar(500) NOT NULL default ',greeter:off,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off,gamecommands:off,webtitle:off,randomkick:off,chatterbot:off';
UPDATE `channel` SET `Functions` = concat(Functions, ',chatterbot:off');

-- ----------------------------
-- Table structure for wordpressinfo
-- ----------------------------
DROP TABLE IF EXISTS `wordpressinfo`;
CREATE TABLE `wordpressinfo` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Link` varchar(255) NOT NULL default '',
  `Channel` text NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- INSERT INTO `wordpressinfo` VALUES ('1', 'Yeahunter.hu', 'http://yeahunter.hu/feed/', '#hun_bot,#schumix2'); -- Példa a használatra

-- huHU
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'wordpress/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'wordpress/list', '2Lista:3{0}');

INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'wordpress/start', '{0} már el van indítva!\n{0} sikeresen el lett indítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'wordpress/stop', '{0} már le van állítva!\n{0} sikeresen le lett állítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'wordpress/reload', '{0} sikeresen újra lett indítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'wordpress/reload/all', 'Minden rss újra lett indítva.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'wordpress/channel/add', 'Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'wordpress/channel/remove', 'Csatorna sikeresen törölve.\nNem létezik ilyen név!');

-- enUS
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'wordpress/info', '3{0} Channel: 2{1}');

INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'wordpress/list', '2List:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'wordpress/start', '{0} already translated!\n{0} successfully started.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'wordpress/stop', '{0} already stopped!\n{0} successfully stopped.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'wordpress/reload', '{0} successfully restarted.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'wordpress/reload/all', 'All of Rss is restarted.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'wordpress/channel/add', 'Successfully added channel.\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'wordpress/channel/remove', 'Successfully deleted channel!\nNo such name!');

-- huHU
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'wordpress', '1', 'WordPress rss-ek kezelése.\nWordPress parancsai: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'wordpress/channel', '1', 'Rss csatornákra való kiírásának kezelése.\nChannel parancsai: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'wordpress/channel/add', '1', 'Új csatorna hozzáadása az rss-hez.\nHasználata: {0}wordpress channel add <rss neve> <csatorna neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'wordpress/channel/remove', '1', 'Nem használatos csatorna eltávolítása az rss-ből.\nHasználata: {0}wordpress channel remove <rss neve> <csatorna neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'wordpress/info', '1', 'Kiírja az rss-ek állapotát.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'wordpress/list', '1', 'Választható rss-ek listája.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'wordpress/start', '1', 'Új rss betöltése.\nHasználata: {0}wordpress start <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'wordpress/stop', '1', 'Rss leállítása.\nHasználata: {0}wordpress stop <rss neve>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'wordpress/reload', '1', 'Megadott rss újratöltése.\nWordPress reload parancsai: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'wordpress/reload/all', '1', 'Minden rss újratöltése.\nHasználata: {0}wordpress reload <rss neve>');

-- enUS
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'wordpress', '1', 'Rss wordpress \'s management.\nWordPress commands: channel | info | list | start | stop | reload');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'wordpress/channel', '1', 'RSS feeds on their handling of the announcement.\nChannel commands: add | remove');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'wordpress/channel/add', '1', 'New channel added to the rss.\nUse: {0}wordpress channel add <rss name> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'wordpress/channel/remove', '1', 'Removed from the RSS Channel.\nUse: {0}wordpress channel remove <rss name> <channel name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'wordpress/info', '1', 'Prints rss-s condition.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'wordpress/list', '1', 'Optional list of rss.');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'wordpress/start', '1', 'New RSS feeds.\nUse: {0}wordpress start <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'wordpress/stop', '1', 'Rss stop.\nUse: {0}wordpress stop <rss name>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'wordpress/reload', '1', 'Specify rss reload.\nWordPress reload command: all');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'wordpress/reload/all', '1', 'All RSS reload.\nUse: {0}wordpress reload <rss name>');
