dnl Warning: This is an automatically generated file, do not edit!
dnl Process this file with autoconf to produce a configure script.
AC_PREREQ([2.54])
AC_INIT([Schumix], [4.1.x])
AM_INIT_AUTOMAKE([foreign])
AM_MAINTAINER_MODE

dnl pkg-config
AC_PATH_PROG(PKG_CONFIG, pkg-config, no)
if test "x$PKG_CONFIG" = "xno"; then
        AC_MSG_ERROR([You need to install pkg-config])
fi

SHAMROCK_EXPAND_LIBDIR
SHAMROCK_EXPAND_BINDIR
SHAMROCK_EXPAND_DATADIR

AC_PROG_INSTALL

AC_PATH_PROG(DMCS, dmcs, no)
if test "x$DMCS" = "xno"; then
        AC_MSG_ERROR([dmcs Not found])
fi
AC_PATH_PROG(GMCS, gmcs, no)
if test "x$GMCS" = "xno"; then
        AC_MSG_ERROR([gmcs Not found])
fi


AC_ARG_ENABLE(debug,
	AC_HELP_STRING([--enable-debug],
		[Use 'DEBUG' Configuration [default=NO]]),
		enable_debug=yes, enable_debug=no)
AM_CONDITIONAL(ENABLE_DEBUG, test x$enable_debug = xyes)
if test "x$enable_debug" = "xyes" ; then
	CONFIG_REQUESTED="yes"
fi
AC_ARG_ENABLE(release,
	AC_HELP_STRING([--enable-release],
		[Use 'RELEASE' Configuration [default=YES]]),
		enable_release=yes, enable_release=no)
AM_CONDITIONAL(ENABLE_RELEASE, test x$enable_release = xyes)
if test "x$enable_release" = "xyes" ; then
	CONFIG_REQUESTED="yes"
fi
if test -z "$CONFIG_REQUESTED" ; then
	AM_CONDITIONAL(ENABLE_RELEASE, true)
	enable_release=yes
fi


dnl package checks, common for all configs

dnl package checks, per config


AC_CONFIG_FILES([
External/ngit/External/SharpZipLib/src/icsharpcode.sharpzlib.pc
External/ngit/External/SharpZipLib/src/Makefile
External/YamlDotNet/YamlDotNet.Core/yamldotnet.core.pc
External/YamlDotNet/YamlDotNet.Core/Makefile
External/YamlDotNet/YamlDotNet.RepresentationModel/yamldotnet.representationmodel.pc
External/YamlDotNet/YamlDotNet.RepresentationModel/Makefile
External/NLua/Core/KopiLua/KopiLua/kopilua.pc
External/NLua/Core/KopiLua/KopiLua/Makefile
External/NLua/Core/KeraLua/src/keralua.pc
External/NLua/Core/KeraLua/src/Makefile
External/ChatterBotApi/dotnet/ChatterBotAPI/chatterbotapi.pc
External/ChatterBotApi/dotnet/ChatterBotAPI/Makefile
External/csharp-sqlite/Community.CsharpSqlite/community.csharpsqlite.pc
External/csharp-sqlite/Community.CsharpSqlite/Makefile
External/csharp-sqlite/Community.CsharpSqlite.SQLiteClient/community.csharpsqlite.sqliteclient.pc
External/csharp-sqlite/Community.CsharpSqlite.SQLiteClient/Makefile
Po/Makefile
Core/Schumix.Framework/schumix.framework.pc
Core/Schumix.Framework/Makefile
Core/Schumix.Irc/schumix.irc.pc
Core/Schumix.Irc/Makefile
Core/Makefile
Libraries/WolframAPI/wolframapi.pc
Libraries/WolframAPI/Makefile
External/ngit/Sharpen/sharpen.pc
External/ngit/Sharpen/Makefile
External/ngit/Sharpen.Unix/sharpen.unix.pc
External/ngit/Sharpen.Unix/Makefile
External/NLua/Core/NLua/nlua.pc
External/NLua/Core/NLua/Makefile
Addons/Schumix.TestAddon/schumix.testaddon.pc
Addons/Schumix.TestAddon/Makefile
Addons/Schumix.RevisionAddon/schumix.revisionaddon.pc
Addons/Schumix.RevisionAddon/Makefile
Addons/Schumix.WordPressRssAddon/schumix.wordpressrssaddon.pc
Addons/Schumix.WordPressRssAddon/Makefile
Addons/Schumix.SvnRssAddon/schumix.svnrssaddon.pc
Addons/Schumix.SvnRssAddon/Makefile
Addons/Schumix.MantisBTRssAddon/schumix.mantisbtrssaddon.pc
Addons/Schumix.MantisBTRssAddon/Makefile
Addons/Schumix.ChatterBotAddon/schumix.chatterbotaddon.pc
Addons/Schumix.ChatterBotAddon/Makefile
Addons/Schumix.CompilerAddon/schumix.compileraddon.pc
Addons/Schumix.CompilerAddon/Makefile
Addons/Schumix.HgRssAddon/schumix.hgrssaddon.pc
Addons/Schumix.HgRssAddon/Makefile
Addons/Schumix.GitRssAddon/schumix.gitrssaddon.pc
Addons/Schumix.GitRssAddon/Makefile
Addons/Schumix.CalendarAddon/schumix.calendaraddon.pc
Addons/Schumix.CalendarAddon/Makefile
Addons/Schumix.GameAddon/schumix.gameaddon.pc
Addons/Schumix.GameAddon/Makefile
Addons/Schumix.ExtraAddon/schumix.extraaddon.pc
Addons/Schumix.ExtraAddon/Makefile
Addons/Makefile
External/ngit/NSch/nsch.pc
External/ngit/NSch/Makefile
External/ngit/NGit/ngit.pc
External/ngit/NGit/Makefile
Components/Schumix.Libraries/schumix.libraries.pc
Components/Schumix.Libraries/Makefile
Components/Schumix.Components/schumix.components.pc
Components/Schumix.Components/Makefile
Components/Makefile
Applications/Schumix/schumix
Applications/Schumix/Makefile
Applications/Schumix.Config/schumix.config
Applications/Schumix.Config/Makefile
Applications/Schumix.Installer/schumix.installer
Applications/Schumix.Installer/Makefile
Applications/Schumix.Server/schumix.server
Applications/Schumix.Server/Makefile
Applications/Makefile
Makefile

])

AC_OUTPUT
