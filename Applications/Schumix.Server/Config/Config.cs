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
using System.Threading;
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Server.Config
{
	sealed class Config : DefaultConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private string _configfile;

		public Config(string configdir, string configfile, bool colorbindmode)
		{
			try
			{
				configdir = sUtilities.GetSpecialDirectory(configdir);
				_configfile = configfile;

				if(!IsConfig(configdir, configfile, colorbindmode))
				{
					if(!errors)
					{
						Log.Notice("Config", sLConsole.GetString("Program shutting down!"));
						Log.Notice("Config", sLConsole.GetString("Please set up the Config file!"));
					}

					Thread.Sleep(5*1000);
					Environment.Exit(1);
				}
				else
				{
					switch(ConfigType(configdir, _configfile))
					{
						case 0:
							new YamlConfig(configdir, _configfile, colorbindmode);
							break;
						case 1:
							new XmlConfig(configdir, _configfile, colorbindmode);
							break;
						default:
							new YamlConfig(configdir, _configfile, colorbindmode);
							break;
					}

					new ServerConfig(configdir, _configfile, colorbindmode);
					new Schumix.Framework.Config.SchumixConfig(configdir, _configfile, colorbindmode);
				}
			}
			catch(Exception e)
			{
				new Framework.Config.LogConfig(d_logfilename, d_logdatefilename, d_logmaxfilesize, 3, d_logdirectory, string.Empty, false);
				Log.Initialize(d_logfilename, colorbindmode);
				Log.Error("Config", sLConsole.GetString("Failure details: {0}"), e.Message);
			}
		}

		private int ConfigType(string ConfigDirectory, string ConfigFile)
		{
			if(ConfigFile == "Server.yml")
			{
				string filename = sUtilities.DirectoryToSpecial(ConfigDirectory, ConfigFile);
				string filename2 = sUtilities.DirectoryToSpecial(ConfigDirectory, "Server.xml");

				if(File.Exists(filename))
					return 0;
				else if(File.Exists(filename2))
				{
					_configfile = "Server.xml";
					return 1;
				}
			}
			else if(ConfigFile == "Server.xml")
			{
				string filename = sUtilities.DirectoryToSpecial(ConfigDirectory, ConfigFile);
				string filename2 = sUtilities.DirectoryToSpecial(ConfigDirectory, "Server.yml");

				if(File.Exists(filename))
					return 1;
				else if(File.Exists(filename2))
				{
					_configfile = "Server.yml";
					return 0;
				}
			}

			if(ConfigFile.EndsWith(".yml"))
				return 0;
			else if(ConfigFile.EndsWith(".xml"))
				return 1;

			return 0;
		}

		private bool IsConfig(string ConfigDirectory, string ConfigFile, bool ColorBindMode)
		{
			sUtilities.CreateDirectory(ConfigDirectory);

			switch(ConfigType(ConfigDirectory, ConfigFile))
			{
				case 0:
					return new YamlConfig().CreateConfig(ConfigDirectory, _configfile, ColorBindMode);
				case 1:
					return new XmlConfig().CreateConfig(ConfigDirectory, _configfile, ColorBindMode);
				default:
					return new YamlConfig().CreateConfig(ConfigDirectory, _configfile, ColorBindMode);
			}
		}
	}
}