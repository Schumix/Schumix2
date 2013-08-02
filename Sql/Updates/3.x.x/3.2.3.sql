INSERT INTO `schumix` VALUES ('15', 'gamecommands', 'on');
INSERT INTO `schumix` VALUES ('16', 'webtitle', 'on');
INSERT INTO `schumix` VALUES ('17', 'randomkick', 'off');
ALTER TABLE channel CHANGE column `Functions` `Functions` varchar(500) NOT NULL default ',koszones:off,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off,gamecommands:off,webtitle:off,randomkick:off';
UPDATE `channel` SET `Functions` = concat(Functions, ',gamecommands:off,webtitle:off,randomkick:off');

DELETE FROM `localized_console_command` WHERE Command = 'left';
DELETE FROM `localized_console_command_help` WHERE Command = 'left';
DELETE FROM `localized_command` WHERE Command = 'left';
DELETE FROM `localized_command_help` WHERE Command = 'left';

INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'leave', 'Lelépés erről a csatornáról: {0}');
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "leave", "Part of this channel: {0}");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ('huHU', 'leave', 'Lelépés megadot csatonáról.\nHasználata: {0}leave <csatona>');

-- huHU
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'leave', 'Lelépés erről a csatornáról: {0}');

-- enUS
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'leave', 'Part of this channel: {0}');

-- huHU
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ('huHU', 'leave', '0', 'Lelépés megadot csatonáról.\nHasználata: {0}leave <csatona>');
