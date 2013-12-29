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
using System.Data;
using Schumix.Framework;
using Schumix.Framework.Util;
using Schumix.Framework.Logger;
using Schumix.Framework.Extensions;
using Schumix.Framework.Localization;

namespace Schumix.Irc.Commands
{	
	public abstract class CommandInfo : IDisposable
	{
		private readonly LocalizationConsole sLConsole = Singleton<LocalizationConsole>.Instance;
		private readonly Utilities sUtilities = Singleton<Utilities>.Instance;
		public string servername;

		protected CommandInfo(string ServerName)
		{
			servername = ServerName;
			Log.Debug("CommandInfo", sLConsole.GetString("Successfully started the CommandInfo."));
		}

		~CommandInfo()
		{
			Log.Debug("CommandInfo", "~CommandInfo() {0}", sLConsole.GetString("[ServerName: {0}]", servername));
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// none
		}

		public bool IsAdmin(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername);
			return !db.IsNull();
		}

		public bool IsAdmin(string Name, AdminFlag Flag)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername);
			if(!db.IsNull())
			{
				int flag = db["Flag"].ToInt32();
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

				int flag = db["Flag"].ToInt32();

				if((flag == 1 && Flag == AdminFlag.HalfOperator) ||
					(flag == 2 && Flag == AdminFlag.HalfOperator) ||
					(flag == 2 && Flag == AdminFlag.Operator))
					return true;

				return Flag == (AdminFlag)flag;
			}

			return false;
		}

		public AdminFlag Adminflag(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Flag FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername);
			return !db.IsNull() ? (AdminFlag)(db["Flag"].ToInt32()) : AdminFlag.None;
		}

		public AdminFlag Adminflag(string Name, string Vhost)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT Vhost, Flag FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername);
			if(!db.IsNull())
			{
				string vhost = db["Vhost"].ToString();
				return Vhost != vhost ? AdminFlag.None : (AdminFlag)(db["Flag"].ToInt32());
			}
			else
				return AdminFlag.None;
		}

		public void RandomVhost(string Name)
		{
			var db = SchumixBase.DManager.QueryFirstRow("SELECT 1 FROM admins WHERE Name = '{0}' And ServerName = '{1}'", sUtilities.SqlEscape(Name.ToLower()), servername);
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