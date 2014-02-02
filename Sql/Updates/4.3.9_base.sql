-- huHU
-- többit is iderakni
UPDATE `localized_command` SET Text = "[{0}] 3{1} Channel: 2{2}\n2Információ: 3{0}" WHERE Language = 'huHU' AND Command = 'svn/info';
UPDATE `localized_command` SET Text = "[{0}] 3{1} Channel: 2{2}\n2Információ: 3{0}" WHERE Language = 'huHU' AND Command = 'hg/info';
UPDATE `localized_command` SET Text = "[{0}] 3{1}15/7{2} Channel: 2{3}\n2Információ: 3{0}" WHERE Language = 'huHU' AND Command = 'git/info';
UPDATE `localized_command` SET Text = "[{0}] 3{1} Channel: 2{2}\n2Információ: 3{0}" WHERE Language = 'huHU' AND Command = 'mantisbt/info';
UPDATE `localized_command` SET Text = "[{0}] 3{1} Channel: 2{2}\n2Információ: 3{0}" WHERE Language = 'huHU' AND Command = 'wordpress/info';
UPDATE `localized_command` SET Text = "[{0}] 3{1} Channel: 2{2}\n2Információ: 3{0}" WHERE Language = 'huHU' AND Command = 'rss/info';

UPDATE `localized_command` SET Text = "Csatorna sikeresen törölve.\nNem létezik ilyen név!\nNincs ilyen csatorna hozzáadva így nem törölhető!" WHERE Language = 'huHU' AND Command = 'svn/channel/remove';
UPDATE `localized_command` SET Text = "Csatorna sikeresen törölve.\nNem létezik ilyen név!\nNincs ilyen csatorna hozzáadva így nem törölhető!" WHERE Language = 'huHU' AND Command = 'hg/channel/remove';
UPDATE `localized_command` SET Text = "Csatorna sikeresen törölve.\nNem létezik ilyen név!\nNincs ilyen csatorna hozzáadva így nem törölhető!" WHERE Language = 'huHU' AND Command = 'rss/channel/remove';

-- enUS
UPDATE `localized_command` SET Text = "[{0}] 3{1} Channel: 2{2}\n2Information: 3{0}" WHERE Language = 'enUS' AND Command = 'svn/info';
UPDATE `localized_command` SET Text = "[{0}] 3{1} Channel: 2{2}\n2Information: 3{0}" WHERE Language = 'enUS' AND Command = 'hg/info';
UPDATE `localized_command` SET Text = "[{0}] 3{1}15/7{2} Channel: 2{3}\n2Information: 3{0}" WHERE Language = 'enUS' AND Command = 'git/info';
UPDATE `localized_command` SET Text = "[{0}] 3{1} Channel: 2{2}\n2Information: 3{0}" WHERE Language = 'enUS' AND Command = 'mantisbt/info';
UPDATE `localized_command` SET Text = "[{0}] 3{1} Channel: 2{2}\n2Information: 3{0}" WHERE Language = 'enUS' AND Command = 'wordpress/info';
UPDATE `localized_command` SET Text = "[{0}] 3{1} Channel: 2{2}\n2Information: 3{0}" WHERE Language = 'enUS' AND Command = 'rss/info';
