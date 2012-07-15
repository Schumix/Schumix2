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
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Localization;

namespace Schumix.Irc
{
	public sealed class NickInfoo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly Sendere sSender;
		private string _NickStorage;
		private bool _Identify;
		private bool _Vhost;

		public string NickStorage
		{
			get { return _NickStorage; }
		}

		public bool IsNickStorage()
		{
			return _NickStorage.ToLower() == IRCConfig.NickName.ToLower();
		}

		public bool IsIdentify
		{
			get { return _Identify; }
		}

		public bool IsVhost
		{
			get { return _Vhost; }
		}

		public NickInfoo(string ServerName)
		{
			_Identify = false;
			_Vhost = false;
			sSender = sIrcBase.Networks[ServerName].sSender;
		}

		public string ChangeNick()
		{
			if(_NickStorage == IRCConfig.NickName)
			{
				_NickStorage = IRCConfig.NickName2;
				return _NickStorage;
			}
			else if(_NickStorage == IRCConfig.NickName2)
			{
				_NickStorage = IRCConfig.NickName3;
				return _NickStorage;
			}
			else if(_NickStorage == IRCConfig.NickName3)
			{
				_NickStorage = string.Format("_{0}_", IRCConfig.NickName);
				return _NickStorage;
			}
			else if(_NickStorage == string.Format("_{0}_", IRCConfig.NickName))
			{
				_NickStorage = string.Format("__{0}_", IRCConfig.NickName);
				return _NickStorage;
			}
			else if(_NickStorage == string.Format("__{0}_", IRCConfig.NickName))
			{
				_NickStorage = string.Format("__{0}__", IRCConfig.NickName);
				return _NickStorage;
			}
			else if(_NickStorage == string.Format("__{0}__", IRCConfig.NickName))
			{
				_NickStorage = string.Format("___{0}", IRCConfig.NickName);
				return _NickStorage;
			}
			else if(_NickStorage == string.Format("___{0}", IRCConfig.NickName))
			{
				_NickStorage = string.Format("___{0}_", IRCConfig.NickName);
				return _NickStorage;
			}
			else if(_NickStorage == string.Format("___{0}_", IRCConfig.NickName))
			{
				_NickStorage = string.Format("___{0}__", IRCConfig.NickName);
				return _NickStorage;
			}
			else if(_NickStorage == string.Format("___{0}__", IRCConfig.NickName))
			{
				_NickStorage = string.Format("___{0}___", IRCConfig.NickName);
				return _NickStorage;
			}
			else if(_NickStorage == string.Format("___{0}___", IRCConfig.NickName))
			{
				_NickStorage = IRCConfig.NickName;
				return _NickStorage;
			}
			else
			{
				_NickStorage = IRCConfig.NickName;
				return _NickStorage;
			}
		}

		public void ChangeNick(string newnick)
		{
			_NickStorage = newnick;
		}

		public void Identify(string Password)
		{
			if(!_Identify)
			{
				Log.Notice("NickServ", sLConsole.NickServ("Text"));
				sSender.NickServ(Password);
			}
		}

		public void Vhost(string Status)
		{
			if(!_Vhost)
			{
				if(Status == SchumixBase.Off)
				{
					_Vhost = true;
					Log.Notice("HostServ", sLConsole.HostServ("Text2"));
				}
				else if(Status == SchumixBase.On)
					Log.Notice("HostServ", sLConsole.HostServ("Text"));

				sSender.HostServ(Status);
			}
		}

		public void ChangeIdentifyStatus(bool Status)
		{
			_Identify = Status;
		}

		public void ChangeVhostStatus(bool Status)
		{
			_Vhost = Status;
		}
	}
}