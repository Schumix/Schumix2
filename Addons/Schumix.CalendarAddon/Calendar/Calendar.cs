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
using Schumix.Framework.Localization;
using Schumix.CalendarAddon.Config;
using Schumix.CalendarAddon.Localization;

namespace Schumix.CalendarAddon
{
	public sealed class Calendar
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly Ban sBan = Singleton<Ban>.Instance;
		private readonly Unban sUnban = Singleton<Unban>.Instance;
		private System.Timers.Timer _timerflood = new System.Timers.Timer();
		private System.Timers.Timer _timerunban = new System.Timers.Timer();
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

			// Unban
			_timerunban.Interval = 60*1000;
			_timerunban.Elapsed += HandleTimerUnbanElapsed;
			_timerunban.Enabled = true;
			_timerunban.Start();
		}

		public void Stop()
		{
			// Flood
			_timerflood.Enabled = false;
			_timerflood.Elapsed -= HandleTimerFloodElapsed;
			_timerflood.Stop();

			// Unban
			_timerunban.Enabled = false;
			_timerunban.Elapsed -= HandleTimerUnbanElapsed;
			_timerunban.Stop();
		}
		
		private void HandleTimerFloodElapsed(object sender, ElapsedEventArgs e)
		{
			UpdateFlood();
		}

		private void HandleTimerUnbanElapsed(object sender, ElapsedEventArgs e)
		{
			UpdateUnban();
		}

		private void UpdateFlood()
		{
			try
			{
				Task.Factory.StartNew(() => Flood());
			}
			catch(Exception e)
			{
				Log.Error("Calendar", sLocalization.Exception("Error"), e.Message);
			}
		}

		private void UpdateUnban()
		{
			try
			{
				Task.Factory.StartNew(() => Unban());
			}
			catch(Exception e)
			{
				Log.Error("Calendar", sLocalization.Exception("Error2"), e.Message);
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
						var time = DateTime.Now;
						if(time.Minute < 30)
							sBan.BanName(list.Name, list.Channel, sLManager.GetWarningText("RecurrentFlooding", list.Channel), DateTime.Now.Hour, DateTime.Now.Minute+30);
						else if(time.Minute >= 30)
							sBan.BanName(list.Name, list.Channel, sLManager.GetWarningText("RecurrentFlooding", list.Channel), DateTime.Now.Hour+1, DateTime.Now.Minute-30);

						list.Piece = 0;
					}
					else
					{
						if(list.Message >= CalendarConfig.NumberOfMessages)
						{
							sSender.Kick(list.Channel, list.Name, sLManager.GetWarningText("StopFlooding", list.Channel));
							list.Message = 0;
							list.Piece++;
						}
						else
							list.Message = 0;
					}
				}
			}
		}

		private void Unban()
		{
			var time = DateTime.Now;
			var db = SchumixBase.DManager.Query("SELECT Name, Channel, Reason, Year, Month, Day, Hour, Minute FROM banned");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string name = row["Name"].ToString();
					string channel = row["Channel"].ToString();
					int year = Convert.ToInt32(row["Year"].ToString());

					if(time.Year > year)
						sUnban.UnbanName(name, channel);
					else if(time.Year < year)
						continue;
					else if(time.Year == year)
					{
						int month = Convert.ToInt32(row["Month"].ToString());

						if(time.Month > month)
							sUnban.UnbanName(name, channel);
						else if(time.Month < month)
							continue;
						else
						{
							int day = Convert.ToInt32(row["Day"].ToString());

							if(time.Month == month)
							{
								if(time.Day > day)
									sUnban.UnbanName(name, channel);
								else if(time.Day < day)
									continue;
								else
								{
									if(time.Day == day)
									{
										int hour = Convert.ToInt32(row["Hour"].ToString());

										if(time.Hour > hour)
											sUnban.UnbanName(name, channel);
										else if(time.Hour < hour)
											continue;
										else
										{
											if(time.Hour == hour)
											{
												int minute = Convert.ToInt32(row["Minute"].ToString());

												if(time.Minute > minute)
													sUnban.UnbanName(name, channel);
												else if(time.Minute < minute)
													continue;
												else
												{
													if(time.Minute == minute)
														sUnban.UnbanName(name, channel);
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