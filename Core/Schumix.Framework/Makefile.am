
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Schumix.Framework.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../External/YamlDotNet/YamlDotNet.Core/bin/Debug/YamlDotNet.Core.dll \
	../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Debug/YamlDotNet.RepresentationModel.dll
BUILD_DIR = ../../Run/Debug

SCHUMIX_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Framework.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/Schumix.Framework.dll.mdb
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

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Schumix.Framework.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../External/YamlDotNet/YamlDotNet.Core/bin/Release/YamlDotNet.Core.dll \
	../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Release/YamlDotNet.RepresentationModel.dll
BUILD_DIR = ../../Run/Release

SCHUMIX_FRAMEWORK_DLL_MDB=
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

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SCHUMIX_FRAMEWORK_DLL_MDB) \
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
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_FRAMEWORK_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Config/Config.cs \
	Config/DefaultConfig.cs \
	Singleton.cs \
	SchumixBase.cs \
	Properties/AssemblyInfo.cs \
	Exceptions/SingletonException.cs \
	Database/SQLite.cs \
	Database/DatabaseManager.cs \
	Database/MySql.cs \
	Exceptions/SchumixException.cs \
	Utilities.cs \
	Timer.cs \
	Localization/LocalizationManager.cs \
	Localization/LocalizationConsole.cs \
	Extensions/GeneralExtensions.cs \
	Extensions/CollectionExtensions.cs \
	Extensions/ReflectionExtensions.cs \
	Network/ClientPacketHandler.cs \
	Network/Opcodes.cs \
	Network/SchumixPacket.cs \
	Network/ClientSocket.cs \
	Runtime.cs \
	CrashDumper.cs \
	Config/Consts.cs \
	Config/SchumixConfig.cs \
	Config/IRCConfig.cs \
	Config/MySqlConfig.cs \
	Config/SQLiteConfig.cs \
	Config/LogConfig.cs \
	Config/AddonsConfig.cs \
	Config/ScriptsConfig.cs \
	Config/LocalizationConfig.cs \
	Config/UpdateConfig.cs \
	Config/ServerConfig.cs \
	Config/IRCConfigBase.cs \
	Addon/AddonManager.cs \
	Addon/AddonBase.cs \
	Config/XmlConfig.cs \
	Config/YamlConfig.cs \
	Network/ClientPacketHandlerDelegate.cs \
	Config/CrashConfig.cs \
	JsonHelper.cs \
	Config/ShutdownConfig.cs \
	Config/FloodingConfig.cs \
	Database/Cache/CacheDB.cs \
	Database/Cache/LocalizedCommand.cs \
	Database/Cache/LocalizedConsoleCommand.cs \
	Database/Cache/LocalizedConsoleCommandHelp.cs \
	Database/Cache/LocalizedConsoleWarning.cs \
	Database/Cache/Channels.cs \
	Database/Cache/LocalizedCommandHelp.cs \
	Database/Cache/LocalizedWarning.cs \
	Clean/CleanManager.cs \
	Clean/CleanConfig.cs \
	Clean/CleanDatabase.cs \
	Config/CleanConfig.cs \
	CodeBureau/StringEnum.cs \
	CodeBureau/StringValueAttribute.cs \
	Bitly/BitlyApi.cs \
	Bitly/BitlyResults.cs \
	Config/ShortUrlConfig.cs \
	Options/Option.cs \
	Options/OptionValueCollection.cs \
	Options/OptionDelegate.cs \
	Options/OptionValueType.cs \
	Options/OptionContext.cs \
	Options/OptionSet.cs \
	Exceptions/OptionException.cs \
	Options/ActionOption.cs \
	Platforms/Platform.cs \
	Platforms/PlatformType.cs \
	Options/StringCoda.cs \
	Options/ArgumentSource.cs \
	Options/ResponseFileSource.cs \
	Options/ArgumentEnumerator.cs \
	Options/Category.cs \
	Extensions/YamlExtensions.cs \
	Extensions/ArrayExtensions.cs \
	Extensions/StringExtensions.cs \
	Extensions/DateTimeExtensions.cs \
	Extensions/IrcExtensions.cs \
	Extensions/NumberExtensions.cs \
	Logger/Log.cs \
	Logger/LogType.cs \
	Logger/ConsoleLog.cs \
	Delegate/CommandDelegate.cs \
	Delegate/IRCDelegate.cs \
	Functions/IChannelFunctions.cs \
	Functions/IFunctions.cs \
	Functions/IFunctionsClass.cs \
	Functions/IFunctionsClassBase.cs \
	Irc/INetwork.cs \
	Irc/IRCMessage.cs \
	Irc/MessageType.cs \
	ISchumix.cs \
	Maths/Algorithms/QuickSort.cs \
	Maths/Types/AutoSortedArray.cs \
	Maths/MathFunctions.cs \
	Network/ClientPacketMethod.cs \
	Config/ListenerConfig.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	../../Sql/Schumix.db3 \
	../../Dependencies/intl.dll \
	../../Dependencies/MonoPosixHelper.dll \
	../../Dependencies/sqlite3.dll \
	Database/Cache \
	Clean \
	CodeBureau \
	Bitly \
	Options \
	Platforms \
	Logger \
	schumix.framework.pc.in 

REFERENCES =  \
	System \
	System.Data \
	System.Xml \
	System.Web \
	System.Management \
	System.Core \
	System.Net \
	System.Runtime.Serialization \
	System.Xml.Linq

DLL_REFERENCES =  \
	../../Dependencies/System.Data.SQLite.dll \
	../../Dependencies/MySql.Data.dll \
	../../Dependencies/Mono.Posix.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SCHUMIX_DB3 = $(BUILD_DIR)/Schumix.db3
INTL_DLL = $(BUILD_DIR)/intl.dll
MONOPOSIXHELPER_DLL = $(BUILD_DIR)/MonoPosixHelper.dll
SQLITE3_DLL = $(BUILD_DIR)/sqlite3.dll
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
YAMLDOTNET_CORE_DLL = $(BUILD_DIR)/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL = $(BUILD_DIR)/YamlDotNet.RepresentationModel.dll
SCHUMIX_FRAMEWORK_PC = $(BUILD_DIR)/schumix.framework.pc

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
$(eval $(call emit-deploy-wrapper,SCHUMIX_FRAMEWORK_PC,schumix.framework.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
