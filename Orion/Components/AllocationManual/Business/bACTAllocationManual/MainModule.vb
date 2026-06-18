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
    ' RAW 12/03/2003 : ISS2893 : added extra ACTrans constants
	
    ' This App
	Public Const ACApp As String = "bACTAllocationManual"
	
	' This Class
	Private Const ACClass As String = "MainModule"
	
	'System Options
	Public Const ACCurrencyDifferenceCrebitAccount As Integer = 150
	Public Const ACCurrencyDifferenceDebitAccount As Integer = 151
	Public Const ACWriteOffDebtorAccount As Integer = 152
	Public Const ACWriteOffCrebitorAccount As Integer = 153
	Public Const ACWriteOffInterMediateAccount As Integer = 5028

	' Log Level

	Public Const ACTransDetailId As Integer = 0
	Public Const ACTransFullyPaid As Integer = 1
	Public Const ACTransCurrencyId As Integer = 2
	Public Const ACTransBaseAmount As Integer = 3
	Public Const ACTransCurrencyAmount As Integer = 4
	Public Const ACTransCurrencyBaseXrate As Integer = 5
	Public Const ACTransDocumentId As Integer = 6
	Public Const ACTransIsPrimary As Integer = 7
	Public Const ACTransDocumentReference As Integer = 8
	Public Const ACTransDocumentTypeID As Integer = 9
	Public Const ACTransDocumentDate As Integer = 10
	Public Const ACTransMatchedBaseAmount As Integer = 11
	Public Const ACTransMatchedCurrencyAmount As Integer = 12
	'eck010800
	Public Const ACTransPayCurrencyAmount As Integer = 13
    Public Const ACTransPayBaseAmount As Integer = 14

	' RAW 12/03/2003 : ISS2893 : added
	Public Const ACTransBaseAmountUnrounded As Integer = 15
	Public Const ACTransCurrencyAmountUnrounded As Integer = 16
	'DJM 08/01/2004
	Public Const ACTransSpare As Integer = 17
	Public Const ACTransAccountID As Integer = 18
	Public Const ACTransCompanyID As Integer = 19
    Public Const kTransAccountAmount = 20
    Public Const kTransSystemAmount = 21
    Public Const kTransDetailExId As Integer = 22
    Public Const kWriteOffReasonId As Integer = 23
    Public Const kWriteOffAmount As Integer = 24
    Public Const kIsCurrencyDiff As Integer = 25
	Public Const ACTransLastField = 25
	Public Const kSAMInsurerPaymentCalling As Integer = -99
	Public Const KSAMBDXCalling As Integer = -88

	Public Const AsDebited As String = "0"
	Public Const ClientPayment As String = "1"
	Public Const InsurerSetted As String = "2"

	Public Const WhenEffective As String = "3"
    Public Const ClientPaymentincDID As String = "4"
	
    Public Const ACSelClientPostingsSQL As String = "spu_ACT_Sel_Client_Postings"
	Public Const ACSelClientPostingsName As String = "ClientPostings"
	Public Const ACSelClientPostingsStored As Boolean = True
	
    Public Const ACSelReversalSQL As String = "spu_ACT_Sel_Reversal"
	Public Const ACSelReversalName As String = "Reversal"
	Public Const ACSelReversalStored As Boolean = True
	
    Public Const ACSelDocumentSQL As String = "spu_ACT_Select_Document"
	Public Const ACSelDocumentName As String = "Get SubBranch From Document"
	Public Const ACSelDocumentStored As Boolean = True
	
	'FSA Phase 3.2
    Public Const ACSelDDReversalTransactionSQL As String = "spu_ACT_Select_Reversal_Transaction"
	Public Const ACSelDDReversalTransactionName As String = "DDRevTransaction"
    Public Const ACSelDDReversalTransactionStored As Boolean = True

    Public Const ACUpdateRoundOffTransMatchSQL As String = "spu_ACT_Update_RoundOff_TransMatch"
    Public Const ACUpdateRoundOffTransMatchName As String = "Update RoundOff TransMatch"
    Public Const ACUpdateRoundOffTransMatchStored As Boolean = True

    'Add Allocation SQL
    Public Const kAddAllocationBatchStored = True
    Public Const kAddAllocationBatchName = "AddAllocationBatch"
    Public Const kAddAllocationBatchSQL = "spu_ACT_Add_AllocationBatch"

    Public Const kGetTransDetailTypeIDStored = False
    Public Const kGetTransDetailTypeIDName = "GetTransDetailTypeID"
    Public Const kGetTransDetailTypeIDSQL = "SELECT transdetail_type_id from transdetail_type where code = {code}"

    Public Const kGetMatchPaymentStored = False
    Public Const kGetMatchPaymentName = "GetTransDetailTypeID"
	Public Const kGetMatchPaymentSQL = "SELECT allocationdetail_id, alloc_base_amount, alloc_ccy_amount  " &
				"FROM allocationdetail WHERE transdetail_id = {transdetail_id} and ISNULL(is_reversed, 0) = 0"

	Public Const ACUpdateWriteOffDocumentIdSQL As String = "spu_update_writeoff_documentId"
	Public Const ACUpdateWriteOffDocumentIdName As String = "UpdateWriteOffDocument"

End Module