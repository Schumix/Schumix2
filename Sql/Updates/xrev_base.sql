-- huHU
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "xrev/github", "Nincs megadva a felhasználó név!\nNincs megadva a project!\nNincs megadva a sha1 kód!\nNincs ilyen kommit!\n3Kommit: {0}\n3Link: {0}\n3Szerző: {0}");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "xrev", "9", "Többféle verziókezelő oldalon tárolt információ olvasható ki az adott projektről.\nXrev parancsok: github");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "xrev/github", "9", "Lekérdezhető vele az adott projekt megadott kommitjának információi.\nHasználata: {0}xrev github <felhasználó> <projekt neve> <sha1 kód>");

-- enUS
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "xrev/github", "Username is not set!\nProject is not set!\nSha1 code is not set!\nThere is no such a commit!\n3Commit: {0}\n3Link: {0}\n3Author: {0}");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "xrev", "9", "Multiple data from revision control sites.\nXrev commands: github");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "xrev/github", "9", "You can query the project's commit information.\nUsage: {0}xrev github <username> <project name> <sha1 code>");
