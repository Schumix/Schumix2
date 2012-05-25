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
using System.Data;
using System.Collections.Generic;
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.Irc.Commands;
using Schumix.WordPressRssAddon.Config;
using Schumix.WordPressRssAddon.Commands;
using Schumix.WordPressRssAddon.Localization;

namespace Schumix.WordPressRssAddon
{
	class WordPressRssAddon : RssCommand, ISchumixAddon
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		public static readonly List<WordPressRss> RssList = new List<WordPressRss>();
#pragma warning disable 414
		private AddonConfig _config;
#pragma warning restore 414

		public void Setup()
		{
			sLocalization.Locale = sLConsole.Locale;
			_config = new AddonConfig(Name + ".xml");
			InitIrcCommand();

			var db = SchumixBase.DManager.Query("SELECT Name, Link FROM wordpressinfo");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string name = row["Name"].ToString();
					string link = row["Link"].ToString();
					var rss = new WordPressRss(name, link);
					RssList.Add(rss);
				}

				int x = 0;

				foreach(var list in RssList)
				{
					list.Start();
					x++;
				}

				Log.Notice("WordPressRssAddon", sLocalization.WordPressRssAddon("Text"), x);
			}
			else
				Log.Warning("WordPressRssAddon", sLocalization.WordPressRssAddon("Text2"));
		}

		public void Destroy()
		{
			RemoveIrcCommand();
			_config = null;

			foreach(var list in RssList)
				list.Stop();

			RssList.Clear();
		}

		public int Reload(string RName, string SName = "")
		{
			try
			{
				switch(RName.ToLower())
				{
					case "config":
						_config = new AddonConfig(Name + ".xml");
						return 1;
					case "command":
						InitIrcCommand();
						RemoveIrcCommand();
						return 1;
				}
			}
			catch(Exception e)
			{
				Log.Error("WordPressRssAddon", "Reload: " + sLConsole.Exception("Error"), e.Message);
				return 0;
			}

			return -1;
		}

		private void InitIrcCommand()
		{
			CommandManager.SchumixRegisterHandler("wordpress", HandleWordPress, CommandPermission.Operator);
		}

		private void RemoveIrcCommand()
		{
			CommandManager.SchumixRemoveHandler("wordpress",   HandleWordPress);
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
			get { return "WordPressRssAddon"; }
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