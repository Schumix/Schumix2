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

namespace Schumix.ExtraAddon.Config
{
	public sealed class AddonConfig
	{
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
				Log.Error("ExtraAddonConfig", "Hiba oka: {0}", e.Message);
			}
		}

		private void Init(string configfile)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(string.Format("./{0}/{1}", SchumixConfig.ConfigDirectory, configfile));

			Log.Notice("ExtraAddonConfig", "Config fajl betoltese.");

			bool Enabled = Convert.ToBoolean(xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Enabled").InnerText);
			string Type = xmldoc.SelectSingleNode("ExtraAddon/Mode/Remove/Type").InnerText;
			new ModeConfig(Enabled, Type);

			Log.Success("ExtraAddonConfig", "Config adatbazis betoltve.");
			Console.WriteLine();
		}

		private bool IsConfig(string ConfigDirectory, string ConfigFile)
		{
			if(File.Exists(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile)))
				return true;
			else
			{
				Log.Error("ExtraAddonConfig", "Nincs config fajl!");
				Log.Debug("ExtraAddonConfig", "Elkeszitese folyamatban...");
				var w = new XmlTextWriter(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile), null);

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
					w.WriteElementString("Enabled", "false");
					w.WriteElementString("Type", "aohv");

					// </Remove>
					w.WriteEndElement();

					// </Mode>
					w.WriteEndElement();

					// </ExtraAddon>
					w.WriteEndElement();

					w.Flush();
					w.Close();

					Log.Success("ExtraAddonConfig", "Config fajl elkeszult!");
					return false;
				}
				catch(Exception e)
				{
					Log.Error("ExtraAddonConfig", "Hiba az xml irasa soran: {0}", e.Message);
					return false;
				}
			}
		}
	}

	public sealed class ModeConfig
	{
		public static bool RemoveEnabled { get; private set; }
		public static string RemoveType { get; private set; }

		public ModeConfig(bool removeenabled, string removetype)
		{
			RemoveEnabled = removeenabled;
			RemoveType    = removetype;
			Log.Notice("ModeConfig", "Mode beallitasai betoltve.");
		}
	}
}