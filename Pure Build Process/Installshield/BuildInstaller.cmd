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
set buildNum=00000%build%

attrib -R *.* /S

"C:\Program Files (x86)\InstallShield\2022 SAB\System\iscmdbld.exe" -p "PureInstaller.ism" -l PURE_PRODUCT_VERSION=506.000.%buildNum:~-4%

echo Consolidating Database Scripts...
md "PureInstaller\Media\Release 1\Disk Images\Disk1\Portal"
md "PureInstaller\Media\Release 1\Disk Images\Disk1\Dashboard"
md "PureInstaller\Media\Release 1\Disk Images\Disk1\Dashboard\scripts"


md "PureInstaller\Media\Release 1\Disk Images\Disk1\Rules"
xcopy /E /Y "..\..\Pure Build Process\Other Installation Items\Rules Templates\*.*" "PureInstaller\Media\Release 1\Disk Images\Disk1\Rules"

md "PureInstaller\Media\Release 1\Disk Images\Disk1\Lists"
xcopy /E /Y "..\..\Pure Build Process\Other Installation Items\Lists\*.*" "PureInstaller\Media\Release 1\Disk Images\Disk1\Lists"


md "PureInstaller\Media\Release 1\Disk Images\Disk1\PMDocs"
md "PureInstaller\Media\Release 1\Disk Images\Disk1\PMDocs\Documents"
md "PureInstaller\Media\Release 1\Disk Images\Disk1\PMDocs\Documents\Type 5"
md "PureInstaller\Media\Release 1\Disk Images\Disk1\PMDocs\Documents\Spooled Documents"
md "PureInstaller\Media\Release 1\Disk Images\Disk1\PMDocs\Signatures"


xcopy /E /Y "..\..\Pure Build Process\Other Installation Items\PMDocs\Documents\Type 5\*.*" "PureInstaller\Media\Release 1\Disk Images\Disk1\PMDocs\Documents\Type 5"
xcopy /E /Y "..\..\Pure Build Process\Other Installation Items\PMDocs\Documents\BlankXML.zip" "PureInstaller\Media\Release 1\Disk Images\Disk1\PMDocs\Documents"

xcopy /E /Y "..\..\Pure Build Process\Other Installation Items\New Install\PurePortal.bak" "PureInstaller\Media\Release 1\Disk Images\Disk1\Portal"
xcopy /E /Y "..\..\Pure Build Process\Other Installation Items\New Install\PureMonitorDB.bak" "PureInstaller\Media\Release 1\Disk Images\Disk1\Dashboard"
xcopy /E /Y "..\..\Pure Build Process\Other Installation Items\New Install\Refresh_Sirius.bat" "PureInstaller\Media\Release 1\Disk Images\Disk1\Dashboard\scripts"
xcopy /E /Y "..\..\Pure Build Process\Other Installation Items\Dashboard\*.*" "PureInstaller\Media\Release 1\Disk Images\Disk1\Dashboard\scripts"

xcopy /Y "..\..\Pure Build Process\Installshield\Output\*.zip" "PureInstaller\Media\Release 1\Disk Images\Disk1\Portal"
xcopy /E /Y "..\..\Pure Build Process\Other Installation Items\New Install\pkzip25.exe" "PureInstaller\Media\Release 1\Disk Images\Disk1\Portal"


md "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts"
md "PureInstaller\Media\Release 1\Disk Images\Disk1\SetupPrerequisites"



xcopy /E /Y "..\..\Pure Build Process\SetupPrerequisites\CRRuntime_32bit_13_0_20.msi" "PureInstaller\Media\Release 1\Disk Images\Disk1\SetupPrerequisites"

xcopy /E /Y "..\..\Databases\Installer\Scripts Template" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts"
xcopy /E /Y "..\..\Pure Build Process\Other Installation Items\New Install\Pure.bak" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts"

xcopy /E /Y "..\..\Pure Build Process\Other Installation Items\Developer License\PMLicence.ini" "PureInstaller\Media\Release 1\Disk Images\Disk1"
xcopy /E /Y "..\..\Pure Build Process\Other Installation Items\New Install\PureConfigFiles.xml" "PureInstaller\Media\Release 1\Disk Images\Disk1"

set Version=50600.0.%buildNum:~-5%

md "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Data\Upgrade\%Version%"
md "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Structure\Upgrade\%Version%"
md "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Procedures\Upgrade\%Version%"
md "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Views\Upgrade\%Version%"

rem Generate the System Options SQL file
"..\..\Binaries\SystemOptionBuildConfigurationS4U.exe" c:\
move /Y "..\..\Binaries\SysOptConfigS4I.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Data\Upgrade\%Version%"

rem Copy over the scripts and merge procedures

xcopy /Y "..\..\Databases\Pure\Data\PURE_DATA.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Data\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Data\TScriptUpdateSysOption.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Data\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Structure\PURE_STRUCTURE.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Structure\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Procedures\MetaProcedures.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Structure\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Procedures\Views\*.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Views\Upgrade\%Version%"

xcopy /Y "..\..\Databases\Pure\Procedures\A-B\*.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Procedures\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Procedures\C-F\*.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Procedures\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Procedures\G-O\*.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Procedures\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Procedures\P-R\*.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Procedures\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Procedures\S-Z\*.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Procedures\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Procedures\Merge Fields\*.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Procedures\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Procedures\Reports\*.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Procedures\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Procedures\spe\*.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Procedures\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Procedures\Triggers\*.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Procedures\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Procedures\DME\*.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Procedures\Upgrade\%Version%"
xcopy /Y "..\..\Databases\Pure\Procedures\SSP Product\*.sql" "PureInstaller\Media\Release 1\Disk Images\Disk1\scripts\Procedures\Upgrade\%Version%"


rem Copy it all to the output
SET path=%2

SET path=###%path%###
SET path=%path:"###=%
SET path=%path:###"=%
SET path=%path:###=%
SET path=%path%\%1.zip
SET path=%path::=%
   
cd "PureInstaller\Media\Release 1\Disk Images\Disk1"


 
