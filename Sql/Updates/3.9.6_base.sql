-- huHU
UPDATE `localized_console_command` SET Text = "Jelenleg fél operátor.\nJelenleg operátor.\nJelenleg adminisztrátor." WHERE Language = 'huHU' AND Command = 'admin/info';
UPDATE `localized_command` SET Text = "Jelenleg fél operátor.\nJelenleg operátor.\nJelenleg adminisztrátor." WHERE Language = 'huHU' AND Command = 'admin/info';
UPDATE `localized_command_help` SET Text = "Kiírja éppen milyen rangod van.\nVagy:\nKiírja, hogy éppen milyen rangja van.\nHasználata: admin info <admin neve>" WHERE Language = 'huHU' AND Command = 'admin/info';

-- enUS
UPDATE `localized_console_command` SET Text = "You are half operator.\nYou are operator.\nYou are administrator." WHERE Language = 'enUS' AND Command = 'admin/info';
UPDATE `localized_command` SET Text = "You are half operator.\nYou are operator.\nYou are administrator." WHERE Language = 'enUS' AND Command = 'admin/info';
UPDATE `localized_command_help` SET Text = "It show you admin level.\nOr:\nShow the admin's rank.\nUse: admin info <admin name>" WHERE Language = 'enUS' AND Command = 'admin/info';
