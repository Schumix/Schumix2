-- huHU
UPDATE `localized_command_help` SET Text = "Megmondja az időjárást a megadott városban.\nHasználata: {0}weather <ország> <város>" WHERE Language = 'huHU' AND Command = 'weather';
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "NoCountryName", "Nem adott meg egy országnevet sem!");

-- enUS
UPDATE `localized_command_help` SET Text = "Displays of the canal, what is the weather in the town.\nUse: {0}weather <country> <city>" WHERE Language = 'enUS' AND Command = 'weather';
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "NoCountryName", "No such country name!");
