/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2012-2013 Jackneill
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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Schumix.Server.Config
{
	sealed class YamlConfig : DefaultConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly New.Schumix sSchumix = Singleton<New.Schumix>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly Dictionary<YamlNode, YamlNode> NullYMap = null;

		public YamlConfig()
		{
		}

		public YamlConfig(string configdir, string configfile, bool colorbindmode)
		{
			var yaml = new YamlStream();
			yaml.Load(File.OpenText(sUtilities.DirectoryToSpecial(configdir, configfile)));

			var servermap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey("Server")) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children["Server".ToYamlNode()]).Children : NullYMap;
			LogMap((!servermap.IsNull() && servermap.ContainsKey("Log")) ? ((YamlMappingNode)servermap["Log".ToYamlNode()]).Children : NullYMap);
			Log.Initialize(Framework.Config.LogConfig.FileName, colorbindmode);
			Log.Debug("YamlConfig", ">> {0}", configfile);

			Log.Notice("YamlConfig", sLConsole.GetString("Config file is loading."));
			ServerMap((!servermap.IsNull() && servermap.ContainsKey("Server")) ? ((YamlMappingNode)servermap["Server".ToYamlNode()]).Children : NullYMap);
			CrashMap((!servermap.IsNull() && servermap.ContainsKey("Crash")) ? ((YamlMappingNode)servermap["Crash".ToYamlNode()]).Children : NullYMap);
			LocalizationMap((!servermap.IsNull() && servermap.ContainsKey("Localization")) ? ((YamlMappingNode)servermap["Localization".ToYamlNode()]).Children : NullYMap);
			UpdateMap((!servermap.IsNull() && servermap.ContainsKey("Update")) ? ((YamlMappingNode)servermap["Update".ToYamlNode()]).Children : NullYMap);
			ShutdownMap((!servermap.IsNull() && servermap.ContainsKey("Shutdown")) ? ((YamlMappingNode)servermap["Shutdown".ToYamlNode()]).Children : NullYMap);
			CleanMap((!servermap.IsNull() && servermap.ContainsKey("Clean")) ? ((YamlMappingNode)servermap["Clean".ToYamlNode()]).Children : NullYMap);
			SchumixsMap((!servermap.IsNull() && servermap.ContainsKey("Schumixs")) ? ((YamlMappingNode)servermap["Schumixs".ToYamlNode()]).Children : NullYMap);

			Log.Success("YamlConfig", sLConsole.GetString("Config database is loading."));
			Console.WriteLine();
		}

		~YamlConfig()
		{
		}

		public bool CreateConfig(string ConfigDirectory, string ConfigFile, bool ColorBindMode)
		{
			try
			{
				string filename = sUtilities.DirectoryToSpecial(ConfigDirectory, ConfigFile);

				if(File.Exists(filename))
					return true;
				else
				{
					new Framework.Config.LogConfig(d_logfilename, d_logdatefilename, d_logmaxfilesize, 3, d_logdirectory, string.Empty, false);
					Log.Initialize(d_logfilename, ColorBindMode);
					Log.Error("YamlConfig", sLConsole.GetString("No such config file!"));
					Log.Debug("YamlConfig", sLConsole.GetString("Preparing..."));
					var yaml = new YamlStream();
					string filename2 = sUtilities.DirectoryToSpecial(ConfigDirectory, "_" + ConfigFile);

					if(File.Exists(filename2))
						yaml.Load(File.OpenText(filename2));

					try
					{
						var servermap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey("Server")) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children["Server".ToYamlNode()]).Children : NullYMap;
						var nodes = new YamlMappingNode();
						var nodes2 = new YamlMappingNode();
						var nodes3 = new YamlMappingNode();

						nodes2.Add("Log",          CreateLogMap((!servermap.IsNull() && servermap.ContainsKey("Log")) ? ((YamlMappingNode)servermap["Log".ToYamlNode()]).Children : NullYMap));
						nodes2.Add("Server",       CreateServerMap((!servermap.IsNull() && servermap.ContainsKey("Server")) ? ((YamlMappingNode)servermap["Server".ToYamlNode()]).Children : NullYMap));
						nodes2.Add("Crash",        CreateCrashMap((!servermap.IsNull() && servermap.ContainsKey("Crash")) ? ((YamlMappingNode)servermap["Crash".ToYamlNode()]).Children : NullYMap));
						nodes2.Add("Localization", CreateLocalizationMap((!servermap.IsNull() && servermap.ContainsKey("Localization")) ? ((YamlMappingNode)servermap["Localization".ToYamlNode()]).Children : NullYMap));
						nodes2.Add("Update",       CreateUpdateMap((!servermap.IsNull() && servermap.ContainsKey("Update")) ? ((YamlMappingNode)servermap["Update".ToYamlNode()]).Children : NullYMap));
						nodes2.Add("Shutdown",     CreateShutdownMap((!servermap.IsNull() && servermap.ContainsKey("Shutdown")) ? ((YamlMappingNode)servermap["Shutdown".ToYamlNode()]).Children : NullYMap));
						nodes2.Add("Clean",        CreateCleanMap((!servermap.IsNull() && servermap.ContainsKey("Clean")) ? ((YamlMappingNode)servermap["Clean".ToYamlNode()]).Children : NullYMap));

						if((!servermap.IsNull() && servermap.ContainsKey("Schumixs")))
						{
							var n = ((YamlMappingNode)servermap["Schumixs".ToYamlNode()]).Children;
							nodes3.Add("Enabled", (!n.IsNull() && n.ContainsKey("Enabled")) ? n["Enabled".ToYamlNode()].ToString() : d_schumixsenabled.ToString());

							foreach(var schumixmap in n)
							{
								if(schumixmap.Key.ToString().Contains("Schumix"))
									nodes3.Add(schumixmap.Key, CreateSchumixsMap(((YamlMappingNode)schumixmap.Value).Children));
							}

							nodes2.Add("Schumixs", nodes3);
						}
						else
						{
							nodes3.Add("Enabled", d_schumixsenabled.ToString());
							nodes3.Add("Schumix", CreateSchumixsMap(NullYMap));
							nodes2.Add("Schumixs", nodes3);
						}

						nodes.Add("Server", nodes2);

						sUtilities.CreateFile(filename);
						var file = new StreamWriter(filename, true) { AutoFlush = true };
						file.Write(nodes.Children.ToString("Server"));
						file.Close();

						if(File.Exists(filename2))
							File.Delete(filename2);

						Log.Success("YamlConfig", sLConsole.GetString("Config file is completed!"));
					}
					catch(Exception e)
					{
						Log.Error("YamlConfig", sLConsole.GetString("Failure was handled during the yml writing. Details: {0}"), e.Message);
						errors = true;
					}
				}
			}
			catch(DirectoryNotFoundException)
			{
				CreateConfig(ConfigDirectory, ConfigFile, ColorBindMode);
			}

			return false;
		}

		private void LogMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			string LogFileName = (!nodes.IsNull() && nodes.ContainsKey("FileName")) ? nodes["FileName".ToYamlNode()].ToString() : d_logfilename;
			bool LogDateFileName = (!nodes.IsNull() && nodes.ContainsKey("DateFileName")) ? Convert.ToBoolean(nodes["DateFileName".ToYamlNode()].ToString()) : d_logdatefilename;
			int LogMaxFileSize = (!nodes.IsNull() && nodes.ContainsKey("MaxFileSize")) ? Convert.ToInt32(nodes["MaxFileSize".ToYamlNode()].ToString()) : d_logmaxfilesize;
			int LogLevel = (!nodes.IsNull() && nodes.ContainsKey("LogLevel")) ? Convert.ToInt32(nodes["LogLevel".ToYamlNode()].ToString()) : d_loglevel;
			string LogDirectory = (!nodes.IsNull() && nodes.ContainsKey("LogDirectory")) ? nodes["LogDirectory".ToYamlNode()].ToString() : d_logdirectory;

			new Framework.Config.LogConfig(LogFileName, LogDateFileName, LogMaxFileSize, LogLevel, sUtilities.GetSpecialDirectory(LogDirectory), string.Empty, false);
		}

		private void ServerMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			int ListenerPort = d_listenerport;

			if(!nodes.IsNull() && nodes.ContainsKey("Listener"))
			{
				var node = ((YamlMappingNode)nodes["Listener".ToYamlNode()]).Children;
				ListenerPort = (!node.IsNull() && node.ContainsKey("Port")) ? Convert.ToInt32(node["Port".ToYamlNode()].ToString()) : d_listenerport;
			}

			string Password = (!nodes.IsNull() && nodes.ContainsKey("Password")) ? nodes["Password".ToYamlNode()].ToString() : d_password;

			new ServerConfigs(ListenerPort, Password);
		}

		private void CrashMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			string Directory = (!nodes.IsNull() && nodes.ContainsKey("Directory")) ? nodes["Directory".ToYamlNode()].ToString() : d_crashdirectory;

			new Framework.Config.CrashConfig(sUtilities.GetSpecialDirectory(Directory));
		}

		private void LocalizationMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			string Locale = (!nodes.IsNull() && nodes.ContainsKey("Locale")) ? nodes["Locale".ToYamlNode()].ToString() : d_locale;

			new LocalizationConfig(Locale);
		}

		private void UpdateMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool Enabled = (!nodes.IsNull() && nodes.ContainsKey("Enabled")) ? Convert.ToBoolean(nodes["Enabled".ToYamlNode()].ToString()) : d_updateenabled;
			string Version = (!nodes.IsNull() && nodes.ContainsKey("Version")) ? nodes["Version".ToYamlNode()].ToString() : d_updateversion;
			string Branch = (!nodes.IsNull() && nodes.ContainsKey("Branch")) ? nodes["Branch".ToYamlNode()].ToString() : d_updatebranch;
			string WebPage = (!nodes.IsNull() && nodes.ContainsKey("WebPage")) ? nodes["WebPage".ToYamlNode()].ToString() : d_updatewebpage;

			new Framework.Config.UpdateConfig(Enabled, Version.ToLower(), Branch, WebPage);
		}

		private void ShutdownMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			int MaxMemory = (!nodes.IsNull() && nodes.ContainsKey("MaxMemory")) ? Convert.ToInt32(nodes["MaxMemory".ToYamlNode()].ToString()) : d_shutdownmaxmemory;

			new Framework.Config.ShutdownConfig(MaxMemory);
		}

		private void CleanMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool Config = (!nodes.IsNull() && nodes.ContainsKey("Config")) ? Convert.ToBoolean(nodes["Config".ToYamlNode()].ToString()) : d_cleanconfig;
			bool Database = (!nodes.IsNull() && nodes.ContainsKey("Database")) ? Convert.ToBoolean(nodes["Database".ToYamlNode()].ToString()) : d_cleandatabase;
			new Framework.Config.CleanConfig(Config, Database);
		}

		private void SchumixsMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			if((!nodes.IsNull() && nodes.ContainsKey("Enabled")) ? Convert.ToBoolean(nodes["Enabled".ToYamlNode()].ToString()) : d_schumixsenabled)
			{
				Task.Factory.StartNew(() =>
				{
					Log.Notice("Schumix", sLConsole.GetString("Schumixs starting..."));
					Log.Notice("Schumix", sLConsole.GetString("Schumixs number: {0}"), nodes.Count-1);

					foreach(var maps in nodes)
					{
						if(maps.Key.ToString().Contains("Schumix"))
						{
							var node = ((YamlMappingNode)maps.Value).Children;

							if(!node.IsNull() && node.ContainsKey("Config"))
							{
								var node2 = ((YamlMappingNode)node["Config".ToYamlNode()]).Children;
								string file = (!node2.IsNull() && node2.ContainsKey("File")) ? node2["File".ToYamlNode()].ToString() : d_schumixfile;
								string dir = (!node2.IsNull() && node2.ContainsKey("Directory")) ? node2["Directory".ToYamlNode()].ToString() : d_schumixdirectory;
								string ce = (!node2.IsNull() && node2.ContainsKey("ConsoleEncoding")) ? node2["ConsoleEncoding".ToYamlNode()].ToString() : d_schumixconsoleencoding;
								string lo = (!node2.IsNull() && node2.ContainsKey("Locale")) ? node2["Locale".ToYamlNode()].ToString() : d_schumixlocale;
								sSchumix.Start(file, dir, ce, lo, sUtilities.GetRandomString());
								Thread.Sleep(10*1000);
							}
							else
								Log.Warning("Schumix", sLConsole.GetString("There is no load of Schumix!"));
						}
					}
				});
			}
			else
				Log.Warning("Schumix", sLConsole.GetString("There is no load of Schumix!"));
		}

		private YamlMappingNode CreateLogMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("FileName",        (!nodes.IsNull() && nodes.ContainsKey("FileName")) ? nodes["FileName".ToYamlNode()].ToString() : d_logfilename);
			map.Add("DateFileName",    (!nodes.IsNull() && nodes.ContainsKey("DateFileName")) ? nodes["DateFileName".ToYamlNode()].ToString() : d_logdatefilename.ToString());
			map.Add("MaxFileSize",     (!nodes.IsNull() && nodes.ContainsKey("MaxFileSize")) ? nodes["MaxFileSize".ToYamlNode()].ToString() : d_logmaxfilesize.ToString());
			map.Add("LogLevel",        (!nodes.IsNull() && nodes.ContainsKey("LogLevel")) ? nodes["LogLevel".ToYamlNode()].ToString() : d_loglevel.ToString());
			map.Add("LogDirectory",    (!nodes.IsNull() && nodes.ContainsKey("LogDirectory")) ? nodes["LogDirectory".ToYamlNode()].ToString() : d_logdirectory);
			return map;
		}

		private YamlMappingNode CreateServerMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			var map2 = new YamlMappingNode();

			if(!nodes.IsNull() && nodes.ContainsKey("Listener"))
			{
				var node = ((YamlMappingNode)nodes["Listener".ToYamlNode()]).Children;
				map2.Add("Port", ((!node.IsNull() && node.ContainsKey("Port")) ? node["Port".ToYamlNode()].ToString() : d_listenerport.ToString()));
			}
			else
				map2.Add("Port", d_listenerport.ToString());

			map.Add("Listener", map2);
			map.Add("Password", (!nodes.IsNull() && nodes.ContainsKey("Password")) ? nodes["Password".ToYamlNode()].ToString() : d_password);
			return map;
		}

		private YamlMappingNode CreateCrashMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Directory", (!nodes.IsNull() && nodes.ContainsKey("Directory")) ? nodes["Directory".ToYamlNode()].ToString() : d_crashdirectory);
			return map;
		}

		private YamlMappingNode CreateLocalizationMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Locale", (!nodes.IsNull() && nodes.ContainsKey("Locale")) ? nodes["Locale".ToYamlNode()].ToString() : d_locale);
			return map;
		}

		private YamlMappingNode CreateUpdateMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Enabled", (!nodes.IsNull() && nodes.ContainsKey("Enabled")) ? nodes["Enabled".ToYamlNode()].ToString() : d_updateenabled.ToString());
			map.Add("Version", (!nodes.IsNull() && nodes.ContainsKey("Version")) ? nodes["Version".ToYamlNode()].ToString() : d_updateversion);
			map.Add("Branch",  (!nodes.IsNull() && nodes.ContainsKey("Branch")) ? nodes["Branch".ToYamlNode()].ToString() : d_updatebranch);
			map.Add("WebPage", (!nodes.IsNull() && nodes.ContainsKey("WebPage")) ? nodes["WebPage".ToYamlNode()].ToString() : d_updatewebpage);
			return map;
		}

		private YamlMappingNode CreateShutdownMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("MaxMemory", (!nodes.IsNull() && nodes.ContainsKey("MaxMemory")) ? nodes["MaxMemory".ToYamlNode()].ToString() : d_shutdownmaxmemory.ToString());
			return map;
		}

		private YamlMappingNode CreateCleanMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();
			map.Add("Config",   (!nodes.IsNull() && nodes.ContainsKey("Config")) ? nodes["Config".ToYamlNode()].ToString() : d_cleanconfig.ToString());
			map.Add("Database", (!nodes.IsNull() && nodes.ContainsKey("Database")) ? nodes["Database".ToYamlNode()].ToString() : d_cleandatabase.ToString());
			return map;
		}

		private YamlMappingNode CreateSchumixsMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new YamlMappingNode();

			if(nodes.IsNull())
			{
				var map2 = new YamlMappingNode();
				map2.Add("File",            d_schumixfile);
				map2.Add("Directory",       d_schumixdirectory);
				map2.Add("ConsoleEncoding", d_schumixconsoleencoding);
				map2.Add("Locale",          d_schumixlocale);
				map.Add("Config",           map2);
			}
			else
			{
				var map2 = new YamlMappingNode();

				if(!nodes.IsNull() && nodes.ContainsKey("Config"))
				{
					var node2 = ((YamlMappingNode)nodes["Config".ToYamlNode()]).Children;
					map2.Add("File",            (!node2.IsNull() && node2.ContainsKey("File")) ? node2["File".ToYamlNode()].ToString() : d_schumixfile);
					map2.Add("Directory",       (!node2.IsNull() && node2.ContainsKey("Directory")) ? node2["Directory".ToYamlNode()].ToString() : d_schumixdirectory);
					map2.Add("ConsoleEncoding", (!node2.IsNull() && node2.ContainsKey("ConsoleEncoding")) ? node2["ConsoleEncoding".ToYamlNode()].ToString() : d_schumixconsoleencoding);
					map2.Add("Locale",          (!node2.IsNull() && node2.ContainsKey("Locale")) ? node2["Locale".ToYamlNode()].ToString() : d_schumixlocale);
				}
				else
				{
					map2.Add("File",            d_schumixfile);
					map2.Add("Directory",       d_schumixdirectory);
					map2.Add("ConsoleEncoding", d_schumixconsoleencoding);
					map2.Add("Locale",          d_schumixlocale);
				}

				map.Add("Config", map2);
			}

			return map;
		}
	}
}