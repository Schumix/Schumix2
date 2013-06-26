-- huHU
UPDATE `localized_command` SET Text = "Nem vagy beregisztrálva! Kérlek végezd el a regisztrálást hogy tudjad használni a funkciót. Parancs: {0}birthday register <éééé.hh.nn>" WHERE Language = 'huHU' AND Command = 'birthday';
UPDATE `localized_command_help` SET Text = "Frissíthető vele a születésnap dátuma.\nHasználata: {0}birthday change birthday <éééé.hh.nn>" WHERE Language = 'huHU' AND Command = 'birthday/change/birthday';
UPDATE `localized_command_help` SET Text = "Beregisztrálja a születésnapot.\nHasználata: {0}birthday register <éééé.hh.nn>" WHERE Language = 'huHU' AND Command = 'birthday/register';

-- enUS
UPDATE `localized_command` SET Text = "You are not registered! Please register to use this function. Command: {0}birthday register <yyyy.mm.dd>" WHERE Language = 'enUS' AND Command = 'birthday';
UPDATE `localized_command_help` SET Text = "With it you can update your birth date.\nUse: {0}birthday change birthday <yyyy.mm.dd>" WHERE Language = 'enUS' AND Command = 'birthday/change/birthday';
UPDATE `localized_command_help` SET Text = "Register the birth date.\nUse: {0}birthday register <yyyy.mm.dd>" WHERE Language = 'enUS' AND Command = 'birthday/register';
