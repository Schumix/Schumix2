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
using System.Collections.Generic;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.ExtraAddon;

namespace Schumix.ExtraAddon.Commands
{
	public class NameList
	{
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly Dictionary<string, string> _names = new Dictionary<string, string>();
		private NameList() {}

		public void Add(string Channel, string Name)
		{
			if(_names.ContainsKey(Channel.ToLower()))
			{
				string channel = _names[Channel.ToLower()];

				if(!channel.Contains(Name.ToLower()))
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
				_names.Remove(Channel.ToLower());
		}

		public void Remove(string Channel, string Name, bool Quit = false)
		{
			if(_names.ContainsKey(Channel.ToLower()))
			{
				if(_names[Channel.ToLower()].Contains(Name.ToLower()))
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

					_names.Add(Channel.ToLower(), names.Remove(0, 1, SchumixBase.Comma));
				}
			}
			else if(Quit)
			{
				var channel = new Dictionary<string, string>();

				foreach(var chan in _names)
				{
					if(chan.Value.Contains(Name.ToLower()))
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

				if(IRCConfig.NickName.ToLower() == Name.ToLower())
				{
					ExtraAddon.IsOnline = true;
					sSender.NickServInfo(Name);
				}
			}
		}

		public void Change(string Name, string NewName, bool Identify = false)
		{
			var channel = new Dictionary<string, string>();

			foreach(var chan in _names)
			{
				if(chan.Value.Contains(Name.ToLower()))
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

			if(IRCConfig.NickName.ToLower() == Name.ToLower() && !Identify)
			{
				ExtraAddon.IsOnline = true;
				sSender.NickServInfo(Name);
			}
		}

		public void RemoveAll()
		{
			_names.Clear();
		}
	}
}