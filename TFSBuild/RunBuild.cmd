@echo off
RD /Q /S "C:\Pure\Application"
RD /Q /S "C:\PureInsurance\PureInsurance\Binaries"
MD "C:\PureInsurance\PureInsurance\Binaries"
MD "C:\Pure\Application"

RD /Q /S "C:\PureInsurance\PureInsurance\TempFiles"
MD "C:\PureInsurance\PureInsurance\TempFiles"

RD /Q /S "C:\PureInsurance\PureInsurance\Pure Build Process\Installshield\Output
MD "C:\PureInsurance\PureInsurance\Pure Build Process\Installshield\Output\Portal"

attrib -r "C:\PureInsurance\PureInsurance\Web Portal\*" /s /d
attrib -r "C:\PureInsurance\PureInsurance\Pure Build Process\*" /s /d


::move the default theme out of the app_themes folder to TempFiles
move "C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\app_themes" "C:\PureInsurance\PureInsurance\TempFiles"

::move the template product out of the products folder
move "C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\products" "C:\PureInsurance\PureInsurance\TempFiles"

::move the sample email templates out of the email templates folder
move "C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\emailtemplates" "C:\PureInsurance\PureInsurance\TempFiles"

::move the sample Masterpages out of the masterpages folder
move "C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\masterpages" "C:\PureInsurance\PureInsurance\TempFiles"

::move the sample portal files out of the portal folder
move "C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\portal" "C:\PureInsurance\PureInsurance\TempFiles"

move "C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\Claims\ClientPages" "C:\PureInsurance\PureInsurance\TempFiles"
xcopy /E /Y "C:\PureInsurance\PureInsurance\Pure Build Process\Core DLLs\*.*" "C:\PureInsurance\PureInsurance\Binaries"
xcopy /E /Y "C:\PureInsurance\PureInsurance\Pure Build Process\Core DLLs\*.*" "C:\Pure\Application"

copy /Y "C:\PureInsurance\PureInsurance\Pure Build Process\Core DLLs\Microsoft.Practices.EnterpriseLibrary.Logging*.*" "C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\Bin"

copy /Y "C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\Libs\*.*" "C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\Bin"

::remove the libs folder, all assemblies should have been copied to the bin during build 
rd "C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\Libs" /s /q
 
"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" "C:\PureInsurance\PureInsurance\TFSBuild\TFSBuild.proj"

"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" "C:\PureInsurance\PureInsurance\Web Services\STS\SAM Solution\SiriusFS.SAM.WCFService\SiriusFS.SAM.WCFService.vbproj" /p:DeployOnBuild=true /p:PublishProfile="C:\PureInsurance\PureInsurance\Web Services\STS\SAM Solution\SiriusFS.SAM.WCFService\My Project\PublishProfiles\PureWebService.pubxml"

xcopy /E /Y "C:\PureInsurance\PureInsurance\Pure Build Process\Core DLLs\*.*" "C:\PureInsurance\PureInsurance\Binaries\_PublishedWebsites\SiriusFS.SAM.WCFService\bin\"
xcopy /E /Y "C:\PureInsurance\PureInsurance\Pure Build Process\Core DLLs\*.*" "C:\PureInsurance\PureInsurance\Binaries\"

"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" "C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\website.publishproj" /p:DeployOnBuild=true /p:PublishProfile="C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\App_Data\PublishProfiles\PurePortalBuild.pubxml"

