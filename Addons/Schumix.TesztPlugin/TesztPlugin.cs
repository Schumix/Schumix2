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
using System.Collections.Generic;
using Schumix.API;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.TesztPlugin.Commands;

namespace Schumix.TesztPlugin
{
	public class SchumixPlugin : TesztCommand, ISchumixBase
	{
		public SchumixPlugin()
		{

		}

		~SchumixPlugin()
		{
			Log.Debug("TesztPlugin", "~SchumixPlugin()");
		}

		public void Setup()
		{
			CommandManager.PublicRegisterHandler("teszt", Teszt);
		}

		public void Destroy()
		{
			CommandManager.PublicRemoveHandler("teszt");
		}

		public string Name
		{
			get
			{
				return "TesztPlugin";
			}
		}

		public string Author
		{
			get
			{
				return "Megax";
			}
		}

		public string Website
		{
			get
			{
				return "http://www.github.com/megax/Schumix2";
			}
		}
	}
}
