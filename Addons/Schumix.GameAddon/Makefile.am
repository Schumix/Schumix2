
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Addons/GameAddon.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Framework.dll \
	../../Run/Debug/Schumix.Irc.dll
BUILD_DIR = ../../Run/Debug/Addons

GAMEADDON_DLL_MDB_SOURCE=../../Run/Debug/Addons/GameAddon.dll.mdb
GAMEADDON_DLL_MDB=$(BUILD_DIR)/GameAddon.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Addons/GameAddon.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Framework.dll \
	../../Run/Release/Schumix.Irc.dll
BUILD_DIR = ../../Run/Release/Addons

GAMEADDON_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(GAMEADDON_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_GAMEADDON_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	GameAddon.cs \
	Properties/AssemblyInfo.cs \
	Commands/Commands.cs \
	MaffiaGame/Join.cs \
	MaffiaGame/Kill.cs \
	MaffiaGame/Lynch.cs \
	MaffiaGame/MaffiaGame.cs \
	MaffiaGame/See.cs \
	MaffiaGame/Start.cs \
	MaffiaGame/Stats.cs \
	MaffiaGame/Leave.cs \
	MaffiaGame/Rescue.cs \
	MaffiaGame/Player.cs \
	MaffiaGame/Rank.cs \
	MaffiaGame/GameOver.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Properties \
	Commands \
	schumix.gameaddon.pc.in 

REFERENCES =  \
	System \
	System.Data

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SCHUMIX_GAMEADDON_PC = $(BUILD_DIR)/schumix.gameaddon.pc

$(eval $(call emit-deploy-wrapper,SCHUMIX_GAMEADDON_PC,schumix.gameaddon.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
