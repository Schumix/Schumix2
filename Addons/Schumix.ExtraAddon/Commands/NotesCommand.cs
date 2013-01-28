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
using Schumix.Api.Irc;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.ExtraAddon.Commands
{
	sealed class Notes : CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly LocalizationManager sLManager = Singleton<LocalizationManager>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private NameList sNameList;
		private string _servername;

		public Notes(string ServerName, NameList nl) : base(ServerName)
		{
			_servername = ServerName;
			sNameList = nl;
		}

		public void HandleNotes(IRCMessage sIRCMessage)
		{
			var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;

			if(sIRCMessage.Info.Length >= 6 && sIRCMessage.Info[4].ToLower() == "user" && sIRCMessage.Info[5].ToLower() == "register")
			{
			}
			else
			{
				if(!Warning(sIRCMessage))
					return;
			}

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				if(!IsUser(sIRCMessage.Nick, sIRCMessage.Host))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoDataNoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				var db = SchumixBase.DManager.Query("SELECT Code FROM notes WHERE Name = '{0}' And ServerName = '{1}'", sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					string codes = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string code = row["Code"].ToString();
						codes += ", " + code;
					}

					if(codes == string.Empty)
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("notes/info", sIRCMessage.Channel, sIRCMessage.ServerName), sLConsole.Other("Nothing"));
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("notes/info", sIRCMessage.Channel, sIRCMessage.ServerName), codes.Remove(0, 2, ", "));
				}
				else
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
			}
			else if(sIRCMessage.Info[4].ToLower() == "user")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoValue", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "access")
				{
					var text = sLManager.GetCommandTexts("notes/user/access", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 4)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoPassword", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string name = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM notes_users WHERE Name = '{0}' And ServerName = '{1}'", name.ToLower(), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						if(db["Password"].ToString() == sUtilities.Sha1(sIRCMessage.Info[6]))
						{
							SchumixBase.DManager.Update("notes_users", string.Format("Vhost = '{0}'", sIRCMessage.Host), string.Format("Name = '{0}' And ServerName = '{1}'", name.ToLower(), sIRCMessage.ServerName));
							sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						}
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}

					if(!sNameList.IsChannelList(name))
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[2]);
						sSendMessage.SendChatMessage(sIRCMessage, text[3]);
						sNameList.NewThread(name);
					}
				}
				else if(sIRCMessage.Info[5].ToLower() == "newpassword")
				{
					var text = sLManager.GetCommandTexts("notes/user/newpassword", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoOldPassword", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoNewPassword", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string name = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM notes_users WHERE Name = '{0}' And ServerName = '{1}'", name.ToLower(), _servername);
					if(!db.IsNull())
					{
						if(db["Password"].ToString() == sUtilities.Sha1(sIRCMessage.Info[6]))
						{
							SchumixBase.DManager.Update("notes_users", string.Format("Password = '{0}'", sUtilities.Sha1(sIRCMessage.Info[7])), string.Format("Name = '{0}' And ServerName = '{1}'", name.ToLower(), sIRCMessage.ServerName));
							sSendMessage.SendChatMessage(sIRCMessage, text[0], sIRCMessage.Info[7]);
						}
						else
							sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					}
				}
				else if(sIRCMessage.Info[5].ToLower() == "register")
				{
					var text = sLManager.GetCommandTexts("notes/user/register", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoPassword", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					string name = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes_users WHERE Name = '{0}' And ServerName = '{1}'", name.ToLower(), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					string pass = sIRCMessage.Info[6];
					SchumixBase.DManager.Insert("`notes_users`(ServerId, ServerName, Name, Password, Vhost)", sIRCMessage.ServerId, sIRCMessage.ServerName, name.ToLower(), sUtilities.Sha1(pass), sIRCMessage.Host);
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("notes/user/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 5)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					string name = sIRCMessage.Nick;
					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes_users WHERE Name = '{0}' And ServerName = '{1}'", name.ToLower(), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[1]);
						return;
					}

					db = SchumixBase.DManager.QueryFirstRow("SELECT Password FROM notes_users WHERE Name = '{0}' And ServerName = '{1}'", name.ToLower(), sIRCMessage.ServerName);
					if(!db.IsNull())
					{
						if(db["Password"].ToString() != sUtilities.Sha1(sIRCMessage.Info[6]))
						{
							sSendMessage.SendChatMessage(sIRCMessage, text[2]);
							sSendMessage.SendChatMessage(sIRCMessage, text[3]);
							return;
						}
					}

					SchumixBase.DManager.Delete("notes_users", string.Format("Name = '{0}' And ServerName = '{1}'", name.ToLower(), sIRCMessage.ServerName));
					SchumixBase.DManager.Delete("notes", string.Format("Name = '{0}' And ServerName = '{1}'", name.ToLower(), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[4]);
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "code")
			{
				if(!IsUser(sIRCMessage.Nick, sIRCMessage.Host))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoDataNoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCode", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "remove")
				{
					var text = sLManager.GetCommandTexts("notes/code/remove", sIRCMessage.Channel, sIRCMessage.ServerName);
					if(text.Length < 2)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
						return;
					}

					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoCode", sIRCMessage.Channel, sIRCMessage.ServerName));
						return;
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes WHERE Code = '{0}' AND Name = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName);
					if(db.IsNull())
					{
						sSendMessage.SendChatMessage(sIRCMessage, text[0]);
						return;
					}

					SchumixBase.DManager.Delete("notes", string.Format("Code = '{0}' AND Name = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[6].ToLower()), sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName));
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				}
				else
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Note FROM notes WHERE Code = '{0}' AND Name = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(sIRCMessage.Info[5].ToLower()), sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName);
					if(!db.IsNull())
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetCommandText("notes/code", sIRCMessage.Channel, sIRCMessage.ServerName), db["Note"].ToString());
					else
						sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("FaultyQuery", sIRCMessage.Channel, sIRCMessage.ServerName));
				}
			}
			else
			{
				var text = sLManager.GetCommandTexts("notes", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 3)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return;
				}

				if(!IsUser(sIRCMessage.Nick, sIRCMessage.Host))
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLManager.GetWarningText("NoDataNoCommand", sIRCMessage.Channel, sIRCMessage.ServerName));
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[0]);
					return;
				}

				string code = sIRCMessage.Info[4];
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM notes WHERE Code = '{0}' AND Name = '{1}' And ServerName = '{2}'", sUtilities.SqlEscape(code.ToLower()), sIRCMessage.Nick.ToLower(), sIRCMessage.ServerName);
				if(!db.IsNull())
				{
					sSendMessage.SendChatMessage(sIRCMessage, text[1]);
					return;
				}

				SchumixBase.DManager.Insert("`notes`(ServerId, ServerName, Code, Name, Note)", sIRCMessage.ServerId, sIRCMessage.ServerName, sUtilities.SqlEscape(code.ToLower()), sIRCMessage.Nick.ToLower(), sUtilities.SqlEscape(sIRCMessage.Info.SplitToString(5, SchumixBase.Space)));
				sSendMessage.SendChatMessage(sIRCMessage, text[2], code);
			}
		}

		private bool IsUser(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM notes_users WHERE Name = '{0}' And ServerName = '{1}'", Name.ToLower(), _servername);
			return !db.IsNull();
		}

		private bool IsUser(string Name, string Vhost)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Vhost FROM notes_users WHERE Name = '{0}' And ServerName = '{1}'", Name.ToLower(), _servername);
			if(!db.IsNull())
			{
				string vhost = db["Vhost"].ToString();

				if(Vhost != vhost)
					return false;

				return true;
			}

			return false;
		}

		private bool Warning(IRCMessage sIRCMessage)
		{
			if(!IsUser(sIRCMessage.Nick))
			{
				var sSendMessage = sIrcBase.Networks[sIRCMessage.ServerName].sSendMessage;
				var text = sLManager.GetCommandTexts("notes/warning", sIRCMessage.Channel, sIRCMessage.ServerName);
				if(text.Length < 4)
				{
					sSendMessage.SendChatMessage(sIRCMessage, sLConsole.Translations("NoFound2", sLManager.GetChannelLocalization(sIRCMessage.Channel, sIRCMessage.ServerName)));
					return false;
				}

				sSendMessage.SendChatMessage(sIRCMessage, text[0]);
				sSendMessage.SendChatMessage(sIRCMessage, text[1]);
				sSendMessage.SendChatMessage(sIRCMessage, text[2], IRCConfig.List[sIRCMessage.ServerName].CommandPrefix);
				sSendMessage.SendChatMessage(sIRCMessage, text[3], IRCConfig.List[sIRCMessage.ServerName].CommandPrefix);
				return false;
			}

			return true;
		}
	}
}