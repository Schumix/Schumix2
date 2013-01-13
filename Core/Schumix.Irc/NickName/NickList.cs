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
using System.Collections.Generic;

namespace Schumix.Irc.NickName
{
	public sealed class NickList
	{
		private readonly Dictionary<string, NickInfo> _list = new Dictionary<string, NickInfo>();

		public NickList()
		{
		}
		
		public Dictionary<string, NickInfo> List
		{
			get { return _list; }
		}
		
		public void Add(string Name, string Channel)
		{
			if(_list.ContainsKey(Name.ToLower()))
			{
				if(!_list[Name.ToLower()].Channels.Contains(Channel.ToLower()))
					_list[Name.ToLower()].Channels.Add(Channel.ToLower());
			}
			else
			{
				_list.Add(Name.ToLower(), new NickInfo());
				_list[Name.ToLower()].Channels.Add(Channel.ToLower());
			}
		}
		
		public void Remove(string Name)
		{
			if(_list.ContainsKey(Name.ToLower()))
				_list.Remove(Name.ToLower());
		}
		
		public void Remove(string Name, string Channel, bool Quit = false)
		{
			if(_list.ContainsKey(Name.ToLower()))
			{
				if(_list[Name.ToLower()].Channels.Contains(Channel.ToLower()))
					_list[Name.ToLower()].Channels.Remove(Channel.ToLower());
			}
			else if(Quit)
			{
				if(_list.ContainsKey(Name.ToLower()))
					_list.Remove(Name.ToLower());
			}
		}
		
		public void Change(string Name, string NewName)
		{
			if(_list.ContainsKey(Name.ToLower()))
			{
				_list.Add(NewName.ToLower(), _list[Name.ToLower()]);
				_list.Remove(Name.ToLower());
			}
		}
		
		public bool IsNameList(string Channel)
		{
			foreach(var chan in _list)
			{
				if(chan.Value.Channels.Contains(Channel.ToLower()))
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