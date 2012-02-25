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
using System.IO;
using System.Xml;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.HgRssAddon.Localization;

namespace Schumix.HgRssAddon.Config
{
	sealed class AddonConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private const int _querytime = 60;

		public AddonConfig(string configfile)
		{
			try
			{
				Log.Debug("HgRssAddonConfig", ">> {0}", configfile);

				if(!IsConfig(SchumixConfig.ConfigDirectory, configfile))
					Init(configfile);
				else
					Init(configfile);
			}
			catch(Exception e)
			{
				Log.Error("HgRssAddonConfig", sLConsole.Exception("Error"), e.Message);
			}
		}

		private void Init(string configfile)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(string.Format("./{0}/{1}", SchumixConfig.ConfigDirectory, configfile));

			Log.Notice("HgRssAddonConfig", sLocalization.Config("Text"));

			int QueryTime = !xmldoc.SelectSingleNode("HgRssAddon/Rss/QueryTime").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("HgRssAddon/Rss/QueryTime").InnerText) : _querytime;
			new RssConfig(QueryTime);

			Log.Success("HgRssAddonConfig", sLocalization.Config("Text2"));
			Console.WriteLine();
		}

		private bool IsConfig(string ConfigDirectory, string ConfigFile)
		{
			if(File.Exists(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile)))
				return true;
			else
			{
				Log.Error("HgRssAddonConfig", sLocalization.Config("Text3"));
				Log.Debug("HgRssAddonConfig", sLocalization.Config("Text4"));
				var w = new XmlTextWriter(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile), null);
				var xmldoc = new XmlDocument();

				if(File.Exists(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile)))
					xmldoc.Load(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile));

				try
				{
					w.Formatting = Formatting.Indented;
					w.Indentation = 4;
					w.Namespaces = false;
					w.WriteStartDocument();

					// <HgRssAddon>
					w.WriteStartElement("HgRssAddon");

					// <Rss>
					w.WriteStartElement("Rss");
					w.WriteElementString("QueryTime", (!xmldoc.SelectSingleNode("HgRssAddon/Rss/QueryTime").IsNull() ? xmldoc.SelectSingleNode("HgRssAddon/Rss/QueryTime").InnerText : _querytime.ToString()));

					// </Rss>
					w.WriteEndElement();

					// </HgRssAddon>
					w.WriteEndElement();

					w.Flush();
					w.Close();

					if(File.Exists(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile)))
						File.Delete(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile));

					Log.Success("HgRssAddonConfig", sLocalization.Config("Text5"));
					return false;
				}
				catch(Exception e)
				{
					Log.Error("HgRssAddonConfig", sLocalization.Config("Text6"), e.Message);
					return false;
				}
			}
		}
	}

	sealed class RssConfig
	{
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		public static int QueryTime { get; private set; }

		public RssConfig(int querytime)
		{
			QueryTime = querytime;
			Log.Notice("HgRssConfig", sLocalization.RssConfig("Text"));
		}
	}
}