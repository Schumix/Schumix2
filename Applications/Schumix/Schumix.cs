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
using System.Threading;
using System.Threading.Tasks;
using Schumix.API;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Database;
using Schumix.Framework.Localization;

namespace Schumix
{
	/// <summary>
	///     Fő class. Innen indul a konzol vezérlés és az irc kapcsolat létrehozása.
	/// </summary>
	sealed class SchumixBot
	{
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationConsole segítségével állíthatók be a konzol nyelvi tulajdonságai.
		/// </summary>
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		public static SchumixBase sSchumixBase { get; private set; }

		/// <summary>
		///     Indulási függvény.
		/// </summary>
		public SchumixBot()
		{
			try
			{
				bool e = false;
				string eserver = string.Empty;
				Log.Notice("SchumixBot", sLConsole.SchumixBot("Text"));
				Log.Debug("SchumixBot", sLConsole.SchumixBot("Text2"));

				foreach(var sn in IRCConfig.List)
				{
					if(!e)
					{
						eserver = sn.Key;
						e = true;
					}

					sIrcBase.NewServer(sn.Key, sn.Value.ServerId, sn.Value.Server, sn.Value.Port);
				}

				sSchumixBase = new SchumixBase();

				Task.Factory.StartNew(() =>
				{
					if(IRCConfig.List.Count == 1)
					{
						sIrcBase.Connect(eserver);
						return;
					}

					foreach(var sn in IRCConfig.List)
					{
						sIrcBase.Connect(sn.Key);

						while(!sIrcBase.Networks[sn.Key].Online)
							Thread.Sleep(1000);
					}
				});

				Log.Debug("SchumixBot", sLConsole.SchumixBot("Text3"));
				new ScriptManager(ScriptsConfig.Directory);
				new Console.Console(eserver);
			}
			catch(Exception e)
			{
				Log.Error("SchumixBot", sLConsole.Exception("Error"), e);
				Thread.Sleep(100);
			}
		}

		/// <summary>
		///     Destruktor.
		/// </summary>
		/// <remarks>
		///     Ha ez lefut, akkor a class leáll.
		/// </remarks>
		~SchumixBot()
		{
			Log.Debug("SchumixBot", "~SchumixBot()");
		}
	}
}