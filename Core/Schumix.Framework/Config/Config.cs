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

namespace Schumix.Framework.Config
{
	public class Config
	{
		private bool error = false;

		public Config(string configfile)
		{
			try
			{
				Log.Debug("Config", ">> {0}", configfile);
				new SchumixConfig(configfile);

				if(!IsConfig(configfile))
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
					xmldoc.Load(configfile);

					Log.Notice("Config", "Config fajl betoltese...");
					string Server = xmldoc.SelectSingleNode("Schumix/Irc/Server").InnerText;
					int Port = Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Port").InnerText);
					string NickName = xmldoc.SelectSingleNode("Schumix/Irc/NickName").InnerText;
					string NickName2 = xmldoc.SelectSingleNode("Schumix/Irc/NickName2").InnerText;
					string NickName3 = xmldoc.SelectSingleNode("Schumix/Irc/NickName3").InnerText;
					string UserName = xmldoc.SelectSingleNode("Schumix/Irc/UserName").InnerText;
					string MasterChannel = xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel").InnerText;
					bool UseNickServ = Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Enabled").InnerText);
					string NickServPassword = xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Password").InnerText;
					bool UseHostServ = Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Enabled").InnerText);
					bool HostServAllapot = Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Vhost").InnerText);
					int MessageSending = Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Wait/MessageSending").InnerText);
					string CommandPrefix = xmldoc.SelectSingleNode("Schumix/Command/Prefix").InnerText;

					new IRCConfig(Server, Port, NickName, NickName2, NickName3, UserName, MasterChannel, UseNickServ, NickServPassword, UseHostServ, HostServAllapot, MessageSending, CommandPrefix);

					bool Enabled = Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/MySql/Enabled").InnerText);
					string Host = xmldoc.SelectSingleNode("Schumix/MySql/Host").InnerText;
					string User = xmldoc.SelectSingleNode("Schumix/MySql/User").InnerText;
					string Password = xmldoc.SelectSingleNode("Schumix/MySql/Password").InnerText;
					string Database = xmldoc.SelectSingleNode("Schumix/MySql/Database").InnerText;

					new MySqlConfig(Enabled, Host, User, Password, Database);

					Enabled = Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/SQLite/Enabled").InnerText);
					string FileName = xmldoc.SelectSingleNode("Schumix/SQLite/FileName").InnerText;

					new SQLiteConfig(Enabled, FileName);

					int LogLevel = Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Log/LogLevel").InnerText);
					string LogDirectory = xmldoc.SelectSingleNode("Schumix/Log/LogDirectory").InnerText;
					string IrcLogDirectory = xmldoc.SelectSingleNode("Schumix/Log/IrcLogDirectory").InnerText;
					bool IrcLog = Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Log/IrcLog").InnerText);

					new LogConfig(LogLevel, LogDirectory, IrcLogDirectory, IrcLog);

					Enabled = Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Addons/Enabled").InnerText);
					string Directory = xmldoc.SelectSingleNode("Schumix/Addons/Directory").InnerText;

					new AddonsConfig(Enabled, Directory);

					Log.Success("Config", "Config adatbazis betoltve.");
					Console.WriteLine("");
				}
			}
			catch(Exception e)
			{
				new LogConfig(3, "Logs", "Szoba", false);
				Log.Error("Config", "Hiba oka: {0}", e.Message);
			}
		}

		private bool IsConfig(string ConfigFile)
		{
			if(File.Exists(ConfigFile))
				return true;
			else
			{
				new LogConfig(3, "Logs", "Szoba", false);
				Log.Error("Config", "Nincs config fajl!");
				Log.Debug("Config", "Elkeszitese folyamatban...");
				var w = new XmlTextWriter(ConfigFile, null);

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
					w.WriteElementString("MasterChannel", "#schumix2");

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

					// </Irc>
					w.WriteEndElement();

					// <Log>
					w.WriteStartElement("Log");
					w.WriteElementString("LogLevel", "2");
					w.WriteElementString("LogDirectory", "Logs");
					w.WriteElementString("IrcLogDirectory", "Csatornak");
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

					// </MySql>
					w.WriteEndElement();

					// <SQLite>
					w.WriteStartElement("SQLite");
					w.WriteElementString("Enabled", "false");
					w.WriteElementString("FileName", "Schumix.db3");

					// </SQLite>
					w.WriteEndElement();

					// <Parancs>
					w.WriteStartElement("Command");
					w.WriteElementString("Prefix", "$");

					// </Parancs>
					w.WriteEndElement();

					// <Plugins>
					w.WriteStartElement("Addons");
					w.WriteElementString("Enabled", "true");
					w.WriteElementString("Directory", "Addons");

					// </Plugins>
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
	}

	public sealed class SchumixConfig
	{
		public static string ConfigFile { get; private set; }

		public SchumixConfig(string configfile)
		{
			ConfigFile = configfile;
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
		public static string MasterChannel { get; private set; }
		public static bool UseNickServ { get; private set; }
		public static string NickServPassword { get; private set; }
		public static bool UseHostServ { get; private set; }
		public static bool HostServEnabled { get; private set; }
		public static int MessageSending { get; private set; }
		public static string CommandPrefix { get; private set; }

		public IRCConfig(string server, int port, string nickname, string nickname2, string nickname3, string username, string masterchannel, bool usenickserv, string nickservpassword, bool usehostserv, bool hostservenabled, int messagesending, string commandprefix)
		{
			Server           = server;
			Port             = port;
			NickName         = nickname;
			NickName2        = nickname2;
			NickName3        = nickname3;
			UserName         = username;
			MasterChannel    = masterchannel;
			UseNickServ      = usenickserv;
			NickServPassword = nickservpassword;
			UseHostServ      = usehostserv;
			HostServEnabled  = hostservenabled;
			MessageSending   = messagesending;
			CommandPrefix    = commandprefix;
		}
	}

	public sealed class MySqlConfig
	{
		public static bool Enabled { get; private set; }
		public static string Host { get; private set; }
		public static string User { get; private set; }
		public static string Password { get; private set; }
		public static string Database { get; private set; }

		public MySqlConfig(bool enabled, string host, string user, string password, string database)
		{
			Enabled  = enabled;
			Host     = host;
			User     = user;
			Password = password;
			Database = database;
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
		public static string Directory { get; private set; }

		public AddonsConfig(bool enabled, string directory)
		{
			Enabled   = enabled;
			Directory = directory;
		}
	}
}
