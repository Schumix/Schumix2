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
using Schumix.GitRssAddon.Localization;

namespace Schumix.GitRssAddon.Config
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
				Log.Debug("GitRssAddonConfig", ">> {0}", configfile);

				if(!IsConfig(SchumixConfig.ConfigDirectory, configfile))
					Init(configfile);
				else
					Init(configfile);
			}
			catch(Exception e)
			{
				Log.Error("GitRssAddonConfig", sLConsole.Exception("Error"), e.Message);
			}
		}

		private void Init(string configfile)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(string.Format("./{0}/{1}", SchumixConfig.ConfigDirectory, configfile));

			Log.Notice("GitRssAddonConfig", sLocalization.Config("Text"));

			int QueryTime = !xmldoc.SelectSingleNode("GitRssAddon/Rss/QueryTime").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("GitRssAddon/Rss/QueryTime").InnerText) : _querytime;
			new RssConfig(QueryTime);

			Log.Success("GitRssAddonConfig", sLocalization.Config("Text2"));
			Console.WriteLine();
		}

		private bool IsConfig(string ConfigDirectory, string ConfigFile)
		{
			if(File.Exists(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile)))
				return true;
			else
			{
				Log.Error("GitRssAddonConfig", sLocalization.Config("Text3"));
				Log.Debug("GitRssAddonConfig", sLocalization.Config("Text4"));
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

					// <GitRssAddon>
					w.WriteStartElement("GitRssAddon");

					// <Rss>
					w.WriteStartElement("Rss");
					w.WriteElementString("QueryTime", (!xmldoc.SelectSingleNode("GitRssAddon/Rss/QueryTime").IsNull() ? xmldoc.SelectSingleNode("GitRssAddon/Rss/QueryTime").InnerText : _querytime.ToString()));

					// </Rss>
					w.WriteEndElement();

					// </GitRssAddon>
					w.WriteEndElement();

					w.Flush();
					w.Close();

					if(File.Exists(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile)))
						File.Delete(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile));

					Log.Success("GitRssAddonConfig", sLocalization.Config("Text5"));
					return false;
				}
				catch(Exception e)
				{
					Log.Error("GitRssAddonConfig", sLocalization.Config("Text6"), e.Message);
					return false;
				}
			}
		}
	}
}