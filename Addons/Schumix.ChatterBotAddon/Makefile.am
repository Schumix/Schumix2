
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Addons/ChatterBotAddon.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Framework.dll \
	../../Run/Debug/Schumix.Irc.dll \
	../../External/ChatterBotApi/dotnet/ChatterBotAPI/bin/Debug/ChatterBotAPI.dll
BUILD_DIR = ../../Run/Debug/Addons

CHATTERBOTADDON_DLL_MDB_SOURCE=../../Run/Debug/Addons/ChatterBotAddon.dll.mdb
CHATTERBOTADDON_DLL_MDB=$(BUILD_DIR)/ChatterBotAddon.dll.mdb
CHATTERBOTAPI_DLL_SOURCE=../../External/ChatterBotApi/dotnet/ChatterBotAPI/bin/Debug/ChatterBotAPI.dll
CHATTERBOTAPI_DLL_MDB_SOURCE=../../External/ChatterBotApi/dotnet/ChatterBotAPI/bin/Debug/ChatterBotAPI.dll.mdb
CHATTERBOTAPI_DLL_MDB=$(BUILD_DIR)/ChatterBotAPI.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Addons/ChatterBotAddon.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Framework.dll \
	../../Run/Release/Schumix.Irc.dll \
	../../External/ChatterBotApi/dotnet/ChatterBotAPI/bin/Release/ChatterBotAPI.dll
BUILD_DIR = ../../Run/Release/Addons

CHATTERBOTADDON_DLL_MDB=
CHATTERBOTAPI_DLL_SOURCE=../../External/ChatterBotApi/dotnet/ChatterBotAPI/bin/Release/ChatterBotAPI.dll
CHATTERBOTAPI_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(CHATTERBOTADDON_DLL_MDB) \
	$(CHATTERBOTAPI_DLL) \
	$(CHATTERBOTAPI_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_CHATTERBOTADDON_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	ChatterBotAddon.cs \
	Properties/AssemblyInfo.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Properties \
	schumix.chatterbotaddon.pc.in 

REFERENCES =  \
	System \
	System.Web

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

CHATTERBOTAPI_DLL = $(BUILD_DIR)/ChatterBotAPI.dll
SCHUMIX_CHATTERBOTADDON_PC = $(BUILD_DIR)/schumix.chatterbotaddon.pc

$(eval $(call emit-deploy-target,CHATTERBOTAPI_DLL))
$(eval $(call emit-deploy-target,CHATTERBOTAPI_DLL_MDB))
$(eval $(call emit-deploy-wrapper,SCHUMIX_CHATTERBOTADDON_PC,schumix.chatterbotaddon.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
