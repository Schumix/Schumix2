/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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

namespace Schumix.CalendarAddon.Config
{
	sealed class AddonYamlConfig : AddonDefaultConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		public AddonYamlConfig()
		{
		}

		public AddonYamlConfig(string configdir, string configfile)
		{
			var yaml = new YamlStream();
			yaml.Load(File.OpenText(Path.Combine(configdir, configfile)));

			Log.Notice("CalendarAddonConfig", sLConsole.GetString("Config file is loading."));

			var calendarmap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey("CalendarAddon")) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children["CalendarAddon".ToYamlNode()]).Children : YamlExtensions.NullYMap;
			FloodingMap(calendarmap.GetYamlChildren("Flooding"));

			Log.Success("CalendarAddonConfig", sLConsole.GetString("Config database is loading."));
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
				Log.Error("CalendarAddonConfig", sLConsole.GetString("No such config file!"));
				Log.Debug("CalendarAddonConfig", sLConsole.GetString("Preparing..."));
				var yaml = new YamlStream();
				string filename2 = Path.Combine(ConfigDirectory, "_" + ConfigFile);

				if(File.Exists(filename2))
				{
					Log.Notice("CalendarAddonConfig", sLConsole.GetString("The backup files will be used to renew the data."));
					yaml.Load(File.OpenText(filename2));
				}

				try
				{
					var calendarmap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey("CalendarAddon")) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children["CalendarAddon".ToYamlNode()]).Children : YamlExtensions.NullYMap;
					var nodes = new YamlMappingNode();
					var nodes2 = new YamlMappingNode();
					nodes2.Add("Flooding", CreateFloodingMap(calendarmap.GetYamlChildren("Flooding")));
					nodes.Add("CalendarAddon", nodes2);

					sUtilities.CreateFile(filename);
					var file = new StreamWriter(filename, true) { AutoFlush = true };
					file.Write(nodes.Children.ToString("CalendarAddon"));
					file.Close();

					if(File.Exists(filename2))
					{
						Log.Notice("CalendarAddonConfig", sLConsole.GetString("The backup has been deleted during the re-use."));
						File.Delete(filename2);
					}

					Log.Success("CalendarAddonConfig", sLConsole.GetString("Config file is completed!"));
				}
				catch(Exception e)
				{
					Log.Error("CalendarAddonConfig", sLConsole.GetString("Failure was handled during the xml writing. Details: {0}"), e.Message);
				}
			}

			return false;
		}

		private void FloodingMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			int Seconds = (!nodes.IsNull() && nodes.ContainsKey("Seconds")) ? nodes["Seconds".ToYamlNode()].ToString().ToInt32() : _seconds;
			int NumberOfMessages = (!nodes.IsNull() && nodes.ContainsKey("NumberOfMessages")) ? nodes["NumberOfMessages".ToYamlNode()].ToString().ToInt32() : _numberofmessages;
			int NumberOfFlooding = (!nodes.IsNull() && nodes.ContainsKey("NumberOfFlooding")) ? nodes["NumberOfFlooding".ToYamlNode()].ToString().ToInt32() : _numberofflooding;
			new CalendarConfig(Seconds, NumberOfMessages, NumberOfFlooding);
		}

		private YamlMappingNode CreateFloodingMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Seconds",          (!nodes.IsNull() && nodes.ContainsKey("Seconds")) ? nodes["Seconds".ToYamlNode()].ToString() : _seconds.ToString());
			map.Add("NumberOfMessages", (!nodes.IsNull() && nodes.ContainsKey("NumberOfMessages")) ? nodes["NumberOfMessages".ToYamlNode()].ToString() : _numberofmessages.ToString());
			map.Add("NumberOfFlooding", (!nodes.IsNull() && nodes.ContainsKey("NumberOfFlooding")) ? nodes["NumberOfFlooding".ToYamlNode()].ToString() : _numberofflooding.ToString());
			return map;
		}
	}
}