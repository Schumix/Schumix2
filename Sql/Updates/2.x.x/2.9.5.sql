DROP TABLE IF EXISTS `banned`;
CREATE TABLE `banned` (
  `Id` int(5) unsigned NOT NULL auto_increment,
  `Name` varchar(50) NOT NULL default '',
  `Channel` varchar(20) NOT NULL default '',
  `Reason` text NOT NULL,
  `Year` int(4) NOT NULL DEFAULT '0',
  `Month` int(2) NOT NULL DEFAULT '0',
  `Day` int(2) NOT NULL DEFAULT '0',
  `Hour` int(2) NOT NULL DEFAULT '0',
  `Minute` int(2) NOT NULL DEFAULT '0',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

ALTER TABLE channel CHANGE column `Functions` `Functions` varchar(255) NOT NULL default ',koszones:ki,log:be,rejoin:be,parancsok:be,autohl:ki,autokick:ki,automode:ki,antiflood:ki';
UPDATE channel SET Functions = ',koszones:ki,log:be,rejoin:be,parancsok:be,autohl:ki,autokick:ki,automode:ki,antiflood:ki';

REPLACE `schumix` VALUES ('6', '', 'autohl', 'ki');
REPLACE `schumix` VALUES ('7', '', 'autokick', 'ki');
REPLACE `schumix` VALUES ('8', '', 'automode', 'ki');
INSERT INTO `schumix` VALUES ('12', '', 'antiflood', 'ki');
