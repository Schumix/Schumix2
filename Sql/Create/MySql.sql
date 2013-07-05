/* MySql.sql */

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for admins
-- ----------------------------
CREATE TABLE `admins` (
  `Id` int(3) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Password` varchar(40) NOT NULL default '',
  `Vhost` varchar(50) NOT NULL default '',
  `Flag` int(1) NOT NULL default '0',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for banned
-- ----------------------------
CREATE TABLE `banned` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(50) collate utf8_hungarian_ci NOT NULL default '',
  `Channel` varchar(20) collate utf8_hungarian_ci NOT NULL default '',
  `Reason` text collate utf8_hungarian_ci NOT NULL,
  `Year` int(4) NOT NULL default '0',
  `Month` int(2) NOT NULL default '0',
  `Day` int(2) NOT NULL default '0',
  `Hour` int(2) NOT NULL default '0',
  `Minute` int(2) NOT NULL default '0',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for calendar
-- ----------------------------
CREATE TABLE `calendar` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(50) collate utf8_hungarian_ci NOT NULL default '',
  `Channel` varchar(20) collate utf8_hungarian_ci NOT NULL default '',
  `Message` text collate utf8_hungarian_ci NOT NULL,
  `Loops` varchar(5) NOT NULL default 'false',
  `Year` int(4) NOT NULL default '0',
  `Month` int(2) NOT NULL default '0',
  `Day` int(2) NOT NULL default '0',
  `Hour` int(2) NOT NULL default '0',
  `Minute` int(2) NOT NULL default '0',
  `UnixTime` int(20) NOT NULL DEFAULT '0',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for channels
-- ----------------------------
CREATE TABLE `channels` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Functions` varchar(500) NOT NULL default ',greeter:off,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off,gamecommands:off,webtitle:off,otherkick:off,chatterbot:off,nameday:off,birthday:off',
  `Channel` varchar(20) NOT NULL default '',
  `Password` varchar(30) NOT NULL default '',
  `Enabled` varchar(5) NOT NULL default 'false',
  `Hidden` varchar(5) NOT NULL default 'false',
  `Error` text NOT NULL,
  `Language` varchar(4) NOT NULL default 'enUS',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for gitinfo
-- ----------------------------
CREATE TABLE `gitinfo` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Type` varchar(20) NOT NULL default '',
  `Link` varchar(255) NOT NULL default '',
  `Website` varchar(30) NOT NULL default '',
  `ShortUrl` varchar(5) NOT NULL default 'false',
  `Colors` varchar(5) NOT NULL default 'true',
  `Channel` text NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for hginfo
-- ----------------------------
CREATE TABLE `hginfo` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Link` varchar(255) NOT NULL default '',
  `Website` varchar(30) NOT NULL default '',
  `ShortUrl` varchar(5) NOT NULL default 'false',
  `Colors` varchar(5) NOT NULL default 'true',
  `Channel` text NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 CHARSET=latin1;

-- ----------------------------
-- Table structure for mantisbt
-- ----------------------------
CREATE TABLE `mantisbt` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Link` varchar(255) NOT NULL default '',
  `ShortUrl` varchar(5) NOT NULL default 'false',
  `Colors` varchar(5) NOT NULL default 'true',
  `Channel` text NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 CHARSET=latin1;

-- ----------------------------
-- Table structure for hlmessage
-- ----------------------------
CREATE TABLE `hlmessage` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) collate utf8_hungarian_ci NOT NULL default '',
  `Info` text collate utf8_hungarian_ci NOT NULL,
  `Enabled` varchar(3) collate utf8_hungarian_ci NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for irc_commands
-- ----------------------------
CREATE TABLE `irc_commands` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` varchar(30) collate utf8_hungarian_ci NOT NULL default '',
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for kicklist
-- ----------------------------
CREATE TABLE `kicklist` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) collate utf8_hungarian_ci NOT NULL default '',
  `Channel` varchar(20) collate utf8_hungarian_ci NOT NULL default '',
  `Reason` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for localized_console_command
-- ----------------------------
CREATE TABLE `localized_console_command` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for localized_console_command_help
-- ----------------------------
CREATE TABLE `localized_console_command_help` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for localized_console_warning
-- ----------------------------
CREATE TABLE `localized_console_warning` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for localized_command
-- ----------------------------
CREATE TABLE `localized_command` (
  `Id` int(8) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for localized_command_help
-- ----------------------------
CREATE TABLE `localized_command_help` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `Language` varchar(4) collate utf8_hungarian_ci NOT NULL default 'enUS',
  `Command` text collate utf8_hungarian_ci NOT NULL,
  `Rank` int(1) NOT NULL default '0',
  `Text` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for localized_warning
-- ----------------------------
CREATE TABLE `localized_warning` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Language` varchar(4) COLLATE utf8_hungarian_ci NOT NULL DEFAULT 'enUS',
  `Command` text COLLATE utf8_hungarian_ci NOT NULL,
  `Text` text COLLATE utf8_hungarian_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for message
-- ----------------------------
CREATE TABLE `message` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) collate utf8_hungarian_ci NOT NULL default '',
  `Channel` varchar(20) collate utf8_hungarian_ci NOT NULL default '',
  `Message` text collate utf8_hungarian_ci NOT NULL,
  `Wrote` varchar(20) collate utf8_hungarian_ci NOT NULL default '',
  `UnixTime` int(20) NOT NULL DEFAULT '0',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for modelist
-- ----------------------------
CREATE TABLE `modelist` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Channel` varchar(20) NOT NULL default '',
  `Rank` varchar(10) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for notes
-- ----------------------------
CREATE TABLE `notes` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Code` text collate utf8_hungarian_ci NOT NULL,
  `Name` varchar(20) collate utf8_hungarian_ci NOT NULL default '',
  `Note` text collate utf8_hungarian_ci NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

-- ----------------------------
-- Table structure for notes_users
-- ----------------------------
CREATE TABLE `notes_users` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Password` varchar(40) NOT NULL default '',
  `Vhost` varchar(50) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for schumix
-- ----------------------------
CREATE TABLE `schumix` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `FunctionName` varchar(20) NOT NULL default '',
  `FunctionStatus` varchar(3) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for svninfo
-- ----------------------------
CREATE TABLE `svninfo` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Link` varchar(255) NOT NULL default '',
  `Website` varchar(30) NOT NULL default '',
  `ShortUrl` varchar(5) NOT NULL default 'false',
  `Colors` varchar(5) NOT NULL default 'true',
  `Channel` text NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for birthday
-- ----------------------------
CREATE TABLE `birthday` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Year` int(4) NOT NULL default '0',
  `Month` int(2) unsigned NOT NULL default '1',
  `Day` int(2) unsigned NOT NULL default '1',
  `Enabled` varchar(5) NOT NULL default 'false',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for uptime
-- ----------------------------
CREATE TABLE `uptime` (
  `Id` int(100) unsigned NOT NULL auto_increment,
  `Date` text NOT NULL,
  `Uptime` text NOT NULL,
  `Memory` int(20) NOT NULL DEFAULT '0',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for wordpressinfo
-- ----------------------------
CREATE TABLE `wordpressinfo` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Name` varchar(20) NOT NULL default '',
  `Link` varchar(255) NOT NULL default '',
  `ShortUrl` varchar(5) NOT NULL default 'false',
  `Colors` varchar(5) NOT NULL default 'true',
  `Channel` text NOT NULL,
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for ignore_channels
-- ----------------------------
CREATE TABLE `ignore_channels` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Channel` varchar(20) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for ignore_nicks
-- ----------------------------
CREATE TABLE `ignore_nicks` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Nick` varchar(30) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for ignore_commands
-- ----------------------------
CREATE TABLE `ignore_commands` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Command` varchar(30) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for ignore_irc_commands
-- ----------------------------
CREATE TABLE `ignore_irc_commands` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Command` varchar(30) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

-- ----------------------------
-- Table structure for maffiagame
-- ----------------------------
CREATE TABLE `maffiagame` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'key',
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Game` int(5) NOT NULL COMMENT 'th',
  `Name` varchar(25) NOT NULL COMMENT 'Player name',
  `Survivor` int(3) NOT NULL DEFAULT '1' COMMENT 'The player has survived?',
  `Job` int(3) NOT NULL COMMENT 'What the players job?',
  `Active` int(3) NOT NULL DEFAULT '1' COMMENT 'Is active game?',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=1;

-- ----------------------------
-- Table structure for ignore_addons
-- ----------------------------
CREATE TABLE `ignore_addons` (
  `Id` int(10) unsigned NOT NULL auto_increment,
  `ServerId` INT(10) NOT NULL DEFAULT '1',
  `ServerName` varchar(40) NOT NULL default '',
  `Addon` varchar(50) NOT NULL default '',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
