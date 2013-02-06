
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Installer.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = ../../Run/Debug

INSTALLER_EXE_MDB_SOURCE=../../Run/Debug/Installer.exe.mdb
INSTALLER_EXE_MDB=$(BUILD_DIR)/Installer.exe.mdb
ICSHARPCODE_SHARPZIPLIB_DLL_SOURCE=../../Dependencies/ICSharpCode.SharpZipLib.dll

endif

if ENABLE_RELEASE_X86
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Installer.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = ../../Run/Release

INSTALLER_EXE_MDB=
ICSHARPCODE_SHARPZIPLIB_DLL_SOURCE=../../Dependencies/ICSharpCode.SharpZipLib.dll

endif

if ENABLE_DEBUG_X64
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug_x64/Installer.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = ../../Run/Debug_x64

INSTALLER_EXE_MDB_SOURCE=../../Run/Debug_x64/Installer.exe.mdb
INSTALLER_EXE_MDB=$(BUILD_DIR)/Installer.exe.mdb
ICSHARPCODE_SHARPZIPLIB_DLL_SOURCE=../../Dependencies/ICSharpCode.SharpZipLib.dll

endif

if ENABLE_RELEASE_X64
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release_x64/Installer.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = ../../Run/Release_x64

INSTALLER_EXE_MDB=
ICSHARPCODE_SHARPZIPLIB_DLL_SOURCE=../../Dependencies/ICSharpCode.SharpZipLib.dll

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(INSTALLER_EXE_MDB) \
	$(ICSHARPCODE_SHARPZIPLIB_DLL)  

BINARIES = \
	$(SCHUMIX_INSTALLER)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES = \
	Main.cs \
	Compiler/Build.cs \
	Download/DownloadFile.cs \
	UnZip/GZip.cs \
	Properties/AssemblyInfo.cs \
	Localization/LocalizationConsole.cs \
	Utilities.cs \
	Singleton.cs \
	CopyTo/Copy.cs \
	Clean/DirectoryClean.cs \
	Clean/FileClean.cs \
	PlatformType.cs \
	Log.cs \
	Config/InstallerConfig.cs \
	Extensions/GeneralExtensions.cs 

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
	Microsoft.Build.Utilities.v4.0

DLL_REFERENCES =  \
	../../Dependencies/ICSharpCode.SharpZipLib.dll

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

ICSHARPCODE_SHARPZIPLIB_DLL = $(BUILD_DIR)/ICSharpCode.SharpZipLib.dll
SCHUMIX_INSTALLER = $(BUILD_DIR)/schumix.installer

$(eval $(call emit-deploy-target,ICSHARPCODE_SHARPZIPLIB_DLL))
$(eval $(call emit-deploy-wrapper,SCHUMIX_INSTALLER,schumix.installer,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
