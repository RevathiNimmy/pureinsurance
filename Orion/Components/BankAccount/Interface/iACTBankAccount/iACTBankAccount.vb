Option Strict Off
Option Explicit On
Imports System
'Developer Guide no.129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 26/07/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTBankAccount"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACLabTitle1 As Integer = 102
	Public Const ACLabTitle2 As Integer = 103
	Public Const ACLabTitle3 As Integer = 104
	Public Const ACLabTitle4 As Integer = 105
	Public Const ACLabTitle5 As Integer = 106
	
	Public Const ACNextChequeNumber As Integer = 304
	Public Const ACReconciledDate As Integer = 305
	
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAccountHolderButton As Integer = 204
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	' Menus
	'individual data array elements for bank account rules
	Public Const ACBankAccountRulesID As Integer = 0
	Public Const ACBankAccountID As Integer = 1
	Public Const ACMediaTypeID As Integer = 2
	Public Const ACMediaDescription As Integer = 3
	Public Const ACMatchToTransdetail As Integer = 4
	Public Const ACMatchAccountCode As Integer = 5
	Public Const ACCodeIsMerchantNumber As Integer = 6
	Public Const ACMatchBatchNumber As Integer = 7
	Public Const ACBatchIsRemitCode As Integer = 8
	Public Const ACMatchChequeNumber As Integer = 9
	Public Const ACMatchAmount As Integer = 10
	Public Const ACMatchDate As Integer = 11
	Public Const ACSkipIfReasonNull As Integer = 12
	Public Const ACActive As Integer = 13
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iCompanyId As Integer
	
	Public g_iCurrencyID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Used for the purposes of defaulting when adding a bank
	Public Const ACPoundsSterling As String = "pounds sterling"
	
	Sub Main_Renamed()
		
    End Sub

End Module