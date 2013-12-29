@echo off

SETLOCAL
SET BUILD_CONFIG=%1
SET MSBUILD_EXE=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

Dependencies\SGit\SGit.exe --appveyor
cd .temp
%MSBUILD_EXE% Schumix.sln /p:Configuration=Debug /flp:LogFile=msbuild.log;Verbosity=Detailed

ENDLOCAL
