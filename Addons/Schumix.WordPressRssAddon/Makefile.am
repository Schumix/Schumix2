
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Addons/WordpressRssAddon.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Irc.dll \
	../../Run/Debug/Schumix.Framework.dll \
	../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Debug/YamlDotNet.RepresentationModel.dll \
	../../External/YamlDotNet/YamlDotNet.Core/bin/Debug/YamlDotNet.Core.dll
BUILD_DIR = ../../Run/Debug/Addons

WORDPRESSRSSADDON_DLL_MDB_SOURCE=../../Run/Debug/Addons/WordpressRssAddon.dll.mdb
WORDPRESSRSSADDON_DLL_MDB=$(BUILD_DIR)/WordpressRssAddon.dll.mdb
SCHUMIX_IRC_DLL_SOURCE=../../Run/Debug/Schumix.Irc.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Debug/Schumix.Framework.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.Core/bin/Debug/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Debug/YamlDotNet.RepresentationModel.dll
COMMUNITY_CSHARPSQLITE_DLL_SOURCE=../../External/csharp-sqlite/Community.CsharpSqlite/bin/Debug/Community.CsharpSqlite.dll
COMMUNITY_CSHARPSQLITE_SQLITECLIENT_DLL_SOURCE=../../External/csharp-sqlite/Community.CsharpSqlite.SQLiteClient/bin/Debug/Community.CsharpSqlite.SQLiteClient.dll
SCHUMIX_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Framework.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/Schumix.Framework.dll.mdb
SCHUMIX_IRC_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Irc.dll.mdb
SCHUMIX_IRC_DLL_MDB=$(BUILD_DIR)/Schumix.Irc.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Addons/WordpressRssAddon.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Irc.dll \
	../../Run/Release/Schumix.Framework.dll \
	../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Release/YamlDotNet.RepresentationModel.dll \
	../../External/YamlDotNet/YamlDotNet.Core/bin/Release/YamlDotNet.Core.dll
BUILD_DIR = ../../Run/Release/Addons

WORDPRESSRSSADDON_DLL_MDB=
SCHUMIX_IRC_DLL_SOURCE=../../Run/Release/Schumix.Irc.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Release/Schumix.Framework.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.Core/bin/Release/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Release/YamlDotNet.RepresentationModel.dll
COMMUNITY_CSHARPSQLITE_DLL_SOURCE=../../External/csharp-sqlite/Community.CsharpSqlite/bin/Release/Community.CsharpSqlite.dll
COMMUNITY_CSHARPSQLITE_SQLITECLIENT_DLL_SOURCE=../../External/csharp-sqlite/Community.CsharpSqlite.SQLiteClient/bin/Release/Community.CsharpSqlite.SQLiteClient.dll
SCHUMIX_FRAMEWORK_DLL_MDB=
SCHUMIX_IRC_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(WORDPRESSRSSADDON_DLL_MDB) \
	$(SCHUMIX_IRC_DLL) \
	$(SCHUMIX_FRAMEWORK_DLL) \
	$(MYSQL_DATA_DLL) \
	$(MONO_POSIX_DLL) \
	$(YAMLDOTNET_CORE_DLL) \
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL) \
	$(COMMUNITY_CSHARPSQLITE_DLL) \
	$(COMMUNITY_CSHARPSQLITE_SQLITECLIENT_DLL) \
	$(SCHUMIX_FRAMEWORK_DLL_MDB) \
	$(SCHUMIX_IRC_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_WORDPRESSRSSADDON_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Properties/AssemblyInfo.cs \
	Commands/Commands.cs \
	Config/AddonConfig.cs \
	Localization/PLocalization.cs \
	Rss/WordPressRss.cs \
	WordPressRssAddon.cs \
	Config/RssConfig.cs \
	Config/AddonDefaultConfig.cs \
	Config/AddonXmlConfig.cs \
	Config/AddonYamlConfig.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	schumix.wordpressrssaddon.pc.in 

REFERENCES =  \
	System \
	System.Data \
	System.Xml \
	System.Core

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SCHUMIX_IRC_DLL = $(BUILD_DIR)/Schumix.Irc.dll
SCHUMIX_FRAMEWORK_DLL = $(BUILD_DIR)/Schumix.Framework.dll
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
YAMLDOTNET_CORE_DLL = $(BUILD_DIR)/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL = $(BUILD_DIR)/YamlDotNet.RepresentationModel.dll
COMMUNITY_CSHARPSQLITE_DLL = $(BUILD_DIR)/Community.CsharpSqlite.dll
COMMUNITY_CSHARPSQLITE_SQLITECLIENT_DLL = $(BUILD_DIR)/Community.CsharpSqlite.SQLiteClient.dll
SCHUMIX_WORDPRESSRSSADDON_PC = $(BUILD_DIR)/schumix.wordpressrssaddon.pc

$(eval $(call emit-deploy-target,SCHUMIX_IRC_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_FRAMEWORK_DLL))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_CORE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_REPRESENTATIONMODEL_DLL))
$(eval $(call emit-deploy-target,COMMUNITY_CSHARPSQLITE_DLL))
$(eval $(call emit-deploy-target,COMMUNITY_CSHARPSQLITE_SQLITECLIENT_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_FRAMEWORK_DLL_MDB))
$(eval $(call emit-deploy-target,SCHUMIX_IRC_DLL_MDB))
$(eval $(call emit-deploy-wrapper,SCHUMIX_WORDPRESSRSSADDON_PC,schumix.wordpressrssaddon.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
