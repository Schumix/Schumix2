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

namespace Schumix.GameAddon.MaffiaGames
{
	public sealed partial class MaffiaGame
	{
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly Dictionary<string, string> _detectivelist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _killerlist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _doctorlist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _normallist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _ghostlist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _lynchlist = new Dictionary<string, string>();
		private readonly Dictionary<int, string> _playerlist = new Dictionary<int, string>();
		public Dictionary<string, string> GetDetectiveList() { return _detectivelist; }
		public Dictionary<string, string> GetKillerList() { return _killerlist; }
		public Dictionary<string, string> GetNormalList() { return _normallist; }
		public Dictionary<int, string> GetPlayerList() { return _playerlist; }
		public string GetDetective() { return detective_; }
		public string GetKiller() { return killer_; }
		public string GetOwner() { return _owner; }
		public bool Started { get; private set; }
		public bool Running { get; private set; }
		private Thread _thread;
		private string _owner;
		private string _channel;
		private string newghost;
		private string _rank;
		private string detective_;
		private string killer_;
		private bool _day;
		private bool _stop;
		private bool _killer;
		private bool _detective;
		private bool _ghostdetective;
		private bool _joinstop;
		private int _lynchmaxnumber;
		private int _players;

		public MaffiaGame(string Name, string Channel)
		{
			_playerlist.Clear();
			_detectivelist.Clear();
			_killerlist.Clear();
			_doctorlist.Clear();
			_normallist.Clear();
			_ghostlist.Clear();
			_day = false;
			_stop = false;
			_killer = false;
			_detective = false;
			_joinstop = false;
			_ghostdetective = false;
			_players = 0;
			_lynchmaxnumber = 0;
			Running = true;
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
			_playerlist.Clear();
			_detectivelist.Clear();
			_killerlist.Clear();
			_doctorlist.Clear();
			_normallist.Clear();
			_ghostlist.Clear();
			_day = false;
			_stop = false;
			_killer = false;
			_detective = false;
			_joinstop = false;
			_ghostdetective = false;
			_players = 0;
			_lynchmaxnumber = 0;
			Running = true;
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

		public void NewNick(int Id, string OldName, string NewName)
		{

		}

		private void StartThread()
		{
			_thread = new Thread(Game);
			_thread.Start();
		}

		public void StopThread()
		{
			Running = false;
			SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", GameAddon.GameChannelFunction[_channel], _channel);
			sChannelInfo.ChannelFunctionReload();

			if(Started)
				_thread.Abort();

			Started = false;
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
					_rank = string.Empty;

					if(_killerlist.ContainsKey(newghost.ToLower()))
					{
						_killerlist.Remove(newghost.ToLower());
						_rank = "killer";
					}
					else if(_detectivelist.ContainsKey(newghost.ToLower()))
					{
						_detectivelist.Remove(newghost.ToLower());
						_ghostdetective = true;
						_rank = "detective";
					}
					else if(_normallist.ContainsKey(newghost.ToLower()))
					{
						_normallist.Remove(newghost.ToLower());
						_rank = "normal";
					}

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

					if(!_ghostdetective)
						_detective = false;
					else
						_detective = true;

					sSender.Mode(_channel, "-v", newghost);
					sSendMessage.SendCMPrivmsg(newghost, "Meghaltál. Kérlek maradj csendben amíg a játék véget ér.");
					EndGame();
				}

				if(!Started)
					StopThread();

				EndGame();

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
					else if(_rank == "normal")
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
			if(_killerlist.Count == 0 && Running)
			{
				Running = false;

				foreach(var end in _playerlist)
					sSender.Mode(_channel, "-v", end.Value);

				sSender.Mode(_channel, "-m");
				sSendMessage.SendCMPrivmsg(_channel, "A gyilkosok halottak! A falusiak győztek.");
				sSendMessage.SendCMPrivmsg(_channel, "A játék befejeződött.");
				sSendMessage.SendCMPrivmsg(_channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig None. Mindenki más hétköznapi civil volt.", killer_, detective_);
				StopThread();
				return;
			}
			else
			{
				if(_playerlist.Count == 2 && Running)
				{
					if(_players < 8)
					{
						Running = false;

						foreach(var end in _playerlist)
							sSender.Mode(_channel, "-v", end.Value);

						sSender.Mode(_channel, "-m");
						sSendMessage.SendCMPrivmsg(_channel, "A falusiak halottak! A 4gyilkosok győztek.");
						sSendMessage.SendCMPrivmsg(_channel, "A játék befejeződött.");
						sSendMessage.SendCMPrivmsg(_channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig None. Mindenki más hétköznapi civil volt.", killer_, detective_);
						StopThread();
						return;
					}
				}
			}
		}
	}
}