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
using System.IO;
using System.Collections.Generic;
using Schumix.Config.Util;

namespace Schumix.Config.CopyTo
{
	sealed class Copy
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		/// <summary>
		///     Több helyről átmásolja az új fájlokat.
		/// </summary>
		public Copy(string Dir, string Addons, string Configs)
		{
			string ndir = Dir;
			Dir = Dir + "/Run/Release";

			var dir = new DirectoryInfo(Dir + "/Addons");

			foreach(var file in dir.GetFiles())
			{
				if(file.Name.ToLower().Contains(".db3"))
					continue;

				if(File.Exists(Addons + "/" + file.Name))
					File.Delete(Addons + "/" + file.Name);

				File.Move(Dir + "/Addons/" + file.Name, Addons + "/" + file.Name);
			}

			dir = new DirectoryInfo(Dir);

			foreach(var file in dir.GetFiles())
			{
				if(file.Name.ToLower().Contains(".db3"))
					continue;

				if(File.Exists(file.Name))
					File.Delete(file.Name);

				File.Move(Dir + "/" + file.Name, file.Name);
			}

			if(Directory.Exists(ndir + "/Scripts"))
				CopyDirectory(ndir + "/Scripts", "Scripts");
			
			if(Directory.Exists(Dir + "/locale"))
				CopyDirectory(Dir + "/locale", "locale");

			dir = new DirectoryInfo(Configs);

			foreach(var fi in dir.GetFiles())
			{
				if(fi.Name.Substring(0, 1) == "_")
					continue;

				if("Configs/" + fi.Name == Configs + "/_" + fi.Name)
					continue;

				File.Move("Configs/" + fi.Name, Configs + "/_" + fi.Name);
			}
		}

		public void CopyDirectory(string source, string target)
		{
			var stack = new Stack<Folders>();
			stack.Push(new Folders(source, target));

			while(stack.Count > 0)
			{
				var folders = stack.Pop();
				sUtilities.CreateDirectory(folders.Target);

				foreach(var file in Directory.GetFiles(folders.Source, "*.*"))
				{
					string targetFile = Path.Combine(folders.Target, Path.GetFileName(file));

					if(File.Exists(targetFile))
						File.Delete(targetFile);

					File.Move(file, targetFile);
				}

				foreach(var folder in Directory.GetDirectories(folders.Source))
				{
					stack.Push(new Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
				}
			}
		}
	}
}