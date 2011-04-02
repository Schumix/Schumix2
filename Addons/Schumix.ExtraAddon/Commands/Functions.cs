using System;
using Schumix.Irc;
using Schumix.Irc.Commands;
using Schumix.Framework;

namespace Schumix.ExtraAddon.Commands
{
	public partial class Functions : CommandInfo
	{
		public void HLUzenet()
		{
			if(Network.sChannelInfo.FSelect("hl") && Network.sChannelInfo.FSelect("hl", Network.IMessage.Channel))
			{
				for(int i = 3; i < Network.IMessage.Info.Length; i++)
				{
					if(i == 3)
					{
						if(Network.IMessage.Info[3].Substring(0, 1) == ":")
							Network.IMessage.Info[3] = Network.IMessage.Info[3].Remove(0, 1);
					}

					var db = SchumixBase.DManager.QueryFirstRow("SELECT Info, Enabled FROM hlmessage WHERE Name = '{0}'", Network.IMessage.Info[i].ToLower());
					if(db != null)
					{
						string info = db["Info"].ToString();
						string allapot = db["Enabled"].ToString();

						if(allapot != "be")
							return;

						sSendMessage.SendCMPrivmsg(Network.IMessage.Channel, "{0}", info);
						break;
					}
				}
			}
		}

		public bool AutoKick(string allapot)
		{
			if(allapot == "join")
			{
				string channel = Network.IMessage.Channel;

				if(channel.Substring(0, 1) == ":")
					channel = channel.Remove(0, 1);

				if(Network.sChannelInfo.FSelect("kick") && Network.sChannelInfo.FSelect("kick", channel))
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Reason FROM kicklist WHERE Name = '{0}'", Network.IMessage.Nick.ToLower());
					if(db != null)
					{
						string oka = db["Reason"].ToString();
						sSender.Kick(channel, Network.IMessage.Nick, oka);
						return true;
					}
				}

				return false;
			}

			if(allapot == "privmsg")
			{
				if(Network.sChannelInfo.FSelect("kick") && Network.sChannelInfo.FSelect("kick", Network.IMessage.Channel))
				{
					var db = SchumixBase.DManager.QueryFirstRow("SELECT Reason FROM kicklist WHERE Name = '{0}'", Network.IMessage.Nick.ToLower());
					if(db != null)
					{
						string oka = db["Reason"].ToString();
						sSender.Kick(Network.IMessage.Channel, Network.IMessage.Nick, oka);
						return true;
					}
				}

				return false;
			}

			return false;
		}
	}
}
