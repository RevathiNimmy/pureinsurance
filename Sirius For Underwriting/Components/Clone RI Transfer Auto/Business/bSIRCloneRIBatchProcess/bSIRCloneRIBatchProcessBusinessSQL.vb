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
    ' Date: 26/09/00
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRDeferredRIAuto.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    'Developer Guide No. 39
    Public Const ACSelClonedRIPoliciesStored As Boolean = True
    Public Const ACSelClonedRIPoliciesName As String = "GetClonedRIPolicies"
    Public Const ACSelClonedRIPoliciesSQL As String = "spu_Risks_Cloned_RI_Status_Sel"

    ' For Claim
    Public Const ACSelClonedRIClaimsStored As Boolean = True
    Public Const ACSelClonedRIClaimsName As String = "GetClonedRIClaims"
    Public Const ACSelClonedRIClaimsSQL As String = "spu_Risks_Cloned_RI_Cloned_Claim_Sel"

    ' For all Claim Versions
    Public Const ACSelClonedRIClaimVersionsStored As Boolean = True
    Public Const ACSelClonedRIClaimVersionsName As String = "GetClonedRIClaimVersions"
    Public Const ACSelClonedRIClaimVersionsSQL As String = "spu_Risks_Cloned_RI_Status_ForClaim_Sel"

    Public Const ACUpdCloneRIPoliciesStored As Boolean = True
    Public Const ACUpdCloneRIPoliciesName As String = "UpdateInsFileClonedRIUsage"
    Public Const ACUpdCloneRIPoliciesSQL As String = "spu_cloned_Usage_upd"

    Public Const ACInsertInsClaimCloneRIUsageStored As Boolean = True
    Public Const ACInsertInsClaimCloneRIUsageName As String = "InsertClaimClonedRIUsage"
    Public Const ACInsertInsClaimCloneRIUsageSQL As String = "spu_claim_Cloned_RI_Usage_ins"

    Public Const ACInsertInsFileClonedRIUsageStored As Boolean = True
    Public Const ACInsertInsFileClonedRIUsageName As String = "InsertInsFileClonedRIUsage"
    Public Const ACInsertInsFileClonedRIUsageSQL As String = "spu_Ins_File_Cloned_RI_Usage_ins"

    Public Const ACUpdateRiskRIModelStored As Boolean = True
    Public Const ACUpdateRiskRIModelName As String = "UpdateRiskRIModel"
    Public Const ACUpdateRiskRIModelSQL As String = "spu_Deferred_RI_Change_RI_Model"

    Public Const ACGetRiskStatusStored As Boolean = True
    Public Const ACGetRiskStatusName As String = "GetRiskStatus"
    Public Const ACGetRiskStatusSQL As String = "spu_CLM_risk_status_sel"

    Public Const ACGetRiskClonedRIBandStored As Boolean = True
    Public Const ACGetRiskClonedRIBandName As String = "GetRiskClonedRIBand"
    Public Const ACGetRiskClonedRIBandSQL As String = "spu_GetRiskClonedRIBand"

    Public Const ACMoveClaimToNewRiskStored As Boolean = True
    Public Const ACMoveClaimToNewRiskName As String = "MoveClaimToNewRisk"
    Public Const ACMoveClaimToNewRiskSQL As String = "spu_MoveClaimToNewRisk"

    Public Const ACSelClonedRIPolicyForAmendStored As Boolean = True
    Public Const ACSelClonedRIPolicyForAmendName As String = "Get policies for manual cloned RI amendment"
    Public Const ACSelClonedRIPolicyForAmendSQL As String = "spu_Risks_Cloned_RI_Status_Sel_amend"

    Public Const ACDeletePolicyStored As Boolean = True
    Public Const ACDeletePolicyName As String = "Delete This Policy Version"
    Public Const ACDeletePolicySQL As String = "spu_DeletePolicy"

    Public Const ACDeleteInsFileClonedRIUsageName As String = "Delete Insurance File Cloned RI Usage for specified Insurance File Cnt"
    Public Const ACDeleteInsFileClonedRIUsageSQL As String = "spu_Insurance_File_Cloned_RI_Usage_Del_By_InsFileCnt"
    Public Const ACDeleteInsFileClonedRIUsageStored As Boolean = True

    Public Const ACGetInsuranceRefStored As Boolean = True
    Public Const ACGetInsuranceRefName As String = "Gets the Insurance Referance"
    Public Const ACGetInsuranceRefSQL As String = "spu_Get_Insurance_Ref"

    Public Const ACUpdRIArrangementClonedStatusStored As Boolean = True
    Public Const ACUpdRIArrangementClonedStatusName As String = "UpdateRIArrangementClonedStatus"
    Public Const ACUpdRIArrangementClonedStatusSQL As String = "spu_Update_RIArrangement_ClonedStatus"

    Public Const ACClaimRIArrangementDelStored As Boolean = True
    Public Const ACClaimRIArrangementDelName As String = "ClaimRIArrangementDel"
    Public Const ACClaimRIArrangementDelSQL As String = "spu_Claim_RI_Arrangement_Del_RI2007"

    Public Const ACGetClaimDocumentsForReversalStored As Boolean = True
    Public Const ACGetClaimDocumentsForReversalName As String = "GetClaimDocumentsForReversal"
    Public Const ACGetClaimDocumentsForReversalSQL As String = "spu_Get_ClaimsDocument_ForReversal"

    Public Const ACUpdateClaimClonedRIUsageStored As Boolean = True
    Public Const ACUpdateClaimClonedRIUsageName As String = "UpdateClaimClonedRIUsage"
    Public Const ACUpdateClaimClonedRIUsageSQL As String = "spu_Claim_Cloned_RI_Usage_upd"

    Public Const ACGetHiddenOptionStored As Boolean = True
    Public Const ACGetHiddenOptionName As String = "Get Hidden Option"
    Public Const ACGetHiddenOptionSQL As String = "SPU_GETHIDDENOPTION"

    Public Const ACGetPreviousRiskCntForTransferName As String = "get_Previous_RiskCnt_ForTransfer"
    Public Const ACGetPreviousRiskCntForTransferSQL As String = "spu_get_Previous_RiskCnt_ForTransfer"
    Public Const ACGetPreviousRiskCntForTransferStored As Boolean = True

    Public Const ACGetAutoReinsuredStored As Boolean = False
    Public Const ACGetAutoReinsuredName As String = "Get Auto Reinsured"
    Public Const ACGetAutoReinsuredSQL As String = "SELECT is_auto_reinsured FROM risk_type" & vbCrLf & "WHERE risk_type_id = {risk_type_id}"


    Public Const ACAddRiskStored As Boolean = True
    Public Const ACAddRiskName As String = "AddRisk"
    Public Const ACAddRiskSQL As String = "spe_Risk_add"

    Public Const ACDelRatingSectionStored As Boolean = False
    Public Const ACDelRatingSectionName As String = "DeleteRatingSection"
    Public Const ACDelRatingSectionSQL As String = "Delete From Rating_section where risk_cnt = {risk_cnt} and original_flag=0"

    Public Const ACDelPerilStored As Boolean = False
    Public Const ACDelPerilName As String = "DeletePeril"
    Public Const ACDelPerilSQL As String = " delete from peril where risk_cnt={risk_cnt}" & " and rating_section_id in (SELECT rating_section_id from Rating_Section " & " WHERE risk_cnt={risk_cnt} and original_flag=0)"

    'Select the Rating sections for the given Insurance file and the risk25
    Public Const ACSelectRatingSectionStored As Boolean = True
    Public Const ACSelectRatingSectionName As String = "SelectRatingSection"
    Public Const ACSelectRatingSectionSQL As String = "spu_sir_rating_section_sel_original"

    'Add Section and Perils SQL
    Public Const ACAddSectionAndPerilsStored As Boolean = True
    Public Const ACAddSectionAndPerilsName As String = "AddSectionAndPerils"
    Public Const ACAddSectionAndPerilsSQL As String = "spu_sir_peril_allocation"



    Public Const ACGetAllRiskStatusStored As Boolean = True
    Public Const ACGetAllRiskStatusName As String = "GetAllRiskStatus"
    Public Const ACGetAllRiskStatusSQL As String = "spu_all_risk_status_sel"


    Public Const ACUpdateClaimRIStatusStored As Boolean = True
    Public Const ACUpdateClaimRIStatusName As String = "UpdateClaimClonedRIUsage"
    Public Const ACUpdateClaimRIStatusSQL As String = "spu_Claim_RI_Status_upd"


    Public Const ACReverseClaimStatsStored As Boolean = True
    Public Const ACReverseClaimStatsName As String = "reverse_claim_stats"
    Public Const ACReverseClaimStatsSQL As String = "spu_reverse_claim_stats"

    Public Const kRecalculateRIStored As Boolean = True
    Public Const kRecalculateRIName As String = "spu_recalculate_RI_for_Clone"
    Public Const kRecalculateRISQL As String = "spu_recalculate_RI_for_Clone"

    Public Const kClaimRIArrangementArchiveStored As Boolean = True
    Public Const kClaimRIArrangementArchiveName As String = "ClaimRIArrangementArchive"
    Public Const kClaimRIArrangementArchiveSQL As String = "spu_Claim_RI_Arrangement_Archive"


    Public Const kRecalculateRIQuoteStored As Boolean = True
    Public Const kRecalculateRIQuoteName As String = "spu_recalculate_RI_Quote_for_Clone"
    Public Const kRecalculateRIQuoteSQL As String = "spu_recalculate_RI_Quote_for_Clone"

End Module