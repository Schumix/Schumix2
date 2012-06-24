

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG;CONTRACTS_FULL;WITH_ASYNC"
ASSEMBLY = ../../Run/Libraries/Debug/WolframAPI.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Framework.dll
BUILD_DIR = ../../Run/Libraries/Debug

WOLFRAMAPI_DLL_MDB_SOURCE=../../Run/Libraries/Debug/WolframAPI.dll.mdb
WOLFRAMAPI_DLL_MDB=$(BUILD_DIR)/WolframAPI.dll.mdb
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Debug/Schumix.Framework.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Debug/Schumix.API.dll
SCHUMIX_API_DLL_MDB_SOURCE=../../Run/Debug/Schumix.API.dll.mdb
SCHUMIX_API_DLL_MDB=$(BUILD_DIR)/Schumix.API.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Framework.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB=$(BUILD_DIR)/Schumix.Framework.dll.mdb

endif

if ENABLE_RELEASE_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE;CONTRACTS_FULL;WITH_ASYNC"
ASSEMBLY = ../../Run/Libraries/Release/WolframAPI.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Framework.dll
BUILD_DIR = ../../Run/Libraries/Release

WOLFRAMAPI_DLL_MDB=
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Release/Schumix.Framework.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Release/Schumix.API.dll
SCHUMIX_API_DLL_MDB=
SCHUMIX_FRAMEWORK_DLL_MDB=

endif

if ENABLE_DEBUG_X64
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG;CONTRACTS_FULL;WITH_ASYNC"
ASSEMBLY = ../../Run/Libraries/Debug_x64/WolframAPI.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug_x64/Schumix.Framework.dll
BUILD_DIR = ../../Run/Libraries/Debug_x64

WOLFRAMAPI_DLL_MDB_SOURCE=../../Run/Libraries/Debug_x64/WolframAPI.dll.mdb
WOLFRAMAPI_DLL_MDB=$(BUILD_DIR)/WolframAPI.dll.mdb
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Debug_x64/Schumix.Framework.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Debug_x64/Schumix.API.dll
SCHUMIX_API_DLL_MDB_SOURCE=../../Run/Debug_x64/Schumix.API.dll.mdb
SCHUMIX_API_DLL_MDB=$(BUILD_DIR)/Schumix.API.dll.mdb
SCHUMIX_FRAMEWORK_DLL_MDB=

endif

if ENABLE_RELEASE_X64
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE;CONTRACTS_FULL;WITH_ASYNC"
ASSEMBLY = ../../Run/Libraries/Release_x64/WolframAPI.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release_x64/Schumix.Framework.dll
BUILD_DIR = ../../Run/Libraries/Release_x64

WOLFRAMAPI_DLL_MDB=
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Release_x64/Schumix.Framework.dll
SQLITE3_DLL_SOURCE=../../Dependencies/sqlite3.dll
SCHUMIX_DB3_SOURCE=../../Sql/Schumix.db3
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Release_x64/Schumix.API.dll
SCHUMIX_API_DLL_MDB=
SCHUMIX_FRAMEWORK_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(WOLFRAMAPI_DLL_MDB) \
	$(SCHUMIX_FRAMEWORK_DLL) \
	$(SQLITE3_DLL) \
	$(SCHUMIX_DB3) \
	$(MYSQL_DATA_DLL) \
	$(SYSTEM_DATA_SQLITE_DLL) \
	$(SCHUMIX_API_DLL) \
	$(SCHUMIX_API_DLL_MDB) \
	$(SCHUMIX_FRAMEWORK_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(WOLFRAMAPI_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Exceptions/WolframException.cs \
	ISerializableType.cs \
	Properties/AssemblyInfo.cs \
	Collections/UniqueList.cs \
	WAClient.cs \
	WAImage.cs \
	WAPod.cs \
	WAResult.cs \
	WASubPod.cs \
	XmlSerialized.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	wolframapi.pc.in 

REFERENCES =  \
	System.Web \
	System.Xml \
	System.Core \
	System

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SCHUMIX_FRAMEWORK_DLL = $(BUILD_DIR)/Schumix.Framework.dll
SQLITE3_DLL = $(BUILD_DIR)/sqlite3.dll
SCHUMIX_DB3 = $(BUILD_DIR)/Schumix.db3
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
SCHUMIX_API_DLL = $(BUILD_DIR)/Schumix.API.dll
WOLFRAMAPI_PC = $(BUILD_DIR)/wolframapi.pc

$(eval $(call emit-deploy-target,SCHUMIX_FRAMEWORK_DLL))
$(eval $(call emit-deploy-target,SQLITE3_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_DB3))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_API_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_API_DLL_MDB))
$(eval $(call emit-deploy-target,SCHUMIX_FRAMEWORK_DLL_MDB))
$(eval $(call emit-deploy-wrapper,WOLFRAMAPI_PC,wolframapi.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
