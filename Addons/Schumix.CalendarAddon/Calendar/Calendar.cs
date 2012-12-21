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
using System.Data;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using Schumix.API;
using Schumix.API.Functions;
using Schumix.Irc;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.CalendarAddon.Config;
using Schumix.CalendarAddon.Localization;

namespace Schumix.CalendarAddon
{
	sealed class Calendar
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly PLocalization sLocalization = Singleton<PLocalization>.Instance;
		private System.Timers.Timer _timercalendar = new System.Timers.Timer();
		private System.Timers.Timer _timernameday = new System.Timers.Timer();
		private System.Timers.Timer _timerflood = new System.Timers.Timer();
		private System.Timers.Timer _timerunban = new System.Timers.Timer();
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private CalendarFunctions sCalendarFunctions;
		private string _servername;
		private Unban sUnban;

		public Calendar(string ServerName)
		{
			_servername = ServerName;
			sUnban = new Unban(ServerName);
			sCalendarFunctions = new CalendarFunctions(ServerName);
		}

		public void Start()
		{
			// Flood
			_timerflood.Interval = CalendarConfig.Seconds * 1000;
			_timerflood.Elapsed += HandleTimerFloodElapsed;
			_timerflood.Enabled = true;
			_timerflood.Start();

			// Unban
			_timerunban.Interval = 60*1000;
			_timerunban.Elapsed += HandleTimerUnbanElapsed;
			_timerunban.Enabled = true;
			_timerunban.Start();

			// Calendar
			_timercalendar.Interval = 60*1000;
			_timercalendar.Elapsed += HandleTimerCalendarElapsed;
			_timercalendar.Enabled = true;
			_timercalendar.Start();

			// NameDay
			_timernameday.Interval = 60*1000;
			_timernameday.Elapsed += HandleTimerNameDayElapsed;
			_timernameday.Enabled = true;
			_timernameday.Start();
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

			// Calendar
			_timerunban.Enabled = false;
			_timerunban.Elapsed -= HandleTimerCalendarElapsed;
			_timerunban.Stop();

			// NameDay
			_timernameday.Enabled = false;
			_timernameday.Elapsed -= HandleTimerNameDayElapsed;
			_timernameday.Stop();
		}
		
		private void HandleTimerFloodElapsed(object sender, ElapsedEventArgs e)
		{
			UpdateFlood();
		}

		private void HandleTimerUnbanElapsed(object sender, ElapsedEventArgs e)
		{
			UpdateUnban();
		}

		private void HandleTimerCalendarElapsed(object sender, ElapsedEventArgs e)
		{
			UpdateCalendar();
		}

		private void HandleTimerNameDayElapsed(object sender, ElapsedEventArgs e)
		{
			UpdateNameDay();
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

		private void UpdateCalendar()
		{
			try
			{
				Task.Factory.StartNew(() => CalendarTimeRemove());
			}
			catch(Exception e)
			{
				Log.Error("Calendar", sLocalization.Exception("Error3"), e.Message);
			}
		}

		private void UpdateNameDay()
		{
			try
			{
				Task.Factory.StartNew(() => NameDay());
			}
			catch(Exception e)
			{
				Log.Error("Calendar", sLocalization.Exception("Error4"), e.Message);
			}
		}

		private void Flood()
		{
			foreach(var list in CalendarAddon.FloodList)
			{
				foreach(var list2 in list.Value.Channel)
					list2.Value.Message = 0;
			}
		}

		private void Unban()
		{
			var time = DateTime.Now;
			var db = SchumixBase.DManager.Query("SELECT Name, Channel, Reason, Year, Month, Day, Hour, Minute FROM banned WHERE ServerName = '{0}'", _servername);
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

		private void CalendarTimeRemove()
		{
			var time = DateTime.Now;
			var db = SchumixBase.DManager.Query("SELECT Id, Name, Channel, Message, Loops, Year, Month, Day, Hour, Minute FROM calendar WHERE ServerName = '{0}'", _servername);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					if(Convert.ToBoolean(row["Loops"].ToString()))
					{
						string name0 = row["Name"].ToString();
						string channel0 = row["Channel"].ToString();
						string message0 = row["Message"].ToString();

						int hour = Convert.ToInt32(row["Hour"].ToString());

						if(time.Hour > hour)
							continue;
						else if(time.Hour < hour)
							continue;
						else
						{
							if(time.Hour == hour)
							{
								int minute = Convert.ToInt32(row["Minute"].ToString());

								if(time.Minute > minute)
									continue;
								else if(time.Minute < minute)
									continue;
								else
								{
									if(time.Minute == minute)
										sCalendarFunctions.Write(name0, channel0, message0);
								}
							}
						}

						continue;
					}

					int id = Convert.ToInt32(row["Id"].ToString());
					string name = row["Name"].ToString();
					string channel = row["Channel"].ToString();
					string message = row["Message"].ToString();
					int year = Convert.ToInt32(row["Year"].ToString());

					if(time.Year > year)
						sCalendarFunctions.Remove(id);
					else if(time.Year < year)
						continue;
					else if(time.Year == year)
					{
						int month = Convert.ToInt32(row["Month"].ToString());

						if(time.Month > month)
							sCalendarFunctions.Remove(id);
						else if(time.Month < month)
							continue;
						else
						{
							int day = Convert.ToInt32(row["Day"].ToString());

							if(time.Month == month)
							{
								if(time.Day > day)
									sCalendarFunctions.Remove(id);
								else if(time.Day < day)
									continue;
								else
								{
									if(time.Day == day)
									{
										int hour = Convert.ToInt32(row["Hour"].ToString());

										if(time.Hour > hour)
											sCalendarFunctions.Remove(id);
										else if(time.Hour < hour)
											continue;
										else
										{
											if(time.Hour == hour)
											{
												int minute = Convert.ToInt32(row["Minute"].ToString());

												if(time.Minute > minute)
													sCalendarFunctions.Remove(id);
												else if(time.Minute < minute)
													continue;
												else
												{
													if(time.Minute == minute)
														sCalendarFunctions.Write(name, channel, message);
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

		private void NameDay()
		{
			var time = DateTime.Now;
			var sChannelInfo = sIrcBase.Networks[_servername].sChannelInfo;
			var sSendMessage = sIrcBase.Networks[_servername].sSendMessage;

			if((time.Hour == 8 && time.Minute == 0) || (time.Hour == 12 && time.Minute == 0) ||
			   (time.Hour == 16 && time.Minute == 0) || (time.Hour == 20 && time.Minute == 0))
			{
				foreach(var channel in sChannelInfo.CList)
				{
					if(sChannelInfo.FSelect(IFunctions.NameDay) && sChannelInfo.FSelect(IChannelFunctions.NameDay, channel.Key))
					{
						sSendMessage.SendCMPrivmsg(channel.Key, sLManager.GetWarningText("NameDay", channel.Key, _servername), sUtilities.NameDay(channel.Key));
						Thread.Sleep(400);
					}
				}
			}
		}
	}
}