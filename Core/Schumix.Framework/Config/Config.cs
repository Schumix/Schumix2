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
using System.Xml;
using System.Threading;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Config
{
	public sealed class Config : DefaultConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
        private readonly XMLConfig XmlConf;
        private readonly YAMLConfig YamlConf;

		public Config(string configdir, string configfile)
		{
			try
			{
				new SchumixConfig(configdir, configfile);

				if(!IsConfig(configdir, configfile))
				{
					if(!error)
					{
						Log.Notice("Config", sLConsole.Config("Text"));
						Log.Notice("Config", sLConsole.Config("Text2"));
					}

					Thread.Sleep(5*1000);
					Environment.Exit(1);
				}
				else
				{
                    switch (ConfigType(configfile))
                    {
                        case 0:
                            YamlConf = new YAMLConfig(configdir, configfile);
                            break;
                        case 1:
                            XmlConf = new XMLConfig(configdir, configfile);
                            break;
                        default:
                            YamlConf = new YAMLConfig(configdir, configfile);
                            break;
                    }
				}
			}
			catch(Exception e)
			{
				new LogConfig(_logfilename, 3, _logdirectory, _irclogdirectory, _irclog);
				Log.Error("Config", sLConsole.Exception("Error"), e.Message);
			}
		}

        private int ConfigType(string ConfigFile)
        {
            if (ConfigFile.EndsWith(".yml"))
                return 0;
            else if (ConfigFile.EndsWith(".xml"))
                return 1;
            else return 0;
        }

        private void CheckAndCreate(string ConfigDirectory)
        {
            if (!Directory.Exists(ConfigDirectory))
                Directory.CreateDirectory(ConfigDirectory);
        }

        private bool IsConfig(string ConfigDirectory, string ConfigFile)
        {
            CheckAndCreate(ConfigDirectory);

            switch (ConfigType(ConfigFile))
            {
                case 0:
                    return YamlConf.CreateConfig(ConfigDirectory, ConfigFile);
                case 1:
                    return XmlConf.CreateConfig(ConfigDirectory, ConfigFile);
                default:
                    return YamlConf.CreateConfig(ConfigDirectory, ConfigFile);
            }
		}
	}
}