-- huHU
UPDATE `localized_command` SET Text = "Jelenleg itt van fent: {0}\nJelenleg nincs fent!\nJelenleg egy csatornán sincs fent!" WHERE Language = 'huHU' AND Command = 'whois';

-- enUS
UPDATE `localized_command` SET Text = "Now online here: {0}\nCurrently offline!\nCurrent is not in any channels!" WHERE Language = 'enUS' AND Command = 'whois';
