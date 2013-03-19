INSERT INTO `schumix` (`ServerId`, `ServerName`, `FunctionName`, `FunctionStatus`) VALUES ("1", "default", "birthday", "on");

-- huHU
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "WrongSwitch", "Nem megfelelő kapcsoló lett megadva!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "birthday", "Nem vagy beregisztrálva! Kérlek végezd el a regisztrálást hogy tudjad használni a funkciót. Parancs: {0}birthday register <hónap> <nap>");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "birthday/info", "3Születésnap funkció állapota: {0}\n3Születésnap időpontja: 2[Hónap] {0}, 2[Nap] {1}\nNem vagy regisztrálva!\nNincs regisztrálva!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "birthday/change/status", "Születésnapod jelzése bekapcsolva.\nSzületésnapod jelzése kikapcsolva.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "birthday/change/birthday", "Nincs megadva a születési hónap!\nNincs megadva a születési nap!\nSikeresen frissítve lett a születésnapod.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "birthday/register", "Már regisztrálva vagy!\nNincs megadva a születési hónap!\nNincs megadva a születési nap!\nSikeresen hozzáadásra került a születésnapod.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "birthday/remove", "Nem szerepelsz a listán!\nTörölve lett a születésnapod!");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "birthday", "1", "A születésnap funkció beállításai kezelhetők vele.\nBirthDay parancsok: info | change | register | remove");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "birthday/info", "1", "Kiírja a születésnap funkció állapotát.");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "birthday/change", "1", "Megváltoztatható vele a funkció több beállítása.\nParancsok: status | birthday");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "birthday/change/status", "1", "Bekapcsolható vagy kikapcsolható vele a születésnap funkció.\nHasználata: {0}birthday change status <on vagy off>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "birthday/change/birthday", "1", "Frissíthető vele a születésnap dátuma.\nHasználata: {0}birthday change birthday <hónap> <nap>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "birthday/register", "1", "Beregisztrálja a születésnapot.\nHasználata: {0}birthday register <hónap> <nap>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("huHU", "birthday/remove", "1", "Törli a születésnapot.");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "WrongSwitch", "Nem megfelelő kapcsoló lett megadva!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "BirthDay", "Ma {0} születésnapja van.");

-- enUS
INSERT INTO `localized_console_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "WrongSwitch", "Wrong Switch!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "birthday", "You are not registered! Please register to use this function. Command: {0}birthday register <month> <day>");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "birthday/info", "3State of birthday function: {0}\n3Date of birth: 2[Month] {0}, 2[Day] {1}\nYou are not registered!\nYou are not registered!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "birthday/change/status", "Your birthday sign is on.\nYour birthday sign is off.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "birthday/change/birthday", "Month of birth missing!\nDay of birth missing!\nYour birthday is succesfully updated.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "birthday/register", "You are already registered!\nMonth of birth missing!\nDay of birth missing!\nBirthday succesfully added.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("enUS", "birthday/remove", "You are not in the list!\nBirthday deleted!");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "birthday", "1", "With it you can control the paramteres of the birthday function.\nBirthDay commands: info | change | register | remove");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "birthday/info", "1", "Shows the state of the birthday function.");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "birthday/change", "1", "With it you can change the paramteres of the function.\nCommands: status | birthday");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "birthday/change/status", "1", "With it you can turn on/off the birthday function.\nUse: {0}birthday change status <on or off>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "birthday/change/birthday", "1", "With it you can update your birth date.\nUse: {0}birthday change birthday <month> <day>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "birthday/register", "1", "Register the birth date.\nUse: {0}birthday register <month> <day>");
INSERT INTO `localized_command_help` (`Language`, `Command`, `Rank`, `Text`) VALUES ("enUS", "birthday/remove", "1", "Delete the birth date.");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "WrongSwitch", "Wrong Switch!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "BirthDay", "Today is {0}'s birthday.");
