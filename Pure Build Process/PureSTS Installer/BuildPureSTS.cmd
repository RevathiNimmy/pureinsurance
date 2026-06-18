REM Cleanup

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

rd STS /s /q
rd Output /s /q



REM make dir

mkdir STS
mkdir Output



REM TFS Get Files

rem "C:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\tf.exe" get "$/PureInsurance/Main/STS/" /force /recursive


REM read only


attrib -r -s /S /D "STS\*.*"

@echo off  
echo Build the installer
"C:\Program Files (x86)\InstallShield\2013 SP1 SAB\System\iscmdbld.exe" -p "%BUILD_SOURCESDIRECTORY%\SSP.SecureTokenService\Pure Build Process\PureSTS Installer\Pure Insurance STS.ism" -l PURE_PRODUCT_VERSION=%ProductVersion%

REM build it


REM "C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "C:\STSMain\STS\SSP.SecureTokenService\Thinktecture.IdentityServer.v2-master\src\OnPremise\WebSite\WebSite.csproj" /p:VisualStudioVersion=11.0 /p:DeployOnBuild=true /p:PublishProfile="C:\STSMain\PureSTSPublish.pubxml" /l:FileLogger,Microsoft.Build.Engine;logfile=C:\STSMain\Build.log;append=false;verbosity=diagnostic;encoding=utf-8


REM build completed



set OutputDirectory=Output
set ApplicationName=PureSTSMain
set MajorVersion=5060.0
set BuildNumber=%BuildNumber%
set RevisionNumber=0
set STSOutputFolder=%ApplicationName%_%MajorVersion%.%BuildNumber%.%RevisionNumber%

echo %STSOutputFolder%
echo output folder
echo %BUILD_SOURCESDIRECTORY%\Output\%STSOutputFolder%

move "%BUILD_SOURCESDIRECTORY%\PureSTSPublsh" "%BUILD_SOURCESDIRECTORY%\Output\%STSOutputFolder%"


cd Output

"c:\Program Files\7-Zip\7z.exe" a -tzip -r -mx9 "%STSOutputFolder%.zip" *.*

REM Create installshield package
md "%BUILD_SOURCESDIRECTORY%\SSP.SecureTokenService\Pure Build Process\PureSTS Installer\Pure Insurance STS\Media\Release 1\Disk Images\Disk1\PureSTS"


echo Merging Binaries 
xcopy /Y "%BUILD_SOURCESDIRECTORY%\Output\*.zip*" "%BUILD_SOURCESDIRECTORY%\SSP.SecureTokenService\Pure Build Process\PureSTS Installer\Pure Insurance STS\Media\Release 1\Disk Images\Disk1\PureSTS"

Rem Copy the exe to STS Build folder
xcopy /E /Y "%BUILD_SOURCESDIRECTORY%\SSP.SecureTokenService\Pure Build Process\Other Installation Items\New Install\pkzip25.exe" "%BUILD_SOURCESDIRECTORY%\SSP.SecureTokenService\Pure Build Process\PureSTS Installer\Pure Insurance STS\Media\Release 1\Disk Images\Disk1\PureSTS"


REM build completed


