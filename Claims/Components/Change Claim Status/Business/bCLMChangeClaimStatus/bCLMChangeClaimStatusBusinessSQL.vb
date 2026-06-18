Option Strict Off
Option Explicit On
Imports System
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
    '              bCLMChangeClaimStatus.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    Public Const ACUpdPolicyPremiumStored As Boolean = True
    Public Const ACUpdPolicyPremiumName As String = "UpdatePolicyPremium"
    Public Const ACUpdPolicyPremiumSQL As String = "spu_Upd_Policy_Premium"

    ' Delete any Work Claim records
    Public Const ACDeleteClaimStored As Boolean = True
    Public Const ACDeleteClaimName As String = "Delete Work Claim"
    Public Const ACDeleteClaimSQL As String = "spu_delete_claim"

    '' Copy records from Work To Claim
    'Public Const ACCopyWorkToClaimStored = True
    'Public Const ACCopyWorkToClaimName = "Copy Work To Claim"
    'Public Const ACCopyWorkToClaimSQL = "spu_copy_work_to_claim"

    'get original claim id
    'Public Const ACSelOriginalClaimIDStored = False
    'Public Const ACSelOriginalClaimIDName = "Get Original Claim ID"
    'Public Const ACSelOriginalClaimIDSQL = "SELECT original_claim_id from work_claim where claim_id = {work_claim_id}"

    'delete claim associates
    'Public Const ACDelClaimAssociateStored = True
    'Public Const ACDelClaimAssociateName = "Delete Claim Associates"
    'Public Const ACDelClaimAssociateSQL = "spu_delete_claim_details"

    'get client and policy cnts
    Public Const ACGetCountsStored As Boolean = True
    Public Const ACGetCountsName As String = "Get Counts"
    Public Const ACGetCountsSQL As String = "spu_get_claim_cnts"

    'get transaction type id
    Public Const ACGetTransactionTypeIDName As String = "Get Transaction Type ID"
    Public Const ACGetTransactionTypeIDSQL As String = "spu_CLM_Get_Transaction_Type_Details"

    'get loss dates from claim and work claim
    Public Const ACGetLossDateName As String = "Get claim lost dates"
    Public Const ACGetLossDateSQL As String = "spu_CLM_Get_Claim_Loss_Dates"

    'get claim flag
    Public Const ACGetClaimFlagName As String = "get claim flags"
    Public Const ACGetClaimFlagSQL As String = "spu_CLM_Get_Add_Task_Details"

    'get claim details - (select more fields when you need other information)
    Public Const ACGetClaimDetailName As String = "get claim details"
    Public Const ACGetClaimDetailSQL As String = "spu_CLM_Get_Claim_Details"

    'get client details - modify sql when you need more information
    'Public Const ACGetClientDetailStored = False
    'Public Const ACGetClientDetailName = "get client details"
    'Public Const ACGetClientDetailSQL = "SELECT ifo.insurance_holder_cnt" & vbCrLf & _
    ''                                    "FROM insurance_file ifi, insurance_folder ifo" & vbCrLf & _
    ''                                    "WHERE ifi.insurance_file_cnt = {insurance_file_cnt}" & vbCrLf & _
    ''                                    "AND ifi.insurance_folder_cnt = ifo.insurance_folder_cnt"

    ' Check Renewals SQL - Alix Bergeret
    ' Check if policy is in renewal cycle
    'Public Const ACCheckRenewalsStored = True
    'Public Const ACCheckRenewalsName = "CheckRenewals"
    'Public Const ACCheckRenewalsSQL = "spu_check_in_renewal_claim"

    ' Alix Bergeret - 23/01/2003
    ' Check if policy has been renewed since last claim
    'Public Const ACCheckPolicyRenewedStored = True
    'Public Const ACCheckPolicyRenewedName = SIRLookupRenewalStopCode & "CheckPolicyRenewed"
    'Public Const ACCheckPolicyRenewedSQL = "spu_check_policy_renewed"

    '' Alix Bergeret - 27/01/2003
    '' Check if the No Claim Discount flag has changed
    'Public Const ACNCDStatusHasChangedStored = True
    'Public Const ACNCDStatusHasChangedName = SIRLookupRenewalStopCode & "NCDStatusHasChanged"
    'Public Const ACNCDStatusHasChangedSQL = "spu_check_ncd_status"

    ' Get folderID and File ID for a given claimID
    'Public Const ACGetInsFolderFileCntStored = True
    'Public Const ACGetInsFolderFileCntName = SIRLookupRenewalStopCode & "GetInsFolderFileCnt"
    'Public Const ACGetInsFolderFileCntSQL = "spu_get_ins_folder_file_cnt"

    ' RVH - 26/2/2003
    ' Copy Work To Claim GIS
    'Public Const ACGISCopyWorkToClaimStored = True
    'Public Const ACGISCopyWorkToClaimName = "Copy Work To Claim GIS"
    'Public Const ACGISCopyWorkToClaimSQLStart = "spg_"
    'Public Const ACGISCopyWorkToClaimSQLEnd = "_copy_work_to_claim"

    ' Get datamodel code for claim
    Public Const ACGetDatamodeCodeforClaimStored As Boolean = True
    Public Const ACGetDatamodeCodeforClaimName As String = "Get Datamodel Code for Claim"
    Public Const ACGetDatamodeCodeforClaimSQL As String = "spu_Get_DataModel_Code_For_Claim"
    ' RVH - END

    'AK 060503 - get product id
    Public Const ACGetInsFileCntProductIdStored As Boolean = True
    Public Const ACGetInsFileCntProductIdName As String = "Get Claim Ins File Cnt Product Id"
    Public Const ACGetInsFileCntProductIdSQL As String = "spu_get_claiminsfilecnt_productid"

    'AK 270503 - set referred payment
    Public Const ACSetReferredPaymentStored As Boolean = True
    Public Const ACSetReferredPaymentName As String = "Set Referred Payment"
    Public Const ACSetReferredPaymentSQL As String = "spu_CLM_Set_Referred_Payments"

    'AK 280503 - Get Total Claim Payment
    Public Const ACGetTotalPaymentStored As Boolean = True
    Public Const ACGetTotalPaymentName As String = "Get Total Payment"
    Public Const ACGetTotalPaymentSQL As String = "spu_CLM_Get_Total_Payment"

    'AK 280503 - Get User Group Id for a group code
    Public Const ACGetUserGroupIdStored As Boolean = True
    Public Const ACGetUserGroupIdName As String = "Get User Group Id"
    Public Const ACGetUserGroupIdSQL As String = "spu_Get_User_Group_Id"

    'AK 100603 - remove existing authorisation tasks
    Public Const ACRemoveAuthTasksStored As Boolean = True
    Public Const ACRemoveAuthTasksName As String = "Remove Authrisation Tasks"
    Public Const ACRemoveAuthTasksSQL As String = "spu_clm_remove_authorisation_tasks"

    Public Const kGetPaymentAmountName As String = "Get the Payment Amount for the Specified Work Claim"
    Public Const kGetPaymentAmountSQL As String = "spu_clm_get_payment_amount"

    Public Const ACGetReserveRecoveryOSStored As Boolean = True
    Public Const ACGetReserveRecoveryOSName As String = "Get Reserve and Recovery Outstandings"
    Public Const ACGetReserveRecoveryOSSQL As String = "spu_GetCurrentReserveRecovery"

    Public Const ACUpdClaimStatusName As String = "Update claim status"
    Public Const ACUpdClaimStatusSQL As String = "spu_CLM_Update_Claim_status"

    Public Const kGetClaimDMEFolderDetailsName As String = "Returns all detail necessary to create claim DME folder"
    Public Const kGetClaimDMEFolderDetailsSQL As String = "spu_CLM_Get_Claim_DME_Details"

    Public Const kUpdatePaymentDocumentDetailsName As String = "Updates the stats folders associated payment"
    Public Const kUpdatePaymentDocumentDetailsSQL As String = "spu_CLM_Payment_Document_Update"

    Public Const kUpdateReceiptDocumentDetailsName As String = "Updates the stats folders associated receipt to create a direct link between the receipt and accounts"
    Public Const kUpdateReceiptDocumentDetailsSQL As String = "spu_CLM_Receipt_Document_Update"

    Public Const kGetClaimPaymentAccountsDetailsName As String = "Returns the required details for the claim payment made in this session"
    Public Const kGetClaimPaymentAccountsDetailsSQL As String = "spu_CLM_Get_Claim_Payment_Accounts_Details"

    Public Const kUpdateClaimIsDirtyName As String = "updates the is dirty flag on the claim"
    Public Const kUpdateClaimIsDirtySQL As String = "spu_CLM_Claim_Is_Dirty_Update"

    Public Const kGetStatsFolderForClaimName As String = "returns the stats folder for the claim"
    Public Const kGetStatsFolderForClaimSQL As String = "spu_CLM_Get_Claims_Stats_Folders"

    Public Const kFinaliseClaimDetailsName As String = "compalte off any database processing that is required for the specified claims"
    Public Const kFinaliseClaimDetailsSQL As String = "spu_CLM_Finalise_Claim_Details"

    Public Const kGetOriginalClaimIdName As String = "returns the base claim id for the claim specified"
    Public Const kGetOriginalClaimIdSQL As String = "spu_CLM_Get_Base_Claim"

    Public Const kUpdateClaimDescName As String = "Update status"
    Public Const kUpdateClaimDescSQL As String = "spu_CLM_UpdateClaimDescription"


    Public Const kReverseStstsTransactionsName As String = "Claim_RI_ReverseTransaction_Stats"
    Public Const kReverseStstsTransactionsSQL As String = "spu_Claim_RI_ReverseTransaction_Stats"

    Public Const kRepostTransactionsName As String = "spu_add_claims_stats_details_reins_process"
    Public Const kRepostTransactionsSQL As String = "spu_add_claims_stats_details_reins_process"

    Public Const kIsReversalRequiredName As String = "is claim reversal required"
    Public Const kIsReversalRequiredSQL As String = "spu_is_claim_reversal_required"


    Public Const kGetClaimRiArrangementName As String = "Get Claim RI Arrangement Details"
    Public Const kGetClaimRiArrangementSQL As String = "spu_Claim_RI_Arrangement_saa"

    'AK - Get old policy attached to a claim
    Public Const ACGetClaimOldPolicyStored As Boolean = True
    Public Const ACGetClaimOldPolicyName As String = "Get Claim Old Policy"
    Public Const ACGetClaimOldPolicySQL As String = "spu_get_claim_old_policy"

    'WR08
    Public Const kUpdCLMPaymentRefStored As Boolean = True
    Public Const kUpdCLMPaymentRefName As String = "Update Claim Payment Reference"
    Public Const kUpdCLMPaymentRefSQL As String = "spu_CLM_Update_Payment_Reference"

    Public Const ACSetPaymentForRecommendationStored As Boolean = True
    Public Const ACSetPaymentForRecommendationName As String = "Set Payment for Recommandation"
    Public Const ACSetPaymentForRecommendationSQL As String = "spu_CLM_Set_Payment_for_Recommendation"

    'Get Claim Status(PN-71999 - Sushil Kumar)
    Public Const ACGetClaimStatusStored As Boolean = True
    Public Const ACGetClaimStatusName As String = "Get Claim Status"
    Public Const ACGetClaimStatusSQL As String = "spu_CLM_Get_Claim_Status"

    Public Const kGetClaimTransactionTypeName As String = "Get Claim Transaction Type"
    Public Const kGetClaimTransactionTypeSQL As String = "spu_CLM_Get_transaction_type"

    Public Const ACAddStatsFolderName As String = "AddStatsFolderClaims"
    Public Const ACAddStatsFolderSQL As String = "spu_clm_add_stats_folder"

    Public Const ACAddStatsDetailsName As String = "AddStatsDetailsClaims"
    Public Const ACAddStatsDetailsSQL As String = "spu_clm_add_stats_details_GRS"

    Public Const kGetClaimTaxAmountsByTaxTypeName As String = "returns the tax entries by tax type for the specified payment or receipt"
    Public Const kGetClaimTaxAmountsByTaxTypeSQL As String = "spu_CLM_Get_Claim_Tax_Amounts_By_Tax_Type"

    Public Const ACDoCurrencyConversionName As String = "spu_ACT_Do_Currency_Conversion"
    Public Const ACDoCurrencyConversionSQL As String = "spu_ACT_Do_Currency_Conversion"

    Public Const ACGetReserveAmountName As String = "Get Reserve Amount"
    Public Const ACGetReserveAmountSQL As String = "spu_clm_check_reserve_amount"

    Public Const kGetTransDetailsName As String = "Get Trans details"
    Public Const kGetTransDetailsSQL As String = "spu_clm_get_transdetails"

    Public Const kGetRITransDetailsName As String = "Get RI Trans details"
    Public Const kGetRITransDetailsSQL As String = "spu_clm_get_transdetails_RI"

    Public Const kGetThisClaimReceiptItem As String = "Get This Claim Receipt Item"
    Public Const kGetThisClaimReceiptItemSQL As String = "spu_CLM_Get_ThisReceipt_Item"

    Public Const KGetCLMGetStatsFolderForClaim As String = "Get CLM Get Stats Folder For Claim"
    Public Const KGetCLMGetStatsFolderForClaimSQL As String = "spu_CLM_Get_Stats_Folder_For_Claim"

    Public Const ACGetDetailsForCompletionIntimation As String = "Get Details For Completion Intimation"
    Public Const ACGetDetailsForCompletionIntimationSQL As String = "spu_Get_Details_For_Completion_Intimation"


End Module