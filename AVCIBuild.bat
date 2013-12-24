@echo off
SETLOCAL
SET BUILD_CONFIG=%1
SET MSBUILD_EXE=msbuild

git submodule init
git submodule update

if "%BUILD_CONFIG%" == "" (
	set BUILD_CONFIG=Release
)

IF %BUILD_CONFIG%==Debug (
	%MSBUILD_EXE% Schumix.sln /p:Configuration=Debug /flp:LogFile=msbuild.log;Verbosity=Detailed
)

IF %BUILD_CONFIG%==Release (
	%MSBUILD_EXE% Schumix.sln /p:Configuration=Release /flp:LogFile=msbuild.log;Verbosity=Detailed
)

ENDLOCAL
