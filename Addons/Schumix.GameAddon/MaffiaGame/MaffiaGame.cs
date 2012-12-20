/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
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
using System.Timers;
using System.Threading;
using System.Collections.Generic;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.GameAddon;
using Schumix.GameAddon.Commands;

namespace Schumix.GameAddon.MaffiaGames
{
	sealed partial class MaffiaGame : CommandInfo
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly Dictionary<string, string> _detectivelist = new Dictionary<string, string>();
		private readonly Dictionary<string, Player> _playerflist = new Dictionary<string, Player>();
		private readonly Dictionary<string, string> _killerlist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _doctorlist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _normallist = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _ghostlist = new Dictionary<string, string>();
		private readonly Dictionary<int, string> _playerlist = new Dictionary<int, string>();
		private System.Timers.Timer _timerowner = new System.Timers.Timer();
		private readonly List<string> _gameoverlist = new List<string>();
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly List<string> _leftlist = new List<string>();
		private readonly List<string> _joinlist = new List<string>();
		public Dictionary<string, string> GetDetectiveList() { return _detectivelist; }
		public Dictionary<string, Player> GetPlayerFList() { return _playerflist; }
		public Dictionary<string, string> GetKillerList() { return _killerlist; }
		public Dictionary<string, string> GetDoctorList() { return _doctorlist; }
		public Dictionary<string, string> GetNormalList() { return _normallist; }
		public Dictionary<int, string> GetPlayerList() { return _playerlist; }
		public string GetOwner() { return _owner; }
		public int GetPlayers() { return _players; }
		public bool Running { get; private set; }
		public bool Started { get; set; }
		public bool NoLynch { get; set; }
		public bool NoVoice { get; set; }
		private GameCommand sGameCommand;
		private DateTime OwnerMsgTime;
		private Thread _thread;
		private string _owner;
		private string _channel;
		private string _killerchannel;
		private bool _day;
		private bool _stop;
		private bool _joinstop;
		private bool _start;
		private bool _lynch;
		private int _lynchmaxnumber;
		private string _servername;
		private int _players;
		private int _gameid;

		public MaffiaGame(string ServerName, string Name, string Channel, GameCommand gc) : base(ServerName)
		{
			_servername = ServerName;
			sGameCommand = gc;
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
			_players = 0;
			_lynchmaxnumber = 0;
			Running = true;
			Started = false;
			NoLynch = false;
			NoVoice = false;
			_owner = Name;
			_channel = Channel.ToLower();
			_killerchannel = string.Empty;
			_playerlist.Add(1, Name);
			NewOwnerTime();

			_timerowner.Interval = 60*1000;
			_timerowner.Elapsed += HandleIsOwnerAfk;
			_timerowner.Enabled = true;
			_timerowner.Start();

			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
			var text = sLManager.GetCommandTexts("maffiagame/base/newgame", _channel, _servername);
			if(text.Length < 2)
			{
				sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
				return;
			}

			sSendMessage.SendCMPrivmsg(_channel, text[0], _owner);
			sSendMessage.SendCMPrivmsg(_channel, text[1], _owner);

			var db = SchumixBase.DManager.QueryFirstRow("SELECT Game FROM maffiagame WHERE ServerName = '{0}' ORDER BY Game DESC", _servername);
			_gameid = !db.IsNull() ? (Convert.ToInt32(db["Game"].ToString()) + 1) : 1;
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
					_playerflist.Add(NewName.ToLower(), new Player(_playerflist[OldName.ToLower()].Rank, _playerflist[OldName.ToLower()].Master));
					_playerflist[NewName.ToLower()].RName = _playerflist[OldName.ToLower()].RName;
					_playerflist[NewName.ToLower()].DRank = _playerflist[OldName.ToLower()].DRank;
					_playerflist[NewName.ToLower()].Ghost = _playerflist[OldName.ToLower()].Ghost;
					_playerflist[NewName.ToLower()].Detective = _playerflist[OldName.ToLower()].Detective;

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

				SchumixBase.DManager.Update("maffiagame", string.Format("Name = '{0}'", NewName), string.Format("Name = '{0}' And ServerName = '{1}'", OldName, _servername));
			}
		}

		public string GetKiller()
		{
			if(_players < 8)
			{
				foreach(var function in _playerflist)
				{
					if(function.Value.Rank == Rank.Killer)
					{
						if(_killerlist.ContainsKey(function.Key))
							return _killerlist[function.Key];
						else if(_ghostlist.ContainsKey(function.Key))
							return _ghostlist[function.Key];
					}
				}
			}
			else if(_players >= 8 && _players < 15)
			{
				string names = string.Empty;
				foreach(var function in _playerflist)
				{
					if(function.Value.Rank == Rank.Killer)
					{
						if(_killerlist.ContainsKey(function.Key))
							names += SchumixBase.Space + _killerlist[function.Key];
						else if(_ghostlist.ContainsKey(function.Key))
							names += SchumixBase.Space + _ghostlist[function.Key];
					}
				}

				names = names.Remove(0, 1, SchumixBase.Space);
				var split = names.Split(SchumixBase.Space);

				if(split.Length == 2)
					return split[0] + " és " + split[1];
				else
					return "Nem tudom kik =(";
			}
			else
			{
				string names = string.Empty;
				foreach(var function in _playerflist)
				{
					if(function.Value.Rank == Rank.Killer)
					{
						if(_killerlist.ContainsKey(function.Key))
							names += SchumixBase.Space + _killerlist[function.Key];
						else if(_ghostlist.ContainsKey(function.Key))
							names += SchumixBase.Space + _ghostlist[function.Key];
					}
				}

				names = names.Remove(0, 1, SchumixBase.Space);
				var split = names.Split(SchumixBase.Space);

				if(split.Length == 3)
					return split[0] + ", " + split[1] + " és " + split[2];
				else
					return "Nem tudom kik =(";
			}

			return "Nem tudom ki vagy kik =(";
		}

		public string GetDetective()
		{
			if(_players < 15)
			{
				foreach(var function in _playerflist)
				{
					if(function.Value.Rank == Rank.Detective)
					{
						if(_detectivelist.ContainsKey(function.Key))
							return _detectivelist[function.Key];
						else if(_ghostlist.ContainsKey(function.Key))
							return _ghostlist[function.Key];
					}
				}
			}
			else
			{
				string names = string.Empty;
				foreach(var function in _playerflist)
				{
					if(function.Value.Rank == Rank.Detective)
					{
						if(_detectivelist.ContainsKey(function.Key))
							names += SchumixBase.Space + _detectivelist[function.Key];
						else if(_ghostlist.ContainsKey(function.Key))
							names += SchumixBase.Space + _ghostlist[function.Key];
					}
				}

				names = names.Remove(0, 1, SchumixBase.Space);
				var split = names.Split(SchumixBase.Space);

				if(split.Length == 2)
					return split[0] + " és " + split[1];
				else
					return "Nem tudom kik =(";
			}

			return "Nem tudom ki vagy kik =(";
		}

		public string GetDoctor()
		{
			foreach(var function in _playerflist)
			{
				if(function.Value.Rank == Rank.Doctor)
				{
					if(_doctorlist.ContainsKey(function.Key))
						return _doctorlist[function.Key];
					else if(_ghostlist.ContainsKey(function.Key))
						return _ghostlist[function.Key];
				}
			}

			return "Nem tudom ki =(";
		}

		public void NewOwnerTime()
		{
			OwnerMsgTime = DateTime.Now;
		}

		private void HandleIsOwnerAfk(object sender, ElapsedEventArgs e)
		{
			if((DateTime.Now - OwnerMsgTime).Minutes >= 10 && _owner != string.Empty)
			{
				_owner = string.Empty;
				var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
				var text = sLManager.GetCommandTexts("maffiagame/base/handleisownerafk", _channel, _servername);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
					return;
				}

				sSendMessage.SendCMPrivmsg(_channel, text[0]);
				sSendMessage.SendCMPrivmsg(_channel, text[1]);

				_timerowner.Enabled = false;
				_timerowner.Elapsed -= HandleIsOwnerAfk;
				_timerowner.Stop();
			}
		}

		private void RemovePlayer(string Name, string channel)
		{
			if(Name.Replace(SchumixBase.Space.ToString(), string.Empty) == string.Empty)
				return;

			if(Started)
			{
				if(_killerlist.ContainsKey(Name.ToLower()))
				{
					Name = _killerlist[Name.ToLower()];
					_killerlist.Remove(Name.ToLower());
				}
				else if(_detectivelist.ContainsKey(Name.ToLower()))
				{
					Name = _detectivelist[Name.ToLower()];
					_detectivelist.Remove(Name.ToLower());
				}
				else if(_doctorlist.ContainsKey(Name.ToLower()))
				{
					Name = _doctorlist[Name.ToLower()];
					_doctorlist.Remove(Name.ToLower());
				}
				else if(_normallist.ContainsKey(Name.ToLower()))
				{
					Name = _normallist[Name.ToLower()];
					_normallist.Remove(Name.ToLower());
				}
			}

			if(_joinlist.Contains(Name.ToLower()))
				_joinlist.Remove(Name.ToLower());

			if(_gameoverlist.Contains(Name.ToLower()))
				_gameoverlist.Remove(Name.ToLower());

			int i = 0;
			foreach(var player in _playerlist)
			{
				if(player.Value.ToLower() == Name.ToLower())
				{
					i = player.Key;
					break;
				}
			}

			_playerlist.Remove(i);

			if(_playerflist.ContainsKey(Name.ToLower()))
				_playerflist[Name.ToLower()].Ghost = true;

			if(Started && !_ghostlist.ContainsKey(Name.ToLower()))
				_ghostlist.Add(Name.ToLower(), Name);

			SchumixBase.DManager.Update("maffiagame", "Survivor = '0'", string.Format("Name = '{0}' AND Game = '{1}' And ServerName = '{2}'", Name, _gameid, _servername));

			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
			var sSender = sIrcBase.Networks[_servername].sSender;
			sSender.Mode(_channel, "-v", Name);

			if(channel != string.Empty)
				sSendMessage.SendCMPrivmsg(channel, "Meghaltál. Kérlek maradj csendben amíg a játék véget ér.");
		}

		private void Corpse(string Name)
		{
			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
			var text = sLManager.GetCommandTexts("maffiagame/base/corpse", _channel, _servername);
			if(text.Length < 4)
			{
				sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
				return;
			}

			var rank = GetRank(Name);

			if(rank == Rank.Killer)
				sSendMessage.SendCMPrivmsg(_channel, text[0]);
			else if(rank == Rank.Detective)
				sSendMessage.SendCMPrivmsg(_channel, text[1]);
			else if(rank == Rank.Doctor)
				sSendMessage.SendCMPrivmsg(_channel, text[2]);
			else if(rank == Rank.Normal)
				sSendMessage.SendCMPrivmsg(_channel, text[3]);
		}

		public void EndText()
		{
			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
			var text = sLManager.GetCommandTexts("maffiagame/base/endtext", _channel, _servername);
			if(text.Length < 2)
			{
				sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
				return;
			}

			if(_players < 8)
				sSendMessage.SendCMPrivmsg(_channel, text[0], GetKiller(), GetDetective());
			else
				sSendMessage.SendCMPrivmsg(_channel, text[1], GetKiller(), GetDetective(), GetDoctor());
		}

		public void EndGameText()
		{
			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
			sSendMessage.SendCMPrivmsg(_channel, sLManager.GetCommandText("maffiagame/base/endgametext", _channel, _servername));
		}

		private bool IsRunning(string Channel, string Name = "")
		{
			if(!Running)
			{
				var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
				var text = sLManager.GetCommandTexts("maffiagame/base/isrunning", _channel, _servername);
				if(text.Length < 2)
				{
					sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
					return false;
				}

				if(Name == string.Empty)
					sSendMessage.SendCMPrivmsg(_channel, text[0]);
				else
					sSendMessage.SendCMPrivmsg(_channel, text[1], Name);

				return false;
			}
			else
				return true;
		}

		private bool IsStarted(string Channel, string Name)
		{
			if(!Running)
			{
				var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
				sSendMessage.SendCMPrivmsg(Channel, sLManager.GetCommandText("maffiagame/base/isstarted", _channel, _servername), Name);
				return false;
			}
			else
				return true;
		}

		private Rank GetRank(string Name)
		{
			return _playerflist.ContainsKey(Name.ToLower()) ? _playerflist[Name.ToLower()].Rank : Rank.None;
		}

		private void AddRanks()
		{
			var sSender = sIrcBase.Networks[_servername].sSender;
			int i = 0;
			string namesss = string.Empty;
			var list = new List<string>();

			foreach(var end in _playerlist)
			{
				i++;
				namesss += SchumixBase.Space + end.Value;

				if(i == 4)
				{
					i = 0;
					list.Add(namesss.Remove(0, 1, SchumixBase.Space));
					namesss = string.Empty;
				}
			}

			foreach(var l in list)
				sSender.Mode(_channel, "+vvvv", l);

			list.Clear();
			namesss = namesss.Remove(0, 1, SchumixBase.Space);

			if(namesss != string.Empty)
			{
				var split = namesss.Split(SchumixBase.Space);

				if(split.Length == 1)
					sSender.Mode(_channel, "+v", namesss);
				else if(split.Length == 2)
					sSender.Mode(_channel, "+vv", namesss);
				else if(split.Length == 3)
					sSender.Mode(_channel, "+vvv", namesss);
			}
		}

		public void RemoveRanks(bool night = false)
		{
			if(!night)
				Running = false;

			var sSender = sIrcBase.Networks[_servername].sSender;
			int i = 0;
			string namesss = string.Empty;
			var list = new List<string>();

			foreach(var end in _playerlist)
			{
				i++;
				namesss += SchumixBase.Space + end.Value;

				if(i == 4)
				{
					i = 0;
					list.Add(namesss.Remove(0, 1, SchumixBase.Space));
					namesss = string.Empty;
				}
			}

			foreach(var l in list)
				sSender.Mode(_channel, "-vvvv", l);

			list.Clear();
			namesss = namesss.Remove(0, 1, SchumixBase.Space);

			if(namesss != string.Empty)
			{
				var split = namesss.Split(SchumixBase.Space);

				if(split.Length == 1)
					sSender.Mode(_channel, "-v", namesss);
				else if(split.Length == 2)
					sSender.Mode(_channel, "-vv", namesss);
				else if(split.Length == 3)
					sSender.Mode(_channel, "-vvv", namesss);
			}

			if(!night)
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

		private bool GetPlayerMaster(string Name)
		{
			bool master = false;

			foreach(var function in _playerflist)
			{
				if(function.Key == Name.ToLower())
					master = function.Value.Master;
			}

			return master;
		}

		private void StartThread()
		{
			_thread = new Thread(Game);
			_thread.Start();
		}

		public void StopThread()
		{
			var sChannelInfo = sIrcBase.Networks[_servername].sChannelInfo;
			var sSender = sIrcBase.Networks[_servername].sSender;
			Running = false;

			if(!sGameCommand.GameChannelFunction.ContainsKey(_channel))
				return;

			SchumixBase.DManager.Update("maffiagame", "Active = '0'", string.Format("Active = '1' AND Game = '{0}' And ServerName = '{1}'", _gameid, _servername));
			SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sGameCommand.GameChannelFunction[_channel]), string.Format("Channel = '{0}' And ServerName = '{1}'", _channel, _servername));
			sChannelInfo.ChannelFunctionsReload();
			SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", sChannelInfo.ChannelFunctions("gamecommands", SchumixBase.Off, _channel)), string.Format("Channel = '{0}' And ServerName = '{1}'", _channel, _servername));
			sChannelInfo.ChannelFunctionsReload();

			if(Started)
				_thread.Abort();

			if(_players >= 15)
			{
				sSender.Part(_killerchannel);
				SchumixBase.DManager.Delete("channels", string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(_killerchannel), _servername));
				sChannelInfo.ChannelListReload();
				sChannelInfo.ChannelFunctionsReload();
			}

			if(_timerowner.Enabled)
			{
				_timerowner.Enabled = false;
				_timerowner.Elapsed -= HandleIsOwnerAfk;
				_timerowner.Stop();
			}

			Clean();
			Started = false;
			sGameCommand.GameChannelFunction.Remove(_channel);
		}

		private void Game()
		{
			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
			var text = sLManager.GetCommandTexts("maffiagame/base/game", _channel, _servername);
			if(text.Length < 43)
			{
				sSendMessage.SendCMPrivmsg(_channel, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(_channel, _servername)));
				return;
			}

			try
			{
				bool newghost = false;
				bool enabledk = false;
				bool enableddoctor = false;
				bool enabledkiller = false;
				bool enableddetective = false;
				string names = string.Empty;
				string newkillghost = string.Empty;

				if(_players < 8)
					sSendMessage.SendCMPrivmsg(_channel, text[0]);
				else if(_players >= 8 && _players < 15)
					sSendMessage.SendCMPrivmsg(_channel, text[1]);
				else if(_players >= 15)
					sSendMessage.SendCMPrivmsg(_channel, text[2]);

				sSendMessage.SendCMPrivmsg(_channel, text[3]);
				sSendMessage.SendCMPrivmsg(_channel, text[4]);
				sSendMessage.SendCMPrivmsg(_channel, text[5]);
				Thread.Sleep(2000);

				for(;;)
				{
					Thread.Sleep(1000);

					if(_killerlist.Count == 1 && Started)
					{
						foreach(var function in _playerflist)
						{
							if(function.Value.Rank == Rank.Killer && function.Value.RName != string.Empty && !function.Value.Ghost)
							{
								newkillghost = GetPlayerName(function.Value.RName.Trim());
								enabledkiller = true;
							}
						}
					}
					else if(_killerlist.Count == 2 && Started)
					{
						var list = new List<string>();

						foreach(var function in _playerflist)
						{
							if(function.Value.Rank == Rank.Killer && function.Value.RName != string.Empty && !function.Value.Ghost)
								list.Add(function.Value.RName);
						}

						if(list.Count == 2)
						{
							if(list.CompareDataInBlock() && !enabledk)
							{
								foreach(var kill in _killerlist)
									sSendMessage.SendCMPrivmsg(kill.Key, text[6]);

								enabledk = true;
								newkillghost = GetPlayerName(list[0].Trim());
								enabledkiller = true;
							}
							else if(!list.CompareDataInBlock())
								enabledk = false;
						}

						list.Clear();
					}
					else if(_killerlist.Count == 3 && Started)
					{
						var list = new List<string>();

						foreach(var function in _playerflist)
						{
							if(function.Value.Rank == Rank.Killer && function.Value.RName != string.Empty && !function.Value.Ghost)
								list.Add(function.Value.RName);
						}

						if(list.Count == 3)
						{
							if(list.CompareDataInBlock() && !enabledk)
							{
								foreach(var kill in _killerlist)
									sSendMessage.SendCMPrivmsg(kill.Key, text[6]);

								enabledk = true;
								newkillghost = GetPlayerName(list[0].Trim());
								enabledkiller = true;
							}
							else if(!list.CompareDataInBlock())
								enabledk = false;
						}

						list.Clear();
					}

					if(_detectivelist.Count == 1 && Started)
					{
						foreach(var function in _playerflist)
						{
							if(function.Value.Rank == Rank.Detective && function.Value.Detective && !function.Value.Ghost)
								enableddetective = true;
						}
					}
					else if(_detectivelist.Count == 2 && Started)
					{
						int number = 0;

						foreach(var function in _playerflist)
						{
							if(function.Value.Rank == Rank.Detective && function.Value.Detective && !function.Value.Ghost)
								number++;
						}

						if(number == 2)
							enableddetective = true;
					}
					else if(_detectivelist.Count == 0 && Started)
						enableddetective = true;

					if(_doctorlist.Count == 1 && Started)
					{
						foreach(var function in _playerflist)
						{
							if(function.Value.Rank == Rank.Doctor && function.Value.RName != string.Empty && !function.Value.Ghost)
								enableddoctor = true;
						}
					}
					else if(_doctorlist.Count == 0 && Started)
						enableddoctor = true;

					if(enabledkiller && enableddetective && enableddoctor)
					{
						foreach(var function in _playerflist)
						{
							if(function.Value.Rank == Rank.Doctor)
							{
								if((newkillghost.ToLower() != function.Value.RName) && (newkillghost.ToLower() != string.Empty))
									newghost = true;
							}

							function.Value.RName = string.Empty;
						}

						foreach(var function in _playerflist)
						{
							if(function.Value.Rank == Rank.Detective)
							{
								if((function.Key == newkillghost.ToLower() && newghost) ||
								   (function.Key == newkillghost.ToLower() && _players < 8))
									continue;

								if(function.Value.DRank == Rank.Killer)
									sSendMessage.SendCMPrivmsg(function.Key, text[7]);
								else if(function.Value.DRank == Rank.Normal)
									sSendMessage.SendCMPrivmsg(function.Key, text[8]);
								else if(function.Value.DRank == Rank.Doctor)
									sSendMessage.SendCMPrivmsg(function.Key, text[9]);
								else if(function.Value.DRank == Rank.Detective)
									sSendMessage.SendCMPrivmsg(function.Key, text[10]);

								function.Value.Detective = false;
								function.Value.DRank = Rank.None;
							}
						}

						_day = true;
						_stop = false;
						enabledk = false;
						enabledkiller = false;
						enableddoctor = false;
						enableddetective = false;

						if(_players >= 8)
						{
							if(newghost)
								RemovePlayer(newkillghost, newkillghost);
						}
						else
						{
							newghost = true;
							RemovePlayer(newkillghost, newkillghost);
						}

						EndGame(newkillghost, true);
					}

					if(!Started)
						StopThread();

					if(!_lynch)
						EndGame();

					if(!_day)
					{
						if(_stop)
							continue;

						_stop = true;

						foreach(var function in _playerflist)
							function.Value.Lynch.Clear();

						if(NoVoice)
							RemoveRanks(true);

						names = string.Empty;

						foreach(var name in _playerlist)
							names += ", " + name.Value;

						sSendMessage.SendCMPrivmsg(_channel, text[11], names.Remove(0, 2, ", "));
						sSendMessage.SendCMPrivmsg(_channel, text[12]);
						sSendMessage.SendCMPrivmsg(_channel, text[13]);
						Thread.Sleep(1000);

						foreach(var name in _killerlist)
						{
							sSendMessage.SendCMPrivmsg(name.Key, text[14]);
							sSendMessage.SendCMPrivmsg(name.Key, text[15]);
							sSendMessage.SendCMPrivmsg(name.Key, text[16]);
							Thread.Sleep(400);
						}

						if(_players >= 8 && _players < 15 && _killerlist.Count == 2)
						{
							names = string.Empty;
							foreach(var name in _killerlist)
								names += SchumixBase.Space + name.Key;

							names = names.Remove(0, 1, SchumixBase.Space);
							var split = names.Split(SchumixBase.Space);

							foreach(var name in _killerlist)
							{
								if(name.Key == split[0])
									sSendMessage.SendCMPrivmsg(name.Key, text[17], split[1]);
								else
									sSendMessage.SendCMPrivmsg(name.Key, text[17], split[0]);

								Thread.Sleep(400);
							}
						}
						else if(_players >= 15)
						{
							foreach(var name in _killerlist)
							{
								sSendMessage.SendCMPrivmsg(name.Key, text[18], _killerchannel);
								Thread.Sleep(400);
							}
						}

						if(_players >= 8)
						{
							foreach(var name in _doctorlist)
							{
								sSendMessage.SendCMPrivmsg(name.Key, text[19]);
								sSendMessage.SendCMPrivmsg(name.Key, text[20]);
								Thread.Sleep(400);
							}
						}

						foreach(var name in _detectivelist)
						{
							sSendMessage.SendCMPrivmsg(name.Key, text[21]);
							sSendMessage.SendCMPrivmsg(name.Key, text[22]);
							Thread.Sleep(400);
						}
					}
					else
					{
						if(_stop)
							continue;

						_stop = true;
						sSendMessage.SendCMPrivmsg(_channel, text[23]);

						if(NoVoice)
							AddRanks();

						if(newghost)
						{
							sSendMessage.SendCMPrivmsg(_channel, text[24], newkillghost);

							if(GetPlayerMaster(newkillghost))
								sSendMessage.SendCMPrivmsg(_channel, text[25]);

							Corpse(newkillghost);
							sSendMessage.SendCMPrivmsg(_channel, text[26], newkillghost);
						}
						else
							sSendMessage.SendCMPrivmsg(_channel, text[27]);

						newghost = false;
						newkillghost = string.Empty;

						names = string.Empty;
						foreach(var name in _playerlist)
							names += ", " + name.Value;

						sSendMessage.SendCMPrivmsg(_channel, text[28], names.Remove(0, 2, ", "));

						/*names = string.Empty;
						foreach(var name in _ghostlist)
							names += ", " + name.Value;

						sSendMessage.SendCMPrivmsg(_channel, "A következő személyek halottak: {0}", names.Remove(0, 2, ", "));*/
						sSendMessage.SendCMPrivmsg(_channel, text[29], _ghostlist.Count);

						if(!NoLynch)
						{
							sSendMessage.SendCMPrivmsg(_channel, text[30]);
							sSendMessage.SendCMPrivmsg(_channel, text[31]);
							sSendMessage.SendCMPrivmsg(_channel, text[32]);
							sSendMessage.SendCMPrivmsg(_channel, text[33]);
							sSendMessage.SendCMPrivmsg(_channel, text[34]);
						}
						else
						{
							sSendMessage.SendCMPrivmsg(_channel, text[35]);
							sSendMessage.SendCMPrivmsg(_channel, text[36]);
							Thread.Sleep(30*1000);
							sSendMessage.SendCMPrivmsg(_channel, text[37]);
							Thread.Sleep(30*1000);
							sSendMessage.SendCMPrivmsg(_channel, text[38]);
							Thread.Sleep(30*1000);
							sSendMessage.SendCMPrivmsg(_channel, text[39]);
							Thread.Sleep(20*1000);
							sSendMessage.SendCMPrivmsg(_channel, text[40]);
							Thread.Sleep(5*1000);
							sSendMessage.SendCMPrivmsg(_channel, text[41]);
							Thread.Sleep(5*1000);
							sSendMessage.SendCMPrivmsg(_channel, text[42]);

							_day = false;
							_stop = false;
						}
					}
				}
			}
			catch(Exception e)
			{
				if(Running && Started)
				{
					RemoveRanks();
					sSendMessage.SendCMPrivmsg(_channel, "Meghibásodás történt a játékban! Oka: ", e.Message);
					EndGameText();
					EndText();
					StopThread();
				}

				return;
			}
		}

		private void EndGame(bool ghosttext = false)
		{
			EndGame(string.Empty, ghosttext);
		}

		private void EndGame(string newghost, bool ghosttext = false)
		{
			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;

			if(_killerlist.Count == 0 && Running)
			{
				RemoveRanks();
				sSendMessage.SendCMPrivmsg(_channel, "A gyilkosok halottak! A 4falusiak győztek.");
				EndGameText();
				EndText();
				StopThread();
				return;
			}
			else
			{
				if((_killerlist.Count >= _detectivelist.Count + _doctorlist.Count + _normallist.Count) && Running)
				{
					RemoveRanks();

					if(_killerlist.Count >= 1 && ghosttext)
					{
						if(newghost != string.Empty)
							sSendMessage.SendCMPrivmsg(_channel, "A falusiakat szörnyű látvány fogadja: megtalálták 4{0} holttestét!", newghost);

						SchumixBase.DManager.Update("maffiagame", "Survivor = '0'", string.Format("Name = '{0}' AND Game = '{1}' And ServerName = '{2}'", newghost, _gameid, _servername));
						Corpse(newghost);
					}

					sSendMessage.SendCMPrivmsg(_channel, "A falusiak halottak! A 4gyilkosok győztek.");
					EndGameText();
					EndText();
					StopThread();
					return;
				}
				else if((_playerlist.Count <= 2) && Running)
				{
					RemoveRanks();
					sSendMessage.SendCMPrivmsg(_channel, "Elfogytak a játékosok!");
					EndGameText();
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