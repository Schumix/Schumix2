-- huHU

-- git
UPDATE `localized_command` SET Text = "Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!\nMár hozzá van adva a csatorna!" WHERE Language = 'huHU' AND Command = 'git/channel/add';
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

-- hg
UPDATE `localized_command` SET Text = "Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!\nMár hozzá van adva a csatorna!" WHERE Language = 'huHU' AND Command = 'hg/channel/add';
UPDATE `localized_command` SET Text = "Csatorna sikeresen törölve.\nNem létezik ilyen név!\nNincs ilyen csatorna hozzáadva így nem törölhető!" WHERE Language = 'huHU' AND Command = 'hg/channel/remove';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "hg/add", "Sikeresen hozzáadva a listához!\nMár szerepel a listán!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "hg/remove", "Sikeresen eltávolítva a listából!\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "hg/change/colors", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "hg/change/shorturl", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "hg/change/url", "Nem szerepel a név a listában!\nUrl sikeresen módosítva.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "hg/change/website", "Nem szerepel a név a listában!\nOldal neve sikeresen módosítva.");

UPDATE `localized_command_help` SET Text = "Hg rss-ek kezelése.\nHg parancsai: add | remove | channel | info | list | start | stop | reload" WHERE Language = 'huHU' AND Command = 'hg';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "hg/add", "1", "Új rss hozzáadása.\nHasználata: {0}hg add <rss neve> <url> <weboldal (egyedi, a kód alapján kell beállítani)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "hg/remove", "1", "Törli az rss-t.\nHasználata: {0}hg remove <rss neve>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "hg/change", "1", "Rss beállítása módosíthatóak vele.\nChange parancsai: colors | shorturl | url | website");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "hg/change/colors", "1", "Be illetve kikapcsolható vele az rss színezése.\nHasználata: {0}hg change colors <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "hg/change/shorturl", "1", "Be illetve kikapcsolható vele az rss url rövidítése.\nHasználata: {0}hg change shorturl <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "hg/change/url", "1", "Megváltoztatható vele az rss url címe.\nHasználata: {0}hg change url <rss neve> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "hg/change/website", "1", "Megváltoztatható vele az rss weboldal címe.\nHasználata: {0}hg change url <rss neve> <weboldal (egyedi, a kód alapján kell beállítani)>");

-- svn
UPDATE `localized_command` SET Text = "Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!\nMár hozzá van adva a csatorna!" WHERE Language = 'huHU' AND Command = 'svn/channel/add';
UPDATE `localized_command` SET Text = "Csatorna sikeresen törölve.\nNem létezik ilyen név!\nNincs ilyen csatorna hozzáadva így nem törölhető!" WHERE Language = 'huHU' AND Command = 'svn/channel/remove';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "svn/add", "Sikeresen hozzáadva a listához!\nMár szerepel a listán!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "svn/remove", "Sikeresen eltávolítva a listából!\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "svn/change/colors", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "svn/change/shorturl", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "svn/change/url", "Nem szerepel a név a listában!\nUrl sikeresen módosítva.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "svn/change/website", "Nem szerepel a név a listában!\nOldal neve sikeresen módosítva.");

UPDATE `localized_command_help` SET Text = "Svn rss-ek kezelése.\nSvn parancsai: add | remove | channel | info | list | start | stop | reload" WHERE Language = 'huHU' AND Command = 'svn';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "svn/add", "1", "Új rss hozzáadása.\nHasználata: {0}svn add <rss neve> <url> <weboldal (egyedi, a kód alapján kell beállítani)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "svn/remove", "1", "Törli az rss-t.\nHasználata: {0}svn remove <rss neve>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "svn/change", "1", "Rss beállítása módosíthatóak vele.\nChange parancsai: colors | shorturl | url | website");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "svn/change/colors", "1", "Be illetve kikapcsolható vele az rss színezése.\nHasználata: {0}svn change colors <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "svn/change/shorturl", "1", "Be illetve kikapcsolható vele az rss url rövidítése.\nHasználata: {0}svn change shorturl <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "svn/change/url", "1", "Megváltoztatható vele az rss url címe.\nHasználata: {0}svn change url <rss neve> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "svn/change/website", "1", "Megváltoztatható vele az rss weboldal címe.\nHasználata: {0}svn change url <rss neve> <weboldal (egyedi, a kód alapján kell beállítani)>");

-- mantisbt
UPDATE `localized_command` SET Text = "Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!\nMár hozzá van adva a csatorna!" WHERE Language = 'huHU' AND Command = 'mantisbt/channel/add';
UPDATE `localized_command` SET Text = "Csatorna sikeresen törölve.\nNem létezik ilyen név!\nNincs ilyen csatorna hozzáadva így nem törölhető!" WHERE Language = 'huHU' AND Command = 'mantisbt/channel/remove';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "mantisbt/add", "Sikeresen hozzáadva a listához!\nMár szerepel a listán!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "mantisbt/remove", "Sikeresen eltávolítva a listából!\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "mantisbt/change/colors", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "mantisbt/change/shorturl", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "mantisbt/change/url", "Nem szerepel a név a listában!\nUrl sikeresen módosítva.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "mantisbt/change/website", "Nem szerepel a név a listában!\nOldal neve sikeresen módosítva.");

UPDATE `localized_command_help` SET Text = "MantisBT rss-ek kezelése.\nMantisBT parancsai: add | remove | channel | info | list | start | stop | reload" WHERE Language = 'huHU' AND Command = 'mantisbt';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "mantisbt/add", "1", "Új rss hozzáadása.\nHasználata: {0}mantisbt add <rss neve> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "mantisbt/remove", "1", "Törli az rss-t.\nHasználata: {0}mantisbt remove <rss neve>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "mantisbt/change", "1", "Rss beállítása módosíthatóak vele.\nChange parancsai: colors | shorturl | url");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "mantisbt/change/colors", "1", "Be illetve kikapcsolható vele az rss színezése.\nHasználata: {0}mantisbt change colors <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "mantisbt/change/shorturl", "1", "Be illetve kikapcsolható vele az rss url rövidítése.\nHasználata: {0}mantisbt change shorturl <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "mantisbt/change/url", "1", "Megváltoztatható vele az rss url címe.\nHasználata: {0}mantisbt change url <rss neve> <url>");

-- wordpress
UPDATE `localized_command` SET Text = "Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!\nMár hozzá van adva a csatorna!" WHERE Language = 'huHU' AND Command = 'wordpress/channel/add';
UPDATE `localized_command` SET Text = "Csatorna sikeresen törölve.\nNem létezik ilyen név!\nNincs ilyen csatorna hozzáadva így nem törölhető!" WHERE Language = 'huHU' AND Command = 'wordpress/channel/remove';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "wordpress/add", "Sikeresen hozzáadva a listához!\nMár szerepel a listán!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "wordpress/remove", "Sikeresen eltávolítva a listából!\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "wordpress/change/colors", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "wordpress/change/shorturl", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "wordpress/change/url", "Nem szerepel a név a listában!\nUrl sikeresen módosítva.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "wordpress/change/website", "Nem szerepel a név a listában!\nOldal neve sikeresen módosítva.");

UPDATE `localized_command_help` SET Text = "WordPress rss-ek kezelése.\nWordPress parancsai: add | remove | channel | info | list | start | stop | reload" WHERE Language = 'huHU' AND Command = 'wordpress';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "wordpress/add", "1", "Új rss hozzáadása.\nHasználata: {0}wordpress add <rss neve> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "wordpress/remove", "1", "Törli az rss-t.\nHasználata: {0}wordpress remove <rss neve>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "wordpress/change", "1", "Rss beállítása módosíthatóak vele.\nChange parancsai: colors | shorturl | url");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "wordpress/change/colors", "1", "Be illetve kikapcsolható vele az rss színezése.\nHasználata: {0}wordpress change colors <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "wordpress/change/shorturl", "1", "Be illetve kikapcsolható vele az rss url rövidítése.\nHasználata: {0}wordpress change shorturl <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "wordpress/change/url", "1", "Megváltoztatható vele az rss url címe.\nHasználata: {0}wordpress change url <rss neve> <url>");

INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "UrlMissing", "Nincs megadva az url!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "WebsiteNameMissing", "Nincs megadva az oldal neve!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "StatusIsMissing", "Nincs megadva az állapot!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "ValueIsNotTrueOrFalse", "Nem true vagy false érték lett megadva!");

-- enUS

-- git
UPDATE `localized_command` SET Text = "The channel is succesfully added.\This username is not existing!\nThis channel is already added!" WHERE Language = 'enUS' AND Command = 'git/channel/add';
UPDATE `localized_command` SET Text = "Channel successfully deleted.\nThis name is not existing!\nThere is no such a channel, so you cant delete!" WHERE Language = 'enUS' AND Command = 'git/channel/remove';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "git/add", "Successfully added!\nAlready on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "git/remove", "Succesfully removed from the list!\nThis name is not on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "git/change/colors", "Option successfuly modified.\nThis name is not on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "git/change/shorturl", "Option succesfully modified.\nThis name is not in the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "git/change/url", "This name is not on the list!\nUrl succesfully modified.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "git/change/website", "This name is not on the list!\nPage name succesfully changed.");
     
UPDATE `localized_command_help` SET Text = "Rss git 's management.\nGit command: add | remove | channel | info | list | start | stop | reload" WHERE Language = 'enUS' AND Command = 'git';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/add", "1", "Add new rss.\nUsage: {0}git add <rss name> <type> <url> <website (unique)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/remove", "1", "Deletes the rss.\nUsage: {0}git remove <rss name> <type>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/change", "1", "You can change the Rss settings with it.\nChange command: colors | shorturl | url | website");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/change/colors", "1", "Rss on/off switch.\nUsage: {0}git change colors <true or false>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/change/shorturl", "1", "Url shortener on/off switch.\usage: {0}git change shorturl <true or false>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/change/url", "1", "Rss url changer.\nUsage: {0}git change url <rss name> <type> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "git/change/website", "1", "Rss website changer.\nUsage: {0}git change url <rss name> <type> <unique>");

-- hg
UPDATE `localized_command` SET Text = "The channel is succesfully added.\This username is not existing!\nThis channel is already added!" WHERE Language = 'enUS' AND Command = 'hg/channel/add';
UPDATE `localized_command` SET Text = "Channel successfully deleted.\nThis name is not existing!\nThere is no such a channel, so you cant delete!" WHERE Language = 'enUS' AND Command = 'hg/channel/remove';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "hg/add", "Successfully added!\nAlready on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "hg/remove", "Succesfully removed from the list!\nThis name is not on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "hg/change/colors", "Option successfuly modified.\nThis name is not on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "hg/change/shorturl", "Option succesfully modified.\nThis name is not in the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "hg/change/url", "This name is not on the list!\nUrl succesfully modified.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "hg/change/website", "This name is not on the list!\nPage name succesfully changed.");
     
UPDATE `localized_command_help` SET Text = "Rss hg 's management.\nHg command: add | remove | channel | info | list | start | stop | reload" WHERE Language = 'enUS' AND Command = 'hg';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "hg/add", "1", "Add new rss.\nUsage: {0}hg add <rss name> <url> <website (unique)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "hg/remove", "1", "Deletes the rss.\nUsage: {0}hg remove <rss name>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "hg/change", "1", "You can change the Rss settings with it.\nChange command: colors | shorturl | url | website");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "hg/change/colors", "1", "Rss on/off switch.\nUsage: {0}hg change colors <true or false>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "hg/change/shorturl", "1", "Url shortener on/off switch.\usage: {0}hg change shorturl <true or false>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "hg/change/url", "1", "Rss url changer.\nUsage: {0}hg change url <rss name> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "hg/change/website", "1", "Rss website changer.\nUsage: {0}hg change url <rss name> <unique>");

-- svn
UPDATE `localized_command` SET Text = "The channel is succesfully added.\This username is not existing!\nThis channel is already added!" WHERE Language = 'enUS' AND Command = 'svn/channel/add';
UPDATE `localized_command` SET Text = "Channel successfully deleted.\nThis name is not existing!\nThere is no such a channel, so you cant delete!" WHERE Language = 'enUS' AND Command = 'svn/channel/remove';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "svn/add", "Successfully added!\nAlready on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "svn/remove", "Succesfully removed from the list!\nThis name is not on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "svn/change/colors", "Option successfuly modified.\nThis name is not on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "svn/change/shorturl", "Option succesfully modified.\nThis name is not in the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "svn/change/url", "This name is not on the list!\nUrl succesfully modified.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "svn/change/website", "This name is not on the list!\nPage name succesfully changed.");
     
UPDATE `localized_command_help` SET Text = "Rss svn 's management.\nSvn command: add | remove | channel | info | list | start | stop | reload" WHERE Language = 'enUS' AND Command = 'svn';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "svn/add", "1", "Add new rss.\nUsage: {0}svn add <rss name> <url> <website (unique)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "svn/remove", "1", "Deletes the rss.\nUsage: {0}svn remove <rss name>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "svn/change", "1", "You can change the Rss settings with it.\nChange command: colors | shorturl | url | website");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "svn/change/colors", "1", "Rss on/off switch.\nUsage: {0}svn change colors <true or false>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "svn/change/shorturl", "1", "Url shortener on/off switch.\usage: {0}svn change shorturl <true or false>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "svn/change/url", "1", "Rss url changer.\nUsage: {0}svn change url <rss name> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "svn/change/website", "1", "Rss website changer.\nUsage: {0}svn change url <rss name> <unique>");

-- mantisbt
UPDATE `localized_command` SET Text = "The channel is succesfully added.\This username is not existing!\nThis channel is already added!" WHERE Language = 'enUS' AND Command = 'mantisbt/channel/add';
UPDATE `localized_command` SET Text = "Channel successfully deleted.\nThis name is not existing!\nThere is no such a channel, so you cant delete!" WHERE Language = 'enUS' AND Command = 'mantisbt/channel/remove';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "mantisbt/add", "Successfully added!\nAlready on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "mantisbt/remove", "Succesfully removed from the list!\nThis name is not on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "mantisbt/change/colors", "Option successfuly modified.\nThis name is not on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "mantisbt/change/shorturl", "Option succesfully modified.\nThis name is not in the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "mantisbt/change/url", "This name is not on the list!\nUrl succesfully modified.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "mantisbt/change/website", "This name is not on the list!\nPage name succesfully changed.");
     
UPDATE `localized_command_help` SET Text = "Rss mantisbt 's management.\nMantisBT command: add | remove | channel | info | list | start | stop | reload" WHERE Language = 'enUS' AND Command = 'mantisbt';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "mantisbt/add", "1", "Add new rss.\nUsage: {0}mantisbt add <rss name> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "mantisbt/remove", "1", "Deletes the rss.\nUsage: {0}mantisbt remove <rss name>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "mantisbt/change", "1", "You can change the Rss settings with it.\nChange command: colors | shorturl | url");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "mantisbt/change/colors", "1", "Rss on/off switch.\nUsage: {0}mantisbt change colors <true or false>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "mantisbt/change/shorturl", "1", "Url shortener on/off switch.\usage: {0}mantisbt change shorturl <true or false>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "mantisbt/change/url", "1", "Rss url changer.\nUsage: {0}mantisbt change url <rss name> <url>");

-- wordpress
UPDATE `localized_command` SET Text = "The channel is succesfully added.\This username is not existing!\nThis channel is already added!" WHERE Language = 'enUS' AND Command = 'wordpress/channel/add';
UPDATE `localized_command` SET Text = "Channel successfully deleted.\nThis name is not existing!\nThere is no such a channel, so you cant delete!" WHERE Language = 'enUS' AND Command = 'wordpress/channel/remove';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "wordpress/add", "Successfully added!\nAlready on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "wordpress/remove", "Succesfully removed from the list!\nThis name is not on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "wordpress/change/colors", "Option successfuly modified.\nThis name is not on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "wordpress/change/shorturl", "Option succesfully modified.\nThis name is not in the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "wordpress/change/url", "This name is not on the list!\nUrl succesfully modified.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "wordpress/change/website", "This name is not on the list!\nPage name succesfully changed.");
     
UPDATE `localized_command_help` SET Text = "Rss wordpress 's management.\nWordPress command: add | remove | channel | info | list | start | stop | reload" WHERE Language = 'enUS' AND Command = 'wordpress';
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "wordpress/add", "1", "Add new rss.\nUsage: {0}wordpress add <rss name> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "wordpress/remove", "1", "Deletes the rss.\nUsage: {0}wordpress remove <rss name>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "wordpress/change", "1", "You can change the Rss settings with it.\nChange command: colors | shorturl | url");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "wordpress/change/colors", "1", "Rss on/off switch.\nUsage: {0}wordpress change colors <true or false>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "wordpress/change/shorturl", "1", "Url shortener on/off switch.\usage: {0}wordpress change shorturl <true or false>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "wordpress/change/url", "1", "Rss url changer.\nUsage: {0}mantisbt change url <rss name> <url>");

INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "UrlMissing", "Url missing!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "WebsiteNameMissing", "Website name missing!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "StatusIsMissing", "Status is missing!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "ValueIsNotTrueOrFalse", "Value is not true or false!");
