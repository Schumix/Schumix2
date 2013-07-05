/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2012-2013 Jackneill
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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
using System.Text.RegularExpressions;
using Schumix.Framework.Extensions;

namespace Schumix.Framework.Config
{
	public abstract class DefaultConfig
	{
		protected const string d_logfilename           = "Schumix.log";
		protected const bool d_logdatefilename         = false;
		protected const int d_logmaxfilesize           = 100;
		protected const int d_loglevel                 = 2;
		protected const string d_logdirectory          = "Logs";
		protected const string d_irclogdirectory       = "Channels";
		protected const bool d_irclog                  = false;
		protected const bool d_serverenabled           = false;
		protected const string d_serverhost            = "127.0.0.1";
		protected const int d_serverport               = 35220;
		protected const string d_serverpassword        = "schumix";
		protected const bool d_listenerenabled         = false;
		protected const int d_listenerport             = 36200;
		protected const string d_listenerpassword      = "schumix";
		protected const string d_servername            = "Default";
		protected const string d_server                = "localhost";
		protected const string d_ircserverpassword     = " ";
		protected const int d_port                     = 6667;
		protected const int d_modemask                 = 8;
		protected const bool d_ssl                     = false;
		protected const string d_nickname              = "Schumix2";
		protected const string d_nickname2             = "_Schumix2";
		protected const string d_nickname3             = "__Schumix2";
		protected const string d_username              = "Schumix2";
		protected const string d_userinfo              = "Schumix2 IRC Bot";
		protected const string d_masterchannel         = "#schumix2";
		protected const string d_masterchannelpassword = " ";
		protected const string d_ignorechannels        = " ";
		protected const string d_ignorenames           = " ";
		protected const bool d_usenickserv             = false;
		protected const string d_nickservpassword      = "password";
		protected const bool d_usehostserv             = false;
		protected const bool d_hostservstatus          = false;
		protected const int d_messagesending           = 400;
		protected const string d_commandprefix         = "$";
		protected const string d_messagetype           = "Privmsg";
		protected const bool d_mysqlenabled            = false;
		protected const string d_mysqlhost             = "localhost";
		protected const string d_mysqluser             = "root";
		protected const string d_mysqlpassword         = "password";
		protected const string d_mysqldatabase         = "database";
		protected const string d_mysqlcharset          = "utf8";
		protected const bool d_sqliteenabled           = true;
		protected const string d_sqlitefilename        = "Schumix.db3";
		protected const bool d_addonenabled            = true;
		protected const string d_addonignore           = "MantisBTRssAddon,SvnRssAddon,GitRssAddon,HgRssAddon,WordPressRssAddon,TestAddon";
		protected const string d_addondirectory        = "Addons";
		protected const bool d_scriptsluaenabled       = false;
		protected const bool d_scriptspythonenabled    = false;
		protected const string d_scriptsdirectory      = "Scripts";
		protected const string d_crashdirectory        = "Dumps";
		protected const string d_locale                = "enUS";
		protected const bool d_updateenabled           = false;
		protected const string d_updateversion         = "stable";
		protected const string d_updatebranch          = "master";
		protected const string d_updatewebpage         = "https://github.com/Schumix/Schumix2";
		protected const int d_shutdownmaxmemory        = 100;
		protected const int d_floodingseconds          = 4;
		protected const int d_floodingnumberofcommands = 2;
		protected const bool d_cleanconfig             = false;
		protected const bool d_cleandatabase           = false;
		protected const string d_shorturlname          = " ";
		protected const string d_shorturlapikey        = " ";
		protected bool errors                          = false;

		// Nick
		private const string Nick = @"^[" + Special + @"a-zA-Z0-9\-]{0,20}$";
		private const string Special = @"\[\]\`_\^\{\|\}";
		private readonly Regex NickRegex = new Regex(Nick); 

		/// <summary>
		/// Using the rules set forth in RFC 2812 determine if
		/// the nickname is valid.
		/// </summary>
		/// <returns>True is the nickname is valid</returns>
		protected bool IsValidNick(string nick)
		{
			if(nick.IsNull() || nick.Trim().Length == 0)
				return false;
			
			return !ContainsSpace(nick) && !nick.IsNumber() && !nick.Substring(0, 1).IsNumber() && NickRegex.IsMatch(nick);
		}

		private bool ContainsSpace(string text)
		{
			return text.IndexOf(SchumixBase.Space, 0, text.Length) != -1;
		}
	}
}