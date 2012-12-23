-- ----------------------------
-- Table structure for birthday
-- ----------------------------
CREATE TABLE `birthday` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Month` int(2) unsigned NOT NULL default '1',
  `Day` int(2) unsigned NOT NULL default '1',
  `Enabled` varchar(5) NOT NULL default 'false',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
