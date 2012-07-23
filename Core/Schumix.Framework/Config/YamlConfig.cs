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

			// IrcMap
			MySqlMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("MySql"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("MySql")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			SQLiteMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("SQLite"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("SQLite")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			AddonsMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Addons"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Addons")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			ScriptsMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Scripts"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Scripts")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			LocalizationMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Localization"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Localization")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			UpdateMap((!schumixmap.IsNull() && schumixmap.ContainsKey(new YamlScalarNode("Update"))) ? ((YamlMappingNode)schumixmap[new YamlScalarNode("Update")]).Children : (Dictionary<YamlNode, YamlNode>)null);
			/*

			int ServerId = 1;
			var xmlirclist = xmldoc.SelectNodes("Schumix/Irc");
			var IrcList = new Dictionary<string, IRCConfigBase>();

			if(xmlirclist.Count == 0)
			{
				string ServerName = !xmldoc.SelectSingleNode("Schumix/Irc/ServerName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/ServerName").InnerText : d_servername;
				string Server = !xmldoc.SelectSingleNode("Schumix/Irc/Server").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Server").InnerText : d_server;
				int Port = !xmldoc.SelectSingleNode("Schumix/Irc/Port").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Port").InnerText) : d_port;
				bool Ssl = !xmldoc.SelectSingleNode("Schumix/Irc/Ssl").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/Ssl").InnerText) : d_ssl;
				string NickName = !xmldoc.SelectSingleNode("Schumix/Irc/NickName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName").InnerText : d_nickname;
				string NickName2 = !xmldoc.SelectSingleNode("Schumix/Irc/NickName2").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName2").InnerText : d_nickname2;
				string NickName3 = !xmldoc.SelectSingleNode("Schumix/Irc/NickName3").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName3").InnerText : d_nickname3;
				string UserName = !xmldoc.SelectSingleNode("Schumix/Irc/UserName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/UserName").InnerText : d_username;
				string UserInfo = !xmldoc.SelectSingleNode("Schumix/Irc/UserInfo").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/UserInfo").InnerText : d_userinfo;
				string MasterChannel = !xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel/Name").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel/Name").InnerText : d_masterchannel;
				string MasterChannelPassword = !xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel/Password").InnerText : d_masterchannelpassword;
				string IgnoreChannels = !xmldoc.SelectSingleNode("Schumix/Irc/IgnoreChannels").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/IgnoreChannels").InnerText : d_ignorechannels;
				string IgnoreNames = !xmldoc.SelectSingleNode("Schumix/Irc/IgnoreNames").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/IgnoreNames").InnerText : d_ignorenames;
				bool UseNickServ = !xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Enabled").InnerText) : d_usenickserv;
				string NickServPassword = !xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Password").InnerText : d_nickservpassword;
				bool UseHostServ = !xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Enabled").InnerText) : d_usehostserv;
				bool HostServStatus = !xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Vhost").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Vhost").InnerText) : d_hostservstatus;
				int MessageSending = !xmldoc.SelectSingleNode("Schumix/Irc/Wait/MessageSending").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Wait/MessageSending").InnerText) : d_messagesending;
				string CommandPrefix = !xmldoc.SelectSingleNode("Schumix/Irc/Command/Prefix").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Command/Prefix").InnerText : d_commandprefix;
				string MessageType = !xmldoc.SelectSingleNode("Schumix/Irc/MessageType").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/MessageType").InnerText : d_messagetype;

				if(MasterChannel.Length >= 2 && MasterChannel.Trim().Length > 1 && MasterChannel.Substring(0, 1) != "#")
					MasterChannel = "#" + MasterChannel;
				else if(MasterChannel.Length < 2 && MasterChannel.Trim().Length <= 1)
					MasterChannel = d_masterchannel;

				IrcList.Add(ServerName.ToLower(), new IRCConfigBase(ServerId, Server, Port, Ssl, NickName, NickName2, NickName3, UserName, UserInfo, MasterChannel, MasterChannelPassword.Trim(), IgnoreChannels, IgnoreNames, UseNickServ, NickServPassword, UseHostServ, HostServStatus, MessageSending, CommandPrefix, MessageType));
			}
			else
			{
				foreach (XmlNode xn in xmlirclist)
				{
					string ServerName = !xn["ServerName"].IsNull() ? xn["ServerName"].InnerText : d_servername;
					string Server = !xn["Server"].IsNull() ? xn["Server"].InnerText : d_server;
					int Port = !xn["Port"].IsNull() ? Convert.ToInt32(xn["Port"].InnerText) : d_port;
					bool Ssl = !xn["Ssl"].IsNull() ? Convert.ToBoolean(xn["Ssl"].InnerText) : d_ssl;
					string NickName = !xn["NickName"].IsNull() ? xn["NickName"].InnerText : d_nickname;
					string NickName2 = !xn["NickName2"].IsNull() ? xn["NickName2"].InnerText : d_nickname2;
					string NickName3 = !xn["NickName3"].IsNull() ? xn["NickName3"].InnerText : d_nickname3;
					string UserName = !xn["UserName"].IsNull() ? xn["UserName"].InnerText : d_username;
					string UserInfo = !xn["UserInfo"].IsNull() ? xn["UserInfo"].InnerText : d_userinfo;
					string MasterChannel = !xn.SelectSingleNode("MasterChannel/Name").IsNull() ? xn.SelectSingleNode("MasterChannel/Name").InnerText : d_masterchannel;
					string MasterChannelPassword = !xn.SelectSingleNode("MasterChannel/Password").IsNull() ? xn.SelectSingleNode("MasterChannel/Password").InnerText : d_masterchannelpassword;
					string IgnoreChannels = !xn["IgnoreChannels"].IsNull() ? xn["IgnoreChannels"].InnerText : d_ignorechannels;
					string IgnoreNames = !xn["IgnoreNames"].IsNull() ? xn["IgnoreNames"].InnerText : d_ignorenames;
					bool UseNickServ = !xn.SelectSingleNode("NickServ/Enabled").IsNull() ? Convert.ToBoolean(xn.SelectSingleNode("NickServ/Enabled").InnerText) : d_usenickserv;
					string NickServPassword = !xn.SelectSingleNode("NickServ/Password").IsNull() ? xn.SelectSingleNode("NickServ/Password").InnerText : d_nickservpassword;
					bool UseHostServ = !xn.SelectSingleNode("HostServ/Enabled").IsNull() ? Convert.ToBoolean(xn.SelectSingleNode("HostServ/Enabled").InnerText) : d_usehostserv;
					bool HostServStatus = !xn.SelectSingleNode("HostServ/Vhost").IsNull() ? Convert.ToBoolean(xn.SelectSingleNode("HostServ/Vhost").InnerText) : d_hostservstatus;
					int MessageSending = !xn.SelectSingleNode("Wait/MessageSending").IsNull() ? Convert.ToInt32(xn.SelectSingleNode("Wait/MessageSending").InnerText) : d_messagesending;
					string CommandPrefix = !xn.SelectSingleNode("Command/Prefix").IsNull() ? xn.SelectSingleNode("Command/Prefix").InnerText : d_commandprefix;
					string MessageType = !xn["MessageType"].IsNull() ? xn["MessageType"].InnerText : d_messagetype;

					if(MasterChannel.Length >= 2 && MasterChannel.Trim().Length > 1 && MasterChannel.Substring(0, 1) != "#")
						MasterChannel = "#" + MasterChannel;
					else if(MasterChannel.Length < 2 && MasterChannel.Trim().Length <= 1)
						MasterChannel = d_masterchannel;

					if(IrcList.ContainsKey(ServerName.ToLower()))
						Log.Error("XmlConfig", sLConsole.Config("Text12"), ServerName);
					else
					{
						IrcList.Add(ServerName.ToLower(), new IRCConfigBase(ServerId, Server, Port, Ssl, NickName, NickName2, NickName3, UserName, UserInfo, MasterChannel, MasterChannelPassword.Trim(), IgnoreChannels, IgnoreNames, UseNickServ, NickServPassword, UseHostServ, HostServStatus, MessageSending, CommandPrefix, MessageType));
						ServerId++;
					}
				}

				new IRCConfig(IrcList);
			}
*/

			Log.Success("XmlConfig", sLConsole.Config("Text4"));
			Console.WriteLine();
		}

		~YamlConfig()
		{
		}

		public bool CreateConfig(string configdir, string configfile)
		{
			// TODO
			return true;
			//return false;
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
	}
}
