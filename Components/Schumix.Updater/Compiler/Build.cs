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

namespace Schumix.Updater.Compiler
{
	sealed class Build
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
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

			if(sUtilities.GetPlatformType() == PlatformType.Linux)
			{
				File.Copy(ToolLocationHelper.GetPathToDotNetFramework(TargetDotNetFrameworkVersion.Version40) + "/xbuild.exe", dir + "/xbuild.exe");
				build.StartInfo.FileName = "mono";
				build.StartInfo.Arguments = dir + "/xbuild.exe /p:Configuration=\"Release\" " + dir + "/Schumix.sln";
			}
			else if(sUtilities.GetPlatformType() == PlatformType.Windows)
			{
				File.Copy(ToolLocationHelper.GetPathToDotNetFramework(TargetDotNetFrameworkVersion.Version40) + "\\MSBuild.exe", dir + "\\MSBuild.exe");
				build.StartInfo.FileName = dir + "\\MSBuild.exe";
				build.StartInfo.Arguments = "/p:Configuration=\"Release\" " + dir + "/Schumix.sln";
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