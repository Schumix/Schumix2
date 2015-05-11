/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Ignore
{
	public sealed class IgnoreNickName
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly List<string> _ignorelist = new List<string>();
		private string _servername;

		public IgnoreNickName(string ServerName)
		{
			_servername = ServerName;
		}

		public bool IsIgnore(string Name)
		{
			return Contains(Name);
		}

		public void LoadConfig()
		{
			string[] ignore = IRCConfig.List[_servername].IgnoreNames.Split(SchumixBase.Comma);

			if(ignore.Length > 1)
			{
				foreach(var name in ignore)
					Add(name.ToLower());
			}
			else
				Add(IRCConfig.List[_servername].IgnoreNames.ToLower());
		}

		public void LoadSql()
		{
			var db = SchumixBase.DManager.Query("SELECT Nick FROM ignore_nicks WHERE ServerName = '{0}'", _servername);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string name = row["Nick"].ToString();
					
					if(!Contains(name))
						_ignorelist.Add(name.ToLower());
				}
			}
		}

		public void Add(string Name)
		{
			if(Name.IsNullOrEmpty())
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM ignore_nicks WHERE Nick = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			if(!db.IsNull())
				return;

			_ignorelist.Add(Name.ToLower());
			SchumixBase.DManager.Insert("`ignore_nicks`(ServerId, ServerName, Nick)", IRCConfig.List[_servername].ServerId, _servername, sUtilities.SqlEscape(Name.ToLower()));
		}

		public void Remove(string Name)
		{
			if(Name.IsNullOrEmpty())
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM ignore_nicks WHERE Nick = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			if(db.IsNull())
				return;

			_ignorelist.Remove(Name.ToLower());
			SchumixBase.DManager.Delete("ignore_nicks", string.Format("Nick = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername));
		}

		public bool Contains(string Name)
		{
			if(Name.IsNullOrEmpty())
				return false;

			return _ignorelist.Contains(Name.ToLower());
		}

		public void RemoveConfig()
		{
			string[] ignore = IRCConfig.List[_servername].IgnoreNames.Split(SchumixBase.Comma);

			if(ignore.Length > 1)
			{
				foreach(var name in ignore)
					Remove(name.ToLower());
			}
			else
				Remove(IRCConfig.List[_servername].IgnoreNames.ToLower());
		}

		public void Clean()
		{
			_ignorelist.Clear();
		}
	}
}