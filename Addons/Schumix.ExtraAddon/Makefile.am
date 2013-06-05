
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Addons/ExtraAddon.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Api.dll \
	../../Run/Debug/Schumix.Framework.dll \
	../../Run/Debug/Schumix.Irc.dll \
	../../Run/Libraries/Debug/WolframAPI.dll \
	../../External/YamlDotNet/YamlDotNet.Core/bin/Debug/YamlDotNet.Core.dll \
	../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Debug/YamlDotNet.RepresentationModel.dll
BUILD_DIR = ../../Run/Debug/Addons

EXTRAADDON_DLL_MDB_SOURCE=../../Run/Debug/Addons/ExtraAddon.dll.mdb
EXTRAADDON_DLL_MDB=$(BUILD_DIR)/ExtraAddon.dll.mdb
SCHUMIX_API_DLL_SOURCE=../../Run/Debug/Schumix.Api.dll
SCHUMIX_API_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Api.dll.mdb
SCHUMIX_API_DLL_MDB=$(BUILD_DIR)/Schumix.Api.dll.mdb
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Debug/Schumix.Framework.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
INTL_DLL_SOURCE=../../Dependencies/intl.dll
MONOPOSIXHELPER_DLL_SOURCE=../../Dependencies/MonoPosixHelper.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.Core/bin/Debug/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Debug/YamlDotNet.RepresentationModel.dll
SCHUMIX_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Framework.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/Schumix.Framework.dll.mdb
SCHUMIX_IRC_DLL_SOURCE=../../Run/Debug/Schumix.Irc.dll
SCHUMIX_IRC_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Irc.dll.mdb
SCHUMIX_IRC_DLL_MDB=$(BUILD_DIR)/Schumix.Irc.dll.mdb
WOLFRAMAPI_DLL_SOURCE=../../Run/Libraries/Debug/WolframAPI.dll
WOLFRAMAPI_DLL_MDB_SOURCE=../../Run/Libraries/Debug/WolframAPI.dll.mdb
WOLFRAMAPI_DLL_MDB=$(BUILD_DIR)/WolframAPI.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Addons/ExtraAddon.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Api.dll \
	../../Run/Release/Schumix.Framework.dll \
	../../Run/Release/Schumix.Irc.dll \
	../../Run/Libraries/Release/WolframAPI.dll \
	../../External/YamlDotNet/YamlDotNet.Core/bin/Release/YamlDotNet.Core.dll \
	../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Release/YamlDotNet.RepresentationModel.dll
BUILD_DIR = ../../Run/Release/Addons

EXTRAADDON_DLL_MDB=
SCHUMIX_API_DLL_SOURCE=../../Run/Release/Schumix.Api.dll
SCHUMIX_API_DLL_MDB=
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Release/Schumix.Framework.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
INTL_DLL_SOURCE=../../Dependencies/intl.dll
MONOPOSIXHELPER_DLL_SOURCE=../../Dependencies/MonoPosixHelper.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.Core/bin/Release/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Release/YamlDotNet.RepresentationModel.dll
SCHUMIX_FRAMEWORK_DLL_MDB=
SCHUMIX_IRC_DLL_SOURCE=../../Run/Release/Schumix.Irc.dll
SCHUMIX_IRC_DLL_MDB=
WOLFRAMAPI_DLL_SOURCE=../../Run/Libraries/Release/WolframAPI.dll
WOLFRAMAPI_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(EXTRAADDON_DLL_MDB) \
	$(SCHUMIX_API_DLL) \
	$(SCHUMIX_API_DLL_MDB) \
	$(SCHUMIX_FRAMEWORK_DLL) \
	$(SQLITE3_DLL) \
	$(SCHUMIX_DB3) \
	$(INTL_DLL) \
	$(MONOPOSIXHELPER_DLL) \
	$(SYSTEM_DATA_SQLITE_DLL) \
	$(MYSQL_DATA_DLL) \
	$(MONO_POSIX_DLL) \
	$(YAMLDOTNET_CORE_DLL) \
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL) \
	$(SCHUMIX_FRAMEWORK_DLL_MDB) \
	$(SCHUMIX_IRC_DLL) \
	$(SCHUMIX_IRC_DLL_MDB) \
	$(WOLFRAMAPI_DLL) \
	$(WOLFRAMAPI_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_EXTRAADDON_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Properties/AssemblyInfo.cs \
	Commands/IrcHandler.cs \
	ExtraAddon.cs \
	Commands/Commands.cs \
	Commands/Help.cs \
	Commands/Functions.cs \
	Config/AddonConfig.cs \
	Commands/WebHelper.cs \
	Commands/NameList.cs \
	Commands/NotesCommand.cs \
	Config/ModeConfig.cs \
	Config/WeatherConfig.cs \
	Config/WolframAlphaConfig.cs \
	Config/AddonDefaultConfig.cs \
	Config/AddonXmlConfig.cs \
	Config/AddonYamlConfig.cs \
	Commands/YoutubeTitle.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Properties \
	Commands \
	Config \
	schumix.extraaddon.pc.in 

REFERENCES =  \
	System \
	System.Xml \
	System.Data \
	System.Web

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SCHUMIX_API_DLL = $(BUILD_DIR)/Schumix.Api.dll
SCHUMIX_FRAMEWORK_DLL = $(BUILD_DIR)/Schumix.Framework.dll
SQLITE3_DLL = $(BUILD_DIR)/sqlite3.dll
SCHUMIX_DB3 = $(BUILD_DIR)/Schumix.db3
INTL_DLL = $(BUILD_DIR)/intl.dll
MONOPOSIXHELPER_DLL = $(BUILD_DIR)/MonoPosixHelper.dll
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
YAMLDOTNET_CORE_DLL = $(BUILD_DIR)/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL = $(BUILD_DIR)/YamlDotNet.RepresentationModel.dll
SCHUMIX_IRC_DLL = $(BUILD_DIR)/Schumix.Irc.dll
WOLFRAMAPI_DLL = $(BUILD_DIR)/WolframAPI.dll
SCHUMIX_EXTRAADDON_PC = $(BUILD_DIR)/schumix.extraaddon.pc

$(eval $(call emit-deploy-target,SCHUMIX_API_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_API_DLL_MDB))
$(eval $(call emit-deploy-target,SCHUMIX_FRAMEWORK_DLL))
$(eval $(call emit-deploy-target,SQLITE3_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_DB3))
$(eval $(call emit-deploy-target,INTL_DLL))
$(eval $(call emit-deploy-target,MONOPOSIXHELPER_DLL))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_CORE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_REPRESENTATIONMODEL_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_FRAMEWORK_DLL_MDB))
$(eval $(call emit-deploy-target,SCHUMIX_IRC_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_IRC_DLL_MDB))
$(eval $(call emit-deploy-target,WOLFRAMAPI_DLL))
$(eval $(call emit-deploy-target,WOLFRAMAPI_DLL_MDB))
$(eval $(call emit-deploy-wrapper,SCHUMIX_EXTRAADDON_PC,schumix.extraaddon.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
