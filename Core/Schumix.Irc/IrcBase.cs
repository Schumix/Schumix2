/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Addon;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Delegate;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc
{
	public class IrcBase
	{
		private readonly Dictionary<string, Network> _networks = new Dictionary<string, Network>();
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly AddonManager sAddonManager = Singleton<AddonManager>.Instance;
		private readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		public bool ReloadStatus { get; private set; }
		private readonly object Lock = new object();
		private bool shutdown = false;

		public Dictionary<string, Network> Networks
		{
			get { return _networks; }
		}

		private IrcBase()
		{
			ReloadStatus = false;
		}

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

		public void LoadProcessMethods(string ServerName)
		{
			var asms = sAddonManager.Addons[ServerName].Assemblies.ToDictionary(v => v.Key, v => v.Value);
			Parallel.ForEach(asms, asm =>
			{
				var types = asm.Value.GetTypes();
				LoadProcessMethods(ServerName, types);
			});
		}

		public void LoadProcessMethods(string ServerName, Type[] types)
		{
			Parallel.ForEach(types, type =>
			{
				var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
				Parallel.ForEach(methods, method =>
				{
					foreach(var attribute in Attribute.GetCustomAttributes(method))
					{
						if(attribute.IsOfType(typeof(IrcCommandAttribute)))
						{
							var attr = (IrcCommandAttribute)attribute;
							lock(Lock)
							{
								var del = Delegate.CreateDelegate(typeof(IRCDelegate), method) as IRCDelegate;
								_networks[ServerName].IrcRegisterHandler(attr.Command, del);
							}
						}

						if(attribute.IsOfType(typeof(SchumixCommandAttribute)))
						{
							var attr = (SchumixCommandAttribute)attribute;
							lock(Lock)
							{
								var del = Delegate.CreateDelegate(typeof(CommandDelegate), method) as CommandDelegate;
								_networks[ServerName].SchumixRegisterHandler(attr.Command, del, attr.Permission);
							}
						}
					}
				});
			});
		}

		public void UnloadProcessMethods(string ServerName)
		{
			var asms = sAddonManager.Addons[ServerName].Assemblies.ToDictionary(v => v.Key, v => v.Value);
			Parallel.ForEach(asms, asm =>
			{
				var types = asm.Value.GetTypes();
				UnloadProcessMethods(ServerName, types);
			});
		}

		public void UnloadProcessMethods(string ServerName, Type[] types)
		{
			Parallel.ForEach(types, type =>
			{
				var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
				Parallel.ForEach(methods, method =>
				{
					foreach(var attribute in Attribute.GetCustomAttributes(method))
					{
						if(attribute.IsOfType(typeof(IrcCommandAttribute)))
						{
							var attr = (IrcCommandAttribute)attribute;
							lock(Lock)
							{
								var del = Delegate.CreateDelegate(typeof(IRCDelegate), method) as IRCDelegate;
								_networks[ServerName].IrcRemoveHandler(attr.Command, del);
							}
						}

						if(attribute.IsOfType(typeof(SchumixCommandAttribute)))
						{
							var attr = (SchumixCommandAttribute)attribute;
							lock(Lock)
							{
								var del = Delegate.CreateDelegate(typeof(CommandDelegate), method) as CommandDelegate;
								_networks[ServerName].SchumixRemoveHandler(attr.Command, del);
							}
						}
					}
				});
			});
		}

		public string FirstStart()
		{
			bool e = false;
			string eserver = string.Empty;

			if(_networks.Count > 0)
				_networks.Clear();

			foreach(var sn in IRCConfig.List)
			{
				if(!e)
				{
					eserver = sn.Key;
					e = true;
				}

				NewServer(sn.Key, sn.Value.ServerId, sn.Value.Server, sn.Value.Port);
			}

			return eserver;
		}

		public void Start(string Name)
		{
			Task.Factory.StartNew(() =>
			{
				if(IRCConfig.List.Count == 1)
				{
					Connect(Name);
					return;
				}

				int i = 0;
				foreach(var sn in IRCConfig.List)
				{
					Connect(sn.Key);

					while(!_networks[sn.Key].Online)
					{
						if(i >= 30)
							break;

						i++;
						Thread.Sleep(1000);
					}
				}
			});
		}

		public void Reload()
		{
			ReloadStatus = true;
			AllIrcServerShutdown(sLConsole.GetString("Reload irc module!"), true);
			Thread.Sleep(_networks.Count * 1000);
			string eserver = FirstStart();
			Start(eserver);
			Thread.Sleep(_networks.Count * 3000);
			ReloadStatus = false;
		}

		public void Shutdown(string Message)
		{
			if(shutdown)
				return;

			shutdown = true;
			AllIrcServerShutdown(Message);
			Log.Warning("IrcBase", sLConsole.GetString("Program shutting down!"));
			sRuntime.Exit();
		}

		public void AllIrcServerShutdown(string Message, bool reload = false)
		{
			foreach(var nw in _networks)
				nw.Value.sSender.Quit(Message);

			int i = 0;

			while(true)
			{
				if(i >= 30 && !reload)
					break;

				var list = new List<bool>();

				foreach(var nw in _networks)
					list.Add(nw.Value.Shutdown);

				if(list.CompareDataInBlock())
				{
					list.Clear();
					break;
				}
				else
					Thread.Sleep(200);

				i++;
				list.Clear();
			}
		}
	}
}