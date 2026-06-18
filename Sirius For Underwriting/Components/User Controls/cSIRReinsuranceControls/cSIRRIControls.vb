Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Module Name: MainModule
	' ***************************************************************** '
	
	' Main public constant for all functions to identify which application this is.
	Public Const ACApp As String = "cSIRReinsuranceControls"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"

    ' Public instance of the object manager.
    <ThreadStatic()> _
     Public g_oObjectManager As bObjectManager.ObjectManager

	' ***************************************************************** '
	'                         RI MODEL ARRAY
	' ***************************************************************** '
	Public Enum RIModelEnum
		DBMRIModelID
		DBMCode
		DBMDescription
		DBMIsDeleted
		DBMEffectiveDate
		DBMExpiryDate
		DBMRIModelType
		DBMFACPremiumType
		DBMClaimAllocationType
		DBMCurrencyID
		DBMCurrencyDescription
		DBMXOLClmRIModelID
		DBMXOLClmLimit
		DBMXOLCatRIModelID
		DBMXOLCatLimit
		DBMXOLCatReinstatements
	End Enum

	Public DBMMax As RIModelEnum = RIModelEnum.DBMXOLCatReinstatements
	
	' ***************************************************************** '
	'                      RI MODEL LINE ARRAY
	' ***************************************************************** '
	Public Enum RIModelLineEnum
		DBMLRIModelLineID
		DBMLRIModelID
		DBMLPriority
		DBMLNumberOfLines
		DBMLLineLimit
		DBMLTreatyID
		DBMLDescription
		DBMLSharePercent
		DBMLLowerLimit
		DBMLCedingrate
		DBMLTreatyTypeID
	End Enum
    Public DBMLMax As RIModelLineEnum = RIModelLineEnum.DBMLTreatyTypeID
	
	' ***************************************************************** '
	'                       TREATY PARTY ARRAY
	' ***************************************************************** '
	Public Enum TreatyPartyEnum
		DBTPTreatyPartyID
		DBTPPartyCnt
		DBTPResolvedName
		DBTPTreatyID
		DBTPSharePercent
		DBTPCommissionPercent
		DBTPDomiciledForTax
		DBTPTaxGroupID
		DBTPTaxGroup
	End Enum
    Public DBTPMax As TreatyPartyEnum = TreatyPartyEnum.DBTPTaxGroup
End Module