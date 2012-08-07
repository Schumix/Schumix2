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
using Schumix.CalendarAddon.Localization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Schumix.CalendarAddon.Config
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
			yaml.Load(File.OpenText(sUtilities.DirectoryToSpecial(configdir, configfile)));

			Log.Notice("CalendarAddonConfig", sLocalization.Config("Text"));

			var calendarmap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey(new YamlScalarNode("CalendarAddon"))) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children[new YamlScalarNode("CalendarAddon")]).Children : (Dictionary<YamlNode, YamlNode>)null;
			FloodingMap((!calendarmap.IsNull() && calendarmap.ContainsKey(new YamlScalarNode("Flooding"))) ? ((YamlMappingNode)calendarmap[new YamlScalarNode("Flooding")]).Children : (Dictionary<YamlNode, YamlNode>)null);

			Log.Success("CalendarAddonConfig", sLocalization.Config("Text2"));
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
				Log.Error("CalendarAddonConfig", sLocalization.Config("Text3"));
				Log.Debug("CalendarAddonConfig", sLocalization.Config("Text4"));
				var yaml = new YamlStream();
				string filename2 = sUtilities.DirectoryToSpecial(ConfigDirectory, "_" + ConfigFile);

				if(File.Exists(filename2))
					yaml.Load(File.OpenText(filename2));

				try
				{
					var calendarmap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey(new YamlScalarNode("CalendarAddon"))) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children[new YamlScalarNode("CalendarAddon")]).Children : (Dictionary<YamlNode, YamlNode>)null;
					var nodes = new YamlMappingNode();
					var nodes2 = new YamlMappingNode();
					nodes2.Add("Flooding", CreateFloodingMap((!calendarmap.IsNull() && calendarmap.ContainsKey(new YamlScalarNode("Flooding"))) ? ((YamlMappingNode)calendarmap[new YamlScalarNode("Flooding")]).Children : (Dictionary<YamlNode, YamlNode>)null));
					nodes.Add("CalendarAddon", nodes2);

					sUtilities.CreateFile(filename);
					var file = new StreamWriter(filename, true) { AutoFlush = true };
					file.Write(nodes.Children.ToString("CalendarAddon"));
					file.Close();

					if(File.Exists(filename2))
						File.Delete(filename2);

					Log.Success("CalendarAddonConfig", sLocalization.Config("Text5"));
				}
				catch(Exception e)
				{
					Log.Error("CalendarAddonConfig", sLocalization.Config("Text6"), e.Message);
				}
			}

			return false;
		}

		private void FloodingMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			int Seconds = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Seconds"))) ? Convert.ToInt32(nodes[new YamlScalarNode("Seconds")].ToString()) : _seconds;
			int NumberOfMessages = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("NumberOfMessages"))) ? Convert.ToInt32(nodes[new YamlScalarNode("NumberOfMessages")].ToString()) : _numberofmessages;
			int NumberOfFlooding = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("NumberOfFlooding"))) ? Convert.ToInt32(nodes[new YamlScalarNode("NumberOfFlooding")].ToString()) : _numberofflooding;
			new CalendarConfig(Seconds, NumberOfMessages, NumberOfFlooding);
		}

		private YamlMappingNode CreateFloodingMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Seconds",          (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Seconds"))) ? nodes[new YamlScalarNode("Seconds")].ToString() : _seconds.ToString());
			map.Add("NumberOfMessages", (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("NumberOfMessages"))) ? nodes[new YamlScalarNode("NumberOfMessages")].ToString() : _numberofmessages.ToString());
			map.Add("NumberOfFlooding", (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("NumberOfFlooding"))) ? nodes[new YamlScalarNode("NumberOfFlooding")].ToString() : _numberofflooding.ToString());
			return map;
		}
	}
}