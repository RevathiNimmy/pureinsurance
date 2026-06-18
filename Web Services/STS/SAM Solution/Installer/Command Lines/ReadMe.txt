The projects in Sourcesafe that you need to build are :

$STS\SAM Solution\SiriusFS.SAM.sln  (The solution file)
$STS\SAM Solution\SiriusFS.SAM.Structures\*,*
$STS\SAM Solution\SiriusFS.SAM.ServiceAgent\*.*

This process needs to be performed intially for just the core build, but will eventually be required for other versions of the code sets. 

The steps required to build and publish the web service are as follows :

*First run the command lines contained in "BuildInterops.cmd".  This builds the COM Interops for out VB6 components
*Move the resulting files (*.dll) to the bin folder in the web service source folder. i.e. "C:\SOURCE\STS\SAM Solution\SIRIUSFS.SAM.SERVICEAGENT\bin"
*These COM Interop dlls should be checked back into sourcesafe.
*Next the command lines contained in "BuildSoloution.cmd" need to be run.  This builds the project.
*Finally the command lines in "PublishSite.cmd" should be run.  This copies the final release code to a release folder.
