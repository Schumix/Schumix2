
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../../Run/Debug/Installer.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../../External/ngit/bin/NGit.dll \
	../../External/ngit/bin/NSch.dll \
	../../External/ngit/External/SharpZipLib/bin/Debug/ICSharpCode.SharpZipLib.dll \
	../../External/ngit/bin/Sharpen.dll \
	../../External/ngit/bin/Sharpen.Unix.dll \
	../../External/NGettext/src/NGettext/bin/Debug/NGettext.dll
BUILD_DIR = ../../Run/Debug

INSTALLER_EXE_MDB_SOURCE=../../Run/Debug/Installer.exe.mdb
INSTALLER_EXE_MDB=$(BUILD_DIR)/Installer.exe.mdb
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
NGIT_DLL_SOURCE=../../External/ngit/bin/NGit.dll
SHARPEN_DLL_SOURCE=../../External/ngit/bin/Sharpen.dll
ICSHARPCODE_SHARPZIPLIB_DLL_SOURCE=../../External/ngit/External/SharpZipLib/bin/Debug/ICSharpCode.SharpZipLib.dll
ICSHARPCODE_SHARPZIPLIB_DLL_MDB_SOURCE=../../External/ngit/External/SharpZipLib/bin/Debug/ICSharpCode.SharpZipLib.dll.mdb
ICSHARPCODE_SHARPZIPLIB_DLL_MDB=$(BUILD_DIR)/ICSharpCode.SharpZipLib.dll.mdb
SHARPEN_DLL_MDB_SOURCE=../../External/ngit/bin/Sharpen.dll.mdb
SHARPEN_DLL_MDB=$(BUILD_DIR)/Sharpen.dll.mdb
NSCH_DLL_SOURCE=../../External/ngit/bin/NSch.dll
MONO_SECURITY_DLL_SOURCE=../../External/ngit/Dependencies/Mono.Security.dll
NSCH_DLL_MDB_SOURCE=../../External/ngit/bin/NSch.dll.mdb
NSCH_DLL_MDB=$(BUILD_DIR)/NSch.dll.mdb
NGIT_DLL_MDB_SOURCE=../../External/ngit/bin/NGit.dll.mdb
NGIT_DLL_MDB=$(BUILD_DIR)/NGit.dll.mdb
SHARPEN_UNIX_DLL_SOURCE=../../External/ngit/bin/Sharpen.Unix.dll
SHARPEN_UNIX_DLL_MDB_SOURCE=../../External/ngit/bin/Sharpen.Unix.dll.mdb
SHARPEN_UNIX_DLL_MDB=$(BUILD_DIR)/Sharpen.Unix.dll.mdb
NGETTEXT_DLL_SOURCE=../../External/NGettext/src/NGettext/bin/Debug/NGettext.dll
NGETTEXT_DLL_MDB_SOURCE=../../External/NGettext/src/NGettext/bin/Debug/NGettext.dll.mdb
NGETTEXT_DLL_MDB=$(BUILD_DIR)/NGettext.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:RELEASE"
ASSEMBLY = ../../Run/Release/Installer.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES =  \
	../../External/ngit/bin/NGit.dll \
	../../External/ngit/bin/NSch.dll \
	../../External/ngit/External/SharpZipLib/bin/Release/ICSharpCode.SharpZipLib.dll \
	../../External/ngit/bin/Sharpen.dll \
	../../External/ngit/bin/Sharpen.Unix.dll \
	../../External/NGettext/src/NGettext/bin/Release/NGettext.dll
BUILD_DIR = ../../Run/Release

INSTALLER_EXE_MDB=
MONO_POSIX_DLL_SOURCE=../../Dependencies/Mono.Posix.dll
NGIT_DLL_SOURCE=../../External/ngit/bin/NGit.dll
SHARPEN_DLL_SOURCE=../../External/ngit/bin/Sharpen.dll
ICSHARPCODE_SHARPZIPLIB_DLL_SOURCE=../../External/ngit/External/SharpZipLib/bin/Release/ICSharpCode.SharpZipLib.dll
ICSHARPCODE_SHARPZIPLIB_DLL_MDB=
SHARPEN_DLL_MDB=
NSCH_DLL_SOURCE=../../External/ngit/bin/NSch.dll
MONO_SECURITY_DLL_SOURCE=../../External/ngit/Dependencies/Mono.Security.dll
NSCH_DLL_MDB=
NGIT_DLL_MDB=
SHARPEN_UNIX_DLL_SOURCE=../../External/ngit/bin/Sharpen.Unix.dll
SHARPEN_UNIX_DLL_MDB=
NGETTEXT_DLL_SOURCE=../../External/NGettext/src/NGettext/bin/Release/NGettext.dll
NGETTEXT_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(INSTALLER_EXE_MDB) \
	$(MONO_POSIX_DLL) \
	$(NGIT_DLL) \
	$(SHARPEN_DLL) \
	$(ICSHARPCODE_SHARPZIPLIB_DLL) \
	$(ICSHARPCODE_SHARPZIPLIB_DLL_MDB) \
	$(SHARPEN_DLL_MDB) \
	$(NSCH_DLL) \
	$(MONO_SECURITY_DLL) \
	$(NSCH_DLL_MDB) \
	$(NGIT_DLL_MDB) \
	$(SHARPEN_UNIX_DLL) \
	$(SHARPEN_UNIX_DLL_MDB) \
	$(NGETTEXT_DLL) \
	$(NGETTEXT_DLL_MDB)  

BINARIES = \
	$(SCHUMIX_INSTALLER)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(BINARIES) 

FILES = \
	Main.cs \
	Utilities.cs \
	Singleton.cs \
	Runtime.cs \
	InstallerBase.cs \
	CrashDumper.cs \
	Compiler/Build.cs \
	Config/Consts.cs \
	CopyTo/Copy.cs \
	Download/CloneSchumix.cs \
	Download/NGitProgressMonitor.cs \
	Exceptions/InstallerException.cs \
	Exceptions/OptionException.cs \
	Exceptions/SingletonException.cs \
	Extensions/ArrayExtensions.cs \
	Extensions/ConvertExtensions.cs \
	Extensions/GeneralExtensions.cs \
	Extensions/StringExtensions.cs \
	Linux/Linux.cs \
	Localization/LocalizationConsole.cs \
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
	Properties/AssemblyInfo.cs \
	Windows/CtrlType.cs \
	Windows/Windows.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Compiler \
	Config \
	CopyTo \
	Download \
	Exceptions \
	Extensions \
	Linux \
	Localization \
	Logger \
	Options \
	Platforms \
	Properties \
	Windows \
	schumix.installer.in 

REFERENCES =  \
	System \
	System.Net \
	System.Web \
	Microsoft.Build.Utilities.v4.0 \
	System.Core

DLL_REFERENCES =  \
	../../Dependencies/Mono.Posix.dll

CLEANFILES = $(PROGRAMFILES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

MONO_POSIX_DLL = $(BUILD_DIR)/Mono.Posix.dll
NGIT_DLL = $(BUILD_DIR)/NGit.dll
SHARPEN_DLL = $(BUILD_DIR)/Sharpen.dll
ICSHARPCODE_SHARPZIPLIB_DLL = $(BUILD_DIR)/ICSharpCode.SharpZipLib.dll
NSCH_DLL = $(BUILD_DIR)/NSch.dll
MONO_SECURITY_DLL = $(BUILD_DIR)/Mono.Security.dll
SHARPEN_UNIX_DLL = $(BUILD_DIR)/Sharpen.Unix.dll
NGETTEXT_DLL = $(BUILD_DIR)/NGettext.dll
SCHUMIX_INSTALLER = $(BUILD_DIR)/schumix.installer

$(eval $(call emit-deploy-target,MONO_POSIX_DLL))
$(eval $(call emit-deploy-target,NGIT_DLL))
$(eval $(call emit-deploy-target,SHARPEN_DLL))
$(eval $(call emit-deploy-target,ICSHARPCODE_SHARPZIPLIB_DLL))
$(eval $(call emit-deploy-target,ICSHARPCODE_SHARPZIPLIB_DLL_MDB))
$(eval $(call emit-deploy-target,SHARPEN_DLL_MDB))
$(eval $(call emit-deploy-target,NSCH_DLL))
$(eval $(call emit-deploy-target,MONO_SECURITY_DLL))
$(eval $(call emit-deploy-target,NSCH_DLL_MDB))
$(eval $(call emit-deploy-target,NGIT_DLL_MDB))
$(eval $(call emit-deploy-target,SHARPEN_UNIX_DLL))
$(eval $(call emit-deploy-target,SHARPEN_UNIX_DLL_MDB))
$(eval $(call emit-deploy-target,NGETTEXT_DLL))
$(eval $(call emit-deploy-target,NGETTEXT_DLL_MDB))
$(eval $(call emit-deploy-wrapper,SCHUMIX_INSTALLER,schumix.installer,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
