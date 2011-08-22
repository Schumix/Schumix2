-- ----------------------------
-- Table structure for "irc_commands"
-- ----------------------------
DROP TABLE IF EXISTS "irc_commands";
CREATE TABLE "irc_commands" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4)    DEFAULT 'enUS',
Command VARCHAR(30),
Text TEXT
);

-- huHU
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'mode', 'Mode használata: /mode <csatorna> <rang> <név>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'fixrank', 'Rang mentése: /chanserv <rang (sop, aop, hop, vop)> <csatorna> ADD <név>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Nick név megváltoztatása: /nick <új név>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'kick', 'Kick használata: /kick <csatorna> <név> (<oka> nem feltétlen kell)');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('huHU', 'owner', 'Owner mód bekapcsolása a csatornán: /msg chanserv SET <csatorna> ownermode on');

-- enUS
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'mode', 'Mode usage: /mode <channel> <rank> <name>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'fixrank', 'Save rank: /chanserv <rank (sop, aop, hop, vop)> <channel> ADD <name>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'nick', 'Nick change usage: /nick <new nick>');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'kick', 'Kick usage: /kick <channel> <name> (<reason>)');
INSERT INTO `irc_commands` (`Language`, `Command`, `Text`) VALUES ('enUS', 'owner', 'Turn on owner mode: /msg chanserv SET <channel> ownermode on');

-- ----------------------------
-- Table structure for "localized_command"
-- ----------------------------
DROP TABLE IF EXISTS "localized_command";
CREATE TABLE "localized_command" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4)    DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- huHU
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/sys', '3Verzió: 10{0}\n3Platform: {0}\n3OSVerzió: {0}\n3Programnyelv: c#\n3Memória használat:5 {0} MB\n3Memória használat:8 {0} MB\n3Memória használat:3 {0} MB\n3Mûködési idõ: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/help', '3Parancsok: nick | sys\n3Parancsok: ghost | nick | sys\n3Parancsok: ghost | nick | sys | clean\n3Parancsok: sys');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/ghost', 'Ghost paranccsal az elsõdleges nick visszaszerzése.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/nick', 'Név megváltoztatása erre: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/nick/identify', 'Azonosító jelszó küldése a kiszolgálónak.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'schumix2/clean', 'Lefoglalt memória felszabadításra kerül.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'help', 'Ha a parancs mögé írod a megadott parancs nevét vagy a nevet és alparancsát információt ad a használatáról.\nFõ parancsom: {0}xbot');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'xbot', '3Verzió: 10{0}\n3Parancsok: {0}\nProgrammed by: 3Csaba');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'info', '3Programozóm: Csaba, Jackneill.\n3Weboldal: https://github.com/megax/Schumix2\n3Elérhetõség: [MSN] megax@megaxx.info');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'time', 'Helyi idõ: {0}:0{1}\nHelyi idõ: {0}:{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'date', 'Ma {0}. 0{1}. 0{2}. {3} napja van.\nMa {0}. 0{1}. {2}. {3} napja van.\nMa {0}. {1}. 0{2}. {3} napja van.\nMa {0}. {1}. {2}. {3} napja van.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'roll', 'Százalékos aránya {0}%');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'irc', '3Parancsok: {0}\nNem létezik ilyen parancs!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'whois', 'Jelenleg itt van fent: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'warning', 'Keresnek téged itt: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'google', '2Title: Nincs Title.\n2Link: Nincs Link.\n2Title: {0}\n2Link: 3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'translate', 'Nincs fórdított szöveg.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'prime', 'Nem csak számot tartalmaz!\n{0} nem prímszám.\n{0} primszám.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/access', 'Hozzáférés engedélyezve.\nHozzáférés megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/newpassword', 'Jelszó sikeresen meg lett változtatva erre: {0}\nA mostani jelszó nem egyezik, modósítás megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/info', 'Jelenleg Fél Operátor vagy.\nJelenleg Operátor vagy.\nJelenleg Adminisztrátor vagy.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/list', '2Adminok: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/add', 'A név már szerepel az admin listán!\nAdmin hozzáadva: {0}\nMostantól Schumix adminja vagy. A mostani jelszavad: {0}\nHa megszeretnéd változtatni használd az {0}admin newpassword parancsot. Használata: {0}admin newpassword <régi> <új>\nAdmin nick élesítése: {0}admin access <jelszó>');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/remove', 'Ilyen név nem létezik!\nAdmin törölve: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin/rank', 'Rang sikeresen módosítva.\nHibás rang!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'admin', '3Fél Operátor parancsok!\n3Parancsok: {0}\n3Operátor parancsok!\n3Parancsok: {0}\n3Adminisztrátor parancsok!\n3Parancsok: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'colors', '1teszt1 2teszt2 3teszt3 4teszt4 5teszt5 6teszt6 7teszt7 8teszt8 9teszt9 10teszt10 11teszt11 12teszt12 13teszt13 14teszt14 15teszt15');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'nick', 'Nick megváltoztatása erre: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'join', 'Kapcsolódás ehhez a csatornához: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'leave', 'Lelépés errõl a csatornáról: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/all/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/all', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/channel', '{0}: bekapcsolva\n{0}: kikapcsolva\nIlyen csatorna nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update', 'Sikeresen frissítve {0} csatornán a funkciók.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function/update/all', 'Sikeresen frissítve minden csatornán a funkciók.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel', '3Parancsok: add | remove | info | update | language');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/add', 'A név már szerepel a csatorna listán!\nCsatorna hozzáadva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/remove', 'A mester csatorna nem törölhetõ!\nIlyen csatorna nem létezik!\nCsatorna eltávolítva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/update', 'A csatorna információk frissítésre kerültek.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/info', '3Aktív: {0}\n3Aktív: Nincs információ.\n3Inaktív: {0}\n3Inaktív: Nincs információ.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'channel/language', 'Csatorna nyelvezete sikeresen meg lett változtatva erre: {0}\nIlyen csatorna nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'plugin/load', '2[Betöltés]: Összes plugin betöltése 3sikeres.\n2[Betöltés]: Összes plugin betöltése 5sikertelen.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'plugin/unload', '2[Leválasztás]: Összes plugin leválasztása 3sikeres.\n2[Leválasztás]: Összes plugin leválasztása 5sikertelen.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'plugin', '{0}: 3loaded.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'quit', 'Viszlát :(\n{0} leállított paranccsal.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/start', '{0} már el van indítva!\n{0} sikeresen el lett indítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/stop', '{0} már le van állítva!\n{0} sikeresen le lett állítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/reload', '{0} sikeresen újra lett indítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/reload/all', 'Minden rss újra lett indítva.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/channel/add', 'Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'svn/channel/remove', 'Csatorna sikeresen törölve.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/start', '{0} már el van indítva!\n{0} sikeresen el lett indítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/stop', '{0} már le van állítva!\n{0} sikeresen le lett állítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/reload', '{0} sikeresen újra lett indítva.\n{0} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/reload/all', 'Minden rss újra lett indítva.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/channel/add', 'Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'hg/channel/remove', 'Csatorna sikeresen törölve.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/info', '3{0} 7{1} Channel: 2{2}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/list', '2Lista:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/start', '{0} {1} már el van indítva!\n{0} {1} sikeresen el lett indítva.\n{0} {1} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/stop', '{0} {1} már le van állítva!\n{0} {1} sikeresen le lett állítva.\n{0} {1} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/reload', '{0} {1} sikeresen újra lett indítva.\n{0} {1} nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/reload/all', 'Minden rss újra lett indítva.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/channel/add', 'Csatorna sikeresen hozzáadva.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'git/channel/remove', 'Csatorna sikeresen törölve.\nNem létezik ilyen név!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/memory', 'Jelenleg túl sok memóriát fogyaszt a bot ezért ezen funkció nem elérhetõ!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/warning', 'A kódban olyan részek vannak melyek veszélyeztetik a programot. Ezért leállt a fordítás!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler', 'Nincs megadva a fõ fv! (Schumix)\nNincs megadva a fõ class!\nA kimeneti szöveg túl hosszú ezért nem került kiírásra!\nA kód sikeresen lefordult csak nincs kimenõ üzenet!\nHátramaradt még {0} kiírás!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/code', 'Hibák: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'compiler/kill', 'Szál kilõve!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handlekick', '{0} kirúgta a következõ felhasználót: {1} oka: {2}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'ban', 'Helytelen dátum formátum!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction', '3Parancsok: hlmessage\n3Parancsok: kick | mode | hlmessage');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/info', '3Létezõ nickek: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/update', 'Az adatbázis sikeresen frissítésre került.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/hlmessage/function', '{0}: bekapcsolva\n{0}: kikapcsolva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/hlmessage', 'Az üzenet módosításra került.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/add', 'A név már szerepel a kick listán!\nKick listához a név hozzáadva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/remove', 'Ilyen név nem létezik!\nKick listából a név eltávólításra került: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/list', '3Kick listán lévõk: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/add', 'A név már szerepel a kick listán!\nKick listához a név hozzáadva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/remove', 'Ilyen név nem létezik!\nKick listából a név eltávólításra került: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/kick/channel/list', '3Kick listán lévõk: {0}\nIlyen csatorna nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/add', 'A név már szerepel a mode listán!\nMode listához a név hozzáadva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/change', 'Ilyen név nem létezik!\n{0} új rangja: {1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/remove', 'Ilyen név nem létezik!\nMode listából a név eltávólításra került: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/list', '3Voice listán lévõk: {0}\n3Fél Operátor listán lévõk: {0}\n3Operátor listán lévõk: {0}\n3Adminisztrátor listán lévõk: {0}\n3Tulajdonos listán lévõk: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/add', 'A név már szerepel a mode listán!\nMode listához a név hozzáadva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/change', 'Ilyen név nem létezik!\n{0} új rangja: {1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/remove', 'Ilyen név nem létezik!\nMode listából a név eltávólításra került: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'autofunction/mode/channel/list', '3Voice listán lévõk: {0}\n3Fél Operátor listán lévõk: {0}\n3Operátor listán lévõk: {0}\n3Adminisztrátor listán lévõk: {0}\n3Tulajdonos listán lévõk: {0}\nIlyen csatorna nem létezik!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'message/channel', 'Az üzenet sikeresen feljegyzésre került.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'message', 'Az üzenet sikeresen feljegyzésre került.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/info', '3Jegyzetek kódjai: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/access', 'Hozzáférés engedélyezve.\nHozzáférés megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/newpassword', 'Jelszó sikeresen meg lett változtatva erre: {0}\nA mostani jelszó nem egyezik, módosítás megtagadva!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/register', 'Már szerepelsz a felhasználói listán!\nSikeresen hozzá vagy adva a felhasználói listához.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/user/remove', 'Nincs megadva a jelszó a törlés megerõsítéséhez!\nNem szerepelsz a felhasználói listán!\nA jelszó nem egyezik meg az adatbázisban tároltal!\nTörlés meg lett szakítva!\nSikeresen törölve lett a felhasználód.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/code/remove', 'Ilyen kód nem szerepel a listán!\nA jegyzet sikeresen törlésre került.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/code', '3Jegyzet: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes', 'Nincs megadva jegyzetnek semmi se!\nA jegyzet kódneve már szerepel az adatbázisban!\nJegyzet kódja: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'notes/warning', 'Jelenleg nem szerepelsz a jegyzetek felhasználói listáján!\nAhoz hogy hozzáadd magad nem kell mást tenned mint az alábbi parancsot végrehajtani. (Lehetõleg privát üzenetként nehogy más megtudja.)\n{0}notes user register <jelszó>\nFelhasználói adatok frissítése (ha nem fogadná el adataidat) pedig: {0}notes user access <jelszó>');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'message2', 'Üzenetet hagyta neked: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'reload', '{0} újra lett indítva.\nA programban nincs ilyen rész!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'weather', '12Idõjárás otthon!\n5{0} 12idõjárása!\n3Nappal: {0}\n3Éjszaka: {0}\nNem szerepel ilyen város a listán!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handlejoin/random', 'Hello\nCsáó\nHy\nSzevasz\nÜdv\nSzervusz\nAloha\nJó napot\nSzia\nHi\nSzerbusz\nHali\nSzeva');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handlejoin', 'Jó reggelt {0}\nJó estét {0}\nÜdv fõnök');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handleleft/random', 'Viszlát\nBye');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('huHU', 'handleleft', 'Jóét {0}');

-- enUS
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/sys', '3Version: 10{0}\n3Platform: {0}\n3OSVersion: {0}\n3Programming language: c#\n3Memory allocation:5 {0} MB\n3Memory allocation:8 {0} MB\n3Memory allocation:3 {0} MB\n3Uptime: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/help', '3Commands: nick | sys\n3Commands: ghost | nick | sys\n3Commands: ghost | nick | sys | clean\n3Commands: sys');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/ghost', 'Return to the primary nick.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/nick', 'Change nick to: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/nick/identify', 'Send identifcation password to the server.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'schumix2/clean', 'Allocated memory will be freed.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'help', 'If you wrote behind the command the command or the name or co-command then gets information about usage.\nMain command: {0}xbot');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'xbot', '3Version: 10{0}\n3Commands: {0}\nProgrammed by: 3Csaba');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'info', '3My programmer(s): Csaba, Jackneill.\n3Website: https://github.com/megax/Schumix2\n3Contact: [MSN] megax@megaxx.info');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'time', 'Local time: {0}:0{1}\nLocal time: {0}:{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'date', "Today is {0}. 0{1}. 0{2}. and {3}'s day.\nToday is {0}. 0{1}. {2}. and {3}'s day.\nToday is {0}. {1}. 0{2}. and {3}'s day.\nToday is {0}. {1}. {2}. and {3}'s day.");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'roll', 'Pencentage rate: {0}%');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'irc', '3Commands: {0}\nNo such command!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'whois', 'Now online here: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'warning', 'Looking for you here: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'google', '2Title: Nothing Title.\n2Link: Nothing Link.\n2Title: {0}\n2Link: 3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'translate', 'Nothing translated text.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'prime', 'This is not a numeric text!\n{0} is not a prime number.\n{0} is a prime number.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/access', 'Access granted.\nAccess denied.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/newpassword', 'Successfully changed to password to: {0}\nThe current password does not match, modification denied!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/info', 'You are half operator now.\nYou are operator now.\nYou are administrator now.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/list', '2Admins: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/add', "The name is already in the admin list!\nAdmin added to the list: {0}\nYou are schumix's admin now. Your current password is: {0}\nIf you want to change it, use this command: {0}admin newpassword. Usage: {0}admin newpassword <Old> <New>\nAdmin nick confirmation: {0}admin access <Password>");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/remove', 'No such nick!\nAdmin was deleted: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin/rank', 'Successfully changed the rank!\nRank error!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'admin', '3half operator commands!\n3Commands: {0}\n3Operator commands!\n3Commands: {0}\n3Administrator commands!\n3Commands: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'colors', '1test1 2test2 3test3 4test4 5test5 6test6 7test7 8test8 9test9 10test10 11test11 12test12 13test13 14test14 15test15');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'nick', 'Nick changes to: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'join', 'Join to this channel: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'leave', 'Part of this channel: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/info', '2On: {0}\n2Off: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/all/info', '2Bekapcsolva: {0}\n2Kikapcsolva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/all', '{0}: are on.\n{0}: are off.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/channel/info', '2On: {0}\n2Off: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/channel', '{0}: On\n{0}: Off\nNo such channel!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/update', 'Successfully updated the channel functions in: {0}.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function/update/all', 'Successfully updated the information of all channels.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'function', '{0}: are on.\n{0}: are off.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel', '3Commands: add | remove | info | update | language');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/add', 'The name is already exists in the channel list.\nAdded channel: {0}\nNo such channel!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/remove', 'The master channel cannot delete!\nNo such channel!\nDeleted channel: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/update', 'The channel informations are updated.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/info', '3Active: {0}\n3Active: Nothing information.\n3Inactive: {0}\n3Inactive: Nothing information.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'channel/language', 'Successfully changed the channel language to: {0}\nNo such channel!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'plugin/load', '2[Load]: All plugins 3done.\n2[Load]: All plugins 5failed.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'plugin/unload', '2[Unload]: All plugins 3done.\n2[Unload]: All plugins 5failed.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'plugin', '{0}: 3loaded.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'quit', 'Bye :(\n{0} shutted down me with command.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/list', '2List:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/start', '{0} already translated!\n{0} successfully started.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/stop', '{0} already stopped!\n{0} successfully stopped.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/reload', '{0} successfully restarted.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/reload/all', 'All of Rss is restarted.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/channel/add', 'Successfully added channel.\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'svn/channel/remove', 'Successfully deleted channel!\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/info', '3{0} Channel: 2{1}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/list', '2List:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/start', '{0} already translated!\n{0} successfully started.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/stop', '{0} already stopped!\n{0} successfully stopped.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/reload', '{0} successfully restarted.\n{0} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/reload/all', 'All of Rss is restarted.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/channel/add', 'Sccessfully added channel!\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'hg/channel/remove', 'Successfully deleted channel!.\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/info', '3{0} 7{1} Channel: 2{2}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/list', '2List:3{0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/start', '{0} {1} already translated!\n{0} {1} successfully started.\n{0} {1} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/stop', '{0} {1} already stopped!\n{0} {1} successfully stopped.\n{0} {1} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/reload', '{0} {1} successfully restarted.\n{0} {1} no such!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/reload/all', 'All of Rss is restarted.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/channel/add', 'Successfully added channel!\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'git/channel/remove', 'Successfully deleted channel!\nNo such name!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'compiler/memory', 'Currently too many memory is allocated so this function is disabled!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'compiler/warning', 'This code contains dangerous parts. Compiling stopped!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'compiler', 'The main function is not specified! (Schumix)\nThe main class is not specified!\nThe output text is too long so did not written out!\nSuccessfully compiled the code, only nothing output text!\nLeft: {0} line!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'compiler/code', 'Errors: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'compiler/kill', 'Killed thread!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handlekick', '{0} kicked that user: {1} reason: {2}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'ban', 'Date format error!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction', '3Commands: hlmessage\n3Commands: kick | mode | hlmessage');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/hlmessage/info', '3Exciting nicks: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/hlmessage/update', 'Successfully updated the database.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/hlmessage/function', '{0}: are on\n{0}: are off.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/hlmessage', 'The message has been modified!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/add', 'The name is already on the kick list!\nThe name has been added to the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/remove', 'No such name!\nThe name has been deleted from the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/list', '3On the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/add', 'The name is already on the kick list!A név már szerepel a kick listán!\nKick listához a név hozzáadva: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/remove', 'No such name!\nThe name has been deleted from the kick list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/kick/channel/list', '3On the kick list: {0}\nNo such channel!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/add', 'The name is already on the mode list!\nThe name added to the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/change', "No such name!\n{0}'s new rank: {1}");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/remove', 'No such name!\The name has been deleted from the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/list', '3Voice list: {0}\n3Half Operator list: {0}\n3Operator list: {0}\n3Administrator list: {0}\n3Owner list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/add', 'The name is already on the mode list!\nThe name added to the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/change', "No such name!\n{0}'s new rank: {1}");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/remove', 'No such name!\nThe name has been deleted from the mode list: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'autofunction/mode/channel/list', '3Voice list: {0}\n3Half Operator list: {0}\n3Operator list: {0}\n3Administrator list: {0}\n3Owner list: {0}\nNo such channel!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'message/channel', 'Successfully recorded the message.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'message', 'Successfully recorded the message.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/info', '3Recorded message (quotes) codes: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/user/access', 'Access granted.\nAccess denied!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/user/newpassword', 'Successfully changed the password to: {0}\nThe password does not match! Modification is denied!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/user/register', 'You are already on the user list!\nSuccessfully added you to the user list!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/user/remove', 'The password for confirmation is not specified!The password for delete confirmation is not specified!\nYou are not in user list!\nYour password does not match which is stored in database!\nThe password does not match which is stored in database!\nThe deleting is aborted!\nSuccessfully deleted your account.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/code/remove', 'This code is not in the list!\nSuccessfully deleted the note.');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/code', '3Note: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes', "No text found for note!\nNote's codename is alreadyin the database!\nNote's code: {0}");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'notes/warning', "You are not in the note's user list!\nIf you want to add yourself, you have to do the following command. (Must be a private message, do not gather info someone else.)\n{0}notes user register <password>\nUpdating user data (If do not accept your datas) Do: {0}notes user access <password>");
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'message2', 'Left the note for you: {0}');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'reload', '{0} reloaded.\nThe program does not contains that part!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'weather', '12Local weather!\n5{0} 12weather!\n3Day: {0}\n3Night: {0}\nNo such city in the list!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handlejoin/random', 'Hello\nHy\nHi\nGood afternoon');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handlejoin', 'Good Morning {0}\nGood Night {0}\nWelcome boss!');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handleleft/random', 'ByeBye\nBye');
INSERT INTO `localized_command` (`Language`, `Command`, `Text`) VALUES ('enUS', 'handleleft', 'Goodbye {0}');

-- ----------------------------
-- Table structure for "localized_warning"
-- ----------------------------
DROP TABLE IF EXISTS "localized_warning";
CREATE TABLE "localized_warning" (
Id INTEGER  PRIMARY KEY AUTOINCREMENT,
Language VARCHAR(4)    DEFAULT 'enUS',
Command TEXT,
Text TEXT
);

-- huHU
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoName', 'A név nincs megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoValue', 'Nincs paraméter!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'No1Value', 'Nincs megadva egy paraméter!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'FaultyQuery', 'Hibás lekérdezés!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoWhoisName', 'Nincs megadva a keresendõ személy neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoGoogleText', 'Nincs megadva a keresendõ szöveg!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTranslateText', 'Nincs megadva a fordítandó szöveg!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTranslateLanguage', 'Nincs megadva melyik nyelvrõl melyikre fordítsa le!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoNumber', 'Nincs megadva szám!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoPassword', 'Nincs megadva a jelszó!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoOldPassword', 'Nincs megadva a régi jelszó!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoNewPassword', 'Nincs megadva az új jelszó!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoOperator', 'Nem vagy Operátor!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoAdministrator', 'Nem vagy Adminisztrátor!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoChannelName', 'Nincs megadva a csatorna neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoRank', 'Nincs megadva a rang!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoFunctionName', 'Nincs megadva a funkció neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoFunctionStatus', 'Nincs megadva a funkció állapota!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoCommand', 'Nincs megadva a parancs!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTypeName', 'Nincs megadva a típus neve!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'CapsLockOff', 'Kapcsold ki a caps lock-ot!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoTime', 'Nincs megadva az idõ!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoBanNameOrVhost', 'Nincs megadva a kitiltandó neve vagy a vhost!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoUnbanNameOrVhost', 'Nincs megadva a kitiltott neve vagy a vhost!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'BanList', 'Már szerepel a tiltó listán!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'BanList1', 'Sikeresen hozzá lett adva a tiltó listához.');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'UnbanList', 'Nem szerepel a tiltó listán!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'UnbanList1', 'Sikeresen törölve lett a tiltó listához.');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'RecurrentFlooding', 'Ismétlõdõ flooddal!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'StopFlooding', 'Állj le a floodolással!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoMessage', 'Üzenet nincs megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoCode', 'A kód nincs megadva!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoReason', 'Nincs megadva az ok!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoChannelLanguage', 'Nincs megadva a csatorna nyelvezete!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoDataNoCommand', 'Az adataid nem megfelelõek ezért nem folytatható a parancs!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoCityName', 'Nem adott meg egy városnevet sem!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NoMessageFunction', 'A funkció jelenleg nem üzemel!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('huHU', 'NotaChannelHasBeenSet', 'Nem csatorna lett megadva!');

-- enUS
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoName', 'The name is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoValue', 'The parameters are not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'No1Value', 'A parameter was not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'FaultyQuery', 'Syntax error!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoWhoisName', "The searching person's name are not specified!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoGoogleText', 'The searching text is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoTranslateText', 'The text to be translated is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoTranslateLanguage', 'Which language to other language text is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoNumber', 'The number is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoPassword', 'The password is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoOldPassword', 'The old password is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoNewPassword', 'The new password is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoOperator', 'You are not an operator!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoAdministrator', 'You are not an administrator!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoChannelName', 'The channel name is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoRank', 'The rank is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoFunctionName', 'The function name is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoFunctionStatus', 'The function status is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoCommand', 'The command is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoTypeName', 'The type is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'CapsLockOff', 'Turn caps lock OFF!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoTime', 'The time is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoBanNameOrVhost', "The banning person's name or his/her vhost is not specified!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoUnbanNameOrVhost', "The unbanning person's name or his/her vhost is not specified!");
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'BanList', 'Already on the bann list!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'BanList1', 'Successfully added to the bann list!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'UnbanList', 'He/She is not on the bann list!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'UnbanList1', 'Successfully deleted from the bann list!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'RecurrentFlooding', 'Recurrent flooding!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'StopFlooding', 'Stop flooding!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoMessage', 'Message is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoCode', 'The code is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoReason', 'Reason is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoChannelLanguage', 'The channel language is not specified!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoDataNoCommand', 'Your datas are bad, so aborted the command!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoCityName', 'No such city name!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NoMessageFunction', 'This function is currently not operating!');
INSERT INTO `localized_warning` (`Language`, `Command`, `Text`) VALUES ('enUS', 'NotaChannelHasBeenSet', 'Not a channel has been set!');
