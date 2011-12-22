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
		private readonly Dictionary<string, string> _ChannelList = new Dictionary<string, string>();
		private readonly Dictionary<string, string> ChannelFunction = new Dictionary<string, string>();

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
			if(IFunctionsClass.Functions.ContainsKey(Name.ToLower()))
				return IFunctionsClass.Functions[Name.ToLower()] == SchumixBase.On;

			return false;
		}

		public bool FSelect(string Name, string Channel)
		{
			return (ChannelFunction.ContainsKey(Channel.ToLower()) && ChannelFunction[Channel.ToLower()].Contains(Name.ToLower() + SchumixBase.Colon + SchumixBase.On));
		}

		public bool SearchFunction(string Name)
		{
			return IFunctionsClass.Functions.ContainsKey(Name.ToString().ToLower());
		}

		public bool SearchChannelFunction(string Name)
		{
			foreach(var name in Enum.GetNames(typeof(IChannelFunctions)))
			{
				if(Name.ToLower() == name.ToString().ToLower())
					return true;
			}

			return false;
		}

		public bool FSelect(IFunctions Name)
		{
			if(IFunctionsClass.Functions.ContainsKey(Name.ToString().ToLower()))
				return IFunctionsClass.Functions[Name.ToString().ToLower()] == SchumixBase.On;

			return false;
		}

		public bool FSelect(IChannelFunctions Name, string Channel)
		{
			return (ChannelFunction.ContainsKey(Channel.ToLower()) && ChannelFunction[Channel.ToLower()].Contains(Name.ToString().ToLower() + SchumixBase.Colon + SchumixBase.On));
		}

		public void FunctionsReload()
		{
			IFunctionsClass.Functions.Clear();

			var db = SchumixBase.DManager.Query("SELECT FunctionName, FunctionStatus FROM schumix");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string name = row["FunctionName"].ToString();
					string status = row["FunctionStatus"].ToString();
					IFunctionsClass.Functions.Add(name.ToLower(), status.ToLower());
				}
			}
			else
				Log.Error("ChannelInfo", sLConsole.ChannelInfo("Text11"));
		}

		public void ChannelFunctionsReload()
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
						ChannelFunction.Add(channel, db1["Functions"].ToString());
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
			string functions = string.Empty;

			if(ChannelFunction.ContainsKey(channel.ToLower()))
			{
				foreach(var comma in ChannelFunction[channel.ToLower()].Split(SchumixBase.Comma))
				{
					if(comma == string.Empty)
						continue;

					string[] point = comma.Split(SchumixBase.Colon);

					if(point[0] != name.ToLower())
						functions += SchumixBase.Comma + comma;
				}

				foreach(var comma in ChannelFunction[channel.ToLower()].Split(SchumixBase.Comma))
				{
					if(comma == string.Empty)
						continue;

					string[] point = comma.Split(SchumixBase.Colon);

					if(point[0] == name.ToLower())
						functions += SchumixBase.Comma + name.ToLower() + SchumixBase.Colon + status.ToLower();
				}
			}

			return functions;
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

					if(status == SchumixBase.On)
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

			if(ChannelFunction.ContainsKey(channel.ToLower()))
			{
				foreach(var comma in ChannelFunction[channel.ToLower()].Split(SchumixBase.Comma))
				{
					if(comma == string.Empty)
						continue;

					string[] point = comma.Split(SchumixBase.Colon);

					if(point[1] == SchumixBase.On)
						on += point[0] + SchumixBase.Space;
					else
						off += point[0] + SchumixBase.Space;
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

			ChannelFunctionsReload();
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