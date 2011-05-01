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
using System.Threading;
using System.Threading.Tasks;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Console.Commands;

namespace Schumix.Console
{
	public sealed class Console
	{
		private readonly SendMessage sSendMessage = Singleton<SendMessage>.Instance;
		private readonly CCommandManager CCManager;

		/// <summary>
		///     Console írást indítja.
		/// </summary>
		/// <remarks>
		///     Ha érzékel valamit, akkor alapértelmezésben az IRC szobába írja,
		///     ha azt parancsnak érzékeli, akkor végrehajtja azt.
		/// </remarks>
		public Console(Network network)
		{
			Log.Notice("Console", "Console sikeresen elindult.");
			Log.Debug("Console", "Console parancs olvasoja indul...");
			Task.Factory.StartNew(() => ConsoleRead());
			CCManager = new CCommandManager(network);

			var db = SchumixBase.DManager.QueryFirstRow("SELECT csatorna FROM schumix WHERE entry = '1'");
			if(!db.IsNull())
				System.Console.Title = SchumixBase.Title + " || Console Writing Channel: " + db["csatorna"].ToString();
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
				string uzenet;
				Log.Notice("Console", "Console parancs olvaso resze elindult.");

				while(true)
				{
					uzenet = System.Console.ReadLine();
					if(CCManager.CBejovoInfo(uzenet))
						continue;

					var db = SchumixBase.DManager.QueryFirstRow("SELECT csatorna FROM schumix WHERE entry = '1'");
					if(!db.IsNull())
						sSendMessage.SendCMPrivmsg(db["csatorna"].ToString(), uzenet);

					Thread.Sleep(1000);
				}
			}
			catch(Exception e)
			{
				Log.Error("ConsoleRead", "Hiba oka: {0}", e.Message);
				Thread.Sleep(1000);
				ConsoleRead();
			}
		}
	}
}