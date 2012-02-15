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
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Config;

namespace Schumix.Irc
{
	public sealed class NickName
	{
		private List<string> Names = new List<string>();

		private NickName()
		{
			string[] ignore = IRCConfig.IgnoreNames.Split(SchumixBase.Comma);

			if(ignore.Length > 1)
			{
				foreach(var name in ignore)
					Names.Add(name.ToLower());
			}
			else
				Names.Add(IRCConfig.IgnoreNames.ToLower());
		}

		public bool IsIgnore(string Name)
		{
			foreach(var name in Names)
			{
				if(Name.ToLower() == name)
					return true;
			}

			return false;
		}

		public void Add(string Name)
		{
			if(!Names.Contains(Name.ToLower()))
				Names.Add(Name.ToLower());
		}

		public void Remove(string Name)
		{
			if(Names.Contains(Name.ToLower()))
				Names.Remove(Name.ToLower());
		}

		public void Clean()
		{
			Names.Clear();
		}
	}
}