Option Strict Off
Option Explicit On
Module BusinessSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: BusinessSQL
	'
	' Date: 02/09/2000
	'
	' Description: Contains the SQL Statements required by the
	'              bSIRRenSelection.Business class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements
	
	'''For Finding Transaction Details
	Public Const ACPrepareAgentSummary As Boolean = True
	Public Const ACPrepareAgentSummaryCodeName As String = "PrepareAgentSummary"
	Public Const ACPrepareAgentSummaryCodeSQL As String = "spu_ACT_PrepareAgentSummary"

    '''For Finding Transaction Details for Allocated Transactions
    Public Const ACPrepareAgentSummaryAllocatedTrans As Boolean = True
    Public Const ACPrepareAgentSummaryAllocatedTransCodeName As String = "PrepareAgentSummaryAllocatedTrans"
    Public Const ACPrepareAgentSummaryAllocatedTransCodeSQL As String = "spu_ACT_PrepareAgentSummaryAllocation"

	'''For Create Commission Payments Batch
	Public Const ACCreateCommPaymentsBatch As Boolean = True
	Public Const ACCreateCommPaymentsBatchName As String = "CreateCommissionPaymentsBatch"
	Public Const ACCreateCommPaymentsBatchSQL As String = "spu_ACT_CreateCommissionPaymentsBatch"
	
	'''For Create Commission Payments In Batch
	Public Const ACMarkCommissionPaymentsInBatch As Boolean = True
	Public Const ACMarkCommissionPaymentsInBatchName As String = "MarkCommissionPaymentsInBatch"
	Public Const ACMarkCommissionPaymentsInBatchSQL As String = "spu_ACT_MarkCommissionPaymentsInBatch"
	
	'''For Get Chosen Commission Payments
	Public Const ACGetChosenCommissionPayments As Boolean = True
	Public Const ACGetChosenCommissionPaymentsName As String = "GetChosenCommissionPayments"
    Public Const ACGetChosenCommissionPaymentsSQL As String = "spu_ACT_GetChosenCommissionPayments"
	
	'''For Get Agent Details for Payments
	Public Const ACGetAgentDetailsforPayments As Boolean = True
	Public Const ACGetAgentDetailsforPaymentsName As String = "GetAgentDetailsforPayments"
	Public Const ACGetAgentDetailsforPaymentsSQL As String = "spu_get_Agent_Details_for_Payments"
	
	'''For getting documents on the basis of Account Id and Batch Id
	Public Const ACGetDocumentsForAccountBatch As Boolean = True
	Public Const ACGetDocumentsForAccountBatchName As String = "GetDocumentsForAccountBatch"
	Public Const ACGetDocumentsForAccountBatchSQL As String = "spu_get_Documents_For_Account_Batch"
	
	'''For removing batch ID
	Public Const ACRemoveCommissionPaymentsBatch As Boolean = True
	Public Const ACRemoveCommissionPaymentsBatchName As String = "RemoveCommissionPaymentsBatch"
	Public Const ACRemoveCommissionPaymentsBatchSQL As String = "spu_ACT_RemoveCommissionPaymentsBatch"
	
	Public Const ACGetPartyShortname As Boolean = True
	Public Const ACGetPartyShortnameName As String = "GetPartyShortname"
	Public Const ACGetPartyShortnameSQL As String = "spu_Get_Party_Shortname"
	
	Public Const ACGetWorkTaskIDStored As Boolean = True
	Public Const ACGetWorkTaskIDName As String = "GetWorkTaskID"
	Public Const ACGetWorkTaskIDSQL As String = "spu_SAM_get_wrk_task_id"
	'
	Public Const ACGetWorkTaskGroupIDStored As Boolean = True
	Public Const ACGetWorkTaskGroupIDName As String = "TaskGroup For WrkTaskID"
	Public Const ACGetWorkTaskGroupIDSQL As String = "spu_Get_TaskGroup_For_WrkTaskID"
End Module