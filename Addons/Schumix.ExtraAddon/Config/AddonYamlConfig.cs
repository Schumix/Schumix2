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
using Schumix.ExtraAddon.Localization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Schumix.ExtraAddon.Config
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

			Log.Notice("ExtraAddonConfig", sLocalization.Config("Text"));

			var extramap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey(new YamlScalarNode("ExtraAddon"))) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children[new YamlScalarNode("ExtraAddon")]).Children : (Dictionary<YamlNode, YamlNode>)null;
			ModeMap((!extramap.IsNull() && extramap.ContainsKey(new YamlScalarNode("Mode"))) ? ((YamlMappingNode)extramap[new YamlScalarNode("Mode")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			WeatherMap((!extramap.IsNull() && extramap.ContainsKey(new YamlScalarNode("Weather"))) ? ((YamlMappingNode)extramap[new YamlScalarNode("Weather")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			WolframAlphaMap((!extramap.IsNull() && extramap.ContainsKey(new YamlScalarNode("WolframAlpha"))) ? ((YamlMappingNode)extramap[new YamlScalarNode("WolframAlpha")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			Log.Success("ExtraAddonConfig", sLocalization.Config("Text2"));
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
				Log.Error("ExtraAddonConfig", sLocalization.Config("Text3"));
				Log.Debug("ExtraAddonConfig", sLocalization.Config("Text4"));
				var yaml = new YamlStream();
				string filename2 = sUtilities.DirectoryToHome(ConfigDirectory, "_" + ConfigFile);

				if(File.Exists(filename2))
					yaml.Load(File.OpenText(filename2));

				try
				{
					var extramap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey(new YamlScalarNode("ExtraAddon"))) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children[new YamlScalarNode("ExtraAddon")]).Children : (Dictionary<YamlNode, YamlNode>)null;
					var nodes = new YamlMappingNode();
					var nodes2 = new YamlMappingNode();
					nodes2.Add("Mode",         CreateModeMap((!extramap.IsNull() && extramap.ContainsKey(new YamlScalarNode("Mode"))) ? ((YamlMappingNode)extramap[new YamlScalarNode("Mode")]).Children : (Dictionary<YamlNode, YamlNode>)null));
					nodes2.Add("Weather",      CreateWeatherMap((!extramap.IsNull() && extramap.ContainsKey(new YamlScalarNode("Weather"))) ? ((YamlMappingNode)extramap[new YamlScalarNode("Weather")]).Children : (Dictionary<YamlNode, YamlNode>)null));
					nodes2.Add("WolframAlpha", CreateWolframAlphaMap((!extramap.IsNull() && extramap.ContainsKey(new YamlScalarNode("WolframAlpha"))) ? ((YamlMappingNode)extramap[new YamlScalarNode("WolframAlpha")]).Children : (Dictionary<YamlNode, YamlNode>)null));
					nodes.Add("ExtraAddon",    nodes2);

					sUtilities.CreateFile(filename);
					var file = new StreamWriter(filename, true) { AutoFlush = true };
					file.Write(ToString(nodes.Children));
					file.Close();

					if(File.Exists(filename2))
						File.Delete(filename2);

					Log.Success("ExtraAddonConfig", sLocalization.Config("Text5"));
				}
				catch(Exception e)
				{
					Log.Error("ExtraAddonConfig", sLocalization.Config("Text6"), e.Message);
				}
			}

			return false;
		}

		private void ModeMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool Enabled = d_enabled;
			string Type = d_type;

			if(!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Remove")))
			{
				var node2 = ((YamlMappingNode)nodes[new YamlScalarNode("Remove")]).Children;
				Enabled = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Enabled"))) ? Convert.ToBoolean(node2[new YamlScalarNode("Enabled")].ToString()) : d_enabled;
				Type = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Type"))) ? node2[new YamlScalarNode("Type")].ToString() : d_type;
			}

			new ModeConfig(Enabled, Type);
		}

		private void WeatherMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			string City = d_weatherhomecity;

			if(!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Home")))
			{
				var node2 = ((YamlMappingNode)nodes[new YamlScalarNode("Home")]).Children;
				City = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("City"))) ? node2[new YamlScalarNode("City")].ToString() : d_weatherhomecity;
			}

			new WeatherConfig(City);
		}

		private void WolframAlphaMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			string Key = d_wolframalphaapikey;

			if(!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Api")))
			{
				var node2 = ((YamlMappingNode)nodes[new YamlScalarNode("Api")]).Children;
				Key = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Key"))) ? node2[new YamlScalarNode("Key")].ToString() : d_wolframalphaapikey;
			}

			new WolframAlphaConfig(Key);
		}

		private YamlMappingNode CreateModeMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			var map2 = new YamlMappingNode();

			if(!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Remove")))
			{
				var node2 = ((YamlMappingNode)nodes[new YamlScalarNode("Remove")]).Children;
				map2.Add("Enabled", (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Enabled"))) ? node2[new YamlScalarNode("Enabled")].ToString() : d_enabled.ToString());
				map2.Add("Type",    (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Type"))) ? node2[new YamlScalarNode("Type")].ToString() : d_type);
			}
			else
			{
				map2.Add("Enabled", d_enabled.ToString());
				map2.Add("Type",    d_type);
			}

			map.Add("Remove", map2);
			return map;
		}

		private YamlMappingNode CreateWeatherMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			var map2 = new YamlMappingNode();

			if(!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Home")))
			{
				var node2 = ((YamlMappingNode)nodes[new YamlScalarNode("Home")]).Children;
				map2.Add("City", (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("City"))) ? node2[new YamlScalarNode("City")].ToString() : d_weatherhomecity);
			}
			else
				map2.Add("City", d_weatherhomecity);

			map.Add("Home", map2);
			return map;
		}

		private YamlMappingNode CreateWolframAlphaMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			var map2 = new YamlMappingNode();

			if(!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Api")))
			{
				var node2 = ((YamlMappingNode)nodes[new YamlScalarNode("Api")]).Children;
				map2.Add("Key", (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Key"))) ? node2[new YamlScalarNode("Key")].ToString() : d_wolframalphaapikey);
			}
			else
				map2.Add("Key", d_wolframalphaapikey);

			map.Add("Api", map2);
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

			text.Replace("Remove:", "Remove:\n   ");
			text.Replace("Home:", "Home:\n   ");
			text.Replace("Api:", "Api:\n   ");
			split = text.ToString().Split(SchumixBase.NewLine);
			text.Remove(0, text.Length);
			bool e = false;

			foreach(var st in split)
			{
				if(e)
					text.Append("    " + st + SchumixBase.NewLine.ToString());
				else
					text.Append(st + SchumixBase.NewLine.ToString());

				if(st.ToString().Contains("Remove") || st.ToString().Contains("Home") || st.ToString().Contains("Api"))
					e = true;

				if(st.ToString().Contains("Type") || st.ToString().Contains("City") || st.ToString().Contains("Key"))
					e = false;
			}

			return "# ExtraAddon config file (yaml)\n" + text.ToString();
		}
	}
}