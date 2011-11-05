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
using Schumix.Framework.Localization;
using Schumix.GameAddon;

namespace Schumix.GameAddon.MaffiaGames
{
	public sealed partial class MaffiaGame
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly ChannelInfo sChannelInfo = Singleton<ChannelInfo>.Instance;
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly Dictionary<string, string> _detectivelist = new Dictionary<string, string>();
		private readonly Dictionary<string, Player> _playerflist = new Dictionary<string, Player>();
		private readonly Dictionary<string, string> _killerlist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _doctorlist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _normallist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _ghostlist = new Dictionary<string, string>();
		private readonly Dictionary<int, string> _playerlist = new Dictionary<int, string>();
		private readonly List<string> _gameoverlist = new List<string>();
		private readonly List<string> _leftlist = new List<string>();
		private readonly List<string> _joinlist = new List<string>();
		public Dictionary<string, string> GetDetectiveList() { return _detectivelist; }
		public Dictionary<string, string> GetKillerList() { return _killerlist; }
		public Dictionary<string, string> GetDoctorList() { return _doctorlist; }
		public Dictionary<string, string> GetNormalList() { return _normallist; }
		public Dictionary<int, string> GetPlayerList() { return _playerlist; }
		public string GetDetective() { return _players < 15 ? detective_ : detective_ + " és " + detective2_; }
		public string GetDoctor() { return doctor_; }
		public string GetOwner() { return _owner; }
		public int GetPlayers() { return _players; }
		public bool Running { get; private set; }
		public bool Started { get; private set; }
		private Thread _thread;
		private string _owner;
		private string _channel;
		private string newkillghost;
		private Rank _rank;
		private string detective_;
		private string detective2_;
		private string doctor_;
		private string killer_;
		private string killer2_;
		private string killer3_;
		private string rescued;
		private string _killerchannel;
		private bool _day;
		private bool _stop;
		private bool _joinstop;
		private bool _start;
		private bool _lynch;
		private bool _ghosttext;
		private int _lynchmaxnumber;
		private int _players;

		public string GetKiller()
		{
			if(_players < 8)
				return killer_;
			else if(_players >= 8 && _players < 15)
				return killer_ + " és " + killer2_;
			else
				return killer_ + ", " + killer2_ + " és " + killer3_;
		}

		public MaffiaGame(string Name, string Channel)
		{
			NewGame(Name, Channel);
		}

		public void NewGame(string Name, string Channel)
		{
			Clean();
			_day = false;
			_stop = false;
			_joinstop = false;
			_start = false;
			_lynch = false;
			_ghosttext = false;
			_players = 0;
			_lynchmaxnumber = 0;
			Running = true;
			Started = false;
			_owner = Name;
			newkillghost = "new";
			_rank = Rank.None;
			rescued = string.Empty;
			doctor_ = string.Empty;
			killer_ = string.Empty;
			killer2_ = string.Empty;
			killer3_ = string.Empty;
			detective_ = string.Empty;
			detective2_ = string.Empty;
			_channel = Channel.ToLower();
			_killerchannel = string.Empty;
			_playerlist.Add(1, Name);
			sSendMessage.SendCMPrivmsg(_channel, "{0} új játékot indított. Csatlakozni a '!join' paranccsal tudtok.", _owner);
			sSendMessage.SendCMPrivmsg(_channel, "{0}: Írd be a '!start' parancsot, ha mindenki készen áll.", _owner);
		}

		public void NewNick(int Id, string OldName, string NewName)
		{
			if(_playerlist.ContainsKey(Id))
			{
				_playerlist.Remove(Id);
				_playerlist.Add(Id, NewName);
			}

			if(_joinlist.Contains(OldName.ToLower()))
			{
				_joinlist.Remove(OldName.ToLower());
				_joinlist.Add(NewName.ToLower());
			}

			if(_owner == OldName)
				_owner = NewName;

			if(Started)
			{
				if(_killerlist.ContainsKey(OldName.ToLower()))
				{
					_killerlist.Remove(OldName.ToLower());
					_killerlist.Add(NewName.ToLower(), NewName);
				}
				else if(_detectivelist.ContainsKey(OldName.ToLower()))
				{
					_detectivelist.Remove(OldName.ToLower());
					_detectivelist.Add(NewName.ToLower(), NewName);
				}
				else if(_doctorlist.ContainsKey(OldName.ToLower()))
				{
					_doctorlist.Remove(OldName.ToLower());
					_doctorlist.Add(NewName.ToLower(), NewName);
				}
				else if(_normallist.ContainsKey(OldName.ToLower()))
				{
					_normallist.Remove(OldName.ToLower());
					_normallist.Add(NewName.ToLower(), NewName);
				}
				else if(_ghostlist.ContainsKey(OldName.ToLower()))
				{
					_ghostlist.Remove(OldName.ToLower());
					_ghostlist.Add(NewName.ToLower(), NewName);
				}

				if(_playerflist.ContainsKey(OldName.ToLower()))
				{
					_playerflist.Add(NewName.ToLower(), new Player(_playerflist[OldName.ToLower()].Rank));
					_playerflist[NewName.ToLower()].RName = _playerflist[OldName.ToLower()].RName;

					foreach(var lynch in _playerflist[OldName.ToLower()].Lynch)
						_playerflist[NewName.ToLower()].Lynch.Add(lynch);

					foreach(var function in _playerflist)
					{
						if(function.Value.Lynch.Contains(OldName.ToLower()))
						{
							function.Value.Lynch.Remove(OldName.ToLower());
							function.Value.Lynch.Add(NewName.ToLower());
						}
					}

					_playerflist[OldName.ToLower()].Lynch.Clear();
					_playerflist.Remove(OldName.ToLower());
				}

				if(_gameoverlist.Contains(OldName.ToLower()))
				{
					_gameoverlist.Remove(OldName.ToLower());
					_gameoverlist.Add(NewName.ToLower());
				}

				if(killer_.ToLower() == OldName.ToLower())
					killer_ = NewName;

				if(killer2_.ToLower() == OldName.ToLower())
					killer2_ = NewName;

				if(killer3_.ToLower() == OldName.ToLower())
					killer3_ = NewName;

				if(detective_.ToLower() == OldName.ToLower())
					detective_ = NewName;

				if(detective2_.ToLower() == OldName.ToLower())
					detective2_ = NewName;

				if(doctor_.ToLower() == OldName.ToLower())
					doctor_ = NewName;
			}
		}

		private void RemovePlayer(string Name)
		{
			if(Name.Replace(SchumixBase.Space.ToString(), string.Empty) == string.Empty)
				return;

			_rank = Rank.None;
			string name = string.Empty;

			if(Started)
			{
				if(_killerlist.ContainsKey(Name.ToLower()))
				{
					name = _killerlist[Name.ToLower()];
					_killerlist.Remove(Name.ToLower());
					_rank = Rank.Killer;
				}
				else if(_detectivelist.ContainsKey(Name.ToLower()))
				{
					name = _detectivelist[Name.ToLower()];
					_detectivelist.Remove(Name.ToLower());
					_rank = Rank.Detective;
				}
				else if(_doctorlist.ContainsKey(Name.ToLower()))
				{
					name = _doctorlist[Name.ToLower()];
					_doctorlist.Remove(Name.ToLower());
					rescued = string.Empty;
					_rank = Rank.Doctor;
				}
				else if(_normallist.ContainsKey(Name.ToLower()))
				{
					name = _normallist[Name.ToLower()];
					_normallist.Remove(Name.ToLower());
					_rank = Rank.Normal;
				}
			}

			if(_joinlist.Contains(Name.ToLower()))
				_joinlist.Remove(Name.ToLower());

			if(_gameoverlist.Contains(Name.ToLower()))
				_gameoverlist.Remove(Name.ToLower());

			int i = 0;
			foreach(var player in _playerlist)
			{
				if(player.Value == name || player.Value == Name)
				{
					i = player.Key;
					break;
				}
			}

			_playerlist.Remove(i);

			if(_playerflist.ContainsKey(Name.ToLower()))
				_playerflist.Remove(Name.ToLower());

			if(Started && !_ghostlist.ContainsKey(name.ToLower()))
				_ghostlist.Add(name.ToLower(), name);

			if(newkillghost.ToLower() == name.ToLower())
				newkillghost = name;

			sSender.Mode(_channel, "-v", name);
		}

		private void Corpse()
		{
			if(_rank == Rank.Killer)
				sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy 4gyilkos volt.");
			else if(_rank == Rank.Detective)
				sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy 4nyomozó volt.");
			else if(_rank == Rank.Doctor)
				sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy 4orvos volt.");
			else if(_rank == Rank.Normal)
				sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy egy ártatlan falusi volt.");
		}

		private void EndText()
		{
			if(_players < 8)
				sSendMessage.SendCMPrivmsg(_channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig nem volt. Mindenki más hétköznapi civil volt.", killer_, detective_);
			else
				sSendMessage.SendCMPrivmsg(_channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig 4{2}. Mindenki más hétköznapi civil volt.", GetKiller(), GetDetective(), GetDoctor());
		}

		public void RemoveRanks()
		{
			Running = false;
			int i = 0;
			string namesss = string.Empty;
			var list = new List<string>();

			foreach(var end in _playerlist)
			{
				if(i == 4)
				{
					i = 0;
					list.Add(namesss.Remove(0, 1, SchumixBase.Space));
					namesss = string.Empty;
				}

				i++;
				namesss += SchumixBase.Space + end.Value;
			}

			foreach(var l in list)
				sSender.Mode(_channel, "-vvvv", l);

			list.Clear();
			namesss = namesss.Remove(0, 1, SchumixBase.Space);
			var split = namesss.Split(SchumixBase.Space);

			if(split.Length == 1)
				sSender.Mode(_channel, "-v", split[0]);
			else if(split.Length == 2)
				sSender.Mode(_channel, "-vv", split[1]);
			else if(split.Length == 3)
				sSender.Mode(_channel, "-vvv", split[2]);

			foreach(var end in _playerlist)
				sSender.Mode(_channel, "-v", end.Value);

			sSender.Mode(_channel, "-m");
		}

		private string GetPlayerName(string Name)
		{
			string player = string.Empty;

			if(_killerlist.ContainsKey(Name))
				player = _killerlist[Name];
			else if(_detectivelist.ContainsKey(Name))
				player = _detectivelist[Name];
			else if(_doctorlist.ContainsKey(Name))
				player = _doctorlist[Name];
			else if(_normallist.ContainsKey(Name))
				player = _normallist[Name];

			return player;
		}

		private void StartThread()
		{
			_thread = new Thread(Game);
			_thread.Start();
		}

		public void StopThread()
		{
			Running = false;
			SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", GameAddon.GameChannelFunction[_channel]), string.Format("Channel = '{0}'", _channel));
			sChannelInfo.ChannelFunctionReload();
			SchumixBase.DManager.Update("channel", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions("gamecommands", "off", _channel)), string.Format("Channel = '{0}'", _channel));
			sChannelInfo.ChannelFunctionReload();

			if(Started)
				_thread.Abort();

			if(_players >= 15)
			{
				sSender.Part(_killerchannel);
				SchumixBase.DManager.Delete("channel", string.Format("Channel = '{0}'", sUtilities.SqlEscape(_killerchannel)));
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionReload();
			}

			Clean();
			Started = false;
			GameAddon.GameChannelFunction.Remove(_channel);
		}

		private void Game()
		{
			bool enabledk = false;
			bool enableddoctor = false;
			bool enabledkiller = false;
			bool enableddetective = false;

			if(_players < 8)
				sSendMessage.SendCMPrivmsg(_channel, "Nincs elég játékos két gyilkoshoz, csak egy gyilkos van játékban (illetve nincs orvos).");
			else if(_players >= 8 && _players < 15)
				sSendMessage.SendCMPrivmsg(_channel, "Mivel legalább 8 játékos van, ezért 2 gyilkos és egy orvos lesz.");
			else if(_players >= 15)
				sSendMessage.SendCMPrivmsg(_channel, "Mivel legalább 15 játékos van, ezért 3 gyilkos, 2 nyomozó és egy orvos lesz.");

			sSendMessage.SendCMPrivmsg(_channel, "Itt mindenki egyszerű civilnek tűnhet, de valójában köztetek van 1, 2 vagy 3 4gyilkos, akiknek célja mindenkit megölni az éj leple alatt.");
			sSendMessage.SendCMPrivmsg(_channel, "Köztetek van egy vagy kettő 4nyomozó is: ő képes éjszakánként megtudni 1-1 emberről, hogy gyilkos-e, és lebuktatni őt a falusiak előtt, illetve a falu 4orvosa, aki minden éjjel megmenthet valakit...");
			sSendMessage.SendCMPrivmsg(_channel, "A csoport célja tehát lebuktatni és meglincselni a gyilkos(oka)t, mielőtt mindenkit megölnek álmukban.");

			string names = string.Empty;
			foreach(var name in _playerlist)
				names += ", " + name.Value;

			Thread.Sleep(2000);

			for(;;)
			{
				Thread.Sleep(1000);

				if(_killerlist.Count == 1)
				{
					foreach(var function in _playerflist)
					{
						if(function.Value.Rank == Rank.Killer && function.Value.RName != string.Empty)
						{
							newkillghost = function.Value.RName;
							enabledkiller = true;
						}
					}
				}
				else if(_killerlist.Count == 2)
				{
					var list = new List<string>();

					foreach(var function in _playerflist)
					{
						if(function.Value.Rank == Rank.Killer && function.Value.RName != string.Empty)
							list.Add(function.Value.RName);
					}

					if(list.Count == 2 && list[0] == list[1] && !enabledk)
					{
						foreach(var kill in _killerlist)
							sSendMessage.SendCMPrivmsg(kill.Key, "A gyilkosok megegyeztek!");

						enabledk = true;
						newkillghost = list[0];
						enabledkiller = true;
					}
					else if(list.Count == 2 && list[0] != list[1])
						enabledk = false;

					list.Clear();
				}
				else if(_killerlist.Count == 3)
				{
					var list = new List<string>();

					foreach(var function in _playerflist)
					{
						if(function.Value.Rank == Rank.Killer && function.Value.RName != string.Empty)
							list.Add(function.Value.RName);
					}

					if(list.Count == 3 && list[0] == list[1] && list[0] == list[2] && list[1] == list[2] && !enabledk)
					{
						foreach(var kill in _killerlist)
							sSendMessage.SendCMPrivmsg(kill.Key, "A gyilkosok megegyeztek!");

						enabledk = true;
						newkillghost = list[0];
						enabledkiller = true;
					}
					else if(list.Count == 3 && list[0] != list[1] && list[0] == list[2] && list[1] == list[2])
						enabledk = false;
					else if(list.Count == 3 && list[0] == list[1] && list[0] != list[2] && list[1] == list[2])
						enabledk = false;
					else if(list.Count == 3 && list[0] == list[1] && list[0] == list[2] && list[1] != list[2])
						enabledk = false;
					else if(list.Count == 3 && list[0] != list[1] && list[0] != list[2] && list[1] == list[2])
						enabledk = false;
					else if(list.Count == 3 && list[0] == list[1] && list[0] != list[2] && list[1] != list[2])
						enabledk = false;
					else if(list.Count == 3 && list[0] != list[1] && list[0] == list[2] && list[1] != list[2])
						enabledk = false;
					else if(list.Count == 3 && list[0] != list[1] && list[0] != list[2] && list[1] != list[2])
						enabledk = false;

					list.Clear();
				}

				if(_detectivelist.Count == 1)
				{
					foreach(var function in _playerflist)
					{
						if(function.Value.Rank == Rank.Detective && function.Value.Detective)
							enableddetective = true;
					}
				}
				else if(_detectivelist.Count == 2)
				{
					int number = 0;

					foreach(var function in _playerflist)
					{
						if(function.Value.Rank == Rank.Detective && function.Value.Detective)
							number++;
					}

					if(number == 2)
						enableddetective = true;
				}
				else if(_detectivelist.Count == 0)
					enableddetective = true;

				if(_doctorlist.Count == 1)
				{
					foreach(var function in _playerflist)
					{
						if(function.Value.Rank == Rank.Doctor && rescued != string.Empty)
							enableddoctor = true;
					}
				}
				else if(_doctorlist.Count == 0)
					enableddoctor = true;

				if(enabledkiller && enableddetective && enableddoctor)
				{
					foreach(var function in _playerflist)
					{
						if(function.Value.Rank == Rank.Detective)
							function.Value.Detective = false;

						function.Value.RName = string.Empty;
					}

					_day = true;
					_stop = false;
					enabledk = false;
					_ghosttext = true;
					enabledkiller = false;
					enableddoctor = false;
					enableddetective = false;

					if(_players >= 8)
					{
						if(newkillghost.ToLower() != rescued.ToLower())
						{
							RemovePlayer(newkillghost);
							_ghosttext = true;
							sSendMessage.SendCMPrivmsg(newkillghost, "Meghaltál. Kérlek maradj csendben amíg a játék véget ér.");
						}
					}
					else
					{
						RemovePlayer(newkillghost);
						sSendMessage.SendCMPrivmsg(newkillghost, "Meghaltál. Kérlek maradj csendben amíg a játék véget ér.");
					}

					EndGame();
					_ghosttext = false;
				}

				if(!Started)
					StopThread();

				EndGame();

				if(!_day)
				{
					if(_stop)
						continue;

					_stop = true;

					foreach(var function in _playerflist)
						function.Value.Lynch.Clear();

					names = string.Empty;

					foreach(var name in _playerlist)
						names += ", " + name.Value;

					sSendMessage.SendCMPrivmsg(_channel, "A következő személyek vannak még életben: {0}", names.Remove(0, 2, ", "));
					sSendMessage.SendCMPrivmsg(_channel, "Leszállt az 4éj.");
					sSendMessage.SendCMPrivmsg(_channel, "Az összes civil békésen szundikál...");
					Thread.Sleep(1000);

					foreach(var name in _killerlist)
					{
						sSendMessage.SendCMPrivmsg(name.Key, "Miközben a falusiak alszanak, te eldöntöd, hogy kit ölsz meg az éj leple alatt.");
						sSendMessage.SendCMPrivmsg(name.Key, "Te és a másik gyilkos (ha létezik, és él egyáltalán) meg fogjátok vitatni (PM-ben), hogy ki legyen az áldozat.");
						sSendMessage.SendCMPrivmsg(name.Key, "Írd be PM-ként nekem: '!kill <nickname>'");
						Thread.Sleep(400);
					}

					if(_players >= 8 && _players < 15)
					{
						foreach(var name in _killerlist)
						{
							if(name.Key == killer_.ToLower())
								sSendMessage.SendCMPrivmsg(name.Key, "A másik gyilkos {0}. PM-ben beszélgessetek.", killer2_);
							else
								sSendMessage.SendCMPrivmsg(name.Key, "A másik gyilkos {0}. PM-ben beszélgessetek.", killer_);

							Thread.Sleep(400);
						}
					}
					else if(_players >= 15)
					{
						foreach(var name in _killerlist)
						{
							sSendMessage.SendCMPrivmsg(name.Key, "Csatlakoz ide: {0} és beszéljétek meg ki haljon meg!", _killerchannel);
							Thread.Sleep(400);
						}
					}

					if(_players >= 8)
					{
						foreach(var name in _doctorlist)
						{
							sSendMessage.SendCMPrivmsg(name.Key, "A te dolgod éjszaka vigyázni a falu betegére.");
							sSendMessage.SendCMPrivmsg(name.Key, "Most kell eldöntened hogy kit akarsz vizsgálni éjszaka: írd be PM-ként nekem: '!rescue <nickname>'.");
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

					if(newkillghost.ToLower() != rescued.ToLower())
					{
						sSendMessage.SendCMPrivmsg(_channel, "A falusiakat szörnyű látvány fogadja: megtalálták 4{0} holttestét!", newkillghost);
						Corpse();
						sSendMessage.SendCMPrivmsg(_channel, "({0} meghalt, és nem szólhat hozzá a játékhoz.)", newkillghost);
					}
					else
						sSendMessage.SendCMPrivmsg(_channel, "Nem halt meg senki!");
	
					newkillghost = "new";
					rescued = string.Empty;

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
				RemoveRanks();
				sSendMessage.SendCMPrivmsg(_channel, "A gyilkosok halottak! A 4falusiak győztek.");
				sSendMessage.SendCMPrivmsg(_channel, "A játék befejeződött.");
				EndText();
				StopThread();
				return;
			}
			else
			{
				if((_killerlist.Count >= _detectivelist.Count + _doctorlist.Count + _normallist.Count) && Running)
				{
					RemoveRanks();

					if(_killerlist.Count >= 1 && _ghosttext)
					{
						if(newkillghost != string.Empty)
							sSendMessage.SendCMPrivmsg(_channel, "A falusiakat szörnyű látvány fogadja: megtalálták 4{0} holttestét!", newkillghost);

						Corpse();
					}

					sSendMessage.SendCMPrivmsg(_channel, "A falusiak halottak! A 4gyilkosok győztek.");
					sSendMessage.SendCMPrivmsg(_channel, "A játék befejeződött.");
					EndText();
					StopThread();
					return;
				}
				else if((_playerlist.Count <= 2) && Running)
				{
					RemoveRanks();
					sSendMessage.SendCMPrivmsg(_channel, "Elfogytak a játékosok!");
					sSendMessage.SendCMPrivmsg(_channel, "A játék befejeződött.");
					EndText();
					StopThread();
					return;
				}
			}
		}

		private void Clean()
		{
			foreach(var function in _playerflist)
				function.Value.Lynch.Clear();

			_playerflist.Clear();
			_playerlist.Clear();
			_detectivelist.Clear();
			_killerlist.Clear();
			_doctorlist.Clear();
			_normallist.Clear();
			_ghostlist.Clear();
			_joinlist.Clear();
			_leftlist.Clear();
			_gameoverlist.Clear();
		}
	}
}