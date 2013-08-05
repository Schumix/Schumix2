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
using System.Threading;
using System.Diagnostics;
using Microsoft.Build.Utilities;
using Schumix.Framework;
using Schumix.Framework.Logger;
using Schumix.Framework.Platforms;

namespace Schumix.Components.Updater.Compiler
{
	sealed class Build
	{
		private readonly Platform sPlatform = Singleton<Platform>.Instance;
		public bool HasError { get; private set; }

		public Build(string dir)
		{
			Compile(dir);
		}

		private void Compile(string dir)
		{
			var build = new Process();
			build.StartInfo.UseShellExecute = false;
			build.StartInfo.RedirectStandardOutput = true;
			build.StartInfo.RedirectStandardError = true;

			if(sPlatform.IsLinux)
			{
				File.Copy(ToolLocationHelper.GetPathToDotNetFramework(TargetDotNetFrameworkVersion.Version40) + "/xbuild.exe", dir + "/xbuild.exe");
				build.StartInfo.FileName = "mono";
				build.StartInfo.Arguments = string.Format("{0}/xbuild.exe /p:Configuration=\"Release\" {0}/Schumix.sln /flp:LogFile=xbuild.log;Verbosity=Detailed", dir);
			}
			else if(sPlatform.IsWindows)
			{
				File.Copy(ToolLocationHelper.GetPathToDotNetFramework(TargetDotNetFrameworkVersion.Version40) + "\\MSBuild.exe", dir + "\\MSBuild.exe");
				build.StartInfo.FileName = dir + "\\MSBuild.exe";
				build.StartInfo.Arguments = string.Format("/p:Configuration=\"Release\" {0}/Schumix.sln /flp:LogFile=msbuild.log;Verbosity=Detailed", dir);
			}

			build.Start();
			build.PriorityClass = ProcessPriorityClass.Normal;

			//var error = build.StandardError;
			var output = build.StandardOutput;
			HasError = false;

			//while(!error.EndOfStream)
			//	HasError = true;

			while(!output.EndOfStream)
				Log.Debug("Build", output.ReadLine());

			build.WaitForExit();
			build.Dispose();
		}
	}
}