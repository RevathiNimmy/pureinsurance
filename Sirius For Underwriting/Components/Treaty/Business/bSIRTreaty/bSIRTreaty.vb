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
	Public Const ACApp As String = "bSIRTreatyParty"
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "MainModule"

	' ***************************************************************** '
	'                          TREATY ARRAY
	' ***************************************************************** '
	Public Enum TreatyEnum
		DBTTreatyID
		DBTCode
		DBTDescription
		DBTIsDeleted
		DBTEffectiveDate
		DBTExpiryDate
		DBTAgreementCode
		DBTReinsuranceTypeID
		DBTReinsuranceType
		DBTReplacesTreatyID
		DBTReplacesTreaty
	End Enum
    Public DBTMax As TreatyEnum = TreatyEnum.DBTReplacesTreaty
	
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
        DBTPIsReinsurerApproved
	End Enum
    Public DBTPMax As TreatyPartyEnum = TreatyPartyEnum.DBTPIsReinsurerApproved
End Module