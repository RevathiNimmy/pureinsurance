Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 15/07/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:Pandu
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iSIRFindBankGuarantee"
	
	' String Resources
	'DC130202
	Public Const AC_RES_INSURER As Integer = 121
	Public Const AC_RES_ACCOUNTEXEC As Integer = 122
	Public Const AC_RES_CLIENTNAME As Integer = 123
	Public Const AC_RES_VEHICLEREG As Integer = 124
	
	Public Const AC_RES_LISTTITLE11 As Integer = 125
	Public Const AC_RES_LISTTITLE12 As Integer = 126
	Public Const AC_RES_LISTTITLE13 As Integer = 127
	
	'Date Formats
	Public Const ACDateConversion As String = "dd/mm/yyyy"
	Public Const ACDateDispaly As String = "long date"
	Public Const ACShortDate As String = "short date"
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	Public Const ACFindNowButton As Integer = 204
	Public Const ACNewSearchButton As Integer = 205
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	
	Public Const ACInvalidDateMsg As Integer = 400 'Invalid Date Entered
	Public Const ACDateDiffError As Integer = 401 'Date Diff Error
	
	Public Const ACClosedBranchError As Integer = 402
	Public Const ACClosedBranchError_Payments As Integer = 403
	Public Const ACClosedBranchError_Recoveries As Integer = 404
	
	Public Const ACLookupFailTitle As Integer = 308
	Public Const ACLookupFail As Integer = 309
	
	'Constants for UnderWriting and Broking
	Public Const ACBroking As String = "A"
	Public Const ACUnderWriting As String = "U"
	
	
	' Constants for the search data array indexes.
	Public Const ACIInsuranceid As Integer = 0
	Public Const ACIClaimCnt As Integer = 1
	Public Const ACIClaimType As Integer = 2
	Public Const ACIClaimRef As Integer = 3
	Public Const ACIInsuranceRef As Integer = 4
	Public Const ACIUClientCode As Integer = 5
	Public Const ACIUProductCode As Integer = 6
	Public Const ACIULossDate As Integer = 7
	Public Const ACIUPolicyHolder As Integer = 8
	Public Const ACIUClaimStatus As Integer = 9
	
	'Constants for Broking
	Public Const ACIBLossDate As Integer = 5
	Public Const ACIBPolicyHolder As Integer = 6
	
	Public Const ACIRiskTypeID As Integer = 9
	'DC250501 for Broking
	Public Const ACIClientCode As Integer = 17
	'DC250501
	
	Public Const ACIInsurerCode As Integer = 20 '2005 add insurer code
	Public Const ACIBranchClosed As Integer = 21
	Public Const ACIClaimsAllowed As Integer = 22
	
	'DC130202 for Broking
	Public Const ACIDescription As Integer = 2
	Public Const ACIStatusId As Integer = 7
	Public Const ACIHandler As Integer = 8
	'DC130202
	
	'S4B Claims Enhancements R&D 2005
	Public Const ACIBClientResolvedName As Integer = 21
	Public Const ACIBAccountExecutive As Integer = 22
	Public Const ACIBVehicleRegistration As Integer = 23
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bSIRFindBankGuarantee.Business
	
    ' MKW 190503 PN2032 START
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As Object
	
	Private m_lReturn As Integer
	' MKW 190503 PN2032 END
	
	
	'**********************************************
	' list view Party Bank level constants
	'**********************************************
	Public Const kBankGuaranteeColHIndexBankName As Integer = 0
	Public Const kBankGuaranteeColHIndexBGNo As Integer = 1
	Public Const kBankGuaranteeColHIndexBGLimit As Integer = 2
	Public Const kBankGuaranteeColHIndexAvailableBal As Integer = 3
	Public Const kBankGuaranteeColHIndexExpdate As Integer = 4
	Public Const kBankGuaranteeColHIndexClientCode As Integer = 5
	Public Const kBankGuaranteeColHIndexClientName As Integer = 6
	Public Const kBankGuaranteeColHIndexproduct As Integer = 7
	Public Const kBankGuaranteeColHIndexBranch As Integer = 8
	
	'**********************************************
	' list view Party Bank level column Headers
	'**********************************************
	Public Const kRegKeyConstLvwBankName As Integer = 300
	Public Const kRegKeyConstLvwBGNo As Integer = 301
	Public Const kRegKeyConstLvwBGLimit As Integer = 302
	Public Const kRegKeyConstLvwAvailableBal As Integer = 303
	Public Const kRegKeyConstLvwExpDate As Integer = 304
	Public Const kRegKeyConstLvwClientCode As Integer = 306
	Public Const kRegKeyConstLvwClientName As Integer = 305
	Public Const kRegKeyConstLvwProduct As Integer = 307
	Public Const kRegKeyConstLvwBranch As Integer = 308
	
	Public Enum ENBankGuarantee
		RowStatus = 0
		RowIndex = 1
		BGId = 2
		BankNameId = 3
		BankBranch = 4
		PartyCnt = 5
		BGRef = 6
		BGCurrencyId = 7
		BGLimit = 8
		AvailableBal = 9
		ExpiryDate = 10
		IsPolicyLock = 11
		Branches = 12 ' Sankar - Modified 13 to 12
		Products = 13 ' Sankar - Modified 12 to 13
		IsDeleted = 14
		ResolvedName = 15
		ShortName = 16
		BGStatusDesc = 17
		BankName = 18
		uBoundBankGuarantee = ENBankGuarantee.BankName
	End Enum
	Public Enum ENPMLookups
		Id = 0
		Description = 1
		uboundeNPMLookups = ENPMLookups.Description
	End Enum

	
	Sub Main_Renamed()
	End Sub
End Module