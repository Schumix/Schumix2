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
using Schumix.API;
using Schumix.API.Irc;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.CalendarAddon;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.CalendarAddon.Commands
{
	class CalendarCommand : CommandInfo
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private CalendarFunctions sCalendarFunctions;

		public CalendarCommand(string ServerName) : base(ServerName)
		{
			sCalendarFunctions = new CalendarFunctions(ServerName);
		}

		public void HandleCalendar(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "loop")
			{
				if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
					return;

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "nick")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info[6].ToLower() == "remove")
					{
						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sIRCMessage.Info[8].Contains(SchumixBase.Colon.ToString()))
						{
							int hour = sIRCMessage.Info[8].Substring(0, sIRCMessage.Info[8].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
							if(hour >= 24 || hour < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int minute = sIRCMessage.Info[8].Substring(sIRCMessage.Info[8].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
							if(minute >= 60 || minute < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Remove(sIRCMessage.Info[7].ToLower(), sIRCMessage.Channel, hour, minute));
						}
					}
					else
					{
						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sIRCMessage.Info[7].Contains(SchumixBase.Colon.ToString()))
						{
							if(sIRCMessage.Info.Length < 9)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int hour = sIRCMessage.Info[7].Substring(0, sIRCMessage.Info[7].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
							if(hour >= 24 || hour < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int minute = sIRCMessage.Info[7].Substring(sIRCMessage.Info[7].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
							if(minute >= 60 || minute < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Info[6].ToLower(), sIRCMessage.Channel, sIRCMessage.Info.SplitToString(8, SchumixBase.Space), hour, minute, true));
						}
					}
				}
				else if(sIRCMessage.Info[5].ToLower() == "private")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
	
					if(sIRCMessage.Info[6].ToLower() == "nick")
					{
						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sIRCMessage.Info[7].ToLower() == "remove")
						{
							if(sIRCMessage.Info.Length < 9)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(sIRCMessage.Info.Length < 10)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(sIRCMessage.Info[9].Contains(SchumixBase.Colon.ToString()))
							{
								int hour = sIRCMessage.Info[9].Substring(0, sIRCMessage.Info[9].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
								if(hour >= 24 || hour < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int minute = sIRCMessage.Info[9].Substring(sIRCMessage.Info[9].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
								if(minute >= 60 || minute < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Remove(sIRCMessage.Info[8].ToLower(), sIRCMessage.Info[8].ToLower(), hour, minute));
							}
						}
						else
						{
							if(sIRCMessage.Info.Length < 9)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(sIRCMessage.Info[8].Contains(SchumixBase.Colon.ToString()))
							{
								if(sIRCMessage.Info.Length < 10)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int hour = sIRCMessage.Info[8].Substring(0, sIRCMessage.Info[8].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
								if(hour >= 24 || hour < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int minute = sIRCMessage.Info[8].Substring(sIRCMessage.Info[8].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
								if(minute >= 60 || minute < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Info[7].ToLower(), sIRCMessage.Info[7].ToLower(), sIRCMessage.Info.SplitToString(9, SchumixBase.Space), hour, minute, true));
							}
						}
					}
					else
					{
						if(sIRCMessage.Info[6].ToLower() == "remove")
						{
							if(sIRCMessage.Info.Length < 8)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(sIRCMessage.Info[7].Contains(SchumixBase.Colon.ToString()))
							{
								int hour = sIRCMessage.Info[7].Substring(0, sIRCMessage.Info[7].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
								if(hour >= 24 || hour < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int minute = sIRCMessage.Info[7].Substring(sIRCMessage.Info[7].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
								if(minute >= 60 || minute < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Remove(sIRCMessage.Nick.ToLower(), sIRCMessage.Nick.ToLower(), hour, minute));
							}
						}
						else
						{
							if(sIRCMessage.Info[6].Contains(SchumixBase.Colon.ToString()))
							{
								if(sIRCMessage.Info.Length < 8)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int hour = sIRCMessage.Info[6].Substring(0, sIRCMessage.Info[6].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
								if(hour >= 24 || hour < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int minute = sIRCMessage.Info[6].Substring(sIRCMessage.Info[6].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
								if(minute >= 60 || minute < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Nick.ToLower(), sIRCMessage.Nick.ToLower(), sIRCMessage.Info.SplitToString(7, SchumixBase.Space), hour, minute, true));
							}
						}
					}
				}
				else
				{
					if(sIRCMessage.Info[5].ToLower() == "remove")
					{
						if(sIRCMessage.Info.Length < 7)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sIRCMessage.Info[6].Contains(SchumixBase.Colon.ToString()))
						{
							int hour = sIRCMessage.Info[6].Substring(0, sIRCMessage.Info[6].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
							if(hour >= 24 || hour < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int minute = sIRCMessage.Info[6].Substring(sIRCMessage.Info[6].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
							if(minute >= 60 || minute < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Remove(sIRCMessage.Nick.ToLower(), sIRCMessage.Channel, hour, minute));
						}
					}
					else
					{
						if(sIRCMessage.Info[5].Contains(SchumixBase.Colon.ToString()))
						{
							if(sIRCMessage.Info.Length < 7)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int hour = sIRCMessage.Info[5].Substring(0, sIRCMessage.Info[5].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
							if(hour >= 24 || hour < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int minute = sIRCMessage.Info[5].Substring(sIRCMessage.Info[5].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
							if(minute >= 60 || minute < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Nick.ToLower(), sIRCMessage.Channel, sIRCMessage.Info.SplitToString(6, SchumixBase.Space), hour, minute, true));
						}
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "nick")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[6].Contains(SchumixBase.Colon.ToString()))
				{
					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int hour = sIRCMessage.Info[6].Substring(0, sIRCMessage.Info[6].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
					if(hour >= 24 || hour < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int minute = sIRCMessage.Info[6].Substring(sIRCMessage.Info[6].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
					if(minute >= 60 || minute < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Info[5].ToLower(), sIRCMessage.Channel, sIRCMessage.Info.SplitToString(7, SchumixBase.Space), hour, minute));
				}
				else
				{
					if(sIRCMessage.Info.Length < 9)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string[] s = sIRCMessage.Info[6].Split(SchumixBase.Point);
					if(s.Length < 3)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("calendar", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int year = s[0].ToNumber().ToInt();
					int month = s[1].ToNumber(13).ToInt();
					if(month > 12 || month <= 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMonth", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int day = s[2].ToNumber(32).ToInt();
					if(!sUtilities.IsDay(year, month, day))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorDay", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int hour = sIRCMessage.Info[7].Substring(0, sIRCMessage.Info[7].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
					if(hour >= 24 || hour < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int minute = sIRCMessage.Info[7].Substring(sIRCMessage.Info[7].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
					if(minute >= 60 || minute < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Info[5].ToLower(), sIRCMessage.Channel, sIRCMessage.Info.SplitToString(8, SchumixBase.Space), year, month, day, hour, minute));
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "private")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "nick")
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info[7].Contains(SchumixBase.Colon.ToString()))
					{
						if(sIRCMessage.Info.Length < 9)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int hour = sIRCMessage.Info[7].Substring(0, sIRCMessage.Info[7].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
						if(hour >= 24 || hour < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int minute = sIRCMessage.Info[7].Substring(sIRCMessage.Info[7].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
						if(minute >= 60 || minute < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Info[6].ToLower(), sIRCMessage.Info[6].ToLower(), sIRCMessage.Info.SplitToString(8, SchumixBase.Space), hour, minute));
					}
					else
					{
						if(sIRCMessage.Info.Length < 10)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						string[] s = sIRCMessage.Info[7].Split(SchumixBase.Point);
						if(s.Length < 3)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("calendar", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int year = s[0].ToNumber().ToInt();
						int month = s[1].ToNumber(13).ToInt();
						if(month > 12 || month <= 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMonth", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int day = s[2].ToNumber(32).ToInt();
						if(!sUtilities.IsDay(year, month, day))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorDay", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int hour = sIRCMessage.Info[8].Substring(0, sIRCMessage.Info[8].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
						if(hour >= 24 || hour < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int minute = sIRCMessage.Info[8].Substring(sIRCMessage.Info[8].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
						if(minute >= 60 || minute < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Info[6].ToLower(), sIRCMessage.Info[6].ToLower(), sIRCMessage.Info.SplitToString(9, SchumixBase.Space), year, month, day, hour, minute));
					}
				}
				else if(sIRCMessage.Info[5].ToLower() == "nextmessage")
				{
					var text = sLManager.GetCommandTexts("calendar/private/nextmessage", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 3)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Message, Year, Month, Day, Hour, Minute FROM calendar WHERE ServerName = '{0}' And Channel = '{1}' And Name = '{2}' And Loops = 'false' ORDER BY UnixTime ASC", sIRCMessage.ServerName, sIRCMessage.Nick.ToLower(), sIRCMessage.Nick.ToLower());
					if(!db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0], db["Message"].ToString());
						sSendMessage.SendChatMessage(sIRCMessage, text[1], db["Year"].ToString(), db["Month"].ToString(), db["Day"].ToString(), db["Hour"].ToString(), db["Minute"].ToString());
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[2]);
				}
				else
				{
					if(sIRCMessage.Info[5].Contains(SchumixBase.Colon.ToString()))
					{
						if(sIRCMessage.Info.Length < 7)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int hour = sIRCMessage.Info[5].Substring(0, sIRCMessage.Info[5].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
						if(hour >= 24 || hour < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int minute = sIRCMessage.Info[5].Substring(sIRCMessage.Info[5].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
						if(minute >= 60 || minute < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Nick.ToLower(), sIRCMessage.Nick.ToLower(), sIRCMessage.Info.SplitToString(6, SchumixBase.Space), hour, minute));
					}
					else
					{
						if(sIRCMessage.Info.Length < 8)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						string[] s = sIRCMessage.Info[5].Split(SchumixBase.Point);
						if(s.Length < 3)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("calendar", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int year = s[0].ToNumber().ToInt();
						int month = s[1].ToNumber(13).ToInt();
						if(month > 12 || month <= 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMonth", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int day = s[2].ToNumber(32).ToInt();
						if(!sUtilities.IsDay(year, month, day))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorDay", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int hour = sIRCMessage.Info[6].Substring(0, sIRCMessage.Info[6].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
						if(hour >= 24 || hour < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int minute = sIRCMessage.Info[6].Substring(sIRCMessage.Info[6].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
						if(minute >= 60 || minute < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Nick.ToLower(), sIRCMessage.Nick.ToLower(), sIRCMessage.Info.SplitToString(7, SchumixBase.Space), year, month, day, hour, minute));
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "nextmessage")
			{
				var text = sLManager.GetCommandTexts("calendar/nextmessage", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Message, Year, Month, Day, Hour, Minute FROM calendar WHERE ServerName = '{0}' And Channel = '{1}' And Name = '{2}' And Loops = 'false' ORDER BY UnixTime ASC", sIRCMessage.ServerName, sIRCMessage.Channel.ToLower(), sIRCMessage.Nick.ToLower());
				if(!db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0], db["Message"].ToString());
					sSendMessage.SendChatMessage(sIRCMessage, text[1], db["Year"].ToString(), db["Month"].ToString(), db["Day"].ToString(), db["Hour"].ToString(), db["Minute"].ToString());
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, text[2]);
			}
			else
			{
				if(sIRCMessage.Info[4].Contains(SchumixBase.Colon.ToString()))
				{
					if(sIRCMessage.Info.Length < 6)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int hour = sIRCMessage.Info[4].Substring(0, sIRCMessage.Info[4].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
					if(hour >= 24 || hour < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int minute = sIRCMessage.Info[4].Substring(sIRCMessage.Info[4].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
					if(minute >= 60 || minute < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Nick.ToLower(), sIRCMessage.Channel, sIRCMessage.Info.SplitToString(5, SchumixBase.Space), hour, minute));
				}
				else
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoReason", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string[] s = sIRCMessage.Info[4].Split(SchumixBase.Point);
					if(s.Length < 3)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("calendar", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int year = s[0].ToNumber().ToInt();
					int month = s[1].ToNumber(13).ToInt();
					if(month > 12 || month <= 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMonth", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int day = s[2].ToNumber(32).ToInt();
					if(!sUtilities.IsDay(year, month, day))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorDay", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int hour = sIRCMessage.Info[5].Substring(0, sIRCMessage.Info[5].IndexOf(SchumixBase.Colon)).ToNumber(25).ToInt();
					if(hour >= 24 || hour < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int minute = sIRCMessage.Info[5].Substring(sIRCMessage.Info[5].IndexOf(SchumixBase.Colon)+1).ToNumber(61).ToInt();
					if(minute >= 60 || minute < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Nick.ToLower(), sIRCMessage.Channel, sIRCMessage.Info.SplitToString(6, SchumixBase.Space), year, month, day, hour, minute));
				}
			}
		}
	}
}