/*
 * This file is part of Schumix.
 * 
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
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Schumix.API.Delegate;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc
{
	public class IrcBase
	{
		private readonly Dictionary<string, Network> _networks = new Dictionary<string, Network>();
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly object Lock = new object();
		public Dictionary<string, Network> Networks
		{
			get { return _networks; }
		}

		private bool shutdown = false;
		private IrcBase() {}

		public void NewServer(string ServerName, int ServerId, string Host, int Port)
		{
			lock(Lock)
			{
				var nw = new Network(ServerName.ToLower(), ServerId, Host, Port);

				if(IRCConfig.List[ServerName].Ssl)
					nw.SetConnectionType(ConnectionType.Ssl);

				_networks.Add(ServerName, nw);
				_networks[ServerName.ToLower()].InitializeIgnoreCommand();
			}
		}

		public void Connect(string ServerName)
		{
			lock(Lock)
			{
				if(_networks.ContainsKey(ServerName.ToLower()))
				{
					_networks[ServerName.ToLower()].Initialize();
					_networks[ServerName.ToLower()].Connect(true);
					_networks[ServerName.ToLower()].InitializeOpcodesAndPing();
				}
			}
		}

		public void IrcRegisterHandler(string code, IRCDelegate method)
		{
			lock(Lock)
			{
				foreach(var nw in _networks)
					nw.Value.IrcRegisterHandler(code, method);
			}
		}

		public void IrcRemoveHandler(string code)
		{
			lock(Lock)
			{
				foreach(var nw in _networks)
					nw.Value.IrcRemoveHandler(code);
			}
		}

		public void IrcRemoveHandler(string code, IRCDelegate method)
		{
			lock(Lock)
			{
				foreach(var nw in _networks)
					nw.Value.IrcRemoveHandler(code, method);
			}
		}

		public void IrcRegisterHandler(ReplyCode code, IRCDelegate method)
		{
			lock(Lock)
			{
				foreach(var nw in _networks)
					nw.Value.IrcRegisterHandler(code, method);
			}
		}

		public void IrcRemoveHandler(ReplyCode code)
		{
			lock(Lock)
			{
				foreach(var nw in _networks)
					nw.Value.IrcRemoveHandler(code);
			}
		}

		public void IrcRemoveHandler(ReplyCode code, IRCDelegate method)
		{
			lock(Lock)
			{
				foreach(var nw in _networks)
					nw.Value.IrcRemoveHandler(code, method);
			}
		}

		public void IrcRegisterHandler(int code, IRCDelegate method)
		{
			lock(Lock)
			{
				foreach(var nw in _networks)
					nw.Value.IrcRegisterHandler(code, method);
			}
		}

		public void IrcRemoveHandler(int code)
		{
			lock(Lock)
			{
				foreach(var nw in _networks)
					nw.Value.IrcRemoveHandler(code);
			}
		}

		public void IrcRemoveHandler(int code, IRCDelegate method)
		{
			lock(Lock)
			{
				foreach(var nw in _networks)
					nw.Value.IrcRemoveHandler(code, method);
			}
		}

		public void SchumixRegisterHandler(string code, CommandDelegate method, CommandPermission permission = CommandPermission.Normal)
		{
			lock(Lock)
			{
				foreach(var nw in _networks)
					nw.Value.SchumixRegisterHandler(code, method, permission);
			}
		}

		public void SchumixRemoveHandler(string code)
		{
			lock(Lock)
			{
				foreach(var nw in _networks)
					nw.Value.SchumixRemoveHandler(code);
			}
		}

		public void SchumixRemoveHandler(string code, CommandDelegate method)
		{
			lock(Lock)
			{
				foreach(var nw in _networks)
					nw.Value.SchumixRemoveHandler(code, method);
			}
		}

		public void Shutdown(string Message)
		{
			if(shutdown)
				return;

			shutdown = true;

			foreach(var nw in Networks)
				nw.Value.sSender.Quit(Message);

			int i = 0;

			while(true)
			{
				if(i >= 30)
					break;

				var list = new List<bool>();

				foreach(var nw in Networks)
					list.Add(nw.Value.Shutdown);

				if(list.CompareDataInBlock())
					break;
				else
					Thread.Sleep(100);

				i++;
			}

			Log.Warning("IrcBase", sLConsole.IrcBase("Text"));
			Process.GetCurrentProcess().Kill();
		}
	}
}