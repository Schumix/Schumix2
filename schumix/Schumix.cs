/*
 * This file is part of Schumix.
 * 
 * Copyright (C) 2010 Megax <http://www.megaxx.info/>
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
using System.Xml;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;

namespace Schumix
{
	class SchumixBot
	{
        /// <summary>
        ///     A Schumix bot revision száma.
        /// </summary>
		public static string revision = "0.1.3";

        /// <summary>
        ///     A server amire csatlakozik a bot.
        /// </summary>
		private string Server;

        /// <summary>
        ///     A port a szerverhez.
        /// </summary>
		private int Port;

        /// <summary>
        ///     Az éppen használt nick.
        /// </summary>
		public static string NickTarolo;

        /// <summary>
        ///     Az elsődleges nick.
        /// </summary>
		public static string Nick;

        /// <summary>
        ///     A másodlagos nick.
        /// </summary>
		public static string Nick2;

        /// <summary>
        ///     A harmadlagos nick,
        /// </summary>
		public static string Nick3;

        /// <summary>
        ///     Ha "0" akkor nem aktiválja a nickjét (identify),
        ///     ha "1" akkor igen.
        /// </summary>
		public static int Aktivitas;

        /// <summary>
        ///     Nickname aktiváló jelszó.
        /// </summary>
		public static string Jelszo;

        /// <summary>
        ///     Az átlagosan használt parancsok előjele. Konfigból töltödik be.
        /// </summary>
		public static string _parancselojel;

        /// <summary>
        ///     A bot elindításának ideje.
        /// </summary>
		public static DateTime StartTime;

        /// <summary>
        ///     Ebbe a változóba töltödik bele a szoba neve és a jelszava, ha van.
        /// </summary>
		public static Dictionary<string,string> m_ChannelLista = new Dictionary<string,string>();


        ///***START***********************************///
        /// <summary>
        ///     A MySQL class-t hívja meg.
        ///     Dekralálja a MySQL kapcsolódást.
        /// </summary>
		public static Mysql mSQLConn;

        /// <summary>
        ///     A MySQL hostja.
        /// </summary>
		public static string Host;

        /// <summary>
        ///     A MySQL felhasználóneve.
        /// </summary>
		public static string User;

        /// <summary>
        ///     A MySQL jelszava.
        /// </summary>
		public static string Password;

        /// <summary>
        ///     A MySQL adatbázis neve.
        /// </summary>
		public static string Database;
        ///***END***********************************///

        ///<summary>
        ///     Meghívja a Schumix függvényt.
        ///</summary>
		public SchumixBot()
		{
			Schumix();
		}

        /// <summary>
        ///     Ha lefut, akkor leáll a class.
        /// </summary>
		~SchumixBot()
		{
			Log.Debug("SchumixBot", "~SchumixBot()");
		}

        /// <summary>
        ///     Innentől kezd el futni a Schumix. Fő függvény.
        /// </summary>
		private void Schumix()
		{
			try
			{
				ConfigFajl();
				Log.Debug("Schumix", "Consol thread indul...");
				new Consol();
				NickTarolo = Nick;
				StartTime = DateTime.Now;

				new Network(Server, Port);
			}
			catch(Exception e)
			{
				Log.Error("Schumix", String.Format("Hiba oka: {0}", e.ToString()));
				Schumix();
				Thread.Sleep(50);
			}
		}

        /// <summary>
        ///     Ez a függvény olvassa be a schumix.xml config file-t.
        ///     Ebben az XML-ben vannak a fontos adatok.
        /// </summary>
		private void ConfigFajl()
		{
			Log.Debug("Config", ">> schumix.xml");
			var xmldoc = new XmlDocument();
			xmldoc.Load(@"schumix.xml");
			Log.Notice("Config", "Config fajl betoltese...");

			Server = xmldoc.SelectSingleNode("Schumix/Irc/Server").InnerText;
			Port = Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Port").InnerText);
			Nick = xmldoc.SelectSingleNode("Schumix/Irc/Nick").InnerText;
			Nick2 = xmldoc.SelectSingleNode("Schumix/Irc/Nick2").InnerText;
			Nick3 = xmldoc.SelectSingleNode("Schumix/Irc/Nick3").InnerText;
			Aktivitas = Convert.ToInt32(xmldoc.SelectSingleNode("Schumix/Irc/Allapot/Aktivitas").InnerText);
			Jelszo = xmldoc.SelectSingleNode("Schumix/Irc/Allapot/Jelszo").InnerText;

			Host = xmldoc.SelectSingleNode("Schumix/Mysql/Host").InnerText;
			User = xmldoc.SelectSingleNode("Schumix/Mysql/User").InnerText;
			Password = xmldoc.SelectSingleNode("Schumix/Mysql/Password").InnerText;
			Database = xmldoc.SelectSingleNode("Schumix/Mysql/Database").InnerText;

			mSQLConn = new Mysql(Host, User, Password, Database);
			Log.Notice("Config", "Mysql adatbazishoz sikeres a kapcsolodas.");
	
			_parancselojel = xmldoc.SelectSingleNode("Schumix/Parancs/Elojel").InnerText;

			var db = mSQLConn.QueryRow(String.Format("SELECT szoba, jelszo FROM channel"));
			for(int i = 0; i < db.Rows.Count; ++i)
			{
				var row = db.Rows[i];
				string channel = row["szoba"].ToString();
				string jelszo = row["jelszo"].ToString();

				m_ChannelLista.Add(channel, jelszo);
			}

			Log.Success("Config", "Config adatbazis betoltve.");
			Console.WriteLine("");
		}

        /// <returns>
        ///     Megmutatja mennyi ideje üzemel a program.
        /// </returns>
		public static string Uptime()
		{
			var Time = DateTime.Now - StartTime;
			return String.Format("{0} nap, {1} óra, {2} perc, {3} másodperc.", Time.Days, Time.Hours, Time.Minutes, Time.Seconds);
		}
	}
}
