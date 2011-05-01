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
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc
{
	public sealed class ChannelInfo
	{
		private readonly List<string> ChannelFunkcio = new List<string>();
		private readonly Dictionary<string, string> _ChannelLista = new Dictionary<string, string>();
		private readonly Sender sSender = Singleton<Sender>.Instance;
		public Dictionary<string, string> CLista
		{
			get { return _ChannelLista; }
		}

		private ChannelInfo() {}

		public void ChannelLista()
		{
			var db = SchumixBase.DManager.Query("SELECT Channel, Password FROM channel");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string csatorna = row["Channel"].ToString();
					string jelszo = row["Password"].ToString();
					_ChannelLista.Add(csatorna, jelszo);
				}
			}
			else
				Log.Error("ChannelInfo", "ChannelLista: Hibas lekerdezes!");
		}

		public bool FSelect(string nev)
		{
			string status = string.Empty;

			var db = SchumixBase.DManager.QueryFirstRow("SELECT funkcio_status FROM schumix WHERE funkcio_nev = '{0}'", nev);
			if(!db.IsNull())
				status = db["funkcio_status"].ToString();
			else
				Log.Error("ChannelInfo", "FSelect: Hibas lekerdezes!");

			if(status == "be")
				return true;
			else
				return false;
		}

		public bool FSelect(string nev, string channel)
		{
			string status = string.Empty;

			foreach(var csatornak in ChannelFunkcio)
			{
				string[] pont = csatornak.Split('.');
				string csatorna = pont[0];
				string funkciok = pont[1];
				string[] kettospont = funkciok.Split(':');

				if(csatorna == channel.ToLower())
				{
					if(kettospont[0] == nev.ToLower())
					{
						status = kettospont[1];
						break;
					}
				}
			}

			if(status == "be")
				return true;
			else
				return false;
		}

		public void ChannelFunkcioReload()
		{
			ChannelFunkcio.Clear();

			var db = SchumixBase.DManager.Query("SELECT Channel FROM channel");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string csatorna = row["Channel"].ToString();

					var db1 = SchumixBase.DManager.QueryFirstRow("SELECT Functions FROM channel WHERE Channel = '{0}'", csatorna);
					if(!db1.IsNull())
					{
						string funkciok = db1["Functions"].ToString();
						string[] vesszo = funkciok.Split(',');

						for(int x = 1; x < vesszo.Length; x++)
						{
							string csatornaadat = csatorna + "." + vesszo[x];
							ChannelFunkcio.Add(csatornaadat);
						}
					}
					else
						Log.Error("ChannelInfo", "ChannelFunkcioReload: Hibas lekerdezes!");
				}
			}
			else
				Log.Error("ChannelInfo", "ChannelFunkcioReload: Hibas lekerdezes!");
		}

		public void ChannelListaReload()
		{
			_ChannelLista.Clear();
			var db = SchumixBase.DManager.Query("SELECT Channel, Password FROM channel");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string csatorna = row["Channel"].ToString();
					string jelszo = row["Password"].ToString();
					_ChannelLista.Add(csatorna, jelszo);
				}
			}
			else
				Log.Error("ChannelInfo", "ChannelListaReload: Hibas lekerdezes!");
		}

		public string ChannelFunkciok(string nev, string status, string channel)
		{
			string funkcio = string.Empty;

			foreach(var csatornak in ChannelFunkcio)
			{
				string[] pont = csatornak.Split('.');
				string csatorna = pont[0];
				string funkciok = pont[1];
				string[] kettospont = funkciok.Split(':');

				if(csatorna == channel.ToLower())
				{
					if(kettospont[0] != nev.ToLower())
						funkcio += "," + funkciok;
				}
			}

			foreach(var csatornak in ChannelFunkcio)
			{
				string[] pont = csatornak.Split('.');
				string csatorna = pont[0];
				string funkciok = pont[1];
				string[] kettospont = funkciok.Split(':');

				if(csatorna == channel.ToLower())
				{
					if(kettospont[0] == nev.ToLower())
						funkcio += "," + nev + ":" + status;
				}
			}

			return funkcio;
		}

		public string FunkciokInfo()
		{
			string be = string.Empty, ki = string.Empty;

			var db = SchumixBase.DManager.Query("SELECT funkcio_nev, funkcio_status FROM schumix");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
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
			string be = string.Empty, ki = string.Empty;

			foreach(var csatornak in ChannelFunkcio)
			{
				string[] pont = csatornak.Split('.');
				string csatorna = pont[0];
				string funkciok = pont[1];
				string[] kettospont = funkciok.Split(':');

				if(csatorna == channel.ToLower())
				{
					if(kettospont[1] == "be")
						be += kettospont[0] + " ";
					else
						ki += kettospont[0] + " ";
				}
			}

			return be + "|" + ki;
		}

		public void JoinChannel()
		{
			Log.Debug("ChannelInfo", "Kapcsolodas a csatornakhoz...");
			bool error = false;

			foreach(var channel in _ChannelLista)
			{
				sSender.Join(channel.Key, channel.Value);
				SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'true', Error = '' WHERE Channel = '{0}'", channel.Key);
			}

			ChannelFunkcioReload();
			var db = SchumixBase.DManager.Query("SELECT Enabled FROM channel");
			if(!db.IsNull())
			{
				foreach(DataRow row in db.Rows)
				{
					string aktivitas = row["Enabled"].ToString();

					if(aktivitas == "false")
						error = true;
				}
			}
			else
				Log.Error("ChannelInfo", "JoinChannel: Hibas lekerdezes!");

			if(!error)
				Log.Success("ChannelInfo", "Sikeres kapcsolodas a csatornakhoz.");
			else
				Log.Warning("ChannelInfo", "Nehany kapcsolodas sikertelen!");

			if(SchumixBase.IIdo)
			{
				SchumixBase.timer.IndulasiIdo();
				SchumixBase.IIdo = false;
			}
		}
	}
}
