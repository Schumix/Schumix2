# Schumix2 ChangeLog

## 4.2.0 (2013-xx-xx)

 * CsharpSQLite bekötve a magba. Így teljesen c# kóddal lesz megoldva az sqlite használata.
 * NGettext került felhasználásra a Mono.Unix verzió helyett.
 * Linux csomagoknál mostantól az addonok minden indításkor frissülni fognaka telepített állományból így ami a bothoz tartozik addon az automatikusan frissülni fog.
 * Console osztály parancsai átrendezve és a működési elve felújítva.
 * DebugLog-ba belekerült az sql lekérdezések logolása.
 * Sql lekérdezésnél a SELECT* átírva SELECT 1-re.
 * SqlEscape() függvény feljavítva. Mostantól a MySql-hez az ahhoz beépítettet fogja használni.
 * SchumixListener hozzáadva a bothoz.Feljavításra kerültek a régi hasnoló alapon működő rendszerek.
 * Schumix description frissítve.
 * Mathematics osztály átnevezve Maths-ra.
 * Addonok verziója mostantól meg fog egyezni a mag verziójával.
 * Api beolvasztva a Framework-be.
 * LuaEngine, PythonEngine és Updater összevonva egy projektbe.
 * Komponensek verziója meg fog egyezni ezentúl a rendszer verziójával.
 * Log részek átmozgatva a Logger névtérbe.
 * Release_x64 elméletileg most már teljesen törölve lett a programból.
 * Installer.exe forrásfájlai lefrissítve. Új verzió a 0.0.7. Jelenleg is alpha verziónak tekinthető.
 * CheckAndCreate() függvény megszüntetve a konfig fájlokban.
 * GeneralExtensions.cs fájl szét lett darabólva így könnyebb átlátni.
 * Mac OS X és Linux megkülönböztetése javítva.
 * ChatterBotApi.dll helyett mostantól külső tárolóból lesz elérhető a kódja így rugalmasabban fog illeszkedni a bothoz.
 * NewNick változó áthelyezve a Network osztályba. Így elkerülhető azon bug mely a többszerveres mód után lépett fel.
 * AutoPing() függvény mostantól nem fog feleslegesen hibát okozni.
 * DebugLog mappa létrehozva. Belekerülnek a logba az IrcWrite és IrcRead üzenetek.
 * Runtime osztályban létrehozva az Exit() függvény.
 * birthday parancson is elvégezve minden módosítás ami a calendar parancson is volt.
 * build.bat és build.sh frissítve.
 * Calendar addonban a calendar és a ban parancsoknál alkalmazva az új regex-es működés.
 * IsValueBiggerDateTimeNow() függvény kijavítva.
 * Kick parancs feljavítása elkészült.
 * LogToFile függvény átnevezve LogInFile függvényre.
 * Randomkick funkciónév lecserélve Otherkick-re. Így jobban jelzi amire való.

## 4.1.0 (2013-06-23)

 * NetworkQuit akkor is true értéket kap ezentúl ha egyéb hiba lépett fel a kapcsolatban.
 * IrcBase osztályban létrehozva a LoadProcessMethods() és UnloadProcessMethods(). Így egységesebb lesz a program több része is. Javítva az addonok leválasztása.
 * Runtime osztályhoz hozzáadva a MemorySize és MemorySizeInMB változó
 * Reload részhez hozzáadva az irc rész.
 * Névnapok kibövítésre kerültek az alábbi nyelveken: czCZ, deDE, fiFI, frFR, huHU, lvLV, plPL, seSE
 * CompilerAddon yaml konfig olvasása kijavítva.
 * Mono.Options frissítésre került a mono-ban tárolt verzió alapján.
 * Platform osztályhoz hozzáadva több olyan függvény amivel pontosabban megállapítható az adott rendszer beállításai.
 * Kijavításra került a 64 bites Windows-okon jelentkező indulási hiba. Mostantól Windows alatt csak 32 bites verzióban lehet lefordítani a programot és csak azt szabad elindítani. Persze a 32 bitesnél ez nem feltétlen lényeges viszont a 64 bites verziónál feltétlenül így kell.
 * Konfig újratöltésénél a log fájl módosítás/létrehozás mostantól tökéletesen fog működni.
 * Service nick neveket észleli a bot és másikra vált róla. (Persze csak a 3 névből. De lehet hogy még így se tud olyat találni ami nincs letiltva.)
 * Ha a három névből bármelyik is egyezést mutat akkor a bot figyelmeztetést ír ki és leáll.
 * Service nick neveket ezentúl úgy észleli a bot mintha lefoglalt nickek lenének így képes felcsatlakozni egy másikkal a szerverre.
 * HexChat-ből felhasználva az url regex hogy tökéletes legyen Schumixban is.
 * Maffia játék kicsit szépítve. Mostantól aki meghalt nem fogja tudni a szerepéhez tartozó parancsokat még véletlen se használni.
 * "Reload: " szöveg magyarosítása elkészült.
 * 10 perc után ezentúl a maffiás játék ki fogja vágni az inaktiv játékosokat.
 * LuaInterface lecserélve NLua-ra.
 * Platform osztály létrehozva.
 * Windows 8 felismerése hozzáadva a GetOSName() függvényhez.
 * ngit és YamlDotNet modulként hozzáadva a kódhoz.
 * Bármilyen cpu-n működni fog mostantól a program. Nincs direktbe egyhez se megcsinálva. (AnyCPU)
 * IsEmpty() fügvény helyett a beépített IsNullOrEmpty() függvény lesz használva.
 * Idő formátum kezelése szépítve.
 * Paraméterek megadásának lehetősége átalakítva. A Mono.Options került beépítésre.

## 4.0.0 (2013-03-19)

 * UrlShort beépítve a magba.
 * Almodulként hozzáadva a kódhoz a LuaInterface.
 * Nyelvezetek kezelése teljesen átalakítva. Mostantól jobb megoldással lesz kezelve a konzolos részen.
 * Hozzáadva az irc konfighoz a szerver jelszó és modemask beállításának lehetősége.
 * Sender kibővítve.
 * Schumix.API átnevezve Schumix.Api-ra.
 * Csatornákhoz hozzáadva az elrejtés lehetősége.
 * Admin info parancs átalakítva.
 * Ranggal kapcsolatos hibák javítva. (pl.: GameAddon-ban)
 * Nick név információk letárolva.
 * IsChannel() függvény áthelyezve az Utilities osztályba.
 * Csatorna infomációk egy helyre gyűjtve.
 * Csatorna funkciókat a program ellenőrzi és javítja.
 * ChannelNameList átnevezve ChannelList-re.
 * Sql injection elleni védelem javítva.
 * Changelog hozzáadva a rendszerhez.
 * Rss szálak mostantól le fognak állni ha bizonyos számú error keletkezik. Ezeket csak akkor lehet újraindítani ha a felhasználó indítja újra.
 * Színvak mód hozzáadva a kódhoz.
 * Átnevezésre került a repo tulajdonosa és még egyéb más helyen is a 'megax'.
 * TesztAddon át lett nevezve TestAddon-ra.
 * Doxygen kezelése hozzáadva a kódhoz.
 * Ctcp kijavítva.
 * Arch Linux-ra ezentúl készíthető csomag.
 * Kész a birthday sok idő után. Nagyon örülök neki hogy végre kész :)
 * Memória kiírása átalakítva. Maxinális fogyasztás átalakítva. Így pontosabb értékekhez lesznek kötve és reálisabb a + memória amit több kapcsolat esetén kell beállítani a botnak.
 * Calendar-ban a Loop rész ellenőrzése javítva.
 * Compiler végzetes hibája javítva.
 * Több helyen alkalmazásra került az IsChannel függvény. Ha már van akkor használni kell :)
 * IsCreatedTable függvény létrehozva. Így ellenőrizhető lesz hogy egy tábla létezik-e vagy sem.
 * GetUrlEncoding() függvények össze lettek olvasztva. Úgyan azt a feladatot így csak egy helyen kell megadni.
 * GetUnixTimeStart() hozzáadva a Libraries-hez.
 * WolframAPI saját izlésre szabva.
 * ConsoleLog ellátva abstract jelzővel.
 * Parse függvény áthelyezve a NickInfo osztályba.
 * Fixalva az alap csatorna sql-ben. CacheDB panaszkodott ra.
 * Whois parancs felújítva.
 * NameList fixálva mindkét helyen.
 * Message parancshoz hozzáadva az unixtime tárolásának lehetősége is.
 * Nextmessage parancs hozzáadva a calendar-hoz.
 * UnixTime oszlop hozzáadva a calendar táblához.
 * UnixTime függvény átalakítva.
 * Windows és debian telepítőben frissítve lett a konfig fájl.
 * Clean mappa hozzáadva a kódhoz.
 * Átláthatobbá téve a reconnect idő.
 * Kapcsolódás típusának kiírása hozzáadva a kódhoz.
 * TrimMessage függvény hozzáadva a kódhoz.
 * Privát logolás újraélesztve.
 * Invite opcode elkészült.
 * Uptime táblában ezentúl byte-ban lesznek megadva a mentett memória méretek.
 * Kitakaritva a LueEnginer referencia listája.

## 3.9.0 (2012-06-26)

 * Kész a CacheDB.
 * Több ponton elhelyezésre került a LargeError.
 * ACTION üzenet kezelése megoldva a log fájlokba.
 * Identify hiba javítva Unreal irc szerverekre.
 * GetUrl függvények összevonásra kerültek.
 * Translate regex fixálva.
 * Fixálva a GetUrl függvény kódolási gondja. Mostantól utf8-ra fogja átalakítani a szöveget.
 * Youtube title lekérdezése javítva. Most sokkal gyorsabban letölti és tökéletesebben.
 * Növelve a title idő limitje és a karakterlimit.
 * Network osztály kezdő függvényéből létrehozva több alternativa is.
 * Abstract hozzáadva pár osztályhoz.
 * Xrev elkészült.
 * A csatornákhoz való kapcsolódás javításra került. Eddig előfordulhatott hogy a rendszer nem lépett felrossz jelszó miatt vagy hibás vhost. De olyan is volt hogy egyik se volt fent a szerveren (chanserv, nickserv) így nem lépett fel a bot. Mostantól ilyen nem fordulhat elő.
 * Scripts mappa létrehozva a főkönyvtárban és a python basic script hozzáadva.
 * PythonEngine hozzáadásra került.
 * Magban lévő antiflood beállításai áthelyezve a konfig fájlba.
 * Extra irc logok hozzáadásra kerültek. Mostantól jelezni fogja a join, left, quit, mode, newnick, topic üzeneteket.
 * MaxFileSize és DateFileName áthelyezve konfigba.
 * Max memória rész áthelyezve konfigba.
 * ScriptManager-ben a Lua külön mappát kapott.
 * Youtube title javítva.
 * Debian mappa hozzá lett adva a kódhoz plusz a debian fájlt generáló (.deb) script. Így bármikor lehet csomagolni a botot debian alapú rendszerekre.
 * Dumps mappa kikerült a konfigba.
 * Title kiírásnál mostantól csak 300 karakterig írja ki. Azután 3 pontot tesz.
 * Calendar és Ban parancsok idő érzékelése javítva.
 * IsSpecialDirectory fv hozzáadva a kodhoz.
 * SQLite lesz a konfigban az alapértelmezett adatbázis. (Persze ez módosítható)
 * Windows installer hozzáadva a kódhoz.
 * Fixálva a memória fogyasztás korlátozása. Mostantól leáll a program.
 * Shutdown rész egyesítve Schumixban.
 * Yaml konfig használata hozzáadva a kódhoz.
 * A főbb szálaknak név lett adva.
 * Part függvény átalakítva. Mostantól lehet üzenetett is küldeni vele.
 * Copyright weboldalcím fixálva.
 * Alenah átnevezve Invisible-re.
 * Többszerveres irc csatlakozás létrehozva.
 * Névnapoknál az Apolónia és Örs név javításra került.
 * GoogleWebSearch szétbontásra került.
 * Config.cs szét lett szedve.
 * Amsg megírásra került.
 * Schumix project dokumentációja frissítve.
 * Törlésre került a config adatok Server.exe álltal történő küldése és vissza lett állytva a régi megoldásra.
 * Fixalva az enUS nevnapkiiras.

## 3.8.0 (2012-06-26)

 * Vhost hiba javítva.
 * LuaInterface lib állomanya kicserélésre került.
 * x64-es fordtasi mod hozzaadva az installerhez es az updaterhez.
 * Is64BitOS() függvény hozzáadva.
 * Installer frissitesre kerult.
 * Frissítő mechanizmus át lett alakítva a botban.
 * Frissítés a maffiajátékba.
 * Ctcp-ben a karakterfelismerés pontosítva.
 * Ignore addon parancs hozzá lett adva a kódhoz.
 * WolframAPI fixálva windows-ra is.
 * Windows-os leallitas javitva a botban.
 * Attributum kezels hozzaadva az irc es command parancsokhoz.
 * Kisebb száll pihentetés került be az opcode feldolgozásba.
 * Reconnect rendszer javításra került.
 * Singleton crash hiba oka megtalálva így mostantól nem kell plusz fv ahhoz hogy az eredeti funkciót lehessen használni.
 * AntiFlood lehetőség kibővítve a parancsok floodolásának észlelésével.
 * Compiler enum lecserélve PlatformType-re.
 * Tiltó listánál fixálva a mester csatorna letiltása és a saját nick letiltása.
 * Pkg-hez javítva az addon mappa elérésének beállítása.
 * Ssl kapcsolat javításra került.
 * Javítva a log fájl generálása.
 * Identify és vhost aktiválás átrakva a nickinfo-ba.
 * Pid fájl létrehozása hozzáadva a kódhoz.
 * Icon.co lecserélésre került a Schumix.exe-nél.
 * Makefile hozzáadva a kódhoz.
 * Javítva a helpnél az admin parancsok észlelése és visszajelzése nem adminoknak.
 * Console parancsokhoz hozzáadva a plugin parancs.
 * Hozzáadásra került a nick auto visszavétele 10 perc utána ha szabad.
 * Libraries-ben megcsinálva printf olyanra mint ahogy müködnie kéne neki :)
 * Rss lekérdezéseknél fixálva a memóriafogyasztás.
 * Google parancs javításra került.
 * Translate parancs javításra került.
 * Title kezelés javítva.
 * Compiler error üzenet kiírása egybe lett vonva. A szöveg 1000 karakter után megtörésre kerül és új sorba kerül (globálisan).
 * Minenhol fixálva lett az admin jog ellelnőrzése. Ne lehessen ebből törési lehetőség.
 * Warning parancsban fixálva a csatornára való írás joga.
 * WolframAlpha api key csere lehetőség megoldva. Mostantól a configban lehet beállítani.
 * Chatterbot ékezet dekodolása javítva.
 * Sql adatbázis egységesítve lett.
 * MySql kapcsolat fenttartása fixálva.
 * Notes részbe is beépítve az a megoldás amit az adminoknál használok.
 * Weather parancs javítva.
 * Join és leave parancsoknál fixálva hogy ha fentvan azon a csatornán vagy ha már lelépett, esetleg le van tiltva akkor jöjjön válasz miért nem jelenik meg ott a bot.
 * +i érzékelés a cstornára való fellépésnél hozzáadásra került.

## 3.7.0 (2012-02-05)

 * Szerverbe beépítva a config adatok tárolása újraindításhoz.
 * Wordpress és chatterbot addon hozzá lett adva a kódhoz.
 * Nick csere esetén jön válasz ha sikertelen.
 * Sha1 és md5 fixálva ékezetes karakterekre.
 * Magyar ékezetes karakterek használatára megoldás született windows alatt.
 * Ikonok hozzádva a futtatható fájlokhoz.
 * Calendar loop rész csak adminoknak van engedélyezve.
 * Események feljegyzése hozzáadva a kódhoz.
 * Szerverbe beépítve a 10 percenkénti ellenőrzés hogy újra indítsa-e a botot.
 * Git lista és info szinezése kicsit átalakítva.
 * Játékban javítva a rang kiírása lelépés és egyben halál esetén.
 * Javítás a játékvezető lelépésekor fellépő hibára.
 * A játékba bekerült a no lynch funkció és az este folyamán kikapcsolhatóvá vált a voice jog.
 * Opcodes és Bot parancsok kezelése átírva.
 * Rejoin fixálva nagybetüs csatornákra is.
 * Compiler addonban fixálva a szál kilővése.
 * Fixálva windows alatt az irc üzenetek feldolgozása.
 * LuaEnginer tökéletesítve linuxra.
 * Lekérdezések száma csökkentve.
 * Nem létező funkciók észlelése hozzáadva a kódhoz.
 * Forrásból törölve a MONO kapcsoló.
 * A Szerverrel való kapcsolatbontás javítva.
 * Leállási folyamat javítva az újrakapcsolódással együtt.

## 3.6.0 (2011-10-15)

 * A kód mostantól elkapja a végzetes hibákat és egy fájlba írja a hibát.
 * Játékban fixálva a csatornához tartozó funckiók visszaállítása.
 * A rangelvétel fixálva a játékban.
 * 2 bug fixálva a játékban.
 * Játék magja újra lett részben írva.
 * Message fix.
 * koszones funkció át lett nevezve greeter-re.
 * Xbot és info parancs meglett variálva.
 * Rss-ek mostantól felismerik ha felhasználó kell az rss eléréséhez.
 * Gitweb lekérdezés hozzá lett adva a git rss-hez.
 * Mono-ban a https error megkerülve így nem lesz.
 * Runtime-ben fixálva a szál.
 * Hozzáadva a programhoz azon lehetőség ha túllépi a 100 mb-ot akkor leáll.
 * Ha bármi miatt megszakad a mysql kapcsolat és az a hiba jön rá ami megvan határozva akkor a program leáll. Így megakadájozható a befagyás.
 * Updater rendszer áthelyezve a szerverbe is.
 * Fölösleges szóközök törölve.
 * Fölösleges using törölve.
 * Schumix Szerver hozzáadásra került.
 * Ha eléri a Schumix.log a 10 mb-ot törli a program és újat csinál helyette.
 * Title lekérdezés fixálva.
 * CTCP fixálva.

## 3.5.0 (2011-09-24)

 * Online help parancs hozzáadva.
 * Translate parancs fixálva.
 * Wiki parancs hozzáadva.
 * Funkciók fixálva lettek.
 * MantisBT rss rendszer hozzáadásra került.
 * Online parancs hozzáadásra került.
 * A nick visszavétel még írás alatt van.
 * Whois és warning parancs javítva.
 * CTCP üzenetek kezelése hozzáadva.
 * Reverse() megírva stringra is.
 * SplitToString fv továbbfejlesztve char[]-ra is.
 * Forgatási hiba javítva.
 * Installer hozzáadva a kódhoz.
 * Privmsg és Notice között ezentúl konfigban lehet váltani.
 * Integrálva lett a notice alatt müködő paracssor.
 * Reply kódok hozzáadva az irc részhez.
 * Lua rendszer hozza lett adva a kódhoz.
 * Irc parancs frissitve lett.
 * A roll, sha1, md5 es prime parancs át lett helyezve az extraaddonba.

## 3.4.0 (2011-08-22)

 * Sqlite hiba javítva lett.
 * Notes parancs javítva.
 * Info parancs javítva.
 * Windows fix.
 * Mode parancs fixálva.
 * Köszönések üzenetei át lettek helyezve az adatbázisba.
 * Ssl-en való kapcsolódás hozzáadva a kódhoz.
 * Időjárás fixálva.
 * Youtube title javítva.
 * Updater verziókezelése és kicsomagolás megírva.
 * Google regex fixálva lett.
 * Autofunction parancsban a kick és mode parancs fixálva lett. Help szövegek fixálva.
 * Leállás javítva
 * Rss-be megírva a start, stop, reload parancs.
 * GitRss error szövege fixálva.
 * Compiler addon konfigja frisstve lett.

## 3.3.0 (2011-08-11)

 * Sql-hez hozzáadva az update, delete, insert és removetable függvény.
 * botnic, csc parancs hozzáadva. Ez a compiler használatáról ad bővebb információt.
 * A nem használt addonok help részei nem használhatóak ezentúl az adatbázisból. Csak akkor ha aktívak.
 * GameAddon hozzáadásra került.
 * Compiler tiltások fixálva.
 * Console üzenet javítva.
 * Console kapcsoló javítva.
 * Title lekérdezés fixálva.
 * Mode parancs fixálva.
 * Nagybetüs csatornák fixálva.
 * Időjárás jelentés kiirása átalakítva.
 * Libben a végzetes meghívásí hiba javítva.
 * Uptime tabla frissitve lett. SQLite tablak is frissitesre kerultek de az utf-8 olvasassal van kis gond meg. Regi sql fajlok atlettek helyezve.

## 3.2.0 (2011-07-21)

 * Mag nyelvezeti konzol kiírása teljesen kész.
 * Helyesírási hibák javítva.
 * Commentek elhelyezve Schumix projektben.
 * Uptime nyelvezete fixalva.
 * Induláskor meghatározható milyen legyen a konzol nyelvezete amig nem töltődik be a konfig.
 * Console parancsok ellátva a nyelvválasztás lehetőségével.
 * Reload parancs hozzadava a kodhoz.
 * Encoding fixálva.
 * Elkészült az angol fordítások nagy része.

## 3.1.0 (2011-06-24)

 * Üzenet küldés az irc szerver felé fixálva. Inéttől csak a 2000 karakter alatti üzeneteket küldi el a kód.
 * A compilerben hatalmas átalakításon eset át a végtelen ciklus elleni védelem.
 * Compiler további fixálásokon esett át.
 * SQLite sql fájla hozzáadásra került. Schumix.db3 frissitve lett. Compiler végtelen ciklus elleni védelme finomításra került.
 * Compiler szövegének angolosítása kész.
 * Compiler végtelen ciklus elleni védelem továbbfejlesztve.
 * Console encoding beállításának lehetősége megírva. Alapértelmezés utf-8.
 * Nick letiltásának lehetősége hozzáadva a konfighoz.
 * A compiler fugvenyei mostantol hasznaljak a magot is igy a dll-hez kelleni fog az is ha mas celokra lenne felhasznalva.
 * Windows hibak egyresze javitva.
 * Compiler frissitésre került. Loop elleni védelem tökéletesítve lett de még fordulhat elő benne hiba.
 * Compiler beüzemelése kibővítve. Ezentúl a schumix2, (persze ez mindig változik nick alapján ahogy szokott induláskor) is elérhető.
 * Fixálások a kódban.
 * Utilities bővitve.
 * Config betöltése fixálva. Ha valami okból hiányzik az xml-ből valamilyen adat akkor az alapértelmezettet használja a kód.
 * A main fv-ben hozzáadásra került a config fájl és a mappájának nevének módosítása.

## 3.0.0 (2011-05-18)

 * Revision addon hozzáadva de még fejlesztve lesz.
 * Többfunkciós nyelvi használat lehetősége hozzáadva.
 * Verziók óta hordozott híbák javítva.
 * MySql-hez hozzáadva a charset modósításának lehetősége.
 * Minden Irc opcode ami jön új szálra kerül.
 * Uzenet parancs hozzáadva a kódhoz és a régi átnevezve.
 * UserInfo és IgnoreChannel hozzáadva a konfighoz.
 * Caps lock elleni védelem hozzáadva.
 * CNick() fv helye fixálva az függvényeken belül.
 * Nyelvtani hibák kijavítva
 * Compiler egyes adatai áthelyezve konfigba.
 * Calendar bővitve flood elleni védelemmel és bannal.
 * Console parancsokhoz elkészült a help rész.
 * CalendarAddon hozzáadásra került. Egyenlőre még üres.
 * Url title kiirása hozzáadva kódhoz.
 * Fölös using törölve.
 * Végzetes hiba javítva.
 * Legtöbb Assembly információ egy helyre került.
 * Csatorna neve fixálva lett. Mostantól bármilyen formában kezeli a kód. Egyéb javítások.

## 2.9.0 (2011-04-23)

 * Asmg és Me hozzáadva az üzenet küldéshez. De még nem üzemel mert nincs meg a strukturája.
 * Végtelen ciklus elleni védelem bővitve.
 * Compiler ellátva végtelen ciklus elleni védelemmel.
 * Végzetes hiba javítva.
 * SplitToString fv hozzáadva a kóhoz. IsNull() fv használatba véve.
 * SvnRss indulási lekérdezése fixálva.
 * Compiler addonhoz hozzáadva a konfigurálás lehetősége.
 * Compiler rész fixálva lett.
 * Jegyzet parancs végre hozzá lett adva a kódhoz. Remélem elnyeri mindenki tetszését.
 * Config fájl szerkezete megigazítva.
 * GitRss addon hozzáadásra került.

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

## 0.2.0 (2010-10-29)

 * Köszönés alapértelmezetten bekapcsolva lesz.

## 0.1.3 (2010-10-28)

 * A kód hozzá lett adva a githez.
