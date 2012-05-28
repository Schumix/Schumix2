
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Schumix.Framework.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.API.dll
BUILD_DIR = ../../Run/Debug

SCHUMIX_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Framework.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/Schumix.Framework.dll.mdb
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Debug/Schumix.API.dll
SCHUMIX_API_DLL_MDB_SOURCE=../../Run/Debug/Schumix.API.dll.mdb
SCHUMIX_API_DLL_MDB=$(BUILD_DIR)/Schumix.API.dll.mdb

endif

if ENABLE_RELEASE_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Schumix.Framework.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.API.dll
BUILD_DIR = ../../Run/Release

SCHUMIX_FRAMEWORK_DLL_MDB=
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Release/Schumix.API.dll
SCHUMIX_API_DLL_MDB=

endif

if ENABLE_DEBUG_X64
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug_x64/Schumix.Framework.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug_x64/Schumix.API.dll
BUILD_DIR = ../../Run/Debug_x64

SCHUMIX_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug_x64/Schumix.Framework.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/Schumix.Framework.dll.mdb
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Debug_x64/Schumix.API.dll
SCHUMIX_API_DLL_MDB_SOURCE=../../Run/Debug_x64/Schumix.API.dll.mdb
SCHUMIX_API_DLL_MDB=$(BUILD_DIR)/Schumix.API.dll.mdb

endif

if ENABLE_RELEASE_X64
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release_x64/Schumix.Framework.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release_x64/Schumix.API.dll
BUILD_DIR = ../../Run/Release_x64

SCHUMIX_FRAMEWORK_DLL_MDB=
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Release_x64/Schumix.API.dll
SCHUMIX_API_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SCHUMIX_FRAMEWORK_DLL_MDB) \
	$(SQLITE3_DLL) \
	$(SCHUMIX_DB3) \
	$(MYSQL_DATA_DLL) \
	$(SYSTEM_DATA_SQLITE_DLL) \
	$(SCHUMIX_API_DLL) \
	$(SCHUMIX_API_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_FRAMEWORK_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Config/Config.cs \
	Config/SchumixConfig.cs \
	Log.cs \
	Singleton.cs \
	SchumixBase.cs \
	ConsoleLog.cs \
	Properties/AssemblyInfo.cs \
	Exceptions/SingletonException.cs \
	Database/SQLite.cs \
	Database/DatabaseManager.cs \
	Database/MySql.cs \
	Exceptions/SchumixException.cs \
	AddonManager.cs \
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
	PlatformType.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Extensions \
	Localization \
	../../Dependencies/sqlite3.dll \
	../../Sql/Schumix.db3 \
	schumix.framework.pc.in 

REFERENCES =  \
	System \
	System.Data \
	System.Xml \
	System.Web \
	System.Management \
	System.Core \
	System.Net

DLL_REFERENCES =  \
	../../Dependencies/MySql.Data.dll \
	../../Dependencies/System.Data.SQLite.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SQLITE3_DLL = $(BUILD_DIR)/sqlite3.dll
SCHUMIX_DB3 = $(BUILD_DIR)/Schumix.db3
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
SCHUMIX_API_DLL = $(BUILD_DIR)/Schumix.API.dll
SCHUMIX_FRAMEWORK_PC = $(BUILD_DIR)/schumix.framework.pc

$(eval $(call emit-deploy-target,SQLITE3_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_DB3))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL))
$(eval $(call emit-deploy-wrapper,SCHUMIX_FRAMEWORK_PC,schumix.framework.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
