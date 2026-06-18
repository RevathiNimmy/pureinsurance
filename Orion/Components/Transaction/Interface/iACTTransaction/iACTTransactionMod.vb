Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 11th July 1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTTransaction"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	Public Const ACDocumentDetailsTitle As Integer = 100
	Public Const ACMainTabTitle0 As Integer = 101
	Public Const ACMainTabTitle1 As Integer = 102
	Public Const ACMainTabTitle2 As Integer = 103
	Public Const ACCommentCaption As Integer = 104
	Public Const ACCurrencyBaseXrateCaption As Integer = 105
	Public Const ACCurrencyAmountCaption As Integer = 106
	Public Const ACAmountCaption As Integer = 107
	Public Const ACCurrencyCaption As Integer = 108
	Public Const ACAccountCodeCaption As Integer = 109
	
	'Public Const ACClientCaption = 110
	'Public Const ACAgentCaption = 111
	'Public Const ACDepartmentCaption = 112
	'Public Const ACProductCaption = 113
	'Public Const ACContractCaption = 114
	'Public Const ACProjectCaption = 115
	Public Const ACSpareCaption As Integer = 110
	Public Const ACDepartmentCaption As Integer = 111
	Public Const ACPurchaseInvoiceNoCaption As Integer = 112
	Public Const ACPurchaseOrderNoCaption As Integer = 113
	Public Const ACOperatorIDCaption As Integer = 114
	Public Const ACInsuranceRefCaption As Integer = 115
	
	Public Const ACRefUnitsCaption As Integer = 116
	Public Const ACRefQuantityCaption As Integer = 117
	Public Const ACRefAmountCaption As Integer = 118
	Public Const ACRefDateCaption As Integer = 119
	Public Const ACDocumentRefCaption As Integer = 120
	Public Const ACDocumentDateCaption As Integer = 121
	
	Public Const ACDocumentListTitle As Integer = 130
	Public Const ACDocBalanceCaption As Integer = 131
	
	Public Const ACCreditCaption As Integer = 140
	Public Const ACDebitCaption As Integer = 141
	Public Const ACUnderwritingYear As Integer = 142
	
	' Buttons
	Public Const ACCommitCaption As Integer = 200
	Public Const ACNavigateCaption As Integer = 201
	Public Const ACHelpCaption As Integer = 202
	Public Const ACCancelCaption As Integer = 203
	Public Const ACOKCaption As Integer = 204
	Public Const ACRemoveCaption As Integer = 205
	Public Const ACEditCaption As Integer = 206
	Public Const ACAddCaption As Integer = 207
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACZeroAmountTitle As Integer = 304
	Public Const ACZeroAmount As Integer = 305
	Public Const ACNoEditDocument As Integer = 306
	' Menus
	
	
	' Constants for the List data array indicies.
	'
	
	Public Const ACITransdetailID As Integer = 0
	Public Const ACIAccountID As Integer = 1
	Public Const ACIPostingstatusID As Integer = 2
	Public Const ACICompanyID As Integer = 3
	Public Const ACICurrencyID As Integer = 4
	Public Const ACIPeriodID As Integer = 5
	Public Const ACIDocumentID As Integer = 6
	Public Const ACIDocumentSequence As Integer = 7
	Public Const ACIAccountingDate As Integer = 8
	Public Const ACIAmount As Integer = 9
	Public Const ACIFullyMatched As Integer = 10
	Public Const ACICurrencyAmount As Integer = 11
	Public Const ACICurrencyBaseXrate As Integer = 12
	Public Const ACIEuro As Integer = 13
	Public Const ACIEuroAmount As Integer = 14
	Public Const ACIEuroBaseXrate As Integer = 15
	Public Const ACIComment As Integer = 16
	Public Const ACIInsuranceRef As Integer = 17
	Public Const ACIOperatorID As Integer = 18
	Public Const ACIPurchaseOrderNo As Integer = 19
	Public Const ACIPurchaseInvoiceNo As Integer = 20
	Public Const ACIDepartment As Integer = 21
	Public Const ACISpare As Integer = 22
	Public Const ACIRefDate As Integer = 23
	Public Const ACIRefAmount As Integer = 24
	Public Const ACIRefQuantity As Integer = 25
	Public Const ACIRefUnits As Integer = 26
	'EK 6/1/00 New Contstants for extra Euro Fields
	Public Const ACIEuroCCyXrate As Integer = 27
	Public Const ACICCyAmountUnRounded As Integer = 28
	Public Const ACIBaseAmountUnRounded As Integer = 29
	' CTAF 140300
	Public Const ACIDepartmentID As Integer = 30
	
	'Tomo190902
	Public Const ACDeleted As Integer = 31
	Public Const ACIUnderwritingYearID As Integer = 32
	
	Public Const ACI_ArraySize As Integer = 33
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
	Public g_iUserID As Integer
	Public g_sUserName As String = ""
	Public g_sPassword As String = ""
	Public g_iCurrencyID As Integer
	Public g_iLogLevel As Integer
	
	' Orion Company ID
	Public g_iCompanyID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	'Product Family Name for Help

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	' CTAF 150300 - Registry setting
	Public Const ACLastDepartment As String = "LastDepartment"
	
	Public Const ScreenHelpID1 As Integer = 4000
	Public Const ScreenHelpID2 As Integer = 36000
	
	Public Const kSystemOptionUWYearMandatory As Integer = 5012
	

	Public Sub Main()
		
	End Sub

	Sub New()
		Main()
	End Sub
	Sub JustForInvokeMain()
	End Sub
End Module