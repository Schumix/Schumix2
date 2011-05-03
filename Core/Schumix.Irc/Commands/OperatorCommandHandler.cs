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
using Schumix.API;
using Schumix.Framework;
using Schumix.Framework.Config;
using Schumix.Framework.Extensions;

namespace Schumix.Irc.Commands
{
	public partial class CommandHandler
	{
		protected void HandleFunkcio(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs paraméter!");
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "info")
			{
				string[] ChannelInfo = sChannelInfo.ChannelFunkciokInfo(sIRCMessage.Channel).Split('|');
				if(ChannelInfo.Length < 2)
					return;

				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "2Bekapcsolva: {0}", ChannelInfo[0]);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "2Kikapcsolva: {0}", ChannelInfo[1]);
			}
			else if(sIRCMessage.Info[4].ToLower() == "all")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva egy paraméter!");
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "info")
				{
					string f = sChannelInfo.FunkciokInfo();
					if(f == "Hibás lekérdezés!")
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
						return;
					}

					string[] FunkcioInfo = f.Split('|');
					if(FunkcioInfo.Length < 2)
						return;

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "2Bekapcsolva: {0}", FunkcioInfo[0]);
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "2Kikapcsolva: {0}", FunkcioInfo[1]);
				}
				else
				{
					if(sIRCMessage.Info.Length < 7)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a funkció neve!");
						return;
					}

					if(sIRCMessage.Info[5].ToLower() == "be" || sIRCMessage.Info[5].ToLower() == "ki")
					{
						if(sIRCMessage.Info.Length >= 8)
						{
							string alomany = string.Empty;

							for(int i = 6; i < sIRCMessage.Info.Length; i++)
							{
								alomany += ", " + sIRCMessage.Info[i].ToLower();
								SchumixBase.DManager.QueryFirstRow("UPDATE schumix SET funkcio_status = '{0}' WHERE funkcio_nev = '{1}'", sIRCMessage.Info[5].ToLower(), sIRCMessage.Info[i].ToLower());
							}

							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: {1}kapcsolva",  alomany.Remove(0, 2, ", "), sIRCMessage.Info[5].ToLower());
						}
						else
						{
							sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: {1}kapcsolva", sIRCMessage.Info[6].ToLower(), sIRCMessage.Info[5].ToLower());
							SchumixBase.DManager.QueryFirstRow("UPDATE schumix SET funkcio_status = '{0}' WHERE funkcio_nev = '{1}'", sIRCMessage.Info[5].ToLower(), sIRCMessage.Info[6].ToLower());
						}
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "channel")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a csatorna neve!");
					return;
				}

				if(sIRCMessage.Info.Length < 7)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva egy paraméter!");
					return;
				}
			
				string channelinfo = sIRCMessage.Info[5].ToLower();
				string status = sIRCMessage.Info[6].ToLower();
			
				if(sIRCMessage.Info[6].ToLower() == "info")
				{
					string[] ChannelInfo = sChannelInfo.ChannelFunkciokInfo(channelinfo).Split('|');
					if(ChannelInfo.Length < 2)
						return;

					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "2Bekapcsolva: {0}", ChannelInfo[0]);
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "2Kikapcsolva: {0}", ChannelInfo[1]);
				}
				else if(status == "be" || status == "ki")
				{
					if(sIRCMessage.Info.Length < 8)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a funkció neve!");
						return;
					}

					if(sIRCMessage.Info.Length >= 9)
					{
						string alomany = string.Empty;

						for(int i = 7; i < sIRCMessage.Info.Length; i++)
						{
							alomany += ", " + sIRCMessage.Info[i].ToLower();
							SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunkciok(sIRCMessage.Info[i].ToLower(), status, channelinfo), channelinfo);
							sChannelInfo.ChannelFunkcioReload();
						}

						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: {1}kapcsolva",  alomany.Remove(0, 2, ", "), status);
					}
					else
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: {1}kapcsolva", sIRCMessage.Info[7].ToLower(), status);
						SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunkciok(sIRCMessage.Info[7].ToLower(), status, channelinfo), channelinfo);
						sChannelInfo.ChannelFunkcioReload();
					}
				}
			}
			else if(sIRCMessage.Info[4].ToLower() == "update")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Sikeresen frissitve {0} csatornán a funkciók.", sIRCMessage.Channel);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = ',koszones:ki,log:be,rejoin:be,parancsok:be,autohl:ki,autokick:ki,automode:ki,antiflood:ki,uzenet:ki' WHERE Channel = '{0}'", sIRCMessage.Channel);
					sChannelInfo.ChannelFunkcioReload();
					return;
				}

				if(sIRCMessage.Info[5].ToLower() == "all")
				{
					var db = SchumixBase.DManager.Query("SELECT Channel FROM channel");
					if(!db.IsNull())
					{
						foreach(DataRow row in db.Rows)
						{
							string csatorna = row["Channel"].ToString();
							SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = ',koszones:ki,log:be,rejoin:be,parancsok:be,autohl:ki,autokick:ki,automode:ki,antiflood:ki,uzenet:ki' WHERE Channel = '{0}'", csatorna);
						}

						sChannelInfo.ChannelFunkcioReload();
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Sikeresen frissitve minden csatornán a funkciók.");
					}
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
				}
				else
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Sikeresen frissitve {0} csatornán a funkciók.", sIRCMessage.Info[5].ToLower());
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = ',koszones:ki,log:be,rejoin:be,parancsok:be,autohl:ki,autokick:ki,automode:ki,antiflood:ki,uzenet:ki' WHERE Channel = '{0}'", sIRCMessage.Info[5].ToLower());
					sChannelInfo.ChannelFunkcioReload();
				}
			}
			else
			{
				if(sIRCMessage.Info.Length < 5)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a funkció állapota!");
					return;
				}

				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a funkció neve!");
					return;
				}

				string status = sIRCMessage.Info[4].ToLower();

				if(status == "be" || status == "ki")
				{
					if(sIRCMessage.Info.Length >= 7)
					{
						string alomany = string.Empty;

						for(int i = 5; i < sIRCMessage.Info.Length; i++)
						{
							alomany += ", " + sIRCMessage.Info[i].ToLower();
							SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunkciok(sIRCMessage.Info[i].ToLower(), status, sIRCMessage.Channel), sIRCMessage.Channel);
							sChannelInfo.ChannelFunkcioReload();
						}

						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: {1}kapcsolva",  alomany.Remove(0, 2, ", "), status);
					}
					else
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0}: {1}kapcsolva", sIRCMessage.Info[5].ToLower(), status);
						SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Functions = '{0}' WHERE Channel = '{1}'", sChannelInfo.ChannelFunkciok(sIRCMessage.Info[5].ToLower(), status, sIRCMessage.Channel), sIRCMessage.Channel);
						sChannelInfo.ChannelFunkcioReload();
					}
				}
			}
		}

		protected void HandleChannel(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Parancsok: add | del | info | update");
				return;
			}

			if(sIRCMessage.Info[4].ToLower() == "add")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a csatorna neve!");
					return;
				}

				string csatornainfo = sIRCMessage.Info[5].ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", csatornainfo);
				if(!db.IsNull())
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A név már szerepel a csatorna listán!");
					return;
				}

				if(sIRCMessage.Info.Length == 7)
				{
					ChannelPrivmsg = sIRCMessage.Channel;
					string jelszo = sIRCMessage.Info[6];
					sSender.Join(csatornainfo, jelszo);
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `channel`(Channel, Password) VALUES ('{0}', '{1}')", csatornainfo, jelszo);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'true' WHERE Channel = '{0}'", csatornainfo);
				}
				else
				{
					ChannelPrivmsg = sIRCMessage.Channel;
					sSender.Join(csatornainfo);
					SchumixBase.DManager.QueryFirstRow("INSERT INTO `channel`(Channel, Password) VALUES ('{0}', '')", csatornainfo);
					SchumixBase.DManager.QueryFirstRow("UPDATE channel SET Enabled = 'true' WHERE Channel = '{0}'", csatornainfo);
				}

				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Csatorna hozzáadva: {0}", csatornainfo);

				sChannelInfo.ChannelListaReload();
				sChannelInfo.ChannelFunkcioReload();
			}
			else if(sIRCMessage.Info[4].ToLower() == "del")
			{
				if(sIRCMessage.Info.Length < 6)
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs megadva a csatorna neve!");
					return;
				}

				string csatornainfo = sIRCMessage.Info[5].ToLower();
				var db = SchumixBase.DManager.QueryFirstRow("SELECT Id FROM channel WHERE Channel = '{0}'", csatornainfo);
				if(!db.IsNull())
				{
					int id = Convert.ToInt32(db["Id"].ToString());
					if(id == 1)
					{
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A mester csatorna nem törölhető!");
						return;
					}
				}

				db = SchumixBase.DManager.QueryFirstRow("SELECT* FROM channel WHERE Channel = '{0}'", csatornainfo);
				if(db.IsNull())
				{
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Ilyen csatorna nem létezik!");
					return;
				}

				sSender.Part(csatornainfo);
				SchumixBase.DManager.QueryFirstRow("DELETE FROM `channel` WHERE Channel = '{0}'", csatornainfo);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Csatorna eltávolítva: {0}", csatornainfo);

				sChannelInfo.ChannelListaReload();
				sChannelInfo.ChannelFunkcioReload();
			}
			else if(sIRCMessage.Info[4].ToLower() == "update")
			{
				sChannelInfo.ChannelListaReload();
				sChannelInfo.ChannelFunkcioReload();
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "A csatorna információk frissitésre kerültek.");
			}
			else if(sIRCMessage.Info[4].ToLower() == "info")
			{
				var db = SchumixBase.DManager.Query("SELECT Channel, Enabled, Error FROM channel");
				if(!db.IsNull())
				{
					string AktivCsatornak = string.Empty, InAktivCsatornak = string.Empty;

					foreach(DataRow row in db.Rows)
					{
						string csatorna = row["Channel"].ToString();
						string aktivitas = row["Enabled"].ToString();

						if(aktivitas == "true")
							AktivCsatornak += ", " + csatorna;
						else if(aktivitas == "false")
							InAktivCsatornak += ", " + csatorna + ":" + row["Error"].ToString();
					}

					if(AktivCsatornak.Length > 0)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Aktiv: {0}", AktivCsatornak.Remove(0, 2, ", "));
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Aktiv: Nincs adat.");

					if(InAktivCsatornak.Length > 0)
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Inaktiv: {0}", InAktivCsatornak.Remove(0, 2, ", "));
					else
						sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "3Inaktiv: Nincs adat.");
				}
				else
					sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Hibás lekérdezés!");
			}
		}

		protected void HandleSznap(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs név megadva!");
				return;
			}

			var db = SchumixBase.DManager.QueryFirstRow("SELECT nev, honap, nap FROM sznap WHERE nev = '{0}'", sIRCMessage.Info[4]);
			if(!db.IsNull())
			{
				string nev = db["nev"].ToString();
				string honap = db["honap"].ToString();
				int nap = Convert.ToInt32(db["nap"]);
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "{0} születés napja: {1} {2}", nev, honap, nap);
			}
			else
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs ilyen ember!");
		}

		protected void HandleKick(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs név megadva!");
				return;
			}

			string kick = sIRCMessage.Info[4].ToLower();
			int szam = sIRCMessage.Info.Length;
			string nick = sNickInfo.NickStorage;

			if(szam == 5)
			{
				if(kick != nick.ToLower())
					sSender.Kick(sIRCMessage.Channel, kick);
			}
			else if(szam >= 6)
			{
				if(kick != nick.ToLower())
					sSender.Kick(sIRCMessage.Channel, kick, sIRCMessage.Info.SplitToString(5, " "));
			}
		}

		protected void HandleMode(IRCMessage sIRCMessage)
		{
			if(!IsAdmin(sIRCMessage.Nick, sIRCMessage.Host, AdminFlag.Operator))
				return;

			CNick(sIRCMessage);

			if(sIRCMessage.Info.Length < 5)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs a rang megadva!");
				return;
			}

			if(sIRCMessage.Info.Length < 6)
			{
				sSendMessage.SendCMPrivmsg(sIRCMessage.Channel, "Nincs név megadva!");
				return;
			}

			string rang = sIRCMessage.Info[4].ToLower();
			string nev = sIRCMessage.Info.SplitToString(5, " ").ToLower();

			if(!nev.Contains(sNickInfo.NickStorage.ToLower()))
				sSender.Mode(sIRCMessage.Channel, rang, nev);
		}
	}
}
