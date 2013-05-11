
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Installer.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = ../../Run/Debug

INSTALLER_EXE_MDB_SOURCE=../../Run/Debug/Installer.exe.mdb
INSTALLER_EXE_MDB=$(BUILD_DIR)/Installer.exe.mdb
NGIT_DLL_SOURCE=../../Dependencies/NGit/NGit.dll
SHARPEN_DLL_SOURCE=../../Dependencies/NGit/Sharpen.dll
ICSHARPCODE_SHARPZIPLIB_DLL_SOURCE=../../Dependencies/NGit/ICSharpCode.SharpZipLib.dll
NSCH_DLL_SOURCE=../../Dependencies/NGit/NSch.dll
SHARPEN_UNIX_DLL_SOURCE=../../Dependencies/NGit/Sharpen.Unix.dll

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Installer.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = ../../Run/Release

INSTALLER_EXE_MDB=
NGIT_DLL_SOURCE=../../Dependencies/NGit/NGit.dll
SHARPEN_DLL_SOURCE=../../Dependencies/NGit/Sharpen.dll
ICSHARPCODE_SHARPZIPLIB_DLL_SOURCE=../../Dependencies/NGit/ICSharpCode.SharpZipLib.dll
NSCH_DLL_SOURCE=../../Dependencies/NGit/NSch.dll
SHARPEN_UNIX_DLL_SOURCE=../../Dependencies/NGit/Sharpen.Unix.dll

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(INSTALLER_EXE_MDB) \
	$(NGIT_DLL) \
	$(SHARPEN_DLL) \
	$(ICSHARPCODE_SHARPZIPLIB_DLL) \
	$(NSCH_DLL) \
	$(SHARPEN_UNIX_DLL)  

BINARIES = \
	$(SCHUMIX_INSTALLER)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES = \
	Main.cs \
	Compiler/Build.cs \
	Properties/AssemblyInfo.cs \
	Localization/LocalizationConsole.cs \
	Utilities.cs \
	Singleton.cs \
	CopyTo/Copy.cs \
	Clean/DirectoryClean.cs \
	PlatformType.cs \
	Log.cs \
	Config/InstallerConfig.cs \
	Extensions/GeneralExtensions.cs \
	Download/CloneSchumix.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Properties \
	Config \
	Extensions \
	schumix.installer.in 

REFERENCES =  \
	System \
	System.Net \
	System.Web \
	Microsoft.Build.Utilities.v4.0 \
	System.Core

DLL_REFERENCES =  \
	../../Dependencies/NGit/NGit.dll \
	../../Dependencies/NGit/NSch.dll \
	../../Dependencies/NGit/Sharpen.dll \
	../../Dependencies/NGit/Sharpen.Unix.dll \
	../../Dependencies/NGit/ICSharpCode.SharpZipLib.dll

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

NGIT_DLL = $(BUILD_DIR)/NGit.dll
SHARPEN_DLL = $(BUILD_DIR)/Sharpen.dll
ICSHARPCODE_SHARPZIPLIB_DLL = $(BUILD_DIR)/ICSharpCode.SharpZipLib.dll
NSCH_DLL = $(BUILD_DIR)/NSch.dll
SHARPEN_UNIX_DLL = $(BUILD_DIR)/Sharpen.Unix.dll
SCHUMIX_INSTALLER = $(BUILD_DIR)/schumix.installer

$(eval $(call emit-deploy-target,NGIT_DLL))
$(eval $(call emit-deploy-target,SHARPEN_DLL))
$(eval $(call emit-deploy-target,ICSHARPCODE_SHARPZIPLIB_DLL))
$(eval $(call emit-deploy-target,NSCH_DLL))
$(eval $(call emit-deploy-target,SHARPEN_UNIX_DLL))
$(eval $(call emit-deploy-wrapper,SCHUMIX_INSTALLER,schumix.installer,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
