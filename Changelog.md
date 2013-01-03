# Schumix2 ChangeLog

## 3.0.0 (2011-05-18)

 * Revision addon hozzáadva de még fejlesztve lesz. Többfunkciós nyelvi használat lehetősége hozzáadva. Kód nagyon nagy százalékban angol nyelvű lett. Verziók óta hordozott híbák javítva. Egyéb javítás.
 * Mysql-hez hozzáadva a charset modósításának lehetősége.
 * Minden Irc opcode ami jön új szálra kerül.
 * Uzenet parancs hozzáadva a kódhoz és a régi átnevezve. UserInfo és IgnoreChannel hozzáadva a konfighoz. Caps lock elleni védelem hozzáadva. CNick() fv helye fixálva az fv-ken belül.
 * Nyelvtani hibák kijavítva
 * Compiler egyes adatai áthelyezve konfigba. Calendar bővitve flood elleni védelemmel és bannal. Console parancsokhoz elkészült a help rész. Egyéb javítás.
 * CalendarAddon hozzáadásra került. Egyenlőre még üres. Url title kiirása hozzáadva kódhoz.
 * Fölös using törölve.
 * Végzetes hiba javítva.
 * Legtöbb Assembly információ egy helyre került.
 * Csatorna neve fixálva lett. Mostantól bármilyen formában kezeli a kód. Egyéb javítások.

## 2.9.0 (2011-04-23)

 * Asmg és Me hozzáadva az üzenet küldéshez. De még nem üzemel mert nincs meg a strukturája.
 * Végtelen ciklus elleni védelem bővitve.
 * Compiler ellátva végtelen ciklus elleni védelemmel. Ez még majd változhat plusz bővülhet a kód védelemmel mert elég sok rés lehet még. =/
 * Végzetes hiba javítva.
 * SplitToString fv hozzáadva a kóhoz. IsNull() fv használatba véve. SvnRss indulási lekérdezése fixálva.
 * Compiler addonhoz hozzáadva a konfigurálás lehetősége.
 * Compiler rész fixálva lett. Igaz nem kéne hogy a kód mögé lehessen írni szöveget de a sokszoros szóköz bekerülés és ebből a hibából eredő lefordítási hiba már idegesítő. Így jobb lesz (remélem).
 * Jegyzet parancs végre hozzá lett adva a kódhoz. Remélem elnyeri mindenki tetszését.
 * Config fájl szerkezete megigazítva.
 * GitRss addon hozzáadásra került. Másik két rss addonba fixálva lett a csatorna hozzáadás és eltávolításra szólgáló fv.

## 2.8.0 (2011-04-14)

 * Platform kiírása áthelyezve.
 * Konzol uptime kiirasa fixalva.
 * Compiler addon hibája javítva lett.
 * Kis igazitas az assembly infokon.
 * Singleton alkalmazva az AddonManagerre.
 * Sql hibák fixálva lettek.
 * Schumix.sql fixálva. Hg rss hozzáadásra került. Már csak a git rss hiányzik. Svn rss ciklusa javítva. Indulásnál elmenti az akkori verziószámot így ha pl ha útána kapcsoljuk be mikor elindult és még nincs új verzió akkor nem fogja kiírni fölöslegesen a régit.
 * Admin fv lecserélve IsAdmin-ra. Fél Operátor hozzáadva a kódhoz így jobban eloszthatóak a rangok. Régi hibák javításra kerültek. Egyéb javítás.
 * Readme fájl frissitésre került. :)
 * SvnInfo.cs törlésre került.
 * Rendszer válaszüzenetei átírva vagy fixálva. Remélhetőleg így érthetöbb lesz mi baja van a kódnak.
 * Indulasi szoveg atirva.
 * Takarítás.
 * Addon betöltés fixálva. Mostantól újratöltés esetén nem fogja betölteni azokat az addonokat melyek már részei a kódnak.
 * Ignore hozzáadva az addonhoz. Fixálva a betöltendő fájlok listája. Mostantól csak az az addon töltödik be amelyik nevében szerepel az addon szó.
 * Forditas win-en fixalva. SQLite adatbazis frissitve. Schumix.sql frissitve.

## 2.7.0 (2011-04-04)

 * SvnRssAddon hozzáadva a kódhoz.
 * A Konfig fájl ezentúl a Configs mappában lesz tárolva. Emelet az addonok konfig fájlai is oda fognak legenerálodni és onnét töltödnek be. Legalább is amit én adok hozzá.
 * Singleton fellett újítva. Autofunkciók befejezésre kerültek (mode, kick).
 * Autofunkcó hozzáadása megkezdödött. Jelenleg a hlüzenet van csak hozzáadva. Hamarosan a többi is hozzáadásra kerül.
 * IrcLogDirectory módosítva Szobáról Csatornára.
 * Kód egy részéhez frissitve az adatbázis szerkezete. További frissitések várhatóak a késöbbiekben.
 * Üzenet küldés az irc kiszolgáló felé korlátozhatóvá vált. Így megelőzhető a flood elleni védelem például.
 * Help rész fixálva.
 * Fixálva az admin és új csatorna hozzáadása.

## 2.6.0 (2011-03-28)

 * Plugin rendszer frissitésre került.
 * Master csatorna hozzáadva a konfighoz.
 * Mono platform hozzáadva. x64 platform hozzáadva de még nem az igazi vagy csak nálam rossz.
 * gitignore módositva.
 * gitignore lista bővitve.
 * plugin mappa neve átírva.
 * AssemblyInfok frissítesre kerültek.
 * Finomítások a kódon.

## 2.5.0 (2011-03-16)

 * Console rész újra lett írva. Egyébb finomítások is történtek.
 * Console parancsokhoz hozzáadva a channel parancs. FSelect fv átalakítva.
 * Schumix.Libraries project hozzáadva.
 * Előjel elemzése fixálva. Hiba oka az volt hogy ha túl hosszú volt előjel és kevesebb karakter lett beírva összeomlott kód.
 * TODO lista frissitve. Hamarosan teljesen elkészül Schumix2 és egyszinten lesz elődjével.
 * Válasz üzenetek hozzáadva a legtöbb részhez. Ha valahonnét hiányzik csak jelezni kell és potlom.
 * Parancsok letiltása javítva.

## 2.2.0 (2011-03-08)

 * Flood elleni védelem hozzáadva a compiler részhez.
 * SQLite kezelés lehetősége hozzáadva a kódhoz.
 * Md5 és sha1 kódja frissítve.
 * String lecserélve string-re.
 * Fájlba logolás lehetősége hozzáadva a Log-hoz.
 * Konzol parancshoz hozzáadva a nick, join, left parancs.
 * Windows javítás.
 * Konfig automatikus legenárálásának lehetősége hozzáadva a rendszerhez.

## 2.1.0 (2011-03-01)

 * Alapértelmezetten a koszones, kick és mode funkciók kikapcsolásra kerültek.
 * Url encode hozzáadva a kódhoz.
 * WolframAPI frissítve.
 * LargeWarning() hozzáadva a Log-hoz.
 * "" cserélve String.Empty-re.
 * Parancsok kezelése szétválasztva.
 * ExtraPlugin hozzáadva.
 * Összeomlás javítva.
 * Kiírások javítva.

## 2.0.0 (2011-02-12)

 * Compiler plugin hozzáadva.
 * Help() és Privmsg() függvény hozzáadva a plugin API-hoz.
 * Sealed elhelyezve pár osztályban.
 * Amin parancsnál fixálva az admin törlése. Mostantól operátor nem törölhet admin-t.
 * Foglalt nick esetén a bot keres egy szabad nick-et.
 * Fölösleges using törölve.
 * Fordit parancs létrehozva.
 * Parancs előjel innéttől bármilyen hosszú lehet.
 * Konzol parancsoknál a szoba parancs átnevezve csatorna parancsra.
 * Schumix tábla frissítve.
 * Time osztály létrehozva.

## 1.6.0 (2011-02-02)

 * Konfig fájl olvasása átalakítva.
 * Calc, md5, cha1 parancsok javítva.
 * Log level csökkentve kettőre.
 * Network kezelés áthelyezve a SchumixBot-ból a Console-ba.
 * Help parancs fixálva.
 * schumix2, clean parancs hozzáadva.
 * HandleMLeft() függvény fixálva. Nem a LEFT hanem a PART opcode kódhoz lett párosítva.
 * Windows fordítás fixálva.
 * Konzol parancs beírása fixálva. Mostantól nem omlik össze miatta a kód.
 * Másodperc kiírása hozzáadva a Loghoz.
 * Plugin rendszer létrehozva.
 * MySqlEscape hozzáadva a MySql osztályhoz.
 * Irc osztályok örökléses meghívásra lettek átírva.
 * További szerkezeti átalakítások.
 * Takarítás.

## 1.5.0 (2011-01-25)

 * WolframAPI hozzáadva a kódhoz.
 * Mappa szerkezetek átalakításra kerültek.
 * Teljes körű help hozzáadva a parancsokhoz.
 * NickInfo osztály hozzáadva.
 * Sender osztály létrehozve.
 * SendMessage kibővítve több új függvénnyel.

## 1.3.0 (2011-01-23)

 * ChannelInfo class létrehozva.
 * String.Format eltüntetetésre került. A SendMessage, Log és MySql függvényeknél lett alkalmazva ezen átalakítás.
 * Funkció parancs kibővítésre került.
 * Konzole parancsokhoz hozzáadva a funkcio parancs.
 * Konzole parancsokhoz hozzáadva a sys parancs.
 * Reconnect ki-be kapcsolási lehetősége hozzáadva a rendszerhez.
 * Konzol parancsokhoz hozzáadva a hibás részek kiírása.

## 1.2.0 (2011-01-21)

 * Beállítva a program fejlécének a "Schumix2 IRC Bot" szöveg.
 * Konzol parancsok bővitve admin, connect, disconnect, reconnect parancsokkal
 * Uptime mentése hozzáadva a rendszerhez.
 * .gitingore hozzáadva a kódhoz.
 * Csatorna logolásának lehetősége hozzáadva a konfighoz.
 * LogLevel beállításának lehetősége hozzáadva a konfighoz.
 * Vhost használata integrálva a bot.
 * Schumix2, sys és info parancs össze lett olvasztva.
 * Crash javítva.

## 1.1.0 (2011-01-19)

 * A hibás lekérdezések ezentúl kiírásra kerülnek.
 * Mode parancs fixálva.
 * Jelszó aktiválásának jelzése hozzáadva a kódhoz.
 * Az admin jelszavak sha1 titkosítást kaptak.
 * HandleHozzaferes() és HandleUjjelszo() függvények beolvadtak a HandleAdmin()-ba.
 * Az adatbázisban az admin táblában az "ip" oszlop "vhost"-ra át lett nevezve.
 * Admin rendszer szét lett választva adminra és operátorra.
 * UserName hozzáadva a konfig fájlhoz.
 * CNick() függvény létrehozva. A csatornákról lehet vele a Nick nevekre váltani üzenet küldés céljából. (automatikusan vált a függvény közöttük)
 * Logban a függvényekhez hozzá lett adva a Lock. Így nem lehet olyan hogy összefolyik a kiírás.

## 1.0.0 (2011-01-18)

 * TODO fájl hozzáadva.
 * README fájl hozzáadva.
 * Revision lekérdezés eltávolitásra került.
 * Konfig fájl beolvasási rendszere teljesen átalakítva.
 * Parancsok kezelése alapjaitól újra lett írva.
 * schumix4.0.csproj átnevezve schumix.csproj-ra

## 0.1.3 (2010-10-28)

 * A kód hozzá lett adva a githez.
