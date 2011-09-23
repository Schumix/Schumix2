DROP TABLE IF EXISTS `svninfo`;
CREATE TABLE `svninfo` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Link` varchar(255) NOT NULL default '',
  `Website` varchar(30) NOT NULL default '',
  `Channel` text NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- INSERT INTO `svninfo` VALUES ('1', 'Sandshroud', 'http://www.assembla.com/spaces/Sandshroud/stream.rss', 'assembla', '#hun_bot,#schumix'); Példa a használatra

INSERT INTO `schumix` VALUES ('9', '', 'svn', 'ki');
