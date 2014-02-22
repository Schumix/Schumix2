/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using Schumix.Framework.Util;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.WordPressRssAddon.Config
{
	sealed class AddonXmlConfig : AddonDefaultConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;

		public AddonXmlConfig()
		{
		}

		public AddonXmlConfig(string configdir, string configfile)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(Path.Combine(SchumixConfig.ConfigDirectory, configfile));

			Log.Notice("WordPressRssAddonConfig", sLConsole.GetString("Config file is loading."));

			int QueryTime = !xmldoc.SelectSingleNode("WordPressRssAddon/Rss/QueryTime").IsNull() ? xmldoc.SelectSingleNode("WordPressRssAddon/Rss/QueryTime").InnerText.ToInt32() : d_querytime;
			new RssConfig(QueryTime);

			Log.Success("WordPressRssAddonConfig", sLConsole.GetString("Config database is loading."));
			Log.WriteLine();
		}

		~AddonXmlConfig()
		{
		}

		public bool CreateConfig(string ConfigDirectory, string ConfigFile)
		{
			string filename = Path.Combine(ConfigDirectory, ConfigFile);

			if(File.Exists(filename))
				return true;
			else
			{
				Log.Error("WordPressRssAddonConfig", sLConsole.GetString("No such config file!"));
				Log.Debug("WordPressRssAddonConfig", sLConsole.GetString("Preparing..."));
				var w = new XmlTextWriter(filename, null);
				var xmldoc = new XmlDocument();
				string filename2 = Path.Combine(ConfigDirectory, "_" + ConfigFile);

				if(File.Exists(filename2))
				{
					Log.Notice("WordPressRssAddonConfig", sLConsole.GetString("The backup files will be used to renew the data."));
					xmldoc.Load(filename2);
				}

				try
				{
					w.Formatting = Formatting.Indented;
					w.Indentation = 4;
					w.Namespaces = false;
					w.WriteStartDocument();

					// <WordPressRssAddon>
					w.WriteStartElement("WordPressRssAddon");

					// <Rss>
					w.WriteStartElement("Rss");
					w.WriteElementString("QueryTime", (!xmldoc.SelectSingleNode("WordPressRssAddon/Rss/QueryTime").IsNull() ? xmldoc.SelectSingleNode("WordPressRssAddon/Rss/QueryTime").InnerText : d_querytime.ToString()));

					// </Rss>
					w.WriteEndElement();

					// </WordPressRssAddon>
					w.WriteEndElement();

					w.Flush();
					w.Close();

					if(File.Exists(filename2))
					{
						Log.Notice("WordPressRssAddonConfig", sLConsole.GetString("The backup has been deleted during the re-use."));
						File.Delete(filename2);
					}

					Log.Success("WordPressRssAddonConfig", sLConsole.GetString("Config file is completed!"));
				}
				catch(Exception e)
				{
					Log.Error("WordPressRssAddonConfig", sLConsole.GetString("Failure was handled during the xml writing. Details: {0}"), e.Message);
				}
			}

			return false;
		}
	}
}