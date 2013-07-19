
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Schumix.Irc.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Framework.dll
BUILD_DIR = ../../Run/Debug

SCHUMIX_IRC_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Irc.dll.mdb
SCHUMIX_IRC_DLL_MDB=$(BUILD_DIR)/Schumix.Irc.dll.mdb
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Debug/Schumix.Framework.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
INTL_DLL_SOURCE=../../Dependencies/intl.dll
MONOPOSIXHELPER_DLL_SOURCE=../../Dependencies/MonoPosixHelper.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.Core/bin/Debug/YamlDotNet.Core.dll
YAMLDOTNET_CORE_DLL_MDB_SOURCE=../../External/YamlDotNet/YamlDotNet.Core/bin/Debug/YamlDotNet.Core.dll.mdb
YAMLDOTNET_CORE_DLL_MDB=$(BUILD_DIR)/YamlDotNet.Core.dll.mdb
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Debug/YamlDotNet.RepresentationModel.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_MDB_SOURCE=../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Debug/YamlDotNet.RepresentationModel.dll.mdb
YAMLDOTNET_REPRESENTATIONMODEL_DLL_MDB=$(BUILD_DIR)/YamlDotNet.RepresentationModel.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Framework.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/Schumix.Framework.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Schumix.Irc.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Framework.dll
BUILD_DIR = ../../Run/Release

SCHUMIX_IRC_DLL_MDB=
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Release/Schumix.Framework.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
INTL_DLL_SOURCE=../../Dependencies/intl.dll
MONOPOSIXHELPER_DLL_SOURCE=../../Dependencies/MonoPosixHelper.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.Core/bin/Release/YamlDotNet.Core.dll
YAMLDOTNET_CORE_DLL_MDB=
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Release/YamlDotNet.RepresentationModel.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_MDB=
SCHUMIX_FRAMEWORK_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SCHUMIX_IRC_DLL_MDB) \
	$(SCHUMIX_FRAMEWORK_DLL) \
	$(SCHUMIX_DB3) \
	$(INTL_DLL) \
	$(MONOPOSIXHELPER_DLL) \
	$(SQLITE3_DLL) \
	$(SYSTEM_DATA_SQLITE_DLL) \
	$(MYSQL_DATA_DLL) \
	$(MONO_POSIX_DLL) \
	$(YAMLDOTNET_CORE_DLL) \
	$(YAMLDOTNET_CORE_DLL_MDB) \
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL) \
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL_MDB) \
	$(SCHUMIX_FRAMEWORK_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_IRC_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Commands/CommandHandler.cs \
	Commands/CommandManager.cs \
	MessageHandler.cs \
	Network.cs \
	PublicMessageHandler.cs \
	Properties/AssemblyInfo.cs \
	Commands/CommandInfo.cs \
	ReplyCode.cs \
	Ctcp/CtcpSender.cs \
	Ctcp/CtcpUtil.cs \
	ConnectionType.cs \
	Flood/AntiFlood.cs \
	Flood/CommandFlood.cs \
	IrcColors.cs \
	Commands/SchumixCommandAttribute.cs \
	Commands/AdminFlag.cs \
	Commands/CommandPermission.cs \
	Commands/CommandMethod.cs \
	IrcCommandAttribute.cs \
	Sender/Sender.cs \
	Sender/SendMessage.cs \
	Ignore/IgnoreNickName.cs \
	Ignore/IgnoreIrcCommand.cs \
	Ignore/IgnoreCommand.cs \
	Ignore/IgnoreChannel.cs \
	Ignore/IgnoreAddon.cs \
	IrcBase.cs \
	Channel/ChannelList.cs \
	NickName/MyNickInfo.cs \
	NickName/NickInfo.cs \
	Channel/MyChannelInfo.cs \
	Channel/ChannelInfo.cs \
	Channel/ChannelRank.cs \
	Commands/Public/Whois.cs \
	Commands/Public/GoogleWebSearch/GoogleWebResponseData.cs \
	Commands/Public/GoogleWebSearch/GoogleWebResult.cs \
	Commands/Public/GoogleWebSearch/GoogleWebResultSet.cs \
	Commands/Public/XbotCommand.cs \
	Commands/Public/TimeCommand.cs \
	Commands/Public/DateCommand.cs \
	Commands/Public/IrcCommand.cs \
	Commands/Public/InfoCommand.cs \
	Commands/Public/WhoisCommand.cs \
	Commands/Public/WarningCommand.cs \
	Commands/Public/GoogleCommand.cs \
	Commands/Public/TranslateCommand.cs \
	Commands/Public/OnlineCommand.cs \
	Commands/Admin/QuitCommand.cs \
	Commands/Admin/ReloadCommand.cs \
	Commands/Admin/PluginCommand.cs \
	Commands/Operator/FunctionCommand.cs \
	Commands/Operator/ChannelCommand.cs \
	Commands/Operator/KickCommand.cs \
	Commands/Operator/ModeCommand.cs \
	Commands/Operator/IgnoreCommand.cs \
	Commands/HalfOperator/LeaveCommand.cs \
	Commands/HalfOperator/JoinCommand.cs \
	Commands/HalfOperator/NickCommand.cs \
	Commands/HalfOperator/ColorsCommand.cs \
	Commands/HalfOperator/AdminCommand.cs \
	Util/Rfc2812Util.cs \
	Util/ModeAction.cs \
	Util/UserMode.cs \
	Util/ChannelMode.cs \
	Util/StatsQuery.cs \
	Util/UserInfo.cs \
	Util/Serv.cs \
	Commands/ListenerDataUpdate.cs \
	Method/IrcMethod.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Flood \
	Sender \
	Ignore \
	Channel \
	NickName \
	Commands/Admin \
	Commands/Operator \
	Commands/HalfOperator \
	Commands/Public \
	Util \
	Method \
	schumix.irc.pc.in 

REFERENCES =  \
	System \
	System.Xml \
	System.Data \
	System.Core \
	System.Runtime.Serialization

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SCHUMIX_FRAMEWORK_DLL = $(BUILD_DIR)/Schumix.Framework.dll
SCHUMIX_DB3 = $(BUILD_DIR)/Schumix.db3
INTL_DLL = $(BUILD_DIR)/intl.dll
MONOPOSIXHELPER_DLL = $(BUILD_DIR)/MonoPosixHelper.dll
SQLITE3_DLL = $(BUILD_DIR)/sqlite3.dll
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
YAMLDOTNET_CORE_DLL = $(BUILD_DIR)/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL = $(BUILD_DIR)/YamlDotNet.RepresentationModel.dll
SCHUMIX_IRC_PC = $(BUILD_DIR)/schumix.irc.pc

$(eval $(call emit-deploy-target,SCHUMIX_DB3))
$(eval $(call emit-deploy-target,INTL_DLL))
$(eval $(call emit-deploy-target,MONOPOSIXHELPER_DLL))
$(eval $(call emit-deploy-target,SQLITE3_DLL))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_CORE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_CORE_DLL_MDB))
$(eval $(call emit-deploy-target,YAMLDOTNET_REPRESENTATIONMODEL_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_REPRESENTATIONMODEL_DLL_MDB))
$(eval $(call emit-deploy-wrapper,SCHUMIX_IRC_PC,schumix.irc.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
