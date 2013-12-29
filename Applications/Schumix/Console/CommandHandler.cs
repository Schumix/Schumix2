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
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Addon;
using Schumix.Framework.Logger;
using Schumix.Framework.Config;
using Schumix.Framework.Platforms;
using Schumix.Framework.Localization;

namespace Schumix.Console
{
	/// <summary>
	///     CommandHandler class.
	/// </summary>
	partial class CommandHandler : ConsoleLog
	{
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationConsole segítségével állíthatók be a konzol nyelvi tulajdonságai.
		/// </summary>
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     LocalizationManager segítségével állítható be az irc szerver felé menő tárolt üzenetek nyelvezete.
		/// </summary>
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Addonok kezelése.
		/// </summary>
		private readonly AddonManager sAddonManager = Singleton<AddonManager>.Instance;
		/// <summary>
		///     Hozzáférést biztosít singleton-on keresztül a megadott class-hoz.
		///     Utilities sokféle függvényt tartalmaz melyek hasznosak lehetnek.
		/// </summary>
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly Platform sPlatform = Singleton<Platform>.Instance;
		private readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		protected string _servername;
		/// <summary>
		///     Csatorna nevét tárolja.
		/// </summary>
		protected string _channel;

		/// <summary>
		///     Indulási függvény.
		/// </summary>
		protected CommandHandler() : base(LogConfig.IrcLog)
		{

		}
	}
}