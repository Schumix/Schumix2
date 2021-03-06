-- huHU
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/info", "3{0} Channel: 2{1}");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/list", "2Lista:3{0}");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/start", "{0} már el van indítva!\n{0} sikeresen el lett indítva.\n{0} nem létezik!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/stop", "{0} már le van állítva!\n{0} sikeresen le lett állítva.\n{0} nem létezik!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/reload", "{0} sikeresen újra lett indítva.\n{0} nem létezik!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/reload/all", "Minden rss újra lett indítva.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/channel/add", "Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!\nMár hozzá van adva a csatorna!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/channel/remove", "Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!\nMár hozzá van adva a csatorna!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/add", "Sikeresen hozzáadva a listához!\nMár szerepel a listán!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/remove", "Sikeresen eltávolítva a listából!\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/change/colors", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/change/shorturl", "Sikeresen módosítva a beállítás.\nNem szerepel a név a listában!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/change/url", "Nem szerepel a név a listában!\nUrl sikeresen módosítva.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "rss/change/website", "Nem szerepel a név a listában!\nOldal neve sikeresen módosítva.");

INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss", "1", "Rss rss-ek kezelése.\nRss parancsai: add | remove | channel | info | list | start | stop | reload");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/channel", "1", "Rss csatornákra való kiírásának kezelése.\nChannel parancsai: add | remove");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/channel/add", "1", "Új csatorna hozzáadása az rss-hez.\nHasználata: {0}rss channel add <rss neve> <csatorna neve>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/channel/remove", "1", "Nem használatos csatorna eltávolítása az rss-ből.\nHasználata: {0}rss channel remove <rss neve> <csatorna neve>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/info", "1", "Kiírja az rss-ek állapotát.");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/list", "1", "Választható rss-ek listája.");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/start", "1", "Új rss betöltése.\nHasználata: {0}rss start <rss neve>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/stop", "1", "Rss leállítása.\nHasználata: {0}rss stop <rss neve>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/reload", "1", "Megadott rss újratöltése.\nrss reload parancsai: all");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/reload/all", "1", "Minden rss újratöltése.\nHasználata: {0}rss reload <rss neve>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/add", "1", "Új rss hozzáadása.\nHasználata: {0}rss add <rss neve> <url> <weboldal (egyedi, a kód alapján kell beállítani)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/remove", "1", "Törli az rss-t.\nHasználata: {0}rss remove <rss neve>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/change", "1", "Rss beállítása módosíthatóak vele.\nChange parancsai: colors | shorturl | url | website");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/change/colors", "1", "Be illetve kikapcsolható vele az rss színezése.\nHasználata: {0}rss change colors <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/change/shorturl", "1", "Be illetve kikapcsolható vele az rss url rövidítése.\nHasználata: {0}rss change shorturl <true vagy false (értelemszerűen)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/change/url", "1", "Megváltoztatható vele az rss url címe.\nHasználata: {0}rss change url <rss neve> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "rss/change/website", "1", "Megváltoztatható vele az rss weboldal címe.\nHasználata: {0}rss change url <rss neve> <weboldal (egyedi, a kód alapján kell beállítani)>");

-- enUS
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/info", "3{0} Channel: 2{1}");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/list", "2List:3{0}");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/start", "{0} already translated!\n{0} successfully started.\n{0} no such!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/stop", "{0} already stopped!\n{0} successfully stopped.\n{0} no such!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/reload", "{0} successfully restarted.\n{0} no such!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/reload/all", "All of Rss is restarted.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/channel/add", "The channel is succesfully added.\This username is not existing!\nThis channel is already added!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/channel/remove", "Channel successfully deleted.\nThis name is not existing!\nThere is no such a channel, so you cant delete!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/add", "Successfully added!\nAlready on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/remove", "Succesfully removed from the list!\nThis name is not on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/change/colors", "Option successfuly modified.\nThis name is not on the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/change/shorturl", "Option succesfully modified.\nThis name is not in the list!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/change/url", "This name is not on the list!\nUrl succesfully modified.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "rss/change/website", "This name is not on the list!\nPage name succesfully changed.");

INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/channel", "1", "RSS feeds on their handling of the announcement.\nChannel commands: add | remove");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/channel/add", "1", "New channel added to the rss.\nUse: {0}rss channel add <rss name> <channel name>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/channel/remove", "1", "Removed from the RSS Channel.\nUse: {0}rss channel remove <rss name> <channel name>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/info", "1", "Prints rss-s condition.");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/list", "1", "Optional list of rss.");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/start", "1", "New RSS feeds.\nUse: {0}rss start <rss name>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/stop", "1", "Rss stop.\nUse: {0}rss stop <rss name>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/reload", "1", "Specify rss reload.\nRss reload command: all");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/reload/all", "1", "All RSS reload.\nUse: {0}rss reload <rss name>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/add", "1", "Add new rss.\nUsage: {0}rss add <rss name> <url> <website (unique)>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/remove", "1", "Deletes the rss.\nUsage: {0}rss remove <rss name>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/change", "1", "You can change the Rss settings with it.\nChange command: colors | shorturl | url | website");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/change/colors", "1", "Rss on/off switch.\nUsage: {0}rss change colors <true or false>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/change/shorturl", "1", "Url shortener on/off switch.\usage: {0}rss change shorturl <true or false>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/change/url", "1", "Rss url changer.\nUsage: {0}rss change url <rss name> <url>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "rss/change/website", "1", "Rss website changer.\nUsage: {0}rss change url <rss name> <unique>");
