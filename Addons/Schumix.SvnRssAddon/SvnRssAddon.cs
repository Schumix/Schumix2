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
using Schumix.Api;
using Schumix.Api.Irc;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.SvnRssAddon.Config;
using Schumix.SvnRssAddon.Commands;
using Schumix.SvnRssAddon.Localization;

namespace Schumix.SvnRssAddon
{
	class SvnRssAddon : ISchumixAddon
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private RssCommand sRssCommand;
#pragma warning disable 414
		private AddonConfig _config;
#pragma warning restore 414
		private string _servername;

		public void Setup(string ServerName)
		{
			_servername = ServerName;
			sRssCommand = new RssCommand(ServerName);
			sLocalization.Locale = sLConsole.Locale;

			if(IRCConfig.List[ServerName].ServerId == 1)
				_config = new AddonConfig(Name, ".yml");

			InitIrcCommand();
			SchumixBase.sCleanManager.CDatabase.CleanTable("svninfo");
			SchumixBase.DManager.Update("svninfo", string.Format("ServerName = '{0}'", ServerName), string.Format("ServerId = '{0}'", IRCConfig.List[ServerName].ServerId));

			var db = SchumixBase.DManager.Query("SELECT Name, Link, Website FROM svninfo WHERE ServerName = '{0}'", ServerName);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string name = row["Name"].ToString();
					string link = row["Link"].ToString();
					string website = row["Website"].ToString();
					var rss = new SvnRss(ServerName, name, link, website);
					sRssCommand.RssList.Add(rss);
				}

				int x = 0;

				foreach(var list in sRssCommand.RssList)
				{
					list.Start();
					x++;
				}

				Log.Notice("SvnRssAddon", sLConsole.GetString("{0}: {1} rss loaded."), ServerName, x);
			}
			else
				Log.Warning("SvnRssAddon", sLConsole.GetString("{0}: Empty database!"), ServerName);
		}

		public void Destroy()
		{
			RemoveIrcCommand();
			_config = null;

			foreach(var list in sRssCommand.RssList)
				list.Stop();

			sRssCommand.RssList.Clear();
		}

		public int Reload(string RName, string SName = "")
		{
			try
			{
				switch(RName.ToLower())
				{
					case "config":
						if(IRCConfig.List[_servername].ServerId == 1)
							_config = new AddonConfig(Name, ".yml");

						return 1;
					case "command":
						InitIrcCommand();
						RemoveIrcCommand();
						return 1;
				}
			}
			catch(Exception e)
			{
				Log.Error("SvnRssAddon", "Reload: " + sLConsole.GetString("Failure details: {0}"), e.Message);
				return 0;
			}

			return -1;
		}

		private void InitIrcCommand()
		{
			sIrcBase.Networks[_servername].SchumixRegisterHandler("svn", sRssCommand.HandleSvn, CommandPermission.Operator);
		}

		private void RemoveIrcCommand()
		{
			sIrcBase.Networks[_servername].SchumixRemoveHandler("svn",   sRssCommand.HandleSvn);
		}

		public bool HandleHelp(IRCMessage sIRCMessage)
		{
			return false;
		}

		/// <summary>
		/// Name of the addon
		/// </summary>
		public string Name
		{
			get { return "SvnRssAddon"; }
		}

		/// <summary>
		/// Author of the addon.
		/// </summary>
		public string Author
		{
			get { return Consts.SchumixProgrammedBy; }
		}

		/// <summary>
		/// Website where the addon is available.
		/// </summary>
		public string Website
		{
			get { return Consts.SchumixWebsite; }
		}
	}
}