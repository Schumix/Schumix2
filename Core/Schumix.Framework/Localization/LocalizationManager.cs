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
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.Framework.Localization
{
	public sealed class LocalizationManager
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public string Locale { get; set; }
		private LocalizationManager() {}

		public string GetWarningText(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_warning WHERE Language = '{0}' AND Command = '{1}'", Locale, command.ToLower());
			return !db.IsNull() ? db["Text"].ToString() : "No translations found!";
		}

		public string GetWarningText(string command, string channel)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channel WHERE Channel = '{0}'", channel.ToLower());
			string locale = !db.IsNull() ? db["Language"].ToString() : Locale;
			db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_warning WHERE Language = '{0}' AND Command = '{1}'", locale, command.ToLower());
			return !db.IsNull() ? db["Text"].ToString() : "No translations found!";
		}

		public string[] GetWarningTexts(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_warning WHERE Language = '{0}' AND Command = '{1}'", Locale, command.ToLower());
			return !db.IsNull() ? db["Text"].ToString().Split('\n') : new string[] { "No translations found!" };
		}

		public string[] GetWarningTexts(string command, string channel)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channel WHERE Channel = '{0}'", channel.ToLower());
			string locale = !db.IsNull() ? db["Language"].ToString() : Locale;
			db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_warning WHERE Language = '{0}' AND Command = '{1}'", locale, command.ToLower());
			return !db.IsNull() ? db["Text"].ToString().Split('\n') : new string[] { "No translations found!" };
		}

		public string GetCommandText(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_command WHERE Language = '{0}' AND Command = '{1}'", Locale, command.ToLower());
			return !db.IsNull() ? db["Text"].ToString() : "No translations found!";
		}

		public string GetCommandText(string command, string channel)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channel WHERE Channel = '{0}'", channel.ToLower());
			string locale = !db.IsNull() ? db["Language"].ToString() : Locale;
			db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_command WHERE Language = '{0}' AND Command = '{1}'", locale, command.ToLower());
			return !db.IsNull() ? db["Text"].ToString() : "No translations found!";
		}

		public string[] GetCommandTexts(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_command WHERE Language = '{0}' AND Command = '{1}'", Locale, command.ToLower());
			return !db.IsNull() ? db["Text"].ToString().Split('\n') : new string[] { "No translations found!" };
		}

		public string[] GetCommandTexts(string command, string channel)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channel WHERE Channel = '{0}'", channel.ToLower());
			string locale = !db.IsNull() ? db["Language"].ToString() : Locale;
			db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_command WHERE Language = '{0}' AND Command = '{1}'", locale, command.ToLower());
			return !db.IsNull() ? db["Text"].ToString().Split('\n') : new string[] { "No translations found!" };
		}

		public string GetCommandHelpText(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_command_help WHERE Language = '{0}' AND Command = '{1}'", Locale, sUtilities.SqlEscape(command.ToLower()));
			return !db.IsNull() ? db["Text"].ToString() : "No translations found!";
		}

		public string GetCommandHelpText(string command, string channel)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channel WHERE Channel = '{0}'", channel.ToLower());
			string locale = !db.IsNull() ? db["Language"].ToString() : Locale;
			db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_command_help WHERE Language = '{0}' AND Command = '{1}'", locale, sUtilities.SqlEscape(command.ToLower()));
			return !db.IsNull() ? db["Text"].ToString() : "No translations found!";
		}

		public string GetCommandHelpText(string command, string channel, int rank)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channel WHERE Channel = '{0}'", channel.ToLower());
			string locale = !db.IsNull() ? db["Language"].ToString() : Locale;

			db = SchumixBase.DManager.QueryFirstRow("SELECT Text, Rank FROM localized_command_help WHERE Language = '{0}' AND Command = '{1}''", locale, sUtilities.SqlEscape(command.ToLower()));
			if(!db.IsNull())
			{
				if(Convert.ToInt32(db["Rank"].ToString()) == rank)
					return db["Text"].ToString();
				else if(Convert.ToInt32(db["Rank"].ToString()) == 9)
					return db["Text"].ToString();
				else
					return "No translations found!";
			}
			else
				return "No translations found!";
		}

		public string[] GetCommandHelpTexts(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_command_help WHERE Language = '{0}' AND Command = '{1}'", Locale, sUtilities.SqlEscape(command.ToLower()));
			return !db.IsNull() ? db["Text"].ToString().Split('\n') : new string[] { "No translations found!" };
		}

		public string[] GetCommandHelpTexts(string command, string channel)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channel WHERE Channel = '{0}'", channel.ToLower());
			string locale = !db.IsNull() ? db["Language"].ToString() : Locale;
			db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_command_help WHERE Language = '{0}' AND Command = '{1}'", locale, sUtilities.SqlEscape(command.ToLower()));
			return !db.IsNull() ? db["Text"].ToString().Split('\n') : new string[] { "No translations found!" };
		}

		public string[] GetCommandHelpTexts(string command, string channel, int rank)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channel WHERE Channel = '{0}'", channel.ToLower());
			string locale = !db.IsNull() ? db["Language"].ToString() : Locale;

			db = SchumixBase.DManager.QueryFirstRow("SELECT Text, Rank FROM localized_command_help WHERE Language = '{0}' AND Command = '{1}'", locale, sUtilities.SqlEscape(command.ToLower()));
			if(!db.IsNull())
			{
				if(Convert.ToInt32(db["Rank"].ToString()) == rank)
					return db["Text"].ToString().Split('\n');
				else if(Convert.ToInt32(db["Rank"].ToString()) == 9)
					return db["Text"].ToString().Split('\n');
				else
					return new string[] { "No translations found!" };
			}
			else
				return new string[] { "No translations found!" };
		}

		public bool IsAdminCommandHelp(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Rank FROM localized_command_help WHERE Language = '{0}' AND Command = '{1}'", Locale, sUtilities.SqlEscape(command.ToLower()));
			if(!db.IsNull())
			{
				if(Convert.ToInt32(db["Rank"].ToString()) == 0)
					return true;
				else if(Convert.ToInt32(db["Rank"].ToString()) == 1)
					return true;
				else if(Convert.ToInt32(db["Rank"].ToString()) == 2)
					return true;
				else
					return false;
			}
			else
				return false;
		}

		public bool IsAdminCommandHelp(string command, string channel)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channel WHERE Channel = '{0}'", channel.ToLower());
			string locale = !db.IsNull() ? db["Language"].ToString() : Locale;

			db = SchumixBase.DManager.QueryFirstRow("SELECT Rank FROM localized_command_help WHERE Language = '{0}' AND Command = '{1}'", locale, sUtilities.SqlEscape(command.ToLower()));
			if(!db.IsNull())
			{
				if(Convert.ToInt32(db["Rank"].ToString()) == 0)
					return true;
				else if(Convert.ToInt32(db["Rank"].ToString()) == 1)
					return true;
				else if(Convert.ToInt32(db["Rank"].ToString()) == 2)
					return true;
				else
					return false;
			}
			else
				return false;
		}

		public int GetCommandHelpRank(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Rank FROM localized_command_help WHERE Language = '{0}' AND Command = '{1}'", Locale, sUtilities.SqlEscape(command.ToLower()));
			return !db.IsNull() ? Convert.ToInt32(db["Rank"].ToString()) : -1;
		}

		public int GetCommandHelpRank(string command, string channel)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Language FROM channel WHERE Channel = '{0}'", channel.ToLower());
			string locale = !db.IsNull() ? db["Language"].ToString() : Locale;
			db = SchumixBase.DManager.QueryFirstRow("SELECT Rank FROM localized_command_help WHERE Language = '{0}' AND Command = '{1}'", locale, sUtilities.SqlEscape(command.ToLower()));
			return !db.IsNull() ? Convert.ToInt32(db["Rank"].ToString()) : -1;
		}

		public string GetConsoleWarningText(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_console_warning WHERE Language = '{0}' AND Command = '{1}'", Locale, command.ToLower());
			return !db.IsNull() ? db["Text"].ToString() : "No translations found!";
		}

		public string[] GetConsoleWarningTexts(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_console_warning WHERE Language = '{0}' AND Command = '{1}'", Locale, command.ToLower());
			return !db.IsNull() ? db["Text"].ToString().Split('\n') : new string[] { "No translations found!" };
		}

		public string GetConsoleCommandText(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_console_command WHERE Language = '{0}' AND Command = '{1}'", Locale, command.ToLower());
			return !db.IsNull() ? db["Text"].ToString() : "No translations found!";
		}

		public string[] GetConsoleCommandTexts(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_console_command WHERE Language = '{0}' AND Command = '{1}'", Locale, command.ToLower());
			return !db.IsNull() ? db["Text"].ToString().Split('\n') : new string[] { "No translations found!" };
		}

		public string GetConsoleCommandHelpText(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_console_command_help WHERE Language = '{0}' AND Command = '{1}'", Locale, sUtilities.SqlEscape(command.ToLower()));
			return !db.IsNull() ? db["Text"].ToString() : "No translations found!";
		}

		public string[] GetConsoleCommandHelpTexts(string command)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Text FROM localized_console_command_help WHERE Language = '{0}' AND Command = '{1}'", Locale, sUtilities.SqlEscape(command.ToLower()));
			return !db.IsNull() ? db["Text"].ToString().Split('\n') : new string[] { "No translations found!" };
		}
	}
}