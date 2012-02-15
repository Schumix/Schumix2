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
using Schumix.CalendarAddon.Localization;

namespace Schumix.CalendarAddon.Config
{
	sealed class AddonConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private const int _seconds = 10;
		private const int _numberofmessages = 5;
		private const int _numberofflooding = 3;

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
				Log.Error("CalendarAddonConfig", sLConsole.Exception("Error"), e.Message);
			}
		}

		private void Init(string configfile)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(string.Format("./{0}/{1}", SchumixConfig.ConfigDirectory, configfile));

			Log.Notice("CalendarAddonConfig", sLocalization.Config("Text"));

			int Seconds = !xmldoc.SelectSingleNode("CalendarAddon/Flooding/Seconds").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("CalendarAddon/Flooding/Seconds").InnerText) : _seconds;
			int NumberOfMessages = !xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfMessages").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfMessages").InnerText) : _numberofmessages;
			int NumberOfFlooding = !xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfFlooding").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("CalendarAddon/Flooding/NumberOfFlooding").InnerText) : _numberofflooding;
			new CalendarConfig(Seconds, NumberOfMessages, NumberOfFlooding);

			Log.Success("CalendarAddonConfig", sLocalization.Config("Text2"));
			Console.WriteLine();
		}

		private bool IsConfig(string ConfigDirectory, string ConfigFile)
		{
			if(File.Exists(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile)))
				return true;
			else
			{
				Log.Error("CalendarAddonConfig", sLocalization.Config("Text3"));
				Log.Debug("CalendarAddonConfig", sLocalization.Config("Text4"));
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

					if(File.Exists(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile)))
						File.Delete(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile));

					Log.Success("CalendarAddonConfig", sLocalization.Config("Text5"));
					return false;
				}
				catch(Exception e)
				{
					Log.Error("CalendarAddonConfig", sLocalization.Config("Text6"), e.Message);
					return false;
				}
			}
		}
	}

	sealed class CalendarConfig
	{
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		public static int Seconds { get; private set; }
		public static int NumberOfMessages { get; private set; }
		public static int NumberOfFlooding { get; private set; }

		public CalendarConfig(int seconds, int numberofmessages, int numberofflooding)
		{
			Seconds          = seconds;
			NumberOfMessages = numberofmessages;
			NumberOfFlooding = numberofflooding;
			Log.Notice("CalendarConfig", sLocalization.CalendarConfig("Text"));
		}
	}
}