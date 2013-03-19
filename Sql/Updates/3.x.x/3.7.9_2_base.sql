-- huHU
UPDATE `localized_command` SET Text = "{0}: 3betöltve.\n{0}: 8letiltva." WHERE Language = 'huHU' AND Command = 'plugin';
UPDATE `localized_console_command` SET Text = "{0}: betöltve.\n{0}: letiltva." WHERE Language = 'huHU' AND Command = 'plugin';
UPDATE `localized_command_help` SET Text = "Lehetõvé teszi egyes adatok kivételként való kezelését.\nIgnore parancsok: irc | command | channel | nick | addon" WHERE Language = 'huHU' AND Command = 'ignore';
UPDATE `localized_console_command_help` SET Text = "Lehetõvé teszi egyes adatok kivételként való kezelését.\nIgnore parancsok: irc | command | channel | nick | addon" WHERE Language = 'huHU' AND Command = 'ignore';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/add", "Már szerepel az ignore listán!\nAz addon sikeresen hozzáadásra került.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/remove", "Nem szerepel az ignore listán!\nAz addon sikeresen el lett távolítva.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/search", "Szerepel az ignore listán!\nNem szerepel az ignore listán!");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/add", "Már szerepel az ignore listán!\nAz addon sikeresen hozzáadásra került.");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/remove", "Nem szerepel az ignore listán!\nAz addon sikeresen el lett távolítva.");
INSERT INTO `localized_console_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/search", "Szerepel az ignore listán!\nNem szerepel az ignore listán!");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "ignore/addon", "1", "Lehetõvé teszi addonok kivételként való kezelését.\nAddon parancsok: add | remove | search");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "ignore/addon/add", "1", "Addon hozzáadása a kivételekhez.\nHasználata: {0}ignore addon add <parancs>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "ignore/addon/remove", "1", "Addon eltávolítása a kivételek közül.\nHasználata: {0}ignore addon remove <parancs>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "ignore/addon/search", "1", "Addon keresése a kivételekben.\nHasználata: {0}ignore addon search <parancs>");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon", "Lehetõvé teszi addonok kivételként való kezelését.\nAddon parancsok: add | remove | search");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/add", "Addon hozzáadása a kivételekhez.\nHasználata: ignore addon add <parancs>");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/remove", "Addon eltávolítása a kivételek közül.\nHasználata: ignore addon remove <parancs>");
INSERT INTO `localized_console_command_help` (`Language`, `Command`, `Text`) VALUES ("huHU", "ignore/addon/search", "Addon keresése a kivételekben.\nHasználata: ignore addon search <parancs>");

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