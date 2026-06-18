@echo off
echo Building Installshield Project...
echo.
echo BUILD_SOURCESDIRECTORY contents:
echo %BUILD_SOURCESDIRECTORY%

echo Build.BuildNumber contents:
echo %Build_BuildNumber%
echo %BuildNumber%


@echo off
echo Building Installshield Project...
echo.
set ProductVersion=506.000.%BuildNumber%
echo ProductVersion
echo %ProductVersion%
set PURE_PRODUCT_VERSION=%ProductVersion%

attrib -R *.* /S

"C:\Program Files (x86)\InstallShield\2013 SP1 SAB\System\iscmdbld.exe" -p "%BUILD_SOURCESDIRECTORY%\DataManagement\Pure Build Process\DataManagement Installer\DataManagement.ism" -l PURE_PRODUCT_VERSION=%ProductVersion%

echo PURE_PRODUCT_VERSION
echo %PURE_PRODUCT_VERSION%
echo Consolidating Database Scripts...
md "%BUILD_SOURCESDIRECTORY%\DataManagement\Pure Build Process\DataManagement Installer\DataManagement\Media\Release 1\Disk Images\Disk1\Scripts"
md "%BUILD_SOURCESDIRECTORY%\DataManagement\Pure Build Process\DataManagement Installer\DataManagement\Media\Release 1\Disk Images\Disk1\Scripts\Pure"
md "%BUILD_SOURCESDIRECTORY%\DataManagement\Pure Build Process\DataManagement Installer\DataManagement\Media\Release 1\Disk Images\Disk1\DataManagement"
echo Created the folders
xcopy /Y "%BUILD_SOURCESDIRECTORY%\DataManagement\DataBase Scripts\DataScripts.sql" "%BUILD_SOURCESDIRECTORY%\DataManagement\Pure Build Process\DataManagement Installer\DataManagement\Media\Release 1\Disk Images\Disk1\Scripts\Pure"
xcopy /Y "%BUILD_SOURCESDIRECTORY%\DataManagement\DataBase Scripts\Stored Procedures\*.sql" "%BUILD_SOURCESDIRECTORY%\DataManagement\Pure Build Process\DataManagement Installer\DataManagement\Media\Release 1\Disk Images\Disk1\Scripts\Pure"
xcopy /Y "%BUILD_SOURCESDIRECTORY%\DataManagement\DataBase Scripts\PureScripts.txt" "%BUILD_SOURCESDIRECTORY%\DataManagement\Pure Build Process\DataManagement Installer\DataManagement\Media\Release 1\Disk Images\Disk1"


rem DataManagement\Pure Build Process\DataManagement Installer
echo Copied DB scripts
rem echo delete pdb


echo Merging Binaries 
xcopy /Y "%BUILD_SOURCESDIRECTORY%\DataManagement\bin\Release\*.*" "%BUILD_SOURCESDIRECTORY%\DataManagement\Pure Build Process\DataManagement Installer\DataManagement\Media\Release 1\Disk Images\Disk1\DataManagement"

echo build completes


 
 