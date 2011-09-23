ALTER TABLE adminok ADD column `flag` int(10) NOT NULL DEFAULT '0' AFTER `ip`;
ALTER TABLE adminok CHANGE column `ip` `vhost` varchar(100) NOT NULL default '';
