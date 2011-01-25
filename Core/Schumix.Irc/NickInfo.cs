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
using Schumix.Framework.Config;

namespace Schumix.Irc
{
	public class NickInfo
	{
		private string _NickStorage;
		public string NickStorage
		{
			get
			{
				return _NickStorage;
			}
		}

		private NickInfo() {}

		public void ChangeNick()
		{
			if(_NickStorage == IRCConfig.NickName)
			{
				_NickStorage = IRCConfig.NickName2;
				return;
			}
			else if(_NickStorage == IRCConfig.NickName2)
			{
				_NickStorage = IRCConfig.NickName3;
				return;
			}
			else if(_NickStorage == IRCConfig.NickName3)
			{
				_NickStorage = "_Schumix2";
				return;
			}
			else if(_NickStorage == "_Schumix2")
			{
				_NickStorage = "__Schumix2";
				return;
			}
			else if(_NickStorage == "__Schumix2")
			{
				_NickStorage = "_Schumix2_";
				return;
			}
			else if(_NickStorage == "_Schumix2_")
			{
				_NickStorage = "__Schumix2_";
				return;
			}
			else if(_NickStorage == "__Schumix2_")
			{
				_NickStorage = "__Schumix2__";
				return;
			}
			else if(_NickStorage == "__Schumix2__")
			{
				_NickStorage = IRCConfig.NickName;
				return;
			}
			else
			{
				_NickStorage = IRCConfig.NickName;
				return;
			}
		}

		public void ChangeNick(string newnick)
		{
			_NickStorage = newnick;
		}
	}
}
