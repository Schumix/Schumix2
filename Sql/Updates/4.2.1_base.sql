DELETE FROM `localized_console_command` WHERE Command = 'left';
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "leave", "Part of this channel: {0}");
