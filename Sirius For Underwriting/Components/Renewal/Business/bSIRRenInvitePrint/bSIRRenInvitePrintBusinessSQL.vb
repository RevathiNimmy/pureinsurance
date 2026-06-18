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
    '              bSIRRenInvitePrint.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    Public Const ACUpdRenewalStatusStored As Boolean = True
    Public Const ACUpdRenewalStatusName As String = "UpdateRenewalStatus"
    Public Const ACUpdRenewalStatusSQL As String = "spu_Renewal_Status_Invite_Printed_upd"

    Public Const ACAddCreditControlItemInsFileStored As Boolean = True
    Public Const ACAddCreditControlItemInsFileName As String = "AddCreditControlItemInsFile"
    Public Const ACAddCreditControlItemInsFileSQL As String = "spu_ACT_Add_Credit_Control_Item_InsFile"

    Public Const ACAddLastPrintRunStored As Boolean = True
    Public Const ACAddLastPrintRunName As String = "AddLastPrintRun"
    Public Const ACAddLastPrintRunSQL As String = "spe_Last_Print_Run_add"

    Public Const ACDelLastPrintRunStored As Boolean = True
    Public Const ACDelLastPrintRunName As String = "DelLastPrintRun"
    Public Const ACDelLastPrintRunSQL As String = "spu_Last_Print_Run_del"

    Public Const ACSelDocTypeIDStored As Boolean = False
    Public Const ACSelDocTypeIDName As String = "GetDocumentTypeID"
    Public Const ACSelDocTypeIDSQL As String = "SELECT document_type_id FROM document_type" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "WHERE code = {code}" & Strings.ChrW(13) & Strings.ChrW(10)

    Public Const ACSelIsInstalmentStored As Boolean = True
    Public Const ACSelIsInstalmentName As String = "Is Instalments"
    Public Const ACSelIsInstalmentSQL As String = "spu_Is_Instalment"

    'Get Branch Agents
    Public Const ACGetBranchAgentsStored As Boolean = True
    Public Const ACGetBranchAgentsName As String = "Get Branch Agents"
    'Developer Guide No 39
    Public Const ACGetBranchAgentsSQL As String = "spu_Get_BranchAgents"

    'Task WR9 Batch Renewal - Multi Threaded Controller
    Public Const ACGetRenewalInvitationDetailsStored As Boolean = True
    Public Const ACGetRenewalInvitationDetailsName As String = "GetRenewalInvitationDetails"
    Public Const ACGetRenewalInvitationDetailsSQL As String = "spu_SIR_Get_Renewal_Invitation_Details"

    Public Const ACAddBatchRenewalJobRunsStored As Boolean = True
    Public Const ACAddBatchRenewalJobRunsName As String = "AddBatchRenewalJobRuns"
    Public Const ACAddBatchRenewalJobRunsSQL As String = "spu_SIR_Add_Batch_Renewal_Job_Runs"

    Public Const ACUpdateBatchRenewalJobRunsStored As Boolean = True
    Public Const ACUpdateBatchRenewalJobRunsName As String = "UpdateBatchRenewalJobRuns"
    Public Const ACUpdateBatchRenewalJobRunsSQL As String = "spu_SIR_Update_Batch_Renewal_Job_Runs"

    Public Const ACGetBatchJobPrintingOptionsStored As Boolean = True
    Public Const ACGetBatchJobPrintingOptionsName As String = "GetBatchJobPrintingOptions"
    Public Const ACGetBatchJobPrintingOptionsSQL As String = "spu_SIRRen_Get_Batch_Job_Printing_Options"
End Module