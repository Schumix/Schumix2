ALTER TABLE channel CHANGE column `Functions` `Functions` varchar(500) NOT NULL default ',greeter:off,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off,gamecommands:off,webtitle:off,randomkick:off,chatterbot:off,nameday:off';
UPDATE `channel` SET `Functions` = concat(Functions, ',nameday:off');
