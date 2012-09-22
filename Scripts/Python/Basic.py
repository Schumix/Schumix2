import sys
from Schumix.API.Irc import IRCMessage
from Schumix.Irc import IrcBase

sIrcBase = IrcBase

def Setup(IrcBase):
	global sIrcBase
	sIrcBase = IrcBase
	IrcBase.SchumixRegisterHandler("testpython", Test)
	return

def Destroy(IrcBase):
	IrcBase.SchumixRemoveHandler("testpython", Test)
	return

def Test(IRCMessage):
	sSendMessage = sIrcBase.Networks[IRCMessage.ServerName].sSendMessage;
	sSendMessage.SendCMPrivmsg(IRCMessage.Channel, "Teszt!")
	return

