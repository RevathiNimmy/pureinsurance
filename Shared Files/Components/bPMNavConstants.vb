Option Strict Off
Option Explicit On
Imports System
Public Module NavigatorConstants
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: NavigatorConstants
	'
	' Date: 01/09/1998
	'
	' Description: Contains any constants used by the Navigator
	'              Interface & Business components.
	'
	' Edit History:
	' ***************************************************************** '
	' Note: This file is shared between the Interface & Business.
	'
	'       If any of the following ....ColPos enums are changed, the
	'       corresponding SQL statement in Navigator SQL must also be
	'       changed.
	' ***************************************************************** '
	
	Public Enum ACEProcDetsColPos
		acePDProcessID = 0
		acePDMProductID = 1
		acePDProcessCode = 2
		acePDCaption = 3
		acePDTransactionTypeCode = 4
		acePDProcessMode = 5
		acePDStartMapID = 6
		acePDIsLogged = 7
		acePDIsUserDriven = 8
	End Enum
	
	Public Enum ACEMapDetsColPos
		aceMDMapID = 0
		aceMDMapCode = 1
		aceMDMapCaption = 2
		aceMDIsStartMap = 3
	End Enum
	
	Public Enum ACEMapStepsColPos
		aceMSMapID = 0
		aceMSStepID = 1
		aceMSComponentID = 2
		aceMSComponentType = 3
		aceMSObjectName = 4
		aceMSClassName = 5
		aceMSIsServerSide = 6
		aceMSSubMapID = 7
		aceMSTask = 8
		aceMSOKAction = 9
		aceMSCancelAction = 10
		aceMSOKNoOfSteps = 11
		aceMSCancelNoOfSteps = 12
		aceMSOKProcessID = 13
		aceMSCancelProcessID = 14
		aceMSNavigateStatus = 15
		aceMSStepCaption = 16
		aceMSIsHidden = 17
		aceMSIsLogged = 18
	End Enum
	
	Public Enum ACEMapStepsKeyColPos
		aceMSKeyName = gPMConstants.PMENavLetGetKeyColPosition.PMKeyName
		aceMSKeyInitialValue = gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue
		aceMSKeyStepID = gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue + 1
	End Enum
	
	Public Enum ACENavigatorVersion
		aceNavVersion1 = 1
		aceNavVersion2 = 2
		aceNavVersion3 = 3
	End Enum
End Module