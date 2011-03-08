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
using System.Threading;
using System.Diagnostics;

namespace Schumix.Framework
{
	public sealed class Time
	{
		/// <summary>
		///     A bot elindításának ideje.
		/// </summary>
		public readonly DateTime StartTime;
		public readonly Stopwatch SW = new Stopwatch();

		public Time()
		{
			try
			{
				Log.Notice("Time", "Time elindult.");
				SW.Start();
				StartTime = DateTime.Now;
			}
			catch(Exception e)
			{
				Log.Error("Time", "Hiba oka: {0}", e.ToString());
				Thread.Sleep(100);
			}
		}

		public void IndulasiIdo()
		{
			SW.Stop();
			Log.Debug("Time", "A program {0}ms alatt indult el.", SW.ElapsedMilliseconds);
		}

		/// <returns>
		///     Megmutatja mennyi ideje üzemel a program.
		/// </returns>
		public string Uptime()
		{
			var Time = DateTime.Now - StartTime;
			return String.Format("{0} nap, {1} óra, {2} perc, {3} másodperc.", Time.Days, Time.Hours, Time.Minutes, Time.Seconds);
		}

		public void SaveUptime()
		{
			string datum = "";
			int Ev = DateTime.Now.Year;
			int Honap = DateTime.Now.Month;
			int Nap = DateTime.Now.Day;
			int Ora = DateTime.Now.Hour;
			int Perc = DateTime.Now.Minute;
			var mem = Process.GetCurrentProcess().WorkingSet64/1024/1024;

			if(Honap < 10)
			{
				if(Nap < 10)
					datum = String.Format("{0}. 0{1}. 0{2}. {3}:{4}", Ev, Honap, Nap, Ora, Perc);
				else
					datum = String.Format("{0}. 0{1}. {2}. {3}:{4}", Ev, Honap, Nap, Ora, Perc);
			}
			else
			{
				if(Nap < 10)
					datum = String.Format("{0}. {1}. 0{2}. {3}:{4}", Ev, Honap, Nap, Ora, Perc);
				else
					datum = String.Format("{0}. {1}. {2}. {3}:{4}", Ev, Honap, Nap, Ora, Perc);
			}

			SchumixBase.DManager.QueryFirstRow("INSERT INTO `uptime`(datum, uptime, memory) VALUES ('{0}', '{1}', '{2} MB')", datum, Uptime(), mem);
		}
	}
}
