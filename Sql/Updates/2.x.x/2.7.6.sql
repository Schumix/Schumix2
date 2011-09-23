DROP TABLE IF EXISTS `hginfo`;
CREATE TABLE `hginfo` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Link` varchar(255) NOT NULL default '',
  `Website` varchar(30) NOT NULL default '',
  `Channel` text NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- INSERT INTO `hginfo` VALUES ('1', 'TrinityDB', 'http://code.google.com/feeds/p/trinitydb/hgchanges/basic', 'google', '#hun_bot,#schumix'); Példa a használatra
-- INSERT INTO `hginfo` VALUES ('2', 'NeoCore', 'http://bitbucket.org/skyne/neocore/rss?token=2b6ceaf25f0a4c993ddc905327806e9c', 'bitbucket', '#hun_bot,#schumix'); Példa a használatra

INSERT INTO `schumix` VALUES ('10', '', 'hg', 'ki');
