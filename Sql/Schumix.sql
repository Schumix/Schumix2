DROP TABLE IF EXISTS `adminok`;
CREATE TABLE `adminok` (
  `Id` int(3) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Password` varchar(40) NOT NULL default '',
  `Vhost` varchar(50) NOT NULL default '',
  `Flag` int(1) NOT NULL DEFAULT '0',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `channel`;
CREATE TABLE `channel` (
  `Id` int(3) unsigned NOT NULL auto_increment,
  `Functions` varchar(255) NOT NULL default ',koszones:ki,log:be,rejoin:be,parancsok:be,hl:ki,kick:ki,mode:ki',
  `Channel` varchar(20) NOT NULL default '',
  `Password` varchar(30) NOT NULL default '',
  `Enabled` varchar(5) NOT NULL default '',
  `Error` text NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `channel` VALUES ('1', ',koszones:be,log:be,rejoin:be,parancsok:be,hl:ki,kick:ki,mode:ki', '#schumix2', '', '', '');

DROP TABLE IF EXISTS `irc_parancsok`;
CREATE TABLE `irc_parancsok` (
  `guid` int(10) unsigned NOT NULL auto_increment,
  `parancs` varchar(21) NOT NULL default '',
  `hasznalata` text NOT NULL default '',
  PRIMARY KEY  (`guid`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `irc_parancsok` VALUES ('1', 'rang', 'Rang hasznalata: /mode <channel> <rang> <nev>');
INSERT INTO `irc_parancsok` VALUES ('2', 'rang1', 'Rang mentese: /chanserv <rang (sop, aop, hop, vop)> <channel> ADD <nev>');
INSERT INTO `irc_parancsok` VALUES ('3', 'nick', 'Nick csere hasznalata: /nick <uj nev>');
INSERT INTO `irc_parancsok` VALUES ('4', 'kick', 'Kick hasznalata: /kick <channel> <nev> (<oka> nem feltetlen kell)');
INSERT INTO `irc_parancsok` VALUES ('5', 'owner', 'Ownermod hasznalata: /msg chanserv SET <channel> ownermode on');

DROP TABLE IF EXISTS `schumix`;
CREATE TABLE `schumix` (
  `entry` int(10) unsigned NOT NULL auto_increment,
  `csatorna` varchar(21) NOT NULL default '',
  `funkcio_nev` varchar(21) NOT NULL default '',
  `funkcio_status` varchar(10) NOT NULL default '',
  PRIMARY KEY  (`entry`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `schumix` VALUES ('1', '', 'koszones', 'be');
INSERT INTO `schumix` VALUES ('2', '', 'log', 'be');
INSERT INTO `schumix` VALUES ('3', '', 'rejoin', 'be');
INSERT INTO `schumix` VALUES ('4', '', 'parancsok', 'be');
INSERT INTO `schumix` VALUES ('5', '', 'reconnect', 'be');
INSERT INTO `schumix` VALUES ('6', '', 'hl', 'ki');
INSERT INTO `schumix` VALUES ('7', '', 'kick', 'ki');
INSERT INTO `schumix` VALUES ('8', '', 'mode', 'ki');

DROP TABLE IF EXISTS `sznap`;
CREATE TABLE `sznap` (
  `guid` int(10) unsigned NOT NULL auto_increment,
  `nev` text NOT NULL,
  `honap` varchar(21) NOT NULL default '',
  `honap1` tinyint(3) unsigned NOT NULL,
  `nap` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY  (`guid`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `uptime`;
CREATE TABLE `uptime` (
  `id` int(100) unsigned NOT NULL auto_increment,
  `datum` text NOT NULL,
  `uptime` text NOT NULL,
  `memory` text NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `hlmessage`;
CREATE TABLE `hlmessage` (
  `Id` int(3) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Info` text NOT NULL,
  `Enabled` varchar(2) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `kicklist`;
CREATE TABLE `kicklist` (
  `Id` int(3) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Channel` varchar(20) NOT NULL default '',
  `Reason` text NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `modelist`;
CREATE TABLE `modelist` (
  `Id` int(3) unsigned NOT NULL auto_increment,
  `Name` varchar(20) NOT NULL default '',
  `Channel` varchar(20) NOT NULL default '',
  `Rank` varchar(10) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

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
