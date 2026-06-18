Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	'*******************************************************************************
	' Module Name: MainModule
	'
	' Description: Main module containing public variable/constants.
	'
	' History: 020999 - Created
	'
	'          PW141204 - PN17440 - Replicate changes made for PN14670 in Broking
	'                     1.8.6.
	'*******************************************************************************
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTBankReconciliation"

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
	Public Const ACListTitle7 As Integer = 109
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACPayButton As Integer = 204
	Public Const ACMarkButton As Integer = 205
	Public Const ACDrillButton As Integer = 206
	Public Const ACReportButton As Integer = 207
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	Public Const ACNoMarked As Integer = 308
	
	' Menus
	
	' Constants for the search data array indexes.
	'EK 090200 Modifications for New Fields
	Public Const ACSourceID As Integer = 0
	Public Const ACPeriodName As Integer = 1
	Public Const ACClientCode As Integer = 2
	'eck 270401
	Public Const ACChequeNo As Integer = 3
	Public Const ACTransRef As Integer = 4
	Public Const ACTransDate As Integer = 5
	Public Const ACTransId As Integer = 6
	Public Const ACTransAmt As Integer = 7
	Public Const ACCurrencyId As Integer = 8
	Public Const ACCurrency As Integer = 9
	Public Const ACMarkedStatus As Integer = 10
	Public Const ACPeriodEndDate As Integer = 13
	' Icon
	Public Const ACIconCheck As String = "check"
	Public Const ACIconReconciled As String = "reconcile"
	Public Const ACIconBlank As Integer = 0
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	' was just first 500, now all records. PN17440
	Public Const ACMaxSearchDetails As Integer = gPMConstants.PMAllRecords '500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 3
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	'eck220500
	Public g_iUserID As Integer
	'
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.


    'changes refer vb6 code
    'Public g_oBusiness As bACTBankReconciliation.Business
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oBusiness As Object
    'eck030500
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPaymentGroups As Object
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As Object
	
	' Instance of SolutionConfig
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oSirConfig As bSIRSolutionConfig.Business
	'EK 130300
    ' Public instance of account business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oAccount As Object
	
	Public g_iCurrencyID As Integer
	
	Public Const g_kLockedByCurrentUser As Integer = 1
	Public Const g_kLockedByOtherUser As Integer = 2
    Public Const g_kNotLocked As Integer = 0
End Module