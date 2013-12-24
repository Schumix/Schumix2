@echo off
SETLOCAL
SET BUILD_CONFIG=%1
SET MSBUILD_EXE=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

:: git submodule update --init --recursive
:: Rossz a recursive megoldás ezért van így kézileg megoldva. Az a baj hogy nincs elég jogosultság a működtetéséhez.
git submodule update --init
git submodule foreach git submodule update --init
git submodule foreach git submodule foreach git submodule update --init

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
