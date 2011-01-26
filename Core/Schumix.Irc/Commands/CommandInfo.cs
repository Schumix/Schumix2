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
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Schumix.Framework;

namespace Schumix.Irc.Commands
{	
	public enum AdminFlag
	{
		Operator      = 0,
		Administrator = 1
	};

	public class CommandInfo
	{
		protected CommandInfo()
		{
			Log.Notice("CommandInfo", "CommandInfo elindult.");
		}

		protected bool Admin(string Nick)
		{
			var db = SchumixBase.mSQLConn.QueryFirstRow("SELECT * FROM adminok WHERE nev = '{0}'", Nick.ToLower());
			if(db != null)
				return true;

			return false;
		}

		protected bool Admin(string Nick, AdminFlag Flag)
		{
			var db = SchumixBase.mSQLConn.QueryFirstRow("SELECT flag FROM adminok WHERE nev = '{0}'", Nick.ToLower());
			if(db != null)
			{
				int flag = Convert.ToInt32(db["flag"]);

				if(Flag != (AdminFlag)flag)
					return false;

				return true;
			}

			return false;
		}

		protected bool Admin(string Nick, string Vhost, AdminFlag Flag)
		{
			var db = SchumixBase.mSQLConn.QueryFirstRow("SELECT vhost, flag FROM adminok WHERE nev = '{0}'", Nick.ToLower());
			if(db != null)
			{
				string vhost = db["vhost"].ToString();

				if(Vhost != vhost)
					return false;

				int flag = Convert.ToInt32(db["flag"]);

				if(flag == 1 && Flag == 0)
					return true;

				if(Flag != (AdminFlag)flag)
					return false;

				return true;
			}

			return false;
		}

		protected void CNick()
		{
			bool channel = Network.IMessage.Channel.StartsWith("#");
			if(!channel)
				Network.IMessage.Channel = Network.IMessage.Nick;
		}
	}
}
