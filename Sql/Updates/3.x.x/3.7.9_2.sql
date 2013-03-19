-- ----------------------------
-- Table structure for ignore_addons
-- ----------------------------
DROP TABLE IF EXISTS `ignore_addons`;
CREATE TABLE `ignore_addons` (
  `Id` int(4) unsigned NOT NULL auto_increment,
  `Addon` varchar(50) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;