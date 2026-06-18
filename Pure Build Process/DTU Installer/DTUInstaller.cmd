@echo off
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
set buildNum=%build%

attrib -R *.* /S

"C:\Program Files (x86)\InstallShield\2013 SP1 SAB\System\iscmdbld.exe" -p "DTU2.ism" -l PURE_PRODUCT_VERSION=506.000.%buildNum:~-4%

echo Consolidating Database Scripts...
md "DTU2\Media\Release 1\Disk Images\Disk1\Scripts"
md "DTU2\Media\Release 1\Disk Images\Disk1\Scripts\Pure"
md "DTU2\Media\Release 1\Disk Images\Disk1\Scripts\Staging"
md "DTU2\Media\Release 1\Disk Images\Disk1\Scripts\Structure"
md "DTU2\Media\Release 1\Disk Images\Disk1\DTU

xcopy /E /Y "..\..\Scripts\PureStaging.bak" "DTU2\Media\Release 1\Disk Images\Disk1\Scripts"
xcopy /Y "..\..\Scripts\Structure\STRUCTURE.sql" "DTU2\Media\Release 1\Disk Images\Disk1\Scripts\Structure"
xcopy /Y "..\..\Scripts\Staging\*.sql" "DTU2\Media\Release 1\Disk Images\Disk1\Scripts\Staging"
xcopy /Y "..\..\scripts\Pure\*.sql" "DTU2\Media\Release 1\Disk Images\Disk1\Scripts\Pure"
xcopy /E /Y "..\..\Scripts\PureScripts.txt" "DTU2\Media\Release 1\Disk Images\Disk1"

::delete the pdb files before we copy the output
del "..\..\Binaries\*.pdb"

Rem Merging Binaries 
xcopy /Y "..\..\Binaries\*.*" "DTU2\Media\Release 1\Disk Images\Disk1\DTU"




REM set Version=50600.0.%buildNum:~-5%

rem md "DTU2\Media\Release 1\Disk Images\Disk1\scripts\Pure\Upgrade\%Version%"
rem md "DTU2\Media\Release 1\Disk Images\Disk1\scripts\Staging\Upgrade\%Version%"
rem md "DTU2\Media\Release 1\Disk Images\Disk1\scripts\Structure\Upgrade\%Version%"

rem Copy over the scripts and merge procedures

rem xcopy /Y "..\..\Scripts\Structure\STRUCTURE.sql" "DTU2\Media\Release 1\Disk Images\Disk1\scripts\Structure\Upgrade\%Version%"
rem xcopy /Y "..\..\Scripts\Staging\*.sql" "DTU2\Media\Release 1\Disk Images\Disk1\scripts\Staging\Upgrade\%Version%"
rem xcopy /Y "..\..\scripts\Pure\*.sql" "DTU2\Media\Release 1\Disk Images\Disk1\scripts\Pure\Upgrade\%Version%"

rem Copy it all to the output
SET path=%2

SET path=###%path%###
SET path=%path:"###=%
SET path=%path:###"=%
SET path=%path:###=%
SET path=%path%\%1.zip
SET path=%path::=%
   
cd "DTU2\Media\Release 1\Disk Images\Disk1"

"c:\Program Files\7-Zip\7z.exe" a -tzip -r -mx9 "%path%" *.*

 
 
