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
using System.Collections.Generic;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Framework.Database.Cache
{
	public sealed class CacheDB
	{
		private readonly Dictionary<string, LocalizedConsoleCommand> _LocalizedConsoleCommandMap = new Dictionary<string, LocalizedConsoleCommand>();
		private readonly Dictionary<string, LocalizedConsoleCommandHelp> _LocalizedConsoleCommandHelpMap = new Dictionary<string, LocalizedConsoleCommandHelp>();
		private readonly Dictionary<string, LocalizedConsoleWarning> _LocalizedConsoleWarningMap = new Dictionary<string, LocalizedConsoleWarning>();
		private readonly Dictionary<string, LocalizedCommand> _LocalizedCommandMap = new Dictionary<string, LocalizedCommand>();
		private readonly Dictionary<string, LocalizedCommandHelp> _LocalizedCommandHelpMap = new Dictionary<string, LocalizedCommandHelp>();
		private readonly Dictionary<string, LocalizedWarning> _LocalizedWarningMap = new Dictionary<string, LocalizedWarning>();
		private readonly Dictionary<string, Channels> _ChannelsMap = new Dictionary<string, Channels>();
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly object Lock = new object();

		public Dictionary<string, LocalizedConsoleCommand> LocalizedConsoleCommandMap()
		{
			return _LocalizedConsoleCommandMap;
		}

		public Dictionary<string, LocalizedConsoleCommandHelp> LocalizedConsoleCommandHelpMap()
		{
			return _LocalizedConsoleCommandHelpMap;
		}

		public Dictionary<string, LocalizedConsoleWarning> LocalizedConsoleWarningMap()
		{
			return _LocalizedConsoleWarningMap;
		}

		public Dictionary<string, LocalizedCommand> LocalizedCommandMap()
		{
			return _LocalizedCommandMap;
		}

		public Dictionary<string, LocalizedCommandHelp> LocalizedCommandHelpMap()
		{
			return _LocalizedCommandHelpMap;
		}

		public Dictionary<string, LocalizedWarning> LocalizedWarningMap()
		{
			return _LocalizedWarningMap;
		}

		public Dictionary<string, Channels> ChannelsMap()
		{
			return _ChannelsMap;
		}

		public CacheDB()
		{
			Log.Notice("CacheDB", sLConsole.GetString("Successfully started the CacheDB."));
		}

		public void Load(string value = "")
		{
			lock(Lock)
			{
				try
				{
					if(value.IsNullOrEmpty())
						Log.Debug("CacheDB", sLConsole.GetString("Load tables from the database to the core."));
					else
						Log.Debug("CacheDB", sLConsole.GetString("{0} loaded to the core."), value);

					switch(value.ToLower())
					{
						case "localizedconsolecommand":
							LoadLocalizedConsoleCommand();
							break;
						case "localizedconsolecommandhelp":
							LoadLocalizedConsoleCommandHelp();
							break;
						case "localizedconsolewarning":
							LoadLocalizedConsoleWarning();
							break;
						case "localizedcommand":
							LoadLocalizedCommand();
							break;
						case "localizedcommandhelp":
							LoadLocalizedCommandHelp();
							break;
						case "localizedwarning":
							LoadLocalizedWarning();
							break;
						case "channels":
							LoadChannels();
							break;
						default:
							LoadLocalizedConsoleCommand();
							LoadLocalizedConsoleCommandHelp();
							LoadLocalizedConsoleWarning();
							LoadLocalizedCommand();
							LoadLocalizedCommandHelp();
							LoadLocalizedWarning();
							LoadChannels();
							break;
					}
				}
				catch(Exception e)
				{
					Log.Error("CacheDB", sLConsole.GetString("Failure details: {0}"), e.Message);
				}
			}
		}

		public void Unload(string value = "")
		{
			lock(Lock)
			{
				Clean(value);
			}
		}

		public void Reload(string value = "")
		{
			lock(Lock)
			{
				Clean(value);
				Load(value);
			}
		}

		public void Clean(string value = "")
		{
			lock(Lock)
			{
				try
				{
					if(value.IsNullOrEmpty())
						Log.Debug("CacheDB", sLConsole.GetString("Tables cleared from the core."));
					else
						Log.Debug("CacheDB", sLConsole.GetString("Table {0} deleted from the core."), value);

					switch(value.ToLower())
					{
						case "localizedconsolecommand":
							_LocalizedConsoleCommandMap.Clear();
							break;
						case "localizedconsolecommandhelp":
							_LocalizedConsoleCommandHelpMap.Clear();
							break;
						case "localizedconsolewarning":
							_LocalizedConsoleWarningMap.Clear();
							break;
						case "localizedcommand":
							_LocalizedCommandMap.Clear();
							break;
						case "localizedcommandhelp":
							_LocalizedCommandHelpMap.Clear();
							break;
						case "localizedcommandwarning":
							_LocalizedWarningMap.Clear();
							break;
						case "channels":
							_ChannelsMap.Clear();
							break;
						default:
							_LocalizedConsoleCommandMap.Clear();
							_LocalizedConsoleCommandHelpMap.Clear();
							_LocalizedConsoleWarningMap.Clear();
							_LocalizedCommandMap.Clear();
							_LocalizedCommandHelpMap.Clear();
							_LocalizedWarningMap.Clear();
							_ChannelsMap.Clear();
							break;
					}
				}
				catch(Exception e)
				{
					Log.Error("CacheDB", sLConsole.GetString("Failure details: {0}"), e.Message);
				}
			}
		}

		private void LoadLocalizedConsoleCommand()
		{
			var db = SchumixBase.DManager.Query("SELECT Id, Language, Command, Text FROM localized_console_command");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					var map = new LocalizedConsoleCommand();
					map.Id = Convert.ToInt32(row["Id"].ToString());
					map.Language = row["Language"].ToString();
					map.Command = row["Command"].ToString();
					map.Text = row["Text"].ToString();
					_LocalizedConsoleCommandMap.Add(map.Language + map.Command, map);
				}
			}
		}

		private void LoadLocalizedConsoleCommandHelp()
		{
			var db = SchumixBase.DManager.Query("SELECT Id, Language, Command, Text FROM localized_console_command_help");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					var map = new LocalizedConsoleCommandHelp();
					map.Id = Convert.ToInt32(row["Id"].ToString());
					map.Language = row["Language"].ToString();
					map.Command = row["Command"].ToString();
					map.Text = row["Text"].ToString();
					_LocalizedConsoleCommandHelpMap.Add(map.Language + map.Command, map);
				}
			}
		}

		private void LoadLocalizedConsoleWarning()
		{
			var db = SchumixBase.DManager.Query("SELECT Id, Language, Command, Text FROM localized_console_warning");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					var map = new LocalizedConsoleWarning();
					map.Id = Convert.ToInt32(row["Id"].ToString());
					map.Language = row["Language"].ToString();
					map.Command = row["Command"].ToString();
					map.Text = row["Text"].ToString();
					_LocalizedConsoleWarningMap.Add(map.Language + map.Command, map);
				}
			}
		}

		private void LoadLocalizedCommand()
		{
			var db = SchumixBase.DManager.Query("SELECT Id, Language, Command, Text FROM localized_command");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					var map = new LocalizedCommand();
					map.Id = Convert.ToInt32(row["Id"].ToString());
					map.Language = row["Language"].ToString();
					map.Command = row["Command"].ToString();
					map.Text = row["Text"].ToString();
					_LocalizedCommandMap.Add(map.Language + map.Command, map);
				}
			}
		}

		private void LoadLocalizedCommandHelp()
		{
			var db = SchumixBase.DManager.Query("SELECT Id, Language, Command, Rank, Text FROM localized_command_help");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					var map = new LocalizedCommandHelp();
					map.Id = Convert.ToInt32(row["Id"].ToString());
					map.Language = row["Language"].ToString();
					map.Command = row["Command"].ToString();
					map.Rank = Convert.ToInt32(row["Rank"].ToString());
					map.Text = row["Text"].ToString();
					_LocalizedCommandHelpMap.Add(map.Language + map.Command, map);
				}
			}
		}

		private void LoadLocalizedWarning()
		{
			var db = SchumixBase.DManager.Query("SELECT Id, Language, Command, Text FROM localized_warning");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					var map = new LocalizedWarning();
					map.Id = Convert.ToInt32(row["Id"].ToString());
					map.Language = row["Language"].ToString();
					map.Command = row["Command"].ToString();
					map.Text = row["Text"].ToString();
					_LocalizedWarningMap.Add(map.Language + map.Command, map);
				}
			}
		}

		private void LoadChannels()
		{
			var db = SchumixBase.DManager.Query("SELECT Id, ServerId, ServerName, Functions, Channel, Password, Enabled, Hidden, Error, Language FROM channels");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					var map = new Channels();
					map.Id = Convert.ToInt32(row["Id"].ToString());
					map.ServerId = Convert.ToInt32(row["ServerId"].ToString());
					map.ServerName = row["ServerName"].ToString();
					map.Functions = row["Functions"].ToString();
					map.Channel = row["Channel"].ToString();
					map.Password = row["Password"].ToString();
					map.Enabled = Convert.ToBoolean(row["Enabled"].ToString());
					map.Hidden = Convert.ToBoolean(row["Hidden"].ToString());
					map.Error = row["Error"].ToString();
					map.Language = row["Language"].ToString();
					_ChannelsMap.Add(map.ServerName + map.Channel, map);
				}
			}
		}
	}
}