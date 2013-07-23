
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Addons/CalendarAddon.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Irc.dll \
	../../Run/Debug/Schumix.Framework.dll \
	../../External/YamlDotNet/YamlDotNet.Core/bin/Debug/YamlDotNet.Core.dll \
	../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Debug/YamlDotNet.RepresentationModel.dll
BUILD_DIR = ../../Run/Debug/Addons

CALENDARADDON_DLL_MDB_SOURCE=../../Run/Debug/Addons/CalendarAddon.dll.mdb
CALENDARADDON_DLL_MDB=$(BUILD_DIR)/CalendarAddon.dll.mdb
SCHUMIX_IRC_DLL_SOURCE=../../Run/Debug/Schumix.Irc.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Debug/Schumix.Framework.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.Core/bin/Debug/YamlDotNet.Core.dll
YAMLDOTNET_CORE_DLL_MDB_SOURCE=../../External/YamlDotNet/YamlDotNet.Core/bin/Debug/YamlDotNet.Core.dll.mdb
YAMLDOTNET_CORE_DLL_MDB=$(BUILD_DIR)/YamlDotNet.Core.dll.mdb
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Debug/YamlDotNet.RepresentationModel.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_MDB_SOURCE=../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Debug/YamlDotNet.RepresentationModel.dll.mdb
YAMLDOTNET_REPRESENTATIONMODEL_DLL_MDB=$(BUILD_DIR)/YamlDotNet.RepresentationModel.dll.mdb
NGETTEXT_DLL_SOURCE=../../External/NGettext/src/NGettext/bin/Debug/NGettext.dll
NGETTEXT_DLL_MDB_SOURCE=../../External/NGettext/src/NGettext/bin/Debug/NGettext.dll.mdb
NGETTEXT_DLL_MDB=$(BUILD_DIR)/NGettext.dll.mdb
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../External/CsharpSQLite/Run/Debug/System.Data.SQLite.dll
SYSTEM_DATA_SQLITE_DLL_MDB_SOURCE=../../External/CsharpSQLite/Run/Debug/System.Data.SQLite.dll.mdb
SYSTEM_DATA_SQLITE_DLL_MDB=$(BUILD_DIR)/System.Data.SQLite.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Framework.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/Schumix.Framework.dll.mdb
SCHUMIX_IRC_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Irc.dll.mdb
SCHUMIX_IRC_DLL_MDB=$(BUILD_DIR)/Schumix.Irc.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Addons/CalendarAddon.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Irc.dll \
	../../Run/Release/Schumix.Framework.dll \
	../../External/YamlDotNet/YamlDotNet.Core/bin/Release/YamlDotNet.Core.dll \
	../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Release/YamlDotNet.RepresentationModel.dll
BUILD_DIR = ../../Run/Release/Addons

CALENDARADDON_DLL_MDB=
SCHUMIX_IRC_DLL_SOURCE=../../Run/Release/Schumix.Irc.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Release/Schumix.Framework.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.Core/bin/Release/YamlDotNet.Core.dll
YAMLDOTNET_CORE_DLL_MDB=
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Release/YamlDotNet.RepresentationModel.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_MDB=
NGETTEXT_DLL_SOURCE=../../External/NGettext/src/NGettext/bin/Release/NGettext.dll
NGETTEXT_DLL_MDB=
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../External/CsharpSQLite/Run/Release/System.Data.SQLite.dll
SYSTEM_DATA_SQLITE_DLL_MDB=
SCHUMIX_FRAMEWORK_DLL_MDB=
SCHUMIX_IRC_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(CALENDARADDON_DLL_MDB) \
	$(SCHUMIX_IRC_DLL) \
	$(SCHUMIX_FRAMEWORK_DLL) \
	$(MYSQL_DATA_DLL) \
	$(YAMLDOTNET_CORE_DLL) \
	$(YAMLDOTNET_CORE_DLL_MDB) \
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL) \
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL_MDB) \
	$(NGETTEXT_DLL) \
	$(NGETTEXT_DLL_MDB) \
	$(SYSTEM_DATA_SQLITE_DLL) \
	$(SYSTEM_DATA_SQLITE_DLL_MDB) \
	$(SCHUMIX_FRAMEWORK_DLL_MDB) \
	$(SCHUMIX_IRC_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_CALENDARADDON_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	CalendarAddon.cs \
	Properties/AssemblyInfo.cs \
	Flood/Flood.cs \
	Config/AddonConfig.cs \
	Calendar/Calendar.cs \
	Ban/Ban.cs \
	Ban/Unban.cs \
	Calendar/CalendarFunctions.cs \
	Flood/FloodChannelParameter.cs \
	Config/CalendarConfig.cs \
	Commands/CalendarCommand.cs \
	Commands/BanCommand.cs \
	Config/AddonDefaultConfig.cs \
	Config/AddonXmlConfig.cs \
	Config/AddonYamlConfig.cs \
	Commands/BirthdayCommand.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Properties \
	Commands \
	Flood \
	Calendar \
	Ban \
	Config \
	schumix.calendaraddon.pc.in 

REFERENCES =  \
	System \
	System.Data \
	System.Xml

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SCHUMIX_IRC_DLL = $(BUILD_DIR)/Schumix.Irc.dll
SCHUMIX_FRAMEWORK_DLL = $(BUILD_DIR)/Schumix.Framework.dll
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
YAMLDOTNET_CORE_DLL = $(BUILD_DIR)/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL = $(BUILD_DIR)/YamlDotNet.RepresentationModel.dll
NGETTEXT_DLL = $(BUILD_DIR)/NGettext.dll
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
SCHUMIX_CALENDARADDON_PC = $(BUILD_DIR)/schumix.calendaraddon.pc

$(eval $(call emit-deploy-target,SCHUMIX_IRC_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_FRAMEWORK_DLL))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_CORE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_CORE_DLL_MDB))
$(eval $(call emit-deploy-target,YAMLDOTNET_REPRESENTATIONMODEL_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_REPRESENTATIONMODEL_DLL_MDB))
$(eval $(call emit-deploy-target,NGETTEXT_DLL))
$(eval $(call emit-deploy-target,NGETTEXT_DLL_MDB))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL_MDB))
$(eval $(call emit-deploy-target,SCHUMIX_FRAMEWORK_DLL_MDB))
$(eval $(call emit-deploy-target,SCHUMIX_IRC_DLL_MDB))
$(eval $(call emit-deploy-wrapper,SCHUMIX_CALENDARADDON_PC,schumix.calendaraddon.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
