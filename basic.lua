function PublicTest(IRCMessage)
	if IRCMessage.Info.Length < 5 then
		SendMsg(IRCMessage.Channel, "The parameters are not specified!")
		return
	end

	if IRCMessage.Info[4] == "length" then
		SendMsg(IRCMessage.Channel, string.format("Length: %s", IRCMessage.Info.Length))
	else
		SendMsg(IRCMessage.Channel, "Unkown command!")
	end
end

RegisterPublicCommandHook("publictest", "PublicTest")

function HalfOperatorTest(IRCMessage)
	if IsAdmin3(IRCMessage.Nick, IRCMessage.Host, 0) == false then
		return
	end

	if IRCMessage.Info.Length < 5 then
		SendMsg(IRCMessage.Channel, "The parameters are not specified!")
		return
	end

	if IRCMessage.Info[4] == "length" then
		SendMsg(IRCMessage.Channel, string.format("Length: %s", IRCMessage.Info.Length))
	else
		SendMsg(IRCMessage.Channel, "Unkown command!")
	end
end

RegisterHalfOperatorCommandHook("halfoperatortest", "HalfOperatorTest")

function OperatorTest(IRCMessage)
	if IsAdmin3(IRCMessage.Nick, IRCMessage.Host, 1) == false then
		return
	end

	if IRCMessage.Info.Length < 5 then
		SendMsg(IRCMessage.Channel, "The parameters are not specified!")
		return
	end

	if IRCMessage.Info[4] == "length" then
		SendMsg(IRCMessage.Channel, string.format("Length: %s", IRCMessage.Info.Length))
	else
		SendMsg(IRCMessage.Channel, "Unkown command!")
	end
end

RegisterOperatorCommandHook("operatortest", "OperatorTest")

function AdminTest(IRCMessage)
	if IsAdmin3(IRCMessage.Nick, IRCMessage.Host, 2) == false then
		return
	end

	if IRCMessage.Info.Length < 5 then
		SendMsg(IRCMessage.Channel, "The parameters are not specified!")
		return
	end

	if IRCMessage.Info[4] == "length" then
		SendMsg(IRCMessage.Channel, string.format("Length: %s", IRCMessage.Info.Length))
	else
		SendMsg(IRCMessage.Channel, "Unkown command!")
	end
end

RegisterAdminCommandHook("admintest", "AdminTest")