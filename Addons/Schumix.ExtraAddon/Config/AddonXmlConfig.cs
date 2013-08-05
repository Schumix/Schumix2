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
using System.IO;
using System.Xml;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.ExtraAddon.Config
{
	sealed class AddonXmlConfig : AddonDefaultConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		public AddonXmlConfig()
		{
		}

		public AddonXmlConfig(string configdir, string configfile)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(sUtilities.DirectoryToSpecial(SchumixConfig.ConfigDirectory, configfile));

			Log.Notice("ExtraAddonConfig", sLConsole.GetString("Config file is loading."));

			bool Enabled = !xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Enabled").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Enabled").InnerText.ToBoolean() : d_enabled;
			string Type = !xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Type").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Type").InnerText : d_type;
			new ModeConfig(Enabled, Type);

			string City = !xmldoc.SelectSingleNode("ExtraAddon/Weather/Home/City").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/Weather/Home/City").InnerText : d_weatherhomecity;
			new WeatherConfig(City);

			string Key = !xmldoc.SelectSingleNode("ExtraAddon/WolframAlpha/Api/Key").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/WolframAlpha/Api/Key").InnerText : d_wolframalphaapikey;
			new WolframAlphaConfig(Key);

			Log.Success("ExtraAddonConfig", sLConsole.GetString("Config database is loading."));
			Log.WriteLine();
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
				Log.Error("ExtraAddonConfig", sLConsole.GetString("No such config file!"));
				Log.Debug("ExtraAddonConfig", sLConsole.GetString("Preparing..."));
				var w = new XmlTextWriter(filename, null);
				var xmldoc = new XmlDocument();
				string filename2 = sUtilities.DirectoryToSpecial(ConfigDirectory, "_" + ConfigFile);

				if(File.Exists(filename2))
				{
					Log.Notice("ExtraAddonConfig", sLConsole.GetString("The backup files will be used to renew the data."));
					xmldoc.Load(filename2);
				}

				try
				{
					w.Formatting = Formatting.Indented;
					w.Indentation = 4;
					w.Namespaces = false;
					w.WriteStartDocument();

					// <ExtraAddon>
					w.WriteStartElement("ExtraAddon");

					// <Mode>
					w.WriteStartElement("Mode");

					// <Remove>
					w.WriteStartElement("Remove");
					w.WriteElementString("Enabled", (!xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Enabled").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Enabled").InnerText : d_enabled.ToString()));
					w.WriteElementString("Type",    (!xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Type").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Type").InnerText : d_type));

					// </Remove>
					w.WriteEndElement();

					// </Mode>
					w.WriteEndElement();

					// <Weather>
					w.WriteStartElement("Weather");

					// <Home>
					w.WriteStartElement("Home");
					w.WriteElementString("City",    (!xmldoc.SelectSingleNode("ExtraAddon/Weather/Home/City").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/Weather/Home/City").InnerText : d_weatherhomecity));

					// </Home>
					w.WriteEndElement();

					// </Weather>
					w.WriteEndElement();

					// <WolframAlpha>
					w.WriteStartElement("WolframAlpha");

					// <Api>
					w.WriteStartElement("Api");
					w.WriteElementString("Key",     (!xmldoc.SelectSingleNode("ExtraAddon/WolframAlpha/Api/Key").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/WolframAlpha/Api/Key").InnerText : d_wolframalphaapikey));

					// </Api>
					w.WriteEndElement();

					// </WolframAlpha>
					w.WriteEndElement();

					// </ExtraAddon>
					w.WriteEndElement();

					w.Flush();
					w.Close();

					if(File.Exists(filename2))
					{
						Log.Notice("ExtraAddonConfig", sLConsole.GetString("The backup has been deleted during the re-use."));
						File.Delete(filename2);
					}

					Log.Success("ExtraAddonConfig", sLConsole.GetString("Config file is completed!"));
				}
				catch(Exception e)
				{
					Log.Error("ExtraAddonConfig", sLConsole.GetString("Failure was handled during the xml writing. Details: {0}"), e.Message);
				}

				return false;
			}
		}
	}
}