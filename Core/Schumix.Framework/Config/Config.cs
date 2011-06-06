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

namespace Schumix.Framework.Config
{
	public sealed class Config
	{
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
						Log.Notice("Config", "Program leallitasa!");
						Log.Notice("Config", "Kerlek toltsed ki a configot!");
					}

					Thread.Sleep(5*1000);
					Environment.Exit(1);
				}
				else
				{
					var xmldoc = new XmlDocument();
					xmldoc.Load(string.Format("./{0}/{1}", configdir, configfile));

					int LogLevel = !xmldoc.SelectSingleNode("Schumix/Log/LogLevel").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Log/LogLevel").InnerText) : 2;
					string LogDirectory = !xmldoc.SelectSingleNode("Schumix/Log/LogDirectory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Log/LogDirectory").InnerText : "Logs";
					string IrcLogDirectory = !xmldoc.SelectSingleNode("Schumix/Log/IrcLogDirectory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Log/IrcLogDirectory").InnerText : "Channels";
					bool IrcLog = !xmldoc.SelectSingleNode("Schumix/Log/IrcLog").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Log/IrcLog").InnerText) : false;

					new LogConfig(LogLevel, LogDirectory, IrcLogDirectory, IrcLog);

					Log.Init();
					Log.Debug("Config", ">> {0}", configfile);

					Log.Notice("Config", "Config fajl betoltese.");
					string Server = !xmldoc.SelectSingleNode("Schumix/Irc/Server").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Server").InnerText : "localhost";
					int Port = !xmldoc.SelectSingleNode("Schumix/Irc/Port").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Port").InnerText) : 6667;
					string NickName = !xmldoc.SelectSingleNode("Schumix/Irc/NickName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName").InnerText : "Schumix2";
					string NickName2 = !xmldoc.SelectSingleNode("Schumix/Irc/NickName2").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName2").InnerText : "_Schumix2";
					string NickName3 = !xmldoc.SelectSingleNode("Schumix/Irc/NickName3").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName3").InnerText : "__Schumix2";
					string UserName = !xmldoc.SelectSingleNode("Schumix/Irc/UserName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/UserName").InnerText : "Schumix2";
					string UserInfo = !xmldoc.SelectSingleNode("Schumix/Irc/UserInfo").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/UserInfo").InnerText : "Schumix2 IRC Bot";
					string MasterChannel = !xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel").InnerText : "#schumix2";
					string IgnoreChannels = !xmldoc.SelectSingleNode("Schumix/Irc/IgnoreChannels").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/IgnoreChannels").InnerText : "#test";
					string IgnoreNames = !xmldoc.SelectSingleNode("Schumix/Irc/IgnoreNames").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/IgnoreNames").InnerText : "";
					bool UseNickServ = !xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Enabled").InnerText) : false;
					string NickServPassword = !xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Password").InnerText : "pass";
					bool UseHostServ = !xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Enabled").InnerText) : false;
					bool HostServStatus = !xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Vhost").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Vhost").InnerText) : false;
					int MessageSending = !xmldoc.SelectSingleNode("Schumix/Irc/Wait/MessageSending").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Wait/MessageSending").InnerText) : 50;
					string CommandPrefix = !xmldoc.SelectSingleNode("Schumix/Irc/Command/Prefix").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Command/Prefix").InnerText : "$";

					new IRCConfig(Server, Port, NickName, NickName2, NickName3, UserName, UserInfo, MasterChannel, IgnoreChannels, IgnoreNames, UseNickServ, NickServPassword, UseHostServ, HostServStatus, MessageSending, CommandPrefix);

					bool Enabled = !xmldoc.SelectSingleNode("Schumix/MySql/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/MySql/Enabled").InnerText) : false;
					string Host = !xmldoc.SelectSingleNode("Schumix/MySql/Host").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Host").InnerText : "localhost";
					string User = !xmldoc.SelectSingleNode("Schumix/MySql/User").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/User").InnerText : "root";
					string Password = !xmldoc.SelectSingleNode("Schumix/MySql/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Password").InnerText : "pass";
					string Database = !xmldoc.SelectSingleNode("Schumix/MySql/Database").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Database").InnerText : "database";
					string Charset = !xmldoc.SelectSingleNode("Schumix/MySql/Charset").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Charset").InnerText : "utf8";

					new MySqlConfig(Enabled, Host, User, Password, Database, Charset);

					Enabled = !xmldoc.SelectSingleNode("Schumix/SQLite/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/SQLite/Enabled").InnerText) : false;
					string FileName = !xmldoc.SelectSingleNode("Schumix/SQLite/FileName").IsNull() ? xmldoc.SelectSingleNode("Schumix/SQLite/FileName").InnerText : "Schumix.db3";

					new SQLiteConfig(Enabled, FileName);

					Enabled = !xmldoc.SelectSingleNode("Schumix/Addons/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Addons/Enabled").InnerText) : true;
					string Ignore = !xmldoc.SelectSingleNode("Schumix/Addons/Ignore").IsNull() ? xmldoc.SelectSingleNode("Schumix/Addons/Ignore").InnerText : "SvnRssAddon,GitRssAddon,HgRssAddon,TesztAddon";
					string Directory = !xmldoc.SelectSingleNode("Schumix/Addons/Directory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Addons/Directory").InnerText : "Addons";

					new AddonsConfig(Enabled, Ignore, Directory);

					string Locale = !xmldoc.SelectSingleNode("Schumix/Localization/Locale").IsNull() ? xmldoc.SelectSingleNode("Schumix/Localization/Locale").InnerText : "enUS";

					new LocalizationConfig(Locale);

					Log.Success("Config", "Config adatbazis betoltve.");
					Console.WriteLine();
				}
			}
			catch(Exception e)
			{
				new LogConfig(3, "Logs", "Channels", false);
				Log.Error("Config", "Hiba oka: {0}", e.Message);
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
					Log.Error("Config", "Nincs config fajl!");
					Log.Debug("Config", "Elkeszitese folyamatban...");
					var w = new XmlTextWriter(string.Format("./{0}/{1}", ConfigDirectory, ConfigFile), null);

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
						w.WriteElementString("Server", "localhost");
						w.WriteElementString("Port", "6667");
						w.WriteElementString("NickName", "Schumix2");
						w.WriteElementString("NickName2", "_Schumix2");
						w.WriteElementString("NickName3", "__Schumix2");
						w.WriteElementString("UserName", "Schumix2");
						w.WriteElementString("UserInfo", "Schumix2 IRC Bot");
						w.WriteElementString("MasterChannel", "#schumix2");
						w.WriteElementString("IgnoreChannels", "#test");
						w.WriteElementString("IgnoreNames", "");

						// <NickServ>
						w.WriteStartElement("NickServ");
						w.WriteElementString("Enabled", "false");
						w.WriteElementString("Password", "pass");

						// </NickServ>
						w.WriteEndElement();

						// <HostServ>
						w.WriteStartElement("HostServ");
						w.WriteElementString("Enabled", "false");
						w.WriteElementString("Vhost", "false");

						// </HostServ>
						w.WriteEndElement();

						// <Wait>
						w.WriteStartElement("Wait");
						w.WriteElementString("MessageSending", "50");

						// </Wait>
						w.WriteEndElement();

						// <Command>
						w.WriteStartElement("Command");
						w.WriteElementString("Prefix", "$");

						// </Command>
						w.WriteEndElement();

						// </Irc>
						w.WriteEndElement();

						// <Log>
						w.WriteStartElement("Log");
						w.WriteElementString("LogLevel", "2");
						w.WriteElementString("LogDirectory", "Logs");
						w.WriteElementString("IrcLogDirectory", "Channels");
						w.WriteElementString("IrcLog", "false");

						// </Log>
						w.WriteEndElement();

						// <MySql>
						w.WriteStartElement("MySql");
						w.WriteElementString("Enabled", "false");
						w.WriteElementString("Host", "localhost");
						w.WriteElementString("User", "root");
						w.WriteElementString("Password", "pass");
						w.WriteElementString("Database", "database");
						w.WriteElementString("Charset", "utf8");

						// </MySql>
						w.WriteEndElement();

						// <SQLite>
						w.WriteStartElement("SQLite");
						w.WriteElementString("Enabled", "false");
						w.WriteElementString("FileName", "Schumix.db3");

						// </SQLite>
						w.WriteEndElement();

						// <Plugins>
						w.WriteStartElement("Addons");
						w.WriteElementString("Enabled", "true");
						w.WriteElementString("Ignore", "SvnRssAddon,GitRssAddon,HgRssAddon,TesztAddon");
						w.WriteElementString("Directory", "Addons");

						// </Plugins>
						w.WriteEndElement();

						// <Localization>
						w.WriteStartElement("Localization");
						w.WriteElementString("Locale", "enUS");

						// </Localization>
						w.WriteEndElement();

						// </Schumix>
						w.WriteEndElement();

						w.Flush();
						w.Close();

						Log.Success("Config", "Config fajl elkeszult!");
						return false;
					}
					catch(Exception e)
					{
						Log.Error("Config", "Hiba az xml irasa soran: {0}", e.Message);
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
		public static string Server { get; private set; }
		public static int Port { get; private set; }
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

		public IRCConfig(string server, int port, string nickname, string nickname2, string nickname3, string username, string userinfo, string masterchannel, string ignorechannels, string ignorenames, bool usenickserv, string nickservpassword, bool usehostserv, bool hostservenabled, int messagesending, string commandprefix)
		{
			Server           = server;
			Port             = port;
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
			Log.Notice("IRCConfig", "Irc beallitasai betoltve.");
		}
	}

	public sealed class MySqlConfig
	{
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
			Log.Notice("MySqlConfig", "MySql beallitasai betoltve.");
		}
	}

	public sealed class SQLiteConfig
	{
		public static bool Enabled { get; private set; }
		public static string FileName { get; private set; }

		public SQLiteConfig(bool enabled, string filename)
		{
			Enabled  = enabled;
			FileName = filename;
			Log.Notice("SQLiteConfig", "SQLite beallitasai betoltve.");
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
		public static bool Enabled { get; private set; }
		public static string Ignore { get; private set; }
		public static string Directory { get; private set; }

		public AddonsConfig(bool enabled, string ignore, string directory)
		{
			Enabled   = enabled;
			Ignore    = ignore;
			Directory = directory;
			Log.Notice("AddonsConfig", "Addons beallitasai betoltve.");
		}
	}

	public sealed class LocalizationConfig
	{
		public static string Locale { get; private set; }

		public LocalizationConfig(string locale)
		{
			Locale = locale;
			Log.Notice("LocalizationConfig", "Localization beallitasai betoltve.");
		}
	}
}