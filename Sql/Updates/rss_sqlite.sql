-- git
ALTER TABLE gitinfo ADD column `ShortUrl` varchar(5) NOT NULL default 'false' AFTER `Website`;
ALTER TABLE gitinfo ADD column `Colors` varchar(5) NOT NULL default 'true' AFTER `ShortUrl`;

-- hg

-- svn

-- mantisbt

-- wordpress
