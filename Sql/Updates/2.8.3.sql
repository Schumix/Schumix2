DROP TABLE IF EXISTS `notes`;
CREATE TABLE `notes` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Code` text NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Note` text NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `notes_users`;
CREATE TABLE `notes_users` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Password` varchar(40) NOT NULL default '',
  `Vhost` varchar(50) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
