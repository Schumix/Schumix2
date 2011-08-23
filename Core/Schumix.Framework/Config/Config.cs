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
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Config
{
	public sealed class Config
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private const int _loglevel = 2;
		private const string _logdirectory = "Logs";
		private const string _irclogdirectory = "Channels";
		private const bool _irclog = false;
		private const string _server = "localhost";
		private const int _port = 6667;
		private const bool _ssl = false;
		private const string _nickname = "Schumix2";
		private const string _nickname2 = "_Schumix2";
		private const string _nickname3 = "__Schumix2";
		private const string _username = "Schumix2";
		private const string _userinfo = "Schumix2 IRC Bot";
		private const string _masterchannel = "#schumix2";
		private const string _ignorechannels = " ";
		private const string _ignorenames = " ";
		private const bool _usenickserv = false;
		private const string _nickservpassword = "password";
		private const bool _usehostserv = false;
		private const bool _hostservstatus = false;
		private const int _messagesending = 400;
		private const string _commandprefix = "$";
		private const bool _mysqlenabled = false;
		private const string _mysqlhost = "localhost";
		private const string _mysqluser = "root";
		private const string _mysqlpassword = "password";
		private const string _mysqldatabase = "database";
		private const string _mysqlcharset = "utf8";
		private const bool _sqliteenabled = false;
		private const string _sqlitefilename = "Schumix.db3";
		private const bool _addonenabled = true;
		private const string _addonignore = "SvnRssAddon,GitRssAddon,HgRssAddon,TesztAddon";
		private const string _addondirectory = "Addons";
		private const bool _scriptenabled = false;
		private const string _scriptdirectory = "Scripts";
		private const string _locale = "enUS";
		private const bool _updateenabled = false;
		private const bool _updateversionsenabled = false;
		private const string _updateversionsversion = "x.x.x";
		private const string _updatewebpage = "http://megax.uw.hu/Schumix2/";
		private bool error = false;

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
					var xmldoc = new XmlDocument();
					xmldoc.Load(string.Format("./{0}/{1}", configdir, configfile));

					int LogLevel = !xmldoc.SelectSingleNode("Schumix/Log/LogLevel").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Log/LogLevel").InnerText) : _loglevel;
					string LogDirectory = !xmldoc.SelectSingleNode("Schumix/Log/LogDirectory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Log/LogDirectory").InnerText : _logdirectory;
					string IrcLogDirectory = !xmldoc.SelectSingleNode("Schumix/Log/IrcLogDirectory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Log/IrcLogDirectory").InnerText : _irclogdirectory;
					bool IrcLog = !xmldoc.SelectSingleNode("Schumix/Log/IrcLog").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Log/IrcLog").InnerText) : _irclog;

					new LogConfig(LogLevel, LogDirectory, IrcLogDirectory, IrcLog);

					Log.Init();
					Log.Debug("Config", ">> {0}", configfile);

					Log.Notice("Config", sLConsole.Config("Text3"));
					string Server = !xmldoc.SelectSingleNode("Schumix/Irc/Server").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Server").InnerText : _server;
					int Port = !xmldoc.SelectSingleNode("Schumix/Irc/Port").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Port").InnerText) : _port;
					bool Ssl = !xmldoc.SelectSingleNode("Schumix/Irc/Ssl").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/Ssl").InnerText) : _ssl;
					string NickName = !xmldoc.SelectSingleNode("Schumix/Irc/NickName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName").InnerText : _nickname;
					string NickName2 = !xmldoc.SelectSingleNode("Schumix/Irc/NickName2").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName2").InnerText : _nickname2;
					string NickName3 = !xmldoc.SelectSingleNode("Schumix/Irc/NickName3").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName3").InnerText : _nickname3;
					string UserName = !xmldoc.SelectSingleNode("Schumix/Irc/UserName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/UserName").InnerText : _username;
					string UserInfo = !xmldoc.SelectSingleNode("Schumix/Irc/UserInfo").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/UserInfo").InnerText : _userinfo;
					string MasterChannel = !xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel").InnerText : _masterchannel;
					string IgnoreChannels = !xmldoc.SelectSingleNode("Schumix/Irc/IgnoreChannels").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/IgnoreChannels").InnerText : _ignorechannels;
					string IgnoreNames = !xmldoc.SelectSingleNode("Schumix/Irc/IgnoreNames").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/IgnoreNames").InnerText : _ignorenames;
					bool UseNickServ = !xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Enabled").InnerText) : _usenickserv;
					string NickServPassword = !xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Password").InnerText : _nickservpassword;
					bool UseHostServ = !xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Enabled").InnerText) : _usehostserv;
					bool HostServStatus = !xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Vhost").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Vhost").InnerText) : _hostservstatus;
					int MessageSending = !xmldoc.SelectSingleNode("Schumix/Irc/Wait/MessageSending").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Wait/MessageSending").InnerText) : _messagesending;
					string CommandPrefix = !xmldoc.SelectSingleNode("Schumix/Irc/Command/Prefix").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Command/Prefix").InnerText : _commandprefix;

					new IRCConfig(Server, Port, Ssl, NickName, NickName2, NickName3, UserName, UserInfo, MasterChannel, IgnoreChannels, IgnoreNames, UseNickServ, NickServPassword, UseHostServ, HostServStatus, MessageSending, CommandPrefix);

					bool Enabled = !xmldoc.SelectSingleNode("Schumix/MySql/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/MySql/Enabled").InnerText) : _mysqlenabled;
					string Host = !xmldoc.SelectSingleNode("Schumix/MySql/Host").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Host").InnerText : _mysqlhost;
					string User = !xmldoc.SelectSingleNode("Schumix/MySql/User").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/User").InnerText : _mysqluser;
					string Password = !xmldoc.SelectSingleNode("Schumix/MySql/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Password").InnerText : _mysqlpassword;
					string Database = !xmldoc.SelectSingleNode("Schumix/MySql/Database").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Database").InnerText : _mysqldatabase;
					string Charset = !xmldoc.SelectSingleNode("Schumix/MySql/Charset").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Charset").InnerText : _mysqlcharset;

					new MySqlConfig(Enabled, Host, User, Password, Database, Charset);

					Enabled = !xmldoc.SelectSingleNode("Schumix/SQLite/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/SQLite/Enabled").InnerText) : _sqliteenabled;
					string FileName = !xmldoc.SelectSingleNode("Schumix/SQLite/FileName").IsNull() ? xmldoc.SelectSingleNode("Schumix/SQLite/FileName").InnerText : _sqlitefilename;

					new SQLiteConfig(Enabled, FileName);

					Enabled = !xmldoc.SelectSingleNode("Schumix/Addons/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Addons/Enabled").InnerText) : _addonenabled;
					string Ignore = !xmldoc.SelectSingleNode("Schumix/Addons/Ignore").IsNull() ? xmldoc.SelectSingleNode("Schumix/Addons/Ignore").InnerText : _addonignore;
					string Directory = !xmldoc.SelectSingleNode("Schumix/Addons/Directory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Addons/Directory").InnerText : _addondirectory;

					new AddonsConfig(Enabled, Ignore, Directory);

					bool Lua = !xmldoc.SelectSingleNode("Schumix/Scripts/Lua").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Scripts/Lua").InnerText) : _scriptenabled;
					Directory = !xmldoc.SelectSingleNode("Schumix/Scripts/Directory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Scripts/Directory").InnerText : _scriptdirectory;

					new ScriptsConfig(Lua, Directory);

					string Locale = !xmldoc.SelectSingleNode("Schumix/Localization/Locale").IsNull() ? xmldoc.SelectSingleNode("Schumix/Localization/Locale").InnerText : _locale;

					new LocalizationConfig(Locale);

					Enabled = !xmldoc.SelectSingleNode("Schumix/Update/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Update/Enabled").InnerText) : _updateenabled;
					bool VersionsEnabled = !xmldoc.SelectSingleNode("Schumix/Update/Versions/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Update/Versions/Enabled").InnerText) : _updateversionsenabled;
					string Version = !xmldoc.SelectSingleNode("Schumix/Update/Versions/Version").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/Versions/Version").InnerText : _updateversionsversion;
					string WebPage = !xmldoc.SelectSingleNode("Schumix/Update/WebPage").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/WebPage").InnerText : _updatewebpage;

					new UpdateConfig(Enabled, VersionsEnabled, Version, WebPage);

					Log.Success("Config", sLConsole.Config("Text4"));
					Console.WriteLine();
				}
			}
			catch(Exception e)
			{
				new LogConfig(3, "Logs", "Channels", false);
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
					new LogConfig(3, "Logs", "Channels", false);
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

						// <Schumix>
						w.WriteStartElement("Schumix");

						// <Irc>
						w.WriteStartElement("Irc");
						w.WriteElementString("Server",          (!xmldoc.SelectSingleNode("Schumix/Irc/Server").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Server").InnerText : _server));
						w.WriteElementString("Port",            (!xmldoc.SelectSingleNode("Schumix/Irc/Port").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Port").InnerText : _port.ToString()));
						w.WriteElementString("Ssl",             (!xmldoc.SelectSingleNode("Schumix/Irc/Ssl").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Ssl").InnerText : _ssl.ToString()));
						w.WriteElementString("NickName",        (!xmldoc.SelectSingleNode("Schumix/Irc/NickName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName").InnerText : _nickname));
						w.WriteElementString("NickName2",       (!xmldoc.SelectSingleNode("Schumix/Irc/NickName2").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName2").InnerText : _nickname2));
						w.WriteElementString("NickName3",       (!xmldoc.SelectSingleNode("Schumix/Irc/NickName3").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName3").InnerText : _nickname3));
						w.WriteElementString("UserName",        (!xmldoc.SelectSingleNode("Schumix/Irc/UserName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/UserName").InnerText : _username));
						w.WriteElementString("UserInfo",        (!xmldoc.SelectSingleNode("Schumix/Irc/UserInfo").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/UserInfo").InnerText : _userinfo));
						w.WriteElementString("MasterChannel",   (!xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel").InnerText : _masterchannel));
						w.WriteElementString("IgnoreChannels",  (!xmldoc.SelectSingleNode("Schumix/Irc/IgnoreChannels").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/IgnoreChannels").InnerText : _ignorechannels));
						w.WriteElementString("IgnoreNames",     (!xmldoc.SelectSingleNode("Schumix/Irc/IgnoreNames").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/IgnoreNames").InnerText : _ignorenames));

						// <NickServ>
						w.WriteStartElement("NickServ");
						w.WriteElementString("Enabled",         (!xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Enabled").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Enabled").InnerText : _usenickserv.ToString()));
						w.WriteElementString("Password",        (!xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Password").InnerText : _nickservpassword));

						// </NickServ>
						w.WriteEndElement();

						// <HostServ>
						w.WriteStartElement("HostServ");
						w.WriteElementString("Enabled",         (!xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Enabled").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Enabled").InnerText : _usehostserv.ToString()));
						w.WriteElementString("Vhost",           (!xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Vhost").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Vhost").InnerText : _hostservstatus.ToString()));

						// </HostServ>
						w.WriteEndElement();

						// <Wait>
						w.WriteStartElement("Wait");
						w.WriteElementString("MessageSending",  (!xmldoc.SelectSingleNode("Schumix/Irc/Wait/MessageSending").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Wait/MessageSending").InnerText : _messagesending.ToString()));

						// </Wait>
						w.WriteEndElement();

						// <Command>
						w.WriteStartElement("Command");
						w.WriteElementString("Prefix",          (!xmldoc.SelectSingleNode("Schumix/Irc/Command/Prefix").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Command/Prefix").InnerText : _commandprefix));

						// </Command>
						w.WriteEndElement();

						// </Irc>
						w.WriteEndElement();

						// <Log>
						w.WriteStartElement("Log");
						w.WriteElementString("LogLevel",        (!xmldoc.SelectSingleNode("Schumix/Log/LogLevel").IsNull() ? xmldoc.SelectSingleNode("Schumix/Log/LogLevel").InnerText : _loglevel.ToString()));
						w.WriteElementString("LogDirectory",    (!xmldoc.SelectSingleNode("Schumix/Log/LogDirectory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Log/LogDirectory").InnerText : _logdirectory));
						w.WriteElementString("IrcLogDirectory", (!xmldoc.SelectSingleNode("Schumix/Log/IrcLogDirectory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Log/IrcLogDirectory").InnerText : _irclogdirectory));
						w.WriteElementString("IrcLog",          (!xmldoc.SelectSingleNode("Schumix/Log/IrcLog").IsNull() ? xmldoc.SelectSingleNode("Schumix/Log/IrcLog").InnerText : _irclog.ToString()));

						// </Log>
						w.WriteEndElement();

						// <MySql>
						w.WriteStartElement("MySql");
						w.WriteElementString("Enabled",         (!xmldoc.SelectSingleNode("Schumix/MySql/Enabled").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Enabled").InnerText : _mysqlenabled.ToString()));
						w.WriteElementString("Host",            (!xmldoc.SelectSingleNode("Schumix/MySql/Host").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Host").InnerText : _mysqlhost));
						w.WriteElementString("User",            (!xmldoc.SelectSingleNode("Schumix/MySql/User").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/User").InnerText : _mysqluser));
						w.WriteElementString("Password",        (!xmldoc.SelectSingleNode("Schumix/MySql/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Password").InnerText : _mysqlpassword));
						w.WriteElementString("Database",        (!xmldoc.SelectSingleNode("Schumix/MySql/Database").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Database").InnerText : _mysqldatabase));
						w.WriteElementString("Charset",         (!xmldoc.SelectSingleNode("Schumix/MySql/Charset").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Charset").InnerText : _mysqlcharset));

						// </MySql>
						w.WriteEndElement();

						// <SQLite>
						w.WriteStartElement("SQLite");
						w.WriteElementString("Enabled",         (!xmldoc.SelectSingleNode("Schumix/SQLite/Enabled").IsNull() ? xmldoc.SelectSingleNode("Schumix/SQLite/Enabled").InnerText : _sqliteenabled.ToString()));
						w.WriteElementString("FileName",        (!xmldoc.SelectSingleNode("Schumix/SQLite/FileName").IsNull() ? xmldoc.SelectSingleNode("Schumix/SQLite/FileName").InnerText : _sqlitefilename));

						// </SQLite>
						w.WriteEndElement();

						// <Plugins>
						w.WriteStartElement("Addons");
						w.WriteElementString("Enabled",         (!xmldoc.SelectSingleNode("Schumix/Addons/Enabled").IsNull() ? xmldoc.SelectSingleNode("Schumix/Addons/Enabled").InnerText : _addonenabled.ToString()));
						w.WriteElementString("Ignore",          (!xmldoc.SelectSingleNode("Schumix/Addons/Ignore").IsNull() ? xmldoc.SelectSingleNode("Schumix/Addons/Ignore").InnerText : _addonignore));
						w.WriteElementString("Directory",       (!xmldoc.SelectSingleNode("Schumix/Addons/Directory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Addons/Directory").InnerText : _addondirectory));

						// </Plugins>
						w.WriteEndElement();

						// <Scripts>
						w.WriteStartElement("Scripts");
						w.WriteElementString("Lua",             (!xmldoc.SelectSingleNode("Schumix/Scripts/Lua").IsNull() ? xmldoc.SelectSingleNode("Schumix/Scripts/Lua").InnerText : _scriptenabled.ToString()));
						w.WriteElementString("Directory",       (!xmldoc.SelectSingleNode("Schumix/Scripts/Directory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Scripts/Directory").InnerText : _scriptdirectory));

						// </Scripts>
						w.WriteEndElement();

						// <Localization>
						w.WriteStartElement("Localization");
						w.WriteElementString("Locale",          (!xmldoc.SelectSingleNode("Schumix/Localization/Locale").IsNull() ? xmldoc.SelectSingleNode("Schumix/Localization/Locale").InnerText : _locale));

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

						// </Schumix>
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

	public sealed class SchumixConfig
	{
		public static string ConfigDirectory { get; private set; }
		public static string ConfigFile { get; private set; }

		public SchumixConfig(string configdirectory, string configfile)
		{
			ConfigDirectory = configdirectory;
			ConfigFile      = configfile;
		}
	}

	public sealed class IRCConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static string Server { get; private set; }
		public static int Port { get; private set; }
		public static bool Ssl { get; private set; }
		public static string NickName { get; private set; }
		public static string NickName2 { get; private set; }
		public static string NickName3 { get; private set; }
		public static string UserName { get; private set; }
		public static string UserInfo { get; private set; }
		public static string MasterChannel { get; private set; }
		public static string IgnoreChannels { get; private set; }
		public static string IgnoreNames { get; private set; }
		public static bool UseNickServ { get; private set; }
		public static string NickServPassword { get; private set; }
		public static bool UseHostServ { get; private set; }
		public static bool HostServEnabled { get; private set; }
		public static int MessageSending { get; private set; }
		public static string CommandPrefix { get; private set; }

		public IRCConfig(string server, int port, bool ssl, string nickname, string nickname2, string nickname3, string username, string userinfo, string masterchannel, string ignorechannels, string ignorenames, bool usenickserv, string nickservpassword, bool usehostserv, bool hostservenabled, int messagesending, string commandprefix)
		{
			Server           = server;
			Port             = port;
			Ssl              = ssl;
			NickName         = nickname;
			NickName2        = nickname2;
			NickName3        = nickname3;
			UserName         = username;
			UserInfo         = userinfo;
			MasterChannel    = masterchannel.ToLower();
			IgnoreChannels   = ignorechannels;
			IgnoreNames      = ignorenames;
			UseNickServ      = usenickserv;
			NickServPassword = nickservpassword;
			UseHostServ      = usehostserv;
			HostServEnabled  = hostservenabled;
			MessageSending   = messagesending;
			CommandPrefix    = commandprefix;
			Log.Notice("IRCConfig", sLConsole.IRCConfig("Text"));
		}
	}

	public sealed class MySqlConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static bool Enabled { get; private set; }
		public static string Host { get; private set; }
		public static string User { get; private set; }
		public static string Password { get; private set; }
		public static string Database { get; private set; }
		public static string Charset { get; private set; }

		public MySqlConfig(bool enabled, string host, string user, string password, string database, string charset)
		{
			Enabled  = enabled;
			Host     = host;
			User     = user;
			Password = password;
			Database = database;
			Charset  = charset;
			Log.Notice("MySqlConfig", sLConsole.MySqlConfig("Text"));
		}
	}

	public sealed class SQLiteConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static bool Enabled { get; private set; }
		public static string FileName { get; private set; }

		public SQLiteConfig(bool enabled, string filename)
		{
			Enabled  = enabled;
			FileName = filename;
			Log.Notice("SQLiteConfig", sLConsole.SQLiteConfig("Text"));
		}
	}

	public sealed class LogConfig
	{
		public static int LogLevel { get; private set; }
		public static string LogDirectory { get; private set; }
		public static string IrcLogDirectory { get; private set; }
		public static bool IrcLog { get; private set; }

		public LogConfig(int loglevel, string logdirectory, string irclogdirectory, bool irclog)
		{
			LogLevel        = loglevel;
			LogDirectory    = logdirectory;
			IrcLogDirectory = irclogdirectory;
			IrcLog          = irclog;
		}
	}

	public sealed class AddonsConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static bool Enabled { get; private set; }
		public static string Ignore { get; private set; }
		public static string Directory { get; private set; }

		public AddonsConfig(bool enabled, string ignore, string directory)
		{
			Enabled   = enabled;
			Ignore    = ignore;
			Directory = directory;
			Log.Notice("AddonsConfig", sLConsole.AddonsConfig("Text"));
		}
	}

	public sealed class ScriptsConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static bool Lua { get; private set; }
		public static string Directory { get; private set; }

		public ScriptsConfig(bool lua, string directory)
		{
			Lua   = lua;
			Directory = directory;
			Log.Notice("ScriptsConfig", sLConsole.ScriptsConfig("Text"));
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

	public sealed class UpdateConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		public static bool Enabled { get; private set; }
		public static bool VersionsEnabled { get; private set; }
		public static string Version { get; private set; }
		public static string WebPage { get; private set; }

		public UpdateConfig(bool enabled, bool versionsenabled, string version, string webpage)
		{
			Enabled         = enabled;
			VersionsEnabled = versionsenabled;
			Version         = version;
			WebPage         = webpage;
			Log.Notice("UpdateConfig", sLConsole.UpdateConfig("Text"));
		}
	}
}