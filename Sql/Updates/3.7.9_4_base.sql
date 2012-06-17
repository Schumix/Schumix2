-- huHU
UPDATE `localized_command` SET Text = "Csatorna nyelvezete sikeresen meg lett változtatva erre: {0}\nIlyen csatorna nem létezik!\nA csatorna nyelvezete már: {0}" WHERE Language = 'huHU' AND Command = 'channel/language';
UPDATE `localized_console_command` SET Text = "Csatorna nyelvezete sikeresen meg lett változtatva erre: {0}\nIlyen csatorna nem létezik!\nA csatorna nyelvezete már: {0}" WHERE Language = 'huHU' AND Command = 'channel/language';

-- enUS
UPDATE `localized_command` SET Text = "Successfully changed the channel language to: {0}\nNo such channel!\nChannel current language: {0}" WHERE Language = 'enUS' AND Command = 'channel/language';
UPDATE `localized_console_command` SET Text = "Successfully changed the channel language to: {0}\nNo such channel!\nChannel current language: {0}" WHERE Language = 'enUS' AND Command = 'channel/language';