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
using System.Text;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using Schumix.Framework.Platforms;

namespace Schumix.Framework.Extensions
{
	public static class YamlExtensions
	{
		private static readonly Platform sPlatform = Singleton<Platform>.Instance;
		public static readonly Dictionary<YamlNode, YamlNode> NullYMap = null;

		public static string ToString(this IDictionary<YamlNode, YamlNode> Nodes, string FileName = "")
		{
			var text = new StringBuilder();

			foreach(var child in Nodes)
			{
				if(((YamlMappingNode)child.Value).GetType() == typeof(YamlMappingNode))
					text.Append(child.Key).Append(":\n").Append(child.Value);
				else
					text.Append("    ").Append(child.Key).Append(": ").Append(child.Value);
			}

			if(sPlatform.IsWindows)
				text = text.Replace("\r", string.Empty);

			return FileName.IsNullOrEmpty() ? "# Schumix config file (yaml)\n" + text.ToString() : "# " + FileName + " config file (yaml)\n" + text.ToString();
		}

		public static bool ContainsKey(this IDictionary<YamlNode, YamlNode> Nodes, string Key)
		{
			return Nodes.ContainsKey(new YamlScalarNode(Key));
		}

		public static YamlScalarNode ToYamlNode(this string Text)
		{
			return new YamlScalarNode(Text);
		}

		public static IDictionary<YamlNode, YamlNode> GetYamlChildren(this IDictionary<YamlNode, YamlNode> Nodes, string Key)
		{
			return (!Nodes.IsNull() && Nodes.ContainsKey(Key)) ? ((YamlMappingNode)Nodes[Key.ToYamlNode()]).Children : NullYMap;
		}
	}
}