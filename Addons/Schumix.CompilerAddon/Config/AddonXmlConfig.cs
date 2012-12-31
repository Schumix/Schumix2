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
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.CompilerAddon.Localization;

namespace Schumix.CompilerAddon.Config
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

			Log.Notice("CompilerAddonConfig", sLocalization.Config("Text"));

			bool CompilerEnabled = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("CompilerAddon/Compiler/Enabled").InnerText) : d_compilerenabled;
			bool Enabled = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Enabled").InnerText) : d_enabled;
			int Memory = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Memory").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Memory").InnerText) : d_memory;
			string CompilerOptions = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/CompilerOptions").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/CompilerOptions").InnerText : d_compileroptions;
			int WarningLevel = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/WarningLevel").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("CompilerAddon/Compiler/WarningLevel").InnerText) : d_warninglevel;
			bool TreatWarningsAsErrors = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/TreatWarningsAsErrors").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("CompilerAddon/Compiler/TreatWarningsAsErrors").InnerText) : d_treatwarningsaserrors;
			string Referenced = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/Referenced").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/Referenced").InnerText : d_referenced;
			string ReferencedAssemblies = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/ReferencedAssemblies").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/ReferencedAssemblies").InnerText : d_referencedassemblies;
			string MainClass = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainClass").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainClass").InnerText : d_mainclass;
			string MainConstructor = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainConstructor").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainConstructor").InnerText : d_mainconstructor;
			new CompilerConfig(CompilerEnabled, Enabled, Memory, CompilerOptions, WarningLevel, TreatWarningsAsErrors, Referenced, ReferencedAssemblies, MainClass, MainConstructor);

			Log.Success("CompilerAddonConfig", sLocalization.Config("Text2"));
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
				Log.Error("CompilerAddonConfig", sLocalization.Config("Text3"));
				Log.Debug("CompilerAddonConfig", sLocalization.Config("Text4"));
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

					// <CompilerAddon>
					w.WriteStartElement("CompilerAddon");

					// <Compiler>
					w.WriteStartElement("Compiler");
					w.WriteElementString("Enabled",               (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/Enabled").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/Enabled").InnerText : d_compilerenabled.ToString()));

					// <MaxAllocating>
					w.WriteStartElement("MaxAllocating");
					w.WriteElementString("Enabled",               (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Enabled").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Enabled").InnerText : d_enabled.ToString()));
					w.WriteElementString("Memory",                (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Memory").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Memory").InnerText : d_memory.ToString()));

					// </MaxAllocating>
					w.WriteEndElement();

					w.WriteElementString("CompilerOptions",       (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/CompilerOptions").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/CompilerOptions").InnerText : d_compileroptions));
					w.WriteElementString("WarningLevel",          (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/WarningLevel").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/WarningLevel").InnerText : d_warninglevel.ToString()));
					w.WriteElementString("TreatWarningsAsErrors", (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/TreatWarningsAsErrors").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/TreatWarningsAsErrors").InnerText : d_treatwarningsaserrors.ToString()));
					w.WriteElementString("Referenced",            (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/Referenced").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/Referenced").InnerText : d_referenced));
					w.WriteElementString("ReferencedAssemblies",  (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/ReferencedAssemblies").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/ReferencedAssemblies").InnerText : d_referencedassemblies));
					w.WriteElementString("MainClass",             (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainClass").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainClass").InnerText : d_mainclass));
					w.WriteElementString("MainConstructor",       (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainConstructor").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainConstructor").InnerText : d_mainconstructor));

					// </Compiler>
					w.WriteEndElement();

					// </CompilerAddon>
					w.WriteEndElement();

					w.Flush();
					w.Close();

					if(File.Exists(filename2))
						File.Delete(filename2);

					Log.Success("CompilerAddonConfig", sLocalization.Config("Text5"));
				}
				catch(Exception e)
				{
					Log.Error("CompilerAddonConfig", sLocalization.Config("Text6"), e.Message);
				}

				return false;
			}
		}
	}
}