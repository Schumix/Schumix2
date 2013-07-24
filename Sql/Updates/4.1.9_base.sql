-- huHU
UPDATE `localized_command` SET Text = "Ma {0}. {1}. {2}. {3} napja van.\nMa {0}. {1}. {2}. van." WHERE Language = 'huHU' AND Command = 'date';

-- enUS
UPDATE `localized_command` SET Text = "Today is {0}. {1}. {2}. and {3}'s day.\nToday is {0}. {1}. {2}." WHERE Language = 'enUS' AND Command = 'date';
