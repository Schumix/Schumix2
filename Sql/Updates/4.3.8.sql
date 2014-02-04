-- ----------------------------
-- Table structure for rssinfo
-- ----------------------------
CREATE TABLE `rssinfo` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Link` varchar(255) NOT NULL default '',
  `Website` varchar(30) NOT NULL default '',
  `ShortUrl` varchar(5) NOT NULL default 'false',
  `Colors` varchar(5) NOT NULL default 'true',
  `Channel` text NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
