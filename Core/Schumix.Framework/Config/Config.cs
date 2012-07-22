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
                            new YAMLConfig(configdir, configfile);
                            break;
                        case 1:
                            new XMLConfig(configdir, configfile);
                            break;
                        default:
                            new YAMLConfig(configdir, configfile);
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



			try
			{
				string filename = sUtilities.DirectoryToHome(ConfigDirectory, ConfigFile);

				if(File.Exists(filename))
					return true;
				else
				{
					new LogConfig(_logfilename, 3, _logdirectory, _irclogdirectory, _irclog);
					Log.Initialize(_logfilename);
					Log.Error("Config", sLConsole.Config("Text5"));
					Log.Debug("Config", sLConsole.Config("Text6"));
					var w = new XmlTextWriter(filename, null);
					var xmldoc = new XmlDocument();
					string filename2 = sUtilities.DirectoryToHome(ConfigDirectory, "_" + ConfigFile);

					if(File.Exists(filename2))
						xmldoc.Load(filename2);

					try
					{
						w.Formatting = Formatting.Indented;
						w.Indentation = 4;
						w.Namespaces = false;
						w.WriteStartDocument();

						// <Schumix>
						w.WriteStartElement("Schumix");

						// <Server>
						w.WriteStartElement("Server");
						w.WriteElementString("Enabled",         (!xmldoc.SelectSingleNode("Schumix/Server/Enabled").IsNull() ? xmldoc.SelectSingleNode("Schumix/Server/Enabled").InnerText : _serverenabled.ToString()));
						w.WriteElementString("Host",            (!xmldoc.SelectSingleNode("Schumix/Server/Host").IsNull() ? xmldoc.SelectSingleNode("Schumix/Server/Host").InnerText : _serverhost));
						w.WriteElementString("Port",            (!xmldoc.SelectSingleNode("Schumix/Server/Port").IsNull() ? xmldoc.SelectSingleNode("Schumix/Server/Port").InnerText : _serverport.ToString()));
						w.WriteElementString("Password",        (!xmldoc.SelectSingleNode("Schumix/Server/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/Server/Password").InnerText : _serverpassword));

						// </Server>
						w.WriteEndElement();

						var xmlirclist = xmldoc.SelectNodes("Schumix/Irc");

						if(xmlirclist.Count == 0)
						{
							// <Irc>
							w.WriteStartElement("Irc");
							w.WriteElementString("ServerName",      (!xmldoc.SelectSingleNode("Schumix/Irc/ServerName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/ServerName").InnerText : _servername));
							w.WriteElementString("Server",          (!xmldoc.SelectSingleNode("Schumix/Irc/Server").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Server").InnerText : _server));
							w.WriteElementString("Port",            (!xmldoc.SelectSingleNode("Schumix/Irc/Port").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Port").InnerText : _port.ToString()));
							w.WriteElementString("Ssl",             (!xmldoc.SelectSingleNode("Schumix/Irc/Ssl").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Ssl").InnerText : _ssl.ToString()));
							w.WriteElementString("NickName",        (!xmldoc.SelectSingleNode("Schumix/Irc/NickName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName").InnerText : _nickname));
							w.WriteElementString("NickName2",       (!xmldoc.SelectSingleNode("Schumix/Irc/NickName2").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName2").InnerText : _nickname2));
							w.WriteElementString("NickName3",       (!xmldoc.SelectSingleNode("Schumix/Irc/NickName3").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName3").InnerText : _nickname3));
							w.WriteElementString("UserName",        (!xmldoc.SelectSingleNode("Schumix/Irc/UserName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/UserName").InnerText : _username));
							w.WriteElementString("UserInfo",        (!xmldoc.SelectSingleNode("Schumix/Irc/UserInfo").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/UserInfo").InnerText : _userinfo));

							// <MasterChannel>
							w.WriteStartElement("MasterChannel");
							w.WriteElementString("Name",            (!xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel/Name").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel/Name").InnerText : _masterchannel));
							w.WriteElementString("Password",        (!xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel/Password").InnerText : _masterchannelpassword));

							// </MasterChannel>
							w.WriteEndElement();

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

							w.WriteElementString("MessageType",     (!xmldoc.SelectSingleNode("Schumix/Irc/MessageType").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/MessageType").InnerText : _messagetype));

							// </Irc>
							w.WriteEndElement();
						}
						else
						{
							foreach(XmlNode xn in xmlirclist)
							{
								// <Irc>
								w.WriteStartElement("Irc");
								w.WriteElementString("ServerName",      (!xn["ServerName"].IsNull() ? xn["ServerName"].InnerText : _servername));
								w.WriteElementString("Server",          (!xn["Server"].IsNull() ? xn["Server"].InnerText : _server));
								w.WriteElementString("Port",            (!xn["Port"].IsNull() ? xn["Port"].InnerText : _port.ToString()));
								w.WriteElementString("Ssl",             (!xn["Ssl"].IsNull() ? xn["Ssl"].InnerText : _ssl.ToString()));
								w.WriteElementString("NickName",        (!xn["NickName"].IsNull() ? xn["NickName"].InnerText : _nickname));
								w.WriteElementString("NickName2",       (!xn["NickName2"].IsNull() ? xn["NickName2"].InnerText : _nickname2));
								w.WriteElementString("NickName3",       (!xn["NickName3"].IsNull() ? xn["NickName3"].InnerText : _nickname3));
								w.WriteElementString("UserName",        (!xn["UserName"].IsNull() ? xn["UserName"].InnerText : _username));
								w.WriteElementString("UserInfo",        (!xn["UserInfo"].IsNull() ? xn["UserInfo"].InnerText : _userinfo));

								// <MasterChannel>
								w.WriteStartElement("MasterChannel");
								w.WriteElementString("Name",            (!xn.SelectSingleNode("MasterChannel/Name").IsNull() ? xn.SelectSingleNode("MasterChannel/Name").InnerText : _masterchannel));
								w.WriteElementString("Password",        (!xn.SelectSingleNode("MasterChannel/Password").IsNull() ? xn.SelectSingleNode("MasterChannel/Password").InnerText : _masterchannelpassword));

								// </MasterChannel>
								w.WriteEndElement();

								w.WriteElementString("IgnoreChannels",  (!xn["IgnoreChannels"].IsNull() ? xn["IgnoreChannels"].InnerText : _ignorechannels));
								w.WriteElementString("IgnoreNames",     (!xn["IgnoreNames"].IsNull() ? xn["IgnoreNames"].InnerText : _ignorenames));

								// <NickServ>
								w.WriteStartElement("NickServ");
								w.WriteElementString("Enabled",         (!xn.SelectSingleNode("NickServ/Enabled").IsNull() ? xn.SelectSingleNode("NickServ/Enabled").InnerText : _usenickserv.ToString()));
								w.WriteElementString("Password",        (!xn.SelectSingleNode("NickServ/Password").IsNull() ? xn.SelectSingleNode("NickServ/Password").InnerText : _nickservpassword));

								// </NickServ>
								w.WriteEndElement();

								// <HostServ>
								w.WriteStartElement("HostServ");
								w.WriteElementString("Enabled",         (!xn.SelectSingleNode("HostServ/Enabled").IsNull() ? xn.SelectSingleNode("HostServ/Enabled").InnerText : _usehostserv.ToString()));
								w.WriteElementString("Vhost",           (!xn.SelectSingleNode("HostServ/Vhost").IsNull() ? xn.SelectSingleNode("HostServ/Vhost").InnerText : _hostservstatus.ToString()));

								// </HostServ>
								w.WriteEndElement();

								// <Wait>
								w.WriteStartElement("Wait");
								w.WriteElementString("MessageSending",  (!xn.SelectSingleNode("Wait/MessageSending").IsNull() ? xn.SelectSingleNode("Wait/MessageSending").InnerText : _messagesending.ToString()));

								// </Wait>
								w.WriteEndElement();

								// <Command>
								w.WriteStartElement("Command");
								w.WriteElementString("Prefix",          (!xn.SelectSingleNode("Command/Prefix").IsNull() ? xn.SelectSingleNode("Command/Prefix").InnerText : _commandprefix));

								// </Command>
								w.WriteEndElement();

								w.WriteElementString("MessageType",     (!xn["MessageType"].IsNull() ? xn["MessageType"].InnerText : _messagetype));

								// </Irc>
								w.WriteEndElement();
							}
						}

						// <Log>
						w.WriteStartElement("Log");
						w.WriteElementString("FileName",        (!xmldoc.SelectSingleNode("Schumix/Log/FileName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Log/FileName").InnerText : _logfilename));
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
						w.WriteElementString("Version",         (!xmldoc.SelectSingleNode("Schumix/Update/Version").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/Version").InnerText : _updateversion));
						w.WriteElementString("Branch",          (!xmldoc.SelectSingleNode("Schumix/Update/Branch").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/Branch").InnerText : _updatebranch));
						w.WriteElementString("WebPage",         (!xmldoc.SelectSingleNode("Schumix/Update/WebPage").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/WebPage").InnerText : _updatewebpage));

						// </Update>
						w.WriteEndElement();

						// </Schumix>
						w.WriteEndElement();

						w.Flush();
						w.Close();

						if(File.Exists(filename2))
							File.Delete(filename2);

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
}