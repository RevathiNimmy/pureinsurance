Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	' RAW 13/01/2003 : PS187 : replaced spu_ACT_Do_InsurerPayments with spu_ACT_Do_InsurerPayments_All
	
	' This App
	Public Const ACApp As String = "bACTDocumentReversal"
	
	
	' Constants for the List data array indicies.
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
	Public Const ACIBaseAmountUnRounded As Integer = 10
	Public Const ACIFullyMatched As Integer = 11
	Public Const ACICurrencyAmount As Integer = 12
	Public Const ACICCyAmountUnRounded As Integer = 13
	Public Const ACICurrencyBaseXrate As Integer = 14
	Public Const ACIEuro As Integer = 15
	Public Const ACIEuroAmount As Integer = 16
	Public Const ACIEuroBaseXrate As Integer = 17
	Public Const ACIEuroCCyXrate As Integer = 18
	Public Const ACIComment As Integer = 19
	Public Const ACIInsuranceRef As Integer = 20
	Public Const ACIOperatorID As Integer = 21
	Public Const ACIPurchaseOrderNo As Integer = 22
	Public Const ACIPurchaseInvoiceNo As Integer = 23
	Public Const ACIDepartment As Integer = 24
	Public Const ACISpare As Integer = 25
	Public Const ACIRefDate As Integer = 26
	Public Const ACIRefAmount As Integer = 27
	Public Const ACIRefQuantity As Integer = 28
	Public Const ACIRefUnits As Integer = 29
	Public Const ACIOSBaseAmount As Integer = 30
	Public Const ACIOSCurrencyAmount As Integer = 31
	Public Const ACITransdetailTypeID As Integer = 32
	Public Const ACIMAXCOLUMN As Integer = 32
	
	' This Class
	Private Const ACClass As String = "MainModule"
	
	
	'************ MKR 05/10/2004 PN 15368     --Start
	'Constants for the Period Details data array indicies.
	Public Const ACIPDPeriodID As Integer = 0
	Public Const ACIPDCompanyID As Integer = 1
	Public Const ACIPDSubBranchID As Integer = 2
	Public Const ACIPDYearName As Integer = 3
	Public Const ACIPDPeriodName As Integer = 4
	Public Const ACIPDPeriodEndDate As Integer = 5
	Public Const ACIPDPeriodEndComplete As Integer = 6
	'************ --End
	
	'Constant for Credit Control option
	Public Const kSystemOptionCreditControlEnabled As Integer = 4
	
	
	
	' Log Level
	
	
	' SQL
	' RAW 13/01/2003 : PS187 : changed procedure name from spu_ACT_Do_InsurerPayments and added extra argument
    'developer guide no. 39
    Public Const ACInsurerPaymentsSQL As String = "spu_ACT_Do_InsurerPayments_All"
	Public Const ACInsurerPaymentsName As String = "InsurerPayments"
	Public Const ACInsurerPaymentsStored As Boolean = True
	
	'added sw payment maintenance 14-11-2002
	Public Const ACSelectCLIIDfromTDIDName As String = "SelectCashListItemIDFromTransDetailID"
    'developer guide no. 39
    Public Const ACSelectCLIIDfromTDIDSQL As String = "spu_ACT_Select_CashListItemID_From_TransDetailID"
	
	'DC071003 -PN6278 -prevent reversal of direct to insurer
    Public Const ACCheckDirectToInsurerName As String = "SelectCashListItemIDFromTransDetailID"
    'developer guide no. 39
    Public Const ACCheckDirectToInsurerSQL As String = "spu_ACT_Check_DirectToInsurer"
	Public Const ACCheckDirectToInsurerStored As Boolean = True
	
    Public Const ACCheckReconciledName As String = "CheckReconciled"
    'developer guide no. 39
    Public Const ACCheckReconciledSQL As String = "spu_ACT_Check_Reconciled"
	Public Const ACCheckReconciledStored As Boolean = True
	
    'DC250204 PN10641 - allow reversal of Future Annual Premium
    'developer guide no. 39
    Public Const ACGetFAPReversalInfoSQL As String = "spu_get_fap_reversal_info"
	Public Const ACGetFAPReversalInfoName As String = "GetFAPInfo"
	Public Const ACGetFAPReversalInfoStored As Boolean = True
	
    'FSA Phase 3.2
    'developer guide no. 39
    Public Const ACGetReversedAccountsTransactionsforReversalSQL As String = "spu_ACT_Get_ReleasedAccountsTransactions_ForReversal"
	Public Const ACGetReversedAccountsTransactionsforReversalName As String = "Get Released Transactions for Reversal"
	Public Const ACGetReversedAccountsTransactionsforReversalStored As Boolean = True

    'developer guide no. 39
    Public Const ACRecallReleasedAccountsTransactionsSQL As String = "spu_ACT_ReleasedAccountsTransactions_Recall"
	Public Const ACRecallReleasedAccountsTransactionsName As String = "Recall Released Transactions"
	Public Const ACRecallReleasedAccountsTransactionsStored As Boolean = True
	
    'DC310105 : Process Reversing Introducer Transactions
    'developer guide no. 39
    Public Const ACGetIntroducerTransforReversalSQL As String = "spu_ACT_Get_Introducer_Trans_For_Reversal"
	Public Const ACGetIntroducerTransforReversalName As String = "Get Introducer Transactions for Reversal"
	Public Const ACGetIntroducerTransforReversalStored As Boolean = True
    'developer guide no. 39
    Public Const ACGetDirectToInsurerTransforReversalSQL As String = "spu_ACT_Get_Direct_To_Insurer_Trans_For_Reversal"
	Public Const ACGetDirectToInsurerTransforReversalName As String = "Get Direct To Insurer Transactions for Reversal"
	Public Const ACGetDirectToInsurerTransforReversalStored As Boolean = True

    'developer guide no. 39
    Public Const ACCashListItemMarkReversedSQL As String = "spu_ACT_CashListItem_Mark_Reversed"
	Public Const ACCashListItemMarkReversedName As String = "spu_ACT_CashListItem_Mark_Reversed"
	Public Const ACCashListItemMarkReversedStored As Boolean = True
	
    'Credit Control Item
    'developer guide no. 39
    Public Const ACReverseCreditControlItemSQL As String = "spu_ACT_Delete_Credit_Control_Item_DocId"
	Public Const ACReverseCreditControlItemName As String = "spu_ACT_Delete_Credit_Control_Item_DocId"
    Public Const ACReverseCreditControlItemStored As Boolean = True

    Public Const kGetDocumentDetailSQL = "spu_ACT_Select_Document"
    Public Const kGetDocumentDetailName = "Get Document Detail"
    Public Const kGetDocumentDetailStored = True

    Public Const kSelectTransDetailName = "Get TransDetail"
    Public Const kSelectTransDetailSQL = "spu_ACT_Select_TransDetail"

    Public Const kGetAccountIDfromInsuranceFileCntSQL = "spu_GetAccountIDfromInsuranceFileCnt"
    Public Const kGetAccountIDfromInsuranceFileCntName = "Get Document Detail"


End Module