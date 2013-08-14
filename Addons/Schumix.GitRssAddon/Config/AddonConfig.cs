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
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.GitRssAddon.Localization;

namespace Schumix.GitRssAddon.Config
{
	sealed class AddonConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private string _configfiledefaultname;
		private string _configfile;

		public AddonConfig(string FileName, string Data)
		{
			try
			{
				_configfile = FileName + Data;
				_configfiledefaultname = FileName;

				Log.Debug("GitRssAddonConfig", ">> {0}", _configfile);

				if(!IsConfig(SchumixConfig.ConfigDirectory, _configfile))
					Init(SchumixConfig.ConfigDirectory, _configfile);
				else
					Init(SchumixConfig.ConfigDirectory, _configfile);
			}
			catch(Exception e)
			{
				Log.Error("GitRssAddonConfig", sLConsole.GetString("Failure details: {0}"), e.Message);
			}
		}

		private void Init(string configdir, string configfile)
		{
			switch(ConfigType(configdir, configfile))
			{
				case 0:
					new AddonYamlConfig(configdir, configfile);
					break;
				case 1:
					new AddonXmlConfig(configdir, configfile);
					break;
				default:
					new AddonYamlConfig(configdir, configfile);
					break;
			}
		}

		private int ConfigType(string ConfigDirectory, string ConfigFile)
		{
			if(ConfigFile == _configfiledefaultname + ".yml")
			{
				string filename = sUtilities.DirectoryToSpecial(ConfigDirectory, ConfigFile);
				string filename2 = sUtilities.DirectoryToSpecial(ConfigDirectory, _configfiledefaultname + ".xml");

				if(File.Exists(filename))
					return 0;
				else if(File.Exists(filename2))
				{
					_configfile = _configfiledefaultname + ".xml";
					return 1;
				}
			}
			else if(ConfigFile == _configfiledefaultname + ".xml")
			{
				string filename = sUtilities.DirectoryToSpecial(ConfigDirectory, ConfigFile);
				string filename2 = sUtilities.DirectoryToSpecial(ConfigDirectory, _configfiledefaultname + ".yml");

				if(File.Exists(filename))
					return 1;
				else if(File.Exists(filename2))
				{
					_configfile = _configfiledefaultname + ".yml";
					return 0;
				}
			}

			if(ConfigFile.EndsWith(".yml"))
				return 0;
			else if(ConfigFile.EndsWith(".xml"))
				return 1;

			return 0;
		}

		private bool IsConfig(string ConfigDirectory, string ConfigFile)
		{
			sUtilities.CreateDirectory(ConfigDirectory);

			switch(ConfigType(ConfigDirectory, ConfigFile))
			{
				case 0:
					return new AddonYamlConfig().CreateConfig(ConfigDirectory, _configfile);
				case 1:
					return new AddonXmlConfig().CreateConfig(ConfigDirectory, _configfile);
				default:
					return new AddonYamlConfig().CreateConfig(ConfigDirectory, _configfile);
			}
		}
	}
}