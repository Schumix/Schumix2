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

namespace Schumix.IRC.Commands
{
	public struct CommandMessage
	{
		public string Channel { get; set; }
		public string Args { get; set; }
		public string Nick { get; set; }
		public string User { get; set; }
		public string Host { get; set; }
		public string[] Info { get; set; }
	}

	public class CommandManager
	{
		private Dictionary<string, Action> _CommandHandler = new Dictionary<string, Action>();
		private CommandHandler sCommandHandler = Singleton<CommandHandler>.Instance;

		public CommandManager()
		{
			Log.Notice("CommandManager", "CommandManager elindult.");
			InitHandler();
		}

		private void InitHandler()
		{
			// Public
			_CommandHandler.Add("xbot", sCommandHandler.HandleXbot);
			_CommandHandler.Add("info", sCommandHandler.HandleInfo);
			_CommandHandler.Add("help", sCommandHandler.HandleHelp);
			_CommandHandler.Add("ido", sCommandHandler.HandleIdo);
			_CommandHandler.Add("datum", sCommandHandler.HandleDatum);
			_CommandHandler.Add("roll", sCommandHandler.HandleRoll);
			_CommandHandler.Add("calc", sCommandHandler.HandleCalc);

			// Admin
			_CommandHandler.Add("hozzaferes", sCommandHandler.HandleHozzaferes);
			_CommandHandler.Add("ujjelszo",   sCommandHandler.HandleUjjelszo);
			_CommandHandler.Add("szoba",      sCommandHandler.HandleSzoba);
			_CommandHandler.Add("kikapcs",    sCommandHandler.HandleKikapcs);

			Log.Notice("CommandManager", "Osszes Command handler regisztralva.");
		}

		private void RegisterHandler(string code, Action method)
		{
			_CommandHandler.Add(code, method);
		}

		public void BejovoInfo(string handler)
		{
			try
			{
				if(_CommandHandler.ContainsKey(handler))
					_CommandHandler[handler].Invoke();
			}
			catch(Exception e)
			{
				Log.Error("BejovoInfo", String.Format("Hiba oka: {0}", e.ToString()));
			}
		}

        /// <summary>
        ///     Meghatározza, hogy a nick admin-e vagy sem, nick alapján.
        /// </summary>
		public bool Admin(string nick)
		{
			string admin = "";
			string _nick = nick.ToLower();

			var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT nev FROM adminok WHERE nev = '{0}'", _nick));
			if(db != null)
				admin = db["nev"].ToString();

			if(_nick != admin)
				return false;

			return true;
		}

        /// <summary>
        ///     Meghatározza, hogy a nick admin-e vagy sem, nick és host alapján.
        /// </summary>
        /// <param name="host">A nick IP címe.</param>
		public bool Admin(string nick, string host)
		{
			string admin = "";
			string ip = "";
			string _nick = nick.ToLower();

			var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT nev, ip FROM adminok WHERE nev = '{0}'", _nick));
			if(db != null)
			{
				admin = db["nev"].ToString();
				ip = db["ip"].ToString();
			}

			if(_nick != admin)
				return false;

			if(host != ip)
				return false;

			return true;
		}
	}
}