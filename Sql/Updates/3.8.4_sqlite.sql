CREATE TABLE "channels" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
Functions VARCHAR(500) DEFAULT ',greeter:off,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off,gamecommands:off,webtitle:off,randomkick:off,chatterbot:off,nameday:off',
Channel VARCHAR(20),
Password VARCHAR(30),
Enabled VARCHAR(5),
Error TEXT,
Language VARCHAR(4) DEFAULT 'enUS'
);

INSERT INTO channels (Id, Functions, Channel, Password, Enabled, Error, Language) SELECT channel.Id, channel.Functions, channel.Channel, channel.Password, channel.Enabled, channel.Error, channel.Language FROM channel WHERE NOT EXISTS (SELECT 1 FROM channels WHERE channels.Id = channel.Id);
DROP TABLE IF EXISTS `channel`;

ALTER TABLE `channels` ADD COLUMN `ServerId` INTEGER DEFAULT 1 AFTER `Id`;
ALTER TABLE `channels` ADD COLUMN `ServerName` VARCHAR(40) AFTER `ServerId`;
ALTER TABLE `schumix` ADD COLUMN `ServerId` INTEGER DEFAULT 1 AFTER `Id`;
ALTER TABLE `schumix` ADD COLUMN `ServerName` VARCHAR(40) AFTER `ServerId`;
ALTER TABLE `hlmessage` ADD COLUMN `ServerId` INTEGER DEFAULT 1 AFTER `Id`;
ALTER TABLE `hlmessage` ADD COLUMN `ServerName` VARCHAR(40) AFTER `ServerId`;
ALTER TABLE `admins` ADD COLUMN `ServerId` INTEGER DEFAULT 1 AFTER `Id`;
ALTER TABLE `admins` ADD COLUMN `ServerName` VARCHAR(40) AFTER `ServerId`;
ALTER TABLE `ignore_addons` ADD COLUMN `ServerId` INTEGER DEFAULT 1 AFTER `Id`;
ALTER TABLE `ignore_addons` ADD COLUMN `ServerName` VARCHAR(40) AFTER `ServerId`;
ALTER TABLE `ignore_channels` ADD COLUMN `ServerId` INTEGER DEFAULT 1 AFTER `Id`;
ALTER TABLE `ignore_channels` ADD COLUMN `ServerName` VARCHAR(40) AFTER `ServerId`;
ALTER TABLE `ignore_commands` ADD COLUMN `ServerId` INTEGER DEFAULT 1 AFTER `Id`;
ALTER TABLE `ignore_commands` ADD COLUMN `ServerName` VARCHAR(40) AFTER `ServerId`;
ALTER TABLE `ignore_irc_commands` ADD COLUMN `ServerId` INTEGER DEFAULT 1 AFTER `Id`;
ALTER TABLE `ignore_irc_commands` ADD COLUMN `ServerName` VARCHAR(40) AFTER `ServerId`;
ALTER TABLE `ignore_nicks` ADD COLUMN `ServerId` INTEGER DEFAULT 1 AFTER `Id`;
ALTER TABLE `ignore_nicks` ADD COLUMN `ServerName` VARCHAR(40) AFTER `ServerId`;
