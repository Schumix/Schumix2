-- huHU

-- git
/*UPDATE `localized_command` SET Text = "Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!\nMár hozzá van adva a csatorna!" WHERE Language = 'huHU' AND Command = 'git/channel/add';
UPDATE `localized_command` SET Text = "Csatorna sikeresen törölve.\nNem létezik ilyen név!\nNincs ilyen csatorna hozzáadva így nem törölhető!" WHERE Language = 'huHU' AND Command = 'git/channel/remove';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "git/add", "Sikeresen hozzáadva a listához!\nMár szerepel a listán!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "git/remove", "Sikeresen eltávolítva a listából!\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "git/change/colors", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "git/change/shorturl", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "git/change/url", "Nem szerepel a név a listában!\nUrl sikeresen módosítva.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "git/change/website", "Nem szerepel a név a listában!\nOldal neve sikeresen módosítva.");

UPDATE `localized_command_help` SET Text = "Git rss-ek kezelése.\nGit parancsai: add | remove | channel | info | list | start | stop | reload" WHERE Language = 'huHU' AND Command = 'git';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "git/add", "1", "Új rss hozzáadása.\nHasználata: {0}git add <rss neve> <tipus> <url> <weboldal (egyedi, a kód alapján kell beállítani)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "git/remove", "1", "Törli az rss-t.\nHasználata: {0}git remove <rss neve> <tipus>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "git/change", "1", "Rss beállítása módosíthatóak vele.\nChange parancsai: colors | shorturl | url | website");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "git/change/colors", "1", "Be illetve kikapcsolható vele az rss színezése.\nHasználata: {0}git change colors <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "git/change/shorturl", "1", "Be illetve kikapcsolható vele az rss url rövidítése.\nHasználata: {0}git change shorturl <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "git/change/url", "1", "Megváltoztatható vele az rss url címe.\nHasználata: {0}git change url <rss neve> <tipus> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "git/change/website", "1", "Megváltoztatható vele az rss weboldal címe.\nHasználata: {0}git change url <rss neve> <tipus> <weboldal (egyedi, a kód alapján kell beállítani)>");

INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "UrlMissing", "Nincs megadva az url!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "WebsiteNameMissing", "Nincs megadva az oldal neve!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "StatusIsMissing", "Nincs megadva az állapot!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "ValueIsNotTrueOrFalse", "Nem true vagy false érték lett megadva!");*/

-- enUS

-- git
/*UPDATE `localized_command` SET Text = "The channel is succesfully added.\nThis name is not exist!\nThis channel is already added!" WHERE Language = 'enUS' AND Command = 'git/channel/add';
UPDATE `localized_command` SET Text = "Csatorna sikeresen törölve.\nNem létezik ilyen név!\nNincs ilyen csatorna hozzáadva így nem törölhető!" WHERE Language = 'enUS' AND Command = 'git/channel/remove';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "git/add", "Sikeresen hozzáadva a listához!\nMár szerepel a listán!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "git/remove", "Sikeresen eltávolítva a listából!\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "git/change/colors", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "git/change/shorturl", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "git/change/url", "Nem szerepel a név a listában!\nUrl sikeresen módosítva.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "git/change/website", "Nem szerepel a név a listában!\nOldal neve sikeresen módosítva.");

UPDATE `localized_command_help` SET Text = "Git rss-ek kezelése.\nGit parancsai: add | remove | channel | info | list | start | stop | reload" WHERE Language = 'enUS' AND Command = 'git';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/add", "1", "Új rss hozzáadása.\nHasználata: {0}git add <rss neve> <tipus> <url> <weboldal (egyedi, a kód alapján kell beállítani)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/remove", "1", "Törli az rss-t.\nHasználata: {0}git remove <rss neve> <tipus>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/change", "1", "Rss beállítása módosíthatóak vele.\nChange parancsai: colors | shorturl | url | website");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/change/colors", "1", "Be illetve kikapcsolható vele az rss színezése.\nHasználata: {0}git change colors <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/change/shorturl", "1", "Be illetve kikapcsolható vele az rss url rövidítése.\nHasználata: {0}git change shorturl <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/change/url", "1", "Megváltoztatható vele az rss url címe.\nHasználata: {0}git change url <rss neve> <tipus> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/change/website", "1", "Megváltoztatható vele az rss weboldal címe.\nHasználata: {0}git change url <rss neve> <tipus> <weboldal (egyedi, a kód alapján kell beállítani)>");*/

/*INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "UrlMissing", "Url missing!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "WebsiteNameMissing", "Website name missing!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "StatusIsMissing", "Status is missing!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "ValueIsNotTrueOrFalse", "Value is not true or false!");*/
