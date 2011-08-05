RENAME TABLE `adminok` TO `admins`;

DROP TABLE IF EXISTS `irc_parancsok`;
CREATE TABLE `irc_commands` (
  `Id` int(4) unsigned NOT NULL auto_increment,
  `Command` varchar(30) NOT NULL default '',
  `Message` text NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;

INSERT INTO `irc_commands` VALUES ('1', 'rang', 'Rang használata: /mode <channel> <rang> <név>');
INSERT INTO `irc_commands` VALUES ('2', 'rang1', 'Rang mentése: /chanserv <rang (sop, aop, hop, vop)> <channel> ADD <név>');
INSERT INTO `irc_commands` VALUES ('3', 'nick', 'Nick csere használata: /nick <új név>');
INSERT INTO `irc_commands` VALUES ('4', 'kick', 'Kick használata: /kick <channel> <név> (<oka> nem feltétlen kell)');
INSERT INTO `irc_commands` VALUES ('5', 'owner', 'Ownermod használata: /msg chanserv SET <channel> ownermode on');

CREATE TABLE `hlmessage_new` (
  `Id` int(3) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Info` text NOT NULL,
  `Enabled` varchar(2) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
INSERT INTO hlmessage_new (Id, Name, Info, Enabled) SELECT hlmessage.Id, hlmessage.Name, hlmessage.Info, hlmessage.Enabled FROM hlmessage WHERE NOT EXISTS (SELECT 1 FROM hlmessage_new WHERE hlmessage_new.Id = hlmessage.Id);
DROP TABLE IF EXISTS `hlmessage`;
RENAME TABLE `hlmessage_new` TO `hlmessage`;

CREATE TABLE `kicklist_new` (
  `Id` int(3) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Channel` varchar(20) NOT NULL default '',
  `Reason` text NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
INSERT INTO kicklist_new (Id, Name, Channel, Reason) SELECT kicklist.Id, kicklist.Name, kicklist.Channel, kicklist.Reason FROM kicklist WHERE NOT EXISTS (SELECT 1 FROM kicklist_new WHERE kicklist_new.Id = kicklist.Id);
DROP TABLE IF EXISTS `kicklist`;
RENAME TABLE `kicklist_new` TO `kicklist`;

CREATE TABLE `notes_new` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Code` text NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Note` text NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
INSERT INTO notes_new (Id, Code, Name, Note) SELECT notes.Id, notes.Code, notes.Name, notes.Note FROM notes WHERE NOT EXISTS (SELECT 1 FROM notes_new WHERE notes_new.Id = notes.Id);
DROP TABLE IF EXISTS `notes`;
RENAME TABLE `notes_new` TO `notes`;

CREATE TABLE `banned_new` (
  `Id` int(5) unsigned NOT NULL auto_increment,
  `Name` varchar(50) NOT NULL default '',
  `Channel` varchar(20) NOT NULL default '',
  `Reason` text NOT NULL,
  `Year` int(4) NOT NULL DEFAULT '0',
  `Month` int(2) NOT NULL DEFAULT '0',
  `Day` int(2) NOT NULL DEFAULT '0',
  `Hour` int(2) NOT NULL DEFAULT '0',
  `Minute` int(2) NOT NULL DEFAULT '0',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
INSERT INTO banned_new (Id, Name, Channel, Reason, Year, Month, Day, Hour, Minute) SELECT banned.Id, banned.Name, banned.Channel, banned.Reason, banned.Year, banned.Month, banned.Day, banned.Hour, banned.Minute FROM banned WHERE NOT EXISTS (SELECT 1 FROM banned_new WHERE banned_new.Id = banned.Id);
DROP TABLE IF EXISTS `banned`;
RENAME TABLE `banned_new` TO `banned`;

CREATE TABLE `message_new` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Channel` varchar(20) NOT NULL default '',
  `Message` text NOT NULL,
  `Wrote` varchar(20) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
INSERT INTO message_new (Id, Name, Channel, Message, Wrote) SELECT message.Id, message.Name, message.Channel, message.Message, message.Wrote FROM message WHERE NOT EXISTS (SELECT 1 FROM message_new WHERE message_new.Id = message.Id);
DROP TABLE IF EXISTS `message`;
RENAME TABLE `message_new` TO `message`;
