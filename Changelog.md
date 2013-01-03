# Schumix2 ChangeLog

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
