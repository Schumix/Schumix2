
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
	../../External/YamlDotNet/YamlDotNet/bin/Debug/YamlDotNet.dll
BUILD_DIR = ../../Run/Debug/Addons

WORDPRESSRSSADDON_DLL_MDB_SOURCE=../../Run/Debug/Addons/WordpressRssAddon.dll.mdb
WORDPRESSRSSADDON_DLL_MDB=$(BUILD_DIR)/WordpressRssAddon.dll.mdb

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
	../../External/YamlDotNet/YamlDotNet/bin/Release/YamlDotNet.dll
BUILD_DIR = ../../Run/Release/Addons

WORDPRESSRSSADDON_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(WORDPRESSRSSADDON_DLL_MDB)  

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

SCHUMIX_WORDPRESSRSSADDON_PC = $(BUILD_DIR)/schumix.wordpressrssaddon.pc

$(eval $(call emit-deploy-wrapper,SCHUMIX_WORDPRESSRSSADDON_PC,schumix.wordpressrssaddon.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
