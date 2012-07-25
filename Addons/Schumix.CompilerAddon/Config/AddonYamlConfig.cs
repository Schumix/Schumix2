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
using System.Text;
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.CompilerAddon.Localization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Schumix.CompilerAddon.Config
{
	sealed class AddonYamlConfig : AddonDefaultConfig
	{
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		public AddonYamlConfig()
		{
		}

		public AddonYamlConfig(string configdir, string configfile)
		{
			var yaml = new YamlStream();
			yaml.Load(File.OpenText(sUtilities.DirectoryToHome(configdir, configfile)));

			Log.Notice("CompilerAddonConfig", sLocalization.Config("Text"));

			var compilermap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey(new YamlScalarNode("CompilerAddon"))) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children[new YamlScalarNode("CompilerAddon")]).Children : (Dictionary<YamlNode, YamlNode>)null;
			CompilerMap((!compilermap.IsNull() && compilermap.ContainsKey(new YamlScalarNode("Flooding"))) ? ((YamlMappingNode)compilermap[new YamlScalarNode("Flooding")]).Children : (Dictionary<YamlNode, YamlNode>)null);

			Log.Success("CompilerAddonConfig", sLocalization.Config("Text2"));
		}

		~AddonYamlConfig()
		{
		}

		public bool CreateConfig(string ConfigDirectory, string ConfigFile)
		{
			string filename = sUtilities.DirectoryToHome(ConfigDirectory, ConfigFile);

			if(File.Exists(filename))
				return true;
			else
			{
				Log.Error("CompilerAddonConfig", sLocalization.Config("Text3"));
				Log.Debug("CompilerAddonConfig", sLocalization.Config("Text4"));
				var yaml = new YamlStream();
				string filename2 = sUtilities.DirectoryToHome(ConfigDirectory, "_" + ConfigFile);

				if(File.Exists(filename2))
					yaml.Load(File.OpenText(filename2));

				try
				{
					var compilermap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey(new YamlScalarNode("CompilerAddon"))) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children[new YamlScalarNode("CompilerAddon")]).Children : (Dictionary<YamlNode, YamlNode>)null;
					var nodes = new YamlMappingNode();
					var nodes2 = new YamlMappingNode();
					nodes2.Add("Compiler", CreateCompilerMap((!compilermap.IsNull() && compilermap.ContainsKey(new YamlScalarNode("Compiler"))) ? ((YamlMappingNode)compilermap[new YamlScalarNode("Compiler")]).Children : (Dictionary<YamlNode, YamlNode>)null));
					nodes.Add("CompilerAddon", nodes2);

					sUtilities.CreateFile(filename);
					var file = new StreamWriter(filename, true) { AutoFlush = true };
					file.Write(ToString(nodes.Children));
					file.Close();

					if(File.Exists(filename2))
						File.Delete(filename2);

					Log.Success("CompilerAddonConfig", sLocalization.Config("Text5"));
				}
				catch(Exception e)
				{
					Log.Error("CompilerAddonConfig", sLocalization.Config("Text6"), e.Message);
				}
			}

			return false;
		}

		private void CompilerMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool CompilerEnabled = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Enabled"))) ? Convert.ToBoolean(nodes[new YamlScalarNode("Enabled")].ToString()) : d_compilerenabled;
			bool Enabled = d_enabled;
			int Memory = d_memory;

			if(!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("MaxAllocating")))
			{
				var node2 = ((YamlMappingNode)nodes[new YamlScalarNode("MaxAllocating")]).Children;
				Enabled = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Enabled"))) ? Convert.ToBoolean(node2[new YamlScalarNode("Enabled")].ToString()) : d_enabled;
				Memory = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Memory"))) ? Convert.ToInt32(node2[new YamlScalarNode("Memory")].ToString()) : d_memory;
			}

			string CompilerOptions = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("CompilerOptions"))) ? nodes[new YamlScalarNode("CompilerOptions")].ToString() : d_compileroptions;
			int WarningLevel = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("WarningLevel"))) ? Convert.ToInt32(nodes[new YamlScalarNode("WarningLevel")].ToString()) : d_warninglevel;
			bool TreatWarningsAsErrors = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("TreatWarningsAsErrors"))) ? Convert.ToBoolean(nodes[new YamlScalarNode("TreatWarningsAsErrors")].ToString()) : d_treatwarningsaserrors;
			string Referenced = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Referenced"))) ? nodes[new YamlScalarNode("Referenced")].ToString() : d_referenced;
			string ReferencedAssemblies = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("ReferencedAssemblies"))) ? nodes[new YamlScalarNode("ReferencedAssemblies")].ToString() : d_referencedassemblies;
			string MainClass = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("MainClass"))) ? nodes[new YamlScalarNode("MainClass")].ToString() : d_mainclass;
			string MainConstructor = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("MainConstructor"))) ? nodes[new YamlScalarNode("MainConstructor")].ToString() : d_mainconstructor;
			new CompilerConfig(CompilerEnabled, Enabled, Memory, CompilerOptions, WarningLevel, TreatWarningsAsErrors, Referenced, ReferencedAssemblies, MainClass, MainConstructor);
		}

		private YamlMappingNode CreateCompilerMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Enabled",               (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Enabled"))) ? nodes[new YamlScalarNode("Enabled")].ToString() : d_compilerenabled.ToString());

			var map2 = new YamlMappingNode();

			if(!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("MaxAllocating")))
			{
				var node2 = ((YamlMappingNode)nodes[new YamlScalarNode("MaxAllocating")]).Children;
				map2.Add("Enabled", (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Enabled"))) ? node2[new YamlScalarNode("Enabled")].ToString() : d_enabled.ToString());
				map2.Add("Memory",  (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Memory"))) ? node2[new YamlScalarNode("Memory")].ToString() : d_memory.ToString());
			}
			else
			{
				map2.Add("Enabled", d_enabled.ToString());
				map2.Add("Memory",  d_memory.ToString());
			}

			map.Add("MaxAllocating",         map2);
			map.Add("CompilerOptions",       (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("CompilerOptions"))) ? nodes[new YamlScalarNode("CompilerOptions")].ToString() : d_compileroptions);
			map.Add("WarningLevel",          (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("WarningLevel"))) ? nodes[new YamlScalarNode("WarningLevel")].ToString() : d_warninglevel.ToString());
			map.Add("TreatWarningsAsErrors", (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("TreatWarningsAsErrors"))) ? nodes[new YamlScalarNode("TreatWarningsAsErrors")].ToString() : d_treatwarningsaserrors.ToString());
			map.Add("Referenced",            (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Referenced"))) ? nodes[new YamlScalarNode("Referenced")].ToString() : d_referenced);
			map.Add("ReferencedAssemblies",  (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("ReferencedAssemblies"))) ? nodes[new YamlScalarNode("ReferencedAssemblies")].ToString() : d_referencedassemblies);
			map.Add("MainClass",             (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("MainClass"))) ? nodes[new YamlScalarNode("MainClass")].ToString() : d_mainclass);
			map.Add("MainConstructor",       (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("MainConstructor"))) ? nodes[new YamlScalarNode("MainConstructor")].ToString() : d_mainconstructor);
			return map;
		}

		private string ToString(IDictionary<YamlNode, YamlNode> nodes)
		{
			var text = new StringBuilder();

			foreach(var child in nodes)
			{
				if(((YamlMappingNode)child.Value).Children.Count > 0)
					text.Append(child.Key).Append(":\n").Append(child.Value).Append(SchumixBase.NewLine);
				else
					text.Append(child.Key).Append(": ").Append(child.Value).Append(SchumixBase.NewLine);
			}

			text.Replace("{ { ", "    ");
			text.Replace("{ ", "    ");
			text.Replace(" }", SchumixBase.NewLine.ToString());
			text.Replace("\n\n\n", SchumixBase.NewLine.ToString());
			text.Replace("\n\n", SchumixBase.NewLine.ToString());
			text.Replace(", ", ": ");

			var split = text.ToString().Split(SchumixBase.NewLine);
			text.Remove(0, text.Length);

			foreach(var st in split)
				text.Append(st.Remove(0, 2, ": ") + SchumixBase.NewLine.ToString());

			split = text.ToString().Split(SchumixBase.NewLine);
			text.Remove(0, text.Length);

			foreach(var st in split)
			{
				if(st.Contains(": "))
				{
					string a = st.Remove(0, st.IndexOf(": ") + 2);
					if(a.Contains(": "))
						text.Append(st.Substring(0, st.IndexOf(": ") + 2) + SchumixBase.NewLine.ToString() + st.Substring(st.IndexOf(": ") + 2) + SchumixBase.NewLine.ToString());
					else
						text.Append(st.Remove(0, 2, ": ") + SchumixBase.NewLine.ToString());
				}
				else
					text.Append(st + SchumixBase.NewLine.ToString());
			}

			split = text.ToString().Split(SchumixBase.NewLine);
			text.Remove(0, text.Length);

			foreach(var stt in split)
			{
				string st = stt;

				if(st.Trim() == string.Empty)
					continue;

				if(st.EndsWith(": "))
					st = st.Replace(": ", SchumixBase.Colon.ToString());

				if(!st.EndsWith(SchumixBase.Colon.ToString()))
					text.Append("    " + st + SchumixBase.NewLine.ToString());
				else
					text.Append(st + SchumixBase.NewLine.ToString());
			}

			split = text.ToString().Split(SchumixBase.NewLine);
			text.Remove(0, text.Length);
			bool e = false;

			foreach(var st in split)
			{
				if(st.ToString().Contains("MaxAllocating"))
					e = true;

				if(e)
					text.Append("    " + st + SchumixBase.NewLine.ToString());
				else
					text.Append(st + SchumixBase.NewLine.ToString());

				if(st.ToString().Contains("Memory"))
					e = false;
			}

			return "# CompilerAddon config file (yaml)\n" + text.ToString();
		}
	}
}