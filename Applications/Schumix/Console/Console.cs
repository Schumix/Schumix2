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
using System.Threading;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Console
{
	/// <summary>
	///     Console class.
	/// </summary>
	sealed class Console
	{
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationConsole segítségével állíthatók be a konzol nyelvi tulajdonságai.
		/// </summary>
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		/// <summary>
		///     ConsoleCommandManager class elérését tárolja.
		/// </summary>
		private readonly CCommandManager CCManager;
		private Thread ConsoleThread;
		/// <summary>
		/// Gets whether the Console thread is running or not.
		/// </summary>
		public bool IsRunning { get; private set; }

		/// <summary>
		///     Indulási függvény.
		/// </summary>
		public Console(string ServerName)
		{
			Log.Notice("Console", sLConsole.GetString("Successfully started the Console."));
			InitThread();

			Log.Debug("Console", sLConsole.GetString("Console reader starting..."));
			Start();

			CCManager = new CCommandManager();
			CCManager.ServerName = ServerName;
			CCManager.Channel = IRCConfig.List[ServerName].MasterChannel;
			System.Console.Title = SchumixBase.Title + " || Console Writing Channel: " + CCManager.ServerName + SchumixBase.Colon + CCManager.Channel;
		}

		/// <summary>
		///     Destruktor.
		/// </summary>
		/// <remarks>
		///     Ha ez lefut, akkor a class leáll.
		/// </remarks>
		~Console()
		{
			Log.Debug("Console", "~Console()");
		}

		private void InitThread()
		{
			ConsoleThread = new Thread(() => ConsoleRead());
			ConsoleThread.Name = "Schumix Console Thread";
		}

		/// <summary>
		/// Starts the Console thread.
		/// </summary>
		public void Start()
		{
			if(IsRunning || ConsoleThread.ThreadState == ThreadState.Running)
				return;

			Log.Notice("Console", sLConsole.GetString("Console thread is started."));
			IsRunning = true;
			ConsoleThread.Start();
		}

		/// <summary>
		/// Stops the Console thread.
		/// </summary>
		public void Stop()
		{
			if(!IsRunning || ConsoleThread.ThreadState != ThreadState.Running)
				return;

			Log.Notice("Console", sLConsole.GetString("Stopping Console thread."));
			IsRunning = false;
			ConsoleThread.Join(1500);
			ConsoleThread.Abort();
		}

		/// <summary>
		///     Console-ba beírt parancsot hajtja végre.
		/// </summary>
		/// <remarks>
		///     Ha a Console-ba beírt szöveg egy parancs, akkor ez a
		///     függvény hajtja végre.
		/// </remarks>
		private void ConsoleRead()
		{
			try
			{
				string message;
				Log.Notice("Console", sLConsole.GetString("Successfully started the Console reader."));

				while(IsRunning)
				{
					message = System.Console.ReadLine();

					if(!message.IsNull())
						Log.LogInFile(sLConsole.GetString("Console input: {0}"), message);

					if(message.IsNull() || CCManager.CIncomingInfo(message))
						continue;

					sIrcBase.Networks[CCManager.ServerName].sSendMessage.SendCMPrivmsg(CCManager.Channel, message);
					Thread.Sleep(1000);
				}
			}
			catch(Exception e)
			{
				Log.Error("ConsoleRead", sLConsole.GetString("Failure details: {0}"), e.Message);
				Thread.Sleep(1000);

				if(!SchumixBase.ExitStatus)
					ConsoleRead();
			}
		}
	}
}