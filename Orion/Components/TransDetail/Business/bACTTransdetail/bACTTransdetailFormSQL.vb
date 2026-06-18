Option Strict Off
Option Explicit On
Imports System
Module FormSQL
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	' ***************************************************************** '
	' Class Name: FormSQL
	'
	' Date: 08/08/1997
	'
	' Description: Contains the SQL Statements required by the
	'              bACTTransdetail.Form class.
	'
	' Edit History:
	' ***************************************************************** '
	
	'SQL Statements

    ' Select Transdetail SQL
    Public Const ACGetDetailsStored As Boolean = True
    Public Const ACGetDetailsName As String = "SelectTransdetail"
    Public Const ACGetDetailsSQL As String = "spu_ACT_select_TransDetail"

    ' Select All Transdetail SQL by Document ID
    Public Const ACGetAllDetailsStored As Boolean = True
    Public Const ACGetAllDetailsName As String = "SelectAllTransdetail"
    Public Const ACGetAllDetailsSQL As String = "spu_ACT_selall_TransDetail_doc"

    ' Check ID SQL
    Public Const ACCheckIDStored As Boolean = True
    Public Const ACCheckIDName As String = "CheckTransdetailID"
    Public Const ACCheckIDSQL As String = "spu_ACT_check_TransDetail"

    ' Add Transdetail SQL
    Public Const ACAddStored As Boolean = True
    Public Const ACAddName As String = "AddTransdetail"
    Public Const ACAddSQL As String = "spu_ACT_add_TransDetail"

    ' Delete Transdetail SQL
    Public Const ACDeleteStored As Boolean = True
    Public Const ACDeleteName As String = "DeleteTransdetail"
    Public Const ACDeleteSQL As String = "spu_ACT_delete_TransDetail"

    ' Update Transdetail SQL
    Public Const ACUpdateStored As Boolean = True
    Public Const ACUpdateName As String = "UpdateTransdetail"
    Public Const ACUpdateSQL As String = "spu_ACT_update_TransDetail"

    Public Const ACSelCacheStored As Boolean = True
    Public Const ACSelCacheName As String = "CacheSel"
    Public Const ACSelCacheSQL As String = "spu_transdetail_cache_sel"

    Public Const ACUpdateCacheStored As Boolean = True
    Public Const ACUpdateCacheName As String = "CacheUpdate"
    Public Const ACUpdateCacheSQL As String = "spu_transdetail_cache_upd"

    'FSA Phase 3.2
    Public Const ACAddSuspendedTransactionStored As Boolean = True
    Public Const ACAddSuspendedTransactionName As String = "CreateSuspendedTransaction"
    Public Const ACAddSuspendedTransactionSQL As String = "spu_ACT_SuspendedAccountsTransactions_Add"

    Public Const ACSelectSuspendedTransactionStored As Boolean = True
    Public Const ACSelectSuspendedTransactionName As String = "SelectSuspendedTransaction"
    Public Const ACSelectSuspendedTransactionSQL As String = "spu_ACT_SuspendedAccountsTransactions_Sel"

    Public Const ACIsSuspendedTransPostedStored As Boolean = True
    Public Const ACIsSuspendedTransPostedName As String = "IsSuspendedTransactionPosted"
    Public Const ACIsSuspendedTransPostedSQL As String = "spu_ACT_IsSuspendedTransactionPosted"

    Public Const ACRewriteSuspendedTransactionStored As Boolean = True
    Public Const ACRewriteSuspendedTransactionName As String = "SelectSuspendedTransaction"
    Public Const ACRewriteSuspendedTransactionSQL As String = "spu_ACT_SuspendedAccountsTransactions_Rewrite"

    Public Const ACSelectSuspendedAllocationStored As Boolean = True
    Public Const ACSelectSuspendedAllocationName As String = "SelectSuspendedTransaction"
    Public Const ACSelectSuspendedAllocationSQL As String = "spu_ACT_SuspendedAccountsTransactionsAllocation_Sel"

    Public Const ACAddReleasedTransactionStored As Boolean = True
	Public Const ACAddReleasedTransactionName As String = "CreateReleasedTransaction"
    Public Const ACAddReleasedTransactionSQL As String = "spu_ACT_ReleasedAccountsTransactions_Add"
	
	Public Const ACSelectReleasedTransactionStored As Boolean = True
	Public Const ACSelectReleasedTransactionName As String = "SelectReleasedTransaction"
    Public Const ACSelectReleasedTransactionSQL As String = "spu_ACT_ReleasedAccountsTransactions_Sel"
	
	'FSA Phase 4 Commission Partial Movement
	Public Const ACGetAllocationPartStored As Boolean = True
	Public Const ACGetAllocationPartName As String = "GetAllocationPart"
    Public Const ACGetAllocationPartSQL As String = "spu_ACT_GetAllocationPart"
	
	Public Const ACFinanceSuspendedTransactionStored As Boolean = True
	Public Const ACFinanceSuspendedTransactionName As String = "FinanceSuspendedTransaction"
    Public Const ACFinanceSuspendedTransactionSQL As String = "spu_ACT_SuspendedAccountsTransactions_Finance"
	
	Public Const AC_SQL_RiskTransferStatus_SP As Boolean = True
	Public Const AC_SQL_RiskTransferStatus_Name As String = "GetRiskTransferStatus"
    Public Const AC_SQL_RiskTransferStatus_SQL As String = "spu_TRN_risk_transfer_status_select"

    Public Const ACGetInsuranceFileCntName As String = "Get InsuranceFileCnt"
    Public Const ACGetInsuranceFileCntSQL As String = "spu_Act_Get_InsuranceFileCnt"
    Public Const ACGetInsuranceFileCntStored As Boolean = True


End Module