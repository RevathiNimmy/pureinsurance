::clear temp directories and clean up files that shouldn't be there!

REM rd BuildSource /s /q
REM rd AssemblyInfo /s /q
REM rd Source /s /q
REM rd Temp /s /q

REM mkdir BuildSource

::pause

::now copy the latest files into the buildsource directory. the build process will delete / move files so we cannot build against the tfs source

attrib -R *.* /S

REM xcopy /E /Y "..\..\Web Portal\*.*" "Buildsource"

REM robocopy "..\..\Web Portal" "BuildSource" /e

::pause

::set the variables

@echo off
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

set OutputDirectory=Output
set ApplicationName=Pure.Portals
set MajorVersion=3020.4
set BuildNumber=%buildNum%
set RevisionNumber=0
set PortalOutputFolder=%ApplicationName% %MajorVersion%.%BuildNumber%.%RevisionNumber%
echo %PortalOutputFolder%
echo %BuildNumber% >> "..\..\Pure Build Process\PortalBuildScripts\BuildNumber.txt"



::create folders to hold files that we want to replace later, before deleting directories
mkdir "buildsource/nexus/pure.portals/TempFiles/app_themes"
mkdir "buildsource/nexus/pure.portals/TempFiles/products"
mkdir "buildsource/nexus/pure.portals/TempFiles/emailtemplates"
mkdir "buildsource/nexus/pure.portals/TempFiles/masterpages"
mkdir "buildsource/nexus/pure.portals/TempFiles/portal"

REM created folders to hold files that we want to replace later, before deleting directories



::move the default theme out of the app_themes folder to TempFiles
move "buildsource/nexus/pure.portals/app_themes/internal" "buildsource/nexus/pure.portals/TempFiles/app_themes/"
move "buildsource/nexus/pure.portals/app_themes/external" "buildsource/nexus/pure.portals/TempFiles/app_themes/"

REM move the default theme out of the app_themes folder to TempFiles


::move the template product out of the products folder
move "buildsource/nexus/pure.portals/products/retailers" "buildsource/nexus/pure.portals/TempFiles/products/"
move "buildsource/nexus/pure.portals/products/GPA" "buildsource/nexus/pure.portals/TempFiles/products/"
move "buildsource/nexus/pure.portals/products/DANDO" "buildsource/nexus/pure.portals/TempFiles/products/"
move "buildsource/nexus/pure.portals/products/PI" "buildsource/nexus/pure.portals/TempFiles/products/"



REM move the template product out of the products folder


::pause

::move the sample email templates out of the email templates folder
move "buildsource/nexus/pure.portals/emailtemplates/sample" "buildsource/nexus/pure.portals/TempFiles/emailtemplates/"

::move the sample Masterpages out of the masterpages folder
move "buildsource/nexus/pure.portals/masterpages/internal" "buildsource/nexus/pure.portals/TempFiles/masterpages/"
move "buildsource/nexus/pure.portals/masterpages/external" "buildsource/nexus/pure.portals/TempFiles/masterpages/"
move "buildsource/nexus/pure.portals/masterpages/broker" "buildsource/nexus/pure.portals/TempFiles/masterpages/"


::move the sample portal files out of the portal folder
move "buildsource/nexus/pure.portals/portal/internal" "buildsource/nexus/pure.portals/TempFiles/portal/"
move "buildsource/nexus/pure.portals/portal/external" "buildsource/nexus/pure.portals/TempFiles/portal/"


::remove the claims clientdetails folder
rd "buildsource/nexus/pure.portals/Claims/ClientPages" /s /q

::remove the products folder, then create a new empty one
rd "buildsource/nexus/pure.portals/products" /s /q
mkdir "buildsource/nexus/pure.portals/products"

::remove the portal folder, then create a new empty one
rd "buildsource/nexus/pure.portals/portal" /s /q
mkdir "buildsource/nexus/pure.portals/portal"

::delete todo.txt if it's there
del "BuildSource\Pure.Portals\todo.txt" /f


::delete the web.config (it refers to config files that we just deleted)
del "BuildSource\nexus\pure.portals\web.config" /f

::pause

::rename defaultweb.config as web.config
move "BuildSource\nexus\pure.portals\defaultweb.config" "BuildSource\nexus\pure.portals\web.config"

::pause

attrib -r -s /S /D "BuildSource\*.*"

REM read only attribute


::pause

REM start building change the project for reference

::build it
cd ..\..\Pure Build Process\PortalBuildScripts
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "..\..\Pure Build Process\PortalBuildScripts\Pure.Portals.wdproj" /l:FileLogger,Microsoft.Build.Engine;logfile=Build.log;append=false;verbosity=diagnostic;encoding=utf-8

if errorlevel 1 goto errorDone
::pause

REM start compile the assemblyinfo.cs


::compile the assemblyinfo.cs class so that the attributes can be used in the merge
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\Csc.exe" /out:"..\..\Pure Build Process\PortalBuildScripts\AssemblyInfo\AssemblyInfo.dll" /target:library "..\..\Pure Build Process\PortalBuildScripts\AssemblyInfo\AssemblyInfo.cs"

::pause

REM start merge assemblies


::merge assemblies
REM "C:\Program Files (x86)\MSBuild\Microsoft\WebDeployment\v8.0\aspnet_merge.exe" "..\..\Pure Build Process\PortalBuildScripts\Temp" -o Pure.Portals.dll -copyattrs "..\..\Pure Build Process\PortalBuildScripts\AssemblyInfo\AssemblyInfo.dll"
"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\NETFX 4.0 Tools\aspnet_merge.exe" "..\..\Pure Build Process\PortalBuildScripts\Temp" -o Pure.Portals.dll -copyattrs "..\..\Pure Build Process\PortalBuildScripts\AssemblyInfo\AssemblyInfo.dll" -a
::pause

REM Move files merge assemblies


::put the temp files back into the build
move "buildsource/nexus/pure.portals/tempfiles/app_themes" temp/app_themes
move "buildsource/nexus/pure.portals/tempfiles/products" temp/products
move "buildsource/nexus/pure.portals/tempfiles/masterpages" temp/masterpages
move "buildsource/nexus/pure.portals/tempfiles/emailtemplates" temp/emailtemplates
move "buildsource/nexus/pure.portals/tempfiles/portal" temp/portal

::pause

::delete the pdb files before we copy the output
del "Temp\bin\*.pdb"

::delete the xml files from the bin folder before we copy the output
del "Temp\bin\*.xml"

::remove the libs folder, all assemblies should have been copied to the bin during build 
rd "Temp\Libs" /s /q

::remove the products folder, then create a new empty one
rd "Temp\app_themes" /s /q
mkdir "Temp\app_themes"

rd "Temp\Claims\ClientPages" /s /q
mkdir "Temp\Claims\ClientPages"

rd "Temp\emailtemplates" /s /q
mkdir "Temp\emailtemplates"

rd "Temp\masterpages" /s /q
mkdir "Temp\masterpages"

rd "Temp\portal" /s /q
mkdir "Temp\portal"

rd "Temp\products" /s /q
mkdir "Temp\products"

xcopy /Y "..\..\PortalMotor\web.config" "Temp"
xcopy /E /Y "..\..\PortalMotor\app_themes\*.*" "Temp\app_themes"
xcopy /E /Y "..\..\PortalMotor\Claims\ClientPages\*.*" "Temp\Claims\ClientPages"
xcopy /E /Y "..\..\PortalMotor\emailtemplates\*.*" "Temp\emailtemplates"
xcopy /E /Y "..\..\PortalMotor\masterpages\*.*" "Temp\masterpages"
xcopy /E /Y "..\..\PortalMotor\portal\*.*" "Temp\portal"
xcopy /E /Y "..\..\PortalMotor\products\*.*" "Temp\products"



::pause
REM finish


REM copy build to correct output folder

::copy build to correct output folder
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "..\..\Pure Build Process\PortalBuildScripts\Pure.Portals.wdproj" /target:FinalCopy /l:FileLogger,Microsoft.Build.Engine;logfile=Build.log;append=true;verbosity=normal;encoding=utf-8

REM zip
REM cd Output\Pure.Portals\%PortalOutputFolder%
cd Output\Pure.Portals

"c:\Program Files\7-Zip\7z.exe" a -tzip -r -mx9 "%PortalOutputFolder%.zip" *.*

REM xcopy /Y "%PortalOutputFolder%.zip" "..\..\..\..\..\Pure Build Process\Installshield\PureInstaller\Media\Release 1\Disk Images\Disk1\Portal"
xcopy /Y "%PortalOutputFolder%.zip" "..\"

REM build completed

if errorlevel 0 goto END

:errorDone
echo REM build Failed
Exit /b 1

:END
echo REM build completed