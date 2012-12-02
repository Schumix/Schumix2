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
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Commands
{	
	public abstract class CommandInfo
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public string servername;

		protected CommandInfo(string ServerName)
		{
			servername = ServerName;
			Log.Debug("CommandInfo", sLConsole.CommandInfo("Text"));
		}

		public bool IsAdmin(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername);
			return !db.IsNull();
		}

		public bool IsAdmin(string Name, AdminFlag Flag)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername);
			if(!db.IsNull())
			{
				int flag = Convert.ToInt32(db["Flag"]);
				return Flag == (AdminFlag)flag;
			}

			return false;
		}

		public bool IsAdmin(string Name, string Vhost)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Vhost FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername);
			if(!db.IsNull())
			{
				string vhost = db["Vhost"].ToString();
				return Vhost == vhost;
			}

			return false;
		}

		public bool IsAdmin(string Name, string Vhost, AdminFlag Flag)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Vhost, Flag FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername);
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

				return Flag == (AdminFlag)flag;
			}

			return false;
		}

		public bool IsChannel(string Name)
		{
			return (Name.Length >= 2 && Name.Trim().Length > 1 && Name.Substring(0, 1) == "#");
		}

		public int Adminflag(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername);
			return !db.IsNull() ? Convert.ToInt32(db["Flag"]) : -1;
		}

		public int Adminflag(string Name, string Vhost)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Vhost, Flag FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername);
			if(!db.IsNull())
			{
				string vhost = db["Vhost"].ToString();
				return Vhost != vhost ? -1 : Convert.ToInt32(db["Flag"]);
			}
			else
				return -1;
		}

		public void RandomVhost(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT * FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername);
			if(!db.IsNull())
				SchumixBase.DManager.Update("admins", string.Format("Vhost = '{0}'", sUtilities.GetRandomString()), string.Format("Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername));
		}

		public void RandomAllVhost()
		{
			var db = SchumixBase.DManager.Query("SELECT Name FROM admins WHERE ServerName = '{0}'", servername);
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
					SchumixBase.DManager.Update("admins", string.Format("Vhost = '{0}'", sUtilities.GetRandomString()), string.Format("Name = '{0}' And ServerName = '{1}'", row["Name"].ToString(), servername));
			}
		}
	}
}