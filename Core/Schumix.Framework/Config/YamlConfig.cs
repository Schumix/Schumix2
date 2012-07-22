/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
 * Copyright (C) 2012 Jackneill
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
using System.Collections.Generic;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using YamlDotNet.Core;
using YamlDotNet;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Converters;
/*using System.Yaml;
using System.Yaml.Serialization;*/
using System.Collections;
using System.IO;
using System.Text;

namespace Schumix.Framework.Config
{
	public sealed class YamlConfig : DefaultConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		public YamlConfig()
		{
		}

		public YamlConfig(string configdir, string configfile)
		{
			// TODO

            var strdoc = string.Empty;

            // Setup the input
            var input = new StringReader(strdoc);

            var yaml = new YamlStream();
            yaml.Load(input);
            
            var mapping =
                (YamlMappingNode)yaml.Documents[0].RootNode;

            //foreach (var entry in mapping.Children)
            //{
                
            //}

            var items = (YamlSequenceNode)mapping.Children[new YamlScalarNode("Irc")];
            foreach (YamlMappingNode item in items)
            {
                string ServerName = item.Children[new YamlScalarNode("ServerName")].ToString();
				Console.WriteLine(ServerName);

            }
		}

		~YamlConfig()
		{
		}

		public bool CreateConfig(string configdir, string configfile)
		{
			// TODO
			return true;
			//return false;
		}
	}
}
