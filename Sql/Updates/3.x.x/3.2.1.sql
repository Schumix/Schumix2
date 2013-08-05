-- huHU
UPDATE `localized_console_command` SET Text = "A név már szerepel az admin listán!\nAdmin hozzáadva: {0}\nJelenlegi jelszó: {0}" WHERE Language = 'huHU' AND Command = 'admin/add';

-- enUS
UPDATE `localized_console_command` SET Text = "The name is already in the admin list!\nAdmin added to the list: {0}\nPassword: {0}" WHERE Language = 'enUS' AND Command = 'admin/add';

-- huHU
UPDATE `localized_command` SET Text = "Jelszó sikeresen meg lett változtatva erre: {0}\nA mostani jelszó nem egyezik, modósitás megtagadva!" WHERE Language = 'huHU' AND Command = 'admin/newpassword';

-- enUS
UPDATE `localized_command` SET Text = "Successfully changed to password to: {0}\nThe current password does not match, modification denied!" WHERE Language = 'enUS' AND Command = 'admin/newpassword';
