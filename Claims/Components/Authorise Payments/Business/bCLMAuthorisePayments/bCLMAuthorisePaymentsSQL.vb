Option Strict Off
Option Explicit On
Module BusinessSQL
    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 15052003
    '
    ' Description: Contains the SQL Statements required by the
    '              bCLMAuthorisePayments.Business class.
    '
    ' Edit History: Created By - Ajit Kumar
    ' ***************************************************************** '

    'SQL Statements


    'Get the list of all referred payments
    Public Const ACGetReferredCliamsStored As Boolean = True
    Public Const ACGetReferredCliamsName As String = "Get Referred claims"
    Public Const ACGetReferredCliamsSQL As String = "spu_clm_get_referred_payments"

    'Get required information for raising an event
    Public Const ACGetCountsStored As Boolean = True
    Public Const ACGetCountsName As String = "Get Counts"
    Public Const ACGetCountsSQL As String = "spu_get_claim_cnts"

    Public Const ACProcessDeclineStored As Boolean = True
    Public Const ACProcessDeclineName As String = "Process Claims payment - Decline"
    Public Const ACProcessDeclineSQL As String = "spu_clm_Process_Decline"

    Public Const ACProcessAuthoriseStored As Boolean = True
    Public Const ACProcessAuthoriseName As String = "Process Claims payment - Authorise"
    Public Const ACProcessAuthoriseSQL As String = "spu_clm_Process_Authorise"


    Public Const ACCheckUserGroupStored As Boolean = True
    Public Const ACCheckUserGroupName As String = "Check Name Group Member"
    Public Const ACCheckUserGroupSQL As String = "spu_pmuser_is_name_member"

    ' START CHANGES - Changed By: AAB  - Changed On: 25-Nov-2003 09:45
    Public Const ACGetDebtorGroupsStored As Boolean = True
    Public Const ACGetDebtorGroupsName As String = "Get ALL the Debtor groups for perticular type"
    Public Const ACGetDebtorGroupsSQL As String = "spu_Get_Debtor_User_Groups"

    Public Const ACGetApprovalRecordsStored As Boolean = True
    Public Const ACGetApprovalRecordsName As String = "Get the Approval Records for perticular payment"
    Public Const ACGetApprovalRecordsSQL As String = "spu_Approval_Records_Sel"

    Public Const ACApprovalStepDetailsStored As Boolean = True
    Public Const ACApprovalStepDetailsName As String = "Get the Approval User Group and Code for perticular Step"
    Public Const ACApprovalStepDetailsSQL As String = "spu_Get_Approval_Step_Details"

    Public Const ACIsUserUniqueStored As Boolean = True
    Public Const ACIsUserUniqueName As String = "Check if the user has approved a previous step"
    Public Const ACIsUserUniqueSQL As String = "spu_Check_Is_User_Unique"

    Public Const ACUserLimitsStored As Boolean = True
    Public Const ACUserLimitsName As String = "Get the User Claim & Regular Payments Limits"
    Public Const ACUserLimitsSQL As String = "spu_Get_User_Authority_Limit"

    Public Const ACAddPaymentApprovalRecordStored As Boolean = True
    Public Const ACAddPaymentApprovalRecordName As String = "Add a Record to the Payment_Approval Table"
    Public Const ACAddPaymentApprovalRecordSQL As String = "spu_Payment_Approval_Add"

    Public Const ACRemoveAuthTasksStored As Boolean = True
    Public Const ACRemoveAuthTasksName As String = "Remove Authrisation Tasks"
    Public Const ACRemoveAuthTasksSQL As String = "spu_clm_remove_authorisation_tasks"

    Public Const ACGetWTMInstanceCntStored As Boolean = True
    Public Const ACGetWTMInstanceCntName As String = "Get Work Task Instance Cnt Using Key name and key value."
    Public Const ACGetWTMInstanceCntSQL As String = "spu_get_pmwrk_task_instance_cnt"

    Public Const kGetClaimPaymentAccountsDetailsName As String = "Returns the required details for the claim payment made in this session"
    Public Const kGetClaimPaymentAccountsDetailsSQL As String = "spu_CLM_Get_Claim_Payment_Accounts_Details"


    Public Const kGetClaimVersionDescriptionName As String = "spu_CLM_GetClaimVersionDescription"
    Public Const kGetClaimVersionDescriptionSQL As String = "spu_CLM_GetClaimVersionDescription"

    Public Const kGetClaimStatusName As String = "GetClaimStatus"
    Public Const kGetClaimStatusSQL As String = "Spu_get_claim_status"

    Public Const kGetReferredClaimStatusName As String = "Get Referred Claim status"
    Public Const kGetReferredClaimStatusSQL As String = "spu_CLM_Get_ReferredClaim_status"

    Public Const ACClaimUpdateStatusSQL As String = "spu_Claim_Upd_Status"
    Public Const ACClaimUpdateStatusName As String = "Claim Update Status"

    Public Const kGetCashListItemClaimLinkName As String = "CashListItem_Claim_Link_Sel"
    Public Const kGetCashListItemClaimLinkSQL As String = "spu_cashlistitem_claim_link_Sel"

    Public Const kSetClaimRcommendPaymentStatusName As String = "Set Payment For Recommendation"
    Public Const kSetClaimRcommendPaymentStatusSQL As String = "spu_CLM_Set_Payment_for_Recommendation"

    Public Const kGetReserveTotalForClaimPaymentName As String = "Get_Reserve_For_Claim_Payment"
    Public Const kGetReserveTotalForClaimPaymentSQL As String = "spu_Get_Reserve_For_Claim_Payment"

    Public Const kGetTransDetailFromCashListItemName As String = "Select CashListItem"
    Public Const kGetTransDetailFromCashListItemSQL As String = "spu_ACT_Select_CashListItem"

    Public Const kGetAlreadyReferredClaimStatusName As String = "Get Already Referred Claim status"
    Public Const kGetAlreadyReferredClaimStatusSQL As String = "spu_CLM_Get_Already_ReferredClaim_status"

    'archana
    Public Const ACGetUserotherpartyStored As Boolean = True
    Public Const ACGetUserotherpartySQL As String = "spu_Get_User_OtherPartyID"
    Public Const ACGetUserotherpartyName As String = "Get user other party detail"
End Module
