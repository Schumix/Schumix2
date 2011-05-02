DROP TABLE IF EXISTS `message`;
CREATE TABLE `message` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Channel` varchar(20) NOT NULL default '',
  `Message` text NOT NULL,
  `Wrote` varchar(20) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

INSERT INTO `schumix` VALUES ('13', '', 'uzenet', 'ki');
ALTER TABLE channel CHANGE column `Functions` `Functions` varchar(255) NOT NULL default ',koszones:ki,log:be,rejoin:be,parancsok:be,autohl:ki,autokick:ki,automode:ki,antiflood:ki,uzenet:ki';
UPDATE channel SET Functions = ',koszones:ki,log:be,rejoin:be,parancsok:be,autohl:ki,autokick:ki,automode:ki,antiflood:ki,uzenet:ki';
