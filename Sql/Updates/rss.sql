-- git
ALTER TABLE gitinfo ADD column `ShortUrl` varchar(5) NOT NULL default 'false' AFTER `Website`;
ALTER TABLE gitinfo ADD column `Colors` varchar(5) NOT NULL default 'true' AFTER `ShortUrl`;

-- hg
ALTER TABLE hginfo ADD column `ShortUrl` varchar(5) NOT NULL default 'false' AFTER `Website`;
ALTER TABLE hginfo ADD column `Colors` varchar(5) NOT NULL default 'true' AFTER `ShortUrl`;

-- svn
ALTER TABLE svninfo ADD column `ShortUrl` varchar(5) NOT NULL default 'false' AFTER `Website`;
ALTER TABLE svninfo ADD column `Colors` varchar(5) NOT NULL default 'true' AFTER `ShortUrl`;

-- mantisbt
ALTER TABLE mantisbt ADD column `ShortUrl` varchar(5) NOT NULL default 'false' AFTER `Link`;
ALTER TABLE mantisbt ADD column `Colors` varchar(5) NOT NULL default 'true' AFTER `ShortUrl`;

-- wordpress
ALTER TABLE wordpressinfo ADD column `ShortUrl` varchar(5) NOT NULL default 'false' AFTER `Link`;
ALTER TABLE wordpressinfo ADD column `Colors` varchar(5) NOT NULL default 'true' AFTER `ShortUrl`;
