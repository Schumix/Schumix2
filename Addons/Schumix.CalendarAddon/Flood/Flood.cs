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
using Schumix.Framework;

namespace Schumix.CalendarAddon
{
	public sealed class Flood
	{
		private string _name;
		private string _channel;
		public int Piece;
		public int Message;

		public Flood(string name, string channel)
		{
			_name = name;
			_channel = channel;
		}

		public string Name
		{
			get { return _name; }
		}

		public string Channel
		{
			get { return _channel; }
		}
	}
}