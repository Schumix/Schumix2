/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010-2013 Megax <http://megax.yeahunter.hu/>
 * Copyright (C) 2013-2014 Schumix Team <http://schumix.eu/>
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
using System.Text;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Framework.Localization
{
	public sealed class LocalizationManager
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly object WriteLock = new object();
		public string Locale { get; set; }
		private LocalizationManager() {}

		public string GetWarningText(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedWarningMap().ContainsKey(Locale + command) ? SchumixBase.sCacheDB.LocalizedWarningMap()[Locale + command].Text : sLConsole.Translations("NoFound");
			}
		}

		public string GetWarningText(string command, string channel, string servername)
		{
			lock(WriteLock)
			{
				string locale = SchumixBase.sCacheDB.ChannelsMap().ContainsKey(servername + channel) ? SchumixBase.sCacheDB.ChannelsMap()[servername + channel].Language : Locale;
				return SchumixBase.sCacheDB.LocalizedWarningMap().ContainsKey(locale + command) ? SchumixBase.sCacheDB.LocalizedWarningMap()[locale + command].Text : sLConsole.Translations("NoFound", locale);
			}
		}

		public string[] GetWarningTexts(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedWarningMap().ContainsKey(Locale + command) ? Split(SchumixBase.sCacheDB.LocalizedWarningMap()[Locale + command].Text) : new string[] { sLConsole.Translations("NoFound") };
			}
		}

		public string[] GetWarningTexts(string command, string channel, string servername)
		{
			lock(WriteLock)
			{
				string locale = SchumixBase.sCacheDB.ChannelsMap().ContainsKey(servername + channel) ? SchumixBase.sCacheDB.ChannelsMap()[servername + channel].Language : Locale;
				return SchumixBase.sCacheDB.LocalizedWarningMap().ContainsKey(locale + command) ? Split(SchumixBase.sCacheDB.LocalizedWarningMap()[locale + command].Text) : new string[] { sLConsole.Translations("NoFound", locale) };
			}
		}

		public string GetCommandText(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedCommandMap().ContainsKey(Locale + command.ToLower()) ? SchumixBase.sCacheDB.LocalizedCommandMap()[Locale + command.ToLower()].Text : sLConsole.Translations("NoFound");
			}
		}

		public string GetCommandText(string command, string channel, string servername)
		{
			lock(WriteLock)
			{
				string locale = SchumixBase.sCacheDB.ChannelsMap().ContainsKey(servername + channel) ? SchumixBase.sCacheDB.ChannelsMap()[servername + channel].Language : Locale;
				return SchumixBase.sCacheDB.LocalizedCommandMap().ContainsKey(locale + command.ToLower()) ? SchumixBase.sCacheDB.LocalizedCommandMap()[locale + command.ToLower()].Text : sLConsole.Translations("NoFound", locale);
			}
		}

		public string[] GetCommandTexts(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedCommandMap().ContainsKey(Locale + command.ToLower()) ? Split(SchumixBase.sCacheDB.LocalizedCommandMap()[Locale + command.ToLower()].Text) : new string[] { sLConsole.Translations("NoFound") };
			}
		}

		public string[] GetCommandTexts(string command, string channel, string servername)
		{
			lock(WriteLock)
			{
				string locale = SchumixBase.sCacheDB.ChannelsMap().ContainsKey(servername + channel) ? SchumixBase.sCacheDB.ChannelsMap()[servername + channel].Language : Locale;
				return SchumixBase.sCacheDB.LocalizedCommandMap().ContainsKey(locale + command.ToLower()) ? Split(SchumixBase.sCacheDB.LocalizedCommandMap()[locale + command.ToLower()].Text) : new string[] { sLConsole.Translations("NoFound", locale) };
			}
		}

		public string GetCommandHelpText(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedCommandHelpMap().ContainsKey(Locale + command.ToLower()) ? SchumixBase.sCacheDB.LocalizedCommandHelpMap()[Locale + command.ToLower()].Text : sLConsole.Other("NoFoundHelpCommand");
			}
		}

		public string GetCommandHelpText(string command, string channel, string servername)
		{
			lock(WriteLock)
			{
				string locale = SchumixBase.sCacheDB.ChannelsMap().ContainsKey(servername + channel) ? SchumixBase.sCacheDB.ChannelsMap()[servername + channel].Language : Locale;
				return SchumixBase.sCacheDB.LocalizedCommandHelpMap().ContainsKey(locale + command.ToLower()) ? SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Text : sLConsole.Other("NoFoundHelpCommand", locale);
			}
		}

		public string GetCommandHelpText(string command, string channel, string servername, int rank)
		{
			lock(WriteLock)
			{
				string locale = SchumixBase.sCacheDB.ChannelsMap().ContainsKey(servername + channel) ? SchumixBase.sCacheDB.ChannelsMap()[servername + channel].Language : Locale;
				if(SchumixBase.sCacheDB.LocalizedCommandHelpMap().ContainsKey(locale + command.ToLower()))
				{
					if(SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Rank == rank)
						return SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Text;
					else if(SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Rank == 9)
						return SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Text;
					else
						return sLConsole.Other("NoFoundHelpCommand", locale);
				}
				else
					return sLConsole.Other("NoFoundHelpCommand", locale);
			}
		}

		public string[] GetCommandHelpTexts(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedCommandHelpMap().ContainsKey(Locale + command.ToLower()) ? Split(SchumixBase.sCacheDB.LocalizedCommandHelpMap()[Locale + command.ToLower()].Text) : new string[] { sLConsole.Other("NoFoundHelpCommand") };
			}
		}

		public string[] GetCommandHelpTexts(string command, string channel, string servername)
		{
			lock(WriteLock)
			{
				string locale = SchumixBase.sCacheDB.ChannelsMap().ContainsKey(servername + channel) ? SchumixBase.sCacheDB.ChannelsMap()[servername + channel].Language : Locale;
				return SchumixBase.sCacheDB.LocalizedCommandHelpMap().ContainsKey(locale + command.ToLower()) ? Split(SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Text) : new string[] { sLConsole.Other("NoFoundHelpCommand", locale) };
			}
		}

		public string[] GetCommandHelpTexts(string command, string channel, string servername, int rank)
		{
			lock(WriteLock)
			{
				string locale = SchumixBase.sCacheDB.ChannelsMap().ContainsKey(servername + channel) ? SchumixBase.sCacheDB.ChannelsMap()[servername + channel].Language : Locale;
				if(SchumixBase.sCacheDB.LocalizedCommandHelpMap().ContainsKey(locale + command.ToLower()))
				{
					if(SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Rank == rank)
						return Split(SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Text);
					else if(SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Rank == 9)
						return Split(SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Text);
					else
						return new string[] { sLConsole.Other("NoFoundHelpCommand", locale) };
				}
				else
					return new string[] { sLConsole.Other("NoFoundHelpCommand", locale) };
			}
		}

		public bool IsAdminCommandHelp(string command)
		{
			lock(WriteLock)
			{
				if(SchumixBase.sCacheDB.LocalizedCommandHelpMap().ContainsKey(Locale + command.ToLower()))
				{
					if(SchumixBase.sCacheDB.LocalizedCommandHelpMap()[Locale + command.ToLower()].Rank == 0 ||
						SchumixBase.sCacheDB.LocalizedCommandHelpMap()[Locale + command.ToLower()].Rank == 1 ||
						SchumixBase.sCacheDB.LocalizedCommandHelpMap()[Locale + command.ToLower()].Rank == 2)
						return true;
					else
						return false;
				}
				else
					return false;
			}
		}

		public bool IsAdminCommandHelp(string command, string channel, string servername)
		{
			lock(WriteLock)
			{
				string locale = SchumixBase.sCacheDB.ChannelsMap().ContainsKey(servername + channel) ? SchumixBase.sCacheDB.ChannelsMap()[servername + channel].Language : Locale;
				if(SchumixBase.sCacheDB.LocalizedCommandHelpMap().ContainsKey(locale + command.ToLower()))
				{
					if(SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Rank == 0 ||
						SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Rank == 1 ||
						SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Rank == 2)
						return true;
					else
						return false;
				}
				else
					return false;
			}
		}

		public int GetCommandHelpRank(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedCommandHelpMap().ContainsKey(Locale + command.ToLower()) ? SchumixBase.sCacheDB.LocalizedCommandHelpMap()[Locale + command.ToLower()].Rank : -1;
			}
		}

		public int GetCommandHelpRank(string command, string channel, string servername)
		{
			lock(WriteLock)
			{
				string locale = SchumixBase.sCacheDB.ChannelsMap().ContainsKey(servername + channel) ? SchumixBase.sCacheDB.ChannelsMap()[servername + channel].Language : Locale;
				return SchumixBase.sCacheDB.LocalizedCommandHelpMap().ContainsKey(locale + command.ToLower()) ? SchumixBase.sCacheDB.LocalizedCommandHelpMap()[locale + command.ToLower()].Rank : -1;
			}
		}

		public string GetConsoleWarningText(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedConsoleWarningMap().ContainsKey(Locale + command) ? SchumixBase.sCacheDB.LocalizedConsoleWarningMap()[Locale + command].Text : sLConsole.Translations("NoFound");
			}
		}

		public string[] GetConsoleWarningTexts(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedConsoleWarningMap().ContainsKey(Locale + command) ? Split(SchumixBase.sCacheDB.LocalizedConsoleWarningMap()[Locale + command].Text) : new string[] { sLConsole.Translations("NoFound") };
			}
		}

		public string GetConsoleCommandText(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedConsoleCommandMap().ContainsKey(Locale + command.ToLower()) ? SchumixBase.sCacheDB.LocalizedConsoleCommandMap()[Locale + command.ToLower()].Text : sLConsole.Translations("NoFound");
			}
		}

		public string[] GetConsoleCommandTexts(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedConsoleCommandMap().ContainsKey(Locale + command.ToLower()) ? Split(SchumixBase.sCacheDB.LocalizedConsoleCommandMap()[Locale + command.ToLower()].Text) : new string[] { sLConsole.Translations("NoFound") };
			}
		}

		public string GetConsoleCommandHelpText(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedConsoleCommandHelpMap().ContainsKey(Locale + command.ToLower()) ? SchumixBase.sCacheDB.LocalizedConsoleCommandHelpMap()[Locale + command.ToLower()].Text : sLConsole.Other("NoFoundHelpCommand");
			}
		}

		public string[] GetConsoleCommandHelpTexts(string command)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.LocalizedConsoleCommandHelpMap().ContainsKey(Locale + command.ToLower()) ? Split(SchumixBase.sCacheDB.LocalizedConsoleCommandHelpMap()[Locale + command.ToLower()].Text) : new string[] { sLConsole.Other("NoFoundHelpCommand") };
			}
		}

		public string GetChannelLocalization(string channel, string servername)
		{
			lock(WriteLock)
			{
				return SchumixBase.sCacheDB.ChannelsMap().ContainsKey(servername + channel) ? SchumixBase.sCacheDB.ChannelsMap()[servername + channel].Language : Locale;
			}
		}

		private string[] Split(string Text)
		{
			lock(WriteLock)
			{
				return SQLiteConfig.Enabled ? Text.Split(new string[] { @"\n" }, StringSplitOptions.None) : Text.Split(SchumixBase.NewLine);
			}
		}
	}
}