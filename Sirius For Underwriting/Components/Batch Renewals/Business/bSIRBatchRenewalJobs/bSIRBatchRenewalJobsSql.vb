Option Strict Off
Option Explicit On
Imports System
Module BatchRenewalJobs
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	
	Public Const ACBatchRenewalJobQueryStored As Boolean = True
	Public Const ACBatchRenewalJobQueryName As String = "GetRenewalJobsQuery"
	Public Const ACBatchRenewalJobQuerySQL As String = "spu_SIR_Batch_Renewal_Job_sel"
	
	Public Const ACGetProductsListForPickListStored As Boolean = True
	Public Const ACGetProductsListForPickListName As String = "GetProductsListForPickList"
	Public Const ACGetProductsListForPickListSQL As String = "spu_SIR_Get_Products_List_For_PickList"
	
	Public Const ACDelProductsLinkedWithBatchRenewalStored As Boolean = True
	Public Const ACDelProductsLinkedWithBatchRenewalName As String = "Del Products Linked With Batch Renewal"
	Public Const ACDelProductsLinkedWithBatchRenewalSQL As String = "spu_SIR_Del_Products_Linked_With_BatchRenewal"
	
	Public Const ACAddProductsLinkedWithBatchRenewalStored As Boolean = True
	Public Const ACAddProductsLinkedWithBatchRenewalName As String = "Add Products Linked With Batch Renewal"
	Public Const ACAddProductsLinkedWithBatchRenewalSQL As String = "spu_SIR_Add_Products_Linked_With_BatchRenewal"
	
	Public Const ACGetSourcesListForPickListStored As Boolean = True
	Public Const ACGetSourcesListForPickListName As String = "GetSourcesListForPickList"
	Public Const ACGetSourcesListForPickListSQL As String = "spu_SIR_Get_Sources_List_For_PickList"
	
	Public Const ACGetJobCodeStored As Boolean = True
	Public Const ACGetJobCodeName As String = "GetJobCode"
	Public Const ACGetJobCodeSQL As String = "spu_SIR_Get_Batch_Renewal_Job_Code"
	
	Public Const ACAddBatchRenewalJobStored As Boolean = True
	Public Const ACAddBatchRenewalJobName As String = "DirectAddBatchRenewalJob"
	Public Const ACAddBatchRenewalJobSQL As String = "spu_SIR_Batch_Renewal_Job_add"
	
	Public Const ACDelBranchesLinkedWithBatchRenewalStored As Boolean = True
	Public Const ACDelBranchesLinkedWithBatchRenewalName As String = "Del Branches Linked With Batch Renewal"
	Public Const ACDelBranchesLinkedWithBatchRenewalSQL As String = "spu_SIR_Del_Branches_Linked_With_BatchRenewal"
	
	Public Const ACAddBranchesLinkedWithBatchRenewalStored As Boolean = True
	Public Const ACAddBranchesLinkedWithBatchRenewalName As String = "Add Branches Linked With Batch Renewal"
	Public Const ACAddBranchesLinkedWithBatchRenewalSQL As String = "spu_SIR_Add_Branches_Linked_With_BatchRenewal"
	
	Public Const ACDelAgentsLinkedWithBatchRenewalStored As Boolean = True
	Public Const ACDelAgentsLinkedWithBatchRenewalName As String = "Del Agents Linked With Batch Renewal"
	Public Const ACDelAgentsLinkedWithBatchRenewalSQL As String = "spu_SIR_Del_Agents_Linked_With_BatchRenewal"
	
	Public Const ACAddAgentsLinkedWithBatchRenewalStored As Boolean = True
	Public Const ACAddAgentsLinkedWithBatchRenewalName As String = "Add Agents Linked With Batch Renewal"
	Public Const ACAddAgentsLinkedWithBatchRenewalSQL As String = "spu_SIR_Add_Agents_Linked_With_BatchRenewal"
	
	Public Const ACDelBatchRenewalStored As Boolean = True
	Public Const ACDelBatchRenewalName As String = "Del Batch Renewal Job"
	Public Const ACDelBatchRenewalSQL As String = "spu_SIR_Batch_Renewal_Job_del"
	
	Public Const ACSuspendJobStored As Boolean = True
	Public Const ACSuspendJobName As String = "SuspendJob Batch Renewal"
	Public Const ACSuspendJobSQL As String = "spu_SIR_SuspendJobs_BatchRenewal"
	
	Public Const ACAgentSelStored As Boolean = True
	Public Const ACAgentSelName As String = "Agent Select Batch Renewal"
	Public Const ACAgentSelSQL As String = "spu_SIR_Batch_Renewal_Agent_sel"
	
	Public Const ACUpdateBatchRenewalJobQueryStored As Boolean = True
	Public Const ACUpdateBatchRenewalJobQueryName As String = "UpdateRenewalJobsQuery"
	Public Const ACUpdateBatchRenewalJobQuerySQL As String = "spu_SIR_Batch_Renewal_Job_upd"
	
	Public Const ACGetBatchRenewalJobSelectionQueryStored As Boolean = True
	Public Const ACGetBatchRenewalJobSelectionQueryName As String = "GetRenewalJobsSelectionQuery"
	Public Const ACGetBatchRenewalJobSelectionQuerySQL As String = "spu_SIRRen_Get_Renewal_Selection_Policy_Totals"
	
	Public Const ACGetBatchRenewalJobAcceptanceQueryStored As Boolean = True
	Public Const ACGetBatchRenewalJobAcceptanceQueryName As String = "GetRenewalJobsAcceptanceQuery"
	Public Const ACGetBatchRenewalJobAcceptanceQuerySQL As String = "spu_SIRRen_Get_Renewal_Acceptance_Policy_Totals"
	
	Public Const ACGetBatchRenewalJobInvitationQueryStored As Boolean = True
	Public Const ACGetBatchRenewalJobInvitationQueryName As String = "GetRenewalJobsInvitationQuery"
	Public Const ACGetBatchRenewalJobInvitationQuerySQL As String = "spu_SIRRen_Get_Renewal_Invitation_Policy_Totals"
	
	Public Const ACGetActiveBatchRenewalJobInvitationQueryStored As Boolean = True
	Public Const ACGetActiveBatchRenewalJobInvitationQueryName As String = "GetActiveRenewalJobsInvitationQuery"
	Public Const ACGetActiveBatchRenewalJobInvitationQuerySQL As String = "spu_SIR_Batch_Renewal_Job_selActive"
    
End Module