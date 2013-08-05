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
using NGit;
using NGit.Api;

namespace Schumix.Installer.Download
{
	sealed class CloneSchumix
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		public CloneSchumix(string Url, string Dir)
		{
			string path = Path.Combine(Environment.CurrentDirectory, Dir);

			if(Directory.Exists(path))
			{
				sUtilities.ClearAttributes(path);
				Directory.Delete(path, true);
			}

			var cmd = Git.CloneRepository();
			cmd.SetURI(Url);
			cmd.SetRemote("origin");
			cmd.SetBranch("refs/heads/stable");
			cmd.SetDirectory(path);
			cmd.SetCloneSubmodules(true);
			cmd.Call();
		}
	}
}