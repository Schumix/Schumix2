-- huHU
UPDATE `localized_command` SET Text = "3Születésnap funkció állapota: {0}\n3Születésnap időpontja:2 {0}. {1}. {2}.\nNem vagy regisztrálva!\nNincs regisztrálva!" WHERE Language = 'huHU' AND Command = 'birthday/info';
UPDATE `localized_command` SET Text = "Nincs megadva a születési hónap!\nNincs megadva a születési nap!\nSikeresen frissítve lett a születésnapod.\nNincs megadva a születési év!" WHERE Language = 'huHU' AND Command = 'birthday/change/birthday';
UPDATE `localized_command` SET Text = "Már regisztrálva vagy!\nNincs megadva a születési hónap!\nNincs megadva a születési nap!\nSikeresen hozzáadásra került a születésnapod.\nNincs megadva a születési év!" WHERE Language = 'huHU' AND Command = 'birthday/register';
UPDATE `localized_command_help` SET Text = "Frissíthető vele a születésnap dátuma.\nHasználata: {0}birthday change birthday <év> <hónap> <nap>" WHERE Language = 'huHU' AND Command = 'birthday/change/birthday';
UPDATE `localized_command_help` SET Text = "Beregisztrálja a születésnapot.\nHasználata: {0}birthday register <év> <hónap> <nap>" WHERE Language = 'huHU' AND Command = 'birthday/register';
UPDATE `localized_warning` SET Text = "Ma {0} születésnapja van. Most töltötte be a {1}. életévét." WHERE Language = 'huHU' AND Command = 'BirthDay';

-- enUS
UPDATE `localized_command` SET Text = "3State of birthday function: {0}\n3Date of birth:2 {0}. {1}. {2}.\nYou are not registered!\nYou are not registered!" WHERE Language = 'enUS' AND Command = 'birthday/info';
UPDATE `localized_command` SET Text = "Month of birth missing!\nDay of birth missing!\nYour birthday is succesfully updated.\nYear of birth missing!" WHERE Language = 'enUS' AND Command = 'birthday/change/birthday';
UPDATE `localized_command` SET Text = "You are already registered!\nMonth of birth missing!\nDay of birth missing!\nBirthday succesfully added.\nYear of birth missing!" WHERE Language = 'enUS' AND Command = 'birthday/register';
UPDATE `localized_command_help` SET Text = "With it you can update your birth date.\nUse: {0}birthday change birthday <year> <month> <day>" WHERE Language = 'enUS' AND Command = 'birthday/change/birthday';
UPDATE `localized_command_help` SET Text = "Register the birth date.\nUse: {0}birthday register <year> <month> <day>" WHERE Language = 'enUS' AND Command = 'birthday/register';
UPDATE `localized_warning` SET Text = "Today is {0}'s birthday. {0} is now {1} years old." WHERE Language = 'enUS' AND Command = 'BirthDay';
