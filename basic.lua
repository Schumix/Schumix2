function LuaTest(IRCMessage)
	if IRCMessage.Info.Length < 5 then
		SendMsg(IRCMessage.ServerName, IRCMessage.Channel, "The parameters are not specified!")
		return
	end

	if IRCMessage.Info[4] == "length" then
		SendMsg(IRCMessage.ServerName, IRCMessage.Channel, string.format("Length: %s", IRCMessage.Info.Length))
	else
		SendMsg(IRCMessage.ServerName, IRCMessage.Channel, "Unkown command!")
	end
end

RegisterSchumixHook("luatest", "LuaTest", GetCommandPermission("Normal"))
