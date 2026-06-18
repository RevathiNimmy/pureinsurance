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
    ' Date: 26/09/00
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRDeferredRIAuto.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    'Developer Guide No. 39
    Public Const kSelDefRIPoliciesStored As Boolean = True
    Public Const kSelDefRIPoliciesName As String = "GetDeferredRIPolicies"
    Public Const kSelDefRIPoliciesSQL As String = "spu_Risks_Deferred_RI_Status_Sel"

    Public Const kUpdDefRIPoliciesStored As Boolean = True
    Public Const kUpdDefRIPoliciesName As String = "UpdateDeferredRIPolicies"
    Public Const kUpdDefRIPoliciesSQL As String = "spu_Insurance_File_Deferred_RI_Usage_upd"

    Public Const kDeleteInsFileDefRIUsageStored As Boolean = True
    Public Const kDeleteInsFileDefRIUsageName As String = "DelDeferredRIPolicies"
    Public Const kDeleteInsFileDefRIUsageSQL As String = "spu_Insurance_File_Deferred_RI_Usage_del"

    Public Const kInsertInsFileDefRIUsageStored As Boolean = True
    Public Const kInsertInsFileDefRIUsageName As String = "InsertInsFileDefRIUsage"
    Public Const kInsertInsFileDefRIUsageSQL As String = "spu_Insurance_File_Deferred_RI_Usage_ins"

    Public Const kUpdateRiskRIModelStored As Boolean = True
    Public Const kUpdateRiskRIModelName As String = "UpdateRiskRIModel"
    Public Const kUpdateRiskRIModelSQL As String = "spu_Deferred_RI_Change_RI_Model"

    Public Const kGetRiskStatusStored As Boolean = True
    Public Const kGetRiskStatusName As String = "GetRiskStatus"
    Public Const kGetRiskStatusSQL As String = "spu_CLM_risk_status_sel"

    Public Const kGetDeferredRIBandStored As Boolean = True
    Public Const kGetDeferredRIBandName As String = "GetRiskDeferredRIBand"
    Public Const kGetDeferredRIBandSQL As String = "spu_GetRiskDeferredRIBand"

    Public Const kMoveClaimToNewRiskStored As Boolean = True
    Public Const kMoveClaimToNewRiskName As String = "MoveClaimToNewRisk"
    Public Const kMoveClaimToNewRiskSQL As String = "spu_MoveClaimToNewRisk"

    Public Const kSelDeferredRIPolicyForAmendStored As Boolean = True
    Public Const kSelDeferredRIPolicyForAmendName As String = "Get policies for manual deferred RI amendment"
    Public Const kSelDeferredRIPolicyForAmendSQL As String = "spu_Ins_File_Deferred_RI_Usage_sel_amend"

    Public Const kDeletePolicyStored As Boolean = True
    Public Const kDeletePolicyName As String = "Delete This Policy Version"
    Public Const kDeletePolicySQL As String = "spu_DeletePolicy"

    Public Const kDeleteInsFileDeferredRIUsageName As String = "Delete Insurance File Deferred RI Usage for specified Insurance File Cnt"
    Public Const kDeleteInsFileDeferredRIUsageSQL As String = "spu_Insurance_File_Deferred_RI_Usage_Del_By_InsFileCnt"

    Public Const kUpdateRiskLinkStored As Boolean = False
    Public Const kUpdateRiskLinkName As String = "Update Risk Link"
    Public Const kUpdateRiskLinkSQL As String = "Update insurance_file_risk_link set is_risk_edited = 1 where risk_cnt = {risk_cnt}"


    '''
    Public Const kGetAllClaimsOnRiskStored As Boolean = True
    Public Const kGetAllClaimsOnRiskName As String = "GetAllClaimsOnRisk"
    Public Const kGetAllClaimsOnRiskSQL As String = "spu_get_all_claims_on_risk"

    Public Const kNetOfClaimPerilReserveStored As Boolean = True
    Public Const kNetOfClaimPerilReserveName As String = "NetOfClaimPerilReserve"
    Public Const kNetOfClaimPerilReserveSQL As String = "spu_CLM_NetOf_Claim_Peril_Reserve"

    Public Const kGetClaimPerilsStored As Boolean = True
    Public Const kGetClaimPerilsName As String = "GetClaimPerils"
    Public Const kGetClaimPerilsSQL As String = "spu_SAM_CLM_Get_Claim_Perils"

    Public Const kGetNetOfClaimPerilStored As Boolean = True
    Public Const kGetNetOfClaimPerilName As String = "GetNetOfClaimPeril"
    Public Const kGetNetOfClaimPerilSQL As String = "spu_CLM_Get_NetOf_Claim_Peril"

    Public Const kAddStatsFolderClaimsStored As Boolean = True
    Public Const kAddStatsFolderClaimsName As String = "AddStatsFolderClaims"
    Public Const kAddStatsFolderClaimsSQL As String = "spu_add_stats_folder_claims"

    Public Const kAddStatsDetailsClaimsStored As Boolean = True
    Public Const kAddStatsDetailsClaimsName As String = "AddStatsDetailsClaims"
    Public Const kAddStatsDetailsClaimsSQL As String = "spu_add_stats_details_claims"

    Public Const kUpdateClaimFinaliseStatsStored As Boolean = True
    Public Const kUpdateClaimFinaliseStatsName As String = "UpdateClaimFinaliseStats"
    Public Const kUpdateClaimFinaliseStatsSQL As String = "spu_CLM_Finalise_stats"

    Public Const kUpdateClaimStatusStored As Boolean = True
    Public Const kUpdateClaimStatusName As String = "UpdateClaimStatus"
    Public Const kUpdateClaimStatusSQL As String = "spu_CLM_Claim_Is_Dirty_Update"

    Public Const kUpdateDeferredRIRenewalStatusStored As Boolean = True
    Public Const kUpdateDeferredRIRenewalStatusName As String = "UpdateDeferredRIRenewalStatus"
    Public Const kUpdateDeferredRIRenewalStatusSQL As String = "spu_Update_DeferredRI_Renewal_Status"

    Public Const kDelPerilForDeletedRiskStored As Boolean = False
    Public Const kDelPerilForDeletedRiskName As String = "DeletePerilForDeletedRisk"
    Public Const kDelPerilForDeletedRiskSQL As String = " delete from peril where risk_cnt={risk_cnt}"

    Public Const kDelRatingSectionForDeletedRiskStored As Boolean = False
    Public Const kDelRatingSectionForDeletedRiskName As String = "DeleteRatingSectionForDeletedRisk"
    Public Const kDelRatingSectionForDeletedRiskSQL As String = "Delete From Rating_section where risk_cnt = {risk_cnt}"

    'Select the Rating sections for the given Insurance file and the risk
    Public Const kSelectRatingSectionStored As Boolean = True
    Public Const kSelectRatingSectionName As String = "SelectRatingSection"
    Public Const kSelectRatingSectionSQL As String = "spu_sir_rating_section_sel_original"

    'Add Section and Perils SQL
    Public Const kAddSectionAndPerilsStored As Boolean = True
    Public Const kAddSectionAndPerilsName As String = "AddSectionAndPerils"
    Public Const kAddSectionAndPerilsSQL As String = "spu_sir_peril_allocation"

    Public Const kGetInsuranceRefStored As Boolean = True
    Public Const kGetInsuranceRefName As String = "Gets the Insurance Referance"
    Public Const kGetInsuranceRefSQL As String = "spu_Get_Insurance_Ref"

    Public Const kGetAutoReinsuredStored As Boolean = False
    Public Const kGetAutoReinsuredName As String = "Get Auto Reinsured"
    Public Const kGetAutoReinsuredSQL As String = "SELECT is_auto_reinsured FROM risk_type" & vbCrLf & "WHERE risk_type_id = {risk_type_id}"

    Public Const kAddRiskStored As Boolean = True
    Public Const kAddRiskName As String = "AddRisk"
    Public Const kAddRiskSQL As String = "spe_Risk_add"

    Public Const kGetPreviousRiskCntForTransferName As String = "get_Previous_RiskCnt_ForTransfer"
    Public Const kGetPreviousRiskCntForTransferSQL As String = "spu_get_Previous_RiskCnt_ForTransfer"
    Public Const kGetPreviousRiskCntForTransferStored As Boolean = True


    Public Const kDelRatingSectionStored As Boolean = False
    Public Const kDelRatingSectionName As String = "DeleteRatingSection"
    Public Const kDelRatingSectionSQL As String = "Delete From Rating_section where risk_cnt = {risk_cnt} and original_flag=0"

    Public Const kDelPerilStored As Boolean = False
    Public Const kDelPerilName As String = "DeletePeril"
    Public Const kDelPerilSQL As String = " delete from peril where risk_cnt={risk_cnt}" & " and rating_section_id in (SELECT rating_section_id from Rating_Section " & " WHERE risk_cnt={risk_cnt} and original_flag=0)"

    Public Const kGetMaxVersionStored As Boolean = False
    Public Const kGetMaxVersionName As String = "Get Max Policy Version"
    Public Const kGetMaxVersionSQL As String = " SELECT ISNULL (MAX(ifi.policy_version) ,0)    FROM    insurance_file ifi,  insurance_file ifi2 WHERE ifi.insurance_folder_cnt = ifi2.insurance_folder_cnt  AND ifi.policy_ignore is null AND ifi2.insurance_file_cnt = {Insurance_file_cnt} "

    Public Const ACCopyPolicyAssociatesStored As Boolean = True
    Public Const ACCopyPolicyAssociatesName As String = "CopyPolicyAssociates"
    Public Const ACCopyPolicyAssociatesSQL As String = "spu_sir_copy_insurance_file_associates"


    Public Const kRecalculateRIQuoteStored As Boolean = True
    Public Const kRecalculateRIQuoteName As String = "spu_recalculate_RI_Quote_For_Deferred"
    Public Const kRecalculateRIQuoteSQL As String = "spu_recalculate_RI_Quote_For_Deferred"

    Public Const kDelOriginalRatingSectionStored As Boolean = False
    Public Const kDelOriginalRatingSectionName As String = "DeleteRatingSection"
    Public Const kDelOriginalRatingSectionSQL As String = "Delete From Rating_section where risk_cnt = {risk_cnt} and original_flag=1"

    Public Const kDelOriginalPerilStored As Boolean = False
    Public Const kDelOriginalPerilName As String = "DeletePeril"
    Public Const kDelOriginalPerilSQL As String = " delete from peril where risk_cnt={risk_cnt}" & " and rating_section_id in (SELECT rating_section_id from Rating_Section " & " WHERE risk_cnt={risk_cnt} and original_flag=1)"

End Module