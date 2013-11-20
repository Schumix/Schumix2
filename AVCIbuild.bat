@echo off
SETLOCAL
SET BUILD_CONFIG=%1
SET MSBUILD_EXE=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
C:\Program Files (x86)\Git\bin\git.exe submodule update --init --recursive

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
