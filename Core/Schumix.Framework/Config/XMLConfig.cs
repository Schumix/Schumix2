using System;
using System.Collections.Generic;
using System.Xml;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Config
{
    public sealed class XMLConfig : DefaultConfig
    {
        private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

        public XMLConfig(string configdir, string configfile)
        {
            var xmldoc = new XmlDocument();
            xmldoc.Load(sUtilities.DirectoryToHome(configdir, configfile));

            string LogFileName = !xmldoc.SelectSingleNode("Schumix/Log/FileName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Log/FileName").InnerText : _logfilename;
            int LogLevel = !xmldoc.SelectSingleNode("Schumix/Log/LogLevel").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Log/LogLevel").InnerText) : _loglevel;
            string LogDirectory = !xmldoc.SelectSingleNode("Schumix/Log/LogDirectory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Log/LogDirectory").InnerText : _logdirectory;
            string IrcLogDirectory = !xmldoc.SelectSingleNode("Schumix/Log/IrcLogDirectory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Log/IrcLogDirectory").InnerText : _irclogdirectory;
            bool IrcLog = !xmldoc.SelectSingleNode("Schumix/Log/IrcLog").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Log/IrcLog").InnerText) : _irclog;

            new LogConfig(LogFileName, LogLevel, sUtilities.GetHomeDirectory(LogDirectory), sUtilities.GetHomeDirectory(IrcLogDirectory), IrcLog);

            Log.Initialize(LogFileName);
            Log.Debug("Config", ">> {0}", configfile);

            Log.Notice("Config", sLConsole.Config("Text3"));
            bool ServerEnabled = !xmldoc.SelectSingleNode("Schumix/Server/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Server/Enabled").InnerText) : _serverenabled;
            string ServerHost = !xmldoc.SelectSingleNode("Schumix/Server/Host").IsNull() ? xmldoc.SelectSingleNode("Schumix/Server/Host").InnerText : _serverhost;
            int ServerPort = !xmldoc.SelectSingleNode("Schumix/Server/Port").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Server/Port").InnerText) : _serverport;
            string ServerPassword = !xmldoc.SelectSingleNode("Schumix/Server/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/Server/Password").InnerText : _serverpassword;

            new ServerConfig(ServerEnabled, ServerHost, ServerPort, ServerPassword);

            int ServerId = 1;
            var xmlirclist = xmldoc.SelectNodes("Schumix/Irc");
            var IrcList = new Dictionary<string, IRCConfigBase>();

            if (xmlirclist.Count == 0)
            {
                string ServerName = !xmldoc.SelectSingleNode("Schumix/Irc/ServerName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/ServerName").InnerText : _servername;
                string Server = !xmldoc.SelectSingleNode("Schumix/Irc/Server").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Server").InnerText : _server;
                int Port = !xmldoc.SelectSingleNode("Schumix/Irc/Port").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Port").InnerText) : _port;
                bool Ssl = !xmldoc.SelectSingleNode("Schumix/Irc/Ssl").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/Ssl").InnerText) : _ssl;
                string NickName = !xmldoc.SelectSingleNode("Schumix/Irc/NickName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName").InnerText : _nickname;
                string NickName2 = !xmldoc.SelectSingleNode("Schumix/Irc/NickName2").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName2").InnerText : _nickname2;
                string NickName3 = !xmldoc.SelectSingleNode("Schumix/Irc/NickName3").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickName3").InnerText : _nickname3;
                string UserName = !xmldoc.SelectSingleNode("Schumix/Irc/UserName").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/UserName").InnerText : _username;
                string UserInfo = !xmldoc.SelectSingleNode("Schumix/Irc/UserInfo").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/UserInfo").InnerText : _userinfo;
                string MasterChannel = !xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel/Name").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel/Name").InnerText : _masterchannel;
                string MasterChannelPassword = !xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/MasterChannel/Password").InnerText : _masterchannelpassword;
                string IgnoreChannels = !xmldoc.SelectSingleNode("Schumix/Irc/IgnoreChannels").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/IgnoreChannels").InnerText : _ignorechannels;
                string IgnoreNames = !xmldoc.SelectSingleNode("Schumix/Irc/IgnoreNames").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/IgnoreNames").InnerText : _ignorenames;
                bool UseNickServ = !xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Enabled").InnerText) : _usenickserv;
                string NickServPassword = !xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/NickServ/Password").InnerText : _nickservpassword;
                bool UseHostServ = !xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Enabled").InnerText) : _usehostserv;
                bool HostServStatus = !xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Vhost").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Irc/HostServ/Vhost").InnerText) : _hostservstatus;
                int MessageSending = !xmldoc.SelectSingleNode("Schumix/Irc/Wait/MessageSending").IsNull() ? Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Wait/MessageSending").InnerText) : _messagesending;
                string CommandPrefix = !xmldoc.SelectSingleNode("Schumix/Irc/Command/Prefix").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/Command/Prefix").InnerText : _commandprefix;
                string MessageType = !xmldoc.SelectSingleNode("Schumix/Irc/MessageType").IsNull() ? xmldoc.SelectSingleNode("Schumix/Irc/MessageType").InnerText : _messagetype;

                if (MasterChannel.Length >= 2 && MasterChannel.Trim().Length > 1 && MasterChannel.Substring(0, 1) != "#")
                    MasterChannel = "#" + MasterChannel;
                else if (MasterChannel.Length < 2 && MasterChannel.Trim().Length <= 1)
                    MasterChannel = _masterchannel;

                IrcList.Add(ServerName.ToLower(), new IRCConfigBase(ServerId, Server, Port, Ssl, NickName, NickName2, NickName3, UserName, UserInfo, MasterChannel, MasterChannelPassword.Trim(), IgnoreChannels, IgnoreNames, UseNickServ, NickServPassword, UseHostServ, HostServStatus, MessageSending, CommandPrefix, MessageType));
            }
            else
            {
                foreach (XmlNode xn in xmlirclist)
                {
                    string ServerName = !xn["ServerName"].IsNull() ? xn["ServerName"].InnerText : _servername;
                    string Server = !xn["Server"].IsNull() ? xn["Server"].InnerText : _server;
                    int Port = !xn["Port"].IsNull() ? Convert.ToInt32(xn["Port"].InnerText) : _port;
                    bool Ssl = !xn["Ssl"].IsNull() ? Convert.ToBoolean(xn["Ssl"].InnerText) : _ssl;
                    string NickName = !xn["NickName"].IsNull() ? xn["NickName"].InnerText : _nickname;
                    string NickName2 = !xn["NickName2"].IsNull() ? xn["NickName2"].InnerText : _nickname2;
                    string NickName3 = !xn["NickName3"].IsNull() ? xn["NickName3"].InnerText : _nickname3;
                    string UserName = !xn["UserName"].IsNull() ? xn["UserName"].InnerText : _username;
                    string UserInfo = !xn["UserInfo"].IsNull() ? xn["UserInfo"].InnerText : _userinfo;
                    string MasterChannel = !xn.SelectSingleNode("MasterChannel/Name").IsNull() ? xn.SelectSingleNode("MasterChannel/Name").InnerText : _masterchannel;
                    string MasterChannelPassword = !xn.SelectSingleNode("MasterChannel/Password").IsNull() ? xn.SelectSingleNode("MasterChannel/Password").InnerText : _masterchannelpassword;
                    string IgnoreChannels = !xn["IgnoreChannels"].IsNull() ? xn["IgnoreChannels"].InnerText : _ignorechannels;
                    string IgnoreNames = !xn["IgnoreNames"].IsNull() ? xn["IgnoreNames"].InnerText : _ignorenames;
                    bool UseNickServ = !xn.SelectSingleNode("NickServ/Enabled").IsNull() ? Convert.ToBoolean(xn.SelectSingleNode("NickServ/Enabled").InnerText) : _usenickserv;
                    string NickServPassword = !xn.SelectSingleNode("NickServ/Password").IsNull() ? xn.SelectSingleNode("NickServ/Password").InnerText : _nickservpassword;
                    bool UseHostServ = !xn.SelectSingleNode("HostServ/Enabled").IsNull() ? Convert.ToBoolean(xn.SelectSingleNode("HostServ/Enabled").InnerText) : _usehostserv;
                    bool HostServStatus = !xn.SelectSingleNode("HostServ/Vhost").IsNull() ? Convert.ToBoolean(xn.SelectSingleNode("HostServ/Vhost").InnerText) : _hostservstatus;
                    int MessageSending = !xn.SelectSingleNode("Wait/MessageSending").IsNull() ? Convert.ToInt32(xn.SelectSingleNode("Wait/MessageSending").InnerText) : _messagesending;
                    string CommandPrefix = !xn.SelectSingleNode("Command/Prefix").IsNull() ? xn.SelectSingleNode("Command/Prefix").InnerText : _commandprefix;
                    string MessageType = !xn["MessageType"].IsNull() ? xn["MessageType"].InnerText : _messagetype;

                    if (MasterChannel.Length >= 2 && MasterChannel.Trim().Length > 1 && MasterChannel.Substring(0, 1) != "#")
                        MasterChannel = "#" + MasterChannel;
                    else if (MasterChannel.Length < 2 && MasterChannel.Trim().Length <= 1)
                        MasterChannel = _masterchannel;

                    if (IrcList.ContainsKey(ServerName.ToLower()))
                        Log.Error("Config", sLConsole.Config("Text12"), ServerName);
                    else
                    {
                        IrcList.Add(ServerName.ToLower(), new IRCConfigBase(ServerId, Server, Port, Ssl, NickName, NickName2, NickName3, UserName, UserInfo, MasterChannel, MasterChannelPassword.Trim(), IgnoreChannels, IgnoreNames, UseNickServ, NickServPassword, UseHostServ, HostServStatus, MessageSending, CommandPrefix, MessageType));
                        ServerId++;
                    }
                }

                new IRCConfig(IrcList);

                bool Enabled = !xmldoc.SelectSingleNode("Schumix/MySql/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/MySql/Enabled").InnerText) : _mysqlenabled;
                string Host = !xmldoc.SelectSingleNode("Schumix/MySql/Host").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Host").InnerText : _mysqlhost;
                string User = !xmldoc.SelectSingleNode("Schumix/MySql/User").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/User").InnerText : _mysqluser;
                string Password = !xmldoc.SelectSingleNode("Schumix/MySql/Password").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Password").InnerText : _mysqlpassword;
                string Database = !xmldoc.SelectSingleNode("Schumix/MySql/Database").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Database").InnerText : _mysqldatabase;
                string Charset = !xmldoc.SelectSingleNode("Schumix/MySql/Charset").IsNull() ? xmldoc.SelectSingleNode("Schumix/MySql/Charset").InnerText : _mysqlcharset;

                new MySqlConfig(Enabled, Host, User, Password, Database, Charset);

                Enabled = !xmldoc.SelectSingleNode("Schumix/SQLite/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/SQLite/Enabled").InnerText) : _sqliteenabled;
                string FileName = !xmldoc.SelectSingleNode("Schumix/SQLite/FileName").IsNull() ? xmldoc.SelectSingleNode("Schumix/SQLite/FileName").InnerText : _sqlitefilename;

                new SQLiteConfig(Enabled, sUtilities.GetHomeDirectory(FileName));

                Enabled = !xmldoc.SelectSingleNode("Schumix/Addons/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Addons/Enabled").InnerText) : _addonenabled;
                string Ignore = !xmldoc.SelectSingleNode("Schumix/Addons/Ignore").IsNull() ? xmldoc.SelectSingleNode("Schumix/Addons/Ignore").InnerText : _addonignore;
                string Directory = !xmldoc.SelectSingleNode("Schumix/Addons/Directory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Addons/Directory").InnerText : _addondirectory;

                new AddonsConfig(Enabled, Ignore, Directory);

                bool Lua = !xmldoc.SelectSingleNode("Schumix/Scripts/Lua").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Scripts/Lua").InnerText) : _scriptenabled;
                Directory = !xmldoc.SelectSingleNode("Schumix/Scripts/Directory").IsNull() ? xmldoc.SelectSingleNode("Schumix/Scripts/Directory").InnerText : _scriptdirectory;

                new ScriptsConfig(Lua, sUtilities.GetHomeDirectory(Directory));

                string Locale = !xmldoc.SelectSingleNode("Schumix/Localization/Locale").IsNull() ? xmldoc.SelectSingleNode("Schumix/Localization/Locale").InnerText : _locale;

                new LocalizationConfig(Locale);

                Enabled = !xmldoc.SelectSingleNode("Schumix/Update/Enabled").IsNull() ? Convert.ToBoolean(xmldoc.SelectSingleNode("Schumix/Update/Enabled").InnerText) : _updateenabled;
                string Version = !xmldoc.SelectSingleNode("Schumix/Update/Version").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/Version").InnerText : _updateversion;
                string Branch = !xmldoc.SelectSingleNode("Schumix/Update/Branch").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/Branch").InnerText : _updatebranch;
                string WebPage = !xmldoc.SelectSingleNode("Schumix/Update/WebPage").IsNull() ? xmldoc.SelectSingleNode("Schumix/Update/WebPage").InnerText : _updatewebpage;

                new UpdateConfig(Enabled, Version.ToLower(), Branch, WebPage);

                Log.Success("Config", sLConsole.Config("Text4"));
                Console.WriteLine();
            }
        }

        ~XMLConfig() { }
    }
}
