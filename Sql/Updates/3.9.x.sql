ALTER TABLE channels CHANGE column `Enabled` `Enabled` varchar(5) NOT NULL default 'false';
ALTER TABLE calendar ADD `Millisecond` int(20) NOT NULL DEFAULT '0';
