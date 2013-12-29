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

namespace Schumix.CompilerAddon.Config
{
	abstract class AddonDefaultConfig
	{
		protected const bool d_compilerenabled = true;
		protected const bool d_enabled = true;
		protected const int d_memory = 50;
		protected const string d_compileroptions = "/optimize";
		protected const int d_warninglevel = 4;
		protected const bool d_treatwarningsaserrors = false;
		protected const string d_referenced = "using System; using System.Threading; using System.Reflection; " +
			"using System.Threading.Tasks; using System.Linq; using System.Collections.Generic; using System.Text; " +
			"using System.Text.RegularExpressions; using System.Numerics; using Schumix.Libraries; " +
			"using Schumix.Libraries.Maths; using Schumix.Libraries.Maths.Types; using Schumix.Libraries.Extensions;";
		protected const string d_referencedassemblies = "System.dll,System.Core.dll,System.Numerics.dll,Schumix.Libraries.dll,Schumix.Compiler.dll";
		protected const string d_mainclass = "Entry";
		protected const string d_mainconstructor = "Schumix";
	}
}