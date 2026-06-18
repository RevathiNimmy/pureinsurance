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
	Public Const ACApp As String = "uctCashDepositControl"
	
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
	' list view Cash Deposit constants
	'**********************************************
	Public Const kCashDepositColHIndexBankName As Integer = 0
	Public Const kCashDepositColHIndexCDNo As Integer = 1
	Public Const kCashDepositColHIndexAmount As Integer = 2
	Public Const kCashDepositColHIndexAvailableBal As Integer = 3
	Public Const kCashDepositColHIndexParty As Integer = 4
	Public Const kCashDepositColHIndexProduct As Integer = 5
	Public Const kCashDepositColHIndexBranch As Integer = 6
	Public Const kCashDepositColHIndexSinglePolicy As Integer = 7
	Public Const kCashDepositColHIndexUserName As Integer = 8
	
	
	''**********************************************
	'' list view Cash Deposit constants
	''**********************************************
	'Public Const kPolicyDetailsColHIndexClientCode = 0
	'Public Const kPolicyDetailsColHIndexClientName = 1
	'Public Const kPolicyDetailsColHIndexInsuranceRef = 2
	'Public Const kPolicyDetailsColHIndexAgentCode = 3
	'Public Const kPolicyDetailsColHIndexBranch = 4
	'Public Const kPolicyDetailsColHIndexProduct = 5
	'Public Const kPolicyDetailsColHIndexAmount = 6
	'Public Const kPolicyDetailsColHIndeCoverFrom = 7
	'Public Const kPolicyDetailsColHIndeCoverTo = 8
	
	'**********************************************
	' list view Cash Deposit column Headers
	'**********************************************
	Public Const kRegKeyConstLvwBankName As Integer = 100
	Public Const kRegKeyConstLvwCDNo As Integer = 101
	Public Const kRegKeyConstLvwAmount As Integer = 102
	Public Const kRegKeyConstLvwAvailableBal As Integer = 103
	Public Const kRegKeyConstLvwParty As Integer = 104
	Public Const kRegKeyConstLvwProduct As Integer = 105
	Public Const kRegKeyConstLvwBranch As Integer = 106
	Public Const kRegKeyConstLvwSinglePolicy As Integer = 107
	Public Const kRegKeyConstLvwUserName As Integer = 108
	
	'Messages
	Public Const kCashDepositSelectClientOrAgent As Integer = 300
	Public Const kCashDepositAddTaskDescription As Integer = 301
	
	
	'Form
	Public Const kCashDepositAddTask As Integer = 400
	Public Const kCashDepositApplyButton As Integer = 401
	Public Const kCashDepositOkButton As Integer = 402
	Public Const kCashDepositCancelButton As Integer = 403
	Public Const kCashDepositCDNumber As Integer = 404
	Public Const kCashDepositSinglePolicyLock As Integer = 405
	
	'Button
	Public Const kCashDepositAddButton As Integer = 200
	Public Const kCashDepositEditButton As Integer = 201
	Public Const kCashDepositViewButton As Integer = 202
	
	Public Enum ENCashDepositDBDetails
		RowIndex = 1
		CashDepositID = 0
		AccountId = 1
		PartyId = 2
		BankName = 3
		CashDepositRef = 4
		Amount = 5
		AvailableBal = 6
		PartyName = 7
		Products = 8
		Branches = 9
		Is_SinglePolicy = 10
		Is_Deleted = 11
		UserName = 12
		UserID
		DateCreated
		'MaxCashDepositDBData = UserName
	End Enum
	
	Public Enum BGStatus
		Active = 1
		Issued
		Invoked
		Expired
		Deleted
	End Enum
	
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


End Module