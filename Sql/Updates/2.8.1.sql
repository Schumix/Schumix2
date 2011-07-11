DROP TABLE IF EXISTS `gitinfo`;
CREATE TABLE `gitinfo` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Type` varchar(20) NOT NULL default '',
  `Link` varchar(255) NOT NULL default '',
  `Website` varchar(30) NOT NULL default '',
  `Channel` text NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- INSERT INTO `gitinfo` VALUES ('1', 'Schumix2', 'master', 'http://github.com/megax/Schumix2/commits/master.atom', 'github', '#hun_bot,#schumix'); Példa a használatra
INSERT INTO `gitinfo` VALUES ('1', 'Schumix2', 'master', 'http://github.com/megax/Schumix2/commits/master.atom', 'github', '#schumix');

INSERT INTO `schumix` VALUES ('11', '', 'git', 'ki');
