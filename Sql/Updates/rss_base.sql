-- huHU
UPDATE `localized_command` SET Text = "Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!\nMár hozzá van adva a csatorna!" WHERE Language = 'huHU' AND Command = 'git/channel/add';
UPDATE `localized_command` SET Text = "Csatorna sikeresen törölve.\nNem létezik ilyen név!\nNincs ilyen csatorna hozzáadva így nem törölhető!" WHERE Language = 'huHU' AND Command = 'git/channel/remove';
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "git/add", "Sikeresen hozzáadva a listához!\nMár szerepel a listán!");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ("huHU", "git/remove", "Sikeresen eltávolítva a listából!\nNem szerepel a név a listában!");

INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "UrlMissing", "Nincs megadva az url!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("huHU", "WebsiteNameMissing", "Nincs megadva az oldal neve!");

-- enUS

INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "UrlMissing", "Url missing!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ("enUS", "WebsiteNameMissing", "Website name missing!");
