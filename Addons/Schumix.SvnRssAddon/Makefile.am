
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Addons/SvnRssAddon.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Framework.dll \
	../../Run/Debug/Schumix.Irc.dll \
	../../External/YamlDotNet/YamlDotNet/bin/Debug/YamlDotNet.dll
BUILD_DIR = ../../Run/Debug/Addons

SVNRSSADDON_DLL_MDB_SOURCE=../../Run/Debug/Addons/SvnRssAddon.dll.mdb
SVNRSSADDON_DLL_MDB=$(BUILD_DIR)/SvnRssAddon.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Addons/SvnRssAddon.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Framework.dll \
	../../Run/Release/Schumix.Irc.dll \
	../../External/YamlDotNet/YamlDotNet/bin/Release/YamlDotNet.dll
BUILD_DIR = ../../Run/Release/Addons

SVNRSSADDON_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SVNRSSADDON_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_SVNRSSADDON_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Properties/AssemblyInfo.cs \
	SvnRssAddon.cs \
	Commands/Commands.cs \
	Rss/SvnRss.cs \
	Localization/PLocalization.cs \
	Config/AddonConfig.cs \
	Config/AddonDefaultConfig.cs \
	Config/AddonXmlConfig.cs \
	Config/AddonYamlConfig.cs \
	Config/RssConfig.cs \
	Rss/RssWebsiteList.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Properties \
	Commands \
	Rss \
	Config \
	schumix.svnrssaddon.pc.in 

REFERENCES =  \
	System \
	System.Xml \
	System.Data \
	System.Core

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SCHUMIX_SVNRSSADDON_PC = $(BUILD_DIR)/schumix.svnrssaddon.pc

$(eval $(call emit-deploy-wrapper,SCHUMIX_SVNRSSADDON_PC,schumix.svnrssaddon.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
