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
using System.Threading;
using System.Diagnostics;
using Schumix.Installer;

namespace Schumix.Installer.Compiler
{
	sealed class Build
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public bool HasError { get; private set; }

		public Build(string Version)
		{
			var t = new Thread(() => Compile(Version));
			t.Start();
			t.Join(30*1000);
			t.Abort();
			t = new Thread(() => Compile(Version));
			t.Start();
			t.Join(30*1000);
			t.Abort();
		}

		private void Compile(string Version)
		{
			var build = new Process();
			build.StartInfo.UseShellExecute = false;
			build.StartInfo.RedirectStandardOutput = true;
			build.StartInfo.RedirectStandardError = true;

			if(sUtilities.GetCompiler() == Schumix.Installer.SCompiler.Mono)
			{
			build.StartInfo.FileName = "mono";
			build.StartInfo.Arguments = "xbuild.exe /p:DocumentationFile=\"\" /p:DefineConstants=\"RELEASE,MONO\" /p:Configuration=\"Mono-Release\" /p:Platform=\"x86\" " + Version + "/Schumix.sln";
			}
			else if(sUtilities.GetCompiler() == Schumix.Installer.SCompiler.Mono)
			{
			build.StartInfo.FileName = "xbuild.exe";
			build.StartInfo.Arguments = "/p:DocumentationFile=\"\" /p:DefineConstants=\"RELEASE\" /p:Configuration=\"Release\" /p:Platform=\"x86\" " + Version + "/Schumix.sln";
			}

			build.Start();
			//var error = build.StandardError;
			HasError = false;

			//while(!error.EndOfStream)
				//HasError = true;

			build.WaitForExit();
			build.Dispose();
		}
	}
}