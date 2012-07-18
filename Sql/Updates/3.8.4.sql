CREATE TABLE `channels` (
  `Id` int(3) unsigned NOT NULL auto_increment,
  `Functions` varchar(500) NOT NULL default ',greeter:off,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off,gamecommands:off,webtitle:off,randomkick:off,chatterbot:off,nameday:off',
  `Channel` varchar(20) NOT NULL default '',
  `Password` varchar(30) NOT NULL default '',
  `Enabled` varchar(5) NOT NULL default '',
  `Error` text NOT NULL,
  `Language` varchar(4) NOT NULL default 'enUS',
  PRIMARY KEY  (`Id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;

INSERT INTO channels (Id, Functions, Channel, Password, Enabled, Error, Language) SELECT channel.Id, channel.Functions, channel.Channel, channel.Password, channel.Enabled, channel.Error, channel.Language FROM channel WHERE NOT EXISTS (SELECT 1 FROM channels WHERE channels.Id = channel.Id);
DROP TABLE IF EXISTS `channel`;

ALTER TABLE `channels` ADD COLUMN `ServerId` INT(10) NOT NULL DEFAULT '1' AFTER `Id`;
ALTER TABLE `channels` ADD COLUMN `ServerName` varchar(40) NOT NULL default '' AFTER `ServerId`;
ALTER TABLE `schumix` ADD COLUMN `ServerId` INT(10) NOT NULL DEFAULT '1' AFTER `Id`;
ALTER TABLE `schumix` ADD COLUMN `ServerName` varchar(40) NOT NULL default '' AFTER `ServerId`;
ALTER TABLE `hlmessage` ADD COLUMN `ServerId` INT(10) NOT NULL DEFAULT '1' AFTER `Id`;
ALTER TABLE `hlmessage` ADD COLUMN `ServerName` varchar(40) NOT NULL default '' AFTER `ServerId`;
ALTER TABLE `admins` ADD COLUMN `ServerId` INT(10) NOT NULL DEFAULT '1' AFTER `Id`;
ALTER TABLE `admins` ADD COLUMN `ServerName` varchar(40) NOT NULL default '' AFTER `ServerId`;
ALTER TABLE `ignore_addons` ADD COLUMN `ServerId` INT(10) NOT NULL DEFAULT '1' AFTER `Id`;
ALTER TABLE `ignore_addons` ADD COLUMN `ServerName` varchar(40) NOT NULL default '' AFTER `ServerId`;
ALTER TABLE `ignore_channels` ADD COLUMN `ServerId` INT(10) NOT NULL DEFAULT '1' AFTER `Id`;
ALTER TABLE `ignore_channels` ADD COLUMN `ServerName` varchar(40) NOT NULL default '' AFTER `ServerId`;
ALTER TABLE `ignore_commands` ADD COLUMN `ServerId` INT(10) NOT NULL DEFAULT '1' AFTER `Id`;
ALTER TABLE `ignore_commands` ADD COLUMN `ServerName` varchar(40) NOT NULL default '' AFTER `ServerId`;
ALTER TABLE `ignore_irc_commands` ADD COLUMN `ServerId` INT(10) NOT NULL DEFAULT '1' AFTER `Id`;
ALTER TABLE `ignore_irc_commands` ADD COLUMN `ServerName` varchar(40) NOT NULL default '' AFTER `ServerId`;
ALTER TABLE `ignore_nicks` ADD COLUMN `ServerId` INT(10) NOT NULL DEFAULT '1' AFTER `Id`;
ALTER TABLE `ignore_nicks` ADD COLUMN `ServerName` varchar(40) NOT NULL default '' AFTER `ServerId`;
