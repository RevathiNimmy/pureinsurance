Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module bSIRAutoMTABusinessSQL

    'SQL Statements

    'Backdated MTA
    Public Const ACGetBackdatedMTAVersionsName As String = "GetBackdatedMTAVersions"
    Public Const ACGetBackdatedMTAVersionsSQL As String = "spu_SIR_Get_BackdatedVersions"
    Public Const ACGetBackdatedMTAVersionsStored As Boolean = True

    Public Const ACUpdBaseInsuranceFileCntName As String = "UpdateBaseInsuranceFileCnt"
    Public Const ACUpdBaseInsuranceFileCntSQL As String = "Spu_SIR_Upd_BaseInsuranceFileCnt"
    Public Const ACUpdBaseInsuranceFileCntStored As Boolean = True

    Public Const ACDeletePolicyVersionName As String = "Delete Policy Version"
    Public Const ACDeletePolicyVersionSQL As String = "spu_DeleteQuote"
    Public Const ACDeleteAllPolicyVersionsSQL As String = "spu_DeleteAllQuotes"
    Public Const ACDeletePolicyVersionStored As Boolean = True

    'Code Added For back Dated MTA PN-71068 

    Public Const ACDeleteLapsePolicy As String = "Delete Lapsed Policy"
    Public Const ACDeleteLapsePolicySql As String = "Spu_SIR_Del_LapsedPolicy"
    Public Const ACDeleteLapsePolicyStored As Boolean = True
    'Code Ends Here PN-71068

    Public Const ACGetPolicyVersionsName As String = "GetPolicyVersions"
    Public Const ACGetPolicyVersionsSQL As String = "SELECT insurance_file_cnt FROM insurance_file WHERE insurance_file_cnt > {IfileCnt}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    " AND insurance_folder_cnt = CASE WHEN {IfolderCnt} = 0 THEN insurance_folder_cnt ELSE  {IfolderCnt} END "
    Public Const ACUpdREPStatusName As String = "Update REP status with NULL"
    'WPR 33-75 added
    Public Const ACUpdREPStatusSQL As String = "Update insurance_file set insurance_file_status_id=NULL from insurance_file " & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "where insurance_file_type_id in(2,4,5) and ISNULL(insurance_file_status_id,0)<>1 and insurance_file_cnt in " & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "(Select original_linked_insurance_file_cnt  " & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "from mta_insurance_file_link where type_ind > 0 and insurance_file_cnt = {IfileCnt})"

    Public Const ACGetOverlappedQuotesName As String = "GetOverlappedQuotes"
    Public Const ACGetOverlappedQuotesSQL As String = "Select insurance_file_cnt from insurance_file where insurance_file_type_id=4 " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "and insurance_folder_cnt= {IfolderCnt} " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "and cover_start_date > (Select cover_start_date from insurance_file where insurance_file_cnt ={IfileCnt})" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "And Insurance_file_cnt in (Select original_linked_insurance_file_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "from mta_insurance_file_link where insurance_file_cnt ={IfileCnt}) " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "And insurance_file_status_id<>4"

    'Delete MTAInsurance_file_links
    Public Const ACDeleteMTAlinksName As String = "DeleteMTAlinks"
    Public Const ACDeleteMTAlinksSQL As String = "Delete from mta_insurance_file_link where insurance_file_cnt = {IfileCnt} "

    Public Const ACDeleteEventLogName As String = "DeleteEventLogName"
    Public Const ACDeleteEventLogSQL As String = "delete Event_log where description like 'REPLACED%' and insurance_file_cnt in (Select original_linked_insurance_file_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "from mta_insurance_file_link where type_ind > 0 and insurance_file_cnt = {IfileCnt})"

    Public Const ACGetPreviousFileName As String = "GetPreviousInsuranceFileCnt"
    Public Const ACGetPreviousFileSQL As String = "spu_GetPreviousInsuranceFileCnt"
    Public Const ACGetPreviousFileStored As Boolean = True

    Public Const ACMoveSuspendedAgentCommissionName As String = "Move Suspended Agent Commission"
    Public Const ACMoveSuspendedAgentCommissionSQL As String = "spu_ACT_Move_Suspended_Agent_Commission"
    Public Const ACMoveSuspendedAgentCommissionStored As Boolean = True

    Public Const ACOutOfSequenceMTADetailsName As String = "Out of Sequence MTA Details"
    Public Const ACOutOfSequenceMTADetailsSQL As String = "spu_Get_out_of_sequence_mta_details"
    Public Const ACOutOfSequenceMTADetailsStored As Boolean = True

    Public Const ACGetTransIDForReversalName As String = "Get Trans ID For Reversal"
    Public Const ACGetTransIDForReversalSQL As String = "spu_Get_TransactionID_For_Reversal"
    Public Const ACGetTransIDForReversalStored As Boolean = True

    Public Const ACUpdateCommentName As String = "Update Comment"
    Public Const ACUpdateCommentSQL As String = "spu_Update_Comment_Of_Reversed_Transaction"
    Public Const ACUpdateCommentStored As Boolean = True

    'Added for PN-71588
    Public Const ACGetPolicyInfo As String = "GetPolicyInfo"
    Public Const ACGetPolicyInfoSQL As String = "Select source_id,currency_ID From insurance_file Where insurance_file_cnt={InsfileCnt}"
    'End for PN-71588
    Public Const ACUpdCANStatusName As String = "Update REP status with cancelled"
    Public Const ACUpdCANStatusSQL As String = "Update insurance_file set insurance_file_status_id=1 from insurance_file " & vbCrLf & _
    "where insurance_file_type_id in(2, 5, 8, 9) and " & vbCrLf & _
    "insurance_folder_cnt in (Select insurance_folder_cnt " & vbCrLf & _
    "from insurance_file where insurance_file_cnt = {IfileCnt}) AND insurance_file_cnt < {IfileCnt}"

    'WPR 33-75 added
    Public Const ACCheckIfLapsedName As String = "Check If lapsed"
    Public Const ACCheckIfLapsedSQL As String = "spu_sir_check_policy_version_lapsed"

    Public Const ACGetAffectedBackDatedMTAVersionsName As String = "Get Affected BackDated MTA Versions"
    Public Const ACGetAffectedBackDatedMTAVersionsSQL As String = "spu_Get_AffectedBackDatedMTAVersions"
    Public Const ACGetAffectedBackDatedMTAVersionsStored As Boolean = True

    Public Const ACUpdateIsDirtyForBackDatedVersionsName As String = "Update IsDirty Flag for Backdated Versions"
    Public Const ACUpdateIsDirtyForBackDatedVersionsSQL As String = "spu_SIR_BackDated_IsDirty_Update"
    Public Const ACUpdateIsDirtyForBackDatedVersionsStored As Boolean = True

    Public Const ACGetPreviousRiskCntForBackDatedMTAName As String = "get_Previous_RiskCnt_ForBackDatedMTA"
    Public Const ACGetPreviousRiskCntForBackDatedMTASQL As String = "spu_get_Previous_RiskCnt_ForBackDatedMTA"
    Public Const ACGetPreviousRiskCntForBackDatedMTAStored As Boolean = True

    Public Const ACGetPostRiskCntName As String = "GetOverlappedQuotes"
    Public Const ACGetPostRiskCntSQL As String = "Select ifrl.risk_cnt From insurance_file_risk_link ifrl " & vbCrLf & "Where Insurance_file_cnt = {iFileCnt} AND " & vbCrLf & "Risk_cnt IN (Select r2.risk_cnt From Risk r1 with (nolock) " & vbCrLf & "Inner Join Risk r2 with (nolock) ON r1.risk_folder_cnt = r2.risk_folder_cnt Where r1.risk_cnt = {risk_cnt})"

    Public Const ACGetPolicyRisksName As String = "GetPolicyRisks"
    Public Const ACGetPolicyRisksSQL As String = "SPU_SIR_GetBackdatedPolicyRisks"
    Public Const ACGetPolicyRisksStored As Boolean = True

    Public Const ACUpdRiskStatusName As String = "Update Risk status"
    Public Const ACUpdRiskStatusSQL As String = "UPDATE Risk SET total_this_premium=0, total_annual_premium=0, risk_status_id=4" & vbCrLf & "Where risk_folder_Cnt in (Select r.risk_folder_cnt From risk r" & vbCrLf & "Inner Join insurance_file_risk_link ifrl ON ifrl.risk_cnt = r.risk_cnt" & vbCrLf & "Where ifrl.insurance_file_cnt = {iFileCnt})" & vbCrLf & "AND risk_cnt IN (Select ifrl.risk_cnt From Insurance_file ifi" & vbCrLf & "Inner Join insurance_file_risk_link ifrl ON ifrl.insurance_file_Cnt = ifi.insurance_file_cnt" & vbCrLf & "Where ifi.base_insurance_file_cnt = {iFileCnt} AND ifi.insurance_file_status_id IS NULL AND ifi.insurance_file_cnt <> {iFileCnt})"
    Public Const ACUpdLapsedStatusName As String = "Update Lapsed"
    Public Const ACUpdLapsedStatusSQL As String = "spu_SIR_Lapse_OOS_versions"

    Public Const ACGETInsuranceFileTypeIDAndCodeName As String = "GetInsuranceFileTypeIdAndCode"
    Public Const ACGETInsuranceFileTypeIDAndCodeNameSQL As String = "spu_get_insurance_file_type_from_InsuranceFileCnt"
    Public Const ACGETInsuranceFileTypeIDAndCodeStored As Boolean = True

    Public Const ACUPDATEInsuranceFileTypeIdFromCodeName As String = "UpdateInsuranceFileTypeId"
    Public Const ACUPDATEInsuranceFileTypeIdFromCodeSQL As String = "spu_update_insurance_file_type_Id_from_code"
    Public Const ACUPDATEInsuranceFileTypeIdFromCodeStored As Boolean = True

    Public Const ACGetFuturePolicyVersionsName As String = "GetFuturePolicyVersions"
    Public Const ACGetFuturePolicyVersionsSQL As String = "spu_SIR_GetFuturePolicyVersions"
    Public Const ACGetFuturePolicyVersionsStored As Boolean = True

    Public Const ACUpdateRisksInRiskFolderName As String = "spuUpdateRisksInRiskFolder"
    Public Const ACUpdateRisksInRiskFolderSQL As String = "spu_Update_Risks_In_RiskFolder"
    Public Const ACUpdateRisksInRiskFolderStored As Boolean = True

    Public Const ACRestoreMTALinkName As String = "RestoreMTALink"
    Public Const ACRestoreMTALinkSQL As String = "spu_SIR_Restore_MTA_Link"
    Public Const ACRestoreMTALinkStored As Boolean = True

    Public Const ACCopyRiskLinksStored As Boolean = True
    Public Const ACCopyRiskLinksName As String = "ACRefreshRiskLinks"
    Public Const ACCopyRiskLinksSQL As String = "spu_sir_refresh_ifrl"

    Public Const ACUpdateInsuranceFileReplacedStatusStored As Boolean = True
    Public Const ACUpdateInsuranceFileReplacedStatusName As String = "UpdateInsuranceFileReplacedStatus"
    Public Const ACUpdateInsuranceFileReplacedStatusSQL As String = "spu_Update_Insurance_File_Replaced_Status"

    Public Const ACUpdInsuranceFileDetailsOOSReinsStored As Boolean = True
    Public Const ACUpdInsuranceFileDetailsOOSReinsName As String = "Update_Insurance_File_Details_OOS_Reinstate"
    Public Const ACUpdInsuranceFileDetailsOOSReinsSQL As String = "spu_Update_Insurance_File_Details_OOS_Reinstate"

    Public Const ACUpdInsuranceFileDetailsStored As Boolean = True
    Public Const ACUpdInsuranceFileDetailsName As String = "Update_Insurance_File_Details"
    Public Const ACUpdInsuranceFileDetailsSQL As String = "spu_Update_Insurance_File_Details"

    Public Const ACUpdateInsuranceFileTypeIdName As String = "Update Insurance File Type Id"
    Public Const ACUpdateInsuranceFileTypeIdSQL As String = "spu_Update_Insurance_File_type_id"
    Public Const ACUpdateInsuranceFileTypeIdStored As Boolean = True

    Public Const ACGetInsuranceFileTypeIDName As String = "Get Insurance File Type ID"
    Public Const ACGetInsuranceFileTypeIDSQL As String = "spu_Get_Insurance_File_Type_ID"
    Public Const ACGetInsuranceFileTypeIDStored As Boolean = True

    Public Const ACGetInsuranceFileTypeName As String = "Get Insurance File Type"
    Public Const ACGetInsuranceFileTypeSQL As String = "spu_SAM_Get_Insurance_File_Type"
    Public Const ACGetInsuranceFileTypeStored As Boolean = True

    Public Const ACUpdateRiskSelectionStatusStored As Boolean = True
    Public Const ACUpdateRiskSelectionStatusName As String = "UpdateRiskSelectionStatus"
    Public Const ACUpdateRiskSelectionStatusSQL As String = "spu_update_risk_sel_status"

    Public Const kReverseRiskLevelTaxesName As String = "ReverseRiskTaxes"
    Public Const kReverseRiskLevelTaxesSQL As String = "spu_risk_tax_rev"

    Public Const kReverseRiskLevelFeeName As String = "ReverseRiskFee"
    Public Const kReverseRiskLevelFeeSQL As String = "spu_sir_risk_fee_rev"

    Public Const kReversePolicyLevelTaxesName As String = "ReversePolicyLevelFee"
    Public Const kReversePolicyLevelTaxesSQL As String = "spu_Insurance_file_tax_rev"

    Public Const ACCopyPolicyAssociatesName As String = "CopyPolicyAssociates"
    Public Const ACCopyPolicyAssociatesSQL As String = "spu_SIR_copy_insurance_file_associates"

End Module