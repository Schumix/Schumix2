
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Server.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Framework.dll \
	../../Run/Debug/Schumix.Updater.dll
BUILD_DIR = ../../Run/Debug

SERVER_EXE_MDB_SOURCE=../../Run/Debug/Server.exe.mdb
SERVER_EXE_MDB=$(BUILD_DIR)/Server.exe.mdb
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Debug/Schumix.Framework.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Debug/Schumix.Api.dll
SCHUMIX_API_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Api.dll.mdb
SCHUMIX_API_DLL_MDB=$(BUILD_DIR)/Schumix.Api.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Framework.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/Schumix.Framework.dll.mdb
SCHUMIX_UPDATER_DLL_SOURCE=../../Run/Debug/Schumix.Updater.dll
SHARPEN_DLL_SOURCE=../../External/ngit/bin/Sharpen.dll
ICSHARPCODE_SHARPZIPLIB_DLL_SOURCE=../../External/ngit/External/SharpZipLib/bin/ICSharpCode.SharpZipLib.dll
NGIT_DLL_SOURCE=../../External/ngit/bin/NGit.dll
NSCH_DLL_SOURCE=../../External/ngit/bin/NSch.dll
SHARPEN_UNIX_DLL_SOURCE=../../External/ngit/bin/Sharpen.Unix.dll
SCHUMIX_UPDATER_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Updater.dll.mdb
SCHUMIX_UPDATER_DLL_MDB=$(BUILD_DIR)/Schumix.Updater.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Server.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Framework.dll \
	../../Run/Release/Schumix.Updater.dll
BUILD_DIR = ../../Run/Release

SERVER_EXE_MDB=
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Release/Schumix.Framework.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Release/Schumix.Api.dll
SCHUMIX_API_DLL_MDB=
SCHUMIX_FRAMEWORK_DLL_MDB=
SCHUMIX_UPDATER_DLL_SOURCE=../../Run/Release/Schumix.Updater.dll
SHARPEN_DLL_SOURCE=../../External/ngit/bin/Sharpen.dll
ICSHARPCODE_SHARPZIPLIB_DLL_SOURCE=../../External/ngit/External/SharpZipLib/bin/ICSharpCode.SharpZipLib.dll
NGIT_DLL_SOURCE=../../External/ngit/bin/NGit.dll
NSCH_DLL_SOURCE=../../External/ngit/bin/NSch.dll
SHARPEN_UNIX_DLL_SOURCE=../../External/ngit/bin/Sharpen.Unix.dll
SCHUMIX_UPDATER_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SERVER_EXE_MDB) \
	$(MONO_POSIX_DLL) \
	$(YAMLDOTNET_CORE_DLL) \
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL) \
	$(SCHUMIX_FRAMEWORK_DLL) \
	$(SQLITE3_DLL) \
	$(SCHUMIX_DB3) \
	$(SYSTEM_DATA_SQLITE_DLL) \
	$(MYSQL_DATA_DLL) \
	$(SCHUMIX_API_DLL) \
	$(SCHUMIX_API_DLL_MDB) \
	$(SCHUMIX_FRAMEWORK_DLL_MDB) \
	$(SCHUMIX_UPDATER_DLL) \
	$(SHARPEN_DLL) \
	$(ICSHARPCODE_SHARPZIPLIB_DLL) \
	$(NGIT_DLL) \
	$(NSCH_DLL) \
	$(SHARPEN_UNIX_DLL) \
	$(SCHUMIX_UPDATER_DLL_MDB)  

BINARIES = \
	$(SCHUMIX_SERVER)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES = \
	Main.cs \
	ServerListener.cs \
	ServerPacketHandler.cs \
	Properties/AssemblyInfo.cs \
	Config/Config.cs \
	New/Schumix.cs \
	Linux/Linux.cs \
	Windows/Windows.cs \
	Windows/CtrlType.cs \
	New/Settings.cs \
	Config/ServerConfig.cs \
	Config/LocalizationConfig.cs \
	ServerPacketHandlerDelegate.cs \
	Config/DefaultConfig.cs \
	Config/XmlConfig.cs \
	Config/YamlConfig.cs \
	Config/ServerConfigs.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Properties \
	Config \
	New \
	schumix.server.in 

REFERENCES =  \
	System \
	System.Xml \
	System.Core

DLL_REFERENCES =  \
	../../Dependencies/Mono.Posix.dll \
	../../Dependencies/YamlDotNet.Core.dll \
	../../Dependencies/YamlDotNet.RepresentationModel.dll

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
YAMLDOTNET_CORE_DLL = $(BUILD_DIR)/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL = $(BUILD_DIR)/YamlDotNet.RepresentationModel.dll
SCHUMIX_FRAMEWORK_DLL = $(BUILD_DIR)/Schumix.Framework.dll
SQLITE3_DLL = $(BUILD_DIR)/sqlite3.dll
SCHUMIX_DB3 = $(BUILD_DIR)/Schumix.db3
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
SCHUMIX_API_DLL = $(BUILD_DIR)/Schumix.Api.dll
SCHUMIX_UPDATER_DLL = $(BUILD_DIR)/Schumix.Updater.dll
SHARPEN_DLL = $(BUILD_DIR)/Sharpen.dll
ICSHARPCODE_SHARPZIPLIB_DLL = $(BUILD_DIR)/ICSharpCode.SharpZipLib.dll
NGIT_DLL = $(BUILD_DIR)/NGit.dll
NSCH_DLL = $(BUILD_DIR)/NSch.dll
SHARPEN_UNIX_DLL = $(BUILD_DIR)/Sharpen.Unix.dll
SCHUMIX_SERVER = $(BUILD_DIR)/schumix.server

$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_CORE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_REPRESENTATIONMODEL_DLL))
$(eval $(call emit-deploy-target,SQLITE3_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_DB3))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,SHARPEN_DLL))
$(eval $(call emit-deploy-target,ICSHARPCODE_SHARPZIPLIB_DLL))
$(eval $(call emit-deploy-target,NGIT_DLL))
$(eval $(call emit-deploy-target,NSCH_DLL))
$(eval $(call emit-deploy-target,SHARPEN_UNIX_DLL))
$(eval $(call emit-deploy-wrapper,SCHUMIX_SERVER,schumix.server,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
