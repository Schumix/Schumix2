/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Megax <http://www.megaxx.info/>
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
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Server.Config
{
	public sealed class Config
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly New.Schumix sSchumix = Singleton<New.Schumix>.Instance;
		private const string _logfilename             = "Server.log";
		private const int _loglevel                   = 2;
		private const string _logdirectory            = "Logs";
		private const int _listenerport               = 35220;
		private const string _password                = "schumix";
		private const string _locale                  = "enUS";
		private const bool _updateenabled           = false;
		private const bool _updateversionsenabled   = false;
		private const string _updateversionsversion = "x.x.x";
		private const string _updatewebpage         = "http://megax.uw.hu/Schumix2/";
		private const bool _schumixsenabled           = false;
		private const int _schumixsnumber             = 1;
		private const string _schumix0file            = "Schumix.xml";
		private const string _schumix0directory       = "Configs";
		private const string _schumix0consoleencoding = "utf-8";
		private const string _schumix0locale          = "enUS";
		private bool error                            = false;

		public Config(string configdir, string configfile)
		{
			try
			{
				new ServerConfig(configdir, configfile);

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
					var xmldoc = new XmlDocument();
					xmldoc.Load(string.Format("./{0}/{1}", configdir, configfile));

					string LogFileName = !xmldoc.SelectSingleNode("Server/Log/FileName").IsNull() ? xmldoc.SelectSingleNode("Server/Log/FileName").InnerText : _logfilename;
					int LogLevel = !xmldoc.SelectSingleNode("Server/Log/LogLevel").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Server/Log/LogLevel").InnerText) : _loglevel;
					string LogDirectory = !xmldoc.SelectSingleNode("Server/Log/LogDirectory").IsNull() ? xmldoc.SelectSingleNode("Server/Log/LogDirectory").InnerText : _logdirectory;

					new Framework.Config.LogConfig(LogFileName, LogLevel, LogDirectory, string.Empty, false);

					Log.Init(LogFileName);
					Log.Debug("Config", ">> {0}", configfile);

					Log.Notice("Config", sLConsole.Config("Text3"));
					int ListenerPort = !xmldoc.SelectSingleNode("Server/Server/Listener/Port").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Server/Server/Listener/Port").InnerText) : _listenerport;
					string Password = !xmldoc.SelectSingleNode("Server/Server/Password").IsNull() ? xmldoc.SelectSingleNode("Server/Server/Password").InnerText : _password;

					new ServerConfigs(ListenerPort, Password);

					string Locale = !xmldoc.SelectSingleNode("Server/Localization/Locale").IsNull() ? xmldoc.SelectSingleNode("Server/Localization/Locale").InnerText : _locale;

					new LocalizationConfig(Locale);

					bool Enabled = !xmldoc.SelectSingleNode("Schumix/Update/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Update/Enabled").InnerText) : _updateenabled;
					bool VersionsEnabled = !xmldoc.SelectSingleNode("Schumix/Update/Versions/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Update/Versions/Enabled").InnerText) : _updateversionsenabled;
					string Version = !xmldoc.SelectSingleNode("Schumix/Update/Versions/Version").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/Versions/Version").InnerText : _updateversionsversion;
					string WebPage = !xmldoc.SelectSingleNode("Schumix/Update/WebPage").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/WebPage").InnerText : _updatewebpage;

					new Framework.Config.UpdateConfig(Enabled, VersionsEnabled, Version, WebPage);

					int number = !xmldoc.SelectSingleNode("Server/Schumixs/Number").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Server/Schumixs/Number").InnerText) : _schumixsnumber;

					if(!xmldoc.SelectSingleNode("Server/Schumixs/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Server/Schumixs/Enabled").InnerText) : _schumixsenabled)
					{
						Task.Factory.StartNew(() =>
						{
							Log.Notice("Schumix", sLConsole.Config("Text9"));
							Log.Notice("Schumix", sLConsole.Config("Text10"), number);

							for(int i = 0; i < number; i++)
							{
								string file = !xmldoc.SelectSingleNode("Server/Schumixs/Schumix" + i + "/Config/File").IsNull() ? xmldoc.SelectSingleNode("Server/Schumixs/Schumix" + i + "/Config/File").InnerText : _schumix0file;
								string dir = !xmldoc.SelectSingleNode("Server/Schumixs/Schumix" + i + "/Config/Directory").IsNull() ? xmldoc.SelectSingleNode("Server/Schumixs/Schumix" + i + "/Config/Directory").InnerText : _schumix0directory;
								string ce = !xmldoc.SelectSingleNode("Server/Schumixs/Schumix" + i + "/Config/ConsoleEncoding").IsNull() ? xmldoc.SelectSingleNode("Server/Schumixs/Schumix" + i + "/Config/ConsoleEncoding").InnerText : _schumix0consoleencoding;
								string lo = !xmldoc.SelectSingleNode("Server/Schumixs/Schumix" + i + "/Config/Locale").IsNull() ? xmldoc.SelectSingleNode("Server/Schumixs/Schumix" + i + "/Config/Locale").InnerText : _schumix0locale;
								sSchumix.Start(file, dir, ce, lo);
								Thread.Sleep(10*1000);
							}
						});
					}
					else
						Log.Warning("Schumix", sLConsole.Config("Text11"));

					Log.Success("Config", sLConsole.Config("Text4"));
					Console.WriteLine();
				}
			}
			catch(Exception e)
			{
				new Framework.Config.LogConfig(_logfilename, 3, _logdirectory, string.Empty, false);
				Log.Error("Config", sLConsole.Exception("Error"), e.Message);
			}
		}

		private bool IsConfig(string ConfigDirectory, string ConfigFile)
		{
			if(!Directory.Exists(ConfigDirectory))
				Directory.CreateDirectory(ConfigDirectory);

			try
			{
				if(File.Exists(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile)))
					return true;
				else
				{
					new Framework.Config.LogConfig(_logfilename, 3, _logdirectory, string.Empty, false);
					Log.Init(_logfilename);
					Log.Error("Config", sLConsole.Config("Text5"));
					Log.Debug("Config", sLConsole.Config("Text6"));
					var w = new XmlTextWriter(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile), null);
					var xmldoc = new XmlDocument();

					if(File.Exists(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile)))
						xmldoc.Load(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile));

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
						w.WriteElementString("FileName",        (!xmldoc.SelectSingleNode("Server/Log/FileName").IsNull() ? xmldoc.SelectSingleNode("Server/Log/FileName").InnerText : _logfilename));
						w.WriteElementString("LogLevel",        (!xmldoc.SelectSingleNode("Server/Log/LogLevel").IsNull() ? xmldoc.SelectSingleNode("Server/Log/LogLevel").InnerText : _loglevel.ToString()));
						w.WriteElementString("LogDirectory",    (!xmldoc.SelectSingleNode("Server/Log/LogDirectory").IsNull() ? xmldoc.SelectSingleNode("Server/Log/LogDirectory").InnerText : _logdirectory));

						// </Log>
						w.WriteEndElement();

						// <Server>
						w.WriteStartElement("Server");

						// <Listener>
						w.WriteStartElement(@"Listener");
						w.WriteElementString("Port",            (!xmldoc.SelectSingleNode("Server/Server/Listener/Port").IsNull() ? xmldoc.SelectSingleNode("Server/Server/Listener/Port").InnerText : _listenerport.ToString()));

						// </Server>
						w.WriteEndElement();
						w.WriteElementString("Password",        (!xmldoc.SelectSingleNode("Server/Server/Password").IsNull() ? xmldoc.SelectSingleNode("Server/Server/Password").InnerText : _password));

						// </Server>
						w.WriteEndElement();

						// <Localization>
						w.WriteStartElement("Localization");
						w.WriteElementString("Locale",          (!xmldoc.SelectSingleNode("Server/Localization/Locale").IsNull() ? xmldoc.SelectSingleNode("Server/Localization/Locale").InnerText : _locale));

						// </Localization>
						w.WriteEndElement();

						// <Update>
						w.WriteStartElement("Update");
						w.WriteElementString("Enabled",         (!xmldoc.SelectSingleNode("Schumix/Update/Enabled").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/Enabled").InnerText : _updateenabled.ToString()));

						// <Versions>
						w.WriteStartElement("Versions");
						w.WriteElementString("Enabled",         (!xmldoc.SelectSingleNode("Schumix/Update/Versions/Enabled").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/Versions/Enabled").InnerText : _updateversionsenabled.ToString()));
						w.WriteElementString("Version",         (!xmldoc.SelectSingleNode("Schumix/Update/Versions/Version").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/Versions/Version").InnerText : _updateversionsversion));

						// </Versions>
						w.WriteEndElement();

						w.WriteElementString("WebPage",         (!xmldoc.SelectSingleNode("Schumix/Update/WebPage").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/WebPage").InnerText : _updatewebpage));

						// </Update>
						w.WriteEndElement();

						// <Schumixs>
						w.WriteStartElement("Schumixs");
						w.WriteElementString("Enabled",         (!xmldoc.SelectSingleNode("Server/Schumixs/Enabled").IsNull() ? xmldoc.SelectSingleNode("Server/Schumixs/Enabled").InnerText : _schumixsenabled.ToString()));
						w.WriteElementString("Number",          (!xmldoc.SelectSingleNode("Server/Schumixs/Number").IsNull() ? xmldoc.SelectSingleNode("Server/Schumixs/Number").InnerText : _schumixsnumber.ToString()));

						// <Schumix0>
						w.WriteStartElement("Schumix0");

						// <Config>
						w.WriteStartElement("Config");
						w.WriteElementString("File",            (!xmldoc.SelectSingleNode("Server/Schumixs/Schumix0/Config/File").IsNull() ? xmldoc.SelectSingleNode("Server/Schumixs/Schumix0/Config/File").InnerText : _schumix0file));
						w.WriteElementString("Directory",       (!xmldoc.SelectSingleNode("Server/Schumixs/Schumix0/Config/Directory").IsNull() ? xmldoc.SelectSingleNode("Server/Schumixs/Schumix0/Config/Directory").InnerText : _schumix0directory));
						w.WriteElementString("ConsoleEncoding", (!xmldoc.SelectSingleNode("Server/Schumixs/Schumix0/Config/ConsoleEncoding").IsNull() ? xmldoc.SelectSingleNode("Server/Schumixs/Schumix0/Config/ConsoleEncoding").InnerText : _schumix0consoleencoding));
						w.WriteElementString("Locale",          (!xmldoc.SelectSingleNode("Server/Schumixs/Schumix0/Config/Locale").IsNull() ? xmldoc.SelectSingleNode("Server/Schumixs/Schumix0/Config/Locale").InnerText : _schumix0locale));

						// </Config>
						w.WriteEndElement();

						// </Schumix0>
						w.WriteEndElement();

						// </Schumixs>
						w.WriteEndElement();

						// </Server>
						w.WriteEndElement();

						w.Flush();
						w.Close();

						if(File.Exists(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile)))
							File.Delete(string.Format("./{0}/_{1}", ConfigDirectory, ConfigFile));

						Log.Success("Config", sLConsole.Config("Text7"));
						return false;
					}
					catch(Exception e)
					{
						Log.Error("Config", sLConsole.Config("Text8"), e.Message);
						error = true;
						return false;
					}
				}
			}
			catch(DirectoryNotFoundException)
			{
				IsConfig(ConfigDirectory, ConfigFile);
			}

			return true;
		}
	}

	public sealed class ServerConfig
	{
		public static string ConfigDirectory { get; private set; }
		public static string ConfigFile { get; private set; }

		public ServerConfig(string configdirectory, string configfile)
		{
			ConfigDirectory = configdirectory;
			ConfigFile      = configfile;
		}
	}

	public sealed class ServerConfigs
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static int ListenerPort { get; private set; }
		public static string Password { get; private set; }

		public ServerConfigs(int listenerport, string password)
		{
			ListenerPort = listenerport;
			Password     = password;
			Log.Notice("ServerConfigs", sLConsole.ServerConfigs("Text"));
		}
	}

	public sealed class LocalizationConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static string Locale { get; private set; }

		public LocalizationConfig(string locale)
		{
			Locale = locale;
			Log.Notice("LocalizationConfig", sLConsole.LocalizationConfig("Text"));
		}
	}
}