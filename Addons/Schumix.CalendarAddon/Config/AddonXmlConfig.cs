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

namespace Schumix.CalendarAddon.Config
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

			Log.Notice("CalendarAddonConfig", sLConsole.GetString("Config file is loading."));

			int Seconds = !xmldoc.SelectSingleNode("CalendarAddon/Flooding/Seconds").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("CalendarAddon/Flooding/Seconds").InnerText) : _seconds;
			int NumberOfMessages = !xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfMessages").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfMessages").InnerText) : _numberofmessages;
			int NumberOfFlooding = !xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfFlooding").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfFlooding").InnerText) : _numberofflooding;
			new CalendarConfig(Seconds, NumberOfMessages, NumberOfFlooding);

			Log.Success("CalendarAddonConfig", sLConsole.GetString("Config database is loading."));
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
				Log.Error("CalendarAddonConfig", sLConsole.GetString("No such config file!"));
				Log.Debug("CalendarAddonConfig", sLConsole.GetString("Preparing..."));
				var w = new XmlTextWriter(filename, null);
				var xmldoc = new XmlDocument();
				string filename2 = sUtilities.DirectoryToSpecial(ConfigDirectory, "_" + ConfigFile);

				if(File.Exists(filename2))
				{
					Log.Notice("CalendarAddonConfig", sLConsole.GetString("The backup files will be used to renew the data."));
					xmldoc.Load(filename2);
				}

				try
				{
					w.Formatting = Formatting.Indented;
					w.Indentation = 4;
					w.Namespaces = false;
					w.WriteStartDocument();

					// <CalendarAddon>
					w.WriteStartElement("CalendarAddon");

					// <Flooding>
					w.WriteStartElement("Flooding");
					w.WriteElementString("Seconds",          (!xmldoc.SelectSingleNode("CalendarAddon/Flooding/Seconds").IsNull() ? xmldoc.SelectSingleNode("CalendarAddon/Flooding/Seconds").InnerText : _seconds.ToString()));
					w.WriteElementString("NumberOfMessages", (!xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfMessages").IsNull() ? xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfMessages").InnerText : _numberofmessages.ToString()));
					w.WriteElementString("NumberOfFlooding", (!xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfFlooding").IsNull() ? xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfFlooding").InnerText : _numberofflooding.ToString()));

					// </Flooding>
					w.WriteEndElement();

					// </CalendarAddon>
					w.WriteEndElement();

					w.Flush();
					w.Close();

					if(File.Exists(filename2))
					{
						Log.Notice("CalendarAddonConfig", sLConsole.GetString("The backup has been deleted during the re-use."));
						File.Delete(filename2);
					}

					Log.Success("CalendarAddonConfig", sLConsole.GetString("Config file is completed!"));
				}
				catch(Exception e)
				{
					Log.Error("CalendarAddonConfig", sLConsole.GetString("Failure was handled during the xml writing. Details: {0}"), e.Message);
				}

				return false;
			}
		}
	}
}