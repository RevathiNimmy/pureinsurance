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

"C:\Program Files (x86)\InstallShield\2013 SP1 SAB\System\iscmdbld.exe" -p "PIE.ism" -l PURE_PRODUCT_VERSION=506.000.%buildNum:~-4%



::delete the pdb files before we copy the output
del "..\..\Binaries\*.pdb"

md "PIE\Media\Release 1\Disk Images\Disk1\PIE"

Rem Merging Binaries 
xcopy /Y "..\..\Binaries\*.*" "PIE\Media\Release 1\Disk Images\Disk1\PIE"



rem Copy it all to the output
SET path=%2

SET path=###%path%###
SET path=%path:"###=%
SET path=%path:###"=%
SET path=%path:###=%
SET path=%path%\%1.zip
SET path=%path::=%
   
cd "PIE\Media\Release 1\Disk Images\Disk1"

"c:\Program Files\7-Zip\7z.exe" a -tzip -r -mx9 "%path%" *.*

 
 