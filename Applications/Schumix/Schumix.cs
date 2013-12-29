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
using System.Threading.Tasks;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Database;
using Schumix.Framework.Localization;
using Schumix.Components;

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
				Log.Notice("SchumixBot", sLConsole.GetString("Successfully started SchumixBot."));
				Log.Debug("SchumixBot", sLConsole.GetString("Network starting..."));

				string eserver = sIrcBase.FirstStart();
				sSchumixBase = new SchumixBase();
				sIrcBase.Start(eserver);

				Log.Debug("SchumixBot", sLConsole.GetString("Console starting..."));
				new ScriptManager(ScriptsConfig.Directory);
				new Console.Console(eserver);
			}
			catch(Exception e)
			{
				Log.Error("SchumixBot", sLConsole.GetString("Failure details: {0}"), e.Message);
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