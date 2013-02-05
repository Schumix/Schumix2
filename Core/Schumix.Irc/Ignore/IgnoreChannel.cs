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
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Ignore
{
	public sealed class IgnoreChannel
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly List<string> _ignorelist = new List<string>();
		private string _servername;

		public IgnoreChannel(string ServerName)
		{
			_servername = ServerName;
		}

		public bool IsIgnore(string Name)
		{
			return Contains(Name);
		}

		public void LoadConfig()
		{
			string[] ignore = IRCConfig.List[_servername].IgnoreChannels.Split(SchumixBase.Comma);

			if(ignore.Length > 1)
			{
				foreach(var name in ignore)
				{
					if(name.ToLower() == IRCConfig.List[_servername].MasterChannel.ToLower())
						continue;

					Add(name.ToLower());
				}
			}
			else
			{
				if(IRCConfig.List[_servername].IgnoreChannels.ToLower() != IRCConfig.List[_servername].MasterChannel.ToLower())
					Add(IRCConfig.List[_servername].IgnoreChannels.ToLower());
			}
		}

		public void LoadSql()
		{
			var db = SchumixBase.DManager.Query("SELECT Channel FROM ignore_channels WHERE ServerName = '{0}'", _servername);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string name = row["Channel"].ToString();

					if(!Contains(name))
						_ignorelist.Add(name.ToLower());
				}
			}
		}

		public void Add(string Name)
		{
			if(Name.Trim().IsEmpty())
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			if(!db.IsNull())
				return;

			db = SchumixBase.DManager.QueryFirstRow("SELECT Enabled FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			if(!db.IsNull())
			{
				if(db["Enabled"].ToString().IsEmpty() || Convert.ToBoolean(db["Enabled"].ToString()))
				{
					SchumixBase.DManager.Update("channels", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.IgnoreChannel("Text")), string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername));

					if(sIrcBase.Networks[_servername].Online)
					{
						sIrcBase.Networks[_servername].sChannelList.Remove(Name.ToLower());
						sIrcBase.Networks[_servername].sSender.Part(Name.ToLower(), sLConsole.IgnoreChannel("Text2", sLManager.GetChannelLocalization(Name, _servername)));
					}
				}
			}

			_ignorelist.Add(Name.ToLower());
			SchumixBase.DManager.Insert("`ignore_channels`(ServerId, ServerName, Channel)", IRCConfig.List[_servername].ServerId, _servername, sUtilities.SqlEscape(Name.ToLower()));
		}

		public void Remove(string Name)
		{
			if(Name.Trim().IsEmpty())
				return;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM ignore_channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			if(db.IsNull())
				return;

			_ignorelist.Remove(Name.ToLower());
			SchumixBase.DManager.Delete("ignore_channels", string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername));

			if(SchumixBase.ExitStatus)
				return;

			db = SchumixBase.DManager.QueryFirstRow("SELECT Enabled, Password FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername);
			if(!db.IsNull())
			{
				if(db["Enabled"].ToString().IsEmpty() || !Convert.ToBoolean(db["Enabled"].ToString()))
				{
					SchumixBase.DManager.Update("channels", "Enabled = 'true', Error = ''", string.Format("Channel = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), _servername));

					if(sIrcBase.Networks[_servername].Online)
					{
						string password = db["Password"].ToString();

						if(password.Trim().IsEmpty())
							sIrcBase.Networks[_servername].sSender.Join(Name.ToLower());
						else
							sIrcBase.Networks[_servername].sSender.Join(Name.ToLower(), db["Password"].ToString().Trim());
					}
				}
			}
		}

		public bool Contains(string Name)
		{
			if(Name.Trim().IsEmpty())
				return false;

			return _ignorelist.Contains(Name.ToLower());
		}

		public void RemoveConfig()
		{
			string[] ignore = IRCConfig.List[_servername].IgnoreChannels.Split(SchumixBase.Comma);

			if(ignore.Length > 1)
			{
				foreach(var name in ignore)
					Remove(name.ToLower());
			}
			else
				Remove(IRCConfig.List[_servername].IgnoreChannels.ToLower());
		}

		public void Clean()
		{
			_ignorelist.Clear();
		}
	}
}