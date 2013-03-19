-- huHU
UPDATE `localized_command` SET Text = "{0}: 3bet�ltve.\n{0}: 8letiltva." WHERE Language = 'huHU' AND Command = 'plugin';
UPDATE `localized_console_command` SET Text = "{0}: bet�ltve.\n{0}: letiltva." WHERE Language = 'huHU' AND Command = 'plugin';
UPDATE `localized_command_help` SET Text = "Lehet�v� teszi egyes adatok kiv�telk�nt val� kezel�s�t.\nIgnore parancsok: irc | command | channel | nick | addon" WHERE Language = 'huHU' AND Command = 'ignore';
UPDATE `localized_console_command_help` SET Text = "Lehet�v� teszi egyes adatok kiv�telk�nt val� kezel�s�t.\nIgnore parancsok: irc | command | channel | nick | addon" WHERE Language = 'huHU' AND Command = 'ignore';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/add", "M�r szerepel az ignore list�n!\nAz addon sikeresen hozz�ad�sra ker�lt.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/remove", "Nem szerepel az ignore list�n!\nAz addon sikeresen el lett t�vol�tva.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/search", "Szerepel az ignore list�n!\nNem szerepel az ignore list�n!");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/add", "M�r szerepel az ignore list�n!\nAz addon sikeresen hozz�ad�sra ker�lt.");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/remove", "Nem szerepel az ignore list�n!\nAz addon sikeresen el lett t�vol�tva.");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/search", "Szerepel az ignore list�n!\nNem szerepel az ignore list�n!");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "ignore/addon", "1", "Lehet�v� teszi addonok kiv�telk�nt val� kezel�s�t.\nAddon parancsok: add | remove | search");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "ignore/addon/add", "1", "Addon hozz�ad�sa a kiv�telekhez.\nHaszn�lata: {0}ignore addon add <parancs>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "ignore/addon/remove", "1", "Addon elt�vol�t�sa a kiv�telek k�z�l.\nHaszn�lata: {0}ignore addon remove <parancs>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "ignore/addon/search", "1", "Addon keres�se a kiv�telekben.\nHaszn�lata: {0}ignore addon search <parancs>");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon", "Lehet�v� teszi addonok kiv�telk�nt val� kezel�s�t.\nAddon parancsok: add | remove | search");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/add", "Addon hozz�ad�sa a kiv�telekhez.\nHaszn�lata: ignore addon add <parancs>");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/remove", "Addon elt�vol�t�sa a kiv�telek k�z�l.\nHaszn�lata: ignore addon remove <parancs>");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/search", "Addon keres�se a kiv�telekben.\nHaszn�lata: ignore addon search <parancs>");

-- enUS
UPDATE `localized_command` SET Text = "{0}: 3loaded.\n{0}: 8ignored." WHERE Language = 'enUS' AND Command = 'plugin';
UPDATE `localized_console_command` SET Text = "{0}: loaded.\n{0}: ignored." WHERE Language = 'enUS' AND Command = 'plugin';
UPDATE `localized_command_help` SET Text = "With it you can treat the data like an expection.\nIgnore commands: irc | command | channel | nick | addon" WHERE Language = 'enUS' AND Command = 'ignore';
UPDATE `localized_console_command_help` SET Text = "With it you can treat the data like an expection.\nIgnore commands: irc | command | channel | nick | addon" WHERE Language = 'enUS' AND Command = 'ignore';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "ignore/addon/add", "Already exist on the ignore list!\nSuccesfuly added.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "ignore/addon/remove", "Not on the ignore list!\nSuccesfuly removed.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "ignore/addon/search", "Already exist on the ignore list!\nNot on the ignore list!");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "ignore/addon/add", "Already exist on the ignore list!\nSuccesfuly added.");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "ignore/addon/remove", "Not on the ignore list!\nSuccesfuly removed.");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "ignore/addon/search", "Already exist on the ignore list!\nNot on the ignore list!");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "ignore/addon", "1", "With it you can treat the addons like an expection.\nAddon commands: add | remove | search");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "ignore/addon/add", "1", "Add addon to the expection list.\Uses: {0}ignore addon add <Command>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "ignore/addon/remove", "1", "Remove addon from the expection list.\Uses: {0}ignore addon remove <Command>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "ignore/addon/search", "1", "Search addon in the expection list.\Uses: {0}ignore addon search <Command>");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("enUS", "ignore/addon", "With it you can treat the addons like an expection.\nAddon commands: add | remove | search");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("enUS", "ignore/addon/add", "Add addon to the expection list.\Uses: ignore addon add <Command>");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("enUS", "ignore/addon/remove", "Remove addon from the expection list.\Uses: ignore addon remove <Command>");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("enUS", "ignore/addon/search", "Search addon in the expection list.\Uses: ignore addon search <Command>");