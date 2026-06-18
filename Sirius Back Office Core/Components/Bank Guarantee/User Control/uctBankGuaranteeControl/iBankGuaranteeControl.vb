Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	'******************************************************************************
	' Module Name:      MainModule
	' History:          Created 06 July 2007
	' Description:      Main module containing public variable/constants.
	'******************************************************************************
	
	' Main public constant for all functions to identify which application this is
	Public Const ACApp As String = "uctPartyBankControl"
	
	' Constant for the functions to identify which class this is.
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
	
	
	'**********************************************
	' list view Party Bank level constants
	'**********************************************
	Public Const kBankGuaranteeColHIndexBankName As Integer = 0
	Public Const kBankGuaranteeColHIndexBGNo As Integer = 1
	Public Const kBankGuaranteeColHIndexBGLimit As Integer = 2
	Public Const kBankGuaranteeColHIndexAvailableBal As Integer = 3
	Public Const kBankGuaranteeColHIndexExpdate As Integer = 4
	Public Const kBankGuaranteeColHIndexParty As Integer = 5
	Public Const kBankGuaranteeColHIndexproduct As Integer = 5
	Public Const kBankGuaranteeColHIndexBranch As Integer = 6
	'Start - Sankar - Bank Guarantee Bug Fixing
	Public Const kBankGuaranteeColHIndexBGStatus As Integer = 7
	'End - Sankar - Bank Guarantee Bug Fixing
	
	'**********************************************
	' list view Party Bank level constants
	'**********************************************
	Public Const kPolicyDetailsColHIndexClientCode As Integer = 0
	Public Const kPolicyDetailsColHIndexClientName As Integer = 1
	Public Const kPolicyDetailsColHIndexInsuranceRef As Integer = 2
	Public Const kPolicyDetailsColHIndexAgentCode As Integer = 3
	Public Const kPolicyDetailsColHIndexBranch As Integer = 4
	Public Const kPolicyDetailsColHIndexProduct As Integer = 5
	Public Const kPolicyDetailsColHIndexAmount As Integer = 6
	Public Const kPolicyDetailsColHIndeCoverFrom As Integer = 7
	Public Const kPolicyDetailsColHIndeCoverTo As Integer = 8
	
	'**********************************************
	' list view Party Bank level column Headers
	'**********************************************
	Public Const kRegKeyConstLvwBankName As Integer = 300
	Public Const kRegKeyConstLvwBGNo As Integer = 301
	Public Const kRegKeyConstLvwBGLimit As Integer = 302
	Public Const kRegKeyConstLvwAvailableBal As Integer = 303
	Public Const kRegKeyConstLvwExpDate As Integer = 304
	'Public Const kRegKeyConstLvwParty = 305
	Public Const kRegKeyConstLvwProduct As Integer = 305
	Public Const kRegKeyConstLvwBranch As Integer = 306
	'Start - Sankar - Bank Guarantee Bug Fixing
	Public Const kRegKeyConstLvwBGStatus As Integer = 307
	'End - Sankar - Bank Guarantee Bug Fixing
	
	'**********************************************
	' list view Party Bank History level constants
	'**********************************************
	'Public Const kPtyBankHisColHIndexActionCode = 0
	'Public Const kPtyBankHisColHIndexDate = 1
	'Public Const kPtyBankHisColHIndexBankName = 2
	'Public Const kPtyBankHisColHIndexBranch = 3
	'Public Const kPtyBankHisColHIndexAccountName = 4
	'Public Const kPtyBankHisColHIndexSortCode = 5
	'Public Const kPtyBankHisColHIndexAccNum = 6
	'Public Const kPtyBankHisColHIndexUser = 7
	'Public Const kPtyBankHisColHIndexStreetName = 8
	'Public Const kPtyBankHisColHIndexPostCode = 9
	'
	'
	''**********************************************
	'' list view Party Bank History level column Headers
	''**********************************************
	'Public Const kRegKeyConstHisLvwActionCode = 305
	'Public Const kRegKeyConstHisLvwDate = 306
	'Public Const kRegKeyConstHisLvwBankName = 307
	'Public Const kRegKeyConstHisLvwBranch = 308
	'Public Const kRegKeyConstHisLvwAccountName = 309
	'Public Const kRegKeyConstHisLvwSortCode = 310
	'Public Const kRegKeyConstHisLvwAccNum = 311
	'Public Const kRegKeyConstHisLvwUser = 312
	'Public Const kRegKeyConstHisLvwStreetName = 313
	'Public Const kRegKeyConstHisLvwPostCode = 314
	
	
	'Form
	Public Const kTabPartyBank As Integer = 100
	Public Const kTabPartyBankHistory As Integer = 101
	
	'Button
	Public Const kAddButton As Integer = 200
	Public Const kEditButton As Integer = 201
	Public Const kDeleteButton As Integer = 202
	
	'Message
	'Public Const kRegKeyConstUnLinkMsg = 300
	'Public Const kRegKeyConstEditCaseDocument = 301
	'Public Const kRegKeyConstOpenCaseDocument = 302
	
	
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
		BGStatusId = 12
		CustodyBranchId = 13
		IssueDate = 14
		Branches = 15
		Products = 16
		IsDeleted = 17
		Shortname = 18
		Resolved_Name = 19
		uBoundBankGuarantee = ENBankGuarantee.Resolved_Name
	End Enum
	
	Public Enum BGStatus
		Active = 1
		Issued
		Invoked
		Expired
		Deleted
	End Enum
	
	'Start - Sankar - Bank Guarantee Bug Fixing
	Public Const kBGStatusActive As String = "Active"
	Public Const kBGStatusIssued As String = "Issued"
	Public Const kBGStatusInvoked As String = "Invoked"
	Public Const kBGStatusDeleted As String = "Deleted"
	Public Const kBGStatusExpired As String = "Expired"
	'End - Sankar - Bank Guarantee Bug Fixing
	
	Public Enum ENPolicyDetails
		ClientCode = 0
		ClientName = 1
		InsuranceRef = 2
		AgentCode
		Branch = 4
		product = 5
		Amount = 6
		CoverFrom = 7
		CoverTo = 8
		uBoundPolicyDetails = ENPolicyDetails.CoverTo
	End Enum
	
	Public Enum ENPMLookups
		Id = 0
		Description = 1
		uboundeNPMLookups = ENPMLookups.Description
	End Enum
	'Enum ENPartyBankHistory
	'
    'End Enum

End Module