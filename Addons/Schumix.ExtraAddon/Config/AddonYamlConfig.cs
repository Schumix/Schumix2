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
using System.Text;
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Schumix.ExtraAddon.Config
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
			yaml.Load(File.OpenText(sUtilities.DirectoryToSpecial(configdir, configfile)));

			Log.Notice("ExtraAddonConfig", sLConsole.GetString("Config file is loading."));

			var extramap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey("ExtraAddon")) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children["ExtraAddon".ToYamlNode()]).Children : NullYMap;
			ModeMap((!extramap.IsNull() && extramap.ContainsKey("Mode")) ? ((YamlMappingNode)extramap["Mode".ToYamlNode()]).Children : NullYMap);
			WeatherMap((!extramap.IsNull() && extramap.ContainsKey("Weather")) ? ((YamlMappingNode)extramap["Weather".ToYamlNode()]).Children : NullYMap);
			WolframAlphaMap((!extramap.IsNull() && extramap.ContainsKey("WolframAlpha")) ? ((YamlMappingNode)extramap["WolframAlpha".ToYamlNode()]).Children : NullYMap);
			Log.Success("ExtraAddonConfig", sLConsole.GetString("Config database is loading."));
		}

		~AddonYamlConfig()
		{
		}

		public bool CreateConfig(string ConfigDirectory, string ConfigFile)
		{
			string filename = sUtilities.DirectoryToSpecial(ConfigDirectory, ConfigFile);

			if(File.Exists(filename))
				return true;
			else
			{
				Log.Error("ExtraAddonConfig", sLConsole.GetString("No such config file!"));
				Log.Debug("ExtraAddonConfig", sLConsole.GetString("Preparing..."));
				var yaml = new YamlStream();
				string filename2 = sUtilities.DirectoryToSpecial(ConfigDirectory, "_" + ConfigFile);

				if(File.Exists(filename2))
				{
					Log.Notice("ExtraAddonConfig", sLConsole.GetString("The backup files will be used to renew the data."));
					yaml.Load(File.OpenText(filename2));
				}

				try
				{
					var extramap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey("ExtraAddon")) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children["ExtraAddon".ToYamlNode()]).Children : NullYMap;
					var nodes = new YamlMappingNode();
					var nodes2 = new YamlMappingNode();
					nodes2.Add("Mode",         CreateModeMap((!extramap.IsNull() && extramap.ContainsKey("Mode")) ? ((YamlMappingNode)extramap["Mode".ToYamlNode()]).Children : NullYMap));
					nodes2.Add("Weather",      CreateWeatherMap((!extramap.IsNull() && extramap.ContainsKey("Weather")) ? ((YamlMappingNode)extramap["Weather".ToYamlNode()]).Children : NullYMap));
					nodes2.Add("WolframAlpha", CreateWolframAlphaMap((!extramap.IsNull() && extramap.ContainsKey("WolframAlpha")) ? ((YamlMappingNode)extramap["WolframAlpha".ToYamlNode()]).Children : NullYMap));
					nodes.Add("ExtraAddon",    nodes2);

					sUtilities.CreateFile(filename);
					var file = new StreamWriter(filename, true) { AutoFlush = true };
					file.Write(nodes.Children.ToString("ExtraAddon"));
					file.Close();

					if(File.Exists(filename2))
					{
						Log.Notice("ExtraAddonConfig", sLConsole.GetString("The backup has been deleted during the re-use."));
						File.Delete(filename2);
					}

					Log.Success("ExtraAddonConfig", sLConsole.GetString("Config file is completed!"));
				}
				catch(Exception e)
				{
					Log.Error("ExtraAddonConfig", sLConsole.GetString("Failure was handled during the xml writing. Details: {0}"), e.Message);
				}
			}

			return false;
		}

		private void ModeMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool Enabled = d_enabled;
			string Type = d_type;

			if(!nodes.IsNull() && nodes.ContainsKey("Remove"))
			{
				var node2 = ((YamlMappingNode)nodes["Remove".ToYamlNode()]).Children;
				Enabled = (!node2.IsNull() && node2.ContainsKey("Enabled")) ? node2["Enabled".ToYamlNode()].ToString().ToBoolean() : d_enabled;
				Type = (!node2.IsNull() && node2.ContainsKey("Type")) ? node2["Type".ToYamlNode()].ToString() : d_type;
			}

			new ModeConfig(Enabled, Type);
		}

		private void WeatherMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			string City = d_weatherhomecity;

			if(!nodes.IsNull() && nodes.ContainsKey("Home"))
			{
				var node2 = ((YamlMappingNode)nodes["Home".ToYamlNode()]).Children;
				City = (!node2.IsNull() && node2.ContainsKey("City")) ? node2["City".ToYamlNode()].ToString() : d_weatherhomecity;
			}

			new WeatherConfig(City);
		}

		private void WolframAlphaMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			string Key = d_wolframalphaapikey;

			if(!nodes.IsNull() && nodes.ContainsKey("Api"))
			{
				var node2 = ((YamlMappingNode)nodes["Api".ToYamlNode()]).Children;
				Key = (!node2.IsNull() && node2.ContainsKey("Key")) ? node2["Key".ToYamlNode()].ToString() : d_wolframalphaapikey;
			}

			new WolframAlphaConfig(Key);
		}

		private YamlMappingNode CreateModeMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			var map2 = new YamlMappingNode();

			if(!nodes.IsNull() && nodes.ContainsKey("Remove"))
			{
				var node2 = ((YamlMappingNode)nodes["Remove".ToYamlNode()]).Children;
				map2.Add("Enabled", (!node2.IsNull() && node2.ContainsKey("Enabled")) ? node2["Enabled".ToYamlNode()].ToString() : d_enabled.ToString());
				map2.Add("Type",    (!node2.IsNull() && node2.ContainsKey("Type")) ? node2["Type".ToYamlNode()].ToString() : d_type);
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

			if(!nodes.IsNull() && nodes.ContainsKey("Home"))
			{
				var node2 = ((YamlMappingNode)nodes["Home".ToYamlNode()]).Children;
				map2.Add("City", (!node2.IsNull() && node2.ContainsKey("City")) ? node2["City".ToYamlNode()].ToString() : d_weatherhomecity);
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

			if(!nodes.IsNull() && nodes.ContainsKey("Api"))
			{
				var node2 = ((YamlMappingNode)nodes["Api".ToYamlNode()]).Children;
				map2.Add("Key", (!node2.IsNull() && node2.ContainsKey("Key")) ? node2["Key".ToYamlNode()].ToString() : d_wolframalphaapikey);
			}
			else
				map2.Add("Key", d_wolframalphaapikey);

			map.Add("Api", map2);
			return map;
		}
	}
}