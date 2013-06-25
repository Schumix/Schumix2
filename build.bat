@echo off
SETLOCAL
SET BUILD_CONFIG=%1

if "%BUILD_CONFIG%" == "" (
	set BUILD_CONFIG=Release
)

IF %BUILD_CONFIG%==Debug (
	%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe Schumix.sln /p:Configuration=Debug /p:PlatformTarget=x86 /flp:LogFile=msbuild.log;Verbosity=Detailed
)

IF %BUILD_CONFIG%==Release (
	%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe Schumix.sln /p:Configuration=Release /p:PlatformTarget=x86 /flp:LogFile=msbuild.log;Verbosity=Detailed
)

ENDLOCAL