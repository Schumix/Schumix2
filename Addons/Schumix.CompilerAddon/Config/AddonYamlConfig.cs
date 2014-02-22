/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using System.Text;
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Schumix.CompilerAddon.Config
{
	sealed class AddonYamlConfig : AddonDefaultConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly Dictionary<YamlNode, YamlNode> NullYMap = null;

		public AddonYamlConfig()
		{
		}

		public AddonYamlConfig(string configdir, string configfile)
		{
			var yaml = new YamlStream();
			yaml.Load(File.OpenText(Path.Combine(configdir, configfile)));

			Log.Notice("CompilerAddonConfig", sLConsole.GetString("Config file is loading."));

			var compilermap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey("CompilerAddon")) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children["CompilerAddon".ToYamlNode()]).Children : NullYMap;
			CompilerMap((!compilermap.IsNull() && compilermap.ContainsKey("Compiler")) ? ((YamlMappingNode)compilermap["Compiler".ToYamlNode()]).Children : NullYMap);

			Log.Success("CompilerAddonConfig", sLConsole.GetString("Config database is loading."));
		}

		~AddonYamlConfig()
		{
		}

		public bool CreateConfig(string ConfigDirectory, string ConfigFile)
		{
			string filename = Path.Combine(ConfigDirectory, ConfigFile);

			if(File.Exists(filename))
				return true;
			else
			{
				Log.Error("CompilerAddonConfig", sLConsole.GetString("No such config file!"));
				Log.Debug("CompilerAddonConfig", sLConsole.GetString("Preparing..."));
				var yaml = new YamlStream();
				string filename2 = Path.Combine(ConfigDirectory, "_" + ConfigFile);

				if(File.Exists(filename2))
				{
					Log.Notice("CompilerAddonConfig", sLConsole.GetString("The backup files will be used to renew the data."));
					yaml.Load(File.OpenText(filename2));
				}

				try
				{
					var compilermap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey("CompilerAddon")) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children["CompilerAddon".ToYamlNode()]).Children : NullYMap;
					var nodes = new YamlMappingNode();
					var nodes2 = new YamlMappingNode();
					nodes2.Add("Compiler", CreateCompilerMap((!compilermap.IsNull() && compilermap.ContainsKey("Compiler")) ? ((YamlMappingNode)compilermap["Compiler".ToYamlNode()]).Children : NullYMap));
					nodes.Add("CompilerAddon", nodes2);

					sUtilities.CreateFile(filename);
					var file = new StreamWriter(filename, true) { AutoFlush = true };
					file.Write(nodes.Children.ToString("CompilerAddon"));
					file.Close();

					if(File.Exists(filename2))
					{
						Log.Notice("CompilerAddonConfig", sLConsole.GetString("The backup has been deleted during the re-use."));
						File.Delete(filename2);
					}

					Log.Success("CompilerAddonConfig", sLConsole.GetString("Config file is completed!"));
				}
				catch(Exception e)
				{
					Log.Error("CompilerAddonConfig", sLConsole.GetString("Failure was handled during the xml writing. Details: {0}"), e.Message);
				}
			}

			return false;
		}

		private void CompilerMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool CompilerEnabled = (!nodes.IsNull() && nodes.ContainsKey("Enabled")) ? nodes["Enabled".ToYamlNode()].ToString().ToBoolean() : d_compilerenabled;
			bool Enabled = d_enabled;
			int Memory = d_memory;

			if(!nodes.IsNull() && nodes.ContainsKey("MaxAllocating"))
			{
				var node2 = ((YamlMappingNode)nodes["MaxAllocating".ToYamlNode()]).Children;
				Enabled = (!node2.IsNull() && node2.ContainsKey("Enabled")) ? node2["Enabled".ToYamlNode()].ToString().ToBoolean() : d_enabled;
				Memory = (!node2.IsNull() && node2.ContainsKey("Memory")) ? node2["Memory".ToYamlNode()].ToString().ToInt32() : d_memory;
			}

			string CompilerOptions = (!nodes.IsNull() && nodes.ContainsKey("CompilerOptions")) ? nodes["CompilerOptions".ToYamlNode()].ToString() : d_compileroptions;
			int WarningLevel = (!nodes.IsNull() && nodes.ContainsKey("WarningLevel")) ? nodes["WarningLevel".ToYamlNode()].ToString().ToInt32() : d_warninglevel;
			bool TreatWarningsAsErrors = (!nodes.IsNull() && nodes.ContainsKey("TreatWarningsAsErrors")) ? nodes["TreatWarningsAsErrors".ToYamlNode()].ToString().ToBoolean() : d_treatwarningsaserrors;
			string Referenced = (!nodes.IsNull() && nodes.ContainsKey("Referenced")) ? nodes["Referenced".ToYamlNode()].ToString() : d_referenced;
			string ReferencedAssemblies = (!nodes.IsNull() && nodes.ContainsKey("ReferencedAssemblies")) ? nodes["ReferencedAssemblies".ToYamlNode()].ToString() : d_referencedassemblies;
			string MainClass = (!nodes.IsNull() && nodes.ContainsKey("MainClass")) ? nodes["MainClass".ToYamlNode()].ToString() : d_mainclass;
			string MainConstructor = (!nodes.IsNull() && nodes.ContainsKey("MainConstructor")) ? nodes["MainConstructor".ToYamlNode()].ToString() : d_mainconstructor;
			new CompilerConfig(CompilerEnabled, Enabled, Memory, CompilerOptions, WarningLevel, TreatWarningsAsErrors, Referenced, ReferencedAssemblies, MainClass, MainConstructor);
		}

		private YamlMappingNode CreateCompilerMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Enabled",               (!nodes.IsNull() && nodes.ContainsKey("Enabled")) ? nodes["Enabled".ToYamlNode()].ToString() : d_compilerenabled.ToString());

			var map2 = new YamlMappingNode();

			if(!nodes.IsNull() && nodes.ContainsKey("MaxAllocating"))
			{
				var node2 = ((YamlMappingNode)nodes["MaxAllocating".ToYamlNode()]).Children;
				map2.Add("Enabled", (!node2.IsNull() && node2.ContainsKey("Enabled")) ? node2["Enabled".ToYamlNode()].ToString() : d_enabled.ToString());
				map2.Add("Memory",  (!node2.IsNull() && node2.ContainsKey("Memory")) ? node2["Memory".ToYamlNode()].ToString() : d_memory.ToString());
			}
			else
			{
				map2.Add("Enabled", d_enabled.ToString());
				map2.Add("Memory",  d_memory.ToString());
			}

			map.Add("MaxAllocating",         map2);
			map.Add("CompilerOptions",       (!nodes.IsNull() && nodes.ContainsKey("CompilerOptions")) ? nodes["CompilerOptions".ToYamlNode()].ToString() : d_compileroptions);
			map.Add("WarningLevel",          (!nodes.IsNull() && nodes.ContainsKey("WarningLevel")) ? nodes["WarningLevel".ToYamlNode()].ToString() : d_warninglevel.ToString());
			map.Add("TreatWarningsAsErrors", (!nodes.IsNull() && nodes.ContainsKey("TreatWarningsAsErrors")) ? nodes["TreatWarningsAsErrors".ToYamlNode()].ToString() : d_treatwarningsaserrors.ToString());
			map.Add("Referenced",            (!nodes.IsNull() && nodes.ContainsKey("Referenced")) ? nodes["Referenced".ToYamlNode()].ToString() : d_referenced);
			map.Add("ReferencedAssemblies",  (!nodes.IsNull() && nodes.ContainsKey("ReferencedAssemblies")) ? nodes["ReferencedAssemblies".ToYamlNode()].ToString() : d_referencedassemblies);
			map.Add("MainClass",             (!nodes.IsNull() && nodes.ContainsKey("MainClass")) ? nodes["MainClass".ToYamlNode()].ToString() : d_mainclass);
			map.Add("MainConstructor",       (!nodes.IsNull() && nodes.ContainsKey("MainConstructor")) ? nodes["MainConstructor".ToYamlNode()].ToString() : d_mainconstructor);
			return map;
		}
	}
}