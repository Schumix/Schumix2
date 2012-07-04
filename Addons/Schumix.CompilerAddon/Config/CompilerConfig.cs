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
using Schumix.Framework;
using Schumix.CompilerAddon.Localization;

namespace Schumix.CompilerAddon.Config
{
	sealed class CompilerConfig
	{
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		public static bool CompilerEnabled { get; private set; }
		public static bool MaxAllocatingE { get; private set; }
		public static int MaxAllocatingM { get; private set; }
		public static string CompilerOptions { get; private set; }
		public static int WarningLevel { get; private set; }
		public static bool TreatWarningsAsErrors { get; private set; }
		public static string Referenced { get; private set; }
		public static string[] ReferencedAssemblies { get; private set; }
		public static string MainClass { get; private set; }
		public static string MainConstructor { get; private set; }

		public CompilerConfig(bool compilerenabled, bool maxallocatinge, int maxallocatingm, string compileroptions, int warninglevel, bool treatwarningsaserrors, string referenced, string referencedassemblies, string mainclass, string mainconstructor)
		{
			CompilerEnabled       = compilerenabled;
			MaxAllocatingE        = maxallocatinge;
			MaxAllocatingM        = maxallocatingm;
			CompilerOptions       = compileroptions;
			WarningLevel          = warninglevel;
			TreatWarningsAsErrors = treatwarningsaserrors;
			Referenced            = referenced;
			ReferencedAssemblies  = referencedassemblies.Split(',');
			MainClass             = mainclass;
			MainConstructor       = mainconstructor;
			Log.Notice("CompilerConfig", sLocalization.CompilerConfig("Text"));
		}
	}
}