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
using Schumix.GitRssAddon.Localization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Schumix.GitRssAddon.Config
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

			Log.Notice("GitRssAddonConfig", sLocalization.Config("Text"));

			var rssmap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey(new YamlScalarNode("GitRssAddon"))) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children[new YamlScalarNode("GitRssAddon")]).Children : (Dictionary<YamlNode, YamlNode>)null;
			RssMap((!rssmap.IsNull() && rssmap.ContainsKey(new YamlScalarNode("Rss"))) ? ((YamlMappingNode)rssmap[new YamlScalarNode("Rss")]).Children : (Dictionary<YamlNode, YamlNode>)null);

			Log.Success("GitRssAddonConfig", sLocalization.Config("Text2"));
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
				Log.Error("GitRssAddonConfig", sLocalization.Config("Text3"));
				Log.Debug("GitRssAddonConfig", sLocalization.Config("Text4"));
				var yaml = new YamlStream();
				string filename2 = sUtilities.DirectoryToHome(ConfigDirectory, "_" + ConfigFile);

				if(File.Exists(filename2))
					yaml.Load(File.OpenText(filename2));

				try
				{
					var rssmap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey(new YamlScalarNode("GitRssAddon"))) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children[new YamlScalarNode("GitRssAddon")]).Children : (Dictionary<YamlNode, YamlNode>)null;
					var nodes = new YamlMappingNode();
					var nodes2 = new YamlMappingNode();
					nodes2.Add("Rss", CreateRssMap((!rssmap.IsNull() && rssmap.ContainsKey(new YamlScalarNode("Rss"))) ? ((YamlMappingNode)rssmap[new YamlScalarNode("Rss")]).Children : (Dictionary<YamlNode, YamlNode>)null));
					nodes.Add("GitRssAddon", nodes2);

					sUtilities.CreateFile(filename);
					var file = new StreamWriter(filename, true) { AutoFlush = true };
					file.Write(nodes.Children.ToString("GitRssAddon"));
					file.Close();

					if(File.Exists(filename2))
						File.Delete(filename2);

					Log.Success("GitRssAddonConfig", sLocalization.Config("Text5"));
				}
				catch(Exception e)
				{
					Log.Error("GitRssAddonConfig", sLocalization.Config("Text6"), e.Message);
				}
			}

			return false;
		}

		private void RssMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			int QueryTime = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("QueryTime"))) ? Convert.ToInt32(nodes[new YamlScalarNode("QueryTime")].ToString()) : d_querytime;
			new RssConfig(QueryTime);
		}

		private YamlMappingNode CreateRssMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("QueryTime", (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("QueryTime"))) ? nodes[new YamlScalarNode("QueryTime")].ToString() : d_querytime.ToString());
			return map;
		}
	}
}