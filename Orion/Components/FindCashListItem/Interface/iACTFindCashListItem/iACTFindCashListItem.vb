Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'developer guide no.129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 1st September 1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTFindCashListItem"
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 700
	Public Const ACCancelDetails As Integer = 701
	Public Const ACBusinessFailTitle As Integer = 702
	Public Const ACBusinessFail As Integer = 703
	
	Public Const ACClearDetailsTitle As Integer = 704
	Public Const ACClearDetails As Integer = 705
	Public Const ACStatusSearching As Integer = 706
	Public Const ACStatusFound As Integer = 707
	
	Public Const ACAllComboEntry As Integer = 800
	
	' Menus
	
	' Constants for the receipt search data array indexes.
	Public Const ACReceiptCashListItemId As Integer = 0
	Public Const ACReceiptTransactionDate As Integer = 1
	Public Const ACReceiptMediaType As Integer = 2
	Public Const ACReceiptMediaReference As Integer = 3
	Public Const ACReceiptTheirRefernce As Integer = 4
	Public Const ACReceiptType As Integer = 5
	Public Const ACReceiptAccount As Integer = 6
	Public Const ACReceiptAmount As Integer = 7
	Public Const ACReceiptAllocationStatus As Integer = 8
	Public Const ACPMUserReceipt As Integer = 9
	Public Const ACReceiptCashListId As Integer = 10
	
	
	' Constants for the Payment search data array indexes.
	Public Const ACPaymentCashListItemId As Integer = 0
	Public Const ACPaymentName As Integer = 1
	'IR 21 Jul 2003 - CQ1605 (Show a/c short name, not id)
	Public Const ACPaymentAccountName As Integer = 15 '2
	Public Const ACPaymentTypeDesc As Integer = 3
	Public Const ACPaymentTypeCode As Integer = 4
	Public Const ACPaymentMethod As Integer = 5
	Public Const ACPaymentMediaTypeID As Integer = 6
	Public Const ACPaymentMediaRef As Integer = 7
	Public Const ACPaymentAmount As Integer = 8
	Public Const ACPaymentStatus As Integer = 9
	Public Const ACPaymentStatusCode As Integer = 10
	Public Const ACPaymentDatePresented As Integer = 11
	Public Const ACPaymentBatchRef As Integer = 12
	Public Const ACPMUserPayment As Integer = 13
	Public Const ACPaymentCashListID As Integer = 14
	Public Const ACPaymentIsStoppable As Integer = 15
	' RVH 10/12/2003 CQ3623
	Public Const ACPaymentIsManual As Integer = 16
	
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
	
	'sw added payment maintenance 04-11-2002
	'required for generic form to cope with payments and receipts
	Public Const ACPaymentType As Integer = 1
	Public Const ACReceiptingType As Integer = 2
	
	'sw payment maintenance define Media Validation Codes
	Public Const ACMediaTypeCash As String = "CASH"
	Public Const ACMediaTypeCreditCard As String = "CC"
	Public Const ACMediaTypeBasic As String = "BASIC"
	Public Const ACMediaTypeBank As String = "BANK"
	
	'define payment status code Issued
	'sw payment maintenance 14-11-2002
	Public Const ACStatusIssued As String = "ISS"
	Public Const ACStatusPresented As String = "PRES"
	Public Const ACStatusCancelled As String = "CAN"
	Public Const ACStatusStopped As String = "STOPPED"
	Public Const ACStatusStopRequested As String = "STOPREQ"
	
	'define payment type codes
	Public Const ACPaymentTypeCommission As String = "COMM"
	Public Const ACPaymentTypeDebtorRefund As String = "REFUND"
	Public Const ACPaymentTypeClaim As String = "CLM"
	Public Const ACPaymentTypeExpense As String = "EXP"
	
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_iCompanyID As Integer
	
	' Username.
	Public g_sUsername As New FixedLengthString(12)
	Public g_iUserID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As bACTFindCashListItem.Form
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oPMUser As bPMUser.Business
	
	Public g_iCurrencyID As Integer
	'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	Public Const ScreenHelpID As Integer = 51000
	
	'CHECK_PSL put this into gACTLibrary
	Public Const ACTViewBatch As String = "viewbatch"
	Public Const ACTKeyBatchID As String = "BatchID"
	Public Const ACTKeyBatchReference As String = "BatchReference"
	
	Public Const ACClaimModeDebtAllocation As Integer = 1 ' MEvans : 24-06-2004 : CQ4740
	Public Const ACReceiptTypeClaimRecovery As String = "CLMREC" ' MEvans : 24-06-2004 : CQ4740
	

	Public Sub Main()
		
	End Sub
    Sub New()
        Main()
    End Sub
	Sub JustForInvokeMain()
	End Sub
End Module