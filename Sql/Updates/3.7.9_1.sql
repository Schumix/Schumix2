CREATE TABLE IF NOT EXISTS `maffiagame` (
  `ID` int(4) unsigned NOT NULL AUTO_INCREMENT COMMENT 'key',
  `Game` int(5) NOT NULL COMMENT 'th',
  `Name` varchar(25) NOT NULL COMMENT 'Player name',
  `Survivor` int(3) NOT NULL DEFAULT '1' COMMENT 'The player has survived?',
  `Job` int(3) NOT NULL COMMENT 'What the players job?',
  `Active` int(3) NOT NULL DEFAULT '1' COMMENT 'Is active game?',
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;
