-- ----------------------------
-- Table structure for ignore_channels
-- ----------------------------
DROP TABLE IF EXISTS `ignore_channels`;
CREATE TABLE `ignore_channels` (
  `Id` int(4) unsigned NOT NULL auto_increment,
  `Channel` varchar(20) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for ignore_nicks
-- ----------------------------
DROP TABLE IF EXISTS `ignore_nicks`;
CREATE TABLE `ignore_nicks` (
  `Id` int(4) unsigned NOT NULL auto_increment,
  `Nick` varchar(30) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for ignore_commands
-- ----------------------------
DROP TABLE IF EXISTS `ignore_commands`;
CREATE TABLE `ignore_commands` (
  `Id` int(4) unsigned NOT NULL auto_increment,
  `Command` varchar(30) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for ignore_irc_commands
-- ----------------------------
DROP TABLE IF EXISTS `ignore_irc_commands`;
CREATE TABLE `ignore_irc_commands` (
  `Id` int(4) unsigned NOT NULL auto_increment,
  `Command` varchar(30) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
