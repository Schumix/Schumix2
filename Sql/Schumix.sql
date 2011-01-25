DROP TABLE IF EXISTS `adminok`;
CREATE TABLE `adminok` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `nev` varchar(21) NOT NULL default '',
  `jelszo` varchar(100) NOT NULL default '',
  `vhost` varchar(100) NOT NULL default '',
  `flag` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

DROP TABLE IF EXISTS `channel`;
CREATE TABLE `channel` (
  `id` int(10) unsigned NOT NULL auto_increment,
  `funkciok` varchar(255) NOT NULL default ',koszones:be,log:be,rejoin:be,parancsok:be',
  `szoba` text NOT NULL default '',
  `jelszo` text NOT NULL default '',
  `aktivitas` text NOT NULL default '',
  `error` text NOT NULL default '',
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Records 
-- ----------------------------
INSERT INTO `channel` VALUES ('1', ',koszones:be,log:be,rejoin:be,parancsok:be', '#schumix2', '', '', '');

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
INSERT INTO `irc_parancsok` VALUES ('5', 'owner', 'Ownermod hasznalata: /msg chanserv SET <channel> ownermode on ');

DROP TABLE IF EXISTS `schumix`;
CREATE TABLE `schumix` (
  `entry` int(10) unsigned NOT NULL auto_increment,
  `irc_cim` varchar(21) NOT NULL default '',
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
