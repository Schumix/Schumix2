-- huHU
UPDATE `localized_warning` SET Text = "Hibás az év (száma, formátuma)!" WHERE Language = "huHU" AND Command = "ErrorYear";
UPDATE `localized_warning` SET Text = "Hibás a hónap (száma, formátuma)!" WHERE Language = "huHU" AND Command = "ErrorMonth";
UPDATE `localized_warning` SET Text = "Hibás a nap (száma, formátuma)!" WHERE Language = "huHU" AND Command = "ErrorDay";
UPDATE `localized_warning` SET Text = "Hibás az óra (száma, formátuma)!" WHERE Language = "huHU" AND Command = "ErrorHour";
UPDATE `localized_warning` SET Text = "Hibás a perc (száma, formátuma)!" WHERE Language = "huHU" AND Command = "ErrorMinute";
UPDATE `localized_command_help` SET Text = "Megállapítja hogy a szám prímszám-e. Törtszámmal nem 100%, hogy müködik!\nHasználata: {0}prime <szám>" WHERE Language = "huHU" AND Command = "prime";
UPDATE `localized_command_help` SET Text = "Több funkciós számológép.\nHasználata: {0}calc <műveleti számsor>" WHERE Language = "huHU" AND Command = "calc";
UPDATE `localized_command` SET Text = "{0}: 3betöltve." WHERE Language = "huHU" AND Command = "plugin";

INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("huHU", "plugin", "Kiírja milyen pluginok vannak betöltve.\nPlugin parancsok: load | unload");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("huHU", "plugin/load", "Betölt minden plugint.");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("huHU", "plugin/unload", "Eltávolít minden plugint.");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "plugin/load", "[Betöltés]: Összes plugin betöltése sikeres.\n[Betöltés]: Összes plugin betöltése sikertelen.");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "plugin/unload", "[Leválasztás]: Összes plugin leválasztása sikeres.\n[Leválasztás]: Összes plugin leválasztása sikertelen.");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "plugin", "{0}: betöltve.");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "NameDay", "Mai napon {0} névnapja van.");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "GaveExpiredDateTime", "Lejárt dátumot/órát adtál meg!");

-- enUS
UPDATE `localized_warning` SET Text = "Wrong year (number, format)!" WHERE Language = "enUS" AND Command = "ErrorYear";
UPDATE `localized_warning` SET Text = "Wrong month (number, format)!" WHERE Language = "enUS" AND Command = "ErrorMonth";
UPDATE `localized_warning` SET Text = "Wrong day (number, format)!" WHERE Language = "enUS" AND Command = "ErrorDay";
UPDATE `localized_warning` SET Text = "Wrong hour (number, format)!" WHERE Language = "enUS" AND Command = "ErrorHour";
UPDATE `localized_warning` SET Text = "Wrong minute (number, format)!" WHERE Language = "enUS" AND Command = "ErrorMinute";
UPDATE `localized_command_help` SET Text = "It states that the number is prime or not. Not sure that working with fractions!\nUse: {0}prime <number>" WHERE Language = "enUS" AND Command = "prime";
UPDATE `localized_command_help` SET Text = "Multi-function calculator.\nUse: {0}calc <operation sequence>" WHERE Language = "enUS" AND Command = "calc";

INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("enUS", "plugin", "Shows what plugins are loaded.\nPlugin commands: load | unload");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("enUS", "plugin/load", "Loads all the plugin.");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("enUS", "plugin/unload", "Remove all plugin.");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "plugin/load", "[Load]: All plugins done.\n[Load]: All plugins failed.");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "plugin/unload", "[Unload]: All plugins done.\n[Unload]: All plugins failed.");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "plugin", "{0}: loaded.");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "NameDay", "Today is {0}'s name day.");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "GaveExpiredDateTime", "You have gave expired date/time!");
