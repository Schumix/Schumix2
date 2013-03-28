-- huHU
UPDATE `localized_command` SET Text = "Helyi idő: {0}:{1}" WHERE Language = 'huHU' AND Command = 'time';
UPDATE `localized_command` SET Text = "Ma {0}. {1}. {2}. {3} napja van." WHERE Language = 'huHU' AND Command = 'date';

-- enUS
UPDATE `localized_command` SET Text = "Local time: {0}:{1}" WHERE Language = 'enUS' AND Command = 'time';
UPDATE `localized_command` SET Text = "Today is {0}. {1}. {2}. and {3}'s day." WHERE Language = 'enUS' AND Command = 'date';
