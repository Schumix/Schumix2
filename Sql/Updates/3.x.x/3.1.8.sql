DELETE FROM `localized_command` WHERE Command = 'admin/password';

-- huHU
UPDATE `localized_command` SET Text = "Jelszó sikeresen meg lett változtatva erre: {0}\nA mostani jelszó nem egyezik, modósitás megtagadva" WHERE Language = 'huHU' AND Command = 'admin/newpassword';

-- enUS
UPDATE `localized_command` SET Text = "Successfully changed to password to: {0}\nThe current password does not match, modification denied." WHERE Language = 'enUS' AND Command = 'admin/newpassword';
