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
using System.Xml;

namespace Schumix.Config
{
	public class Config
	{
		public Config(string configfile)
		{
			Log.Debug("Config", ">> schumix.xml");
			SchumixConfig.ConfigFile = configfile;
			var xmldoc = new XmlDocument();
			xmldoc.Load(@SchumixConfig.ConfigFile);

			Log.Notice("Config", "Config fajl betoltese...");
			IRCConfig.Server = xmldoc.SelectSingleNode("Schumix/Irc/Server").InnerText;
			IRCConfig.Port = Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Port").InnerText);
			IRCConfig.NickName = xmldoc.SelectSingleNode("Schumix/Irc/Nick").InnerText;
			IRCConfig.NickName2 = xmldoc.SelectSingleNode("Schumix/Irc/Nick2").InnerText;
			IRCConfig.NickName3 = xmldoc.SelectSingleNode("Schumix/Irc/Nick3").InnerText;
			IRCConfig.UserName = xmldoc.SelectSingleNode("Schumix/Irc/UserName").InnerText;
			IRCConfig.UseNickServ = Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/UseNickServ/Allapot").InnerText);
			IRCConfig.NickServPassword = xmldoc.SelectSingleNode("Schumix/Irc/UseNickServ/Jelszo").InnerText;

			MysqlConfig.Host = xmldoc.SelectSingleNode("Schumix/Mysql/Host").InnerText;
			MysqlConfig.User = xmldoc.SelectSingleNode("Schumix/Mysql/User").InnerText;
			MysqlConfig.Password = xmldoc.SelectSingleNode("Schumix/Mysql/Password").InnerText;
			MysqlConfig.Database = xmldoc.SelectSingleNode("Schumix/Mysql/Database").InnerText;

			IRCConfig.Parancselojel = xmldoc.SelectSingleNode("Schumix/Parancs/Elojel").InnerText;

			Log.Success("Config", "Config adatbazis betoltve.");
			Console.WriteLine("");
		}
	}

	public class SchumixConfig
	{
		private static string _ConfigFile;
		public static string ConfigFile
		{
			get
			{
				return _ConfigFile;
			}
			set
			{
				_ConfigFile = value;
			}
		}
	}

	public class IRCConfig
	{
		private static string _Server;
		private static int _Port;
		private static string _NickName;
		private static string _NickName2;
		private static string _NickName3;
		private static string _UserName;
		private static int _UseNickServ;
		private static string _NickServPassword;
		private static string _Parancselojel;

		public static string Server
		{
			get
			{
				return _Server;
			}
			set
			{
				_Server = value;
			}
		}

		public static int Port
		{
			get
			{
				return _Port;
			}
			set
			{
				_Port = value;
			}
		}

		public static string NickName
		{
			get
			{
				return _NickName;
			}
			set
			{
				_NickName = value;
			}
		}

		public static string NickName2
		{
			get
			{
				return _NickName2;
			}
			set
			{
				_NickName2 = value;
			}
		}

		public static string NickName3
		{
			get
			{
				return _NickName3;
			}
			set
			{
				_NickName3 = value;
			}
		}

		public static string UserName
		{
			get
			{
				return _UserName;
			}
			set
			{
				_UserName = value;
			}
		}

		public static int UseNickServ
		{
			get
			{
				return _UseNickServ;
			}
			set
			{
				_UseNickServ = value;
			}
		}

		public static string NickServPassword
		{
			get
			{
				return _NickServPassword;
			}
			set
			{
				_NickServPassword = value;
			}
		}

		public static string Parancselojel
		{
			get
			{
				return _Parancselojel;
			}
			set
			{
				_Parancselojel = value;
			}
		}
	}

	public class MysqlConfig
	{
		private static string _Host;
		private static string _User;
		private static string _Password;
		private static string _Database;

		public static string Host
		{
			get
			{
				return _Host;
			}
			set
			{
				_Host = value;
			}
		}

		public static string User
		{
			get
			{
				return _User;
			}
			set
			{
				_User = value;
			}
		}

		public static string Password
		{
			get
			{
				return _Password;
			}
			set
			{
				_Password = value;
			}
		}

		public static string Database
		{
			get
			{
				return _Database;
			}
			set
			{
				_Database = value;
			}
		}
	}
}
