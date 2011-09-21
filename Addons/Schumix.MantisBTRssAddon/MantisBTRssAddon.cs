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
using System.Data;
using System.Collections.Generic;
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.Irc.Commands;
using Schumix.MantisBTRssAddon.Config;
using Schumix.MantisBTRssAddon.Commands;
using Schumix.MantisBTRssAddon.Localization;

namespace Schumix.MantisBTRssAddon
{
	public class MantisBTRssAddon : RssCommand, ISchumixAddon
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		public static readonly List<MantisBTRss> RssList = new List<MantisBTRss>();
#if MONO
#pragma warning disable 414
		private AddonConfig _config;
#pragma warning restore 414
#else
		private AddonConfig _config;
#endif

		public void Setup()
		{
			sLocalization.Locale = sLConsole.Locale;
			_config = new AddonConfig(Name + ".xml");
			CommandManager.OperatorCRegisterHandler("mantisbt", new Action<IRCMessage>(HandleMantisBT));

			var db = SchumixBase.DManager.Query("SELECT Name, Link FROM mantisbt");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string name = row["Name"].ToString();
					string link = row["Link"].ToString();
					var rss = new MantisBTRss(name, link);
					RssList.Add(rss);
				}

				int x = 0;

				foreach(var list in RssList)
				{
					list.Start();
					x++;
				}

				Log.Notice("MantisBTRssAddon", sLocalization.MantisBTRssAddon("Text"), x);
			}
			else
				Log.Warning("MantisBTRssAddon", sLocalization.MantisBTRssAddon("Text2"));
		}

		public void Destroy()
		{
			CommandManager.OperatorCRemoveHandler("mantisbt");
			_config = null;

			foreach(var list in RssList)
				list.Stop();

			RssList.Clear();
		}

		public bool Reload(string RName)
		{
			switch(RName.ToLower())
			{
				case "config":
					_config = new AddonConfig(Name + ".xml");
					return true;
			}

			return false;
		}

		public void HandlePrivmsg(IRCMessage sIRCMessage)
		{

		}

		public void HandleNotice(IRCMessage sIRCMessage)
		{

		}

		public void HandleLeft(IRCMessage sIRCMessage)
		{

		}

		public void HandleKick(IRCMessage sIRCMessage)
		{

		}

		public void HandleQuit(IRCMessage sIRCMessage)
		{

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
			get { return "MantisBTRssAddon"; }
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