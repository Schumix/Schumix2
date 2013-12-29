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
using System.Collections.Generic;

namespace Schumix.GameAddon.MaffiaGames
{
	sealed class Player
	{
		public readonly List<string> Lynch = new List<string>();
		public Rank Rank { get; private set; }
		public bool Detective { get; set; }
		public string RName { get; set; }
		public Rank DRank { get; set; }
		public bool Ghost { get; set; }
		public bool Master { get; private set; }
		public bool IsNow { get; set; }
		public DateTime MsgTime;

		public Player(Rank rank) : this(rank, false)
		{
			// None
		}

		public Player(Rank rank, bool master)
		{
			Rank = rank;
			IsNow = true;
			Ghost = false;
			Master = master;
			DRank = Rank.None;
			Detective = false;
			RName = string.Empty;
		}
	}
}