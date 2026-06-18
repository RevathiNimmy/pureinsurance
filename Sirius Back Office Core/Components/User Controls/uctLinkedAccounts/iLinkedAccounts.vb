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
	Public Const ACApp As String = "uctLinkedAccounts"
	
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
	Public Const kCashDepositCDNumber As Integer = 1
	Public Const kCashDepositCDBranch As Integer = 2
	Public Const kCashDepositCDProduct As Integer = 3
	Public Const kCashDepositCDDateCreated As Integer = 4
	Public Const kCashDepositCDUserName As Integer = 5
	
	'**********************************************
	' list view Party Bank level column Headers
	'**********************************************
	Public Const kRegKeyConstLvwCDNumber As Integer = 300
	Public Const kRegKeyConstLvwBranch As Integer = 301
	Public Const kRegKeyConstLvwProduct As Integer = 302
	Public Const kRegKeyConstLvwDateCreated As Integer = 303
	Public Const kRegKeyConstLvwUserName As Integer = 304
	
	Public Enum ENCashDeposit
		CDNumber = 0
		Branch = 1
		Product = 2
		DateCreated = 3
		UserName = 4
		CashDepositID = 5
	End Enum
	
	Public Enum ENCDLinkedAccountDetail
		CashDeposit_ID = 0
		Account_ID = 1
		Party_ID = 2
		Bank_Name = 3
		CashDeposit_Ref = 4
		Amount = 5
		Available_Balance = 6
		PartyName = 7
		Product = 8
		Branch = 9
		Is_SinglePolicy = 10
		Is_Deleted = 11
		UserName = 12
		User_Id = 13
		Date_Created = 14
	End Enum
	
	Public Enum ENPMLookups
		Id = 0
		Description = 1
		uboundeNPMLookups = ENPMLookups.Description
	End Enum
End Module