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
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;
using Schumix.API;
using Schumix.API.Functions;
using Schumix.Framework.Addon;
using Schumix.Framework.Clean;
using Schumix.Framework.Client;
using Schumix.Framework.Config;
using Schumix.Framework.Database;
using Schumix.Framework.Database.Cache;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	public sealed class SchumixBase
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private static readonly AddonManager sAddonManager = Singleton<AddonManager>.Instance;
		private static readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly object WriteLock = new object();
		private static readonly Guid _guid = Guid.NewGuid();
		public static CleanManager sCleanManager { get; private set; }
		public static DatabaseManager DManager { get; private set; }
		public static CacheDB sCacheDB { get; private set; }
		public static Timer timer { get; private set; }
		public const string Title = "Schumix2 IRC Bot and Framework";
		public static bool ExitStatus { get; private set; }
		public static string ServerIdentify = string.Empty;
		public static bool UrlTitleEnabled = false;
		public static bool ThreadStop = true;
		public static bool NewNick = false;
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
					Log.Debug("SchumixServer", sLConsole.SchumixBase("Text6"));
					listener.Socket();

					while(ThreadStop)
						Thread.Sleep(100);
				}

				if(sUtilities.GetPlatformType() == PlatformType.Linux)
					System.Net.ServicePointManager.ServerCertificateValidationCallback += (s,ce,ca,p) => true;

				Log.Debug("SchumixBase", sLConsole.SchumixBase("Text"));
				timer = new Timer();

				Log.Debug("SchumixBase", sLConsole.SchumixBase("Text2"));
				DManager = new DatabaseManager();

				Log.Debug("SchumixBase", sLConsole.SchumixBase("Text8"));
				sCacheDB = new CacheDB();
				sCacheDB.Load();

				Log.Notice("SchumixBase", sLConsole.SchumixBase("Text3"));
				sLManager.Locale = LocalizationConfig.Locale;

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
							int id = Convert.ToInt32(row["Id"].ToString());
							var db3 = DManager.Query("SELECT Id, Channel FROM channels WHERE ServerName = '{0}' And Channel = '{1}' ORDER BY Id ASC", row["ServerName"].ToString(), IRCConfig.List[row["ServerName"].ToString()].MasterChannel);
							if(!db3.IsNull())
							{
								int id2 = 0;
								var db4 = DManager.QueryFirstRow("SELECT Id FROM channels WHERE ServerName = '{0}' ORDER BY Id ASC", row["ServerName"].ToString());

								if(!db4.IsNull())
									id2 = Convert.ToInt32(db4["Id"].ToString());

								foreach(DataRow row2 in db3.Rows)
								{
									if(id2 != Convert.ToInt32(row2["Id"].ToString()) && row2["Channel"].ToString() == IRCConfig.List[row["ServerName"].ToString()].MasterChannel)
									{
										ignore = true;
										break;
									}
								}
							}
								
							var db2 = DManager.QueryFirstRow("SELECT Id, ServerName, Channel FROM channels WHERE ServerName = '{0}' ORDER BY Id ASC", row["ServerName"].ToString());

							if(!db2.IsNull())
							{
								if(id == Convert.ToInt32(db2["Id"].ToString()) && !ignore)
								{
									string channel = db2["Channel"].ToString();
									string servername = db2["ServerName"].ToString();
									DManager.Update("channels", string.Format("Channel = '{0}'", IRCConfig.List[servername].MasterChannel), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, servername));
									DManager.Update("channels", string.Format("Password = '{0}'", IRCConfig.List[servername].MasterChannelPassword.Length > 0 ? IRCConfig.List[servername].MasterChannelPassword : string.Empty), string.Format("Channel = '{0}' And ServerName = '{1}'", channel, servername));
									Log.Notice("SchumixBase", sLConsole.SchumixBase("Text4"), servername, IRCConfig.List[servername].MasterChannel);
								}
								else if(id == Convert.ToInt32(db2["Id"].ToString()) && ignore)
									Log.Warning("SchumixBase", sLConsole.SchumixBase("Text7"));
							}
						}
					}

					NewServerSqlData(sn.Value.ServerId, sn.Key);
					IsAllSchumixFunction(sn.Value.ServerId, sn.Key);

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
						Log.Error("SchumixBase", sLConsole.ChannelInfo("Text11"));
				}

				Log.Debug("SchumixBase", sLConsole.SchumixBase("Text9"));
				sCleanManager = new CleanManager();
				sCleanManager.Initialize();

				if(AddonsConfig.Enabled)
				{
					Log.Debug("SchumixBase", sLConsole.SchumixBase("Text5"));
					sAddonManager.Initialize();
					sAddonManager.LoadPluginsFromDirectory(AddonsConfig.Directory);
				}
			}
			catch(Exception e)
			{
				Log.Error("SchumixBase", sLConsole.Exception("Error"), e.Message);
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
			packet.Write<string>("utf-8");
			packet.Write<string>(LocalizationConfig.Locale);
			packet.Write<string>(Reconnect.ToString());
			packet.Write<string>(ServerIdentify);
			ClientSocket.SendPacketToSCS(packet);
		}

		public static void Quit(bool Reconnect = true)
		{
			lock(WriteLock)
			{
				if(ExitStatus)
					return;

				ExitStatus = true;
				var memory = Process.GetCurrentProcess().WorkingSet64;
				sAddonManager.UnloadPlugins();
				sUtilities.RemovePidFile();
				timer.SaveUptime(memory);
				sCacheDB.Clean();
				ServerDisconnect(Reconnect);
			}
		}

		private void NewServerSqlData(int ServerId, string ServerName)
		{
			var db = DManager.QueryFirstRow("SELECT * FROM channels WHERE ServerId = '{0}'", ServerId);
			if(db.IsNull())
				DManager.Insert("`channels`(ServerId, ServerName, Channel, Password, Language)", ServerId, ServerName, IRCConfig.List[ServerName].MasterChannel, IRCConfig.List[ServerName].MasterChannelPassword, sLManager.Locale);

			db = DManager.QueryFirstRow("SELECT * FROM schumix WHERE ServerId = '{0}'", ServerId);
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

		private void IsAllSchumixFunction(int ServerId, string ServerName)
		{
			foreach(var function in Enum.GetNames(typeof(IFunctions)))
			{
				var db = DManager.QueryFirstRow("SELECT * FROM schumix WHERE ServerId = '{0}' And FunctionName = '{1}'", ServerId, function.ToLower());
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
	}
}