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
using System.Collections.Generic;
using Schumix.API;
using Schumix.API.Functions;
using Schumix.Irc.Ignore;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc
{
	public sealed class ChannelInfo
	{
		private readonly Dictionary<string, string> ChannelFunction = new Dictionary<string, string>();
		private readonly Dictionary<string, string> _ChannelList = new Dictionary<string, string>();
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly IrcBase sIrcBase = Singleton<IrcBase>.Instance;
		private readonly object WriteLock = new object();
		private readonly IgnoreChannel sIgnoreChannel;
		private readonly Sender sSender;
		private string _servername;

		public Dictionary<string, string> CList
		{
			get { return _ChannelList; }
		}

		public Dictionary<string, string> CFunction
		{
			get { return ChannelFunction; }
		}

		public ChannelInfo(string ServerName)
		{
			_servername = ServerName;
			sSender = sIrcBase.Networks[ServerName].sSender;
			sIgnoreChannel = sIrcBase.Networks[ServerName].sIgnoreChannel;
		}

		public void ChannelList()
		{
			var db = SchumixBase.DManager.Query("SELECT Channel, Password FROM channels WHERE ServerName = '{0}'", _servername);
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
			lock(WriteLock)
			{
				if(IFunctionsClass.ServerList[_servername].Functions.ContainsKey(Name.ToLower()))
					return IFunctionsClass.ServerList[_servername].Functions[Name.ToLower()] == SchumixBase.On;

				return false;
			}
		}

		public bool FSelect(string Name, string Channel)
		{
			lock(WriteLock)
			{
				if(ChannelFunction.ContainsKey(Channel.ToLower()))
				{
					foreach(var comma in ChannelFunction[Channel.ToLower()].Split(SchumixBase.Comma))
					{
						if(comma == string.Empty)
							continue;

						string[] point = comma.Split(SchumixBase.Colon);

						if(point[0] == Name.ToLower())
							return point[1] == SchumixBase.On;
					}
				}

				return false;
			}
		}

		public bool SearchFunction(string Name)
		{
			return IFunctionsClass.ServerList[_servername].Functions.ContainsKey(Name.ToString().ToLower());
		}

		public bool SearchChannelFunction(string Name)
		{
			foreach(var channel in ChannelFunction)
			{
				foreach(var comma in channel.Value.Split(SchumixBase.Comma))
				{
					if(comma == string.Empty)
						continue;

					string[] point = comma.Split(SchumixBase.Colon);

					if(point[0] == Name.ToLower())
						return true;
				}
			}

			return false;
		}

		public bool FSelect(IFunctions Name)
		{
			lock(WriteLock)
			{
				if(IFunctionsClass.ServerList[_servername].Functions.ContainsKey(Name.ToString().ToLower()))
					return IFunctionsClass.ServerList[_servername].Functions[Name.ToString().ToLower()] == SchumixBase.On;

				return false;
			}
		}

		public bool FSelect(IChannelFunctions Name, string Channel)
		{
			lock(WriteLock)
			{
				if(ChannelFunction.ContainsKey(Channel.ToLower()))
				{
					foreach(var comma in ChannelFunction[Channel.ToLower()].Split(SchumixBase.Comma))
					{
						if(comma == string.Empty)
							continue;

						string[] point = comma.Split(SchumixBase.Colon);

						if(point[0] == Name.ToString().ToLower())
							return point[1] == SchumixBase.On;
					}
				}

				return false;
			}
		}

		public void FunctionsReload()
		{
			IFunctionsClass.ServerList[_servername].Functions.Clear();
			var db = SchumixBase.DManager.Query("SELECT FunctionName, FunctionStatus FROM schumix WHERE ServerName = '{0}'", _servername);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string name = row["FunctionName"].ToString();
					string status = row["FunctionStatus"].ToString();
					IFunctionsClass.ServerList[_servername].Functions.Add(name.ToLower(), status.ToLower());
				}
			}
			else
				Log.Error("ChannelInfo", sLConsole.ChannelInfo("Text11"));
		}

		public void ChannelFunctionsReload()
		{
			ChannelFunction.Clear();
			var db = SchumixBase.DManager.Query("SELECT Channel FROM channels WHERE ServerName = '{0}'", _servername);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string channel = row["Channel"].ToString();
					var db1 = SchumixBase.DManager.QueryFirstRow("SELECT Functions FROM channels WHERE Channel = '{0}' And ServerName = '{1}'", channel, _servername);
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
			var db = SchumixBase.DManager.Query("SELECT Channel, Password FROM channels WHERE ServerName = '{0}'", _servername);
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
			var db = SchumixBase.DManager.Query("SELECT FunctionName, FunctionStatus FROM schumix WHERE ServerName = '{0}'", _servername);
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
				return string.Empty;
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
				sSender.Join(channel.Key, channel.Value.Trim());

				if(sIgnoreChannel.IsIgnore(channel.Key))
				{
					error = true;
					SchumixBase.DManager.Update("channels", string.Format("Enabled = 'false', Error = '{0}'", sLConsole.ChannelInfo("Text10")), string.Format("Channel = '{0}' And ServerName = '{1}'", channel.Key, _servername));
				}
				else
					SchumixBase.DManager.Update("channels", "Enabled = 'true', Error = ''", string.Format("Channel = '{0}' And ServerName = '{1}'", channel.Key, _servername));
			}

			ChannelFunctionsReload();
			var db = SchumixBase.DManager.Query("SELECT Enabled FROM channels WHERE ServerName = '{0}'", _servername);
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

			if(IRCConfig.List[_servername].IgnoreChannels.Length > 0)
				Log.Notice("ChannelInfo", sLConsole.ChannelInfo("Text9"), IRCConfig.List[_servername].IgnoreChannels);

			if(SchumixBase.STime)
			{
				SchumixBase.timer.StartTimer();
				SchumixBase.STime = false;
			}
		}
	}
}