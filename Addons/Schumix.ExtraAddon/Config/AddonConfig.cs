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
using Schumix.ExtraAddon.Localization;

namespace Schumix.ExtraAddon.Config
{
	sealed class AddonConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private const bool _enabled = false;
		private const string _type = "aohv";
		private const string _weatherhomecity = "Zalaegerszeg";
		private const string _wolframalphaapikey = "557QYQ-UUUWTKX95V";

		public AddonConfig(string configfile)
		{
			try
			{
				Log.Debug("ExtraAddonConfig", ">> {0}", configfile);

				if(!IsConfig(SchumixConfig.ConfigDirectory, configfile))
					Init(configfile);
				else
					Init(configfile);
			}
			catch(Exception e)
			{
				Log.Error("ExtraAddonConfig", sLConsole.Exception("Error"), e.Message);
			}
		}

		private void Init(string configfile)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(string.Format("./{0}/{1}", SchumixConfig.ConfigDirectory, configfile));

			Log.Notice("ExtraAddonConfig", sLocalization.Config("Text"));

			bool Enabled = !xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Enabled").InnerText) : _enabled;
			string Type = !xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Type").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Type").InnerText : _type;
			new ModeConfig(Enabled, Type);

			string City = !xmldoc.SelectSingleNode("ExtraAddon/Weather/Home/City").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/Weather/Home/City").InnerText : _weatherhomecity;
			new WeatherConfig(City);

			string Key = !xmldoc.SelectSingleNode("ExtraAddon/WolframAlpha/Api/Key").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/WolframAlpha/Api/Key").InnerText : _wolframalphaapikey;
			new WolframAlphaConfig(Key);

			Log.Success("ExtraAddonConfig", sLocalization.Config("Text2"));
			Console.WriteLine();
		}

		private bool IsConfig(string ConfigDirectory, string ConfigFile)
		{
			if(File.Exists(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile)))
				return true;
			else
			{
				Log.Error("ExtraAddonConfig", sLocalization.Config("Text3"));
				Log.Debug("ExtraAddonConfig", sLocalization.Config("Text4"));
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

					// <ExtraAddon>
					w.WriteStartElement("ExtraAddon");

					// <Mode>
					w.WriteStartElement("Mode");

					// <Remove>
					w.WriteStartElement("Remove");
					w.WriteElementString("Enabled", (!xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Enabled").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Enabled").InnerText : _enabled.ToString()));
					w.WriteElementString("Type",    (!xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Type").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Type").InnerText : _type));

					// </Remove>
					w.WriteEndElement();

					// </Mode>
					w.WriteEndElement();

					// <Weather>
					w.WriteStartElement("Weather");

					// <Home>
					w.WriteStartElement("Home");
					w.WriteElementString("City",    (!xmldoc.SelectSingleNode("ExtraAddon/Weather/Home/City").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/Weather/Home/City").InnerText : _weatherhomecity));

					// </Home>
					w.WriteEndElement();

					// </Weather>
					w.WriteEndElement();

					// <WolframAlpha>
					w.WriteStartElement("WolframAlpha");

					// <Api>
					w.WriteStartElement("Api");
					w.WriteElementString("Key",     (!xmldoc.SelectSingleNode("ExtraAddon/WolframAlpha/Api/Key").IsNull() ? xmldoc.SelectSingleNode("ExtraAddon/WolframAlpha/Api/Key").InnerText : _wolframalphaapikey));

					// </Api>
					w.WriteEndElement();

					// </WolframAlpha>
					w.WriteEndElement();

					// </ExtraAddon>
					w.WriteEndElement();

					w.Flush();
					w.Close();

					if(File.Exists(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile)))
						File.Delete(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile));

					Log.Success("ExtraAddonConfig", sLocalization.Config("Text5"));
					return false;
				}
				catch(Exception e)
				{
					Log.Error("ExtraAddonConfig", sLocalization.Config("Text6"), e.Message);
					return false;
				}
			}
		}
	}

	public sealed class ModeConfig
	{
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		public static bool RemoveEnabled { get; private set; }
		public static string RemoveType { get; private set; }

		public ModeConfig(bool removeenabled, string removetype)
		{
			RemoveEnabled = removeenabled;
			RemoveType    = removetype;
			Log.Notice("ModeConfig", sLocalization.ModeConfig("Text"));
		}
	}

	public sealed class WeatherConfig
	{
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		public static string City { get; private set; }

		public WeatherConfig(string city)
		{
			City = city;
			Log.Notice("WeatherConfig", sLocalization.WeatherConfig("Text"));
		}
	}

	public sealed class WolframAlphaConfig
	{
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		public static string Key { get; private set; }

		public WolframAlphaConfig(string key)
		{
			Key = key;
			Log.Notice("WorlframAlphaConfig", sLocalization.WolframAlphaConfig("Text"));
		}
	}
}