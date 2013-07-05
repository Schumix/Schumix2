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
using System.Threading;
using System.Diagnostics;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework
{
	public sealed class Timer
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Runtime sRuntime = Singleton<Runtime>.Instance;
		public readonly Stopwatch SW = new Stopwatch();
		/// <summary>
		///     A bot elindításának ideje.
		/// </summary>
		public DateTime StartTime { get; private set; }

		public Timer()
		{
			try
			{
				Log.Notice("Timer", sLConsole.GetString("Successfully loaded the Timer."));
			}
			catch(Exception e)
			{
				Log.Error("Timer", sLConsole.GetString("Failure details: {0}"), e.Message);
				Thread.Sleep(100);
			}
		}

		public void Start()
		{
			SW.Start();
			StartTime = DateTime.Now;
			Log.Debug("Timer", sLConsole.GetString("Successfully saved the Program's started time."));
		}

		public void Stop()
		{
			SW.Stop();
			Log.Debug("Timer", sLConsole.GetString("The program is loaded under {0}ms."), SW.ElapsedMilliseconds);
		}

		/// <returns>
		///     Megmutatja mennyi ideje üzemel a program.
		/// </returns>
		public string Uptime()
		{
			return Uptime(sLConsole.Locale);
		}

		public string Uptime(string Language)
		{
			var Time = DateTime.Now - StartTime;
			return string.Format(sLConsole.Timer("Uptime", Language), Time.Days, Time.Hours, Time.Minutes, Time.Seconds);
		}

		public void SaveUptime()
		{
			SaveUptime(sRuntime.MemorySize);
		}

		public void SaveUptime(long Memory)
		{
			var time = DateTime.Now;
			string date = time.ToString("yyyy. MM. dd. HH:mm:ss");
			SchumixBase.DManager.Insert("`uptime`(`Date`, `Uptime`, `Memory`)", date, Uptime(), Memory);
		}
	}
}