UPDATE `localized_command` SET Text = "A programban nincs ilyen rész!\nValahol hiba történt az újratöltésben!\n{0} újra lett indítva." WHERE Language = "huHU" AND Command = "reload";
UPDATE `localized_command` SET Text = "The program does not contains that part!\nThere is an error in the reload!\n{0} reloaded." WHERE Language = "enUS" AND Command = "reload";
UPDATE `localized_console_command` SET Text = "A programban nincs ilyen rész!\nValahol hiba történt az újratöltésben!\n{0} újra lett indítva." WHERE Language = "huHU" AND Command = "reload";
UPDATE `localized_console_command` SET Text = "The program does not contains that part!\nThere is an error in the reload!\n{0} reloaded." WHERE Language = "enUS" AND Command = "reload";
UPDATE `localized_command_help` SET Text = "Naponként ismétli meg a megadott időben az üzenetet.\nParancsok: nick | private | remove\nHasználata:\nÓra és perc: {0}calendar loop <óó:pp> <üzenet>" WHERE Language = "huHU" AND Command = "calendar/loop";
UPDATE `localized_command_help` SET Text = "Megadott személynek jegyzi fel. \nParancsok: remove\nHasználata:\nÓra és perc: {0}calendar loop nick <név> <óó:pp> <üzenet>" WHERE Language = "huHU" AND Command = "calendar/loop/nick";
UPDATE `localized_command_help` SET Text = "Privátban küldi el az üzenetet.\nParancsok: nick | remove\nHasználata:\nÓra és perc: {0}calendar loop private <óó:pp> <üzenet>" WHERE Language = "huHU" AND Command = "calendar/loop/private";
UPDATE `localized_command_help` SET Text = "Megadott személynek jegyzi fel és privátban küldi el neki.\nParancsok: remove\nHasználata:\nÓra és perc: {0}calendar loop private nick <név> <óó:pp> <üzenet>" WHERE Language = "huHU" AND Command = "calendar/loop/private/nick";
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "calendar/loop", "0", "Reapet the message daily.\nCommands: nick | private | remove\nHour and min: {0}calendar loop <hh:mm> <message>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "calendar/loop/nick", "0", "Save for person.\nCommands: remove\nUses:\nHour and min: {0}calendar loop nick <name> <hh:mm> <message>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "calendar/loop/private", "0", "Sends the message in private.\nCommands: nick | remove\nUses:\nHour and min: {0}calendar loop private <hh:mm> <message>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "calendar/loop/private/nick", "0", "Save for person and send in private.\nCommands: remove\nUses:\nHour and min: {0}calendar loop private nick <name> <hh:mm> <message>");

INSERT INTO `schumix` VALUES ("21", "nameday", "on");

-- huHU
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "NoIgnoreCommand", "Nem tehető kivételek közé a parancs!");
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "NoIgnoreCommand", "Nem tehető kivételek közé a parancs!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "Calendar1", "Már jegyeztél fel üzenetet!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "Calendar2", "Feljegyzett üzenet {0} számára!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "Calendar3", "Nem szerepel a listán!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "Calendar4", "Sikeresen törlésre került.");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "ErrorYear", "Nagyobb az év száma mint a megengedett!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "ErrorMonth", "Nagyobb az hónap száma mint a megengedett!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "ErrorDay", "Nagyobb az nap száma mint a megengedett!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "ErrorHour", "Nagyobb az óra száma mint a megengedett!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "ErrorMinute", "Nagyobb az perc száma mint a megengedett!");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "calendar/loop/remove", "0", "Eltávolítja az ismétlődő üzenetet.\nHasználata: {0}calendar loop remove <óó:pp>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "calendar/loop/nick/remove", "0", "Eltávolítja az ismétlődő üzenetet.\nHasználata: {0}calendar loop nick remove <név> <óó:pp>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "calendar/loop/private/remove", "0", "Eltávolítja az ismétlődő üzenetet.\nHasználata: {0}calendar loop private remove <óó:pp>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "calendar/loop/private/nick/remove", "0", "Eltávolítja az ismétlődő üzenetet.\nHasználata: {0}calendar loop private nick remove <név> <óó:pp>");

-- enUS
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "NoIgnoreCommand", "You can't put this command to the inceptions!");
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "NoIgnoreCommand", "You can't put this command to the inceptions!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "Calendar1", "You already set a note");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "Calendar2", "Note for {0}");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "Calendar3", "This isn't in the list!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "Calendar4", "Successfully deleted.");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "ErrorYear", "Greater 'year count' than the aloved");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "ErrorMonth", "Greater 'month count' than the aloved");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "ErrorDay", "Greater 'day count' than the aloved");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "ErrorHour", "Greater 'hour count' than the aloved");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "ErrorMinute", "Greater 'minute count' than the aloved");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "calendar/loop/remove", "0", "Remove the repeatable messages.\nUses: {0}calendar loop remove <hh:mm>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "calendar/loop/nick/remove", "0", "Remove the repeatable messages.\nUses: {0}calendar loop nick remove <name> <hh:mm>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "calendar/loop/private/remove", "0", "Remove the repeatable messages.\nUses: {0}calendar loop private remove <hh:mm>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "calendar/loop/private/nick/remove", "0", "Remove the repeatable messages.\nUses: {0}calendar loop private nick remove <name> <hh:mm>");
