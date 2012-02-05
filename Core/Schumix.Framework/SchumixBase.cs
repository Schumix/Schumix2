/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2011 Megax <http://www.megaxx.info/>
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
using Schumix.API;
using Schumix.Framework.Client;
using Schumix.Framework.Config;
using Schumix.Framework.Database;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	public abstract class SchumixBase
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private static readonly AddonManager sAddonManager = Singleton<AddonManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private static readonly Guid _guid = Guid.NewGuid();
		public static DatabaseManager DManager { get; private set; }
		public static Timer timer { get; private set; }
		public const string Title = "Schumix2 IRC Bot and Framework";
		public static string ServerIdentify = string.Empty;
		public static bool UrlTitleEnabled = false;
		public static bool ExitStatus = false;
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

		/// <summary>
		/// Gets the GUID.
		/// </summary>
		public static Guid GetGuid() { return _guid; }

		protected SchumixBase()
		{
			try
			{
				if(ServerConfig.Enabled)
				{
					var listener = new ClientSocket(ServerConfig.Host, ServerConfig.Port, ServerConfig.Password);
					Log.Debug("SchumixServer", sLConsole.SchumixBase("Text6"));
					listener.Socket();

					while(ThreadStop)
						Thread.Sleep(100);
				}

				if(sUtilities.GetCompiler() == Compiler.Mono)
					System.Net.ServicePointManager.ServerCertificateValidationCallback += (s,ce,ca,p) => true;

				Log.Debug("SchumixBase", sLConsole.SchumixBase("Text"));
				timer = new Timer();
				Log.Debug("SchumixBase", sLConsole.SchumixBase("Text2"));
				DManager = new DatabaseManager();
				Log.Notice("SchumixBase", sLConsole.SchumixBase("Text3"));

				var db = SchumixBase.DManager.Query("SELECT FunctionName, FunctionStatus FROM schumix");
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						string name = row["FunctionName"].ToString();
						string status = row["FunctionStatus"].ToString();
						IFunctionsClass.Functions.Add(name.ToLower(), status.ToLower());
					}
				}
				else
					Log.Error("SchumixBase", sLConsole.ChannelInfo("Text11"));

				SchumixBase.DManager.Update("channel", string.Format("Channel = '{0}'", IRCConfig.MasterChannel), "Id = '1'");
				Log.Notice("SchumixBase", sLConsole.SchumixBase("Text4"), IRCConfig.MasterChannel);

				if(AddonsConfig.Enabled)
				{
					Log.Debug("SchumixBase", sLConsole.SchumixBase("Text5"));
					sAddonManager.Initialize();
					sAddonManager.LoadPluginsFromDirectory(AddonsConfig.Directory);
				}

				sLManager.Locale = LocalizationConfig.Locale;
			}
			catch(Exception e)
			{
				Log.Error("SchumixBase", sLConsole.Exception("Error"), e.Message);
				Thread.Sleep(100);
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
			packet.Write<string>(SchumixBase.ServerIdentify);
			ClientSocket.SendPacketToSCS(packet);
		}

		public static void Quit()
		{
			foreach(var plugin in sAddonManager.GetPlugins())
				plugin.Destroy();

			SchumixBase.timer.SaveUptime();
			SchumixBase.ServerDisconnect();
			SchumixBase.ExitStatus = true;
		}
	}
}