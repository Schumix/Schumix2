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
using Schumix.WordPressRssAddon.Localization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Schumix.WordPressRssAddon.Config
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

			Log.Notice("WordPressRssAddonConfig", sLocalization.Config("Text"));

			var rssmap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey(new YamlScalarNode("WordPressRssAddon"))) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children[new YamlScalarNode("WordPressRssAddon")]).Children : (Dictionary<YamlNode, YamlNode>)null;
			RssMap((!rssmap.IsNull() && rssmap.ContainsKey(new YamlScalarNode("Rss"))) ? ((YamlMappingNode)rssmap[new YamlScalarNode("Rss")]).Children : (Dictionary<YamlNode, YamlNode>)null);

			Log.Success("WordPressRssAddonConfig", sLocalization.Config("Text2"));
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
				Log.Error("WordPressRssAddonConfig", sLocalization.Config("Text3"));
				Log.Debug("WordPressRssAddonConfig", sLocalization.Config("Text4"));
				var yaml = new YamlStream();
				string filename2 = sUtilities.DirectoryToHome(ConfigDirectory, "_" + ConfigFile);

				if(File.Exists(filename2))
					yaml.Load(File.OpenText(filename2));

				try
				{
					var rssmap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey(new YamlScalarNode("WordPressRssAddon"))) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children[new YamlScalarNode("WordPressRssAddon")]).Children : (Dictionary<YamlNode, YamlNode>)null;
					var nodes = new YamlMappingNode();
					var nodes2 = new YamlMappingNode();
					nodes2.Add("Rss", CreateRssMap((!rssmap.IsNull() && rssmap.ContainsKey(new YamlScalarNode("Rss"))) ? ((YamlMappingNode)rssmap[new YamlScalarNode("Rss")]).Children : (Dictionary<YamlNode, YamlNode>)null));
					nodes.Add("WordPressRssAddon", nodes2);

					sUtilities.CreateFile(filename);
					var file = new StreamWriter(filename, true) { AutoFlush = true };
					file.Write(ToString(nodes.Children));
					file.Close();

					if(File.Exists(filename2))
						File.Delete(filename2);

					Log.Success("WordPressRssAddonConfig", sLocalization.Config("Text5"));
				}
				catch(Exception e)
				{
					Log.Error("WordPressRssAddonConfig", sLocalization.Config("Text6"), e.Message);
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

			return "# WordPressRssAddon config file (yaml)\n" + text.ToString();
		}
	}
}