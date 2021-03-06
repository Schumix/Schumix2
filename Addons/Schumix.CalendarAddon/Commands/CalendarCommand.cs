/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2015 Schumix Team <http://schumix.eu/>
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
using System.Text.RegularExpressions;
using Schumix.Irc;
using Schumix.Irc.Util;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Irc;
using Schumix.Framework.Util;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;
using Schumix.CalendarAddon;

namespace Schumix.CalendarAddon.Commands
{
	class CalendarCommand : CommandInfo
	{
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private CalendarFunctions sCalendarFunctions;
		private Regex _hamregex;
		private Regex _regex;

		public CalendarCommand(string ServerName) : base(ServerName)
		{
			sCalendarFunctions = new CalendarFunctions(ServerName);
			_regex = new Regex(@"((?<year>[0-9]{4,4})(?:[\.\s]+|))?"                         // Year
			                   + @"((?<month>[0-9]{1,2}|[a-zóüöúőűáéí]{3,20})(?:[\.\s]+|))?" // Month
			                   + @"((?<day>[0-9]{1,2})(?:[\.\s]+|))?"                        // Day
			                   + @"((?<hour>[0-9]{1,2})(?:[:]|))?"                           // Hour
			                   + @"(?<minute>[0-9]{1,2})?"                                   // Minute
			                   + @"((?:[\s]+)(?<text>(.*)))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
			_hamregex = new Regex(@"((?<hour>[0-9]{1,2})(?:[:]|))?"                          // Hour
			                   + @"(?<minute>[0-9]{1,2})?"                                   // Minute
			                   + @"((?:[\s]+)(?<text>(.*)))?", RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

		private int GetYear(string args)
		{
			return _regex.IsMatch(args) ? _regex.Match(args).Groups["year"].ToString().ToNumber(-1).ToInt32() : -1;
		}

		private string GetMonth(string args)
		{
			return _regex.IsMatch(args) ? _regex.Match(args).Groups["month"].ToString() : string.Empty;
		}

		private int GetDay(string args)
		{
			return _regex.IsMatch(args) ? _regex.Match(args).Groups["day"].ToString().ToNumber(32).ToInt32() : 32;
		}

		private int GetHour(string args)
		{
			if(_regex.IsMatch(args) && IsYear(args))
				return _regex.Match(args).Groups["hour"].ToString().ToNumber(25).ToInt32();

			if(_hamregex.IsMatch(args) && !IsYear(args))
				return _hamregex.Match(args).Groups["hour"].ToString().ToNumber(25).ToInt32();

			return 25;
		}

		private int GetMinute(string args)
		{
			if(_regex.IsMatch(args) && IsYear(args))
				return _regex.Match(args).Groups["minute"].ToString().ToNumber(61).ToInt32();

			if(_hamregex.IsMatch(args) && !IsYear(args))
				return _hamregex.Match(args).Groups["minute"].ToString().ToNumber(61).ToInt32();

			return 61;
		}

		private string GetMessage(string args)
		{
			if(_regex.IsMatch(args) && IsYear(args))
				return _regex.Match(args).Groups["text"].ToString();

			if(_hamregex.IsMatch(args) && !IsYear(args))
				return _hamregex.Match(args).Groups["text"].ToString();

			return string.Empty;
		}

		private bool IsYear(string args)
		{
			return _regex.IsMatch(args) && !_regex.Match(args).Groups["year"].ToString().IsNullOrEmpty();
		}

		private bool IsMonth(string args)
		{
			return _regex.IsMatch(args) && !_regex.Match(args).Groups["month"].ToString().IsNullOrEmpty();
		}

		private bool IsDay(string args)
		{
			return _regex.IsMatch(args) && !_regex.Match(args).Groups["day"].ToString().IsNullOrEmpty();
		}

		private bool IsHour(string args)
		{
			return (_regex.IsMatch(args) && !_regex.Match(args).Groups["hour"].ToString().IsNullOrEmpty() && IsYear(args)) ||
				(_hamregex.IsMatch(args) && !_hamregex.Match(args).Groups["hour"].ToString().IsNullOrEmpty() && !IsYear(args));
		}

		private bool IsMinute(string args)
		{
			return (_regex.IsMatch(args) && !_regex.Match(args).Groups["minute"].ToString().IsNullOrEmpty() && IsYear(args)) ||
				(_hamregex.IsMatch(args) && !_hamregex.Match(args).Groups["minute"].ToString().IsNullOrEmpty() && !IsYear(args));
		}

		private bool IsMessage(string args)
		{
			return (_regex.IsMatch(args) && !_regex.Match(args).Groups["text"].ToString().IsNullOrEmpty() && IsYear(args)) ||
				(_hamregex.IsMatch(args) && !_hamregex.Match(args).Groups["text"].ToString().IsNullOrEmpty() && !IsYear(args));
		}

		private bool IsHourAndMinute(string args)
		{
			return !IsYear(args);
		}

		public void HandleCalendar(IRCMessage sIRCMessage)
		{
			var sMyNickInfo = sIrcBase.Networks[sIRCMessage.ServerName].sMyNickInfo;
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTime", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "loop")
			{
				if(IsWarningAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.HalfOperator))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("WarningAdmin", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

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

						string args = sIRCMessage.Info.SplitToString(8, SchumixBase.Space);

						if(IsHourAndMinute(args))
						{
							if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[7]))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(!IsHour(args))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(!IsMinute(args))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int hour = GetHour(args);
							if(hour >= 24 || hour < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int minute = GetMinute(args);
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

						string args = sIRCMessage.Info.SplitToString(7, SchumixBase.Space);

						if(IsHourAndMinute(args))
						{
							if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[6]))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(sMyNickInfo.NickStorage.ToLower() == sIRCMessage.Info[6].ToLower())
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ICantLeftAMessageForMyself", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(!IsHour(args))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(!IsMinute(args))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(!IsMessage(args))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int hour = GetHour(args);
							if(hour >= 24 || hour < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int minute = GetMinute(args);
							if(minute >= 60 || minute < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Info[6].ToLower(), sIRCMessage.Channel, GetMessage(args), hour, minute, true));
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

							string args = sIRCMessage.Info.SplitToString(9, SchumixBase.Space);

							if(IsHourAndMinute(args))
							{
								if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[8]))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								if(!IsHour(args))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								if(!IsMinute(args))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int hour = GetHour(args);
								if(hour >= 24 || hour < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int minute = GetMinute(args);
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

							string args = sIRCMessage.Info.SplitToString(8, SchumixBase.Space);

							if(IsHourAndMinute(args))
							{
								if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[7]))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								if(sMyNickInfo.NickStorage.ToLower() == sIRCMessage.Info[7].ToLower())
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ICantLeftAMessageForMyself", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}
								if(!IsHour(args))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								if(!IsMinute(args))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								if(!IsMessage(args))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int hour = GetHour(args);
								if(hour >= 24 || hour < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int minute = GetMinute(args);
								if(minute >= 60 || minute < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Info[7].ToLower(), sIRCMessage.Info[7].ToLower(), GetMessage(args), hour, minute, true));
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

							string args = sIRCMessage.Info.SplitToString(7, SchumixBase.Space);

							if(IsHourAndMinute(args))
							{
								if(!IsHour(args))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								if(!IsMinute(args))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int hour = GetHour(args);
								if(hour >= 24 || hour < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int minute = GetMinute(args);
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
							string args = sIRCMessage.Info.SplitToString(6, SchumixBase.Space);

							if(IsHourAndMinute(args))
							{
								if(!IsHour(args))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								if(!IsMinute(args))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								if(!IsMessage(args))
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int hour = GetHour(args);
								if(hour >= 24 || hour < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								int minute = GetMinute(args);
								if(minute >= 60 || minute < 0)
								{
									sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
									return;
								}

								sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Nick.ToLower(), sIRCMessage.Nick.ToLower(), GetMessage(args), hour, minute, true));
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

						string args = sIRCMessage.Info.SplitToString(6, SchumixBase.Space);

						if(IsHourAndMinute(args))
						{
							if(!IsHour(args))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(!IsMinute(args))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int hour = GetHour(args);
							if(hour >= 24 || hour < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int minute = GetMinute(args);
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
						string args = sIRCMessage.Info.SplitToString(5, SchumixBase.Space);

						if(IsHourAndMinute(args))
						{
							if(!IsHour(args))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(!IsMinute(args))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							if(!IsMessage(args))
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int hour = GetHour(args);
							if(hour >= 24 || hour < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							int minute = GetMinute(args);
							if(minute >= 60 || minute < 0)
							{
								sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
								return;
							}

							sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Nick.ToLower(), sIRCMessage.Channel, GetMessage(args), hour, minute, true));
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

				string args = sIRCMessage.Info.SplitToString(6, SchumixBase.Space);

				if(IsHourAndMinute(args))
				{
					if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[5]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sMyNickInfo.NickStorage.ToLower() == sIRCMessage.Info[5].ToLower())
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ICantLeftAMessageForMyself", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsHour(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsMinute(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsMessage(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int hour = GetHour(args);
					if(hour >= 24 || hour < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int minute = GetMinute(args);
					if(minute >= 60 || minute < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Info[5].ToLower(), sIRCMessage.Channel, GetMessage(args), hour, minute));
				}
				else
				{
					if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[5]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sMyNickInfo.NickStorage.ToLower() == sIRCMessage.Info[5].ToLower())
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ICantLeftAMessageForMyself", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsYear(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("YearIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsMonth(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MonthIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsDay(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("DayIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsHour(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsMinute(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsMessage(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int year = GetYear(args);
					if(year < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorYear", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int month = GetMonth(args).GetMonthNameInInt(sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName));
					if(month > 12 || month <= 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMonth", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int day = GetDay(args);
					if(!sUtilities.IsDay(year, month, day))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorDay", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int hour = GetHour(args);
					if(hour >= 24 || hour < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int minute = GetMinute(args);
					if(minute >= 60 || minute < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Info[5].ToLower(), sIRCMessage.Channel, GetMessage(args), year, month, day, hour, minute));
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

					string args = sIRCMessage.Info.SplitToString(7, SchumixBase.Space);

					if(IsHourAndMinute(args))
					{
						if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[6]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sMyNickInfo.NickStorage.ToLower() == sIRCMessage.Info[6].ToLower())
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ICantLeftAMessageForMyself", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsHour(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsMinute(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsMessage(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int hour = GetHour(args);
						if(hour >= 24 || hour < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int minute = GetMinute(args);
						if(minute >= 60 || minute < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Info[6].ToLower(), sIRCMessage.Info[6].ToLower(), GetMessage(args), hour, minute));
					}
					else
					{
						if(!Rfc2812Util.IsValidNick(sIRCMessage.Info[6]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaNickNameHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(sMyNickInfo.NickStorage.ToLower() == sIRCMessage.Info[6].ToLower())
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ICantLeftAMessageForMyself", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsYear(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("YearIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsMonth(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MonthIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsDay(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("DayIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsHour(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsMinute(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsMessage(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int year = GetYear(args);
						if(year < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorYear", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int month = GetMonth(args).GetMonthNameInInt(sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName));
						if(month > 12 || month <= 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMonth", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int day = GetDay(args);
						if(!sUtilities.IsDay(year, month, day))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorDay", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int hour = GetHour(args);
						if(hour >= 24 || hour < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int minute = GetMinute(args);
						if(minute >= 60 || minute < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Info[6].ToLower(), sIRCMessage.Info[6].ToLower(), GetMessage(args), year, month, day, hour, minute));
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

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Message, Year, Month, Day, Hour, Minute FROM calendar WHERE ServerName = '{0}' And Channel = '{1}' And Name = '{2}' And Loops = 'false' ORDER BY UnixTime ASC", sIRCMessage.ServerName, sIRCMessage.SqlEscapeNick.ToLower(), sIRCMessage.SqlEscapeNick.ToLower());
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
					string args = sIRCMessage.Info.SplitToString(5, SchumixBase.Space);

					if(IsHourAndMinute(args))
					{
						if(!IsHour(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsMinute(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsMessage(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int hour = GetHour(args);
						if(hour >= 24 || hour < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int minute = GetMinute(args);
						if(minute >= 60 || minute < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Nick.ToLower(), sIRCMessage.Nick.ToLower(), GetMessage(args), hour, minute));
					}
					else
					{
						if(!IsYear(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("YearIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsMonth(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MonthIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsDay(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("DayIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsHour(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsMinute(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						if(!IsMessage(args))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int year = GetYear(args);
						if(year < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorYear", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int month = GetMonth(args).GetMonthNameInInt(sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName));
						if(month > 12 || month <= 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMonth", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int day = GetDay(args);
						if(!sUtilities.IsDay(year, month, day))
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorDay", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int hour = GetHour(args);
						if(hour >= 24 || hour < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						int minute = GetMinute(args);
						if(minute >= 60 || minute < 0)
						{
							sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
							return;
						}

						sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Nick.ToLower(), sIRCMessage.Nick.ToLower(), GetMessage(args), year, month, day, hour, minute));
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

				var db = SchumixBase.DManager.QueryFirstRow("SELECT Message, Year, Month, Day, Hour, Minute FROM calendar WHERE ServerName = '{0}' And Channel = '{1}' And Name = '{2}' And Loops = 'false' ORDER BY UnixTime ASC", sIRCMessage.ServerName, sIRCMessage.Channel.ToLower(), sIRCMessage.SqlEscapeNick.ToLower());
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
				string args = sIRCMessage.Info.SplitToString(4, SchumixBase.Space);

				if(IsHourAndMinute(args))
				{
					if(!IsHour(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsMinute(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsMessage(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int hour = GetHour(args);
					if(hour >= 24 || hour < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int minute = GetMinute(args);
					if(minute >= 60 || minute < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Nick.ToLower(), sIRCMessage.Channel, GetMessage(args), hour, minute));
				}
				else
				{
					if(!IsYear(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("YearIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsMonth(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MonthIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsDay(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("DayIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsHour(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("HourIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsMinute(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("MinuteIsNotGiven", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!IsMessage(args))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoMessage", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int year = GetYear(args);
					if(year < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorYear", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int month = GetMonth(args).GetMonthNameInInt(sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName));
					if(month > 12 || month <= 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMonth", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int day = GetDay(args);
					if(!sUtilities.IsDay(year, month, day))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorDay", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int hour = GetHour(args);
					if(hour >= 24 || hour < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorHour", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					int minute = GetMinute(args);
					if(minute >= 60 || minute < 0)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("ErrorMinute", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					sSendMessage.SendChatMessage(sIRCMessage, sCalendarFunctions.Add(sIRCMessage.Nick.ToLower(), sIRCMessage.Channel, GetMessage(args), year, month, day, hour, minute));
				}
			}
		}
	}
}