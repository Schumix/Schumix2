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
using System.IO;

namespace Schumix.Config.CopyTo
{
	sealed class Copy
	{
		/// <summary>
		///     Több helyről átmásolja az új fájlokat.
		/// </summary>
		public Copy(string Dir, string Addons, string Configs)
		{
			var dir = new DirectoryInfo(Dir + "/Run/Release/Addons");

			foreach(var file in diraddons.GetFiles())
			{
				File.Delete(file.Name);
				File.Move(Dir + "/Run/Release/" + file.Name, Addons + "/" + file.Name);
			}

			dir = new DirectoryInfo(Dir + "/Run/Release/");

			foreach(var file in dir.GetFiles())
			{
				if(file.Name.ToLower().Contains(".db3"))
					continue;

				File.Delete(file.Name);
				File.Move(Dir + "/Run/Release/" + file.Name, file.Name);
			}

			dir = new DirectoryInfo(Configs);

			foreach(var fi in dir.GetFiles())
				File.Move("Configs/" + fi.Name, "Configs/_" + fi.Name);
		}
	}
}