
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Tests/Debug/Schumix.Framework.Test.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Framework.dll
BUILD_DIR = ../../Run/Tests/Debug

SCHUMIX_FRAMEWORK_TEST_DLL_MDB_SOURCE=../../Run/Tests/Debug/Schumix.Framework.Test.dll.mdb
SCHUMIX_FRAMEWORK_TEST_DLL_MDB=$(BUILD_DIR)/Schumix.Framework.Test.dll.mdb
NUNIT_FRAMEWORK_DLL_SOURCE=../../Dependencies/nunit.framework.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Debug/Schumix.Framework.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.Core/bin/Debug/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Debug/YamlDotNet.RepresentationModel.dll
NGETTEXT_DLL_SOURCE=../../External/NGettext/src/NGettext/bin/Debug/NGettext.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../External/CsharpSQLite/Run/Debug/System.Data.SQLite.dll
SCHUMIX_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Framework.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/Schumix.Framework.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Tests/Release/Schumix.Framework.Test.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Framework.dll
BUILD_DIR = ../../Run/Tests/Release

SCHUMIX_FRAMEWORK_TEST_DLL_MDB=
NUNIT_FRAMEWORK_DLL_SOURCE=../../Dependencies/nunit.framework.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Release/Schumix.Framework.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
YAMLDOTNET_CORE_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.Core/bin/Release/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../External/YamlDotNet/YamlDotNet.RepresentationModel/bin/Release/YamlDotNet.RepresentationModel.dll
NGETTEXT_DLL_SOURCE=../../External/NGettext/src/NGettext/bin/Release/NGettext.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../External/CsharpSQLite/Run/Release/System.Data.SQLite.dll
SCHUMIX_FRAMEWORK_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SCHUMIX_FRAMEWORK_TEST_DLL_MDB) \
	$(NUNIT_FRAMEWORK_DLL) \
	$(SCHUMIX_FRAMEWORK_DLL) \
	$(SCHUMIX_DB3) \
	$(MYSQL_DATA_DLL) \
	$(YAMLDOTNET_CORE_DLL) \
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL) \
	$(NGETTEXT_DLL) \
	$(SYSTEM_DATA_SQLITE_DLL) \
	$(SCHUMIX_FRAMEWORK_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_FRAMEWORK_TEST_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Properties/AssemblyInfo.cs \
	Utilities.cs \
	Options/BaseRocksFixture.cs \
	Options/CollectionContract.cs \
	Options/ListContract.cs \
	Options/OptionContextTest.cs \
	Options/OptionSetTest.cs \
	Options/OptionTest.cs \
	Options/Utils.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Properties \
	Options \
	schumix.framework.test.pc.in 

REFERENCES =  \
	System \
	System.Core

DLL_REFERENCES =  \
	../../Dependencies/nunit.framework.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

NUNIT_FRAMEWORK_DLL = $(BUILD_DIR)/nunit.framework.dll
SCHUMIX_FRAMEWORK_DLL = $(BUILD_DIR)/Schumix.Framework.dll
SCHUMIX_DB3 = $(BUILD_DIR)/Schumix.db3
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
YAMLDOTNET_CORE_DLL = $(BUILD_DIR)/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL = $(BUILD_DIR)/YamlDotNet.RepresentationModel.dll
NGETTEXT_DLL = $(BUILD_DIR)/NGettext.dll
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
SCHUMIX_FRAMEWORK_TEST_PC = $(BUILD_DIR)/schumix.framework.test.pc

$(eval $(call emit-deploy-target,NUNIT_FRAMEWORK_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_FRAMEWORK_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_DB3))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_CORE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_REPRESENTATIONMODEL_DLL))
$(eval $(call emit-deploy-target,NGETTEXT_DLL))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_FRAMEWORK_DLL_MDB))
$(eval $(call emit-deploy-wrapper,SCHUMIX_FRAMEWORK_TEST_PC,schumix.framework.test.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
