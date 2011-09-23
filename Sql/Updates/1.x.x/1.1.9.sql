DROP TABLE IF EXISTS `uptime`;
CREATE TABLE `uptime` (
  `id` int(100) unsigned NOT NULL auto_increment,
  `datum` text NOT NULL,
  `uptime` text NOT NULL,
  `memory` text NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=1 DEFAULT CHARSET=latin1;
