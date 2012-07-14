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
using Schumix.CompilerAddon.Localization;

namespace Schumix.CompilerAddon.Config
{
	sealed class AddonConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private const bool _compilerenabled = true;
		private const bool _enabled = true;
		private const int _memory = 50;
		private const string _compileroptions = "/optimize";
		private const int _warninglevel = 4;
		private const bool _treatwarningsaserrors = false;
		private const string _referenced = "using System; using System.Threading; using System.Reflection; " +
			"using System.Threading.Tasks; using System.Linq; using System.Collections.Generic; using System.Text; " +
			"using System.Text.RegularExpressions; using System.Numerics; using Schumix.Libraries; " +
			"using Schumix.Libraries.Mathematics; using Schumix.Libraries.Mathematics.Types;";
		private const string _referencedassemblies = "System.dll,System.Core.dll,System.Numerics.dll,Schumix.Libraries.dll";
		private const string _mainclass = "Entry";
		private const string _mainconstructor = "Schumix";

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
				Log.Error("CompilerAddonConfig", sLConsole.Exception("Error"), e.Message);
			}
		}

		private void Init(string configfile)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(sUtilities.DirectoryToHome(SchumixConfig.ConfigDirectory, configfile));

			Log.Notice("CompilerAddonConfig", sLocalization.Config("Text"));

			bool CompilerEnabled = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("CompilerAddon/Compiler/Enabled").InnerText) : _compilerenabled;
			bool Enabled = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Enabled").InnerText) : _enabled;
			int Memory = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Memory").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Memory").InnerText) : _memory;
			string CompilerOptions = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/CompilerOptions").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/CompilerOptions").InnerText : _compileroptions;
			int WarningLevel = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/WarningLevel").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("CompilerAddon/Compiler/WarningLevel").InnerText) : _warninglevel;
			bool TreatWarningsAsErrors = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/TreatWarningsAsErrors").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("CompilerAddon/Compiler/TreatWarningsAsErrors").InnerText) : _treatwarningsaserrors;
			string Referenced = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/Referenced").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/Referenced").InnerText : _referenced;
			string ReferencedAssemblies = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/ReferencedAssemblies").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/ReferencedAssemblies").InnerText : _referencedassemblies;
			string MainClass = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainClass").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainClass").InnerText : _mainclass;
			string MainConstructor = !xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainConstructor").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainConstructor").InnerText : _mainconstructor;
			new CompilerConfig(CompilerEnabled, Enabled, Memory, CompilerOptions, WarningLevel, TreatWarningsAsErrors, Referenced, ReferencedAssemblies, MainClass, MainConstructor);

			Log.Success("CompilerAddonConfig", sLocalization.Config("Text2"));
			Console.WriteLine();
		}

		private bool IsConfig(string ConfigDirectory, string ConfigFile)
		{
			string filename = sUtilities.DirectoryToHome(ConfigDirectory, ConfigFile);

			if(File.Exists(filename))
				return true;
			else
			{
				Log.Error("CompilerAddonConfig", sLocalization.Config("Text3"));
				Log.Debug("CompilerAddonConfig", sLocalization.Config("Text4"));
				var w = new XmlTextWriter(filename, null);
				var xmldoc = new XmlDocument();
				string filename2 = sUtilities.DirectoryToHome(ConfigDirectory, "_" + ConfigFile);

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
					w.WriteElementString("Enabled",               (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/Enabled").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/Enabled").InnerText : _compilerenabled.ToString()));

					// <MaxAllocating>
					w.WriteStartElement("MaxAllocating");
					w.WriteElementString("Enabled",               (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Enabled").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Enabled").InnerText : _enabled.ToString()));
					w.WriteElementString("Memory",                (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Memory").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/MaxAllocating/Memory").InnerText : _memory.ToString()));

					// </MaxAllocating>
					w.WriteEndElement();

					w.WriteElementString("CompilerOptions",       (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/CompilerOptions").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/CompilerOptions").InnerText : _compileroptions));
					w.WriteElementString("WarningLevel",          (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/WarningLevel").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/WarningLevel").InnerText : _warninglevel.ToString()));
					w.WriteElementString("TreatWarningsAsErrors", (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/TreatWarningsAsErrors").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/TreatWarningsAsErrors").InnerText : _treatwarningsaserrors.ToString()));
					w.WriteElementString("Referenced",            (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/Referenced").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/Referenced").InnerText : _referenced));
					w.WriteElementString("ReferencedAssemblies",  (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/ReferencedAssemblies").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/ReferencedAssemblies").InnerText : _referencedassemblies));
					w.WriteElementString("MainClass",             (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainClass").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainClass").InnerText : _mainclass));
					w.WriteElementString("MainConstructor",       (!xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainConstructor").IsNull() ? xmldoc.SelectSingleNode("CompilerAddon/Compiler/MainConstructor").InnerText : _mainconstructor));

					// </Compiler>
					w.WriteEndElement();

					// </CompilerAddon>
					w.WriteEndElement();

					w.Flush();
					w.Close();

					if(File.Exists(filename2))
						File.Delete(filename2);

					Log.Success("CompilerAddonConfig", sLocalization.Config("Text5"));
					return false;
				}
				catch(Exception e)
				{
					Log.Error("CompilerAddonConfig", sLocalization.Config("Text6"), e.Message);
					return false;
				}
			}
		}
	}
}