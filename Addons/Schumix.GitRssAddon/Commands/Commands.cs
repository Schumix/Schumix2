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
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Schumix.Api.Irc;
using Schumix.Irc;
using Schumix.Irc.Util;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.GitRssAddon.Commands
{
	class RssCommand : CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		public readonly List<GitRss> RssList = new List<GitRss>();
		
		public RssCommand(string ServerName) : base(ServerName)
		{
		}
		
		public void HandleGit(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;
			
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
			
			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}
			
			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var db = SchumixBase.DManager.Query("SELECT Name, Type, Channel FROM gitinfo WHERE ServerName = '{0}'", sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					foreach(DataRow row in db.Rows)
					{
						string name = row["Name"].ToString();
						string type = row["Type"].ToString();
						string[] channel = row["Channel"].ToString().Split(SchumixBase.Comma);
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("git/info", sIRCMessage.Channel, sIRCMessage.ServerName), name, type, channel.SplitToString(SchumixBase.Space));
					}
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else if(sIRCMessage.Info[4].ToLower() == "list")
			{
				var db = SchumixBase.DManager.Query("SELECT Name, Type FROM gitinfo WHERE ServerName = '{0}'", sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					string list = string.Empty;
					
					foreach(DataRow row in db.Rows)
						list += SchumixBase.Space + string.Format("3{0}/7{1},", row["Name"].ToString(), row["Type"].ToString());

					if(list.IsEmpty())
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("git/list", sIRCMessage.Channel, sIRCMessage.ServerName), SchumixBase.Space + sLConsole.Other("Nothing"));
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("git/list", sIRCMessage.Channel, sIRCMessage.ServerName), list);
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else if(sIRCMessage.Info[4].ToLower() == "start")
			{
				var text = sLManager.GetCommandTexts("git/start", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}
				
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				foreach(var list in RssList)
				{
					if(sIRCMessage.Info[5].ToLower() == list.Name.ToLower() && sIRCMessage.Info[6].ToLower() == list.Type.ToLower())
					{
						if(list.Started)
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0], list.Name, list.Type);
							return;
						}
						
						list.Start();
						sSendMessage.SendChatMessage(sIRCMessage, text[1], list.Name, list.Type);
						return;
					}
				}
				
				sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[5], sIRCMessage.Info[6]);
			}
			else if(sIRCMessage.Info[4].ToLower() == "stop")
			{
				var text = sLManager.GetCommandTexts("git/stop", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}
				
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				foreach(var list in RssList)
				{
					if(sIRCMessage.Info[5].ToLower() == list.Name.ToLower() && sIRCMessage.Info[6].ToLower() == list.Type.ToLower())
					{
						if(!list.Started)
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[0], list.Name, list.Type);
							return;
						}
						
						list.Stop();
						sSendMessage.SendChatMessage(sIRCMessage, text[1], list.Name, list.Type);
						return;
					}
				}
				
				sSendMessage.SendChatMessage(sIRCMessage, text[2], sIRCMessage.Info[5], sIRCMessage.Info[6]);
			}
			else if(sIRCMessage.Info[4].ToLower() == "reload")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("No1Value", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				if(sIRCMessage.Info[5].ToLower() == "all")
				{
					foreach(var list in RssList)
						list.Reload();
					
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("git/reload/all", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
				else
				{
					var text = sLManager.GetCommandTexts("git/reload", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}
					
					if(sIRCMessage.Info.Length < 6)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					foreach(var list in RssList)
					{
						if(sIRCMessage.Info[5].ToLower() == list.Name.ToLower() && sIRCMessage.Info[6].ToLower() == list.Type.ToLower())
						{
							list.Reload();
							sSendMessage.SendChatMessage(sIRCMessage, text[0], list.Name, list.Type);
							return;
						}
					}
					
					sSendMessage.SendChatMessage(sIRCMessage, text[1], sIRCMessage.Info[5], sIRCMessage.Info[6]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				if(sIRCMessage.Info[5].ToLower() == "add")
				{
					var text = sLManager.GetCommandTexts("git/channel/add", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}
					
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 9)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[8]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM gitinfo WHERE LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[7]), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);
						string data = channel.SplitToString(SchumixBase.Comma);

						if(channel.ToList().Contains(sIRCMessage.Info[8].ToLower()))
						{
							sSendMessage.SendChatMessage(sIRCMessage, "Már hozzá van adva a csatorna!");
							return;
						}

						if(channel.Length == 1 && data.IsEmpty())
							data += sIRCMessage.Info[8].ToLower();
						else
							data += SchumixBase.Comma + sIRCMessage.Info[8].ToLower();
						
						SchumixBase.DManager.Update("gitinfo", string.Format("Channel = '{0}'", data), string.Format("LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[7]), sIRCMessage.ServerName));
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("git/channel/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}
					
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 9)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoChannelName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(!Rfc2812Util.IsValidChannelName(sIRCMessage.Info[8]))
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NotaChannelHasBeenSet", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Channel FROM gitinfo WHERE LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[7]), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						string[] channel = db["Channel"].ToString().Split(SchumixBase.Comma);
						string data = string.Empty;

						if(!channel.ToList().Contains(sIRCMessage.Info[8].ToLower()))
						{
							sSendMessage.SendChatMessage(sIRCMessage, "Nincs ilyen csatorna hozzáadva így nem törölhető!");
							return;
						}
						
						for(int x = 0; x < channel.Length; x++)
						{
							if(channel[x] == sIRCMessage.Info[8].ToLower())
								continue;
							
							data += SchumixBase.Comma + channel[x];
						}
						
						SchumixBase.DManager.Update("gitinfo", string.Format("Channel = '{0}'", data.Remove(0, 1, SchumixBase.Comma)), string.Format("LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[7]), sIRCMessage.ServerName));
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "add")
			{
				//var text = sLManager.GetCommandTexts("git/add", sIRCMessage.Channel, sIRCMessage.ServerName);
				//if(text.Length < 2)
				//{
				//	sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				//	return;
				//}
					
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 8)
				{
					sSendMessage.SendChatMessage(sIRCMessage, "Nincs megadva az url!"/*sLManager.GetWarningText("NoUrl", sIRCMessage.Channel, sIRCMessage.ServerName)*/);
					return;
				}

				if(sIRCMessage.Info.Length < 9)
				{
					sSendMessage.SendChatMessage(sIRCMessage, "Nincs megadva az oldal neve!"/*sLManager.GetWarningText("NoUrl", sIRCMessage.Channel, sIRCMessage.ServerName)*/);
					return;
				}

				var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM gitinfo WHERE LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6]), sIRCMessage.ServerName);
				if(db.IsNull())
				{
					SchumixBase.DManager.Insert("`gitinfo`(ServerId, ServerName, Name, Type, Link, Website)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(sIRCMessage.Info[5]), sUtilities.SqlEscape(sIRCMessage.Info[6]), sUtilities.SqlEscape(sIRCMessage.Info[7]), sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower()));
					sSendMessage.SendChatMessage(sIRCMessage, "Sikeresen hozzáadva a listához!");
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, "Már szerepel a listában!");
			}
			else if(sIRCMessage.Info[4].ToLower() == "remove")
			{
				//var text = sLManager.GetCommandTexts("git/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
				//if(text.Length < 2)
				//{
				//	sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
				//	return;
				//}
				
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM gitinfo WHERE LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[6]), sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					SchumixBase.DManager.Delete("gitinfo", string.Format("Name = '{0}' And Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[5]), sUtilities.SqlEscape(sIRCMessage.Info[6]), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, "Sikeresen eltávolítva a listából!");
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, "Nem szerepel a név a listában!");
			}
			else if(sIRCMessage.Info[4].ToLower() == "change")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}
				
				if(sIRCMessage.Info[5].ToLower() == "colors")
				{
					//var text = sLManager.GetCommandTexts("git/change/colors", sIRCMessage.Channel, sIRCMessage.ServerName);
					//if(text.Length < 2)
					//{
					//	sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					//	return;
					//}
					
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info.Length < 9)
					{
						sSendMessage.SendChatMessage(sIRCMessage, "Nincs megadva az állapot!"/*sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName)*/);
						return;
					}

					if(sIRCMessage.Info[8].ToLower() != "true" && sIRCMessage.Info[8].ToLower() != "false")
					{
						sSendMessage.SendChatMessage(sIRCMessage, "Nem true vagy false érték lett megadva!"/*sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName)*/);
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Colors FROM gitinfo WHERE LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[7]), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						bool enabled = Convert.ToBoolean(db["Colors"].ToString());

						if(Convert.ToBoolean(sIRCMessage.Info[8].ToLower()) == enabled)
						{
							if(enabled)
							{
								sSendMessage.SendChatMessage(sIRCMessage, "Már be van kapcsolva!");
								return;
							}
							else
							{
								sSendMessage.SendChatMessage(sIRCMessage, "Már ki van kapcsolva!");
								return;
							}
						}

						SchumixBase.DManager.Update("gitinfo", string.Format("Colors = '{0}'", sIRCMessage.Info[8].ToLower()), string.Format("LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[7]), sIRCMessage.ServerName));
						sSendMessage.SendChatMessage(sIRCMessage, "Sikeresen módosítva a beállítás.");
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, "Nem szerepel a név a listában!");
				}
				else if(sIRCMessage.Info[5].ToLower() == "shorturl")
				{
					//var text = sLManager.GetCommandTexts("git/change/shorturl", sIRCMessage.Channel, sIRCMessage.ServerName);
					//if(text.Length < 2)
					//{
					//	sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					//	return;
					//}
					
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 9)
					{
						sSendMessage.SendChatMessage(sIRCMessage, "Nincs megadva az állapot!"/*sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName)*/);
						return;
					}
					
					if(sIRCMessage.Info[8].ToLower() != "true" && sIRCMessage.Info[8].ToLower() != "false")
					{
						sSendMessage.SendChatMessage(sIRCMessage, "Nem true vagy false érték lett megadva!"/*sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName)*/);
						return;
					}
					
					var db = SchumixBase.DManager.QueryFirstRow("SELECT ShortUrl FROM gitinfo WHERE LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[7]), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						bool enabled = Convert.ToBoolean(db["ShortUrl"].ToString());
						
						if(Convert.ToBoolean(sIRCMessage.Info[8].ToLower()) == enabled)
						{
							if(enabled)
							{
								sSendMessage.SendChatMessage(sIRCMessage, "Már be van kapcsolva!");
								return;
							}
							else
							{
								sSendMessage.SendChatMessage(sIRCMessage, "Már ki van kapcsolva!");
								return;
							}
						}
						
						SchumixBase.DManager.Update("gitinfo", string.Format("ShortUrl = '{0}'", sIRCMessage.Info[8].ToLower()), string.Format("LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[7]), sIRCMessage.ServerName));
						sSendMessage.SendChatMessage(sIRCMessage, "Sikeresen módosítva a beállítás.");
					}
					else
						sSendMessage.SendChatMessage(sIRCMessage, "Nem szerepel a név a listában!");
				}
				else if(sIRCMessage.Info[5].ToLower() == "url")
				{
					//var text = sLManager.GetCommandTexts("git/change/url", sIRCMessage.Channel, sIRCMessage.ServerName);
					//if(text.Length < 2)
					//{
					//	sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					//	return;
					//}
					
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 9)
					{
						sSendMessage.SendChatMessage(sIRCMessage, "Nincs megadva az url!"/*sLManager.GetWarningText("NoUrl", sIRCMessage.Channel, sIRCMessage.ServerName)*/);
						return;
					}
					
					var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM gitinfo WHERE LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[7]), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, "Nem szerepel a név a listában!");
						return;
					}
					
					SchumixBase.DManager.Update("gitinfo", string.Format("Link = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[8])), string.Format("LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[7]), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, "Url sikeresen módosítva!");
				}
				else if(sIRCMessage.Info[5].ToLower() == "website")
				{
					//var text = sLManager.GetCommandTexts("git/change/website", sIRCMessage.Channel, sIRCMessage.ServerName);
					//if(text.Length < 2)
					//{
					//	sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					//	return;
					//}
					
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoTypeName", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}
					
					if(sIRCMessage.Info.Length < 9)
					{
						sSendMessage.SendChatMessage(sIRCMessage, "Nincs megadva az oldal neve!"/*sLManager.GetWarningText("NoUrl", sIRCMessage.Channel, sIRCMessage.ServerName)*/);
						return;
					}
					
					var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM gitinfo WHERE LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[7]), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, "Nem szerepel a név a listában!");
						return;
					}
					
					SchumixBase.DManager.Update("gitinfo", string.Format("Website = '{0}'", sUtilities.SqlEscape(sIRCMessage.Info[8].ToLower())), string.Format("LOWER(Name) = '{0}' AND Type = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sUtilities.SqlEscape(sIRCMessage.Info[7]), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, "Oldal neve sikeresen módosítva!");
				}
			}
		}
	}
}