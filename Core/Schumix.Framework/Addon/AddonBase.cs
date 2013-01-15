/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2012 Twl
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
using System.Reflection;
using System.Collections.Generic;
using Schumix.Api;

namespace Schumix.Framework.Addon
{
	public sealed class AddonBase
	{
		public readonly Dictionary<string, ISchumixAddon> Addons = new Dictionary<string, ISchumixAddon>();

		/// <summary>
		/// List of found assemblies.
		/// </summary>
		public readonly Dictionary<string, Assembly> IgnoreAssemblies = new Dictionary<string, Assembly>();
		public readonly Dictionary<string, Assembly> Assemblies = new Dictionary<string, Assembly>();
		public AddonBase() {}
	}
}