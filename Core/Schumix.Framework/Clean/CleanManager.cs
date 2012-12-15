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
using Schumix.Framework.Localization;

namespace Schumix.Framework.Clean
{
	public sealed class CleanManager
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private bool _server = false;
		public CleanDatabase CDatabase { get; private set; }

		public CleanManager(bool server = false)
		{
			Log.Notice("CleanManager", sLConsole.CleanManager("Text"));
			_server = server;
		}

		public void Initialize()
		{
			int cleanerror = 0;

			if(_server)
			{
				if(true)
				{
					Log.Debug("CleanManager", sLConsole.CleanManager("Text2"));
					var config = new CleanConfig();
					if(!config.IsClean())
						cleanerror++;
				}
			}
			else
			{
				if(true)
				{
					Log.Debug("CleanManager", sLConsole.CleanManager("Text2"));
					var config = new CleanConfig();
					if(!config.IsClean())
						cleanerror++;
				}

				if(true)
				{
					Log.Debug("CleanManager", sLConsole.CleanManager("Text3"));
					CDatabase = new CleanDatabase();
					if(!CDatabase.IsClean())
						cleanerror++;
				}
			}

			if(cleanerror > 0)
				Log.Warning("CleanManager", sLConsole.CleanManager("Text4"));

			if((Schumix.Framework.Config.CleanConfig.Config || Schumix.Framework.Config.CleanConfig.Database) && cleanerror == 0)
				Log.Notice("CleanManager", sLConsole.CleanManager("Text5"));
			else if(cleanerror == 0)
				Log.Warning("CleanManager", sLConsole.CleanManager("Text6"));
		}
	}
}