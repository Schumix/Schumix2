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
using System.Collections.Generic;
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc
{
	public sealed class ChannelInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Sender sSender = Singleton<Sender>.Instance;
		private readonly List<string> ChannelFunction = new List<string>();
		private readonly Dictionary<string, string> _ChannelList = new Dictionary<string, string>();
		public Dictionary<string, string> CList
		{
			get { return _ChannelList; }
		}

		private ChannelInfo() {}

		public void ChannelList()
		{
			var db = SchumixBase.DManager.Query("SELECT Channel, Password FROM channel");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string channel = row["Channel"].ToString();
					string password = row["Password"].ToString();
					_ChannelList.Add(channel, password);
				}
			}
			else
				Log.Error("ChannelInfo", sLConsole.ChannelInfo("Text"));
		}

		public bool FSelect(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT FunctionStatus FROM schumix WHERE FunctionName = '{0}'", Name.ToLower());
			if(!db.IsNull())
			{
				string status = db["FunctionStatus"].ToString();
				return status == "on";
			}
			else
			{
				Log.Error("ChannelInfo", sLConsole.ChannelInfo("Text2"));
				return false;
			}
		}

		public bool FSelect(string Name, string Channel)
		{
			foreach(var channels in ChannelFunction)
			{
				string[] point = channels.Split(SchumixBase.Point);
				string[] point2 = point[1].Split(SchumixBase.Colon);

				if(point[0] == Channel.ToLower())
				{
					if(point2[0] == Name.ToLower())
						return point2[1] == "on";
				}
			}

			return false;
		}

		public bool FSelect(IFunctions Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT FunctionStatus FROM schumix WHERE FunctionName = '{0}'", Name.ToString().ToLower());
			if(!db.IsNull())
			{
				string status = db["FunctionStatus"].ToString();
				return status == "on";
			}
			else
			{
				Log.Error("ChannelInfo", sLConsole.ChannelInfo("Text2"));
				return false;
			}
		}

		public bool FSelect(IFunctions Name, string Channel)
		{
			foreach(var channels in ChannelFunction)
			{
				string[] point = channels.Split(SchumixBase.Point);
				string[] point2 = point[1].Split(SchumixBase.Colon);

				if(point[0] == Channel.ToLower())
				{
					if(point2[0] == Name.ToString().ToLower())
						return point2[1] == "on";
				}
			}

			return false;
		}

		public void ChannelFunctionReload()
		{
			ChannelFunction.Clear();

			var db = SchumixBase.DManager.Query("SELECT Channel FROM channel");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string channel = row["Channel"].ToString();

					var db1 = SchumixBase.DManager.QueryFirstRow("SELECT Functions FROM channel WHERE Channel = '{0}'", channel);
					if(!db1.IsNull())
					{
						string functions = db1["Functions"].ToString();
						string[] comma = functions.Split(SchumixBase.Comma);

						for(int x = 1; x < comma.Length; x++)
							ChannelFunction.Add(channel + SchumixBase.Point + comma[x]);
					}
					else
						Log.Error("ChannelInfo", sLConsole.ChannelInfo("Text3"));
				}
			}
			else
				Log.Error("ChannelInfo", sLConsole.ChannelInfo("Text3"));
		}

		public void ChannelListReload()
		{
			_ChannelList.Clear();
			var db = SchumixBase.DManager.Query("SELECT Channel, Password FROM channel");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string channel = row["Channel"].ToString();
					string password = row["Password"].ToString();
					_ChannelList.Add(channel, password);
				}
			}
			else
				Log.Error("ChannelInfo", sLConsole.ChannelInfo("Text4"));
		}

		public string ChannelFunctions(string name, string status, string channel)
		{
			string function = string.Empty;

			foreach(var channels in ChannelFunction)
			{
				string[] point = channels.Split(SchumixBase.Point);
				string[] point2 = point[1].Split(SchumixBase.Colon);

				if(point[0] == channel.ToLower())
				{
					if(point2[0] != name.ToLower())
						function += SchumixBase.Comma + point[1];
				}
			}

			foreach(var channels in ChannelFunction)
			{
				string[] point = channels.Split(SchumixBase.Point);
				string[] point2 = point[1].Split(SchumixBase.Colon);

				if(point[0] == channel.ToLower())
				{
					if(point2[0] == name.ToLower())
						function += SchumixBase.Comma + name + SchumixBase.Colon + status;
				}
			}

			return function;
		}

		public string FunctionsInfo()
		{
			var db = SchumixBase.DManager.Query("SELECT FunctionName, FunctionStatus FROM schumix");
			if(!db.IsNull())
			{
				string on = string.Empty, off = string.Empty;

				foreach(DataRow row in db.Rows)
				{
					string name = row["FunctionName"].ToString();
					string status = row["FunctionStatus"].ToString();

					if(status == "on")
						on += name + SchumixBase.Space;
					else
						off += name + SchumixBase.Space;
				}

				return on + "|" + off;
			}
			else
				return "Hibás lekérdezés!";
		}

		public string ChannelFunctionsInfo(string channel)
		{
			string on = string.Empty, off = string.Empty;

			foreach(var channels in ChannelFunction)
			{
				string[] point = channels.Split(SchumixBase.Point);
				string[] point2 = point[1].Split(SchumixBase.Colon);

				if(point[0] == channel.ToLower())
				{
					if(point2[1] == "on")
						on += point2[0] + SchumixBase.Space;
					else
						off += point2[0] + SchumixBase.Space;
				}
			}

			return on + "|" + off;
		}

		public void JoinChannel()
		{
			Log.Debug("ChannelInfo", sLConsole.ChannelInfo("Text5"));
			bool error = false;

			foreach(var channel in _ChannelList)
			{
				sSender.Join(channel.Key, channel.Value);

				if(IsIgnore(channel.Key))
				{
					error = IsIgnore(channel.Key);
					SchumixBase.DManager.Update("channel", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.ChannelInfo("Text10")), string.Format("Channel = '{0}'", channel.Key));
				}
				else
					SchumixBase.DManager.Update("channel", "Enabled = 'true', Error = ''", string.Format("Channel = '{0}'", channel.Key));
			}

			ChannelFunctionReload();
			var db = SchumixBase.DManager.Query("SELECT Enabled FROM channel");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					if(!Convert.ToBoolean(row["Enabled"].ToString()))
						error = true;
				}
			}
			else
				Log.Error("ChannelInfo", sLConsole.ChannelInfo("Text6"));

			if(!error)
				Log.Success("ChannelInfo", sLConsole.ChannelInfo("Text7"));
			else
				Log.Warning("ChannelInfo", sLConsole.ChannelInfo("Text8"));

			if(IRCConfig.IgnoreChannels.Length > 0)
				Log.Notice("ChannelInfo", sLConsole.ChannelInfo("Text9"), IRCConfig.IgnoreChannels);

			if(SchumixBase.STime)
			{
				SchumixBase.timer.StartTimer();
				SchumixBase.STime = false;
			}
		}

		public bool IsIgnore(string channel)
		{
			bool enabled = false;
			string[] ignore = IRCConfig.IgnoreChannels.Split(SchumixBase.Comma);

			if(ignore.Length > 1)
			{
				foreach(var _ignore in ignore)
				{
					if(channel.ToLower() == _ignore.ToLower())
						enabled = true;
				}
			}
			else
			{
				if(channel.ToLower() == IRCConfig.IgnoreChannels.ToLower())
					enabled = true;
			}

			return enabled;
		}
	}
}