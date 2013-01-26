
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Addons/CompilerAddon.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Api.dll \
	../../Run/Debug/Schumix.Irc.dll \
	../../Run/Debug/Schumix.Framework.dll
BUILD_DIR = ../../Run/Debug/Addons

COMPILERADDON_DLL_MDB_SOURCE=../../Run/Debug/Addons/CompilerAddon.dll.mdb
COMPILERADDON_DLL_MDB=$(BUILD_DIR)/CompilerAddon.dll.mdb
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Debug/Schumix.Api.dll
SCHUMIX_IRC_DLL_SOURCE=../../Run/Debug/Schumix.Irc.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Debug/Schumix.Framework.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll

endif

if ENABLE_RELEASE_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Addons/CompilerAddon.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Api.dll \
	../../Run/Release/Schumix.Irc.dll \
	../../Run/Release/Schumix.Framework.dll
BUILD_DIR = ../../Run/Release/Addons

COMPILERADDON_DLL_MDB=
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Release/Schumix.Api.dll
SCHUMIX_IRC_DLL_SOURCE=../../Run/Release/Schumix.Irc.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Release/Schumix.Framework.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll

endif

if ENABLE_DEBUG_X64
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug_x64/Addons/CompilerAddon.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug_x64/Schumix.Api.dll \
	../../Run/Debug_x64/Schumix.Irc.dll \
	../../Run/Debug_x64/Schumix.Framework.dll
BUILD_DIR = ../../Run/Debug_x64/Addons

COMPILERADDON_DLL_MDB_SOURCE=../../Run/Debug_x64/Addons/CompilerAddon.dll.mdb
COMPILERADDON_DLL_MDB=$(BUILD_DIR)/CompilerAddon.dll.mdb
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Debug_x64/Schumix.Api.dll
SCHUMIX_IRC_DLL_SOURCE=../../Run/Debug_x64/Schumix.Irc.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Debug_x64/Schumix.Framework.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll

endif

if ENABLE_RELEASE_X64
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release_x64/Addons/CompilerAddon.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release_x64/Schumix.Api.dll \
	../../Run/Release_x64/Schumix.Irc.dll \
	../../Run/Release_x64/Schumix.Framework.dll
BUILD_DIR = ../../Run/Release_x64/Addons

COMPILERADDON_DLL_MDB=
YAMLDOTNET_CORE_DLL_SOURCE=../../Dependencies/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL_SOURCE=../../Dependencies/YamlDotNet.RepresentationModel.dll
SCHUMIX_API_DLL_SOURCE=../../Run/Release_x64/Schumix.Api.dll
SCHUMIX_IRC_DLL_SOURCE=../../Run/Release_x64/Schumix.Irc.dll
SCHUMIX_FRAMEWORK_DLL_SOURCE=../../Run/Release_x64/Schumix.Framework.dll
SYSTEM_DATA_SQLITE_DLL_SOURCE=../../Dependencies/System.Data.SQLite.dll
MYSQL_DATA_DLL_SOURCE=../../Dependencies/MySql.Data.dll
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(COMPILERADDON_DLL_MDB) \
	$(YAMLDOTNET_CORE_DLL) \
	$(YAMLDOTNET_REPRESENTATIONMODEL_DLL) \
	$(SCHUMIX_API_DLL) \
	$(SCHUMIX_IRC_DLL) \
	$(SCHUMIX_FRAMEWORK_DLL) \
	$(SYSTEM_DATA_SQLITE_DLL) \
	$(MYSQL_DATA_DLL) \
	$(MONO_POSIX_DLL)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_COMPILERADDON_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Commands/Compiler.cs \
	Properties/AssemblyInfo.cs \
	CompilerAddon.cs \
	Config/AddonConfig.cs \
	Commands/Help.cs \
	Config/CompilerConfig.cs \
	Config/AddonDefaultConfig.cs \
	Config/AddonXmlConfig.cs \
	Config/AddonYamlConfig.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Commands \
	Properties \
	Config \
	schumix.compileraddon.pc.in 

REFERENCES =  \
	System \
	System.Xml

DLL_REFERENCES =  \
	../../Dependencies/YamlDotNet.Core.dll \
	../../Dependencies/YamlDotNet.RepresentationModel.dll

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

YAMLDOTNET_CORE_DLL = $(BUILD_DIR)/YamlDotNet.Core.dll
YAMLDOTNET_REPRESENTATIONMODEL_DLL = $(BUILD_DIR)/YamlDotNet.RepresentationModel.dll
SCHUMIX_API_DLL = $(BUILD_DIR)/Schumix.Api.dll
SCHUMIX_IRC_DLL = $(BUILD_DIR)/Schumix.Irc.dll
SCHUMIX_FRAMEWORK_DLL = $(BUILD_DIR)/Schumix.Framework.dll
SYSTEM_DATA_SQLITE_DLL = $(BUILD_DIR)/System.Data.SQLite.dll
MYSQL_DATA_DLL = $(BUILD_DIR)/MySql.Data.dll
MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
SCHUMIX_COMPILERADDON_PC = $(BUILD_DIR)/schumix.compileraddon.pc

$(eval $(call emit-deploy-target,YAMLDOTNET_CORE_DLL))
$(eval $(call emit-deploy-target,YAMLDOTNET_REPRESENTATIONMODEL_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_API_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_IRC_DLL))
$(eval $(call emit-deploy-target,SCHUMIX_FRAMEWORK_DLL))
$(eval $(call emit-deploy-target,SYSTEM_DATA_SQLITE_DLL))
$(eval $(call emit-deploy-target,MYSQL_DATA_DLL))
$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-wrapper,SCHUMIX_COMPILERADDON_PC,schumix.compileraddon.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
