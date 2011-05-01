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
using System.IO;
using System.Xml;
using Schumix.Framework;
using Schumix.Framework.Config;

namespace Schumix.CalendarAddon.Config
{
	public sealed class AddonConfig
	{
		public AddonConfig(string configfile)
		{
			try
			{
				Log.Debug("CalendarAddonConfig", ">> {0}", configfile);

				if(!IsConfig(SchumixConfig.ConfigDirectory, configfile))
					Init(configfile);
				else
					Init(configfile);
			}
			catch(Exception e)
			{
				Log.Error("CalendarAddonConfig", "Hiba oka: {0}", e.Message);
			}
		}

		private void Init(string configfile)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(string.Format("./{0}/{1}", SchumixConfig.ConfigDirectory, configfile));

			Log.Notice("CalendarAddonConfig", "Config fajl betoltese.");

			int Seconds = Convert.ToInt32(xmldoc.SelectSingleNode("CalendarAddon/Flooding/Seconds").InnerText);
			int NumberOfMessages = Convert.ToInt32(xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfMessages").InnerText);
			int NumberOfFlooding = Convert.ToInt32(xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfFlooding").InnerText);
			new CalendarConfig(Seconds, NumberOfMessages, NumberOfFlooding);

			Log.Success("CalendarAddonConfig", "Config adatbazis betoltve.");
			Console.WriteLine();
		}

		private bool IsConfig(string ConfigDirectory, string ConfigFile)
		{
			if(File.Exists(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile)))
				return true;
			else
			{
				Log.Error("CalendarAddonConfig", "Nincs config fajl!");
				Log.Debug("CalendarAddonConfig", "Elkeszitese folyamatban...");
				var w = new XmlTextWriter(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile), null);

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
					w.WriteElementString("Seconds", "10");
					w.WriteElementString("NumberOfMessages", "5");
					w.WriteElementString("NumberOfFlooding", "3");

					// </Flooding>
					w.WriteEndElement();

					// </CalendarAddon>
					w.WriteEndElement();

					w.Flush();
					w.Close();

					Log.Success("CalendarAddonConfig", "Config fajl elkeszult!");
					return false;
				}
				catch(Exception e)
				{
					Log.Error("CalendarAddonConfig", "Hiba az xml irasa soran: {0}", e.Message);
					return false;
				}
			}
		}
	}

	public sealed class CalendarConfig
	{
		public static int Seconds { get; private set; }
		public static int NumberOfMessages { get; private set; }
		public static int NumberOfFlooding { get; private set; }

		public CalendarConfig(int seconds, int numberofmessages, int numberofflooding)
		{
			Seconds          = seconds;
			NumberOfMessages = numberofmessages;
			NumberOfFlooding = numberofflooding;
			Log.Notice("CalendarConfig", "Calendar beallitasai betoltve.");
		}
	}
}