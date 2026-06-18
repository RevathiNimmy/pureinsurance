Module BusinessSQL
    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 17/06/2008
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRBatchRenewalController.ProcessJobs
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    Public Const ACGetJobDetailsStored = True
    Public Const ACGetJobDetailsName = "GetJobDetails"
    Public Const ACGetJobDetailsSQL = "spu_SIRRen_Get_Job_Details"

    Public Const ACGetRenewalSelectionPolicyListStored = True
    Public Const ACGetRenewalSelectionPolicyListName = "GetRenewalSelectionPolicyList"
    Public Const ACGetRenewalSelectionPolicyListSQL = "spu_SIRRen_Get_Renewal_Selection_Policy_List"

    Public Const ACGetRenewalInvitationPolicyListStored = True
    Public Const ACGetRenewalInvitationPolicyListName = "GetRenewalInvitationPolicyList"
    Public Const ACGetRenewalInvitationPolicyListSQL = "spu_SIRRen_Get_Renewal_Invitation_Policy_List"

    Public Const ACGetRenewalAcceptancePolicyListStored = True
    Public Const ACGetRenewalAcceptancePolicyListName = "GetRenewalAcceptancePolicyList"
    Public Const ACGetRenewalAcceptancePolicyListSQL = "spu_SIRRen_Get_Renewal_Acceptance_Policy_List"

    Public Const ACDelRenewalReportStored = True
    Public Const ACDelRenewalReportName = "DelRenewalReport"
    Public Const ACDelRenewalReportSQL = "spu_Del_RenewalReport"

    Public Const ACSelRenewalPolicyStored = True
    Public Const ACSelRenewalPolicyName = "SelRenewalPolicies"
    Public Const ACSelRenewalPolicySQL = "spu_Sel_Renewal_Policies"

    Public Const ACUpdRenewalPolicyStored = True
    Public Const ACUpdRenewalPolicyName = "UpdateRenewalPolicies"
    Public Const ACUpdRenewalPolicySQL = "spu_upd_Renewal_Policies"

    Public Const ACDelLastPrintRunStored = True
    Public Const ACDelLastPrintRunName = "DelLastPrintRun"
    Public Const ACDelLastPrintRunSQL = "spu_Del_Last_Print_Run"

    Public Const ACDelRenewalStatusStored = True
    Public Const ACDelRenewalStatusName = "DelRenewalStatus"
    Public Const ACDelRenewalStatusSQL = "spu_Del_Renewal_Status"

    Public Const ACDelPolicyDependantStored = True
    Public Const ACDelPolicyDependantName = "DeletePolicyDependant"
    Public Const ACDelPolicyDependantSQL = "spu_Del_Policy_Dependant"

    Public Const ACDelInsFileSystemStored = True
    Public Const ACDelInsFileSystemName = "DeleteSIRInsuranceFileSystem"
    Public Const ACDelInsFileSystemSQL = "spe_Insurance_File_System_del"

    ' Delete SIRInsuranceFile SQL
    Public Const ACDelInsFileStored = True
    Public Const ACDelInsFileName = "DeleteSIRInsuranceFile"
    Public Const ACDelInsFileSQL = "spe_Insurance_File_del"
End Module
