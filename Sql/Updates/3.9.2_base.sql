-- huHU
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "Calendar5", "Az üzenet a holnapi napra lett feljegyezve!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "calendar/nextmessage", "3Üzenet: {0}\n3Dátum: {0}. {1}. {2}. {3}:{4}\nNincs feljegyezve üzenet!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "calendar/private/nextmessage", "3Üzenet: {0}\n3Dátum: {0}. {1}. {2}. {3}:{4}\nNincs feljegyezve üzenet!");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "calendar/nextmessage", "9", "Kiírja azon üzenetet ami a legközelebb van időpontilag.");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "calendar/private/nextmessage", "9", "Kiírja azon üzenetet ami a legközelebb van időpontilag.");
UPDATE `localized_command_help` SET Text = "Eseményeket, üzeneteket lehet feljegyezni vele megadott időpontra.\nParancsok: loop | nick | private | nextmessage\nHasználata:\nÓra és perc: {0}calendar <óó:pp> <üzenet>\nDátum, Óra és perc: {0}calendar <éééé.hh.nn> <óó:pp> <üzenet>" WHERE Language = 'huHU' AND Command = 'calendar';
UPDATE `localized_command_help` SET Text = "Privátban küldi el az üzenetet.\nParancsok: nick | nextmessage\nHasználata:\nÓra és perc: {0}calendar private <óó:pp> <üzenet>\nDátum, Óra és perc: {0}calendar private <éééé.hh.nn> <óó:pp> <üzenet>" WHERE Language = 'huHU' AND Command = 'calendar/private';

-- enUS
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "Calendar5", "The message has been recorded for tomorrow!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "calendar/nextmessage", "3Message: {0}\n3Date: {0}. {1}. {2}. {3}:{4}\nThere is not any recorded message!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "calendar/private/nextmessage", "3Message: {0}\n3Date: {0}. {1}. {2}. {3}:{4}\nThere is not any recorded message!");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "calendar/nextmessage", "9", "Writes out the message that is the closest in time.");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "calendar/private/nextmessage", "9", "Writes out the message that is the closest in time.");
UPDATE `localized_command_help` SET Text = "With it you can save messages,events. Commands: loop | nick | private | nextmessage\nUses:\nHour and min: {0}calendar <hh:mm> <message>\nDate, hour and min: {0}calendar <yyyy.mm.dd> <hh:mm> <message>" WHERE Language = 'enUS' AND Command = 'calendar';
UPDATE `localized_command_help` SET Text = "Send the message in private.\nCommands: nick | nextmessage\nUses:\nHour and min: {0}calendar private <hh:mm> <message>\nDate, Hour and min: {0}calendar private <yyyy.mm.dd> <hh:mm> <message>" WHERE Language = 'enUS' AND Command = 'calendar/private';
