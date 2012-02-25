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
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public sealed class IgnoreNickName
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		private IgnoreNickName()
		{
			string[] ignore = IRCConfig.IgnoreNames.Split(SchumixBase.Comma);

			if(ignore.Length > 1)
			{
				foreach(var name in ignore)
					Add(name.ToLower());
			}
			else
				Add(IRCConfig.IgnoreNames.ToLower());
		}

		public bool IsIgnore(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_nicks WHERE Nick = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			return !db.IsNull() ? true : false;
		}

		public void Add(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_nicks WHERE Nick = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(!db.IsNull())
				return;

			SchumixBase.DManager.Insert("`ignore_nicks`(Nick)", sUtilities.SqlEscape(Name.ToLower()));
		}

		public void Remove(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_nicks WHERE Nick = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(db.IsNull())
				return;

			SchumixBase.DManager.Delete("ignore_nicks", string.Format("Nick = '{0}'", sUtilities.SqlEscape(Name.ToLower())));
		}

		public void RemoveConfig()
		{
			string[] ignore = IRCConfig.IgnoreNames.Split(SchumixBase.Comma);

			if(ignore.Length > 1)
			{
				foreach(var name in ignore)
					Remove(name.ToLower());
			}
			else
				Remove(IRCConfig.IgnoreNames.ToLower());
		}
	}

	public sealed class IgnoreIrcCommand
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private IgnoreIrcCommand() {}

		public bool IsIgnore(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_irc_commands WHERE Command = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			return !db.IsNull() ? true : false;
		}

		public void Add(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_irc_commands WHERE Command = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(!db.IsNull())
				return;

			SchumixBase.DManager.Insert("`ignore_irc_commands`(Command)", sUtilities.SqlEscape(Name.ToLower()));
		}

		public void Remove(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_irc_commands WHERE Command = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(db.IsNull())
				return;

			SchumixBase.DManager.Delete("ignore_irc_commands", string.Format("Command = '{0}'", sUtilities.SqlEscape(Name.ToLower())));
		}
	}

	public sealed class IgnoreCommand
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private IgnoreCommand() {}

		public bool IsIgnore(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_commands WHERE Command = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			return !db.IsNull() ? true : false;
		}

		public void Add(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_commands WHERE Command = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(!db.IsNull())
				return;

			SchumixBase.DManager.Insert("`ignore_commands`(Command)", sUtilities.SqlEscape(Name.ToLower()));
		}

		public void Remove(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_commands WHERE Command = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(db.IsNull())
				return;

			SchumixBase.DManager.Delete("ignore_commands", string.Format("Command = '{0}'", sUtilities.SqlEscape(Name.ToLower())));
		}
	}

	public sealed class IgnoreChannel
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		private IgnoreChannel()
		{
			string[] ignore = IRCConfig.IgnoreChannels.Split(SchumixBase.Comma);

			if(ignore.Length > 1)
			{
				foreach(var name in ignore)
					Add(name.ToLower());
			}
			else
				Add(IRCConfig.IgnoreNames.ToLower());
		}

		public bool IsIgnore(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_channels WHERE Channel = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			return !db.IsNull() ? true : false;
		}

		public void Add(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_channels WHERE Channel = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(!db.IsNull())
				return;

			SchumixBase.DManager.Insert("`ignore_channels`(Channel)", sUtilities.SqlEscape(Name.ToLower()));
		}

		public void Remove(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_channels WHERE Channel = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(db.IsNull())
				return;

			SchumixBase.DManager.Delete("ignore_channels", string.Format("Channel = '{0}'", sUtilities.SqlEscape(Name.ToLower())));
		}

		public void RemoveConfig()
		{
			string[] ignore = IRCConfig.IgnoreChannels.Split(SchumixBase.Comma);

			if(ignore.Length > 1)
			{
				foreach(var name in ignore)
					Remove(name.ToLower());
			}
			else
				Remove(IRCConfig.IgnoreNames.ToLower());
		}
	}
}