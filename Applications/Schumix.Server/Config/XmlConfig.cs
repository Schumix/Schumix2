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
using System.Xml;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Server.Config
{
	sealed class XmlConfig : DefaultConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly New.Schumix sSchumix = Singleton<New.Schumix>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		public XmlConfig()
		{
		}

		public XmlConfig(string configdir, string configfile, bool colorbindmode)
		{
			var xmldoc = new XmlDocument();
			xmldoc.Load(sUtilities.DirectoryToSpecial(configdir, configfile));

			string LogFileName = !xmldoc.SelectSingleNode("Server/Log/FileName").IsNull() ? xmldoc.SelectSingleNode("Server/Log/FileName").InnerText : d_logfilename;
			bool LogDateFileName = !xmldoc.SelectSingleNode("Server/Log/DateFileName").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Server/Log/DateFileName").InnerText) : d_logdatefilename;
			int LogMaxFileSize = !xmldoc.SelectSingleNode("Server/Log/MaxFileSize").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Server/Log/MaxFileSize").InnerText) : d_logmaxfilesize;
			int LogLevel = !xmldoc.SelectSingleNode("Server/Log/LogLevel").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Server/Log/LogLevel").InnerText) : d_loglevel;
			string LogDirectory = !xmldoc.SelectSingleNode("Server/Log/LogDirectory").IsNull() ? xmldoc.SelectSingleNode("Server/Log/LogDirectory").InnerText : d_logdirectory;

			new Framework.Config.LogConfig(LogFileName, LogDateFileName, LogMaxFileSize, LogLevel, sUtilities.GetSpecialDirectory(LogDirectory), string.Empty, false);

			Log.Initialize(LogFileName, colorbindmode);
			Log.Debug("XmlConfig", ">> {0}", configfile);

			Log.Notice("XmlConfig", sLConsole.GetString("Config file is loading."));
			int ListenerPort = !xmldoc.SelectSingleNode("Server/Server/Listener/Port").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Server/Server/Listener/Port").InnerText) : d_listenerport;
			string Password = !xmldoc.SelectSingleNode("Server/Server/Password").IsNull() ? xmldoc.SelectSingleNode("Server/Server/Password").InnerText : d_password;

			new ServerConfigs(ListenerPort, Password);

			string Directory = !xmldoc.SelectSingleNode("Server/Crash/Directory").IsNull() ? xmldoc.SelectSingleNode("Server/Crash/Directory").InnerText : d_crashdirectory;

			new Framework.Config.CrashConfig(sUtilities.GetSpecialDirectory(Directory));

			string Locale = !xmldoc.SelectSingleNode("Server/Localization/Locale").IsNull() ? xmldoc.SelectSingleNode("Server/Localization/Locale").InnerText : d_locale;

			new LocalizationConfig(Locale);

			bool Enabled = !xmldoc.SelectSingleNode("Server/Update/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Server/Update/Enabled").InnerText) : d_updateenabled;
			string Version = !xmldoc.SelectSingleNode("Server/Update/Version").IsNull() ? xmldoc.SelectSingleNode("Server/Update/Version").InnerText : d_updateversion;
			string Branch = !xmldoc.SelectSingleNode("Server/Update/Branch").IsNull() ? xmldoc.SelectSingleNode("Server/Update/Branch").InnerText : d_updatebranch;
			string WebPage = !xmldoc.SelectSingleNode("Server/Update/WebPage").IsNull() ? xmldoc.SelectSingleNode("Server/Update/WebPage").InnerText : d_updatewebpage;

			new Framework.Config.UpdateConfig(Enabled, Version.ToLower(), Branch, WebPage);

			int MaxMemory = !xmldoc.SelectSingleNode("Server/Shutdown/MaxMemory").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Server/Shutdown/MaxMemory").InnerText) : d_shutdownmaxmemory;

			new Framework.Config.ShutdownConfig(MaxMemory);

			bool Config = !xmldoc.SelectSingleNode("Server/Clean/Config").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Server/Clean/Config").InnerText) : d_cleanconfig;
			bool Database = !xmldoc.SelectSingleNode("Server/Clean/Database").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Server/Clean/Database").InnerText) : d_cleandatabase;

			new Framework.Config.CleanConfig(Config, Database);

			if(!xmldoc.SelectSingleNode("Server/Schumixs/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Server/Schumixs/Enabled").InnerText) : d_schumixsenabled)
			{
				Task.Factory.StartNew(() =>
				{
					Log.Notice("Schumix", sLConsole.GetString("Schumixs starting..."));

					var xmlschumixlist = xmldoc.SelectNodes("Server/Schumixs/Schumix");
					Log.Notice("Schumix", sLConsole.GetString("Schumixs number: {0}"), xmlschumixlist.Count);

					if(xmlschumixlist.Count == 0)
						Log.Warning("Schumix", sLConsole.GetString("There is no load of Schumix!"));
					else
					{
						foreach(XmlNode xn in xmlschumixlist)
						{
							string file = !xn.SelectSingleNode("Config/File").IsNull() ? xn.SelectSingleNode("Config/File").InnerText : d_schumixfile;
							string dir = !xn.SelectSingleNode("Config/Directory").IsNull() ? xn.SelectSingleNode("Config/Directory").InnerText : d_schumixdirectory;
							string ce = !xn.SelectSingleNode("Config/ConsoleEncoding").IsNull() ? xn.SelectSingleNode("Config/ConsoleEncoding").InnerText : d_schumixconsoleencoding;
							string lo = !xn.SelectSingleNode("Config/Locale").IsNull() ? xn.SelectSingleNode("Config/Locale").InnerText : d_schumixlocale;
							sSchumix.Start(file, dir, ce, lo, sUtilities.GetRandomString());
							Thread.Sleep(10*1000);
						}
					}
				});
			}
			else
				Log.Warning("Schumix", sLConsole.GetString("There is no load of Schumix!"));

			Log.Success("XmlConfig", sLConsole.GetString("Config database is loading."));
			Console.WriteLine();
		}

		~XmlConfig()
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
					Log.Error("XmlConfig", sLConsole.GetString("No such config file!"));
					Log.Debug("XmlConfig", sLConsole.GetString("Preparing..."));
					var w = new XmlTextWriter(filename, null);
					var xmldoc = new XmlDocument();
					string filename2 = sUtilities.DirectoryToSpecial(ConfigDirectory, "_" + ConfigFile);

					if(File.Exists(filename2))
					{
						Log.Notice("XmlConfig", sLConsole.GetString("The backup files will be used to renew the data."));
						xmldoc.Load(filename2);
					}

					try
					{
						w.Formatting = Formatting.Indented;
						w.Indentation = 4;
						w.Namespaces = false;
						w.WriteStartDocument();

						// <Server>
						w.WriteStartElement("Server");

						// <Log>
						w.WriteStartElement("Log");
						w.WriteElementString("FileName",         (!xmldoc.SelectSingleNode("Server/Log/FileName").IsNull() ? xmldoc.SelectSingleNode("Server/Log/FileName").InnerText : d_logfilename));
						w.WriteElementString("DateFileName",     (!xmldoc.SelectSingleNode("Server/Log/DateFileName").IsNull() ? xmldoc.SelectSingleNode("Server/Log/DateFileName").InnerText : d_logdatefilename.ToString()));
						w.WriteElementString("MaxFileSize",      (!xmldoc.SelectSingleNode("Server/Log/MaxFileSize").IsNull() ? xmldoc.SelectSingleNode("Server/Log/MaxFileSize").InnerText : d_logmaxfilesize.ToString()));
						w.WriteElementString("LogLevel",         (!xmldoc.SelectSingleNode("Server/Log/LogLevel").IsNull() ? xmldoc.SelectSingleNode("Server/Log/LogLevel").InnerText : d_loglevel.ToString()));
						w.WriteElementString("LogDirectory",     (!xmldoc.SelectSingleNode("Server/Log/LogDirectory").IsNull() ? xmldoc.SelectSingleNode("Server/Log/LogDirectory").InnerText : d_logdirectory));

						// </Log>
						w.WriteEndElement();

						// <Server>
						w.WriteStartElement("Server");

						// <Listener>
						w.WriteStartElement(@"Listener");
						w.WriteElementString("Port",            (!xmldoc.SelectSingleNode("Server/Server/Listener/Port").IsNull() ? xmldoc.SelectSingleNode("Server/Server/Listener/Port").InnerText : d_listenerport.ToString()));

						// </Server>
						w.WriteEndElement();
						w.WriteElementString("Password",        (!xmldoc.SelectSingleNode("Server/Server/Password").IsNull() ? xmldoc.SelectSingleNode("Server/Server/Password").InnerText : d_password));

						// </Server>
						w.WriteEndElement();

						// <Crash>
						w.WriteStartElement("Crash");
						w.WriteElementString("Directory",        (!xmldoc.SelectSingleNode("Server/Crash/Directory").IsNull() ? xmldoc.SelectSingleNode("Server/Crash/Directory").InnerText : d_crashdirectory));

						// </Crash>
						w.WriteEndElement();

						// <Localization>
						w.WriteStartElement("Localization");
						w.WriteElementString("Locale",          (!xmldoc.SelectSingleNode("Server/Localization/Locale").IsNull() ? xmldoc.SelectSingleNode("Server/Localization/Locale").InnerText : d_locale));

						// </Localization>
						w.WriteEndElement();

						// <Update>
						w.WriteStartElement("Update");
						w.WriteElementString("Enabled",          (!xmldoc.SelectSingleNode("Server/Update/Enabled").IsNull() ? xmldoc.SelectSingleNode("Server/Update/Enabled").InnerText : d_updateenabled.ToString()));
						w.WriteElementString("Version",          (!xmldoc.SelectSingleNode("Server/Update/Version").IsNull() ? xmldoc.SelectSingleNode("Server/Update/Version").InnerText : d_updateversion));
						w.WriteElementString("Branch",           (!xmldoc.SelectSingleNode("Server/Update/Branch").IsNull() ? xmldoc.SelectSingleNode("Server/Update/Branch").InnerText : d_updatebranch));
						w.WriteElementString("WebPage",          (!xmldoc.SelectSingleNode("Server/Update/WebPage").IsNull() ? xmldoc.SelectSingleNode("Server/Update/WebPage").InnerText : d_updatewebpage));

						// </Update>
						w.WriteEndElement();

						// <Shutdown>
						w.WriteStartElement("Shutdown");
						w.WriteElementString("MaxMemory",        (!xmldoc.SelectSingleNode("Server/Shutdown/MaxMemory").IsNull() ? xmldoc.SelectSingleNode("Server/Shutdown/MaxMemory").InnerText : d_shutdownmaxmemory.ToString()));

						// </Shutdown>
						w.WriteEndElement();

						// <Clean>
						w.WriteStartElement("Clean");
						w.WriteElementString("Config",           (!xmldoc.SelectSingleNode("Server/Clean/Config").IsNull() ? xmldoc.SelectSingleNode("Server/Clean/Config").InnerText : d_cleanconfig.ToString()));
						w.WriteElementString("Database",         (!xmldoc.SelectSingleNode("Server/Clean/Database").IsNull() ? xmldoc.SelectSingleNode("Server/Clean/Database").InnerText : d_cleandatabase.ToString()));

						// </Clean>
						w.WriteEndElement();

						// <Schumixs>
						w.WriteStartElement("Schumixs");
						w.WriteElementString("Enabled",         (!xmldoc.SelectSingleNode("Server/Schumixs/Enabled").IsNull() ? xmldoc.SelectSingleNode("Server/Schumixs/Enabled").InnerText : d_schumixsenabled.ToString()));

						var xmlschumixlist = xmldoc.SelectNodes("Server/Schumixs/Schumix");

						if(xmlschumixlist.Count == 0)
						{
							// <Schumix>
							w.WriteStartElement("Schumix");

							// <Config>
							w.WriteStartElement("Config");
							w.WriteElementString("File",            d_schumixfile);
							w.WriteElementString("Directory",       d_schumixdirectory);
							w.WriteElementString("ConsoleEncoding", d_schumixconsoleencoding);
							w.WriteElementString("Locale",          d_schumixlocale);

							// </Config>
							w.WriteEndElement();

							// </Schumix>
							w.WriteEndElement();
						}
						else
						{
							foreach(XmlNode xn in xmlschumixlist)
							{
								// <Schumix>
								w.WriteStartElement("Schumix");

								// <Config>
								w.WriteStartElement("Config");
								w.WriteElementString("File",            (!xn.SelectSingleNode("Config/File").IsNull() ? xn.SelectSingleNode("Config/File").InnerText : d_schumixfile));
								w.WriteElementString("Directory",       (!xn.SelectSingleNode("Config/Directory").IsNull() ? xn.SelectSingleNode("Config/Directory").InnerText : d_schumixdirectory));
								w.WriteElementString("ConsoleEncoding", (!xn.SelectSingleNode("Config/ConsoleEncoding").IsNull() ? xn.SelectSingleNode("Config/ConsoleEncoding").InnerText : d_schumixconsoleencoding));
								w.WriteElementString("Locale",          (!xn.SelectSingleNode("Config/Locale").IsNull() ? xn.SelectSingleNode("Config/Locale").InnerText : d_schumixlocale));

								// </Config>
								w.WriteEndElement();

								// </Schumix>
								w.WriteEndElement();
							}
						}

						// </Schumixs>
						w.WriteEndElement();

						// </Server>
						w.WriteEndElement();

						w.Flush();
						w.Close();

						if(File.Exists(filename2))
						{
							Log.Notice("XmlConfig", sLConsole.GetString("The backup has been deleted during the re-use."));
							File.Delete(filename2);
						}

						Log.Success("XmlConfig", sLConsole.GetString("Config file is completed!"));
					}
					catch(Exception e)
					{
						Log.Error("XmlConfig", sLConsole.GetString("Failure was handled during the xml writing. Details: {0}"), e.Message);
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
	}
}