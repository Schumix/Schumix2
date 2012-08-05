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
using Schumix.WordPressRssAddon.Localization;

namespace Schumix.WordPressRssAddon.Config
{
	sealed class AddonXmlConfig : AddonDefaultConfig
	{
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		public AddonXmlConfig()
		{
		}

		public AddonXmlConfig(string configdir, string configfile)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(sUtilities.DirectoryToSpecial(SchumixConfig.ConfigDirectory, configfile));

			Log.Notice("WordPressRssAddonConfig", sLocalization.Config("Text"));

			int QueryTime = !xmldoc.SelectSingleNode("WordPressRssAddon/Rss/QueryTime").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("WordPressRssAddon/Rss/QueryTime").InnerText) : d_querytime;
			new RssConfig(QueryTime);

			Log.Success("WordPressRssAddonConfig", sLocalization.Config("Text2"));
			Console.WriteLine();
		}

		~AddonXmlConfig()
		{
		}

		public bool CreateConfig(string ConfigDirectory, string ConfigFile)
		{
			string filename = sUtilities.DirectoryToSpecial(ConfigDirectory, ConfigFile);

			if(File.Exists(filename))
				return true;
			else
			{
				Log.Error("WordPressRssAddonConfig", sLocalization.Config("Text3"));
				Log.Debug("WordPressRssAddonConfig", sLocalization.Config("Text4"));
				var w = new XmlTextWriter(filename, null);
				var xmldoc = new XmlDocument();
				string filename2 = sUtilities.DirectoryToSpecial(ConfigDirectory, "_" + ConfigFile);

				if(File.Exists(filename2))
					xmldoc.Load(filename2);

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
						File.Delete(filename2);

					Log.Success("WordPressRssAddonConfig", sLocalization.Config("Text5"));
				}
				catch(Exception e)
				{
					Log.Error("WordPressRssAddonConfig", sLocalization.Config("Text6"), e.Message);
				}
			}

			return false;
		}
	}
}