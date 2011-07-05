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
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Database;
using Schumix.Framework.Localization;

namespace Schumix
{
	public sealed class SchumixBot : SchumixBase
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;

		public SchumixBot()
		{
			try
			{
				Log.Notice("SchumixBot", sLConsole.SchumixBot("Text"));
				Log.Debug("SchumixBot", sLConsole.SchumixBot("Text1"));
				var network = new Network(IRCConfig.Server, IRCConfig.Port);
				Log.Debug("SchumixBot", sLConsole.SchumixBot("Text2"));
				new Console.Console(network);
			}
			catch(Exception e)
			{
				Log.Error("SchumixBot", sLConsole.Exception("Error"), e.Message);
				Thread.Sleep(100);
			}
		}

		/// <summary>
		///     Ha lefut, akkor leáll a class.
		/// </summary>
		~SchumixBot()
		{
			Log.Debug("SchumixBot", "~SchumixBot()");
		}
	}
}