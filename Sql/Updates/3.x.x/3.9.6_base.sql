-- huHU
UPDATE `localized_console_command` SET Text = "Jelenleg fél operátor.\nJelenleg operátor.\nJelenleg adminisztrátor." WHERE Language = 'huHU' AND Command = 'admin/info';
UPDATE `localized_console_command` SET Text = "Aktív: {0}\nAktív: Nincs információ.\nInaktív: {0}\nInaktív: Nincs információ.\nRejtett: {0}\nRejtett: Nincs információ." WHERE Language = 'huHU' AND Command = 'channel/info';
UPDATE `localized_command` SET Text = "Jelenleg fél operátor.\nJelenleg operátor.\nJelenleg adminisztrátor." WHERE Language = 'huHU' AND Command = 'admin/info';
UPDATE `localized_command` SET Text = "3Aktív: {0}\n3Aktív: Nincs információ.\n3Inaktív: {0}\n3Inaktív: Nincs információ.\n3Rejtett: {0}\n3Rejtett: Nincs információ." WHERE Language = 'huHU' AND Command = 'channel/info';
UPDATE `localized_command_help` SET Text = "Kiírja éppen milyen rangod van.\nVagy:\nKiírja, hogy éppen milyen rangja van.\nHasználata: admin info <admin neve>" WHERE Language = 'huHU' AND Command = 'admin/info';

-- enUS
UPDATE `localized_console_command` SET Text = "You are half operator.\nYou are operator.\nYou are administrator." WHERE Language = 'enUS' AND Command = 'admin/info';
UPDATE `localized_console_command` SET Text = "Active: {0}\nActive: Nothing information.\nInactive: {0}\nInactive: Nothing information.\nHidden: {0}\nHidden: Nothing information." WHERE Language = 'enUS' AND Command = 'channel/info';
UPDATE `localized_command` SET Text = "You are half operator.\nYou are operator.\nYou are administrator." WHERE Language = 'enUS' AND Command = 'admin/info';
UPDATE `localized_command` SET Text = "3Active: {0}\n3Active: Nothing information.\n3Inactive: {0}\n3Inactive: Nothing information.\n3Hidden: {0}\n3Hidden: Nothing information." WHERE Language = 'enUS' AND Command = 'channel/info';
UPDATE `localized_command_help` SET Text = "It show you admin level.\nOr:\nShow the admin's rank.\nUse: admin info <admin name>" WHERE Language = 'enUS' AND Command = 'admin/info';
