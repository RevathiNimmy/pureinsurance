@echo off
RD /Q /S "C:\PureInsurance\PureInsurance\Pure Build Process\Installshield\Output\Portal"
MD "C:\PureInsurance\PureInsurance\Pure Build Process\Installshield\Output\Portal"

"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" "C:\PureInsurance\PureInsurance\Web Portal\Nexus\Pure.Portals\website.publishproj" /p:DeployOnBuild=true /p:PublishProfile="C:\PureInusurance\PureInsurance\Web Portal\Nexus\Pure.Portals\App_Data\PublishProfiles\PurePortalBuild.pubxml"
pause