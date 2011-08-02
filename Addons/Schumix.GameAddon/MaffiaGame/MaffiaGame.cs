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
		private string newghost;
		private string _newghost;
		private string _newghost2;
		private string _newghost3;
		private string _rank;
		private string detective_;
		private string detective2_;
		private string doctor_;
		private string killer_;
		private string killer2_;
		private string killer3_;
		private string rescued;
		private bool _day;
		private bool _stop;
		private bool _killer;
		private bool _doctor;
		private bool _detective;
		private bool _detective2;
		private bool _ghostdetective;
		private bool _ghostdetective2;
		private bool _ghostdoctor;
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
			_playerlist.Clear();
			_detectivelist.Clear();
			_killerlist.Clear();
			_doctorlist.Clear();
			_normallist.Clear();
			_ghostlist.Clear();
			_joinlist.Clear();
			_leftlist.Clear();
			_day = false;
			_stop = false;
			_killer = false;
			_doctor = false;
			_detective = false;
			_detective2 = false;
			_joinstop = false;
			_ghostdetective = false;
			_ghostdetective2 = false;
			_ghostdoctor = false;
			_start = false;
			_lynch = false;
			_ghosttext = false;
			_players = 0;
			_lynchmaxnumber = 0;
			Running = true;
			Started = false;
			_owner = Name;
			_newghost = "new";
			_newghost2 = "new2";
			_newghost2 = "new3";
			_rank = string.Empty;
			rescued = string.Empty;
			doctor_ = string.Empty;
			killer_ = string.Empty;
			killer2_ = string.Empty;
			killer3_ = string.Empty;
			detective_ = string.Empty;
			detective2_ = string.Empty;
			_channel = Channel.ToLower();
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

				Lynch(NewName, OldName, "newname", "none");
			}
		}

		private void RemovePlayer(string Name)
		{
			_rank = string.Empty;
			string name = string.Empty;

			if(Started)
			{
				if(_killerlist.ContainsKey(Name.ToLower()))
				{
					name = _killerlist[Name.ToLower()];
	
					if(Started && _killerlist.Count == 2)
					{
						if(killer_.ToLower() == Name.ToLower())
						{
							if(_killerlist.ContainsKey(_newghost2) && _detectivelist.ContainsKey(_newghost2) &&
								_normallist.ContainsKey(_newghost2) && !_ghostlist.ContainsKey(_newghost2) &&
								_doctorlist.ContainsKey(_newghost2))
							{
								newghost = _newghost2;
								_killer = true;
							}
						}

						if(killer2_.ToLower() == Name.ToLower())
						{
							if(_killerlist.ContainsKey(_newghost) && _detectivelist.ContainsKey(_newghost) &&
								_normallist.ContainsKey(_newghost) && !_ghostlist.ContainsKey(_newghost) &&
								_doctorlist.ContainsKey(_newghost))
							{
								newghost = _newghost;
								_killer = true;
							}
						}
					}

					// Ha kellene!
					/*if(Started && _killerlist.Count == 3)
					{
						if(killer_.ToLower() == Name.ToLower())
						{
							if(_killerlist.ContainsKey(_newghost2) && _detectivelist.ContainsKey(_newghost2) &&
								_normallist.ContainsKey(_newghost2) && !_ghostlist.ContainsKey(_newghost2) &&
								_doctorlist.ContainsKey(_newghost2))
							{
								newghost = _newghost2;
								_killer = true;
							}
						}

						if(killer2_.ToLower() == Name.ToLower())
						{
							if(_killerlist.ContainsKey(_newghost) && _detectivelist.ContainsKey(_newghost) &&
								_normallist.ContainsKey(_newghost) && !_ghostlist.ContainsKey(_newghost) &&
								_doctorlist.ContainsKey(_newghost))
							{
								newghost = _newghost;
								_killer = true;
							}
						}

						if(killer3_.ToLower() == Name.ToLower())
						{
							if(_killerlist.ContainsKey(_newghost) && _detectivelist.ContainsKey(_newghost) &&
								_normallist.ContainsKey(_newghost) && !_ghostlist.ContainsKey(_newghost) &&
								_doctorlist.ContainsKey(_newghost))
							{
								newghost = _newghost;
								_killer = true;
							}
						}
					}*/

					_killerlist.Remove(Name.ToLower());
					_rank = "killer";
				}
				else if(_detectivelist.ContainsKey(Name.ToLower()))
				{
					name = _detectivelist[Name.ToLower()];
					_detectivelist.Remove(Name.ToLower());

					if(Started)
					{
						if(_players < 15)
						{
							_ghostdetective = true;
							_detective = true;
						}
						else
						{
							if(detective_.ToLower() == Name.ToLower())
							{
								_ghostdetective = true;
								_detective = true;
							}
							else if(detective2_.ToLower() == Name.ToLower())
							{
								_ghostdetective2 = true;
								_detective2 = true;
							}
						}
					}

					_rank = "detective";
				}
				else if(_doctorlist.ContainsKey(Name.ToLower()))
				{
					name = _doctorlist[Name.ToLower()];
					_doctorlist.Remove(Name.ToLower());

					if(Started)
					{
						_ghostdoctor = true;
						_doctor = true;
						rescued = string.Empty;
					}

					_rank = "doctor";
				}
				else if(_normallist.ContainsKey(Name.ToLower()))
				{
					name = _normallist[Name.ToLower()];
					_normallist.Remove(Name.ToLower());
					_rank = "normal";
				}
			}

			if(_joinlist.Contains(Name.ToLower()))
				_joinlist.Remove(Name.ToLower());

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

			if(Started && !_ghostlist.ContainsKey(name.ToLower()))
				_ghostlist.Add(name.ToLower(), name);

			newghost = name;
			sSender.Mode(_channel, "-v", name);
		}

		private void Corpse()
		{
			if(_rank == "killer")
				sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy 4gyilkos volt.");
			else if(_rank == "detective")
				sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy 4nyomozó volt.");
			else if(_rank == "doctor")
				sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy 4orvos volt.");
			else if(_rank == "normal")
				sSendMessage.SendCMPrivmsg(_channel, "*** A holttest megvizsgálása után kiderült, hogy egy ártatlan falusi volt.");
		}

		private void EndText()
		{
			if(_players < 8)
				sSendMessage.SendCMPrivmsg(_channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig nem volt. Mindenki más hétköznapi civil volt.", killer_, detective_);
			else
				sSendMessage.SendCMPrivmsg(_channel, "*** A gyilkos 4{0} volt, a nyomozó 4{1}, az orvos pedig 4{2}. Mindenki más hétköznapi civil volt.", GetKiller(), GetDetective(), GetDoctor());
		}

		/*private void AddedRanks()
		{
			foreach(var player in _playerlist)
				sSender.Mode(_channel, "+v", player.Value);
		}*/

		private void RemoveRanks()
		{
			Running = false;

			foreach(var end in _playerlist)
				sSender.Mode(_channel, "-v", end.Value);

			sSender.Mode(_channel, "-m");
		}

		private bool Lynch(string Name, string NickName, string Mode, string none)
		{
			string name = string.Empty;
			string names = string.Empty;
			string[] split = { string.Empty };

			foreach(var list in _lynchlist)
			{
				if(Mode == "lynch" && list.Key == Name.ToLower())
					names = list.Value;

				if(list.Value.Contains(NickName.ToLower()))
				{
					name = list.Key;

					if((Mode == "lynch" || Mode == "newlynch") && Name.ToLower() == name)
					{
						sSendMessage.SendCMPrivmsg(_channel, "{0}: Már szavaztál rá!", NickName);
						return false;
					}

					split = list.Value.Split(',');
				}
			}

			if(Mode == "lynch")
			{
				_lynchlist.Remove(Name.ToLower());
				_lynchlist.Add(Name.ToLower(), names + "," + NickName.ToLower());
			}

			_lynchlist.Remove(name);
			names = string.Empty;

			if(Mode == "leave")
			{
				if(_lynchlist.ContainsKey(Name.ToLower()))
					_lynchlist.Remove(Name.ToLower());
			}

			if(split.Length > 1)
			{
				foreach(var spl in split)
				{
					if(NickName.ToLower() == spl)
						continue;
					else
						names += "," + spl;
				}

				if(Mode == "newname")
					_lynchlist.Add(name, names.Remove(0, 1, ",") + "," + Name.ToLower());
				else
					_lynchlist.Add(name, names.Remove(0, 1, ","));
			}
			else
			{
				if(Mode == "newlynch")
					_lynchlist.Remove(name);
				else if(Mode == "newname")
					_lynchlist.Add(name, Name.ToLower());
			}

			if(Mode == "newlynch")
				_lynchlist.Add(Name.ToLower(), NickName.ToLower());

			return true;
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
			SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", GameAddon.GameChannelFunction[_channel], _channel);
			sChannelInfo.ChannelFunctionReload();
			SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunctions("gamecommands", "off", _channel), _channel);
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
			_joinlist.Clear();
			_leftlist.Clear();
			GameAddon.GameChannelFunction.Remove(_channel);
		}

		private void Game()
		{
			if(_players < 8)
				sSendMessage.SendCMPrivmsg(_channel, "Nincs elég játékos két gyilkoshoz, csak egy gyilkos van játékban (illetve nincs orvos).");
			else if(_players >= 8 && _players < 15)
				sSendMessage.SendCMPrivmsg(_channel, "Mivel legalább 8 játékos van, ezért 2 gyilkos és egy orvos lesz.");
			else if(_players >= 8)
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

				if(_players < 8 && _killer && _detective)
				{
					RemovePlayer(newghost);
					_day = true;
					_stop = false;
					_killer = false;
					_ghosttext = true;

					if(!_ghostdetective)
						_detective = false;
					else
						_detective = true;

					sSendMessage.SendCMPrivmsg(newghost, "Meghaltál. Kérlek maradj csendben amíg a játék véget ér.");
					EndGame();
					_ghosttext = false;
				}
				else if(_players >= 8 && _players < 15 && _killer && _detective && _doctor)
				{
					_newghost = "new";
					_newghost2 = "new2";
					_rank = string.Empty;

					if(newghost.ToLower() != rescued.ToLower())
					{
						RemovePlayer(newghost);
						_ghosttext = true;
						sSendMessage.SendCMPrivmsg(newghost, "Meghaltál. Kérlek maradj csendben amíg a játék véget ér.");
					}

					_day = true;
					_stop = false;
					_killer = false;

					if(!_ghostdetective)
						_detective = false;
					else
						_detective = true;

					if(!_ghostdoctor)
						_doctor = false;
					else
						_doctor = true;

					EndGame();
					_ghosttext = false;
				}
				else if(_players >= 15 && _killer && _detective && _detective2 && _doctor)
				{
					_newghost = "new";
					_newghost2 = "new2";
					_newghost3 = "new3";
					_rank = string.Empty;

					if(newghost.ToLower() != rescued.ToLower())
					{
						RemovePlayer(newghost);
						_ghosttext = true;
						sSendMessage.SendCMPrivmsg(newghost, "Meghaltál. Kérlek maradj csendben amíg a játék véget ér.");
					}

					_day = true;
					_stop = false;
					_killer = false;

					if(!_ghostdetective)
						_detective = false;
					else
						_detective = true;

					if(!_ghostdetective2)
						_detective2 = false;
					else
						_detective2 = true;

					if(!_ghostdoctor)
						_doctor = false;
					else
						_doctor = true;

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
					_lynchlist.Clear();
					names = string.Empty;

					/*foreach(var end in _playerlist)
						sSender.Mode(_channel, "-v", end.Value);*/

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
							if(name.Key == killer_.ToLower())
								sSendMessage.SendCMPrivmsg(name.Key, "A másik gyilkos(ok) {0} és {1}. PM-ben beszélgessetek.", killer2_, killer3_);
							else if(name.Key == killer2_.ToLower())
								sSendMessage.SendCMPrivmsg(name.Key, "A másik gyilkos(ok) {0} és {1}. PM-ben beszélgessetek.", killer_, killer3_);
							else
								sSendMessage.SendCMPrivmsg(name.Key, "A másik gyilkos(ok) {0} és {1}. PM-ben beszélgessetek.", killer_, killer2_);

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
					//AddedRanks();
					sSendMessage.SendCMPrivmsg(_channel, "Felkelt a nap!");

					if(newghost.ToLower() != rescued.ToLower())
					{
						sSendMessage.SendCMPrivmsg(_channel, "A falusiakat szörnyű látvány fogadja: megtalálták 4{0} holttestét!", newghost);
						Corpse();
						sSendMessage.SendCMPrivmsg(_channel, "({0} meghalt, és nem szólhat hozzá a játékhoz.)", newghost);
					}
					else
						sSendMessage.SendCMPrivmsg(_channel, "Nem halt meg senki!");

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
						sSendMessage.SendCMPrivmsg(_channel, "A falusiakat szörnyű látvány fogadja: megtalálták 4{0} holttestét!", newghost);
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
	}
}