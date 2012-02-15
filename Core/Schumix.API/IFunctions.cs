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
using System.Collections.Generic;

namespace Schumix.API
{
	public enum IFunctions
	{
		Greeter,
		Log,
		Rejoin,
		Commands,
		Reconnect,
		Autohl,
		Autokick,
		Automode,
		Svn,
		Hg,
		Git,
		Antiflood,
		Message,
		Compiler,
		Gamecommands,
		Webtitle,
		Randomkick,
		Mantisbt,
		Wordpress,
		Chatterbot
	};

	public enum IChannelFunctions
	{
		Greeter,
		Log,
		Rejoin,
		Commands,
		Autohl,
		Autokick,
		Automode,
		Antiflood,
		Message,
		Compiler,
		Gamecommands,
		Webtitle,
		Randomkick,
		Chatterbot
	};

	public class IFunctionsClass
	{
		public static readonly Dictionary<string, string> Functions = new Dictionary<string, string>();
	}
}