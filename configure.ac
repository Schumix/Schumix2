dnl Warning: This is an automatically generated file, do not edit!
dnl Process this file with autoconf to produce a configure script.
AC_PREREQ([2.54])
AC_INIT([Schumix], [3.x])
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


AC_ARG_ENABLE(debug_x86,
	AC_HELP_STRING([--enable-debug_x86],
		[Use 'DEBUG_X86' Configuration [default=NO]]),
		enable_debug_x86=yes, enable_debug_x86=no)
AM_CONDITIONAL(ENABLE_DEBUG_X86, test x$enable_debug_x86 = xyes)
if test "x$enable_debug_x86" = "xyes" ; then
	CONFIG_REQUESTED="yes"
fi
AC_ARG_ENABLE(release_x86,
	AC_HELP_STRING([--enable-release_x86],
		[Use 'RELEASE_X86' Configuration [default=YES]]),
		enable_release_x86=yes, enable_release_x86=no)
AM_CONDITIONAL(ENABLE_RELEASE_X86, test x$enable_release_x86 = xyes)
if test "x$enable_release_x86" = "xyes" ; then
	CONFIG_REQUESTED="yes"
fi
AC_ARG_ENABLE(debug_x64,
	AC_HELP_STRING([--enable-debug_x64],
		[Use 'DEBUG_X64' Configuration [default=NO]]),
		enable_debug_x64=yes, enable_debug_x64=no)
AM_CONDITIONAL(ENABLE_DEBUG_X64, test x$enable_debug_x64 = xyes)
if test "x$enable_debug_x64" = "xyes" ; then
	CONFIG_REQUESTED="yes"
fi
AC_ARG_ENABLE(release_x64,
	AC_HELP_STRING([--enable-release_x64],
		[Use 'RELEASE_X64' Configuration [default=NO]]),
		enable_release_x64=yes, enable_release_x64=no)
AM_CONDITIONAL(ENABLE_RELEASE_X64, test x$enable_release_x64 = xyes)
if test "x$enable_release_x64" = "xyes" ; then
	CONFIG_REQUESTED="yes"
fi
if test -z "$CONFIG_REQUESTED" ; then
	AM_CONDITIONAL(ENABLE_RELEASE_X86, true)
	enable_release_x86=yes
fi


dnl package checks, common for all configs

dnl package checks, per config


AC_CONFIG_FILES([
Core/Schumix.Api/schumix.api.pc
Core/Schumix.Api/Makefile
Core/Schumix.Framework/schumix.framework.pc
Core/Schumix.Framework/Makefile
Core/Schumix.Irc/schumix.irc.pc
Core/Schumix.Irc/Makefile
Core/Makefile
Libraries/WolframAPI/wolframapi.pc
Libraries/WolframAPI/Makefile
External/LuaInterface/Core/KopiLua/kopilua.pc
External/LuaInterface/Core/KopiLua/Makefile
External/LuaInterface/Core/LuaInterface/luainterface.pc
External/LuaInterface/Core/LuaInterface/Makefile
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
Components/Schumix.Libraries/schumix.libraries.pc
Components/Schumix.Libraries/Makefile
Components/Schumix.Updater/schumix.updater.pc
Components/Schumix.Updater/Makefile
Components/Schumix.LuaEngine/schumix.luaengine.pc
Components/Schumix.LuaEngine/Makefile
Components/Schumix.PythonEngine/schumix.pythonengine.pc
Components/Schumix.PythonEngine/Makefile
Components/Makefile
Po/Makefile
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
