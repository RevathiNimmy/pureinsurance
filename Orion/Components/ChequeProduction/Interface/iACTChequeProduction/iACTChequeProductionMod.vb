Option Strict Off
Option Explicit On
Imports System
'developer guide no.129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 02/09/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTChequeProduction"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	Public Const ACListTitle1 As Integer = 102
	Public Const ACListTitle2 As Integer = 103
	Public Const ACListTitle3 As Integer = 104
	Public Const ACListTitle4 As Integer = 105
	Public Const ACListTitle5 As Integer = 106
	Public Const ACListTitle6 As Integer = 107
	Public Const ACListTitle7 As Integer = 108
	Public Const ACListTitle8 As Integer = 109
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACSelectButton As Integer = 204
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	Public Const ACNoSelected As Integer = 308
	
	' Menus
	' Constants for the lookup table array indexes.
	Public Const ACLDocumentType As Integer = 0
	Public Const ACLDocTypeGroup As Integer = 1
	Public Const ACLMax As Integer = 1
	
	' Constants for data array indexes.
	Public Const ACIBankCode As Integer = 0
	Public Const ACIChequeID As Integer = 1
	Public Const ACITransactionID As Integer = 2
	Public Const ACITransactionDate As Integer = 3
	Public Const ACIReference As Integer = 4
	Public Const ACIAmount As Integer = 5
	Public Const ACICurrencyID As Integer = 6
	Public Const ACIChequeNumber As Integer = 7
	Public Const ACIAccountID As Integer = 8
	Public Const ACIAccountName As Integer = 9
	Public Const ACIPersonName As Integer = 10
	Public Const ACIAddress1 As Integer = 11
	Public Const ACIAddress2 As Integer = 12
	Public Const ACIAddress3 As Integer = 13
	Public Const ACIAddress4 As Integer = 14
	Public Const ACIPostalCode As Integer = 15
	Public Const ACIDocumentRef As Integer = 16
	Public Const ACIPartyCnt As Integer = 17
	Public Const ACIOurRef As Integer = 18
	Public Const ACISourceID As Integer = 19
	Public Const ACISourceDescription As Integer = 20
	Public Const ACIBankID As Integer = 21
	
	'Cheque Production System Option
	Public Const ACChequeProductionNotInstalled As String = "0"
	Public Const ACChequeProductionCM As String = "1"
	Public Const ACChequeProductionInHouse As String = "2"
	Public Const ACChequeProductionExport As String = "3"
	
	'BankWise Starting Cheque Number
	Public Const ACBankID As Integer = 0
	Public Const ACBankCode As Integer = 1
	Public Const ACStartChequeNumber As Integer = 2
	
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 3
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iUserID As Integer
	Public g_sUserName As String = ""
	' Company ID
	Public g_iCompanyID As Integer
	'
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
	
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bACTChequeProduction.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As bPMUser.Business
	' Instance of SolutionConfig
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oSirConfig As bSIRSolutionConfig.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_bPrinted As Boolean
    
End Module