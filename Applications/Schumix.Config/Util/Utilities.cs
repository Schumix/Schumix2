/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Net;
using System.Web;
using System.Text;
using Schumix.Config.Extensions;

namespace Schumix.Config.Util
{
	sealed class Utilities
	{
		private Utilities() {}

		public string GetVersion()
		{
			return Schumix.Config.Config.Consts.ConfigVersion;
		}

		public void CreateDirectory(string Name)
		{
			if(!Directory.Exists(Name))
				Directory.CreateDirectory(Name);
		}

		public void CreateFile(string Name)
		{
			if(!File.Exists(Name))
				new FileStream(Name, FileMode.Append, FileAccess.Write, FileShare.Write).Close();
		}

		public void ClearAttributes(string currentDir)
		{
			if(Directory.Exists(currentDir))
			{
				var subDirs = Directory.GetDirectories(currentDir);

				foreach(string dir in subDirs)
					ClearAttributes(dir);

				var files = Directory.GetFiles(currentDir);

				foreach(string file in files)
					File.SetAttributes(file, FileAttributes.Normal);
			}
		}
	}
}