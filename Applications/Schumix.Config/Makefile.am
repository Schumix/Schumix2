
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Config.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = ../../Run/Debug

CONFIG_EXE_MDB_SOURCE=../../Run/Debug/Config.exe.mdb
CONFIG_EXE_MDB=$(BUILD_DIR)/Config.exe.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Config.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = ../../Run/Release

CONFIG_EXE_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(CONFIG_EXE_MDB)  

BINARIES = \
	$(SCHUMIX_CONFIG)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES = \
	Main.cs \
	Singleton.cs \
	Utilities.cs \
	Runtime.cs \
	CrashDumper.cs \
	ConfigBase.cs \
	Config/Consts.cs \
	CopyTo/Copy.cs \
	Exceptions/ConfigException.cs \
	Exceptions/OptionException.cs \
	Exceptions/SingletonException.cs \
	Extensions/ConvertExtensions.cs \
	Extensions/GeneralExtensions.cs \
	Logger/Log.cs \
	Options/ActionOption.cs \
	Options/ArgumentEnumerator.cs \
	Options/ArgumentSource.cs \
	Options/Category.cs \
	Options/Option.cs \
	Options/OptionContext.cs \
	Options/OptionDelegate.cs \
	Options/OptionSet.cs \
	Options/OptionValueCollection.cs \
	Options/OptionValueType.cs \
	Options/ResponseFileSource.cs \
	Options/StringCoda.cs \
	Platforms/Platform.cs \
	Platforms/PlatformType.cs \
	Properties/AssemblyInfo.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Config \
	CopyTo \
	Exceptions \
	Extensions \
	Logger \
	Options \
	Platforms \
	Properties \
	schumix.config.in 

REFERENCES =  \
	System

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

SCHUMIX_CONFIG = $(BUILD_DIR)/schumix.config

$(eval $(call emit-deploy-wrapper,SCHUMIX_CONFIG,schumix.config,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
