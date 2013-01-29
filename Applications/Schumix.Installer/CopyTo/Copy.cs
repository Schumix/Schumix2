/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013 Schumix Team <http://schumix.eu/>
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

namespace Schumix.Installer.CopyTo
{
	sealed class Copy
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		/// <summary>
		///     Több helyről átmásolja az új fájlokat.
		/// </summary>
		public Copy(string Dir)
		{
			sUtilities.CreateDirectory("Addons");
			string ndir = Dir;

			if(Environment.Is64BitOperatingSystem)
				Dir = Dir + "/Run/Release_x64";
			else
				Dir = Dir + "/Run/Release";

			var dir = new DirectoryInfo(Dir + "/Addons");

			foreach(var file in dir.GetFiles())
			{
				if(file.Name.ToLower().Contains(".db3"))
					continue;

				File.Move(Dir + "/Addons/" + file.Name, "Addons/" + file.Name);
			}

			if(Directory.Exists(ndir + "/Scripts"))
				Directory.Move(ndir + "/Scripts", "Scripts");

			if(Directory.Exists(Dir + "/locale"))
				Directory.Move(Dir + "/locale", "locale");

			if(Directory.Exists(ndir + "/Configs"))
				Directory.Move(ndir + "/Configs", "Configs");

			dir = new DirectoryInfo(Dir);

			foreach(var file in dir.GetFiles())
			{
				if(File.Exists(file.Name))
					continue;

				File.Move(Dir + "/" + file.Name, file.Name);
			}
		}
	}
}