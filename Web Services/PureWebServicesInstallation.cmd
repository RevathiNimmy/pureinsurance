REM - SSP Pure Insurance Web Services Installation Script
REM -
REM - This script copies all the necessary files to the Web Services directory
REM - For the script to work Pure must be installed to c:\Pure\
REM - This script will not setup the required virtual directory in IIS
REM - Ensure this script is ran "As Administrator"

rmdir"C:\Pure\Web Services\" /Q/S
xcopy "C:\Pure\Application\_PublishedWebsites\SAM.SERVICEAGENT\*.*"  "C:\Pure\Web Services\" /y /s

REM Business Libraries
xcopy "C:\Pure\Application\b*.dll"  "C:\Pure\Web Services\bin\" /y 
xcopy "C:\Pure\Application\b*.pdb"  "C:\Pure\Web Services\bin\" /y

REM cGISDataset Libraries
xcopy "C:\Pure\Application\c*.dll"  "C:\Pure\Web Services\bin\" /y
xcopy "C:\Pure\Application\c*.pdb"  "C:\Pure\Web Services\bin\" /y

REM Data Access Libraries
xcopy "C:\Pure\Application\d*.dll"  "C:\Pure\Web Services\bin\" /y
xcopy "C:\Pure\Application\d*.pdb"  "C:\Pure\Web Services\bin\" /y

REM Aspose and AxInterop Libraries
xcopy "C:\Pure\Application\a*.dll"  "C:\Pure\Web Services\bin\" /y
xcopy "C:\Pure\Application\a*.pdb"  "C:\Pure\Web Services\bin\" /y

REM COM Interop Libraries
xcopy "C:\Pure\Application\interop*.dll"  "C:\Pure\Web Services\bin\" /y  
xcopy "C:\Pure\Application\interop*.pdb"  "C:\Pure\Web Services\bin\" /y 

REM Sirius Libraries
xcopy "C:\Pure\Application\sirius*.dll"  "C:\Pure\Web Services\bin\" /y  
xcopy "C:\Pure\Application\sirius*.pdb"  "C:\Pure\Web Services\bin\" /y  

REM Other External Libaries
xcopy "C:\Pure\Application\iGISListManager.*"  "C:\Pure\Web Services\bin\" /y
xcopy "C:\Pure\Application\Ionic.zip.dll"  "C:\Pure\Web Services\bin\" /y

pause
