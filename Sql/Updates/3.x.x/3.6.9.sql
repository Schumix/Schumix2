-- huHU
UPDATE `localized_command` SET Text = "3{0}15/7{1} Channel: 2{2}" WHERE Language = 'huHU' AND Command = 'git/info';
UPDATE `localized_command` SET Text = "2Lista:{0}" WHERE Language = 'huHU' AND Command = 'git/list';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'calendar', 'Helytelen dátum formátum!');

-- enUS
UPDATE `localized_command` SET Text = "3{0}15/7{1} Channel: 2{2}" WHERE Language = 'enUS' AND Command = 'git/info';
UPDATE `localized_command` SET Text = "2List:{0}" WHERE Language = 'enUS' AND Command = 'git/list';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'calendar', 'Date format error!');

-- huHU
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'calendar', '9', 'Eseményeket, üzeneteket lehet feljegyezni vele megadott időpontra.\nParancsok: loop | nick | private\nHasználata:\nÓra és perc: {0}calendar <óó:pp> <üzenet>\nDátum, Óra és perc: {0}calendar <éééé.hh.nn> <óó:pp> <üzenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'calendar/loop', '0', 'Naponként ismétli meg a megadott időben az üzenetet.\nParancsok: nick | private\nHasználata:\nÓra és perc: {0}calendar loop <óó:pp> <üzenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'calendar/loop/nick', '0', 'Megadott személynek jegyzi fel. \nHasználata:\nÓra és perc: {0}calendar loop nick <név> <óó:pp> <üzenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'calendar/loop/private', '0', 'Privátban küldi el az üzenetet.\nParancsok: nick\nHasználata:\nÓra és perc: {0}calendar loop private <óó:pp> <üzenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'calendar/loop/private/nick', '0', 'Megadott személynek jegyzi fel és privátban küldi el neki. \nHasználata:\nÓra és perc: {0}calendar loop private nick <név> <óó:pp> <üzenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'calendar/nick', '9', 'Megadott személynek jegyzi fel. \nHasználata:\nÓra és perc: {0}calendar nick <név> <óó:pp> <üzenet>\nDátum, Óra és perc: {0}calendar nick <név> <éééé.hh.nn> <óó:pp> <üzenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'calendar/private', '9', 'Privátban küldi el az üzenetet.\nParancsok: nick\nHasználata:\nÓra és perc: {0}calendar private <óó:pp> <üzenet>\nDátum, Óra és perc: {0}calendar private <éééé.hh.nn> <óó:pp> <üzenet>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'calendar/private/nick', '9', 'Megadott személynek jegyzi fel és privátban küldi el neki.\nHasználata:\nÓra és perc: {0}calendar private nick <név> <óó:pp> <üzenet>\nDátum, Óra és perc: {0}calendar private nick <név> <éééé.hh.nn> <óó:pp> <üzenet>');

-- enUS
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'calendar', '9','With it you can save messages,events. Commands: loop | nick | private\nUses:\nHour and min: {0}calendar <hh:mm> <message>\nDate, hour and min: {0}calendar <yyyy.mm.dd> <hh:mm> <message>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'calendar/loop', '0', 'Reapet the message daily.\nCommands: nick | private\nHour and min: {0}calendar loop <hh:mm> <message>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'calendar/loop/nick', '0', 'Save for person.\nUses:\nHour and min: {0}calendar loop nick <name> <hh:mm> <message>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'calendar/loop/private', '0', 'Sends the message in private.\nCommands: nick\nUses:\nHour and min: {0}calendar loop private <hh:mm> <message>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'calendar/loop/private/nick', '0', 'Save for person and send in private.\nUses:\nHour and min: {0}calendar loop private nick <name> <hh:mm> <message>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'calendar/nick', '9', 'Save for person. \nUses:\nHour and min: {0}calendar nick <name> <hh:mm> <message>\nDate, Hour and min: {0}calendar nick <name> <yyyy.mm.dd> <hh:mm> <message>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'calendar/private', '9', 'send the message in private.\nCommands: nick\nUses:\nHour and min: {0}calendar private <hh:mm> <message>\nDate, Hour and min: {0}calendar private <yyyy.mm.dd> <hh:mm> <message>');
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('enUS', 'calendar/private/nick', '9', 'Save for person and send it in private.\nUses:\nHour and min: {0}calendar private nick <name> <óó:pp> <message>\nDate, Hour and min: {0}calendar private nick <name> <yyyy.mm.dd> <hh:mm> <message>');

-- huHU
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'Calendar', 'Sikeresen feljegyzésre került az üzenet.');

-- enUS
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'Calendar', 'Message succesfuly saved.');

-- ----------------------------
-- Table structure for calendar
-- ----------------------------
DROP TABLE IF EXISTS `calendar`;
CREATE TABLE `calendar` (
  `Id` int(5) unsigned NOT NULL auto_increment,
  `Name` varchar(50) collate utf8_hungarian_ci NOT NULL default '',
  `Channel` varchar(20) collate utf8_hungarian_ci NOT NULL default '',
  `Message` text collate utf8_hungarian_ci NOT NULL,
  `Loops` varchar(5) NOT NULL default 'false',
  `Year` int(4) NOT NULL default '0',
  `Month` int(2) NOT NULL default '0',
  `Day` int(2) NOT NULL default '0',
  `Hour` int(2) NOT NULL default '0',
  `Minute` int(2) NOT NULL default '0',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;
