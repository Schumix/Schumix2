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
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	public sealed class Timer
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		/// <summary>
		///     A bot elindításának ideje.
		/// </summary>
		public readonly DateTime StartTime;
		public readonly Stopwatch SW = new Stopwatch();

		public Timer()
		{
			try
			{
				Log.Notice("Timer", sLConsole.Timer("Text"));
				SW.Start();
				StartTime = DateTime.Now;
				Log.Debug("Timer", sLConsole.Timer("Text2"));
			}
			catch(Exception e)
			{
				Log.Error("Timer", sLConsole.Exception("Error"), e.Message);
				Thread.Sleep(100);
			}
		}

		public void StartTimer()
		{
			SW.Stop();
			Log.Debug("Timer", sLConsole.Timer("Text3"), SW.ElapsedMilliseconds);
		}

		/// <returns>
		///     Megmutatja mennyi ideje üzemel a program.
		/// </returns>
		public string Uptime()
		{
			var Time = DateTime.Now - StartTime;
			return string.Format(sLConsole.Timer("Uptime"), Time.Days, Time.Hours, Time.Minutes, Time.Seconds);
		}

		public string Uptime(string Language)
		{
			var Time = DateTime.Now - StartTime;
			return string.Format(sLConsole.Timer("Uptime", Language), Time.Days, Time.Hours, Time.Minutes, Time.Seconds);
		}

		public void SaveUptime()
		{
			string date = string.Empty;
			int month = DateTime.Now.Month;
			int day = DateTime.Now.Day;
			var memory = Process.GetCurrentProcess().WorkingSet64/1024/1024;

			if(month < 10)
			{
				if(day < 10)
					date = string.Format("{0}. 0{1}. 0{2}. {3}:{4}", DateTime.Now.Year, month, day, DateTime.Now.Hour, DateTime.Now.Minute);
				else
					date = string.Format("{0}. 0{1}. {2}. {3}:{4}", DateTime.Now.Year, month, day, DateTime.Now.Hour, DateTime.Now.Minute);
			}
			else
			{
				if(day < 10)
					date = string.Format("{0}. {1}. 0{2}. {3}:{4}", DateTime.Now.Year, month, day, DateTime.Now.Hour, DateTime.Now.Minute);
				else
					date = string.Format("{0}. {1}. {2}. {3}:{4}", DateTime.Now.Year, month, day, DateTime.Now.Hour, DateTime.Now.Minute);
			}

			SchumixBase.DManager.QueryFirstRow("INSERT INTO `uptime`(datum, uptime, memory) VALUES ('{0}', '{1}', '{2} MB')", date, Uptime(), memory);
		}
	}
}
