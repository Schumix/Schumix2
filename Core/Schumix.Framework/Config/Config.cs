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
		public Config(string configfile)
		{
			try
			{
				Log.Debug("Config", ">> {0}", configfile);
				new SchumixConfig(configfile);

				if(!IsConfig(configfile))
				{
					Log.Notice("Config", "Program leallitasa!");
					Log.Notice("Config", "Kerlek toltsed ki a configot!");
					Thread.Sleep(200);
					Environment.Exit(1);
				}
				else
				{
					var xmldoc = new XmlDocument();
					xmldoc.Load(@configfile);

					Log.Notice("Config", "Config fajl betoltese...");
					string Server = xmldoc.SelectSingleNode("Schumix/Irc/Server").InnerText;
					int Port = Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Port").InnerText);
					string NickName = xmldoc.SelectSingleNode("Schumix/Irc/Nick").InnerText;
					string NickName2 = xmldoc.SelectSingleNode("Schumix/Irc/Nick2").InnerText;
					string NickName3 = xmldoc.SelectSingleNode("Schumix/Irc/Nick3").InnerText;
					string UserName = xmldoc.SelectSingleNode("Schumix/Irc/UserName").InnerText;
					bool UseNickServ = Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Allapot").InnerText);
					string NickServPassword = xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Jelszo").InnerText;
					bool UseHostServ = Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Allapot").InnerText);
					bool HostServAllapot = Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Vhost").InnerText);
					string Parancselojel = xmldoc.SelectSingleNode("Schumix/Parancs/Elojel").InnerText;

					new IRCConfig(Server, Port, NickName, NickName2, NickName3, UserName, UseNickServ, NickServPassword, UseHostServ, HostServAllapot, Parancselojel);

					string Host = xmldoc.SelectSingleNode("Schumix/Mysql/Host").InnerText;
					string User = xmldoc.SelectSingleNode("Schumix/Mysql/User").InnerText;
					string Password = xmldoc.SelectSingleNode("Schumix/Mysql/Password").InnerText;
					string Database = xmldoc.SelectSingleNode("Schumix/Mysql/Database").InnerText;

					new MysqlConfig(Host, User, Password, Database);

					int LogLevel = Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Log/LogLevel").InnerText);
					string LogHelye = xmldoc.SelectSingleNode("Schumix/Log/LogHelye").InnerText;
					bool IrcLog = Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Log/IrcLog").InnerText);

					new LogConfig(LogLevel, LogHelye, IrcLog);

					bool Allapot = Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Plugins/Allapot").InnerText);
					string Directory = xmldoc.SelectSingleNode("Schumix/Plugins/Directory").InnerText;

					new PluginsConfig(Allapot, Directory);

					Log.Success("Config", "Config adatbazis betoltve.");
					Console.WriteLine("");
				}
			}
			catch(Exception e)
			{
				new LogConfig(3, "szoba", false);
				Log.Error("Config", "Hiba oka: {0}", e);
			}
		}

		private bool IsConfig(string ConfigFile)
		{
			if(File.Exists(ConfigFile))
				return true;
			else
			{
				new LogConfig(3, "szoba", false);
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
					w.WriteElementString("Server", "irc.rizon.net");
					w.WriteElementString("Port", "6667");
					w.WriteElementString("Nick", "Schumix2");
					w.WriteElementString("Nick2", "_Schumix2");
					w.WriteElementString("Nick3", "_Schumix2");
					w.WriteElementString("UserName", "Schumix2");

					// <NickServ>
					w.WriteStartElement("NickServ");
					w.WriteElementString("Allapot", "false");
					w.WriteElementString("Jelszo", "pass");

					// </NickServ>
					w.WriteEndElement();

					// <HostServ>
					w.WriteStartElement("HostServ");
					w.WriteElementString("Allapot", "false");
					w.WriteElementString("Vhost", "false");

					// </HostServ>
					w.WriteEndElement();

					// </Irc>
					w.WriteEndElement();

					// <Log>
					w.WriteStartElement("Log");
					w.WriteElementString("LogLevel", "2");
					w.WriteElementString("LogHelye", "szoba");
					w.WriteElementString("IrcLog", "false");

					// </Log>
					w.WriteEndElement();

					// <Mysql>
					w.WriteStartElement("Mysql");
					w.WriteElementString("Host", "localhost");
					w.WriteElementString("User", "root");
					w.WriteElementString("Password", "pass");
					w.WriteElementString("Database", "database");

					// </Mysql>
					w.WriteEndElement();

					// <Parancs>
					w.WriteStartElement("Parancs");
					w.WriteElementString("Elojel", "$");

					// </Parancs>
					w.WriteEndElement();

					// <Plugins>
					w.WriteStartElement("Plugins");
					w.WriteElementString("Allapot", "true");
					w.WriteElementString("Directory", "plugins");

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
					Log.Error("Config", "Hiba az xml irasa soran: {0}", e);
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
		public static bool UseNickServ { get; private set; }
		public static string NickServPassword { get; private set; }
		public static bool UseHostServ { get; private set; }
		public static bool HostServAllapot { get; private set; }
		public static string Parancselojel { get; private set; }

		public IRCConfig(string server, int port, string nickname, string nickname2, string nickname3, string username, bool usenickserv, string nickservpassword, bool usehostserv, bool hostservallapot, string parancselojel)
		{
			Server           = server;
			Port             = port;
			NickName         = nickname;
			NickName2        = nickname2;
			NickName3        = nickname3;
			UserName         = username;
			UseNickServ      = usenickserv;
			NickServPassword = nickservpassword;
			UseHostServ      = usehostserv;
			HostServAllapot  = hostservallapot;
			Parancselojel    = parancselojel;
		}
	}

	public sealed class MysqlConfig
	{
		public static string Host { get; private set; }
		public static string User { get; private set; }
		public static string Password { get; private set; }
		public static string Database { get; private set; }

		public MysqlConfig(string host, string user, string password, string database)
		{
			Host     = host;
			User     = user;
			Password = password;
			Database = database;
		}
	}

	public sealed class LogConfig
	{
		public static int LogLevel { get; private set; }
		public static string LogHelye { get; private set; }
		public static bool IrcLog { get; private set; }

		public LogConfig(int loglevel, string loghelye, bool irclog)
		{
			LogLevel = loglevel;
			LogHelye = loghelye;
			IrcLog   = irclog;
		}
	}

	public sealed class PluginsConfig
	{
		public static bool Allapot { get; private set; }
		public static string Directory { get; private set; }

		public PluginsConfig(bool allapot, string directory)
		{
			Allapot   = allapot;
			Directory = directory;
		}
	}
}
