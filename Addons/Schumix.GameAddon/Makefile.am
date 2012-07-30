
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Addons/GameAddon.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.API.dll \
	../../Run/Debug/Schumix.Framework.dll \
	../../Run/Debug/Schumix.Irc.dll
BUILD_DIR = ../../Run/Debug/Addons

GAMEADDON_DLL_MDB_SOURCE=../../Run/Debug/Addons/GameAddon.dll.mdb
GAMEADDON_DLL_MDB=$(BUILD_DIR)/GameAddon.dll.mdb
SCHUMIX_API_DLL_SOURCE=../../Run/Debug/Schumix.API.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Debug/Schumix.Framework.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
SCHUMIX_IRC_DLL_SOURCE=../../Run/Debug/Schumix.Irc.dll

endif

if ENABLE_RELEASE_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Addons/GameAddon.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.API.dll \
	../../Run/Release/Schumix.Framework.dll \
	../../Run/Release/Schumix.Irc.dll
BUILD_DIR = ../../Run/Release/Addons

GAMEADDON_DLL_MDB=
SCHUMIX_API_DLL_SOURCE=../../Run/Release/Schumix.API.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Release/Schumix.Framework.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
SCHUMIX_IRC_DLL_SOURCE=../../Run/Release/Schumix.Irc.dll

endif

if ENABLE_DEBUG_X64
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug_x64/Addons/GameAddon.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug_x64/Schumix.API.dll \
	../../Run/Debug_x64/Schumix.Framework.dll \
	../../Run/Debug_x64/Schumix.Irc.dll
BUILD_DIR = ../../Run/Debug_x64/Addons

GAMEADDON_DLL_MDB_SOURCE=../../Run/Debug_x64/Addons/GameAddon.dll.mdb
GAMEADDON_DLL_MDB=$(BUILD_DIR)/GameAddon.dll.mdb
SCHUMIX_API_DLL_SOURCE=../../Run/Debug_x64/Schumix.API.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Debug_x64/Schumix.Framework.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
SCHUMIX_IRC_DLL_SOURCE=../../Run/Debug_x64/Schumix.Irc.dll

endif

if ENABLE_RELEASE_X64
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release_x64/Addons/GameAddon.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release_x64/Schumix.API.dll \
	../../Run/Release_x64/Schumix.Framework.dll \
	../../Run/Release_x64/Schumix.Irc.dll
BUILD_DIR = ../../Run/Release_x64/Addons

GAMEADDON_DLL_MDB=
SCHUMIX_API_DLL_SOURCE=../../Run/Release_x64/Schumix.API.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Release_x64/Schumix.Framework.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
SCHUMIX_IRC_DLL_SOURCE=../../Run/Release_x64/Schumix.Irc.dll

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(GAMEADDON_DLL_MDB) \
	$(SCHUMIX_API_DLL) \
	$(SCHUMIX_FRAMEWORK_DLL) \
	$(SQLITE3_DLL) \
	$(SCHUMIX_DB3) \
	$(MYSQL_DATA_DLL) \
	$(SYSTEM_DATA_SQLITE_DLL) \
	$(YAMLDOTNET_CORE_DLL) \
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL) \
	$(SCHUMIX_IRC_DLL)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_GAMEADDON_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	GameAddon.cs \
	Properties/AssemblyInfo.cs \
	Commands/Commands.cs \
	MaffiaGame/Join.cs \
	MaffiaGame/Kill.cs \
	MaffiaGame/Lynch.cs \
	MaffiaGame/MaffiaGame.cs \
	MaffiaGame/See.cs \
	MaffiaGame/Start.cs \
	MaffiaGame/Stats.cs \
	MaffiaGame/Leave.cs \
	MaffiaGame/Rescue.cs \
	MaffiaGame/Player.cs \
	MaffiaGame/Rank.cs \
	MaffiaGame/GameOver.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Properties \
	Commands \
	schumix.gameaddon.pc.in 

REFERENCES =  \
	System \
	System.Data

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SCHUMIX_API_DLL = $(BUILD_DIR)/Schumix.API.dll
SCHUMIX_FRAMEWORK_DLL = $(BUILD_DIR)/Schumix.Framework.dll
SQLITE3_DLL = $(BUILD_DIR)/sqlite3.dll
SCHUMIX_DB3 = $(BUILD_DIR)/Schumix.db3
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
YAMLDOTNET_CORE_DLL = $(BUILD_DIR)/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL = $(BUILD_DIR)/YamlDotNet.RepresentationModel.dll
SCHUMIX_IRC_DLL = $(BUILD_DIR)/Schumix.Irc.dll
SCHUMIX_GAMEADDON_PC = $(BUILD_DIR)/schumix.gameaddon.pc

$(eval $(call emit-deploy-target,SCHUMIX_API_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_FRAMEWORK_DLL))
$(eval $(call emit-deploy-target,SQLITE3_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_DB3))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_CORE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_REPRESENTATIONMODEL_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_IRC_DLL))
$(eval $(call emit-deploy-wrapper,SCHUMIX_GAMEADDON_PC,schumix.gameaddon.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
