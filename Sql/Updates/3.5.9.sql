REPLACE `schumix` VALUES ('1', 'greeter', 'on');
ALTER TABLE channel CHANGE column `Functions` `Functions` varchar(500) NOT NULL default ',greeter:off,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off,gamecommands:off,webtitle:off,randomkick:off';
-- Aki bemeri válalni hogy minden visszaáll neki alapértelmezésre csak az használja. Mások pedig manuálisan írják át.
-- UPDATE channel SET Functions = ',greeter:off,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off,gamecommands:off,webtitle:off,randomkick:off';
