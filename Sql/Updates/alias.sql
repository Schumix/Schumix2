-- ----------------------------
-- Table structure for alias
-- ----------------------------
CREATE TABLE `alias_irc_command` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `NewCommand` text collate utf8_hungarian_ci NOT NULL,
  `BaseCommand` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;
