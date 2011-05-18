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
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{	
	public enum AdminFlag
	{
		HalfOperator  = 0,
		Operator      = 1,
		Administrator = 2
	};

	public class CommandInfo
	{
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;

		protected CommandInfo()
		{
			//Log.Notice("CommandInfo", "CommandInfo elindult.");
		}

		protected bool IsAdmin(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM admins WHERE Name = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(!db.IsNull())
				return true;

			return false;
		}

		protected bool IsAdmin(string Name, AdminFlag Flag)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM admins WHERE Name = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(!db.IsNull())
			{
				int flag = Convert.ToInt32(db["Flag"]);

				if(Flag != (AdminFlag)flag)
					return false;

				return true;
			}

			return false;
		}

		protected bool IsAdmin(string Name, string Vhost, AdminFlag Flag)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Vhost, Flag FROM admins WHERE Name = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(!db.IsNull())
			{
				string vhost = db["Vhost"].ToString();

				if(Vhost != vhost)
					return false;

				int flag = Convert.ToInt32(db["Flag"]);

				if((flag == 1 && Flag == AdminFlag.HalfOperator) ||
					(flag == 2 && Flag == AdminFlag.HalfOperator) ||
					(flag == 2 && Flag == AdminFlag.Operator))
					return true;

				if(Flag != (AdminFlag)flag)
					return false;

				return true;
			}

			return false;
		}

		protected void CNick(IRCMessage sIRCMessage)
		{
			bool channel = sIRCMessage.Channel.StartsWith("#");
			if(!channel)
				sIRCMessage.Channel = sIRCMessage.Nick;
		}

		protected int Adminflag(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM admins WHERE Name = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(!db.IsNull())
				return Convert.ToInt32(db["Flag"]);
			else
				return -1;
		}

		protected int Adminflag(string Name, string Vhost)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Vhost, Flag FROM admins WHERE Name = '{0}'", sUtilities.SqlEscape(Name.ToLower()));
			if(!db.IsNull())
			{
				string vhost = db["Vhost"].ToString();

				if(Vhost != vhost)
					return -1;

				return Convert.ToInt32(db["Flag"]);
			}
			else
				return -1;
		}
	}
}
