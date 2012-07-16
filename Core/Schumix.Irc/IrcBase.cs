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
using System.Collections.Generic;
using Schumix.API.Delegate;
using Schumix.Irc.Commands;
using Schumix.Framework.Config;

namespace Schumix.Irc
{
	public class IrcBase
	{
		private readonly Dictionary<string, Network> _networks = new Dictionary<string, Network>();
		private readonly object Lock = new object();
		public Dictionary<string, Network> Networks
		{
			get { return _networks; }
		}

		private IrcBase() {}

		public void NewServer(string Name, int Id, string Host, int Port)
		{
			lock(Lock)
			{
				var nw = new Network(Name.ToLower(), Id, Host, Port);

				if(IRCConfig.Ssl)
					nw.SetConnectionType(ConnectionType.Ssl);

				_networks.Add(Name, nw);
			}
		}

		public void Connect(string Name)
		{
			lock(Lock)
			{
				if(_networks.ContainsKey(Name.ToLower()))
				{
					_networks[Name.ToLower()].Initialize();
					_networks[Name.ToLower()].Connect(true);
					_networks[Name.ToLower()].InitializeOpcodesAndPing();
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
	}
}