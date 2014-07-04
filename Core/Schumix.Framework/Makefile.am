
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- "-nowarn:219" -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Schumix.Framework.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../External/NGettext/src/NGettext/bin/Debug/NGettext.dll \
	../../External/CsharpSQLite/Run/Debug/System.Data.SQLite.dll \
	../../External/YamlDotNet/YamlDotNet/bin/Debug/YamlDotNet.dll \
	../../External/MySql.Data/Source/MySql.Data/bin/Debug/MySql.Data.dll \
	../../External/ngit/External/MonoLibraries/bin/Debug/Mono.Libraries.dll
BUILD_DIR = ../../Run/Debug

SCHUMIX_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Framework.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/Schumix.Framework.dll.mdb
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
NGETTEXT_DLL_SOURCE=../../External/NGettext/src/NGettext/bin/Debug/NGettext.dll
NGETTEXT_DLL_MDB_SOURCE=../../External/NGettext/src/NGettext/bin/Debug/NGettext.dll.mdb
NGETTEXT_DLL_MDB=$(BUILD_DIR)/NGettext.dll.mdb
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../External/CsharpSQLite/Run/Debug/System.Data.SQLite.dll
SYSTEM_DATA_SQLITE_DLL_MDB_SOURCE=../../External/CsharpSQLite/Run/Debug/System.Data.SQLite.dll.mdb
SYSTEM_DATA_SQLITE_DLL_MDB=$(BUILD_DIR)/System.Data.SQLite.dll.mdb
YAMLDOTNET_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet/bin/Debug/YamlDotNet.dll
YAMLDOTNET_DLL_MDB_SOURCE=../../External/YamlDotNet/YamlDotNet/bin/Debug/YamlDotNet.dll.mdb
YAMLDOTNET_DLL_MDB=$(BUILD_DIR)/YamlDotNet.dll.mdb
MYSQL_DATA_DLL_SOURCE=../../External/MySql.Data/Source/MySql.Data/bin/Debug/MySql.Data.dll
MYSQL_DATA_DLL_MDB_SOURCE=../../External/MySql.Data/Source/MySql.Data/bin/Debug/MySql.Data.dll.mdb
MYSQL_DATA_DLL_MDB=$(BUILD_DIR)/MySql.Data.dll.mdb
MONO_LIBRARIES_DLL_SOURCE=../../External/ngit/External/MonoLibraries/bin/Debug/Mono.Libraries.dll
MONO_LIBRARIES_DLL_MDB_SOURCE=../../External/ngit/External/MonoLibraries/bin/Debug/Mono.Libraries.dll.mdb
MONO_LIBRARIES_DLL_MDB=$(BUILD_DIR)/Mono.Libraries.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-nowarn:219" "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Schumix.Framework.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../External/NGettext/src/NGettext/bin/Release/NGettext.dll \
	../../External/CsharpSQLite/Run/Release/System.Data.SQLite.dll \
	../../External/YamlDotNet/YamlDotNet/bin/Release/YamlDotNet.dll \
	../../External/MySql.Data/Source/MySql.Data/bin/Release/MySql.Data.dll \
	../../External/ngit/External/MonoLibraries/bin/Release/Mono.Libraries.dll
BUILD_DIR = ../../Run/Release

SCHUMIX_FRAMEWORK_DLL_MDB=
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
NGETTEXT_DLL_SOURCE=../../External/NGettext/src/NGettext/bin/Release/NGettext.dll
NGETTEXT_DLL_MDB=
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../External/CsharpSQLite/Run/Release/System.Data.SQLite.dll
SYSTEM_DATA_SQLITE_DLL_MDB=
YAMLDOTNET_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet/bin/Release/YamlDotNet.dll
YAMLDOTNET_DLL_MDB=
MYSQL_DATA_DLL_SOURCE=../../External/MySql.Data/Source/MySql.Data/bin/Release/MySql.Data.dll
MYSQL_DATA_DLL_MDB=
MONO_LIBRARIES_DLL_SOURCE=../../External/ngit/External/MonoLibraries/bin/Release/Mono.Libraries.dll
MONO_LIBRARIES_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SCHUMIX_FRAMEWORK_DLL_MDB) \
	$(SCHUMIX_DB3) \
	$(NGETTEXT_DLL) \
	$(NGETTEXT_DLL_MDB) \
	$(SYSTEM_DATA_SQLITE_DLL) \
	$(SYSTEM_DATA_SQLITE_DLL_MDB) \
	$(YAMLDOTNET_DLL) \
	$(YAMLDOTNET_DLL_MDB) \
	$(MYSQL_DATA_DLL) \
	$(MYSQL_DATA_DLL_MDB) \
	$(MONO_LIBRARIES_DLL) \
	$(MONO_LIBRARIES_DLL_MDB)  

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
	Timer.cs \
	Localization/LocalizationManager.cs \
	Localization/LocalizationConsole.cs \
	Extensions/GeneralExtensions.cs \
	Extensions/CollectionExtensions.cs \
	Extensions/ReflectionExtensions.cs \
	Network/ClientPacketHandler.cs \
	Network/Opcodes.cs \
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
	Config/ListenerConfig.cs \
	Logger/DebugLog.cs \
	Listener/Opcodes.cs \
	Listener/SchumixListener.cs \
	Listener/SchumixPacketHandler.cs \
	Listener/SchumixPacketHandlerDelegate.cs \
	Listener/PacketMethod.cs \
	Network/SchumixPacket.cs \
	Listener/Host.cs \
	Listener/ListenerPacket.cs \
	Extensions/ConvertExtensions.cs \
	Util/Folders.cs \
	Util/Utilities.cs \
	Linux/DefaultLinux.cs \
	Windows/CtrlType.cs \
	Windows/DefaultWindows.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	../../Sql/Schumix.db3 \
	Database/Cache \
	Clean \
	CodeBureau \
	Bitly \
	Options \
	Platforms \
	Logger \
	Util \
	Linux \
	Windows \
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

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SCHUMIX_DB3 = $(BUILD_DIR)/Schumix.db3
NGETTEXT_DLL = $(BUILD_DIR)/NGettext.dll
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
YAMLDOTNET_DLL = $(BUILD_DIR)/YamlDotNet.dll
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
MONO_LIBRARIES_DLL = $(BUILD_DIR)/Mono.Libraries.dll
SCHUMIX_FRAMEWORK_PC = $(BUILD_DIR)/schumix.framework.pc

$(eval $(call emit-deploy-target,SCHUMIX_DB3))
$(eval $(call emit-deploy-target,NGETTEXT_DLL))
$(eval $(call emit-deploy-target,NGETTEXT_DLL_MDB))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL_MDB))
$(eval $(call emit-deploy-target,YAMLDOTNET_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_DLL_MDB))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL_MDB))
$(eval $(call emit-deploy-target,MONO_LIBRARIES_DLL))
$(eval $(call emit-deploy-target,MONO_LIBRARIES_DLL_MDB))
$(eval $(call emit-deploy-wrapper,SCHUMIX_FRAMEWORK_PC,schumix.framework.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
