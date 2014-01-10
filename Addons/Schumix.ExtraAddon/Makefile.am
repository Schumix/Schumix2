
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Addons/ExtraAddon.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Debug/Schumix.Framework.dll \
	../../Run/Debug/Schumix.Irc.dll \
	../../Run/Libraries/Debug/WolframAPI.dll \
	../../External/YamlDotNet/YamlDotNet/bin/Debug/YamlDotNet.dll
BUILD_DIR = ../../Run/Debug/Addons

EXTRAADDON_DLL_MDB_SOURCE=../../Run/Debug/Addons/ExtraAddon.dll.mdb
EXTRAADDON_DLL_MDB=$(BUILD_DIR)/ExtraAddon.dll.mdb
WOLFRAMAPI_DLL_SOURCE=../../Run/Libraries/Debug/WolframAPI.dll
WOLFRAMAPI_DLL_MDB_SOURCE=../../Run/Libraries/Debug/WolframAPI.dll.mdb
WOLFRAMAPI_DLL_MDB=$(BUILD_DIR)/WolframAPI.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Addons/ExtraAddon.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../../Run/Release/Schumix.Framework.dll \
	../../Run/Release/Schumix.Irc.dll \
	../../Run/Libraries/Release/WolframAPI.dll \
	../../External/YamlDotNet/YamlDotNet/bin/Release/YamlDotNet.dll
BUILD_DIR = ../../Run/Release/Addons

EXTRAADDON_DLL_MDB=
WOLFRAMAPI_DLL_SOURCE=../../Run/Libraries/Release/WolframAPI.dll
WOLFRAMAPI_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(EXTRAADDON_DLL_MDB) \
	$(WOLFRAMAPI_DLL) \
	$(WOLFRAMAPI_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SCHUMIX_EXTRAADDON_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	Properties/AssemblyInfo.cs \
	Commands/IrcHandler.cs \
	ExtraAddon.cs \
	Commands/Help.cs \
	Commands/Functions.cs \
	Config/AddonConfig.cs \
	Commands/WebHelper.cs \
	Commands/NameList.cs \
	Commands/NotesCommand.cs \
	Config/ModeConfig.cs \
	Config/WeatherConfig.cs \
	Config/WolframAlphaConfig.cs \
	Config/AddonDefaultConfig.cs \
	Config/AddonXmlConfig.cs \
	Config/AddonYamlConfig.cs \
	Commands/YoutubeTitle.cs \
	Commands/AutoFunctionCommand.cs \
	Commands/MessageCommand.cs \
	Commands/WeatherCommand.cs \
	Commands/RollCommand.cs \
	Commands/Sha1Command.cs \
	Commands/Md5Command.cs \
	Commands/PrimeCommand.cs \
	Commands/WikiCommand.cs \
	Commands/CalcCommand.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Properties \
	Commands \
	Config \
	schumix.extraaddon.pc.in 

REFERENCES =  \
	System \
	System.Xml \
	System.Data \
	System.Web

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

WOLFRAMAPI_DLL = $(BUILD_DIR)/WolframAPI.dll
SCHUMIX_EXTRAADDON_PC = $(BUILD_DIR)/schumix.extraaddon.pc

$(eval $(call emit-deploy-target,WOLFRAMAPI_DLL))
$(eval $(call emit-deploy-target,WOLFRAMAPI_DLL_MDB))
$(eval $(call emit-deploy-wrapper,SCHUMIX_EXTRAADDON_PC,schumix.extraaddon.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
