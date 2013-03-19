-- huHU
UPDATE `localized_console_command` SET Text = "[Betöltés]: Összes plugin betöltése sikeres.\n[Betöltés]: Összes plugin betöltése sikertelen.\nA pluginok már be vannak töltve!" WHERE Language = 'huHU' AND Command = 'plugin/load';
UPDATE `localized_console_command` SET Text = "[Leválasztás]: Összes plugin leválasztása sikeres.\n[Leválasztás]: Összes plugin leválasztása sikertelen.\nA pluginok már le vannak választva!" WHERE Language = 'huHU' AND Command = 'plugin/unload';
UPDATE `localized_console_command` SET Text = "{0}: betöltve.\n{0}: letiltva.\nNincsen betöltve plugin!" WHERE Language = 'huHU' AND Command = 'plugin';
UPDATE `localized_command` SET Text = "2[Betöltés]: Összes plugin betöltése 3sikeres.\n2[Betöltés]: Összes plugin betöltése 5sikertelen.\nA pluginok már be vannak töltve!" WHERE Language = 'huHU' AND Command = 'plugin/load';
UPDATE `localized_command` SET Text = "2[Leválasztás]: Összes plugin leválasztása 3sikeres.\n2[Leválasztás]: Összes plugin leválasztása 5sikertelen.\nA pluginok már le vannak választva!" WHERE Language = 'huHU' AND Command = 'plugin/unload';
UPDATE `localized_command` SET Text = "{0}: 3betöltve.\n{0}: 8letiltva.\nNincsen betöltve plugin!" WHERE Language = 'huHU' AND Command = 'plugin';

-- enUS
UPDATE `localized_console_command` SET Text = "[Load]: All plugins done.\n[Load]: All plugins failed.\nPlugins are already loaded." WHERE Language = 'enUS' AND Command = 'plugin/load';
UPDATE `localized_console_command` SET Text = "[Unload]: All plugins done.\n[Unload]: All plugins failed.\nPlugins are already cutted." WHERE Language = 'enUS' AND Command = 'plugin/unload';
UPDATE `localized_console_command` SET Text = "{0}: loaded.\n{0}: ignored.\nNo plugin loeaded." WHERE Language = 'enUS' AND Command = 'plugin';
UPDATE `localized_command` SET Text = "2[Load]: All plugins 3done.\n2[Load]: All plugins 5failed.\nPlugins are already loaded." WHERE Language = 'enUS' AND Command = 'plugin/load';
UPDATE `localized_command` SET Text = "2[Unload]: All plugins 3done.\n2[Unload]: All plugins 5failed.\nPlugins are already cutted." WHERE Language = 'enUS' AND Command = 'plugin/unload';
UPDATE `localized_command` SET Text = "{0}: 3loaded.\n{0}: 8ignored.\nNo plugin loeaded." WHERE Language = 'enUS' AND Command = 'plugin';
