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
using System.Data;
using System.Timers;
using System.Threading.Tasks;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.CalendarAddon.Config;

namespace Schumix.CalendarAddon
{
	public sealed class Calendar
	{
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly Banned sBanned = Singleton<Banned>.Instance;
		private readonly Unbanned sUnbanned = Singleton<Unbanned>.Instance;
		private System.Timers.Timer _timerflood = new System.Timers.Timer();
		private System.Timers.Timer _timerunbanned = new System.Timers.Timer();
		private int flood;

		public Calendar()
		{

		}

		public void Start()
		{
			// Flood
			_timerflood.Interval = 1000;
			_timerflood.Elapsed += HandleTimerFloodElapsed;
			_timerflood.Enabled = true;
			_timerflood.Start();

			// Unbanned
			_timerunbanned.Interval = 60*1000;
			_timerunbanned.Elapsed += HandleTimerUnbannedElapsed;
			_timerunbanned.Enabled = true;
			_timerunbanned.Start();
		}

		public void Stop()
		{
			// Flood
			_timerflood.Enabled = false;
			_timerflood.Elapsed -= HandleTimerFloodElapsed;
			_timerflood.Stop();

			// Unbann
			_timerunbanned.Enabled = false;
			_timerunbanned.Elapsed -= HandleTimerUnbannedElapsed;
			_timerunbanned.Stop();
		}
		
		private void HandleTimerFloodElapsed(object sender, ElapsedEventArgs e)
		{
			UpdateFlood();
		}

		private void HandleTimerUnbannedElapsed(object sender, ElapsedEventArgs e)
		{
			UpdateUnbanned();
		}

		private void UpdateFlood()
		{
			try
			{
				Task.Factory.StartNew(() => Flood());
			}
			catch(Exception e)
			{
				Log.Error("Calendar", "[UpdateFlood] Hiba oka: {0}", e.Message);
			}
		}

		private void UpdateUnbanned()
		{
			try
			{
				Task.Factory.StartNew(() => Unbanned());
			}
			catch(Exception e)
			{
				Log.Error("Calendar", "[UpdateUnbanned] Hiba oka: {0}", e.Message);
			}
		}

		private void Flood()
		{
			flood++;

			if(flood == CalendarConfig.Seconds)
			{
				flood = 0;

				foreach(var list in CalendarAddon.FloodList)
				{
					if(list.Piece == CalendarConfig.NumberOfFlooding)
					{
						sBanned.BannedName(list.Name, list.Channel, "Recurrent flooding!", DateTime.Now);
						list.Piece = 0;
					}
					else
					{
						if(list.Message >= CalendarConfig.NumberOfMessages)
						{
							sSender.Kick(list.Channel, list.Name, "Stop flooding!");
							list.Message = 0;
							list.Piece++;
						}
						else
							list.Message = 0;
					}
				}
			}
		}

		private void Unbanned()
		{
			var time = DateTime.Now;
			var db = SchumixBase.DManager.Query("SELECT Name, Channel, Reason, Year, Month, Day, Hour, Minute FROM banned");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string nev = row["Name"].ToString();
					string csatorna = row["Channel"].ToString();
					int ev = Convert.ToInt32(row["Year"].ToString());

					if(time.Year > ev)
						sUnbanned.UnbannedName(nev, csatorna);
					else if(time.Year < ev)
						continue;
					else if(time.Year == ev)
					{
						int honap = Convert.ToInt32(row["Month"].ToString());

						if(time.Month > honap)
							sUnbanned.UnbannedName(nev, csatorna);
						else if(time.Month < honap)
							continue;
						else
						{
							int nap = Convert.ToInt32(row["Day"].ToString());

							if(time.Month == honap)
							{
								if(time.Day > nap)
									sUnbanned.UnbannedName(nev, csatorna);
								else if(time.Day < nap)
									continue;
								else
								{
									if(time.Day == nap)
									{
										int ora = Convert.ToInt32(row["Hour"].ToString());

										if(time.Hour > ora)
											sUnbanned.UnbannedName(nev, csatorna);
										else if(time.Hour < ora)
											continue;
										else
										{
											if(time.Hour == ora)
											{
												int perc = Convert.ToInt32(row["Minute"].ToString());

												if(time.Minute > perc)
													sUnbanned.UnbannedName(nev, csatorna);
												else if(time.Minute < perc)
													continue;
												else
												{
													if(time.Minute == perc)
														sUnbanned.UnbannedName(nev, csatorna);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}