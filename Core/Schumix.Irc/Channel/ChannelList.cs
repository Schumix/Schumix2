/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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
using Schumix.Irc.Flood;
using Schumix.Irc.NickName;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Channel
{
	public sealed class ChannelList : CommandInfo
	{
		private readonly Dictionary<string, ChannelInfo> _list = new Dictionary<string, ChannelInfo>();
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly SendMessage sSendMessage;
		private readonly AntiFlood sAntiFlood;
		private string _servername;

		public ChannelList(string ServerName) : base(ServerName)
		{
			_servername = ServerName;
			sAntiFlood = sIrcBase.Networks[ServerName].sAntiFlood;
			sSendMessage = sIrcBase.Networks[ServerName].sSendMessage;
		}

		public Dictionary<string, ChannelInfo> List
		{
			get { return _list; }
		}

		public void Add(string Channel, string Name)
		{
			if(_list.ContainsKey(Channel.ToLower()))
			{
				if(!_list[Channel.ToLower()].Names.ContainsKey(Name.ToLower()))
					_list[Channel.ToLower()].Names.Add(Name.ToLower(), new NickInfo());
			}
			else
			{
				_list.Add(Channel.ToLower(), new ChannelInfo());
				_list[Channel.ToLower()].Names.Add(Name.ToLower(), new NickInfo());
			}
		}

		public void Remove(string Channel)
		{
			if(_list.ContainsKey(Channel.ToLower()))
			{
				var db = SchumixBase.DManager.Query("SELECT Name FROM admins WHERE ServerName = '{0}'", _servername);
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						int i = 0;
						string name = row["Name"].ToString();

						foreach(var ch in _list)
						{
							if(ch.Key != Channel.ToLower() && ch.Value.Names.ContainsKey(name.ToLower()))
								i++;
						}

						if(i == 0 && _list[Channel.ToLower()].Names.ContainsKey(name.ToLower()))
							SchumixBase.DManager.Update("admins", string.Format("Vhost = '{0}'", sUtilities.GetRandomString()), string.Format("Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername));
					}
				}

				_list.Remove(Channel.ToLower());
			}
		}

		public void Remove(string Channel, string Name, bool Quit = false)
		{
			if(_list.ContainsKey(Channel.ToLower()))
			{
				if(_list[Channel.ToLower()].Names.ContainsKey(Name.ToLower()))
				{
					int i = 0;
					_list[Channel.ToLower()].Names.Remove(Name.ToLower());

					foreach(var ch in _list)
					{
						if(ch.Value.Names.ContainsKey(Name.ToLower()))
							i++;
					}
					
					if(i == 0)
						RandomVhost(Name.ToLower());
				}
			}
			else if(Quit)
			{
				foreach(var chan in _list)
				{
					if(chan.Value.Names.ContainsKey(Name.ToLower()))
						chan.Value.Names.Remove(Name.ToLower());
				}
				
				RandomVhost(Name.ToLower());
				sAntiFlood.Remove(Name.ToLower());
			}
		}

		public void Change(string Name, string NewName)
		{
			foreach(var chan in _list)
			{
				if(chan.Value.Names.ContainsKey(Name.ToLower()))
				{
					chan.Value.Names.Add(NewName.ToLower(), chan.Value.Names[Name.ToLower()]);
					chan.Value.Names.Remove(Name.ToLower());
				}
			}

			RandomVhost(Name.ToLower());
			sAntiFlood.Remove(Name.ToLower());
		}

		public bool IsChannelList(string Name)
		{
			foreach(var chan in _list)
			{
				if(chan.Value.Names.ContainsKey(Name.ToLower()))
					return true;
			}

			return false;
		}

		public void NewThread(string ServerName, string Name)
		{
			Task.Factory.StartNew(() =>
			{
				Thread.Sleep(5*60*1000);

				if(!IsChannelList(Name))
				{
					sSendMessage.SendCMPrivmsg(Name.ToLower(), sLManager.GetWarningText("NoRegisteredAdminAccess"));
					RandomVhost(Name);
				}
			});
		}

		public ChannelRank StringToChannelRank(string Value)
		{
			var rank = ChannelRank.None;
			
			switch(Value)
			{
			case "~":
			case "q":
				rank = ChannelRank.Owner;
				break;
			case "&":
			case "a":
				rank = ChannelRank.Protected;
				break;
			case "@":
			case "o":
				rank = ChannelRank.Operator;
				break;
			case "%":
			case "h":
				rank = ChannelRank.HalfOperator;
				break;
			case "+":
			case "v":
				rank = ChannelRank.Voice;
				break;
			}
			
			return rank;
		}
		
		public bool IsChannelRank(string Value)
		{
			switch(Value)
			{
			case "~":
			case "q":
				return true;
			case "&":
			case "a":
				return true;
			case "@":
			case "o":
				return true;
			case "%":
			case "h":
				return true;
			case "+":
			case "v":
				return true;
			}
			
			return false;
		}

		public void RemoveAll()
		{
			_list.Clear();
		}
	}
}