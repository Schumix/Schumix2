-- huHU
UPDATE `localized_command` SET Text = "12Időjárás otthon!\n5{0} 12időjárása!\n3Nappal: {0}\n3Éjszaka: {0}\nNem szerepel ilyen város a listán!" WHERE Language = 'huHU' AND Command = 'weather';

-- enUS
UPDATE `localized_command` SET Text = "12Local weather!\n5{0} 12weather!\n3Day: {0}\n3Night: {0}\nNo such city in the list!" WHERE Language = 'enUS' AND Command = 'weather';
