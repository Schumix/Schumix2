CREATE TABLE IF NOT EXISTS `maffiagame` (
  `ID` int(4) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Kulcs',
  `Game` int(5) NOT NULL COMMENT 'Hanyadik Jatek',
  `Nev` varchar(25) NOT NULL COMMENT 'A jatekos neve',
  `Survivor` int(3) NOT NULL DEFAULT '1' COMMENT 'A jatekos tul elte-e az adott jatszmat',
  `Job` int(3) NOT NULL COMMENT 'A jatekos szerepe',
  `Active` int(3) NOT NULL DEFAULT '1' COMMENT 'Megy-e meg az adott jatek',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;
