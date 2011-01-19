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
	public enum AdminFlag
	{
		Operator      = 0,
		Administrator = 1
	};

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
			RegisterHandler("xbot",   sCommandHandler.HandleXbot);
			RegisterHandler("info",   sCommandHandler.HandleInfo);
			RegisterHandler("help",   sCommandHandler.HandleHelp);
			RegisterHandler("ido",    sCommandHandler.HandleIdo);
			RegisterHandler("datum",  sCommandHandler.HandleDatum);
			RegisterHandler("roll",   sCommandHandler.HandleRoll);
			RegisterHandler("calc",   sCommandHandler.HandleCalc);
			RegisterHandler("sha1",   sCommandHandler.HandleSha1);
			RegisterHandler("md5",    sCommandHandler.HandleMd5);
			RegisterHandler("irc",    sCommandHandler.HandleIrc);
			RegisterHandler("whois",  sCommandHandler.HandleWhois);
			RegisterHandler("uzenet", sCommandHandler.HandleUzenet);
			RegisterHandler("keres",  sCommandHandler.HandleKeres);
			RegisterHandler("prime",  sCommandHandler.HandlePrime);

			// Operator
			RegisterHandler("admin",      sCommandHandler.HandleAdmin);
			RegisterHandler("funkcio",    sCommandHandler.HandleFunkcio);
			RegisterHandler("channel",    sCommandHandler.HandleChannel);
			RegisterHandler("sznap",      sCommandHandler.HandleSznap);
			RegisterHandler("szinek",     sCommandHandler.HandleSzinek);
			RegisterHandler("nick",       sCommandHandler.HandleNick);
			RegisterHandler("join",       sCommandHandler.HandleJoin);
			RegisterHandler("left",       sCommandHandler.HandleLeft);
			RegisterHandler("kick",       sCommandHandler.HandleKick);
			RegisterHandler("mode",       sCommandHandler.HandleMode);

			// Admin
			RegisterHandler("teszt",      sCommandHandler.HandleTeszt);
			RegisterHandler("kikapcs",    sCommandHandler.HandleKikapcs);

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

		public bool Admin(string Nick)
		{
			var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT * FROM adminok WHERE nev = '{0}'", Nick.ToLower()));
			if(db != null)
				return true;

			return false;
		}

		public bool Admin(string Nick, AdminFlag Flag)
		{
			var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT flag FROM adminok WHERE nev = '{0}'", Nick.ToLower()));
			if(db != null)
			{
				int flag = Convert.ToInt32(db["flag"]);

				if(Flag != (AdminFlag)flag)
					return false;

				return true;
			}

			return false;
		}

		public bool Admin(string Nick, string Vhost, AdminFlag Flag)
		{
			var db = SchumixBot.mSQLConn.QueryFirstRow(String.Format("SELECT vhost, flag FROM adminok WHERE nev = '{0}'", Nick.ToLower()));
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
	}
}