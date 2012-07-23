/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Megax <http://www.megaxx.info/>
 * Copyright (C) 2012 Jackneill
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
using System.Collections.Generic;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using YamlDotNet.Core;
using YamlDotNet.RepresentationModel;

namespace Schumix.Framework.Config
{
	public sealed class YamlConfig : DefaultConfig
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		public YamlConfig()
		{
		}

		public YamlConfig(string configdir, string configfile)
		{
			var yaml = new YamlStream();
			yaml.Load(File.OpenText(sUtilities.DirectoryToHome(configdir, configfile)));

			var schumixmap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey(new YamlScalarNode("Schumix"))) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children[new YamlScalarNode("Schumix")]).Children : (Dictionary<YamlNode, YamlNode>)null;
			LogMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Log"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Log")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			Log.Initialize(LogConfig.FileName);
			Log.Debug("YamlConfig", ">> {0}", configfile);

			Log.Notice("YamlConfig", sLConsole.Config("Text3"));
			ServerMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Server"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Server")]).Children : (Dictionary<YamlNode, YamlNode>)null);

			if((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Irc"))))
			{
				var list = new Dictionary<YamlNode, YamlNode>();

				foreach(var irc in schumixmap)
				{
					if(irc.Key.ToString().Contains("Irc"))
						list.Add(irc.Key, irc.Value);
				}

				IrcMap(list);
			}
			else
				IrcMap((Dictionary<YamlNode, YamlNode>)null);

			MySqlMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("MySql"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("MySql")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			SQLiteMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("SQLite"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("SQLite")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			AddonsMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Addon"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Addon")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			ScriptsMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Scripts"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Scripts")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			LocalizationMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Localization"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Localization")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			UpdateMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Update"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Update")]).Children : (Dictionary<YamlNode, YamlNode>)null);

			Log.Success("YamlConfig", sLConsole.Config("Text4"));
			Console.WriteLine();
		}

		~YamlConfig()
		{
		}

		public bool CreateConfig(string ConfigDirectory, string ConfigFile)
		{
			try
			{
				string filename = sUtilities.DirectoryToHome(ConfigDirectory, ConfigFile);

				if(File.Exists(filename))
					return true;
				else
				{
					new LogConfig(d_logfilename, 3, d_logdirectory, d_irclogdirectory, d_irclog);
					Log.Initialize(d_logfilename);
					Log.Error("YamlConfig", sLConsole.Config("Text5"));
					Log.Debug("YamlConfig", sLConsole.Config("Text6"));
					var yaml = new YamlStream();
					string filename2 = sUtilities.DirectoryToHome(ConfigDirectory, "_" + ConfigFile);

					if(File.Exists(filename2))
						yaml.Load(File.OpenText(filename2));

					try
					{
						var schumixmap = (yaml.Documents.Count > 0 && ((YamlMappingNode)yaml.Documents[0].RootNode).Children.ContainsKey(new YamlScalarNode("Schumix"))) ? ((YamlMappingNode)((YamlMappingNode)yaml.Documents[0].RootNode).Children[new YamlScalarNode("Schumix")]).Children : (Dictionary<YamlNode, YamlNode>)null;
						string data = "# Schumix config file (yaml)\n";
						var nodes = new System.Yaml.YamlMapping
						(
							"Schumix",
							new System.Yaml.YamlMapping
							(
								"Server", CreateServerMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Server"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Server")]).Children : (Dictionary<YamlNode, YamlNode>)null),
								//"Irc", CreateIrcMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Irc"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Irc")]).Children : (Dictionary<YamlNode, YamlNode>)null),
								"Log", CreateLogMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Log"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Log")]).Children : (Dictionary<YamlNode, YamlNode>)null)
							)
						);

			/*if((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Irc"))))
			{
				var list = new Dictionary<YamlNode, YamlNode>();

				foreach(var irc in schumixmap)
				{
					if(irc.Key.ToString().Contains("Irc"))
						list.Add(irc.Key, irc.Value);
				}

				IrcMap(list);
			}
			else
				IrcMap((Dictionary<YamlNode, YamlNode>)null);

			MySqlMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("MySql"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("MySql")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			SQLiteMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("SQLite"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("SQLite")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			AddonsMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Addon"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Addon")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			ScriptsMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Scripts"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Scripts")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			LocalizationMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Localization"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Localization")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			UpdateMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Update"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Update")]).Children : (Dictionary<YamlNode, YamlNode>)null);
*/
						data += nodes.ToYaml();
						sUtilities.CreateFile(filename);
						var file = new StreamWriter(filename, true) { AutoFlush = true };
						file.Write(data);
						file.Close();

						//if(File.Exists(filename2))
						//	File.Delete(filename2);

						Log.Success("YamlConfig", sLConsole.Config("Text7"));
						return false;
					}
					catch(Exception e)
					{
						Log.Error("YamlConfig", sLConsole.Config("Text8"), e);
						error = true;
						return false;
					}
				}
			}
			catch(DirectoryNotFoundException)
			{
				CreateConfig(ConfigDirectory, ConfigFile);
			}

			return true;
		}

		private void LogMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			string LogFileName = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("FileName"))) ? nodes[new YamlScalarNode("FileName")].ToString() : d_logfilename;
			int LogLevel = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("LogLevel"))) ? Convert.ToInt32(nodes[new YamlScalarNode("LogLevel")].ToString()) : d_loglevel;
			string LogDirectory = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("LogDirectory"))) ? nodes[new YamlScalarNode("LogDirectory")].ToString() : d_logdirectory;
			string IrcLogDirectory = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("IrcLogDirectory"))) ? nodes[new YamlScalarNode("IrcLogDirectory")].ToString() : d_irclogdirectory;
			bool IrcLog = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("IrcLog"))) ? Convert.ToBoolean(nodes[new YamlScalarNode("IrcLog")].ToString()) : d_irclog;

			new LogConfig(LogFileName, LogLevel, sUtilities.GetHomeDirectory(LogDirectory), sUtilities.GetHomeDirectory(IrcLogDirectory), IrcLog);
		}

		private void ServerMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool ServerEnabled = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Enabled"))) ? Convert.ToBoolean(nodes[new YamlScalarNode("Enabled")].ToString()) : d_serverenabled;
			string ServerHost = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Host"))) ? nodes[new YamlScalarNode("Host")].ToString() : d_serverhost;
			int ServerPort = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Port"))) ? Convert.ToInt32(nodes[new YamlScalarNode("Port")].ToString()) : d_serverport;
			string ServerPassword = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Password"))) ? nodes[new YamlScalarNode("Password")].ToString() : d_serverpassword;

			new ServerConfig(ServerEnabled, ServerHost, ServerPort, ServerPassword);
		}

		private void IrcMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			int ServerId = 1;
			var IrcList = new Dictionary<string, IRCConfigBase>();

			if(nodes.IsNull())
			{
				string ServerName            = d_servername;
				string Server                = d_server;
				int Port                     = d_port;
				bool Ssl                     = d_ssl;
				string NickName              = d_nickname;
				string NickName2             = d_nickname2;
				string NickName3             = d_nickname3;
				string UserName              = d_username;
				string UserInfo              = d_userinfo;
				string MasterChannel         = d_masterchannel;
				string MasterChannelPassword = d_masterchannelpassword;
				string IgnoreChannels        = d_ignorechannels;
				string IgnoreNames           = d_ignorenames;
				bool UseNickServ             = d_usenickserv;
				string NickServPassword      = d_nickservpassword;
				bool UseHostServ             = d_usehostserv;
				bool HostServStatus          = d_hostservstatus;
				int MessageSending           = d_messagesending;
				string CommandPrefix         = d_commandprefix;
				string MessageType           = d_messagetype;

				IrcList.Add(ServerName.ToLower(), new IRCConfigBase(ServerId, Server, Port, Ssl, NickName, NickName2, NickName3, UserName, UserInfo, MasterChannel, MasterChannelPassword.Trim(), IgnoreChannels, IgnoreNames, UseNickServ, NickServPassword, UseHostServ, HostServStatus, MessageSending, CommandPrefix, MessageType));
			}
			else
			{
				foreach(var irc in nodes)
				{
					var node = ((YamlMappingNode)irc.Value).Children;
					string ServerName = (!node.IsNull() && node.ContainsKey(new YamlScalarNode("ServerName"))) ? node[new YamlScalarNode("ServerName")].ToString() : d_servername;
					string Server = (!node.IsNull() && node.ContainsKey(new YamlScalarNode("Server"))) ? node[new YamlScalarNode("Server")].ToString() : d_server;
					int Port = (!node.IsNull() && node.ContainsKey(new YamlScalarNode("Port"))) ? Convert.ToInt32(node[new YamlScalarNode("Port")].ToString()) : d_port;
					bool Ssl = (!node.IsNull() && node.ContainsKey(new YamlScalarNode("Ssl"))) ? Convert.ToBoolean(node[new YamlScalarNode("Ssl")].ToString()) : d_ssl;
					string NickName = (!node.IsNull() && node.ContainsKey(new YamlScalarNode("NickName"))) ? node[new YamlScalarNode("NickName")].ToString() : d_nickname;
					string NickName2 = (!node.IsNull() && node.ContainsKey(new YamlScalarNode("NickName2"))) ? node[new YamlScalarNode("NickName2")].ToString() : d_nickname2;
					string NickName3 = (!node.IsNull() && node.ContainsKey(new YamlScalarNode("NickName3"))) ? node[new YamlScalarNode("NickName3")].ToString() : d_nickname3;
					string UserName = (!node.IsNull() && node.ContainsKey(new YamlScalarNode("UserName"))) ? node[new YamlScalarNode("UserName")].ToString() : d_username;
					string UserInfo = (!node.IsNull() && node.ContainsKey(new YamlScalarNode("UserInfo"))) ? node[new YamlScalarNode("UserInfo")].ToString() : d_userinfo;

					string MasterChannel = d_masterchannel;
					string MasterChannelPassword = d_masterchannelpassword;

					if(!node.IsNull() && node.ContainsKey(new YamlScalarNode("MasterChannel")))
					{
						var node2 = ((YamlMappingNode)node[new YamlScalarNode("MasterChannel")]).Children;
						MasterChannel = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Name"))) ? node2[new YamlScalarNode("Name")].ToString() : d_masterchannel;
						MasterChannelPassword = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Password"))) ? node2[new YamlScalarNode("Password")].ToString() : d_masterchannelpassword;
					}

					string IgnoreChannels = (!node.IsNull() && node.ContainsKey(new YamlScalarNode("IgnoreChannels"))) ? node[new YamlScalarNode("IgnoreChannels")].ToString() : d_ignorechannels;
					string IgnoreNames = (!node.IsNull() && node.ContainsKey(new YamlScalarNode("IgnoreNames"))) ? node[new YamlScalarNode("IgnoreNames")].ToString() : d_ignorenames;

					bool UseNickServ = d_usenickserv;
					string NickServPassword = d_nickservpassword;

					if(!node.IsNull() && node.ContainsKey(new YamlScalarNode("NickServ")))
					{
						var node2 = ((YamlMappingNode)node[new YamlScalarNode("NickServ")]).Children;
						UseNickServ = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Enabled"))) ? Convert.ToBoolean(node2[new YamlScalarNode("Enabled")].ToString()) : d_usenickserv;
						NickServPassword = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Password"))) ? node2[new YamlScalarNode("Password")].ToString() : d_nickservpassword;
					}

					bool UseHostServ = d_usehostserv;
					bool HostServStatus = d_hostservstatus;

					if(!node.IsNull() && node.ContainsKey(new YamlScalarNode("HostServ")))
					{
						var node2 = ((YamlMappingNode)node[new YamlScalarNode("HostServ")]).Children;
						UseHostServ = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Enabled"))) ? Convert.ToBoolean(node2[new YamlScalarNode("Enabled")].ToString()) : d_usehostserv;
						HostServStatus = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Vhost"))) ? Convert.ToBoolean(node2[new YamlScalarNode("Vhost")].ToString()) : d_hostservstatus;
					}

					int MessageSending = d_messagesending;

					if(!node.IsNull() && node.ContainsKey(new YamlScalarNode("Wait")))
					{
						var node2 = ((YamlMappingNode)node[new YamlScalarNode("Wait")]).Children;
						MessageSending = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("MessageSending"))) ? Convert.ToInt32(node2[new YamlScalarNode("MessageSending")].ToString()) : d_messagesending;
					}

					string CommandPrefix = d_commandprefix;

					if(!node.IsNull() && node.ContainsKey(new YamlScalarNode("Command")))
					{
						var node2 = ((YamlMappingNode)node[new YamlScalarNode("Command")]).Children;
						CommandPrefix = (!node2.IsNull() && node2.ContainsKey(new YamlScalarNode("Prefix"))) ? node2[new YamlScalarNode("Prefix")].ToString() : d_commandprefix;
					}

					string MessageType = (!node.IsNull() && node.ContainsKey(new YamlScalarNode("MessageType"))) ? node[new YamlScalarNode("MessageType")].ToString() : d_messagetype;

					if(MasterChannel.Length >= 2 && MasterChannel.Trim().Length > 1 && MasterChannel.Substring(0, 1) != "#")
						MasterChannel = "#" + MasterChannel;
					else if(MasterChannel.Length < 2 && MasterChannel.Trim().Length <= 1)
						MasterChannel = d_masterchannel;

					if(IrcList.ContainsKey(ServerName.ToLower()))
						Log.Error("YmlConfig", sLConsole.Config("Text12"), ServerName);
					else
					{
						IrcList.Add(ServerName.ToLower(), new IRCConfigBase(ServerId, Server, Port, Ssl, NickName, NickName2, NickName3, UserName, UserInfo, MasterChannel, MasterChannelPassword.Trim(), IgnoreChannels, IgnoreNames, UseNickServ, NickServPassword, UseHostServ, HostServStatus, MessageSending, CommandPrefix, MessageType));
						ServerId++;
					}
				}
			}

			new IRCConfig(IrcList);
		}

		private void MySqlMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool Enabled = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Enabled"))) ? Convert.ToBoolean(nodes[new YamlScalarNode("Enabled")].ToString()) : d_mysqlenabled;
			string Host = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Host"))) ? nodes[new YamlScalarNode("Host")].ToString() : d_mysqlhost;
			string User = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("User"))) ? nodes[new YamlScalarNode("User")].ToString() : d_mysqluser;
			string Password = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Password"))) ? nodes[new YamlScalarNode("Password")].ToString() : d_mysqlpassword;
			string Database = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Database"))) ? nodes[new YamlScalarNode("Database")].ToString() : d_mysqldatabase;
			string Charset = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Charset"))) ? nodes[new YamlScalarNode("Charset")].ToString() : d_mysqlcharset;

			new MySqlConfig(Enabled, Host, User, Password, Database, Charset);
		}

		private void SQLiteMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool Enabled = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Enabled"))) ? Convert.ToBoolean(nodes[new YamlScalarNode("Enabled")].ToString()) : d_sqliteenabled;
			string FileName = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("FileName"))) ? nodes[new YamlScalarNode("FileName")].ToString() : d_sqlitefilename;

			new SQLiteConfig(Enabled, sUtilities.GetHomeDirectory(FileName));
		}

		private void AddonsMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool Enabled = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Enabled"))) ? Convert.ToBoolean(nodes[new YamlScalarNode("Enabled")].ToString()) : d_addonenabled;
			string Ignore = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Ignore"))) ? nodes[new YamlScalarNode("Ignore")].ToString() : d_addonignore;
			string Directory = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Directory"))) ? nodes[new YamlScalarNode("Directory")].ToString() : d_addondirectory;

			new AddonsConfig(Enabled, Ignore, Directory);
		}

		private void ScriptsMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool Lua = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Lua"))) ? Convert.ToBoolean(nodes[new YamlScalarNode("Lua")].ToString()) : d_scriptenabled;
			string Directory = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Directory"))) ? nodes[new YamlScalarNode("Directory")].ToString() : d_scriptdirectory;

			new ScriptsConfig(Lua, sUtilities.GetHomeDirectory(Directory));
		}

		private void LocalizationMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			string Locale = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Locale"))) ? nodes[new YamlScalarNode("Locale")].ToString() : d_locale;

			new LocalizationConfig(Locale);
		}

		private void UpdateMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			bool Enabled = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Enabled"))) ? Convert.ToBoolean(nodes[new YamlScalarNode("Enabled")].ToString()) : d_updateenabled;
			string Version = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Version"))) ? nodes[new YamlScalarNode("Version")].ToString() : d_updateversion;
			string Branch = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Branch"))) ? nodes[new YamlScalarNode("Branch")].ToString() : d_updatebranch;
			string WebPage = (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("WebPage"))) ? nodes[new YamlScalarNode("WebPage")].ToString() : d_updatewebpage;

			new UpdateConfig(Enabled, Version.ToLower(), Branch, WebPage);
		}

		private System.Yaml.YamlMapping CreateServerMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new System.Yaml.YamlMapping();
			map.Add("Enabled",  (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Enabled"))) ? Convert.ToBoolean(nodes[new YamlScalarNode("Enabled")].ToString()) : d_serverenabled);
			map.Add("Host",     (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Host"))) ? nodes[new YamlScalarNode("Host")].ToString() : d_serverhost);
			map.Add("Port",     (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Port"))) ? Convert.ToInt32(nodes[new YamlScalarNode("Port")].ToString()) : d_serverport);
			map.Add("Password", (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("Password"))) ? nodes[new YamlScalarNode("Password")].ToString() : d_serverpassword);
			return map;
		}

		private System.Yaml.YamlMapping CreateLogMap(IDictionary<YamlNode, YamlNode> nodes)
		{
			var map = new System.Yaml.YamlMapping();
			map.Add("FileName",        (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("FileName"))) ? nodes[new YamlScalarNode("FileName")].ToString() : d_logfilename);
			map.Add("LogLevel",        (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("LogLevel"))) ? Convert.ToInt32(nodes[new YamlScalarNode("LogLevel")].ToString()) : d_loglevel);
			map.Add("LogDirectory",    (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("LogDirectory"))) ? nodes[new YamlScalarNode("LogDirectory")].ToString() : d_logdirectory);
			map.Add("IrcLogDirectory", (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("IrcLogDirectory"))) ? nodes[new YamlScalarNode("IrcLogDirectory")].ToString() : d_irclogdirectory);
			map.Add("IrcLog",          (!nodes.IsNull() && nodes.ContainsKey(new YamlScalarNode("IrcLog"))) ? Convert.ToBoolean(nodes[new YamlScalarNode("IrcLog")].ToString()) : d_irclog);
			return map;
		}
	}
}