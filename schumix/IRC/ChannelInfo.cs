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
using System.Collections.Generic;
using Schumix.Config;

namespace Schumix.IRC
{
	public class ChannelInfo
	{
		private ChannelInfo() {}
		private List<string> m_ChannelFunkcio = new List<string>();
		public readonly Dictionary<string, string> _ChannelLista = new Dictionary<string, string>();

		public void ChannelLista()
		{
			var db = SchumixBot.mSQLConn.QueryRow("SELECT szoba, jelszo FROM channel");
			if(db != null)
			{
				for(int i = 0; i < db.Rows.Count; ++i)
				{
					var row = db.Rows[i];
					string channel = row["szoba"].ToString();
					string jelszo = row["jelszo"].ToString();
					_ChannelLista.Add(channel, jelszo);
				}
			}
			else
				Log.Error("ChannelInfo", "Hibas lekerdezes!");
		}

		public string FSelect(string nev)
		{
			string status = "";

			var db = SchumixBot.mSQLConn.QueryFirstRow("SELECT funkcio_status FROM schumix WHERE funkcio_nev = '{0}'", nev);
			if(db != null)
				status = db["funkcio_status"].ToString();

			return status;
		}

		public string FSelect(string nev, string channel)
		{
			string status = "";

			for(int i = 0; i < m_ChannelFunkcio.Count; i++)
			{
				string szobak = m_ChannelFunkcio[i];
				string[] pont = szobak.Split('.');
				string szoba = pont[0];
				string funkciok = pont[1];
				string[] kettospont = funkciok.Split(':');

				if(szoba == channel)
				{
					if(kettospont[0] == nev)
						status = kettospont[1];
				}
			}

			return status;
		}

		public void ChannelFunkcioReload()
		{
			m_ChannelFunkcio.Clear();

			var db = SchumixBot.mSQLConn.QueryRow("SELECT szoba FROM channel");
			if(db != null)
			{
				for(int i = 0; i < db.Rows.Count; ++i)
				{
					var row = db.Rows[i];
					string szoba = row["szoba"].ToString();

					var db1 = SchumixBot.mSQLConn.QueryFirstRow("SELECT funkciok FROM channel WHERE szoba = '{0}'", szoba);
					if(db1 != null)
					{
						string funkciok = db1["funkciok"].ToString();
						string[] vesszo = funkciok.Split(',');

						for(int x = 1; x < vesszo.Length; x++)
						{
							string szobaadat = szoba + "." + vesszo[x];
							m_ChannelFunkcio.Add(szobaadat);
						}
					}
				}
			}
		}

		public void ChannelListaReload()
		{
			_ChannelLista.Clear();
			var db = SchumixBot.mSQLConn.QueryRow("SELECT szoba, jelszo FROM channel");
			if(db != null)
			{
				for(int i = 0; i < db.Rows.Count; ++i)
				{
					var row = db.Rows[i];
					string szoba = row["szoba"].ToString();
					string jelszo = row["jelszo"].ToString();
					_ChannelLista.Add(szoba, jelszo);
				}
			}
		}

		public string ChannelFunkciok(string nev, string status, string channel)
		{
			string funkcio = "";

			for(int i = 0; i < m_ChannelFunkcio.Count; i++)
			{
				string szobak = m_ChannelFunkcio[i];
				string[] pont = szobak.Split('.');
				string szoba = pont[0];
				string funkciok = pont[1];
				string[] kettospont = funkciok.Split(':');

				if(szoba == channel)
				{
					if(kettospont[0] != nev)
						funkcio += "," + funkciok;
				}
			}

			for(int i = 0; i < m_ChannelFunkcio.Count; i++)
			{
				string szobak = m_ChannelFunkcio[i];
				string[] pont = szobak.Split('.');
				string szoba = pont[0];
				string funkciok = pont[1];
				string[] kettospont = funkciok.Split(':');

				if(szoba == channel)
				{
					if(kettospont[0] == nev)
						funkcio += "," + nev + ":" + status;
				}
			}

			return funkcio;
		}

		public string FunkciokInfo()
		{
			string be = "", ki = "";

			var db = SchumixBot.mSQLConn.QueryRow("SELECT funkcio_nev, funkcio_status FROM schumix");
			if(db != null)
			{
				for(int i = 0; i < db.Rows.Count; ++i)
				{
					var row = db.Rows[i];
					string nev = row["funkcio_nev"].ToString();
					string status = row["funkcio_status"].ToString();
	
					if(status == "be")
						be += nev + " ";
					else
						ki += nev + " ";
				}
			}
			else
				return "Hibás lekérdezés!";

			return be + "|" + ki;
		}

		public string ChannelFunkciokInfo(string channel)
		{
			string be = "", ki = "";

			for(int i = 0; i < m_ChannelFunkcio.Count; i++)
			{
				string szobak = m_ChannelFunkcio[i];
				string[] pont = szobak.Split('.');
				string szoba = pont[0];
				string funkciok = pont[1];
				string[] kettospont = funkciok.Split(':');

				if(szoba == channel)
				{
					if(kettospont[1] == "be")
						be += kettospont[0] + " ";
					else
						ki += kettospont[0] + " ";
				}
			}

			return be + "|" + ki;
		}
	}
}
