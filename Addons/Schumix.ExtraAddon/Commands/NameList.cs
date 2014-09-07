/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.ExtraAddon;

namespace Schumix.ExtraAddon.Commands
{
	class NameList
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Dictionary<string, string> _names = new Dictionary<string, string>();
		private readonly Dictionary<string, bool> _channels = new Dictionary<string, bool>();
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private Functions sFunctions;
		private string _servername;

		public NameList(string ServerName, Functions fs)
		{
			_servername = ServerName;
			sFunctions = fs;
		}

		public Dictionary<string, bool> Channels
		{
			get { return _channels; }
		}

		public void Add(string Channel, string Name)
		{
			if(_names.ContainsKey(Channel.ToLower()))
			{
				string channel = _names[Channel.ToLower()];

				if(!channel.Contains(Name.ToLower(), SchumixBase.Comma))
				{
					_names.Remove(Channel.ToLower());
					_names.Add(Channel.ToLower(), channel + SchumixBase.Comma + Name.ToLower());
				}
			}
			else
				_names.Add(Channel.ToLower(), Name.ToLower());
		}

		public void Remove(string Channel)
		{
			if(_names.ContainsKey(Channel.ToLower()))
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM notes_users WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						int i = 0;
						string name = row["Name"].ToString();

						foreach(var channel in _names)
						{
							if(channel.Key != Channel.ToLower() && channel.Value.Contains(name.ToLower(), SchumixBase.Comma))
								i++;
						}

						if(i == 0 && _names[Channel.ToLower()].Contains(name.ToLower(), SchumixBase.Comma))
							SchumixBase.DManager.Update("notes_users", string.Format("Vhost = '{0}'", sUtilities.GetRandomString()), string.Format("Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername));
					}
				}

				_names.Remove(Channel.ToLower());
			}
		}

		public void Remove(string Channel, string Name, bool Quit = false)
		{
			var sSender = sIrcBase.Networks[_servername].sSender;

			if(_names.ContainsKey(Channel.ToLower()))
			{
				if(_names[Channel.ToLower()].Contains(Name.ToLower(), SchumixBase.Comma))
				{
					string value = _names[Channel.ToLower()];
					_names.Remove(Channel.ToLower());
					string names = string.Empty;
					var split = value.Split(SchumixBase.Comma);

					foreach(var name in split)
					{
						if(name != Name.ToLower())
							names += SchumixBase.Comma + name;
					}

					int i = 0;
					_names.Add(Channel.ToLower(), names.Remove(0, 1, SchumixBase.Comma));

					foreach(var Channels in _names)
					{
						if(Channels.Value.Contains(Name.ToLower(), SchumixBase.Comma))
							i++;
					}

					if(i == 0)
						RandomVhost(Name.ToLower());
				}
			}
			else if(Quit)
			{
				var channel = new Dictionary<string, string>();

				foreach(var chan in _names)
				{
					if(chan.Value.Contains(Name.ToLower(), SchumixBase.Comma))
						channel.Add(chan.Key, chan.Value);
				}

				if(channel.Count.IsNull())
				{
					channel.Clear();
					return;
				}

				foreach(var chan in channel)
				{
					_names.Remove(chan.Key);
					string names = string.Empty;
					var split = chan.Value.Split(SchumixBase.Comma);

					foreach(var name in split)
					{
						if(name != Name.ToLower())
							names += SchumixBase.Comma + name;
					}

					_names.Add(chan.Key, names.Remove(0, 1, SchumixBase.Comma));
				}

				channel.Clear();
				RandomVhost(Name.ToLower());

				if(IRCConfig.List[_servername].NickName.ToLower() == Name.ToLower())
				{
					sFunctions.IsOnline = true;
					sSender.NickServInfo(Name);
				}
			}
		}

		public void Change(string Name, string NewName, bool Identify = false)
		{
			var sSender = sIrcBase.Networks[_servername].sSender;
			var channel = new Dictionary<string, string>();

			foreach(var chan in _names)
			{
				if(chan.Value.Contains(Name.ToLower(), SchumixBase.Comma))
					channel.Add(chan.Key, chan.Value);
			}

			if(channel.Count.IsNull())
			{
				channel.Clear();
				return;
			}

			foreach(var chan in channel)
			{
				_names.Remove(chan.Key);
				string names = string.Empty;
				var split = chan.Value.Split(SchumixBase.Comma);

				foreach(var name in split)
				{
					if(name != Name.ToLower())
						names += SchumixBase.Comma + name;
					else
						names += SchumixBase.Comma + NewName.ToLower();
				}

				_names.Add(chan.Key, names.Remove(0, 1, SchumixBase.Comma));
			}

			channel.Clear();
			RandomVhost(Name.ToLower());

			// Azt ellenőrzi le felszabadul-e az elsődleges nick neve.
			if(IRCConfig.List[_servername].NickName.ToLower() == Name.ToLower() && !Identify)
			{
				sIrcBase.Networks[_servername].sMyNickInfo.ChangeIdentifyStatus(false);
				sFunctions.IsOnline = true;
				sSender.NickServInfo(Name);
			}
		}

		public bool IsChannelList(string Name)
		{
			foreach(var chan in _names)
			{
				if(chan.Value.Contains(Name.ToLower(), SchumixBase.Comma))
					return true;
			}

			return false;
		}

		public void NewThread(string Name)
		{
			Task.Factory.StartNew(() =>
			{
				var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;
				Thread.Sleep(5*60*1000);

				if(!IsChannelList(Name))
				{
					sSendMessage.SendCMPrivmsg(Name.ToLower(), sLManager.GetWarningText("NoRegisteredNotesUserAccess"), _servername);
					RandomVhost(Name);
				}
			});
		}

		public void RemoveAll()
		{
			_names.Clear();
		}

		public void RandomVhost(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM notes_users WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			if(!db.IsNull())
				SchumixBase.DManager.Update("notes_users", string.Format("Vhost = '{0}'", sUtilities.GetRandomString()), string.Format("Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername));
		}

		public void RandomAllVhost()
		{
			var db = SchumixBase.DManager.Query("SELECT Name FROM notes_users WHERE ServerName = '{0}'", _servername);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
					SchumixBase.DManager.Update("notes_users", string.Format("Vhost = '{0}'", sUtilities.GetRandomString()), string.Format("Name = '{0}' And ServerName = '{1}'", row["Name"].ToString(), _servername));
			}
		}
	}
}