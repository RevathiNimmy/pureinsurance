Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	'******************************************************************************
	' Module Name:      MainModule
	' History:          Created 06 July 2007
	' Description:      Main module containing public variable/constants.
	'******************************************************************************
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
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
    'developer guide no. 107
    '**********************************************
    ' list view Party Bank level constants
    '**********************************************
	Public Const kPtyBankColHIndexPaymentType As Integer = 0
	Public Const kPtyBankColHIndexAccountType As Integer = 1
	Public Const kPtyBankColHIndexAccHolderName As Integer = 2
	Public Const kPtyBankColHIndexAccNum As Integer = 3
    Public Const kPtyBankColHIndexBankBranchCode As Integer = 4
    Public Const kPtyBankColHIndexBIC As Integer = 5
    Public Const kPtyBankColHIndexIBAN As Integer = 6
    Public Const kPtyBankColHIndexBankBranch As Integer = 7
    Public Const kPtyBankColHIndexBankName As Integer = 8
    Public Const kPtyBankColHIndexCCExpiryDate As Integer = 9
    Public Const kPtyBankColHIndexCCStartDate As Integer = 10
    Public Const kPtyBankColHIndexCCIssueNumber As Integer = 11
    Public Const kPtyBankColHIndexCCManualAuth As Integer = 12
    Public Const kPtyBankColHIndexCCAuthCode As Integer = 13
    Public Const kPtyBankColHIndexNoStreet As Integer = 14
    Public Const kPtyBankColHIndexLocality As Integer = 15
    Public Const kPtyBankColHIndexPostTown As Integer = 16
    Public Const kPtyBankColHIndexCounty As Integer = 17
    Public Const kPtyBankColHIndexPostCode As Integer = 18
    Public Const kPtyBankColHIndexCountry As Integer = 19
    Public Const kPtyBankColHIndexCCIsDefault As Integer = 20
    '**********************************************
    ' list view Party Bank level column Headers
    '**********************************************
    Public Const kRegKeyConstLvwAccHolderName As Integer = 300
	Public Const kRegKeyConstLvwAccNum As Integer = 301
	Public Const kRegKeyConstLvwBankName As Integer = 302
	Public Const kRegKeyConstLvwPaymentType As Integer = 303
	Public Const kRegKeyConstLvwAccountType As Integer = 304
	
	
	'**********************************************
	' list view Party Bank History level constants
	'**********************************************
	Public Const kPtyBankHisColHIndexActionCode As Integer = 0
	Public Const kPtyBankHisColHIndexDate As Integer = 1
	Public Const kPtyBankHisColHIndexBankName As Integer = 2
	Public Const kPtyBankHisColHIndexBranch As Integer = 3
	Public Const kPtyBankHisColHIndexAccountName As Integer = 4
	Public Const kPtyBankHisColHIndexSortCode As Integer = 5
    Public Const kPtyBankHisColHIndexAccNum As Integer = 6
    Public Const kPtyBankHisColHIndexBIC As Integer = 7
    Public Const kPtyBankHisColHIndexIBAN As Integer = 8
    Public Const kPtyBankHisColHIndexUser As Integer = 9
    Public Const kPtyBankHisColHIndexStreetName As Integer = 10
    Public Const kPtyBankHisColHIndexPostCode As Integer = 11
	
	'Start (Sriram P)61128
	Public Const ACCancelDetailsTitle As Integer = 400
	Public Const ACCancelDetails As Integer = 401
	'End (Sriram P)61128
	'**********************************************
	' list view Party Bank History level column Headers
	'**********************************************
	Public Const kRegKeyConstHisLvwActionCode As Integer = 305
	Public Const kRegKeyConstHisLvwDate As Integer = 306
	Public Const kRegKeyConstHisLvwBankName As Integer = 307
	Public Const kRegKeyConstHisLvwBranch As Integer = 308
	Public Const kRegKeyConstHisLvwAccountName As Integer = 309
	Public Const kRegKeyConstHisLvwSortCode As Integer = 310
	Public Const kRegKeyConstHisLvwAccNum As Integer = 311
	Public Const kRegKeyConstHisLvwUser As Integer = 312
	Public Const kRegKeyConstHisLvwStreetName As Integer = 313
    Public Const kRegKeyConstHisLvwPostCode As Integer = 314

	'New added for WR-22
	Public Const kRegKeyConstHisLvwLocality As Integer = 315
	Public Const kRegKeyConstHisLvwPostTown As Integer = 316
	Public Const kRegKeyConstHisLvwCounty As Integer = 317
	Public Const kRegKeyConstHisLvwCountry As Integer = 318
	Public Const kRegKeyConstHisLvwExpiryDate As Integer = 319
	Public Const kRegKeyConstHisLvwStartDate As Integer = 320
	Public Const kRegKeyConstHisLvwIssueNumber As Integer = 321
	Public Const kRegKeyConstHisLvwManualAuth As Integer = 322
    Public Const kRegKeyConstHisLvwAuthCode As Integer = 323
    Public Const kRegKeyConstHisLvwBIC As Integer = 324
    Public Const kRegKeyConstHisLvwIBAN As Integer = 325

    'Form
    Public Const kTabPartyBank As Integer = 100
	Public Const kTabPartyBankHistory As Integer = 101
	
	'Button
	Public Const kAddButton As Integer = 200
	Public Const kEditButton As Integer = 201
	Public Const kDeleteButton As Integer = 202
	
    Public Enum ENPartyBank
        RowStatus = 0
        RowIndex = 1
        PartyBankId = 2
        IsBank = 3
        AccountId = 4
        BankPaymentTypeId = 5
        BankAccountTypeId = 6
        AccountHolderName = 7
        AccountNumber = 8
        BankNameId = 9
        BankBranch = 10
        BankBranchCode = 11
        BankAdd1 = 12
        BankAdd2 = 13
        BankAdd3 = 14
        BankTown = 15
        BankPCode = 16
        BankRegion = 17
        BankCountry = 18
        CCNum = 19
        CCStartDate = 20
        CCExpiryDate = 21
        CCIssueNum = 22
        CCPIN = 23
        IsRegistered = 24
        CCAdd1 = 25
        CCAdd2 = 26
        CCAdd3 = 27
        CCTown = 28
        CCPCode = 29
        CCCountry = 30
        IsDeleted = 31
        CCNameOnCard = 32
        CCManualAuthorisationNum = 33
        PFLINKEXISTS = 34
        CLILINKEXISTS = 35
        CPLINKEXISTS = 36
        BIC = 37
        IBAN = 38
        CCIsDefault = 39
        uBoundPartyBank = ENPartyBank.CCIsDefault
    End Enum
	
	Public Enum ENPartyBankHistory
		ActionCode = 0
		PartyBankId = 1
		AccountId = 2
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
		CCNameOnCard = 31
		CCManualAuthorisationNum = 32
        User = 33
        BIC
        IBAN
        CCIsDefault
        uBoundPartyBankHistory = ENPartyBankHistory.CCIsDefault
    End Enum
	
	Public Enum ENPMLookups
		Id = 0
		Description = 1
		uboundeNPMLookups = ENPMLookups.Description
	End Enum
End Module