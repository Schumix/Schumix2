-- huHU
UPDATE `localized_console_command` SET Command = "cchannel" WHERE Language = 'huHU' AND Command = 'csatorna';
UPDATE `localized_console_command_help` SET Text = "A bot csatornára írását állíthatjuk vele.\nHasználata: cchannel <csatorna neve>", Command = "cchannel" WHERE Language = 'huHU' AND Command = 'csatorna';
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "cserver", "Új szerver amit mostantól lehet állítani a parancsokkal: {0}");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("huHU", "cserver", "A szerverek között válthatunk vele.\nHasználata: cserver <szerver neve>");
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "NoServerName", "Nincs megadva a szerver neve!");
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "ThereIsNoSuchAServerName", "Ilyen szerver név nem létezik!");
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "ChannelAlreadyBeenUsed", "Jelenleg is ez a csatorna van beállítva!");
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "ServerAlreadyBeenUsed", "Jelenleg is ez a szerver van beállítva!");

-- enUS
UPDATE `localized_console_command` SET Command = "cchannel" WHERE Language = 'enUS' AND Command = 'csatorna';
UPDATE `localized_console_command_help` SET Text = "You can select which channel to send the robot.\nUse: cchannel <channel name>", Command = "cchannel" WHERE Language = 'enUS' AND Command = 'csatorna';
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "cserver", "New server for the commands: {0}");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("enUS", "cserver", "Switch between servers.\nUse: cserver <server's name>");
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "NoServerName", "There is no server name!");
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "ThereIsNoSuchAServerName", "There is no such a server name!");
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "ChannelAlreadyBeenUsed", "Channel already been used!");
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "ServerAlreadyBeenUsed", "Server already been used!");
