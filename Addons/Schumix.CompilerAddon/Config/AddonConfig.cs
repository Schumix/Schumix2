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

namespace Schumix.CompilerAddon.Config
{
	public sealed class AddonConfig
	{
		public AddonConfig(string configfile)
		{
			try
			{
				Log.Debug("CompilerAddonConfig", ">> {0}", configfile);

				if(!IsConfig(SchumixConfig.ConfigDirectory, configfile))
					Init(configfile);
				else
					Init(configfile);
			}
			catch(Exception e)
			{
				Log.Error("CompilerAddonConfig", "Hiba oka: {0}", e.Message);
			}
		}

		private void Init(string configfile)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(string.Format("./{0}/{1}", SchumixConfig.ConfigDirectory, configfile));

			Log.Notice("CompilerAddonConfig", "Config fajl betoltese.");

			bool CompilerEnabled = Convert.ToBoolean(xmldoc.SelectSingleNode("CompilerAddon/Compiler/Enabled").InnerText);
			bool Enabled = Convert.ToBoolean(xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Enabled").InnerText);
			int Memory = Convert.ToInt32(xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Memory").InnerText);
			new CompilerConfig(CompilerEnabled, Enabled, Memory);

			Log.Success("CompilerAddonConfig", "Config adatbazis betoltve.");
			Console.WriteLine();
		}

		private bool IsConfig(string ConfigDirectory, string ConfigFile)
		{
			if(File.Exists(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile)))
				return true;
			else
			{
				Log.Error("CompilerAddonConfig", "Nincs config fajl!");
				Log.Debug("CompilerAddonConfig", "Elkeszitese folyamatban...");
				var w = new XmlTextWriter(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile), null);

				try
				{
					w.Formatting = Formatting.Indented;
					w.Indentation = 4;
					w.Namespaces = false;
					w.WriteStartDocument();

					// <CompilerAddon>
					w.WriteStartElement("CompilerAddon");

					// <Compiler>
					w.WriteStartElement("Compiler");
					w.WriteElementString("Enabled", "true");

					// <MaxAllocating>
					w.WriteStartElement("MaxAllocating");
					w.WriteElementString("Enabled", "true");
					w.WriteElementString("Memory", "50");

					// </MaxAllocating>
					w.WriteEndElement();

					// </Compiler>
					w.WriteEndElement();

					// </CompilerAddon>
					w.WriteEndElement();

					w.Flush();
					w.Close();

					Log.Success("CompilerAddonConfig", "Config fajl elkeszult!");
					return false;
				}
				catch(Exception e)
				{
					Log.Error("CompilerAddonConfig", "Hiba az xml irasa soran: {0}", e.Message);
					return false;
				}
			}
		}
	}

	public sealed class CompilerConfig
	{
		public static bool CompilerEnabled { get; private set; }
		public static bool MaxAllocatingE { get; private set; }
		public static int MaxAllocatingM { get; private set; }

		public CompilerConfig(bool compilerenabled, bool maxallocatinge, int maxallocatingm)
		{
			CompilerEnabled = compilerenabled;
			MaxAllocatingE  = maxallocatinge;
			MaxAllocatingM  = maxallocatingm;
			Log.Notice("Compile", "Compile beallitasai betoltve.");
		}
	}
}