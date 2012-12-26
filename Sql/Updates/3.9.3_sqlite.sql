DROP TABLE IF EXISTS `sznap`;

-- ----------------------------
-- Table structure for birthday
-- ----------------------------
CREATE TABLE "birthday" (
Id INTEGER PRIMARY KEY AUTOINCREMENT,
ServerId INTEGER DEFAULT 1,
ServerName VARCHAR(40),
Name VARCHAR(20),
Month INTEGER DEFAULT 1,
Day INTEGER DEFAULT 1,
Enabled VARCHAR(5) DEFAULT 'false'
);

ALTER TABLE channels CHANGE column `Functions` `Functions` varchar(500) NOT NULL default ',greeter:off,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off,gamecommands:off,webtitle:off,randomkick:off,chatterbot:off,nameday:off,birthday:off';
UPDATE `channels` SET `Functions` = concat(Functions, ',birthday:off');
