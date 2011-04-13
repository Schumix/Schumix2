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

namespace Schumix
{
	public sealed class SchumixBot : SchumixBase
	{
		public SchumixBot()
		{
			try
			{
				Log.Notice("SchumixBot", "SchumixBot sikeresen elindult.");
				Log.Debug("SchumixBot", "Network indul...");
				var network = new Network(IRCConfig.Server, IRCConfig.Port);
				Log.Debug("SchumixBot", "Console indul...");
				new Console.Console(network);
			}
			catch(Exception e)
			{
				Log.Error("SchumixBot", "Hiba oka: {0}", e.Message);
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