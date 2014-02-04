@echo off
SETLOCAL
msbuild Schumix.sln /p:Configuration=Debug /flp:LogFile=msbuild.log;Verbosity=Detailed

.\packages\NUnit.Runners.2.6.2\tools\nunit-console.exe .\Run\Tests\Debug\Schumix.Framework.Test.dll /noshadow /result:shippable\testresults\testresults.xml
.\packages\NUnit.Runners.2.6.2\tools\nunit-console.exe .\Run\Tests\Debug\Schumix.Irc.Test.dll /noshadow /result:shippable\testresults\testresults.xml
ENDLOCAL
