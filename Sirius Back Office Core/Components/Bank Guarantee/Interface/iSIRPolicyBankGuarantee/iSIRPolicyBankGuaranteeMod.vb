Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' Module Name: MainModule
	'
	' Date: 31 July 2006
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMUPayNowOptions"
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bSIRFindBankGuarantee.Business
	
	'**********************************************
	' list view Bank Guarantee level constants
	'**********************************************
	Public Const kBankGuaranteeColHIndexBankNameId As Integer = 0
	Public Const kBankGuaranteeColHIndexBGRef As Integer = 1
	Public Const kBankGuaranteeColHIndexAvailableBalance As Integer = 2
	Public Const kBankGuaranteeColHIndexExpiryDate As Integer = 3
    Public Const kBankGuaranteeColHIndexDueDate As Integer = 4
    Public Const kBankGuaranteeColHIndexBankName As Integer = 5
	
	Public Enum ENBGPartyType
		Client
		agent
	End Enum
	
	Public Enum ENBankGuarantee
		RowIndex = 0
		BGId = 1
		BGRef = 2
		BGLimit = 3
		BankNameId = 4
		BGCurrencyId = 5
		PartyCnt = 6
		ExpiryDate = 7
		AvailableBal = 8
		IssueDate = 9
		Shortname = 10
        ResolvedName = 11
        BankName = 12
        uBoundBankGuarantee = ENBankGuarantee.BankName
	End Enum
End Module