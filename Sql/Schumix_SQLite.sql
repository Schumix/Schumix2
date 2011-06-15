/* Schumix_SQLite.sql */

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for "admins"
-- ----------------------------
DROP TABLE IF EXISTS "admins";
CREATE TABLE "admins" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(20),
Password VARCHAR(40),
Vhost VARCHAR(50),
Flag BIGINT
);

-- ----------------------------
-- Records of admins
-- ----------------------------

-- ----------------------------
-- Table structure for "banned"
-- ----------------------------
DROP TABLE IF EXISTS "banned";
CREATE TABLE "banned" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(50),
Channel VARCHAR(20),
Reason TEXT,
Year INTEGER DEFAULT 0,
Month INTEGER DEFAULT 0,
Day INTEGER DEFAULT 0,
Hour INTEGER DEFAULT 0,
Minute INTEGER DEFAULT 0
);

-- ----------------------------
-- Records of banned
-- ----------------------------

-- ----------------------------
-- Table structure for "channel"
-- ----------------------------
DROP TABLE IF EXISTS "channel";
CREATE TABLE "channel" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Functions VARCHAR(255)    DEFAULT ',koszones:off,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off',
Channel VARCHAR(20),
Password VARCHAR(30),
Enabled VARCHAR(5),
Error TEXT,
Language VARCHAR(4)    DEFAULT 'enUS'
);

-- ----------------------------
-- Records of channel
-- ----------------------------
INSERT INTO "channel" VALUES (1, ',koszones:on,log:on,rejoin:on,commands:on,autohl:off,autokick:off,automode:off,antiflood:off,message:off,compiler:off', '#schumix2', null, null, null, 'huHU');

-- ----------------------------
-- Table structure for "gitinfo"
-- ----------------------------
DROP TABLE IF EXISTS "gitinfo";
CREATE TABLE "gitinfo" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(20),
Type VARCHAR(20),
Link VARCHAR(255),
Website VARCHAR(30),
Channel TEXT
);

-- ----------------------------
-- Records of gitinfo
-- ----------------------------
-- INSERT INTO `gitinfo` VALUES (1, 'Schumix2', 'master', 'http://github.com/megax/Schumix2/commits/master.atom', 'github', '#hun_bot,#schumix'); Példa a használatra
INSERT INTO "gitinfo" VALUES (1, 'Schumix2', 'master', 'http://github.com/megax/Schumix2/commits/master.atom', 'github', '#schumix');

-- ----------------------------
-- Table structure for "hginfo"
-- ----------------------------
DROP TABLE IF EXISTS "hginfo";
CREATE TABLE "hginfo" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(20),
Link VARCHAR(255),
Website VARCHAR(30),
Channel TEXT
);

-- ----------------------------
-- Records of hginfo
-- ----------------------------
-- INSERT INTO "svninfo" VALUES (1, 'Sandshroud', 'http://www.assembla.com/spaces/Sandshroud/stream.rss', 'assembla', '#hun_bot,#schumix'); Példa a használatra

-- ----------------------------
-- Table structure for "hlmessage"
-- ----------------------------
DROP TABLE IF EXISTS "hlmessage";
CREATE TABLE "hlmessage" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(20),
Info TEXT,
Enabled VARCHAR(3)
);

-- ----------------------------
-- Records of hlmessage
-- ----------------------------

-- ----------------------------
-- Table structure for "irc_commands"
-- ----------------------------
DROP TABLE IF EXISTS "irc_commands";
CREATE TABLE "irc_commands" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Command VARCHAR(30),
Message TEXT
);

-- ----------------------------
-- Records of irc_commands
-- ----------------------------
INSERT INTO "irc_commands" VALUES (1, 'rang', 'Rang hasznlata: /mode <channel> <rang> <nv>');
INSERT INTO "irc_commands" VALUES (2, 'rang1', 'Rang mentse: /chanserv <rang (sop, aop, hop, vop)> <channel> ADD <nv>');
INSERT INTO "irc_commands" VALUES (3, 'nick', 'Nick csere hasznlata: /nick <j nv>');
INSERT INTO "irc_commands" VALUES (4, 'kick', 'Kick hasznlata: /kick <channel> <nv> (<oka> nem felttlen kell)');
INSERT INTO "irc_commands" VALUES (5, 'owner', 'Ownermod hasznlata: /msg chanserv SET <channel> ownermode on');

-- ----------------------------
-- Table structure for "kicklist"
-- ----------------------------
DROP TABLE IF EXISTS "kicklist";
CREATE TABLE "kicklist" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(20),
Channel VARCHAR(20),
Reason TEXT
);

-- ----------------------------
-- Records of kicklist
-- ----------------------------

-- ----------------------------
-- Table structure for "localized_command"
-- ----------------------------
DROP TABLE IF EXISTS "localized_command";
CREATE TABLE `localized_command` (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4)    DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- ----------------------------
-- Records of localized_command
-- ----------------------------
INSERT INTO "localized_command" VALUES (1, 'huHU', 'schumix2/sys', X'0203335665727A69F33A0F0F200331307B307D0F5C6E020333506C6174666F726D3A0F0F207B307D5C6E0203334F535665727A69F33A0F0F207B307D5C6E02033350726F6772616D6E79656C763A0F0F2063235C6E0203334D656D6F726961206861737A6EE16C61743A0F0F0335207B307D0F204D425C6E0203334D656D6F726961206861737A6EE16C61743A0F0F0338207B307D0F204D425C6E0203334D656D6F726961206861737A6EE16C61743A0F0F0333207B307D0F204D425C6E020333557074696D653A0F0F207B307D');
INSERT INTO "localized_command" VALUES (2, 'huHU', 'schumix2/help', X'020333506172616E63736F6B3A0F0F20026E69636B207C207379730F5C6E020333506172616E63736F6B3A0F0F200267686F7374207C206E69636B207C207379730F5C6E020333506172616E63736F6B3A0F0F200267686F7374207C206E69636B207C20737973207C20636C65616E0F5C6E020333506172616E63736F6B3A0F0F20027379730F');
INSERT INTO "localized_command" VALUES (3, 'huHU', 'schumix2/ghost', 'Ghost paranccsal elsdleges nick visszaszerzse.');
INSERT INTO "localized_command" VALUES (4, 'huHU', 'schumix2/nick', 'Nv megvltoztatsa erre: {0}');
INSERT INTO "localized_command" VALUES (5, 'huHU', 'schumix2/nick/identify', 'Azonost jelsz kldse a kiszolglonak.');
INSERT INTO "localized_command" VALUES (6, 'huHU', 'schumix2/clean', 'Lefoglalt memria felszabadtsra kerl.');
INSERT INTO "localized_command" VALUES (7, 'huHU', 'help', 'Ha a parancs mg rod a megadott parancs nevt vagy a nevet s alparancst informcit ad a hasznlatrl.\nF parancsom: {0}xbot');
INSERT INTO "localized_command" VALUES (8, 'huHU', 'xbot', X'0203335665727A69F33A0F0F200331307B307D0F5C6E020333506172616E63736F6B3A0F0F20027B307D0F5C6E0250726F6772616D6D65642062793A0F20033343736162610F');
INSERT INTO "localized_command" VALUES (9, 'huHU', 'info', 'Programozm: Csaba, Jackneill.');
INSERT INTO "localized_command" VALUES (10, 'huHU', 'time', 'Helyi id: {0}:0{1}\nHelyi id: {0}:{1}');
INSERT INTO "localized_command" VALUES (11, 'huHU', 'date', 'Ma {0}. 0{1}. 0{2}. {3} napja van.\nMa {0}. 0{1}. {2}. {3} napja van.\nMa {0}. {1}. 0{2}. {3} napja van.\nMa {0}. {1}. {2}. {3} napja van.');
INSERT INTO "localized_command" VALUES (12, 'huHU', 'roll', 'Szzalkos arnya {0}%');
INSERT INTO "localized_command" VALUES (13, 'huHU', 'whois', 'Jelenleg itt van fent: {0}');
INSERT INTO "localized_command" VALUES (14, 'huHU', 'warning', 'Keresnek tged itt: {0}');
INSERT INTO "localized_command" VALUES (15, 'huHU', 'google', X'0203325469746C653A0F204E696E6373205469746C652E5C6E0203324C696E6B3A0F204E696E6373204C696E6B2E5C6E0203325469746C653A0F0F207B307D5C6E0203324C696E6B3A0F0F2003337B307D0F');
INSERT INTO "localized_command" VALUES (16, 'huHU', 'translate', 'Nincs frdtott szveg.');
INSERT INTO "localized_command" VALUES (17, 'huHU', 'prime', 'Nem csak szmot tartalmaz!\n{0} nem primszm.\n{0} primszm.');
INSERT INTO "localized_command" VALUES (18, 'huHU', 'admin/access', 'Hozzfrs engedlyezve.\nHozzfrs megtagadva!');
INSERT INTO "localized_command" VALUES (19, 'huHU', 'admin/password', 'Jelsz sikereset meg lett vltoztatva erre: {0}\nA mostani jelsz nem egyezik, modsits megtagadva');
INSERT INTO "localized_command" VALUES (20, 'huHU', 'admin/info', 'Jelenleg Fl Opertor vagy.\nJelenleg Opertor vagy.\nJelenleg Adminisztrtor vagy.');
INSERT INTO "localized_command" VALUES (21, 'huHU', 'admin/list', X'02033241646D696E6F6B3A0F0F20027B307D0F');
INSERT INTO "localized_command" VALUES (22, 'huHU', 'admin/add', 'A nv mr szerepel az admin listn!\nAdmin hozzadva: {0}\nMostantl Schumix adminja vagy. A te mostani jelszavad: {0}\nHa megszeretnd vltoztatni hasznld az {0}admin newpassword parancsot. Hasznlata: {0}admin newpassword <rgi> <j>\nAdmin nick lestse: {0}admin access <jelsz>');
INSERT INTO "localized_command" VALUES (23, 'huHU', 'admin/remove', 'Ilyen nv nem ltezik!\nAdmin trlve: {0}');
INSERT INTO "localized_command" VALUES (24, 'huHU', 'admin/rank', 'Rang sikeresen mdostva.\nHibs rang!');
INSERT INTO "localized_command" VALUES (25, 'huHU', 'admin', X'02033346E96C204F706572E1746F7220706172616E63736F6B210F0F5C6E020333506172616E63736F6B3A0F0F20027B307D0F5C6E0203334F706572E1746F7220706172616E63736F6B210F0F5C6E020333506172616E63736F6B3A0F0F20027B307D0F5C6E02033341646D696E69737A7472E1746F7220706172616E63736F6B210F0F5C6E020333506172616E63736F6B3A0F0F20027B307D0F');
INSERT INTO "localized_command" VALUES (26, 'huHU', 'colors', X'03317465737A74310F2003327465737A74320F2003337465737A74330F2003347465737A74340F2003357465737A74350F2003367465737A74360F2003377465737A74370F2003387465737A74380F2003397465737A74390F200331307465737A7431300F200331317465737A7431310F200331327465737A7431320F200331337465737A7431330F200331347465737A7431340F200331357465737A7431350F');
INSERT INTO "localized_command" VALUES (27, 'huHU', 'nick', 'Nick megvltoztatsa erre: {0}');
INSERT INTO "localized_command" VALUES (28, 'huHU', 'join', 'Kapcsolds ehhez a csatornhoz: {0}');
INSERT INTO "localized_command" VALUES (29, 'huHU', 'left', 'Lelps errl a csatornrl: {0}');
INSERT INTO "localized_command" VALUES (30, 'huHU', 'function/info', X'02033242656B617063736F6C76613A0F0F20027B307D0F5C6E0203324B696B617063736F6C76613A0F0F20027B307D0F');
INSERT INTO "localized_command" VALUES (31, 'huHU', 'function/all/info', X'02033242656B617063736F6C76613A0F0F20027B307D0F5C6E0203324B696B617063736F6C76613A0F0F20027B307D0F');
INSERT INTO "localized_command" VALUES (32, 'huHU', 'function/all', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO "localized_command" VALUES (33, 'huHU', 'function/channel/info', X'02033242656B617063736F6C76613A0F0F20027B307D0F5C6E0203324B696B617063736F6C76613A0F0F20027B307D0F');
INSERT INTO "localized_command" VALUES (34, 'huHU', 'function/channel', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO "localized_command" VALUES (35, 'huHU', 'function/update', 'Sikeresen frissitve {0} csatornn a funkcik.');
INSERT INTO "localized_command" VALUES (36, 'huHU', 'function/update/all', '"Sikeresen frissitve minden csatornn a funkcik.');
INSERT INTO "localized_command" VALUES (37, 'huHU', 'function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO "localized_command" VALUES (38, 'huHU', 'channel', X'020333506172616E63736F6B3A0F0F20616464207C2072656D6F7665207C20696E666F207C20757064617465207C206C616E6775616765');
INSERT INTO "localized_command" VALUES (39, 'huHU', 'channel/add', 'A nv mr szerepel a csatorna listn!\nCsatorna hozzadva: {0}');
INSERT INTO "localized_command" VALUES (40, 'huHU', 'channel/remove', 'A mester csatorna nem trlhet!\nIlyen csatorna nem ltezik!\nCsatorna eltvoltva: {0}');
INSERT INTO "localized_command" VALUES (41, 'huHU', 'channel/update', 'A csatorna informcik frissitsre kerltek.');
INSERT INTO "localized_command" VALUES (42, 'huHU', 'channel/info', X'020333416B74ED763A0F0F207B307D5C6E020333416B74ED763A0F0F204E696E637320696E666F726DE16369F32E5C6E020333496E616B74ED763A0F0F207B307D5C6E020333496E616B74ED763A0F0F204E696E637320696E666F726DE16369F32E');
INSERT INTO "localized_command" VALUES (43, 'huHU', 'channel/language', 'Csatorna nyelvezete sikeresen meg lett vltoztatva erre: {0}');
INSERT INTO "localized_command" VALUES (44, 'huHU', 'plugin/load', X'0203325B426574F66C74E9735D3A0F0F2002D673737A657320706C7567696E20626574F66C74E973650F20033373696B657265732E0F5C6E0203325B426574F66C74E9735D3A0F0F2002D673737A657320706C7567696E20626574F66C74E973650F20033573696B657274656C656E2E0F');
INSERT INTO "localized_command" VALUES (45, 'huHU', 'plugin/unload', X'0203325B4C6576E16C61737A74E1735D3A0F0F2002D673737A657320706C7567696E206C6576E16C61737A74E173610F20033373696B657265732E0F5C6E0203325B4C6576E16C61737A74E1735D3A0F0F2002D673737A657320706C7567696E206C6576E16C61737A74E173610F20033573696B657274656C656E2E0F');
INSERT INTO "localized_command" VALUES (46, 'huHU', 'plugin', X'027B307D3A0F2003336C6F616465642E0F');
INSERT INTO "localized_command" VALUES (47, 'huHU', 'quit', 'Viszlt :(\n{0} lelltott paranccsal.');
INSERT INTO "localized_command" VALUES (48, 'huHU', 'svn/info', X'0333027B307D0F0F20024368616E6E656C3A0F200203327B317D0F0F');
INSERT INTO "localized_command" VALUES (49, 'huHU', 'svn/list', X'0203324C697374613A0F0F0203337B307D0F0F');
INSERT INTO "localized_command" VALUES (50, 'huHU', 'svn/channel/add', 'Csatorna sikeresen hozzadva.\nNem ltezik ilyen nv!');
INSERT INTO "localized_command" VALUES (51, 'huHU', 'svn/channel/remove', 'Csatorna sikeresen trlve.\nNem ltezik ilyen nv!');
INSERT INTO "localized_command" VALUES (52, 'huHU', 'hg/info', X'0333027B307D0F0F20024368616E6E656C3A0F200203327B317D0F0F');
INSERT INTO "localized_command" VALUES (53, 'huHU', 'hg/list', X'0203324C697374613A0F0F0203337B307D0F0F');
INSERT INTO "localized_command" VALUES (54, 'huHU', 'hg/channel/add', 'Csatorna sikeresen hozzadva.\nNem ltezik ilyen nv!');
INSERT INTO "localized_command" VALUES (55, 'huHU', 'hg/channel/remove', 'Csatorna sikeresen trlve.\nNem ltezik ilyen nv!');
INSERT INTO "localized_command" VALUES (56, 'huHU', 'git/info', X'0333027B307D0F0F2003377B317D0F20024368616E6E656C3A0F200203327B327D0F0F');
INSERT INTO "localized_command" VALUES (57, 'huHU', 'git/list', X'0203324C697374613A0F0F0203337B307D0F0F');
INSERT INTO "localized_command" VALUES (58, 'huHU', 'git/channel/add', 'Csatorna sikeresen hozzadva.\nNem ltezik ilyen nv!');
INSERT INTO "localized_command" VALUES (59, 'huHU', 'git/channel/remove', 'Csatorna sikeresen trlve.\nNem ltezik ilyen nv!');
INSERT INTO "localized_command" VALUES (60, 'huHU', 'compiler/memory', 'Jelenleg tl sok memrit fogyaszt a bot ezrt ezen funkci nem elrhet!');
INSERT INTO "localized_command" VALUES (61, 'huHU', 'compiler/warning', 'A kdban olyan rszek vannak melyek veszlyeztetik a programot. Ezrt lellt a fordts!');
INSERT INTO "localized_command" VALUES (62, 'huHU', 'compiler', 'Nincs megadva a f fv! (Schumix)\nNincs megadva a f class!\nA kimeneti szveg tl hossz ezrt nem kerlt kiirsra!\nA kd sikeresen lefordult csak nincs kimen zenet!\nHtramaradt mg {0} kiirs!');
INSERT INTO "localized_command" VALUES (63, 'huHU', 'compiler/code', 'Hibk: {0}');
INSERT INTO "localized_command" VALUES (64, 'huHU', 'handlekick', '{0} kirgta a kvetkez felhasznlt: {1} oka: {2}');
INSERT INTO "localized_command" VALUES (65, 'huHU', 'ban', 'Helytelen dtum formtum!');
INSERT INTO "localized_command" VALUES (66, 'huHU', 'autofunction', X'020333506172616E63736F6B3A0F0F2002686C6D6573736167650F5C6E020333506172616E63736F6B3A0F0F20026B69636B207C206D6F6465207C20686C6D6573736167650F');
INSERT INTO "localized_command" VALUES (67, 'huHU', 'autofunction/hlmessage/info', X'0203334CE974657AF5206E69636B656B3A0F0F20027B307D0F');
INSERT INTO "localized_command" VALUES (68, 'huHU', 'autofunction/hlmessage/update', 'Az adatbzis sikeresen frissitsre kerlt.');
INSERT INTO "localized_command" VALUES (69, 'huHU', 'autofunction/hlmessage/function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO "localized_command" VALUES (70, 'huHU', 'autofunction/hlmessage', 'Az zenet mdostsra kerlt.');
INSERT INTO "localized_command" VALUES (71, 'huHU', 'autofunction/kick/add', 'A nv mr szerepel a kick listn!\nKick listhoz a nv hozzadva: {0}');
INSERT INTO "localized_command" VALUES (72, 'huHU', 'autofunction/kick/remove', 'Ilyen nv nem ltezik!\nKick listbl a nv eltvltsra kerlt: {0}');
INSERT INTO "localized_command" VALUES (73, 'huHU', 'autofunction/kick/info', 'Kick listn lvk: {0}');
INSERT INTO "localized_command" VALUES (74, 'huHU', 'autofunction/kick/channel/add', 'A nv mr szerepel a kick listn!\nKick listhoz a nv hozzadva: {0}');
INSERT INTO "localized_command" VALUES (75, 'huHU', 'autofunction/kick/channel/remove', 'Ilyen nv nem ltezik!\nKick listbl a nv eltvltsra kerlt: {0}');
INSERT INTO "localized_command" VALUES (76, 'huHU', 'autofunction/kick/channel/info', 'Kick listn lvk: {0}');
INSERT INTO "localized_command" VALUES (77, 'huHU', 'autofunction/mode/add', 'A nv mr szerepel a mode listn!\nMode listhoz a nv hozzadva: {0}');
INSERT INTO "localized_command" VALUES (78, 'huHU', 'autofunction/mode/remove', 'Ilyen nv nem ltezik!\nMode listbl a nv eltvltsra kerlt: {0}');
INSERT INTO "localized_command" VALUES (79, 'huHU', 'autofunction/mode/info', 'Mode listn lvk: {0}');
INSERT INTO "localized_command" VALUES (80, 'huHU', 'autofunction/mode/channel/add', 'A nv mr szerepel a mode listn!\nMode listhoz a nv hozzadva: {0}');
INSERT INTO "localized_command" VALUES (81, 'huHU', 'autofunction/mode/channel/remove', 'Ilyen nv nem ltezik!\nMode listbl a nv eltvltsra kerlt: {0}');
INSERT INTO "localized_command" VALUES (82, 'huHU', 'autofunction/mode/channel/info', 'Mode listn lvk: {0}');
INSERT INTO "localized_command" VALUES (83, 'huHU', 'message/channel', 'Az zenet sikeresen feljegyzsre kerlt.');
INSERT INTO "localized_command" VALUES (84, 'huHU', 'message', 'Az zenet sikeresen feljegyzsre kerlt.');
INSERT INTO "localized_command" VALUES (85, 'huHU', 'notes/info', 'Jegyzetek kdjai: {0}');
INSERT INTO "localized_command" VALUES (86, 'huHU', 'notes/user/access', 'Hozzfrs engedlyezve.\nHozzfrs megtagadva!');
INSERT INTO "localized_command" VALUES (87, 'huHU', 'notes/user/newpassword', 'Jelsz sikereset meg lett vltoztatva erre: {0}\nA mostani jelsz nem egyezik, modsits megtagadva!');
INSERT INTO "localized_command" VALUES (88, 'huHU', 'notes/user/register', 'Mr szerepelsz a felhasznli listn!\nSikeresen hozz vagy adva a felhasznli listhoz.');
INSERT INTO "localized_command" VALUES (89, 'huHU', 'notes/user/remove', 'Nincs megadva a jelsz a trls megerstshez!\nNem szerepelsz a felhasznli listn!\nA jelsz nem egyezik meg az adatbzisban trolttal!\nTrls meg lett szaktva!\nSikeresen trlve lett a felhasznld.');
INSERT INTO "localized_command" VALUES (90, 'huHU', 'notes/code/remove', 'Ilyen kd nem szerepel a listn!\nA jegyzet sikeresen trlsre kerlt.');
INSERT INTO "localized_command" VALUES (91, 'huHU', 'notes/code', 'Jegyzet: {0}');
INSERT INTO "localized_command" VALUES (92, 'huHU', 'notes', 'Nincs megadva jegyzetnek semmi se!\nA jegyzet kdneve mr szerepel az adatbzisban!\nJegyzet kdja: {0}');
INSERT INTO "localized_command" VALUES (93, 'huHU', 'notes/warning', 'Jelenleg nem szerepelsz a jegyzetek felhasznli listjn!\nAhoz hogy hozzad magad nem kell mst tenned mint az albbi parancsot vgrehajtani. (Lehetleg privt zenetknt ne hogy ms megtudja.)\n{0}jegyzet user register <jelsz>\nFelhasznli adatok frissitse (ha nem fogadn el adataidat) pedig: {0}jegyzet user hozzaferes <jelsz>');
INSERT INTO "localized_command" VALUES (94, 'huHU', 'message2', 'zenetet hagyta neked: {0}');
INSERT INTO "localized_command" VALUES (95, 'enUS', 'plugin/load', X'0203325B4C6F61645D3A0F0F2002416C6C20706C7567696E730F200333646F6E652E0F5C6E0203325B4C6F61645D3A0F0F2002416C6C20706C7567696E730F2003356661696C65642E0F');
INSERT INTO "localized_command" VALUES (96, 'enUS', 'plugin/unload', X'0203325B556E6C6F61645D3A0F0F2002416C6C20706C7567696E730F200333646F6E652E0F5C6E0203325B556E6C6F61645D3A0F0F2002416C6C20706C7567696E730F2003356661696C65642E0F');
INSERT INTO "localized_command" VALUES (97, 'enUS', 'plugin', X'027B307D3A0F2003336C6F616465642E0F');
INSERT INTO "localized_command" VALUES (98, 'enUS', 'compiler/memory', 'This function is disabled, because currently too many memory is allocated!');
INSERT INTO "localized_command" VALUES (99, 'enUS', 'compiler/warning', 'This code contains dangerous parts. Compiling stopped!');
INSERT INTO "localized_command" VALUES (100, 'enUS', 'compiler', 'The main function is not specified! (Schumix)\nThe main class is not specified!\nThe output text is too long so do not written out.\nSuccessfully compiled the code, only nothing output text!\nResidual is {0} line!');
INSERT INTO "localized_command" VALUES (101, 'enUS', 'compiler/code', 'Errors: {0}');

-- ----------------------------
-- Table structure for "localized_command_help"
-- ----------------------------
DROP TABLE IF EXISTS "localized_command_help";
CREATE TABLE `localized_command_help` (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4)    DEFAULT 'enUS',
Command TEXT,
Rank INTEGER DEFAULT 0,
Text TEXT
);

-- ----------------------------
-- Records of localized_command_help
-- ----------------------------
INSERT INTO "localized_command_help" VALUES (1, 'huHU', 'xbot', 9, 'Felhasznlok szmra hasznlhat parancslista.');
INSERT INTO "localized_command_help" VALUES (2, 'huHU', 'info', 9, 'Kis lers a botrl.');
INSERT INTO "localized_command_help" VALUES (3, 'huHU', 'whois', 9, 'A parancs segtsgvel megtudhatjuk hogy egy nick milyen channelon van fent.\nHasznlata: {0}whois <nick>');
INSERT INTO "localized_command_help" VALUES (4, 'huHU', 'roll', 9, 'Cspp szorakozs a wowbl, mr ha valaki felismeri :P');
INSERT INTO "localized_command_help" VALUES (5, 'huHU', 'date', 9, 'Az aktulis dtumot rja ki s a hozz tartoz nvnapot.');
INSERT INTO "localized_command_help" VALUES (6, 'huHU', 'time', 9, 'Az aktulis idt rja ki.');
INSERT INTO "localized_command_help" VALUES (7, 'huHU', 'google', 9, 'Ha szksged lenne valamire a google-bl nem kell hozz weboldal csak ez a parancs.\nHasznlata: {0}google <ide jn a keresett szveg>');
INSERT INTO "localized_command_help" VALUES (8, 'huHU', 'translate', 9, 'Ha rgtn kne fordtani msik nyelvre vagy -rl valamit, akkor megteheted ezzel a parancsal.\nHasznlata: {0}translate <kiindulsi nyelv|cl nyelv> <szveg>\nPldul: {0}translate hu|en Szp szveg.');
INSERT INTO "localized_command_help" VALUES (9, 'huHU', 'irc', 9, 'Nhny parancs hasznlata az IRC-n.\nHasznlata: {0}irc <parancs neve>');
INSERT INTO "localized_command_help" VALUES (10, 'huHU', 'calc', 9, 'Tbb funkcis szmolgp.\nHasznlata: {0}calc <szm>');
INSERT INTO "localized_command_help" VALUES (11, 'huHU', 'warning', 9, 'Figyelmeztet zenet kldse, hogy keresik ezen a csatornn vagy egy tetszleges zenet kldse.\nHasznlata: {0}warning <ide jn a szemly> <ha nem felhvt kldenl hanem sajt zenetet>');
INSERT INTO "localized_command_help" VALUES (12, 'huHU', 'sha1', 9, 'Sha1 kdolss talakit parancs.\nHasznlata: {0}sha1 <talaktand szveg>');
INSERT INTO "localized_command_help" VALUES (13, 'huHU', 'md5', 9, 'Md5 kdolss talakt parancs.\nHasznlata: {0}md5 <talaktand szveg>');
INSERT INTO "localized_command_help" VALUES (14, 'huHU', 'prime', 9, 'Meglaptja hogy a szm prmszm-e. Csak egsz szmmal tud szmolni!\nHasznlata: {0}prime <szm>');
INSERT INTO "localized_command_help" VALUES (15, 'huHU', 'admin', 0, 'Kirja az opertorok vagy adminisztrtorok ltal hasznlhat parancsokat.\nAdmin parancsai: info | list | add | remove | rank | access | newpassword');
INSERT INTO "localized_command_help" VALUES (16, 'huHU', 'admin/add', 0, '"j admin hozzadsa.\nHasznlata: {0}admin add <admin neve>');
INSERT INTO "localized_command_help" VALUES (17, 'huHU', 'admin/remove', 0, 'Admin eltvoltsa.\nHasznlata: {0}admin remove <admin neve>');
INSERT INTO "localized_command_help" VALUES (18, 'huHU', 'admin/rank', 0, 'Admin rangjnak megvltoztatsa.\nHasznlata: {0}admin rank <admin neve> <j rang pl halfoperator: 0, operator: 1, administrator: 2>');
INSERT INTO "localized_command_help" VALUES (19, 'huHU', 'admin/info', 0, 'Kiirja ppen milyen rangod van.');
INSERT INTO "localized_command_help" VALUES (20, 'huHU', 'admin/list', 0, 'Kiirja az sszes admin nevt aki az adatbzisban szerepel.');
INSERT INTO "localized_command_help" VALUES (21, 'huHU', 'admin/access', 0, 'Az admin parancsok hasznlathoz szksges jelsz ellenrz s vhost aktivl.\nHasznlata: {0}admin access <jelsz>');
INSERT INTO "localized_command_help" VALUES (22, 'huHU', 'admin/newpassword', 0, 'Az admin jelszavnak cserje ha j kne a rgi helyett.\nHasznlata: {0}admin newpassword <rgi jelsz> <j jelsz>');
INSERT INTO "localized_command_help" VALUES (23, 'huHU', 'colors', 0, 'Adott sklj szinek kirsa amit lehet hasznlni IRC-n.');
INSERT INTO "localized_command_help" VALUES (24, 'huHU', 'nick', 0, 'Bot nick nevnek cserje.\nHasznlata: {0}nick <nv>');
INSERT INTO "localized_command_help" VALUES (25, 'huHU', 'join', 0, 'Kapcsolods megadot csatornra.\nHasznlata:\nJelsz nlkli csatorna: {0}join <csatorna>\nJelszval elltott csatorna: {0}join <csatorna> <jelsz>');
INSERT INTO "localized_command_help" VALUES (26, 'huHU', 'left', 0, 'Lelps megadot csatonrl.\nHasznlata: {0}left <csatona>');
INSERT INTO "localized_command_help" VALUES (27, 'huHU', 'channel', 1, 'Channel parancsai: add | remove | info | update | language');
INSERT INTO "localized_command_help" VALUES (28, 'huHU', 'channel/add', 1, 'j channel hozzadsa.\nHasznlata: {0}channel add <channel> <ha van jelsz akkor az>');
INSERT INTO "localized_command_help" VALUES (29, 'huHU', 'channel/remove', 1, 'Nem hasznlatos channel eltvoltsa.\nHasznlata: {0}channel remove <channel>');
INSERT INTO "localized_command_help" VALUES (30, 'huHU', 'channel/info', 1, 'sszes channel kiirsa ami az adatbzisban van s a hozzjuk tartoz informciok.');
INSERT INTO "localized_command_help" VALUES (31, 'huHU', 'channel/update', 1, 'Channelekhez tartoz sszes informci frisstse, alaprtelmezsre lltsa.');
INSERT INTO "localized_command_help" VALUES (32, 'huHU', 'channel/language', 1, 'Frissti a csatorna nyelvezett.\nHasznlata: {0}channel language <csatorna> <nyelvezet>');
INSERT INTO "localized_command_help" VALUES (33, 'huHU', 'function', 1, 'Funkcik vezrlsre szolgl parancs.\nFunkci parancsai: channel | all | update | info\nHasznlata ahol tartzkodsz:\nChannel funkci kezelse: {0}function <on vagy off> <funkci nv>\nChannel funkcik kezelse: {0}function <on vagy off> <funkci nv1> <funkci nv2> ... stb');
INSERT INTO "localized_command_help" VALUES (34, 'huHU', 'function/channel', 1, '"Megadott channelen llithatk ezzel a parancsal a funkcik.\nFunkci channel parancsai: info\nHasznlata:\nChannel funkci kezelse: {0}function channel <on vagy off> <funkci nv>\nChannel funkcik kezelse: {0}function channel <on vagy off> <funkci nv1> <funkci nv2> ... stb');
INSERT INTO "localized_command_help" VALUES (35, 'huHU', 'function/channel/info', 1, 'Kirja a funkcik llapott.');
INSERT INTO "localized_command_help" VALUES (36, 'huHU', 'function/all', 1, 'Globlis funkcik kezelse.\nFunkci all parancsai: info\nEgyttes kezels: {0}function all <on vagy off> <funkci nv>\nEgyttes funkcik kezelse: {0}function all <on vagy off> <funkci nv1> <funkci nv2> ... stb');
INSERT INTO "localized_command_help" VALUES (37, 'huHU', 'function/all/info', 1, 'Kirja a funkcik llapott.');
INSERT INTO "localized_command_help" VALUES (38, 'huHU', 'function/update', 1, 'Frissti a funkcikat vagy alaprtelmezsre lltja.\nFunkci update parancsai: all\nHasznlata:\nMs channel: {0}function update <channel neve>\nAhol tartozkodsz channel: {0}function update');
INSERT INTO "localized_command_help" VALUES (39, 'huHU', 'function/update/all', 1, 'Frissti az sszes funkcit vagy alaprtelmezsre lltja.\Hasznlata: {0}function update all');
INSERT INTO "localized_command_help" VALUES (40, 'huHU', 'function/info', 1, 'Kirja a funkcik llapott.');
INSERT INTO "localized_command_help" VALUES (41, 'huHU', 'kick', 1, 'Kirgja a nick-et a megadott channelrl.\nHasznlata:\nCsak kirgs: {0}kick <channel> <nv>\nKirgs okkal: {0}kick <channel> <nv> <oka>');
INSERT INTO "localized_command_help" VALUES (42, 'huHU', 'mode', 1, 'Megvltoztatja a nick rangjt megadott channelen.\nHasznlata: {0}mode <rang> <nv vagy nevek>');
INSERT INTO "localized_command_help" VALUES (43, 'huHU', 'plugin', 2, 'Kirja milyen pluginok vannak betltve.\nPlugin parancsok: load | unload');
INSERT INTO "localized_command_help" VALUES (44, 'huHU', 'plugin/load', 2, 'Betlt minden plugint.');
INSERT INTO "localized_command_help" VALUES (45, 'huHU', 'plugin/unload', 2, 'Eltvolt minden plugint.');
INSERT INTO "localized_command_help" VALUES (46, 'huHU', 'quit', 2, 'Bot lelltsra hasznlhat parancs.');
INSERT INTO "localized_command_help" VALUES (47, 'huHU', 'schumix2', 9, 'Parancsok: nick | sys\nParancsok: ghost | nick | sys\nParancsok: ghost | nick | sys | clean\nParancsok: sys');
INSERT INTO "localized_command_help" VALUES (48, 'huHU', 'schumix2/sys', 9, 'Kirja a program informciit.');
INSERT INTO "localized_command_help" VALUES (49, 'huHU', 'schumix2/ghost', 1, 'Kilpteti a f nick-et ha regisztrlva van.\nHasznlata: {0} ghost');
INSERT INTO "localized_command_help" VALUES (50, 'huHU', 'schumix2/nick', 0, 'Bot nick nevnek cserje.\n"Hasznlata: {0} nick <nv>');
INSERT INTO "localized_command_help" VALUES (51, 'huHU', 'schumix2/nick/identify', 0, 'Aktivlja a f nick jelszavt.\nHasznlata: {0} nick identify');
INSERT INTO "localized_command_help" VALUES (52, 'huHU', 'schumix2/clean', 2, 'Felszabadtja a lefoglalt memrit.\nHasznlata: {0} clean');
INSERT INTO "localized_command_help" VALUES (53, 'huHU', 'svn', 1, 'Svn rss-ek kezelse.\nSvn parancsai: channel | info | list | start | stop | reload');
INSERT INTO "localized_command_help" VALUES (54, 'huHU', 'svn/channel', 1, 'Rss csatornkra val kiirsnak kezelse.\nChannel parancsai: add | remove');
INSERT INTO "localized_command_help" VALUES (55, 'huHU', 'svn/channel/add', 1, 'j csatorna hozzadsa az rss-hez.\nHasznlata: {0}svn channel add <rss neve> <csatorna>');
INSERT INTO "localized_command_help" VALUES (56, 'huHU', 'svn/channel/remove', 1, 'Nem hasznlatos csatorna eltvoltsa az rss-bl.\nHasznlata: {0}svn channel remove <rss neve> <csatorna>');
INSERT INTO "localized_command_help" VALUES (57, 'huHU', 'svn/info', 1, 'Kiirja az rss-ek llapott.');
INSERT INTO "localized_command_help" VALUES (58, 'huHU', 'svn/list', 1, 'Vlaszthat rss-ek listja.');
INSERT INTO "localized_command_help" VALUES (59, 'huHU', 'svn/start', 1, 'j rss betltse.\nHasznlata: {0}svn start <rss neve>');
INSERT INTO "localized_command_help" VALUES (60, 'huHU', 'svn/stop', 1, 'Rss lelltsa.\nHasznlata: {0}svn stop <rss neve>');
INSERT INTO "localized_command_help" VALUES (61, 'huHU', 'svn/reload', 1, 'Megadott rss jratltse.\nSvn reload parancsai: all');
INSERT INTO "localized_command_help" VALUES (62, 'huHU', 'svn/reload/all', 1, 'Minden rss jratltse.\nHasznlata: {0}svn reload <rss neve>');
INSERT INTO "localized_command_help" VALUES (63, 'huHU', 'hg', 1, 'Hg rss-ek kezelse.\nHg parancsai: channel | info | list | start | stop | reload');
INSERT INTO "localized_command_help" VALUES (64, 'huHU', 'hg/channel', 1, 'Rss csatornkra val kiirsnak kezelse.\nChannel parancsai: add | remove');
INSERT INTO "localized_command_help" VALUES (65, 'huHU', 'hg/channel/add', 1, 'j csatorna hozzadsa az rss-hez.\nHasznlata: {0}hg channel add <rss neve> <csatorna>');
INSERT INTO "localized_command_help" VALUES (66, 'huHU', 'hg/channel/remove', 1, 'Nem hasznlatos csatorna eltvoltsa az rss-bl.\nHasznlata: {0}hg channel remove <rss neve> <csatorna>');
INSERT INTO "localized_command_help" VALUES (67, 'huHU', 'hg/info', 1, 'Kiirja az rss-ek llapott.');
INSERT INTO "localized_command_help" VALUES (68, 'huHU', 'hg/list', 1, 'Vlaszthat rss-ek listja.');
INSERT INTO "localized_command_help" VALUES (69, 'huHU', 'hg/start', 1, 'j rss betltse.\nHasznlata: {0}hg start <rss neve>');
INSERT INTO "localized_command_help" VALUES (70, 'huHU', 'hg/stop', 1, 'Rss lelltsa.\nHasznlata: {0}hg stop <rss neve>');
INSERT INTO "localized_command_help" VALUES (71, 'huHU', 'hg/reload', 1, 'Megadott rss jratltse.\nHg reload parancsai: all');
INSERT INTO "localized_command_help" VALUES (72, 'huHU', 'hg/reload/all', 1, 'Minden rss jratltse.\nHasznlata: {0}hg reload <rss neve>');
INSERT INTO "localized_command_help" VALUES (73, 'huHU', 'git', 1, 'Git rss-ek kezelse.\nGit parancsai: channel | info | list | start | stop | reload');
INSERT INTO "localized_command_help" VALUES (74, 'huHU', 'git/channel', 1, 'Rss csatornkra val kiirsnak kezelse.\nChannel parancsai: add | remove');
INSERT INTO "localized_command_help" VALUES (75, 'huHU', 'git/channel/add', 1, 'j csatorna hozzadsa az rss-hez.\nHasznlata: {0}git channel add <rss neve> <tipus> <csatorna>');
INSERT INTO "localized_command_help" VALUES (76, 'huHU', 'git/channel/remove', 1, 'Nem hasznlatos csatorna eltvoltsa az rss-bl.\nHasznlata: {0}git channel remove <rss neve> <tipus> <csatorna>');
INSERT INTO "localized_command_help" VALUES (77, 'huHU', 'git/info', 1, 'Kiirja az rss-ek llapott.');
INSERT INTO "localized_command_help" VALUES (78, 'huHU', 'git/list', 1, 'Vlaszthat rss-ek listja.');
INSERT INTO "localized_command_help" VALUES (79, 'huHU', 'git/start', 1, 'j rss betltse.\nHasznlata: {0}git start <rss neve> <tipus>');
INSERT INTO "localized_command_help" VALUES (80, 'huHU', 'git/stop', 1, 'Rss lelltsa.\nHasznlata: {0}git stop <rss neve> <tipus>');
INSERT INTO "localized_command_help" VALUES (81, 'huHU', 'git/reload', 1, 'Megadott rss jratltse.\nGit reload parancsai: all');
INSERT INTO "localized_command_help" VALUES (82, 'huHU', 'git/reload/all', 1, 'Minden rss jratltse.\nHasznlata: {0}git reload <rss neve> <tipus>');
INSERT INTO "localized_command_help" VALUES (83, 'huHU', 'ban', 1, 'Tiltst rak a megadott nvre vagy vhost-ra.\nHasznlata:\nra s perc: {0}ban <nv> <:pp> <oka>\nDtum, ra s perc: {0}ban <nv> <.hh.nn> <:pp> <oka>');
INSERT INTO "localized_command_help" VALUES (84, 'huHU', 'unban', 1, 'Feloldja a tiltst a nvrl vagy vhost-rl ha szerepel a bot rendszerben.\nHasznlata: {0}unban <nv vagy vhost>');
INSERT INTO "localized_command_help" VALUES (85, 'huHU', 'notes', 9, 'Klnbz adatokat jegyezhetnk fel a segtsgvel.\nJegyzet parancsai: user | code\nJegyzet bekldse: {0}notes <egy kd amit megjegyznk pl: schumix> <amit feljegyeznl>');
INSERT INTO "localized_command_help" VALUES (86, 'huHU', 'notes/user', 9, 'Jegyzet felhasznl kezelse.\nUser parancsai: register | remove | access | newpassword');
INSERT INTO "localized_command_help" VALUES (87, 'huHU', 'notes/user/register', 9, 'j felhasznl hozzadsa.\nHasznlata: {0}notes user register <jelsz>');
INSERT INTO "localized_command_help" VALUES (88, 'huHU', 'notes/user/remove', 9, 'Felhasznl eltvoltsa.\nHasznlata: {0}notes user remove <jelsz>');
INSERT INTO "localized_command_help" VALUES (89, 'huHU', 'notes/user/access', 9, 'Az jegyzet parancsok hasznlathoz szksges jelsz ellenrz s vhost aktivl.\nHasznlata: {0}notes user access <jelsz>');
INSERT INTO "localized_command_help" VALUES (90, 'huHU', 'notes/user/newpassword', 9, 'Felhasznl jelszavnak cserje ha j kne a rgi helyet.\nHasznlata: {0}notes user newpassword <rgi jelsz> <j jelsz>');
INSERT INTO "localized_command_help" VALUES (91, 'huHU', 'notes/code', 9, 'Jegyzet kiolvasshoz szksges kd.\nHasznlata: {0}notes code <jegyzet kdja>\nKd parancsai: remove');
INSERT INTO "localized_command_help" VALUES (92, 'huHU', 'notes/code/remove', 9, 'Trli a jegyzetet kd alapjn.\nHasznlata: {0}notes code remove <jegyzet kdja>');
INSERT INTO "localized_command_help" VALUES (93, 'huHU', 'message', 9, 'Ezzel a paranccsal zenetet lehet hagyni brkinek a megadott csatornn.\nHasznlata: {0}message <nv> <zenet>\nzenet parancsai: channel');
INSERT INTO "localized_command_help" VALUES (94, 'huHU', 'message/channel', 9, 'Ezzel a paranccsal zenetet lehet hagyni brkinek a kivlasztott csatornn.\nHasznlata: {0}message channel <csatorna> <nv> <zenet>');
INSERT INTO "localized_command_help" VALUES (95, 'huHU', 'autofunction', 0, 'Autmatikusan mkd kdrszek kezelse.\nAutofunkcio parancsai: hlmessage\nAutmatikusan mkd kdrszek kezelse.\nAutofunkcio parancsai: kick | mode | hlmessage');
INSERT INTO "localized_command_help" VALUES (96, 'huHU', 'autofunction/hlmessage', 0, 'Autmatikusan hl-t kap nick-ek kezelse.\nHl zenet parancsai: function | update | info\nHasznlata: {0}autofunction hlmessage <zenet>');
INSERT INTO "localized_command_help" VALUES (97, 'huHU', 'autofunction/hlmessage/function', 0, 'Ezzel a parancsal llithat a hl llapota.\nHasznlata: {0}autofunction hlmessage function <llapot>');
INSERT INTO "localized_command_help" VALUES (98, 'huHU', 'autofunction/hlmessage/update', 0, 'Frissiti az adatbzisban szerepl hl listt!');
INSERT INTO "localized_command_help" VALUES (99, 'huHU', 'autofunction/hlmessage/info', 0, 'Kiirja a hl-ek llapott.');
INSERT INTO "localized_command_help" VALUES (100, 'huHU', 'autofunction/kick', 1, 'Autmatikusan kirgsra kerl nick-ek kezelse.\nKick parancsai: add | remove | info | channel');
INSERT INTO "localized_command_help" VALUES (101, 'huHU', 'autofunction/kick/add', 1, 'Kirgand nevnek hozzadsa ahol tartozkodsz.\nHasznlata: {0}autofunction kick add <nv> <oka>');
INSERT INTO "localized_command_help" VALUES (102, 'huHU', 'autofunction/kick/remove', 1, 'Kirgand nevnek eltvoltsa ahol tartozkodsz.\nHasznlata: {0}autofunction kick remove <nv>');
INSERT INTO "localized_command_help" VALUES (103, 'huHU', 'autofunction/kick/info', 1, 'Kiirja a kirgandok llapott.');
INSERT INTO "localized_command_help" VALUES (104, 'huHU', 'autofunction/kick/channel', 1, 'Autmatikusan kirgsra kerl nick-ek kezelse megadot channelen.\nKick channel parancsai: add | remove | info');
INSERT INTO "localized_command_help" VALUES (105, 'huHU', 'autofunction/kick/channel/add', 1, 'Kirgand nevnek hozzadsa megadott channelen.\nHasznlata: {0}autofunction kick channel add <nv> <csatorna> <oka>');
INSERT INTO "localized_command_help" VALUES (106, 'huHU', 'autofunction/kick/channel/remove', 1, 'Kirgand nevnek eltvoltsa megadott channelen.\nHasznlata: {0}autofunction kick channel remove <nv>');
INSERT INTO "localized_command_help" VALUES (107, 'huHU', 'autofunction/kick/channel/info', 1, 'Kiirja a kirgandok llapott.');
INSERT INTO "localized_command_help" VALUES (108, 'huHU', 'autofunction/mode', 1, 'Autmatikusan rangot kap nick-ek kezelse.\nMode parancsai: add | remove | info | channel');
INSERT INTO "localized_command_help" VALUES (109, 'huHU', 'autofunction/mode/add', 1, 'Rangot kap nevnek hozzadsa ahol tartozkodsz.\nHasznlata: {0}autofunction mode add <nv> <rang>');
INSERT INTO "localized_command_help" VALUES (110, 'huHU', 'autofunction/mode/remove', 1, 'Rangot kap nevnek eltvoltsa ahol tartozkodsz.\nHasznlata: {0}autofunction mode remove <nv>');
INSERT INTO "localized_command_help" VALUES (111, 'huHU', 'autofunction/mode/info', 1, 'Kiirja a rangot kapk llapott.');
INSERT INTO "localized_command_help" VALUES (112, 'huHU', 'autofunction/mode/channel', 1, 'Autmatikusan rangot kap nick-ek kezelse megadot channelen.\nMode channel parancsai: add | remove | info');
INSERT INTO "localized_command_help" VALUES (113, 'huHU', 'autofunction/mode/channel/add', 1, 'Rangot kap nevnek hozzadsa megadott channelen.\nsSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hasznlata: {0}autofunction mode channel add <nv> <csatorna> <rang>');
INSERT INTO "localized_command_help" VALUES (114, 'huHU', 'autofunction/mode/channel/remove', 1, 'Rangot kap nevnek eltvoltsa megadott channelen.\nHasznlata: {0}autofunction mode channel remove <nv>');
INSERT INTO "localized_command_help" VALUES (115, 'huHU', 'autofunction/mode/channel/info', 1, 'Kiirja a rangot kapk llapott.');

-- ----------------------------
-- Table structure for "localized_warning"
-- ----------------------------
DROP TABLE IF EXISTS "localized_warning";
CREATE TABLE `localized_warning` (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4)    DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- ----------------------------
-- Records of localized_warning
-- ----------------------------
INSERT INTO "localized_warning" VALUES (1, 'huHU', 'NoName', 'A nv nincs megadva!');
INSERT INTO "localized_warning" VALUES (2, 'huHU', 'NoValue', 'Nincs paramter!');
INSERT INTO "localized_warning" VALUES (3, 'huHU', 'No1Value', 'Nincs megadva egy paramter!');
INSERT INTO "localized_warning" VALUES (4, 'huHU', 'FaultyQuery', 'Hibs lekrdezs!');
INSERT INTO "localized_warning" VALUES (5, 'huHU', 'NoIrcCommandName', 'Nincs megadva a parancs neve!');
INSERT INTO "localized_warning" VALUES (6, 'huHU', 'NoWhoisName', 'Nincs megadva a keresend szemly neve!');
INSERT INTO "localized_warning" VALUES (7, 'huHU', 'NoGoogleText', 'Nincs megadva a keresend szveg!');
INSERT INTO "localized_warning" VALUES (8, 'huHU', 'NoTranslateText', 'Nincs megadva a fordtand szveg!');
INSERT INTO "localized_warning" VALUES (9, 'huHU', 'NoTranslateLanguage', 'Nincs megadva melyik nyelvrl melyikre fordtsa le!');
INSERT INTO "localized_warning" VALUES (10, 'huHU', 'NoNumber', 'Nincs megadva szm!');
INSERT INTO "localized_warning" VALUES (11, 'huHU', 'NoPassword', 'Nincs megadva a jelsz!');
INSERT INTO "localized_warning" VALUES (12, 'huHU', 'NoOldPassword', 'Nincs megadva a rgi jelsz!');
INSERT INTO "localized_warning" VALUES (13, 'huHU', 'NoNewPassword', 'Nincs megadva az j jelsz!');
INSERT INTO "localized_warning" VALUES (14, 'huHU', 'NoOperator', 'Nem vagy Opertor!');
INSERT INTO "localized_warning" VALUES (15, 'huHU', 'NoAdministrator', 'Nem vagy Adminisztrtor!');
INSERT INTO "localized_warning" VALUES (16, 'huHU', 'NoChannelName', 'Nincs megadva a csatorna neve!');
INSERT INTO "localized_warning" VALUES (17, 'huHU', 'NoRank', 'Nincs megadva a rang!');
INSERT INTO "localized_warning" VALUES (18, 'huHU', 'NoFunctionName', 'Nincs megadva a funkci neve!');
INSERT INTO "localized_warning" VALUES (19, 'huHU', 'NoFunctionStatus', 'Nincs megadva a funkci llapota!');
INSERT INTO "localized_warning" VALUES (20, 'huHU', 'NoCommand', 'Nincs megadva a parancs!');
INSERT INTO "localized_warning" VALUES (21, 'huHU', 'NoTypeName', 'Nincs a tipus neve megadva!');
INSERT INTO "localized_warning" VALUES (22, 'huHU', 'CapsLockOff', 'Kapcsold ki a caps lock-ot!');
INSERT INTO "localized_warning" VALUES (23, 'huHU', 'NoTime', 'Nincs megadva az id!');
INSERT INTO "localized_warning" VALUES (24, 'huHU', 'NoBanNameOrVhost', 'Nincs megadva a kitiltand neve vagy a vhost!');
INSERT INTO "localized_warning" VALUES (25, 'huHU', 'NoUnbanNameOrVhost', 'Nincs megadva a kitiltott neve vagy a vhost!');
INSERT INTO "localized_warning" VALUES (26, 'huHU', 'BanList', 'Mr szerepel a tilt listn!');
INSERT INTO "localized_warning" VALUES (27, 'huHU', 'BanList1', 'Sikeresen hozz lett adva a tilt listhoz.');
INSERT INTO "localized_warning" VALUES (28, 'huHU', 'UnbanList', 'Nem szerepel a tilt listn!');
INSERT INTO "localized_warning" VALUES (29, 'huHU', 'UnbanList1', 'Sikeresen trlve lett a tilt listhoz.');
INSERT INTO "localized_warning" VALUES (30, 'huHU', 'RecurrentFlooding', 'Ismtld flooding!');
INSERT INTO "localized_warning" VALUES (31, 'huHU', 'StopFlooding', 'llj le a flooding!');
INSERT INTO "localized_warning" VALUES (32, 'huHU', 'NoMessage', 'zenet nincs megadva!');
INSERT INTO "localized_warning" VALUES (33, 'huHU', 'NoCode', 'A kd nincs megadva!');
INSERT INTO "localized_warning" VALUES (34, 'huHU', 'NoReason', 'Nincs ok megadva!');
INSERT INTO "localized_warning" VALUES (35, 'huHU', 'NoChannelLanguage', 'Nincs megadva a csatorna nyelvezete!');
INSERT INTO "localized_warning" VALUES (36, 'huHU', 'NoDataNoCommand', 'Az adataid nem megfelelek ezrt nem folytathat a parancs!');
INSERT INTO "localized_warning" VALUES (37, 'enUS', 'CapsLockOff', 'Turn caps lock OFF!');
INSERT INTO "localized_warning" VALUES (38, 'enUS', 'RecurrentFlooding', 'Recurrent flooding!');
INSERT INTO "localized_warning" VALUES (39, 'enUS', 'StopFlooding', 'Stop flooding!');

-- ----------------------------
-- Table structure for "message"
-- ----------------------------
DROP TABLE IF EXISTS "message";
CREATE TABLE `message` (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(20),
Channel VARCHAR(20),
Message TEXT,
Wrote VARCHAR(20)
);

-- ----------------------------
-- Records of message
-- ----------------------------

-- ----------------------------
-- Table structure for "modelist"
-- ----------------------------
DROP TABLE IF EXISTS "modelist";
CREATE TABLE "modelist" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(20),
Channel VARCHAR(20),
Rank VARCHAR(10)
);

-- ----------------------------
-- Records of modelist
-- ----------------------------

-- ----------------------------
-- Table structure for "notes"
-- ----------------------------
DROP TABLE IF EXISTS "notes";
CREATE TABLE "notes" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Code TEXT,
Name VARCHAR(20),
Note TEXT
);

-- ----------------------------
-- Records of notes
-- ----------------------------

-- ----------------------------
-- Table structure for "notes_users"
-- ----------------------------
DROP TABLE IF EXISTS "notes_users";
CREATE TABLE "notes_users" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(20),
Password VARCHAR(40),
Vhost VARCHAR(50)
);

-- ----------------------------
-- Records of notes_users
-- ----------------------------

-- ----------------------------
-- Table structure for "schumix"
-- ----------------------------
DROP TABLE IF EXISTS "schumix";
CREATE TABLE "schumix" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
FunctionName VARCHAR(20),
FunctionStatus VARCHAR(3)
);

-- ----------------------------
-- Records of schumix
-- ----------------------------
INSERT INTO "schumix" VALUES (1, 'koszones', 'on');
INSERT INTO "schumix" VALUES (2, 'log', 'on');
INSERT INTO "schumix" VALUES (3, 'rejoin', 'on');
INSERT INTO "schumix" VALUES (4, 'commands', 'on');
INSERT INTO "schumix" VALUES (5, 'reconnect', 'on');
INSERT INTO "schumix" VALUES (6, 'autohl', 'off');
INSERT INTO "schumix" VALUES (7, 'autokick', 'off');
INSERT INTO "schumix" VALUES (8, 'automode', 'off');
INSERT INTO "schumix" VALUES (9, 'svn', 'off');
INSERT INTO "schumix" VALUES (10, 'hg', 'off');
INSERT INTO "schumix" VALUES (11, 'git', 'off');
INSERT INTO "schumix" VALUES (12, 'antiflood', 'off');
INSERT INTO "schumix" VALUES (13, 'message', 'off');
INSERT INTO "schumix" VALUES (14, 'compiler', 'on');

-- ----------------------------
-- Table structure for "svninfo"
-- ----------------------------
DROP TABLE IF EXISTS "svninfo";
CREATE TABLE "svninfo" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Name VARCHAR(20),
Link VARCHAR(255),
Website VARCHAR(30),
Channel TEXT
);

-- ----------------------------
-- Records of svninfo
-- ----------------------------
-- INSERT INTO "hginfo" VALUES (1, 'TrinityDB', 'http://code.google.com/feeds/p/trinitydb/hgchanges/basic', 'google', '#hun_bot,#schumix'); Példa a használatra
-- INSERT INTO "hginfo" VALUES (2, 'NeoCore', 'http://bitbucket.org/skyne/neocore/rss?token=2b6ceaf25f0a4c993ddc905327806e9c', 'bitbucket', '#hun_bot,#schumix'); Példa a használatra

-- ----------------------------
-- Table structure for "sznap"
-- ----------------------------
DROP TABLE IF EXISTS "sznap";
CREATE TABLE "sznap" (
guid INTEGER  PRIMARY KEY AUTOINCREMENT,
nev TEXT,
honap VARCHAR(30),
honap1 TINYINT,
nap TINYINT
);

-- ----------------------------
-- Records of sznap
-- ----------------------------

-- ----------------------------
-- Table structure for "uptime"
-- ----------------------------
DROP TABLE IF EXISTS "uptime";
CREATE TABLE "uptime" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
datum TEXT,
uptime TEXT,
memory TEXT
);

-- ----------------------------
-- Records of uptime
-- ----------------------------
