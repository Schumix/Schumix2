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
using System.Net;
using System.Text;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using Schumix.Framework.Addon;
using Schumix.Framework.Clean;
using Schumix.Framework.Config;
using Schumix.Framework.Logger;
using Schumix.Framework.Network;
using Schumix.Framework.Listener;
using Schumix.Framework.Database;
using Schumix.Framework.Database.Cache;
using Schumix.Framework.Platforms;
using Schumix.Framework.Functions;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	public sealed class SchumixBase
	{
		private static readonly SchumixPacketHandler sSchumixPacketHandler = Singleton<SchumixPacketHandler>.Instance;
		private static readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private static readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private static readonly AddonManager sAddonManager = Singleton<AddonManager>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Platform sPlatform = Singleton<Platform>.Instance;
		private static readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		private static readonly object WriteLock = new object();
		private static readonly Guid _guid = Guid.NewGuid();
		public static CleanManager sCleanManager { get; private set; }
		public static DatabaseManager DManager { get; private set; }
		public static CacheDB sCacheDB { get; private set; }
		public static Timer sTimer { get; private set; }
		public const string Title = "Schumix2 IRC Bot and Framework";
		public static bool ExitStatus { get; private set; }
		public static string ServerIdentify = string.Empty;
		public static bool ThreadStop = true;
		public static bool STime = true;
		public const string On = "on";
		public const string Off = "off";
		public const char NewLine = '\n';
		public const char Space = ' ';
		public const char Comma = ',';
		public const char Point = '.';
		public const char Colon = ':';
		public static string PidFile;

		/// <summary>
		/// Gets the GUID.
		/// </summary>
		public static Guid GetGuid() { return _guid; }

		public SchumixBase()
		{
			try
			{
				ExitStatus = false;

				if(ServerConfig.Enabled)
				{
					var listener = new ClientSocket(ServerConfig.Host, ServerConfig.Port, ServerConfig.Password);
					Log.Debug("SchumixServer", sLConsole.GetString("Initiating connection."));
					listener.Socket();

					while(ThreadStop)
						Thread.Sleep(100);
				}

				if(ListenerConfig.Enabled)
				{
					Log.Debug("SchumixBot", sLConsole.GetString("SchumixListener starting..."));
					var sListener = new SchumixListener(ListenerConfig.Port);
					new Thread(() => sListener.Listen()).Start();
				}

				if(sPlatform.IsLinux)
					ServicePointManager.ServerCertificateValidationCallback += (s,ce,ca,p) => true;

				WebRequest.DefaultWebProxy = null;

				Log.Debug("SchumixBase", sLConsole.GetString("Timer is starting..."));
				sTimer = new Timer();
				sTimer.Start();

				Log.Debug("SchumixBase", sLConsole.GetString("MySql is starting..."));
				DManager = new DatabaseManager();

				Log.Debug("SchumixBase", sLConsole.GetString("CacheDB is starting..."));
				sCacheDB = new CacheDB();
				sCacheDB.Load();

				Log.Notice("SchumixBase", sLConsole.GetString("Successfully connected to the database."));
				sLManager.Locale = LocalizationConfig.Locale;

				SqlInfoReConfig();

				Log.Debug("SchumixBase", sLConsole.GetString("CleanManager is starting..."));
				sCleanManager = new CleanManager();
				sCleanManager.Initialize();

				if(AddonsConfig.Enabled)
				{
					Log.Debug("SchumixBase", sLConsole.GetString("AddonManager is loading..."));
					sAddonManager.Initialize();
					sAddonManager.LoadPluginsFromDirectory(AddonsConfig.Directory);
				}
			}
			catch(Exception e)
			{
				Log.Error("SchumixBase", sLConsole.GetString("Failure details: {0}"), e.Message);
			}
		}

		/// <summary>
		///     Ha lefut, akkor leáll a class.
		/// </summary>
		~SchumixBase()
		{
			Log.Debug("SchumixBase", "~SchumixBase()");
		}

		public static void ServerDisconnect(bool Reconnect = true)
		{
			if(!ServerConfig.Enabled)
				return;

			var packet = new SchumixPacket();
			packet.Write<int>((int)Opcode.CMSG_CLOSE_CONNECTION);
			packet.Write<string>(_guid.ToString());
			packet.Write<string>(SchumixConfig.ConfigFile);
			packet.Write<string>(SchumixConfig.ConfigDirectory);
			packet.Write<string>(Encoding.UTF8.BodyName);
			packet.Write<string>(LocalizationConfig.Locale);
			packet.Write<string>(Reconnect.ToString());
			packet.Write<string>(ServerIdentify);
			ClientSocket.SendPacketToSCS(packet);
		}

		public static void ListenerDisconnect()
		{
			if(!ListenerConfig.Enabled)
				return;

			var packet = new ListenerPacket();
			packet.Write<int>((int)ListenerOpcode.SMSG_CLOSE_CONNECTION);
			packet.Write<string>(sLConsole.GetString("Schumix Shutdown"));
			sSchumixPacketHandler.SendPacketBackAllHost(packet);
		}

		public static void Quit(bool Reconnect = true)
		{
			lock(WriteLock)
			{
				if(ExitStatus)
					return;

				ExitStatus = true;
				var memory = sRuntime.MemorySize;
				sAddonManager.UnloadPlugins();
				sUtilities.RemovePidFile();
				sTimer.SaveUptime(memory);
				sCacheDB.Clean();
				ServerDisconnect(Reconnect);
				ListenerDisconnect();
			}
		}

		public static void SqlInfoReConfig()
		{
			foreach(var sn in IRCConfig.List)
			{
				DManager.Update("channels", string.Format("ServerName = '{0}'", sn.Key), string.Format("ServerId = '{0}'", sn.Value.ServerId));
				DManager.Update("schumix", string.Format("ServerName = '{0}'", sn.Key), string.Format("ServerId = '{0}'", sn.Value.ServerId));
				DManager.Update("hlmessage", string.Format("ServerName = '{0}'", sn.Key), string.Format("ServerId = '{0}'", sn.Value.ServerId));
				DManager.Update("admins", string.Format("ServerName = '{0}'", sn.Key), string.Format("ServerId = '{0}'", sn.Value.ServerId));
				DManager.Update("ignore_addons", string.Format("ServerName = '{0}'", sn.Key), string.Format("ServerId = '{0}'", sn.Value.ServerId));
				DManager.Update("ignore_channels", string.Format("ServerName = '{0}'", sn.Key), string.Format("ServerId = '{0}'", sn.Value.ServerId));
				DManager.Update("ignore_commands", string.Format("ServerName = '{0}'", sn.Key), string.Format("ServerId = '{0}'", sn.Value.ServerId));
				DManager.Update("ignore_irc_commands", string.Format("ServerName = '{0}'", sn.Key), string.Format("ServerId = '{0}'", sn.Value.ServerId));
				DManager.Update("ignore_nicks", string.Format("ServerName = '{0}'", sn.Key), string.Format("ServerId = '{0}'", sn.Value.ServerId));

				var db1 = DManager.Query("SELECT Id, ServerName FROM channels WHERE ServerId = '{0}'", sn.Value.ServerId);
				if(!db1.IsNull())
				{
					foreach(DataRow row in db1.Rows)
					{
						bool ignore = false;
						int id = row["Id"].ToString().ToInt32();
						var db3 = DManager.Query("SELECT Id, Channel FROM channels WHERE ServerName = '{0}' And Channel = '{1}' ORDER BY Id ASC", row["ServerName"].ToString(), IRCConfig.List[row["ServerName"].ToString()].MasterChannel);
						if(!db3.IsNull())
						{
							int id2 = 0;
							var db4 = DManager.QueryFirstRow("SELECT Id FROM channels WHERE ServerName = '{0}' ORDER BY Id ASC", row["ServerName"].ToString());

							if(!db4.IsNull())
								id2 = db4["Id"].ToInt32();

							foreach(DataRow row2 in db3.Rows)
							{
								if(id2 != row2["Id"].ToString().ToInt32() && row2["Channel"].ToString() == IRCConfig.List[row["ServerName"].ToString()].MasterChannel)
								{
									ignore = true;
									break;
								}
							}
						}

						var db2 = DManager.QueryFirstRow("SELECT Id, ServerName, Channel FROM channels WHERE ServerName = '{0}' ORDER BY Id ASC", row["ServerName"].ToString());

						if(!db2.IsNull())
						{
							if(id == db2["Id"].ToInt32() && !ignore)
							{
								string channel = db2["Channel"].ToString();
								string servername = db2["ServerName"].ToString();
								DManager.Update("channels", string.Format("Channel = '{0}'", IRCConfig.List[servername].MasterChannel), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, servername));
								DManager.Update("channels", string.Format("Password = '{0}'", IRCConfig.List[servername].MasterChannelPassword.Length > 0 ? IRCConfig.List[servername].MasterChannelPassword : string.Empty), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, servername));
								Log.Notice("SchumixBase", sLConsole.GetString("{0} master channel is updated to: {1}"), servername, IRCConfig.List[servername].MasterChannel);
							}
							else if(id == asd.ToInt32(db2["Id"].ToString()) && ignore)
								Log.Warning("SchumixBase", sLConsole.GetString("The master channel already exist on the database, named by default!"));
						}
					}
				}

				NewServerSqlData(sn.Value.ServerId, sn.Key);
				IsAllSchumixFunction(sn.Value.ServerId, sn.Key);
				IsAllChannelFunction(sn.Value.ServerId);

				var db = DManager.Query("SELECT FunctionName, FunctionStatus FROM schumix WHERE ServerName = '{0}'", sn.Key);
				if(!db.IsNull())
				{
					var list = new Dictionary<string, string>();

					foreach(DataRow row in db.Rows)
					{
						string name = row["FunctionName"].ToString();
						string status = row["FunctionStatus"].ToString();
						list.Add(name.ToLower(), status.ToLower());
					}

					IFunctionsClass.ServerList.Add(sn.Key, new IFunctionsClassBase(list));
				}
				else
					Log.Error("SchumixBase", sLConsole.GetString("FunctionReload: Failre request!"));
			}
		}

		private static void NewServerSqlData(int ServerId, string ServerName)
		{
			var db = DManager.QueryFirstRow("SELECT 1 FROM channels WHERE ServerId = '{0}'", ServerId);
			if(db.IsNull())
				DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", ServerId, ServerName, IRCConfig.List[ServerName].MasterChannel, IRCConfig.List[ServerName].MasterChannelPassword, sLManager.Locale);

			db = DManager.QueryFirstRow("SELECT 1 FROM schumix WHERE ServerId = '{0}'", ServerId);
			if(db.IsNull())
			{
				foreach(var function in Enum.GetNames(typeof(IFunctions)))
				{
					if(function == IFunctions.Mantisbt.ToString() || function == IFunctions.Wordpress.ToString() ||
						function == IFunctions.Svn.ToString() || function == IFunctions.Git.ToString() ||
						function == IFunctions.Hg.ToString())
						DManager.Insert("`schumix`(ServerId, ServerName, FunctionName, FunctionStatus)", ServerId, ServerName, function.ToLower(), Off);
					else
						DManager.Insert("`schumix`(ServerId, ServerName, FunctionName, FunctionStatus)", ServerId, ServerName, function.ToLower(), On);
				}
			}
		}

		private static void IsAllSchumixFunction(int ServerId, string ServerName)
		{
			foreach(var function in Enum.GetNames(typeof(IFunctions)))
			{
				var db = DManager.QueryFirstRow("SELECT 1 FROM schumix WHERE ServerId = '{0}' And FunctionName = '{1}'", ServerId, function.ToLower());
				if(db.IsNull())
				{
					if(function == IFunctions.Mantisbt.ToString() || function == IFunctions.Wordpress.ToString() ||
						function == IFunctions.Svn.ToString() || function == IFunctions.Git.ToString() ||
						function == IFunctions.Hg.ToString())
						DManager.Insert("`schumix`(ServerId, ServerName, FunctionName, FunctionStatus)", ServerId, ServerName, function.ToLower(), Off);
					else
						DManager.Insert("`schumix`(ServerId, ServerName, FunctionName, FunctionStatus)", ServerId, ServerName, function.ToLower(), On);
				}
			}
		}

		private static void IsAllChannelFunction(int ServerId)
		{
			var db = DManager.Query("SELECT Functions, Channel FROM channels WHERE ServerId = '{0}'", ServerId);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string functions = string.Empty;
					var dic = new Dictionary<string, string>();
					string Channel = row["Channel"].ToString();
					string[] f = row["Functions"].ToString().Split(SchumixBase.Comma);

					foreach(var ff in f)
					{
						if(ff.IsNullOrEmpty())
							continue;

						if(!ff.Contains(SchumixBase.Colon.ToString()))
							continue;

						string name = ff.Substring(0, ff.IndexOf(SchumixBase.Colon));
						string status = ff.Substring(ff.IndexOf(SchumixBase.Colon)+1);
						dic.Add(name, status);
					}

					foreach(var function in Enum.GetNames(typeof(IChannelFunctions)))
					{
						if(dic.ContainsKey(function.ToString().ToLower()))
							functions += SchumixBase.Comma + function.ToString().ToLower() + SchumixBase.Colon + dic[function.ToString().ToLower()];
						else
						{
							if(function == IChannelFunctions.Log.ToString() || function == IChannelFunctions.Rejoin.ToString() ||
							   function == IChannelFunctions.Commands.ToString())
								functions += SchumixBase.Comma + function.ToString().ToLower() + SchumixBase.Colon + SchumixBase.On;
							else
								functions += SchumixBase.Comma + function.ToString().ToLower() + SchumixBase.Colon + SchumixBase.Off;
						}
					}

					dic.Clear();
					SchumixBase.DManager.Update("channels", string.Format("Functions = '{0}'", functions), string.Format("Channel = '{0}' And ServerId = '{1}'", Channel, ServerId));
				}
			}
		}
	}
}