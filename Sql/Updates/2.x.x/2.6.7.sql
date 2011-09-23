DROP TABLE IF EXISTS `hlmessage`;
CREATE TABLE `hlmessage` (
  `Id` int(3) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Info` text NOT NULL,
  `Enabled` varchar(2) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

INSERT INTO `schumix` VALUES ('6', '', 'hl', 'ki');
ALTER TABLE channel CHANGE column `Functions` `Functions` varchar(255) NOT NULL default ',koszones:ki,log:be,rejoin:be,parancsok:be,hl:ki';
UPDATE channel SET Functions = ',koszones:ki,log:be,rejoin:be,parancsok:be,hl:ki';
