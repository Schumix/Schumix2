
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Schumix.Compiler.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = ../../Run/Debug

SCHUMIX_COMPILER_DLL_MDB_SOURCE=../../Run/Debug/Schumix.Compiler.dll.mdb
SCHUMIX_COMPILER_DLL_MDB=$(BUILD_DIR)/Schumix.Compiler.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Schumix.Compiler.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = ../../Run/Release

SCHUMIX_COMPILER_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SCHUMIX_COMPILER_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_COMPILER_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Sandbox.cs \
	IEntry.cs \
	Abstract.cs \
	Properties/AssemblyInfo.cs \
	Platforms/Platform.cs \
	Platforms/PlatformType.cs \
	Singleton.cs \
	Exceptions/SingletonException.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Properties \
	Platforms \
	Exceptions \
	schumix.compiler.pc.in 

REFERENCES =  \
	System \
	Microsoft.CSharp

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SCHUMIX_COMPILER_PC = $(BUILD_DIR)/schumix.compiler.pc

$(eval $(call emit-deploy-wrapper,SCHUMIX_COMPILER_PC,schumix.compiler.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
