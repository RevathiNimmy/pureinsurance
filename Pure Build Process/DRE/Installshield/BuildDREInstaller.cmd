@echo off
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "D:\Builds\PureInsurance\PureInsurance-Main\TFSBuild\TFSDREBuild.proj" /l:FileLogger,Microsoft.Build.Engine;logfile=Build.log;append=false;verbosity=diagnostic;encoding=utf-8
echo Building Installshield Project...
echo.

echo Getting build number
SET buildNum=%1
SET build=
:LOOP
	IF "%buildNum:~-1%"=="." GoTo :DONE
	SET build=%buildNum:~-1%%build%
	SET buildNum==%buildNum:~0,-1%
GoTo :LOOP
:DONE
set buildNum=00000%build%

attrib -R *.* /S

attrib -R "..\Site\bin\*.*" /S

xcopy /E /Y "..\..\..\DREBinaries\RulesEngine.*" "..\Site\bin"

del "..\Site\bin\*.pdb"

"C:\Program Files\InstallShield\2012 SAB\System\iscmdbld.exe" -p "DRE.ism" -l DRE_PRODUCT_VERSION=506.000.%buildNum:~-4%

echo Consolidating Database Scripts...

md "DRE\Media\Release 1\Disk Images\Disk1\scripts"

xcopy /E /Y "..\..\..\Pure Build Process\Other Installation Items\New Install\DRE.bak" "DRE\Media\Release 1\Disk Images\Disk1\scripts"






