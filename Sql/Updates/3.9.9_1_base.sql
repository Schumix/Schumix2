-- huHU
UPDATE `localized_command` SET Text = "Saját rangomat nem változtathatom meg!\nTúl sok rang változtatást adtál meg! Maximum 4-et lehet!\n+ vagy - jel megadása kötelező!\nCsak angol abc betűivel lehet rangot megadni!\nValamelyik betű nem rang! Kérlek keresd meg melyik a hibás!\nTúl sok név lett megadva! Maximum 4-et lehet!\nTöbb rangot adtál meg mint ahány nevet!\nTöbb nevet adtál meg mint ahány rangot!\nSzóközöket adtál meg név helyett!" WHERE Language = 'huHU' AND Command = 'mode';

-- enUS
UPDATE `localized_command` SET Text = "You can't change your own rank!\nYou changed too much rank! It can be max 4!\n+ or - symbol requied!\nYou can use characters from the english abc!\nOne of the characters is not a rank!\nYou changed too many name! It can be max 4!\nYou type more rank than name!\nYou type more name than rank!\nYou added a space instead of a name!" WHERE Language = 'enUS' AND Command = 'mode';
