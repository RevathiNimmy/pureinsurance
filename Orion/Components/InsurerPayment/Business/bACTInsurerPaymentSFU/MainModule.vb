Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	' Project and Module Constants
	Public Const ACApp As String = "bACTInsurerPayment"
	Private Const ACClass As String = "MainModule"
	
	' Ledger constants
	Public Const ACInsurerLedger As String = "Insurer"
	Public Const ACAgentLedger As String = "Agent"
	
	
    ' SQL for insurer payment procedure. Applies to insurers and agents!
    'developer guide no.39
    Public Const ACInsurerPaymentsSQL As String = "spu_ACT_Do_InsurerPayments_SFU"
	Public Const ACInsurerPaymentsName As String = "InsurerPayments"
	Public Const ACInsurerPaymentsStored As Boolean = True
	
	
	' Dynamic sql for the unmark process
	Public Const ACUnmarkTransactionSQL As String = "Delete From TransMatch Where transdetail_id = {transdetail_id} And allocationdetail_id is null"
	Public Const ACUnmarkTransactionName As String = "UnmarkTrans"
	Public Const ACUnmarkTransactionStored As Boolean = False
	
	'SQL for Fetching Currenct details against a document
	Public Const ACCurrDetailsSQL As String = "select top 1 currency_id,document_sequence,currency_base_xrate,insurance_ref,period_id from transdetail where document_id ={Doc_id} order by document_sequence desc"
	Public Const ACCurrDetailsName As String = "CurrDetails"

    'SQL for deleting Writeoff Transaction against document
    Public Const ACDeleteWOFFSQL As String = "spu_Delete_WriteOff_Transactions"
    Public Const ACDeleteWOFFName As String = "DeleteWriteOff"
    Public Const ACDeleteWoOFFStored As Boolean = True

    'Delete from TRansmatch
    Public Const ACDeleteWOFF1SQL As String = "delete from transmatch where transdetail_id in (select transdetail_id from transdetail where spare like 'WRITEOFF' and Document_id = {Doc_id})"
	Public Const ACDeleteWOFF1Name As String = "DeleteTransmatch"
	
	' Start - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.4)
	Public Const ACUpdateCommentName As String = "UpdateComment"
	Public Const ACUpdateCommentSQL As String = "spu_ACT_Update_TransDetail_Comment"
    Public Const ACUpdateCommentStored As Boolean = True
    ' End - (Sankar) - (Tech Spec - QBENZCR006 - Insurer Payments Comment Field.doc) - (4.4)
    Public Const ACGetAgentDetailForAccountName As String = "GetAgentDetailForAccount"
    Public Const ACGetAgentDetailForAccountSQL As String = "spu_Get_Agent_Detail_ForAccount"
    Public Const ACGetAgentDetailForAccountStored As Boolean = True

    Public Const ACUpdateInstalmentNumberName As String = "UpdateInstalmentNumber"
    Public Const ACUpdateInstalmentNumberSQL As String = "spu_ACT_Update_TransMatch_InstalmentNumber"
    Public Const ACUpdateInstalmentNumberStored As Boolean = True

    ' Dynamic sql for the unmark process
    Public Const ACUnmarkInstTransactionSQL As String = "Delete From TransMatch Where transdetail_id = {transdetail_id} And allocationdetail_id is null And InstalmentNumber = {InstalmentNumber}"
    Public Const ACUnmarkInstTransactionName As String = "UnmarkInstTrans"
    Public Const ACUnmarkInstTransactionStored As Boolean = False

    Public Const ACSelInstalmentsForPartySettelmentSQL As String = "spu_ACT_Select_Instalments_For_PartySettelment"
    Public Const ACSelInstalmentsForPartySettelmentName As String = "SelectInstalmentsForPartySettelment"
    Public Const ACSelInstalmentsForPartySettelmentStored As Boolean = True

    Public Const ACLoadAllocationPeriodName As String = "LoadAllocationPeriod"
    Public Const ACLoadAllocationPeriodSQL As String = "spu_ACT_Get_Allocation_Period"
    Public Const ACLoadAllocationPeriodStored As Boolean = True

    Public Const ACGetPeriodIdName As String = "GetPeriodId"
    Public Const ACGetPeriodIdSQL As String = "spu_ACT_Do_GetPeriodForDate"
    Public Const ACGetPeriodIdStored As Boolean = True

    Public Const ACDeleteTransMatchInstSQL As String = "Delete From TransMatch Where transdetail_id = {transdetail_id} And InstalmentNumber is not null"
    Public Const ACDeleteTransMatchInstName As String = "Delete Transmatch inst"
    Public Const ACDeleteTransMatchInstStored As Boolean = False

    Public Const ACGetTranDetailContraEntriesForInstalmentsName As String = "GetTranDetailContraEntriesForInstalments"
    Public Const ACGetTranDetailContraEntriesForInstalmentsSQL As String = "spu_Get_TranDetailContraEntriesForInstalments"
    Public Const ACGetTranDetailContraEntriesForInstalmentsStored As Boolean = True

    Public Const ACGetTransDetailIdForSetteledPremiumName As String = "GetTransDetailIdForSetteledPremium"
    Public Const ACGetTransDetailIdForSetteledPremiumSQL As String = "spu_ACT_GetTransDetailIdForSetteledPremium"
    Public Const ACGetTransDetailIdForSetteledPremiumStored As Boolean = True

    Public Const kInsurerPaymentsForBatchSQL As String = "spu_ACT_Find_InsurerPayments_ForBatch"
    Public Const kInsurerPaymentsForBatchName As String = "InsurerPaymentsForBatch"
    Public Const kInsurerPaymentsForBatchStored As Boolean = True


    Public Const KGetPMNavXMBatchTransactionDetailName As String = "GetPMNavXMBatchTransactionDetail"
    Public Const KGetPMNavXMBatchTransactionDetailSQL As String = "spu_Get_PMNav_Batch_Transaction_Details"

    Public Const KTUpdateWriteOffDocumentName As String = "ACUpdateWriteOffDocument"
    Public Const KTUpdateWriteOffDocumentSQL As String = "spu_ACT_Update_WritOff_Document"

End Module
