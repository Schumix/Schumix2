DROP TABLE IF EXISTS `kicklist`;
CREATE TABLE `kicklist` (
  `Id` int(3) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Channel` varchar(20) NOT NULL default '',
  `Reason` text NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

INSERT INTO `schumix` VALUES ('7', '', 'kick', 'ki');
ALTER TABLE channel CHANGE column `Functions` `Functions` varchar(255) NOT NULL default ',koszones:ki,log:be,rejoin:be,parancsok:be,hl:ki,kick:ki';
UPDATE channel SET Functions = ',koszones:ki,log:be,rejoin:be,parancsok:be,hl:ki,kick:ki';

DROP TABLE IF EXISTS `modelist`;
CREATE TABLE `modelist` (
  `Id` int(3) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Channel` varchar(20) NOT NULL default '',
  `Rank` varchar(10) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

INSERT INTO `schumix` VALUES ('8', '', 'mode', 'ki');
ALTER TABLE channel CHANGE column `Functions` `Functions` varchar(255) NOT NULL default ',koszones:ki,log:be,rejoin:be,parancsok:be,hl:ki,kick:ki,mode:ki';
UPDATE channel SET Functions = ',koszones:ki,log:be,rejoin:be,parancsok:be,hl:ki,kick:ki,mode:ki';
