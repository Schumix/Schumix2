using System;

namespace Schumix.Framework.Config
{
    public class DefaultConfig
    {
        public const string _logfilename = "Schumix.log";
        public const int _loglevel = 2;
        public const string _logdirectory = "Logs";
        public const string _irclogdirectory = "Channels";
        public const bool _irclog = false;
        public const bool _serverenabled = false;
        public const string _serverhost = "127.0.0.1";
        public const int _serverport = 35220;
        public const string _serverpassword = "schumix";
        public const string _servername = "Default";
        public const string _server = "localhost";
        public const int _port = 6667;
        public const bool _ssl = false;
        public const string _nickname = "Schumix2";
        public const string _nickname2 = "_Schumix2";
        public const string _nickname3 = "__Schumix2";
        public const string _username = "Schumix2";
        public const string _userinfo = "Schumix2 IRC Bot";
        public const string _masterchannel = "#schumix2";
        public const string _masterchannelpassword = " ";
        public const string _ignorechannels = " ";
        public const string _ignorenames = " ";
        public const bool _usenickserv = false;
        public const string _nickservpassword = "password";
        public const bool _usehostserv = false;
        public const bool _hostservstatus = false;
        public const int _messagesending = 400;
        public const string _commandprefix = "$";
        public const string _messagetype = "Privmsg";
        public const bool _mysqlenabled = false;
        public const string _mysqlhost = "localhost";
        public const string _mysqluser = "root";
        public const string _mysqlpassword = "password";
        public const string _mysqldatabase = "database";
        public const string _mysqlcharset = "utf8";
        public const bool _sqliteenabled = false;
        public const string _sqlitefilename = "Schumix.db3";
        public const bool _addonenabled = true;
        public const string _addonignore = "MantisBTRssAddon,SvnRssAddon,GitRssAddon,HgRssAddon,WordPressRssAddon,TesztAddon";
        public const string _addondirectory = "Addons";
        public const bool _scriptenabled = false;
        public const string _scriptdirectory = "Scripts";
        public const string _locale = "enUS";
        public const bool _updateenabled = false;
        public const string _updateversion = "stable";
        public const string _updatebranch = "master";
        public const string _updatewebpage = "http://megax.uw.hu/Schumix2/";
        public bool error = false;
    }
}
