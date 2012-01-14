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
using System.Diagnostics;
using Schumix.Framework;

namespace Schumix.Server.New
{
	class Schumix
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private Schumix() {}

		public void Start(string File, string Dir, string Encoding, string Locale)
		{
			var exe = new Process();
			exe.StartInfo.UseShellExecute = false;
			exe.StartInfo.RedirectStandardOutput = true;
			exe.StartInfo.RedirectStandardError = true;

			if(sUtilities.GetCompiler() == Compiler.Mono)
			{
				exe.StartInfo.FileName = "mono";
				exe.StartInfo.Arguments = string.Format("Schumix.exe --config-dir={0} --config-file={1} --console-encoding={2} --console-localization={3} --server-enabled={4} --server-host={5} --server-port={6} --server-password={7}", Dir, File, Encoding, Locale, true, "127.0.0.1", Config.ServerConfigs.ListenerPort, Config.ServerConfigs.Password);
			}
			else if(sUtilities.GetCompiler() == Compiler.VisualStudio)
			{
				exe.StartInfo.FileName = "Schumix.exe";
				exe.StartInfo.Arguments = string.Format("--config-dir={0} --config-file={1} --console-encoding={2} --console-localization={3} --server-enabled={4} --server-host={5} --server-port={6} --server-password={7}", Dir, File, Encoding, Locale, true, "127.0.0.1", Config.ServerConfigs.ListenerPort, Config.ServerConfigs.Password);
			}

			exe.Start();
			exe.Dispose();
		}
	}
}