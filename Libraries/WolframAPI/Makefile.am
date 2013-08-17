
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG;WITH_ASYNC"
ASSEMBLY = ../../Run/Libraries/Debug/WolframAPI.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Framework.dll
BUILD_DIR = ../../Run/Libraries/Debug

WOLFRAMAPI_DLL_MDB_SOURCE=../../Run/Libraries/Debug/WolframAPI.dll.mdb
WOLFRAMAPI_DLL_MDB=$(BUILD_DIR)/WolframAPI.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE;WITH_ASYNC"
ASSEMBLY = ../../Run/Libraries/Release/WolframAPI.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Framework.dll
BUILD_DIR = ../../Run/Libraries/Release

WOLFRAMAPI_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(WOLFRAMAPI_DLL_MDB)  

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

WOLFRAMAPI_PC = $(BUILD_DIR)/wolframapi.pc

$(eval $(call emit-deploy-wrapper,WOLFRAMAPI_PC,wolframapi.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
