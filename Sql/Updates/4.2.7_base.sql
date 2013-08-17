-- huHU
UPDATE `localized_console_command` SET Text = "Nick megváltoztatása erre: {0}\nPontosan úgyan ez a nick név van megadva így nem kerül megváltoztatásra!" WHERE Language = 'huHU' AND Command = 'nick';
UPDATE `localized_command` SET Text = "Nick megváltoztatása erre: {0}\nPontosan úgyan ez a nick név van megadva így nem kerül megváltoztatásra!" WHERE Language = 'huHU' AND Command = 'nick';

-- enUS
UPDATE `localized_console_command` SET Text = "Nick megváltoztatása erre: {0}\nPontosan úgyan ez a nick név van megadva így nem kerül megváltoztatásra!" WHERE Language = 'enUS' AND Command = 'nick';
UPDATE `localized_command` SET Text = "Nick changes to: {0}\nExactly the same nick is given thus it won't be changed." WHERE Language = 'enUS' AND Command = 'nick';
