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
using Schumix.ExtraAddon;

namespace Schumix.ExtraAddon.Commands
{
	public class NameList
	{
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly List<string> _names = new List<string>();
		private NameList() {}

		public void Add(string Channel, string Name)
		{
			//Console.WriteLine(Name.ToLower());
			//_names.Add(Channel.ToLower() + SchumixBase.Colon + Name.ToLower());
		}

		public void Remove(string Channel)
		{
			/*for(int i = 0; i < _names.Count; i++)
			{
				if(_names.Contains(Channel.ToLower()))
					_names.Remove(_names[i]);
				Console.WriteLine(_names[i]);
			}*/
		}

		public void Remove(string Channel, string Name, bool NewNick)
		{
			/*Console.WriteLine(_names.Count);
			int x = _names.Count;

			for(int i = 0; i < x; i++)
			{
				if(_names[i].Contains(Name.ToLower()) && !NewNick)
					_names.Remove(_names[i]);
				else if(_names[i].Contains(Channel.ToLower() + SchumixBase.Colon + Name.ToLower()) && NewNick)
					_names.Remove(_names[i]);

				Console.WriteLine(_names[i]);
				Console.WriteLine(x);
			}

			Console.WriteLine(_names.Count);

			if(IRCConfig.NickName.ToLower() == Name.ToLower() && !_names.Contains(Name.ToLower()) && !NewNick)
			{
				ExtraAddon.IsOnline = true;
				sSender.NickServInfo(Name);
			}*/
		}

		public void Remove(string Channel, string Name)
		{
			//if(_names.Contains(Channel.ToLower() + SchumixBase.Colon + Name.ToLower()))
			//	_names.Remove(Channel.ToLower() + SchumixBase.Colon + Name.ToLower());
		}

		public void RemoveAll()
		{
			_names.Clear();
		}
	}
}