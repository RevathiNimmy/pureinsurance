Option Strict Off
Option Explicit On
Imports System
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
	
	
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.

	Public g_oBusiness As bSIRBankGuarantee.Business


    'developer guide no. changed as per the dll name
    Public g_oBackofficelink As bBackOfficeLink.bBOLink
	
	Public g_iUserID As Integer
	Public g_oPMUser As Object
	
	
	'**********************************************
	' list view Party Bank level constants
	'**********************************************
	Public Const kBankGuaranteeColHIndexBankName As Integer = 0
	Public Const kBankGuaranteeColHIndexBGNo As Integer = 1
	Public Const kBankGuaranteeColHIndexBGLimit As Integer = 2
	Public Const kBankGuaranteeColHIndexAvailableBal As Integer = 3
	Public Const kBankGuaranteeColHIndexExpdate As Integer = 4
	Public Const kBankGuaranteeColHIndexParty As Integer = 5
	Public Const kBankGuaranteeColHIndexproduct As Integer = 6
	Public Const kBankGuaranteeColHIndexBranch As Integer = 7
	
	'**********************************************
	' list view Party Bank level column Headers
	'**********************************************
	Public Const kRegKeyConstLvwBankName As Integer = 300
	Public Const kRegKeyConstLvwBGNo As Integer = 301
	Public Const kRegKeyConstLvwBGLimit As Integer = 302
	Public Const kRegKeyConstLvwAvailableBal As Integer = 303
	Public Const kRegKeyConstLvwExpDate As Integer = 304
	Public Const kRegKeyConstLvwParty As Integer = 305
	Public Const kRegKeyConstLvwProduct As Integer = 306
	Public Const kRegKeyConstLvwBranch As Integer = 307
	
	
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
		ConversionRate = 10
		EffectiveDate = 11
		ExpiryDate = 16
		IsPolicyLock = 17
		Branches = 18
		Products = 19
		IsDeleted = 20
		uBoundBankGuarantee = ENBankGuarantee.IsDeleted
	End Enum
	
	Public Enum ENPartyBankHistory
		ActionCode = 0
		PartyBankId = 1
		AccountID = 2
		BankPaymentTypeId = 3
		BankAccountTypeId = 4
		AccountHolderName = 5
		AccountNumber = 6
		BankNameId = 7
		BankBranch = 8
		BankBranchCode = 9
		BankAdd1 = 10
		BankAdd2 = 11
		BankAdd3 = 12
		BankTown = 13
		BankPCode = 14
		BankRegion = 15
		BankCountry = 16
		CCNum = 17
		CCStartDate = 18
		CCExpiryDate = 19
		CCIssueNum = 20
		CCPIN = 21
		IsRegistered = 22
		CCAdd1 = 23
		CCAdd2 = 24
		CCAdd3 = 25
		CCTown = 26
		CCPCode = 27
		CCCountry = 28
		UserID = 29
		DateModified = 30
		uBoundPartyBankHistory = ENPartyBankHistory.DateModified
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