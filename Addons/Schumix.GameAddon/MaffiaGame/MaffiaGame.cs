/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Megax <http://www.megaxx.info/>
 * 
 * Schumix is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * Schumix is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Schumix.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Threading;
using System.Collections.Generic;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.GameAddon;

namespace Schumix.GameAddon.KillerGames
{
	public sealed partial class KillerGame
	{
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly Dictionary<int, string> _playerlist = new Dictionary<int, string>();
		private readonly Dictionary<string, string> _detectivelist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _killerlist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _doctorlist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _normallist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _ghostlist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _lynchlist = new Dictionary<string, string>();
		private string _owner;
		private string _channel;
		private string newghost;
		private Thread _thread;
		private bool _day;
		private bool _stop;
		private bool _killer;
		private bool _detective;
		private bool _joinstop;
		private int _lynchmaxnumber;
		private string _rank;
		private int _players;
		private string killer_;
		private string detective_;
		public string GetOwner() { return _owner; }
		public string GetKiller() { return killer_; }
		public string GetDetective() { return detective_; }
		public bool Started { get; private set; }
		public Dictionary<int, string> GetPlayerList() { return _playerlist; }
		public Dictionary<string, string> GetDetectiveList() { return _detectivelist; }
		public Dictionary<string, string> GetKillerList() { return _killerlist; }
		public Dictionary<string, string> GetNormalList() { return _normallist; }

		public KillerGame(string Name, string Channel)
		{
			_day = false;
			_stop = false;
			_killer = false;
			_detective = false;
			_joinstop = false;
			_players = 0;
			_lynchmaxnumber = 0;
			Started = false;
			_owner = Name;
			_rank = string.Empty;
			killer_ = string.Empty;
			detective_ = string.Empty;
			_channel = Channel.ToLower();
			_playerlist.Add(1, Name);
			sSendMessage.SendCMPrivmsg(_channel, "{0} új játékot indított. Csatlakozni a '!join' paranccsal tudtok.", _owner);
			sSendMessage.SendCMPrivmsg(_channel, "{0}: Írd be a '!start' parancsot, ha mindenki készen áll.", _owner);
		}

		public void NewGame(string Name, string Channel)
		{
			_day = false;
			_stop = false;
			_killer = false;
			_detective = false;
			_joinstop = false;
			_players = 0;
			_lynchmaxnumber = 0;
			Started = false;
			_owner = Name;
			_rank = string.Empty;
			killer_ = string.Empty;
			detective_ = string.Empty;
			_channel = Channel.ToLower();
			_playerlist.Add(1, Name);
			sSendMessage.SendCMPrivmsg(_channel, "{0} új játékot indított. Csatlakozni a '!join' paranccsal tudtok.", _owner);
			sSendMessage.SendCMPrivmsg(_channel, "{0}: Írd be a '!start' parancsot, ha mindenki készen áll.", _owner);
		}

		private void StartThread()
		{
			_thread = new Thread(Game);
			_thread.Start();
		}

		public void StopThread()
		{
			SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", GameAddon.GameChannelFunction[_channel], _channel);
			sChannelInfo.ChannelFunctionReload();
			_thread.Abort();
			_playerlist.Clear();
			_detectivelist.Clear();
			_killerlist.Clear();
			_doctorlist.Clear();
			_normallist.Clear();
			_ghostlist.Clear();
			GameAddon.GameChannelFunction.Remove(_channel);
		}

		private void Game()
		{
			if(_players < 8)
				sSendMessage.SendCMPrivmsg(_channel, "Nincs elég játékos két gyilkoshoz, csak egy gyilkos van játékban (illetve nincs orvos).");
			else if(_players >= 8)
				sSendMessage.SendCMPrivmsg(_channel, "Mivel legalább 8 játékos van, ezért 2 gyilkos és egy orvos lesz.");

			sSendMessage.SendCMPrivmsg(_channel, "Itt mindenki egyszerű civilnek tűnhet, de valójában köztetek van 1 vagy 2 4gyilkos, akiknek célja mindenkit megölni az éj leple alatt.");
			sSendMessage.SendCMPrivmsg(_channel, "Köztetek van egy 4nyomozó is: ő képes éjszakánként megtudni 1-1 emberről, hogy gyilkos-e, és lebuktatni őt a falusiak előtt, illetve a falu 4orvosa, aki minden éjjel megmenthet valakit...");
			sSendMessage.SendCMPrivmsg(_channel, "A csoport célja tehát lebuktatni és meglincselni a gyilkos(oka)t, mielőtt mindenkit megölnek álmukban.");

			string names = string.Empty;
			foreach(var name in _playerlist)
				names += ", " + name.Value;

			Thread.Sleep(2000);

			for(;;)
			{
				Thread.Sleep(1000);

				if(_players < 8 && _killer && _detective)
				{
					if(_killerlist.ContainsKey(newghost.ToLower()))
						_killerlist.Remove(newghost.ToLower());
					else if(_detectivelist.ContainsKey(newghost.ToLower()))
						_detectivelist.Remove(newghost.ToLower());
					else if(_normallist.ContainsKey(newghost.ToLower()))
						_normallist.Remove(newghost.ToLower());

					int i = 0;
					foreach(var player in _playerlist)
					{
						if(player.Value == newghost)
						{
							i = player.Key;
							break;
						}
					}

					_playerlist.Remove(i);
					_ghostlist.Add(newghost.ToLower(), newghost);

					_day = true;
					_stop = false;
					_killer = false;
					_detective = false;
					sSender.Mode(_channel, "-v", newghost);
					sSendMessage.SendCMPrivmsg(newghost, "Meghaltál. Kérlek maradj csendben amíg a játék véget ér.");
				}

				EndGame();

				if(!Started)
					StopThread();

				if(!_day)
				{
					if(_stop)
						continue;

					_stop = true;
					_lynchlist.Clear();
					names = string.Empty;

					foreach(var name in _playerlist)
						names += ", " + name.Value;

					sSendMessage.SendCMPrivmsg(_channel, "A következő személyek vannak még életben: {0}", names.Remove(0, 2, ", "));
					sSendMessage.SendCMPrivmsg(_channel, "Leszállt az 4éj.");
					sSendMessage.SendCMPrivmsg(_channel, "Az összes civil békésen szundikál...");
					Thread.Sleep(1000);

					if(_players < 8)
					{
						foreach(var name in _killerlist)
						{
							sSendMessage.SendCMPrivmsg(name.Key, "Miközben a falusiak alszanak, te eldöntöd, hogy kit ölsz meg az éj leple alatt.");
							sSendMessage.SendCMPrivmsg(name.Key, "Te és a másik gyilkos (ha létezik, és él egyáltalán) meg fogjátok vitatni (PM-ben), hogy ki legyen az áldozat.");
							sSendMessage.SendCMPrivmsg(name.Key, "Írd be PM-ként nekem: '!kill <nickname>'");
							Thread.Sleep(400);
						}
					}
					else
					{
						foreach(var name in _killerlist)
						{
							sSendMessage.SendCMPrivmsg(name.Key, "");
							Thread.Sleep(400);
						}

						foreach(var name in _doctorlist)
						{
							sSendMessage.SendCMPrivmsg(name.Key, "");
							Thread.Sleep(400);
						}
					}

					foreach(var name in _detectivelist)
					{
						sSendMessage.SendCMPrivmsg(name.Key, "A te dolgod megtudni egyes emberekről, hogy gyilkosok-e.");
						sSendMessage.SendCMPrivmsg(name.Key, "Most kell eldöntened kit kövess éjszaka: írd be PM-ként nekem: '!see <nickname>'. Így megtudhatod, ki is ő valójában.");
						Thread.Sleep(400);
					}
				}
				else
				{
					if(_stop)
						continue;

					_stop = true;
					sSendMessage.SendCMPrivmsg(_channel, "Felkelt a nap!");
					sSendMessage.SendCMPrivmsg(_channel, "A falusiakat szörnyű látvány fogadja: megtalálták 4{0} holttestét!", newghost);

					if(_rank == "killer")
						sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy 4gyilkos volt.");
					else if(_rank == "detective")
						sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy 4nyomozó volt.");
					else if(_rank == "normalman")
						sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy egy ártatlan falusi volt.");

					sSendMessage.SendCMPrivmsg(_channel, "({0} meghalt, és nem szólhat hozzá a játékhoz.)", newghost);

					names = string.Empty;
					foreach(var name in _playerlist)
						names += ", " + name.Value;

					sSendMessage.SendCMPrivmsg(_channel, "A következő személyek vannak még életben: {0}", names.Remove(0, 2, ", "));

					names = string.Empty;
					foreach(var name in _ghostlist)
						names += ", " + name.Value;

					sSendMessage.SendCMPrivmsg(_channel, "A következő személyek halottak: {0}", names.Remove(0, 2, ", "));
					sSendMessage.SendCMPrivmsg(_channel, "Felkelt a nap... A falusiak kirohannak a főtérre, hogy megvitassák, ki lehet a gyilkos.");
					sSendMessage.SendCMPrivmsg(_channel, "A falusiaknak el *kell* dönteniük, hogy kit lincseljenek meg.");
					sSendMessage.SendCMPrivmsg(_channel, "Ha mindenki készen áll, írjátok be: '!lynch <nickname>',");
					sSendMessage.SendCMPrivmsg(_channel, "Összeszámolom a szavazatokat, és a döntő többség szava fog érvényesülni.");
					sSendMessage.SendCMPrivmsg(_channel, "Megjegyzés: a szavazatokat bármikor meg lehet változtatni.");
				}
			}
		}

		private void EndGame()
		{
			if(_killerlist.Count == 0)
			{
				foreach(var end in _playerlist)
					sSender.Mode(_channel, "-v", end.Value);

				sSender.Mode(_channel, "-m");
				sSendMessage.SendCMPrivmsg(_channel, "A gyilkosok halottak! A falusiak győztek.");
				sSendMessage.SendCMPrivmsg(_channel, "A játék befejeződött.");
				sSendMessage.SendCMPrivmsg(_channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig None. Mindenki más hétköznapi civil volt.", killer_, detective_);
				StopThread();
			}
			else
			{
				if(_playerlist.Count == 2)
				{
					if(_players < 8)
					{
						foreach(var end in _playerlist)
							sSender.Mode(_channel, "-v", end.Value);
	
						sSender.Mode(_channel, "-m");
						StopThread();
						sSendMessage.SendCMPrivmsg(_channel, "A falusiak halottak! A 4gyilkosok győztek.");
						sSendMessage.SendCMPrivmsg(_channel, "A játék befejeződött.");
						sSendMessage.SendCMPrivmsg(_channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig None. Mindenki más hétköznapi civil volt.", killer_, detective_);
						StopThread();
					}
				}
			}
		}
/*<SzEMBot> esafrany arra szavazott, hogy BNGY legyen meglincselve!
<SzEMBot> A többség BNGY lincselése mellett döntött! Elszabadulnak az indulatok.  Ő mostantól már halott.
<SzEMBot> *** A holttest megvizsgálása után kiderült, hogy gyilkos volt!
<SzEMBot> A gyilkosok halottak!  A falusiak győztek.
<SzEMBot> A játék befejeződött.
<SzEMBot> *** A gyilkos BNGY, a nyomozó esafrany, az orvos pedig None. Mindenki más hétköznapi civil volt.*/
/*<SzEMBot> BNGY új játékot indított; Csatlakozni a '!join' paranccsal tudtok.
* SzEMBot voice jogot ad ennek: BNGY
<SzEMBot> BNGY: Írd be a '!start' parancsot, ha mindenki készen áll.
<Megaxxx> !join
<BNGY> Borovszky, elég bénán terelsz.
* SzEMBot voice jogot ad ennek: Megaxxx
<SzEMBot> Megaxxx: Bekerültél a játékba!
<rushofhungary> vazul se
<rushofhungary> !join
<Megaxxx> !join
* SzEMBot voice jogot ad ennek: rushofhungary
<Vazul> !join
<SzEMBot> rushofhungary: Bekerültél a játékba!
<SzEMBot> Megaxxx: Már játékban vagy!
* SzEMBot voice jogot ad ennek: Vazul
<SzEMBot> Vazul: Bekerültél a játékba!
<Auryn> !join
<SzEMBot> Auryn: Bekerültél a játékba!
<rushofhungary> gabesz24 ird azt hogy !join
* SzEMBot voice jogot ad ennek: Auryn
* franky88 (~Mibbit@Rizon-98EABDFA.flexiton.hu) kilépett innen: #szem
<Borovszky> BNGY mit akarsz mikor a nyomozo lebuktatott es cseszhettem az egeszet mert rush is tudta jol
<Borovszky> !join
<bundas_afk> !join
<SzEMBot> Borovszky: Bekerültél a játékba!
* SzEMBot voice jogot ad ennek: Borovszky
* SzEMBot voice jogot ad ennek: bundas_afk
<SzEMBot> bundas_afk: Bekerültél a játékba!
<BNGY> csak szarul tereltél
<Borovszky> hiaba gyozom meg Vazul-t ugyis max 2-2
<bundas_afk> gabesz24 ird azt hogy !join
<gabesz24> köszi a meghívást, de én passzolnék :)
<rushofhungary> onan tudtam hogy 
<gabesz24> bb
* gabesz24 (cgiirc@Rizon-4E81DE1C.catv.broadband.hu) kilépett innen: #szem
<rushofhungary> borovszky már rég játék kezdés után irta hogy !join
<eRepublik> :: NEWS :: A resistance has started in Ionian Islands ::
<rushofhungary> ebböl sejtettem
<rushofhungary> egy kell még
<Megaxxx> jaja
<rushofhungary> bngy hivj be valakit
<Henya> !join
<SzEMBot> Henya: Bekerültél a játékba!
* SzEMBot voice jogot ad ennek: Henya
<Megaxxx> lett :D
<Borovszky> !join
<rushofhungary> indulhat
<SzEMBot> Borovszky: Már játékban vagy!
<eRepublik> :: NEWS :: Piedmont was conquered by Resistance force of Italy in the war versus Republic of Macedonia (FYROM) ::
<rushofhungary> .lp
<eRepublik> No citizen found linked to your nick. To link one type: .register_citizen <citizen name>
* JohnT (cgiirc@Rizon-C0281512.dsl.pool.telekom.hu) csatlakozott ide: #szem
<Borovszky> rushofhungary csak veletlenul irtam azt a joint :D
<JohnT> hai
<rushofhungary> johnt !join
<JohnT> !join
* SzEMBot voice jogot ad ennek: JohnT
<SzEMBot> JohnT: Bekerültél a játékba!
<rushofhungary> .register_citizen rush of hungary
<eRepublik> rushofhungary: registered eRepublik citizen Rush of Hungary
<Bence_Mate> !join
<SzEMBot> Bence_Mate: Bekerültél a játékba!
* SzEMBot voice jogot ad ennek: Bence_Mate
<rushofhungary> de nem baj
<Bence_Mate> háhháá
<Bence_Mate> Peeteee: 
<Bence_Mate> megöllek
<rushofhungary> akkor véletlen volt:D
<rushofhungary>  bngy !start
<rushofhungary> !start
<BNGY> !start
<SzEMBot> A játékot BNGY indította!
* SzEMBot beállítja a(z) +m #szem módot
<SzEMBot> Új játék lett indítva! Most mindenki megkapja a szerepét.
<SzEMBot> Mivel legalább 8 játékos van, ezért 2 gyilkos és egy orvos lesz.
<rushofhungary> peeteee nem játszik
<SzEMBot> Itt mindenki egyszerű civilnek tűnhet, de valójában köztetek van 1 vagy 2 gyilkos, akiknek célja mindenkit megölni az éj leple alatt.
<SzEMBot> Köztetek van egy nyomozó is: ő képes éjszakánként megtudni 1-1 emberről, hogy gyilkos-e, és lebuktatni őt a falusiak előtt, illetve a falu orvosa, aki minden éjjel megmenthet valakit...
<SzEMBot> A csoport célja tehát lebuktatni és meglincselni a gyilkos(oka)t, mielőtt mindenkit megölnek álmukban.
<SzEMBot> A következő személyek vannak még életben: BNGY, Megaxxx, rushofhungary, Vazul, Auryn, Borovszky, bundas_afk, Henya, JohnT, Bence_Mate
<SzEMBot> Leszállt az éj.
<SzEMBot> Az összes civil békésen szundikál...
<Borovszky> civil
<rushofhungary> nyomozó
<Bence_Mate> akkor aludj
<Borovszky> ha nem horkolna valaki ugy akk aludnak is ...
<Henya> xd
* SzEMBot elvette a voice jogot ettől: Bence_Mate
<SzEMBot> Felkelt a nap!
<SzEMBot> A falusiakat szörnyű látvány fogadja: megtalálták Bence_Mate holttestét!
<SzEMBot> *** A holttest megvizsgálása után kiderült, hogy egy ártatlan falusi volt.
<SzEMBot> (Bence_Mate meghalt, és nem szólhat hozzá a játékhoz.)
<SzEMBot> A következő személyek vannak még életben: BNGY, Megaxxx, rushofhungary, Vazul, Auryn, Borovszky, bundas_afk, Henya, JohnT
<SzEMBot> A következő személyek halottak : Bence_Mate
<SzEMBot> Felkelt a nap... A falusiak kirohannak a főtérre, hogy megvitassák, ki lehet a gyilkos.
<SzEMBot> A falusiaknak el *kell* dönteniük, hogy kit lincseljenek meg.
<SzEMBot> Ha mindenki készen áll, írjátok be:  'lynch <nickname>',
<SzEMBot> Összeszámolom a szavazatokat, és a döntő többség szava fog érvényesülni.
<SzEMBot> Megjegyzés: a szavazatokat bármikor meg lehet változtatni.
<Henya> !lynch megaxxx
<SzEMBot> Henya arra szavazott, hogy Megaxxx legyen meglincselve!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Megaxxx : 1 szavazat)
<Megaxxx> henrik
<Megaxxx> miért engem direkt?
<rushofhungary> auryn falusi
<BNGY> !lynch borovszky
<SzEMBot> BNGY arra szavazott, hogy Borovszky legyen meglincselve!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Megaxxx : 1 szavazat) (Borovszky : 1 szavazat)
<Megaxxx> !lynch henya
<SzEMBot> Megaxxx arra szavazott, hogy Henya legyen meglincselve!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Megaxxx : 1 szavazat) (Henya : 1 szavazat) (Borovszky : 1 szavazat)
<Megaxxx> ha már rámraktad... :D
<rushofhungary> !lynch henya
<Vazul> !lynch Borovszky
<SzEMBot> rushofhungary arra szavazott, hogy Henya legyen meglincselve!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Megaxxx : 1 szavazat) (Henya : 2 szavazat) (Borovszky : 1 szavazat)
<SzEMBot> Vazul arra szavazott, hogy Borovszky legyen meglincselve!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Megaxxx : 1 szavazat) (Henya : 2 szavazat) (Borovszky : 2 szavazat)
<bundas_afk> !lynch henya
<SzEMBot> bundas_afk arra szavazott, hogy Henya legyen meglincselve!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Megaxxx : 1 szavazat) (Henya : 3 szavazat) (Borovszky : 2 szavazat)
<JohnT> !lynch henya
<SzEMBot> JohnT arra szavazott, hogy Henya legyen meglincselve!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Megaxxx : 1 szavazat) (Henya : 4 szavazat) (Borovszky : 2 szavazat)
<BNGY> !lynch henya
* SzEMBot elvette a voice jogot ettől: Henya
<SzEMBot> BNGY arra szavazott, hogy Henya legyen meglincselve!
<SzEMBot> A többség Henya lincselése mellett döntött! Elszabadulnak az indulatok.  Ő mostantól már halott.
<SzEMBot> *** A holttest megvizsgálása után kiderült, hogy egy ártatlan falusi volt.
<SzEMBot> (Henya meghalt, és nem szólhat hozzá a játékhoz.)
<SzEMBot> A következő személyek vannak még életben: BNGY, Megaxxx, rushofhungary, Vazul, Auryn, Borovszky, bundas_afk, JohnT
<BNGY> :s
<SzEMBot> A következő személyek halottak : Bence_Mate, Henya
<SzEMBot> Leszállt az éj.
<SzEMBot> Az összes civil békésen szundikál...
<BNGY> !fail
<Megaxxx> az
<Megaxxx> én azért tettem rá mert szopat
<SzEMBot> ▄██████████████▄▐█▄▄▄▄█▌
<SzEMBot> ██████▌▄▌▄▐▐▌███▌▀▀██▀▀
<SzEMBot> ████▄█▌▄▌▄▐▐▌▀███▄▄█▌
<SzEMBot> ▄▄▄▄▄██████████████▀
<Megaxxx> nem gondoltam hogy kiteszitek
<Megaxxx> =/
<Vazul> aludjatok mán
<eRepublik> :: NEWS :: A resistance has started in Lazio ::
<rushofhungary> bngy falusi
<Borovszky> Vazul inkabb gyilkolj mar
<SzEMBot> Felkelt a nap!
<SzEMBot> Senki sem halt meg.
<SzEMBot> A következő személyek vannak még életben: BNGY, Megaxxx, rushofhungary, Vazul, Auryn, Borovszky, bundas_afk, JohnT
<SzEMBot> A következő személyek halottak : Bence_Mate, Henya
<SzEMBot> Felkelt a nap... A falusiak kirohannak a főtérre, hogy megvitassák, ki lehet a gyilkos.
<Vazul> :D
<SzEMBot> A falusiaknak el *kell* dönteniük, hogy kit lincseljenek meg.
<SzEMBot> Ha mindenki készen áll, írjátok be:  'lynch <nickname>',
<SzEMBot> Összeszámolom a szavazatokat, és a döntő többség szava fog érvényesülni.
<SzEMBot> Megjegyzés: a szavazatokat bármikor meg lehet változtatni.
<Vazul> Ok mindjárt
<Borovszky> !lynch Vazul
<SzEMBot> Borovszky arra szavazott, hogy Vazul legyen meglincselve!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Vazul : 1 szavazat)
<Megaxxx> !lynch Vazul
<SzEMBot> Megaxxx arra szavazott, hogy Vazul legyen meglincselve!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Vazul : 2 szavazat)
<rushofhungary> !lynch vazul
<SzEMBot> rushofhungary arra szavazott, hogy Vazul legyen meglincselve!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Vazul : 3 szavazat)
<BNGY> !lynch borovszky
<SzEMBot> BNGY arra szavazott, hogy Borovszky legyen meglincselve!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Borovszky : 1 szavazat) (Vazul : 3 szavazat)
<JohnT> !lynch vazul
<Vazul> !lynch Borovszky
<SzEMBot> JohnT arra szavazott, hogy Vazul legyen meglincselve!
<Borovszky> CSORDAAAA!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Borovszky : 1 szavazat) (Vazul : 4 szavazat)
<SzEMBot> Vazul arra szavazott, hogy Borovszky legyen meglincselve!
<SzEMBot>  5 szavazat kell a többséghez. Jelenlegi szavazatok: (Borovszky : 2 szavazat) (Vazul : 4 szavazat)
<bundas_afk> !lynch vazul
* SzEMBot elvette a voice jogot ettől: Vazul
<SzEMBot> bundas_afk arra szavazott, hogy Vazul legyen meglincselve!
<SzEMBot> A többség Vazul lincselése mellett döntött! Elszabadulnak az indulatok.  Ő mostantól már halott.
<SzEMBot> *** A holttest megvizsgálása után kiderült, hogy egy ártatlan falusi volt.
<SzEMBot> (Vazul meghalt, és nem szólhat hozzá a játékhoz.)
<Auryn> én a gyilkosok helyében megvárnám amíg rushofhungary éjszaka kiadja az infóit
<SzEMBot> A következő személyek vannak még életben: BNGY, Megaxxx, rushofhungary, Auryn, Borovszky, bundas_afk, JohnT
<SzEMBot> A következő személyek halottak : Bence_Mate, Henya, Vazul
<SzEMBot> Leszállt az éj.
<SzEMBot> Az összes civil békésen szundikál...
<BNGY> csorda múúúúúúúúúú
<Borovszky> :(
<Auryn> le tudjátok szűkíteni az orvosra ;)
<Megaxxx> áááá
<Borovszky> na akk BNGY rush es en tuti tisztak vagyunk
<Auryn> meg én is
<rushofhungary> johnt az
<Auryn> 15:43 rushofhungary auryn falusi
<Auryn> !lynch johnt
<SzEMBot> Auryn: Lincselés csak nappal történhet.
<Borovszky> marmint gyilkos ?
<Borovszky> vagy Johnt is az ? (azaz civil ) :D
<rushofhungary> johnt a gyilkos
<Borovszky> oke
<BNGY> akk maradt bundas_afk megaxxx és borovszky
<Borovszky> igy mar ertem
<Borovszky> tehat bundas es megan
<Borovszky> *megax
<bundas_afk> orvos vagyok én védteem meg rushot
<BNGY> bundas, megax, vagy te
<Megaxxx> én nem vagyok gyilkos
<Borovszky> akk megaxxx es johnt
* SzEMBot elvette a voice jogot ettől: BNGY
<SzEMBot> Felkelt a nap!
<SzEMBot> A falusiakat szörnyű látvány fogadja: megtalálták BNGY holttestét!
<SzEMBot> *** A holttest megvizsgálása után kiderült, hogy egy ártatlan falusi volt.
<SzEMBot> (BNGY meghalt, és nem szólhat hozzá a játékhoz.)
<SzEMBot> A következő személyek vannak még életben: Megaxxx, rushofhungary, Auryn, Borovszky, bundas_afk, JohnT
<SzEMBot> A következő személyek halottak : Bence_Mate, Henya, Vazul, BNGY
<Auryn> lehet hogy hazudik
<SzEMBot> Felkelt a nap... A falusiak kirohannak a főtérre, hogy megvitassák, ki lehet a gyilkos.
<SzEMBot> A falusiaknak el *kell* dönteniük, hogy kit lincseljenek meg.
<SzEMBot> Ha mindenki készen áll, írjátok be:  'lynch <nickname>',
<SzEMBot> Összeszámolom a szavazatokat, és a döntő többség szava fog érvényesülni.
<SzEMBot> Megjegyzés: a szavazatokat bármikor meg lehet változtatni.
<Borovszky> !lynch Johnt
<SzEMBot> Borovszky arra szavazott, hogy JohnT legyen meglincselve!
<rushofhungary> !lynch johnt
<JohnT> gyilkos vagyok :D
<SzEMBot>  4 szavazat kell a többséghez. Jelenlegi szavazatok: (JohnT : 1 szavazat)
<SzEMBot> rushofhungary arra szavazott, hogy JohnT legyen meglincselve!
<SzEMBot>  4 szavazat kell a többséghez. Jelenlegi szavazatok: (JohnT : 2 szavazat)
<Megaxxx> !lynch Johnt
<SzEMBot> Megaxxx arra szavazott, hogy JohnT legyen meglincselve!
<SzEMBot>  4 szavazat kell a többséghez. Jelenlegi szavazatok: (JohnT : 3 szavazat)
<bundas_afk> !lynch johnt
<SzEMBot> bundas_afk arra szavazott, hogy JohnT legyen meglincselve!
<Auryn> !lynch johnt
* SzEMBot elvette a voice jogot ettől: JohnT
<SzEMBot> A többség JohnT lincselése mellett döntött! Elszabadulnak az indulatok.  Ő mostantól már halott.
<SzEMBot> *** A holttest megvizsgálása után kiderült, hogy gyilkos volt!
<SzEMBot> (JohnT meghalt, és nem szólhat hozzá a játékhoz.)
<SzEMBot> A következő személyek vannak még életben: Megaxxx, rushofhungary, Auryn, Borovszky, bundas_afk
<SzEMBot> A következő személyek halottak : Bence_Mate, Henya, Vazul, BNGY, JohnT
<SzEMBot> Leszállt az éj.
<SzEMBot> Az összes civil békésen szundikál...
<SzEMBot> Auryn: Lincselés csak nappal történhet.
<Borovszky> nyomozo bácsi nézzen meg engem
<Borovszky> :D
<Auryn> inkább Megaxxx-t :P
* SzEMBot elvette a voice jogot ettől: Auryn
<SzEMBot> Felkelt a nap!
<SzEMBot> A falusiakat szörnyű látvány fogadja: megtalálták Auryn holttestét!
<SzEMBot> *** A holttest megvizsgálása után kiderült, hogy egy ártatlan falusi volt.
<SzEMBot> (Auryn meghalt, és nem szólhat hozzá a játékhoz.)
<SzEMBot> A következő személyek vannak még életben: Megaxxx, rushofhungary, Borovszky, bundas_afk
<rushofhungary> megaxxx az
<SzEMBot> A következő személyek halottak : Bence_Mate, Henya, Vazul, BNGY, JohnT, Auryn
<SzEMBot> Felkelt a nap... A falusiak kirohannak a főtérre, hogy megvitassák, ki lehet a gyilkos.
<SzEMBot> A falusiaknak el *kell* dönteniük, hogy kit lincseljenek meg.
<SzEMBot> Ha mindenki készen áll, írjátok be:  'lynch <nickname>',
<Megaxxx> engem?
<SzEMBot> Összeszámolom a szavazatokat, és a döntő többség szava fog érvényesülni.
<SzEMBot> Megjegyzés: a szavazatokat bármikor meg lehet változtatni.
<rushofhungary> !lynch megaxxx
<Megaxxx> nem vagyok gyilkos
<SzEMBot> rushofhungary arra szavazott, hogy Megaxxx legyen meglincselve!
<Megaxxx> ...
<Borovszky> !lynch megaxxx
<SzEMBot>  3 szavazat kell a többséghez. Jelenlegi szavazatok: (Megaxxx : 1 szavazat)
<SzEMBot> Borovszky arra szavazott, hogy Megaxxx legyen meglincselve!
<SzEMBot>  3 szavazat kell a többséghez. Jelenlegi szavazatok: (Megaxxx : 2 szavazat)
<bundas_afk> !lynch megaxxx
* SzEMBot elvette a voice jogot ettől: Megaxxx
* SzEMBot beállítja a(z) -m #szem módot
* SzEMBot elvette a voice jogot ettől: Borovszky rushofhungary bundas_afk
<SzEMBot> bundas_afk arra szavazott, hogy Megaxxx legyen meglincselve!
<SzEMBot> A többség Megaxxx lincselése mellett döntött! Elszabadulnak az indulatok.  Ő mostantól már halott.
<SzEMBot> *** A holttest megvizsgálása után kiderült, hogy gyilkos volt!
<SzEMBot> A gyilkosok halottak!  A falusiak győztek.
<SzEMBot> A játék befejeződött.
<Megaxxx> :D
<SzEMBot> *** A két gyilkos Megaxxx és JohnT volt, a nyomozó rushofhungary, az orvos pedig bundas_afk. Mindenki más hétköznapi civil volt.
<JohnT> baze
<Bence_Mate> !start
<SzEMBot> Bence_Mate új játékot indított; Csatlakozni a '!join' paranccsal tudtok.
<JohnT> ki dolgozott ilyen jól? :D
<Bence_Mate> parasztok
* SzEMBot voice jogot ad ennek: Bence_Mate*/

/*
<SzEMBot> Auryn eltűnt egy különös féreglyukban.
<SzEMBot> Auryn -nak unalmas szerepe volt a játékban, mint civil. Remélhetőleg halála izgalmasabb lesz.*/
/*<SzEMBot> Domokos arra szavazott, hogy esafrany legyen meglincselve!
<SzEMBot> A többség esafrany lincselése mellett döntött! Elszabadulnak az indulatok.  Ő mostantól már halott.
<SzEMBot> *** A holttest megvizsgálása után kiderült, hogy gyilkos volt!
<SzEMBot> A gyilkosok halottak!  A falusiak győztek.
<SzEMBot> A játék befejeződött.
<SzEMBot> *** A két gyilkos esafrany és Kinti volt, a nyomozó Feherlo, az orvos pedig Karpatia102. Mindenki más hétköznapi civil volt.*/
	}
}