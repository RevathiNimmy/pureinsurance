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
	
	Public Const ACGetPoliciesPortfolioTransferStored As Boolean = True
	Public Const ACGetPoliciesPortfolioTransferName As String = "GetPoliciesPortfolioTransfer"
    Public Const ACGetPoliciesPortfolioTransferSQL As String = "spu_ri_portfolio_policy_Sel"

    Public Const kGetPoliciesForPortfolioTransferRI2007DisabledStored As Boolean = True
    Public Const kGetPoliciesForPortfolioTransferRI2007DisabledName As String = "GetPoliciesForPortfolioTransfer-RI2007Disabled"
    Public Const kGetPoliciesForPortfolioTransferRI2007DisabledSQL As String = "spu_RI2007Disabled_Portfolio_Policy_Sel"

	Public Const ACUpdDefRIPoliciesStored As Boolean = True
	Public Const ACUpdDefRIPoliciesName As String = "UpdateDeferredRIPolicies"
    Public Const ACUpdDefRIPoliciesSQL As String = "spu_Insurance_File_Deferred_RI_Usage_upd"
	
	Public Const ACDeleteInsFileDefRIUsageStored As Boolean = True
	Public Const ACDeleteInsFileDefRIUsageName As String = "DelDeferredRIPolicies"
    Public Const ACDeleteInsFileDefRIUsageSQL As String = "spu_Insurance_File_Deferred_RI_Usage_del"
	
	Public Const ACInsertInsFileDefRIUsageStored As Boolean = True
	Public Const ACInsertInsFileDefRIUsageName As String = "InsertInsFileDefRIUsage"
    Public Const ACInsertInsFileDefRIUsageSQL As String = "spu_Insurance_File_Deferred_RI_Usage_ins"
	
	Public Const ACUpdateRiskRIModelStored As Boolean = True
	Public Const ACUpdateRiskRIModelName As String = "UpdateRiskRIModel"
    Public Const ACUpdateRiskRIModelSQL As String = "spu_Deferred_RI_Change_RI_Model"
	
	Public Const ACGetRiskStatusStored As Boolean = True
	Public Const ACGetRiskStatusName As String = "GetRiskStatus"
    Public Const ACGetRiskStatusSQL As String = "spu_CLM_risk_status_sel"
	
	Public Const ACGetDeferredRIBandStored As Boolean = True
	Public Const ACGetDeferredRIBandName As String = "GetRiskDeferredRIBand"
    Public Const ACGetDeferredRIBandSQL As String = "spu_GetRiskDeferredRIBand"
	
	Public Const ACMoveClaimToNewRiskStored As Boolean = True
	Public Const ACMoveClaimToNewRiskName As String = "MoveClaimToNewRisk"
    Public Const ACMoveClaimToNewRiskSQL As String = "spu_MoveClaimToNewRisk"
	
	Public Const ACIsRiskMarkedForPortfolioTransferStored As Boolean = True
	Public Const ACIsRiskMarkedForPortfolioTransferName As String = "IsRiskMarkedForPortfolioTransfer"
    Public Const ACIsRiskMarkedForPortfolioTransferSQL As String = "spu_is_risk_marked_for_portfolio_transfer"

    '  E007
    Public Const ACDeleteInsFilePTRIUsageStored As Boolean = True
    Public Const ACDeleteInsFilePTRIUsageName As String = "DeleteInsFilePTRIUsage"
    Public Const ACDeleteInsFilePTRIUsageSQL As String = "spu_Insurance_File_PT_RI_Usage_del"

    Public Const ACDeletePolicyStored As Boolean = True
    Public Const ACDeletePolicyName As String = "Delete This Policy Version"
    Public Const ACDeletePolicySQL As String = "spu_DeletePolicy"

    ' E007
    Public Const ACUpdPTRIStatusStored As Boolean = True
    Public Const ACUpdPTRIStatusName As String = "UpdatePTRIStatus"
    Public Const ACUpdPTRIStatusSQL As String = "spu_Insurance_File_PT_RI_Usage_upd"

    ' E007
    Public Const ACGetPTRIPolicyStored As Boolean = True
    Public Const ACGetPTRIPolicyName As String = "GetPTRIPolicy"
    Public Const ACGetPTRIPolicySQL As String = "spu_Ins_File_PT_RI_Usage_sel_amend"

    ' E007
    Public Const ACInsertInsFilePTRIUsageStored As Boolean = True
    Public Const ACInsertInsFilePTRIUsageName As String = "InsertInsFilePTRIUsage"
    Public Const ACInsertInsFilePTRIUsageSQL As String = "spu_PortfolioTransfer_RI_Usage_ins"

    ' E007 Changes
    Public Const ACGetAllClaimsOnRiskStored As Boolean = True
    Public Const ACGetAllClaimsOnRiskName As String = "GetAllClaimsOnRisk"
    Public Const ACGetAllClaimsOnRiskSQL As String = "spu_get_all_claims_on_risk"

    Public Const ACNetOfClaimPerilReserveStored As Boolean = True
    Public Const ACNetOfClaimPerilReserveName As String = "NetOfClaimPerilReserve"
    Public Const ACNetOfClaimPerilReserveSQL As String = "spu_CLM_NetOf_Claim_Peril_Reserve"

    Public Const ACGetClaimPerilsStored As Boolean = True
    Public Const ACGetClaimPerilsName As String = "GetClaimPerils"
    Public Const ACGetClaimPerilsSQL As String = "spu_SAM_CLM_Get_Claim_Perils"

    Public Const ACGetNetOfClaimPerilStored As Boolean = True
    Public Const ACGetNetOfClaimPerilName As String = "GetNetOfClaimPeril"
    Public Const ACGetNetOfClaimPerilSQL As String = "spu_CLM_Get_NetOf_Claim_Peril"

    Public Const ACAddStatsFolderClaimsStored As Boolean = True
    Public Const ACAddStatsFolderClaimsName As String = "AddStatsFolderClaims"
    Public Const ACAddStatsFolderClaimsSQL As String = "spu_add_stats_folder_claims"

    Public Const ACAddStatsDetailsClaimsStored As Boolean = True
    Public Const ACAddStatsDetailsClaimsName As String = "AddStatsDetailsClaims"
    Public Const ACAddStatsDetailsClaimsSQL As String = "spu_add_stats_details_claims"

    Public Const ACUpdateClaimFinaliseStatsStored As Boolean = True
    Public Const ACUpdateClaimFinaliseStatsName As String = "UpdateClaimFinaliseStats"
    Public Const ACUpdateClaimFinaliseStatsSQL As String = "spu_CLM_Finalise_stats"

    Public Const ACUpdateClaimStatusStored As Boolean = True
    Public Const ACUpdateClaimStatusName As String = "UpdateClaimStatus"
    Public Const ACUpdateClaimStatusSQL As String = "spu_CLM_Claim_Is_Dirty_Update"

    Public Const ACGetInsuranceRefStored As Boolean = True
    Public Const ACGetInsuranceRefName As String = "Gets the Insurance Referance"
    Public Const ACGetInsuranceRefSQL As String = "spu_Get_Insurance_Ref"

    Public Const ACGetPreviousRiskCntForTransferName As String = "get_Previous_RiskCnt_ForTransfer"
    Public Const ACGetPreviousRiskCntForTransferSQL As String = "spu_get_Previous_RiskCnt_ForTransfer"
    Public Const ACGetPreviousRiskCntForTransferStored As Boolean = True

    Public Const ACGetAutoReinsuredStored As Boolean = False
    Public Const ACGetAutoReinsuredName As String = "Get Auto Reinsured"
    Public Const ACGetAutoReinsuredSQL As String = "SELECT is_auto_reinsured FROM risk_type WHERE risk_type_id = {risk_type_id}"

    Public Const ACAddRiskStored As Boolean = True
    Public Const ACAddRiskName As String = "AddRisk"
    Public Const ACAddRiskSQL As String = "spe_Risk_add"

    Public Const ACDelRatingSectionStored As Boolean = False
    Public Const ACDelRatingSectionName As String = "DeleteRatingSection"
    Public Const ACDelRatingSectionSQL As String = "Delete From Rating_section where risk_cnt = {risk_cnt} and original_flag=0"

    Public Const ACDelPerilStored As Boolean = False
    Public Const ACDelPerilName As String = "DeletePeril"
    Public Const ACDelPerilSQL As String = " delete from peril where risk_cnt={risk_cnt}" & _
                                         " and rating_section_id in (SELECT rating_section_id from Rating_Section " & _
                                         " WHERE risk_cnt={risk_cnt} and original_flag=0)"

    'Select the Rating sections for the given Insurance file and the risk
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

    Public Const kCreatePolicyVersionForPTStored As Boolean = True
    Public Const kCreatePolicyVersionForPTName As String = "CreatePolicyVersion"
    Public Const kCreatePolicyVersionForPTSQL As String = "spu_sir_copy_policy_for_pt"

    Public Const kGetTMPStatusStored As Boolean = True
    Public Const kGetTMPStatusName As String = "GetAllRiskStatus"
    Public Const kGetTMPStatusSQL As String = "select is_midnight_renewal,is_true_monthly_policy from Product WHERE product_id={product_id}"

    Public Const kRecalculateRIStored As Boolean = True
    Public Const kRecalculateRIName As String = "RecalculateRI"
    Public Const kRecalculateRISQL As String = "spu_recalculate_RI"

    Public Const kFinaliseClaimDetailsName As String = "Finalise_Claim_Details"
    Public Const kFinaliseClaimDetailsSQL As String = "spu_CLM_Finalise_Claim_Details"

    Public Const kGetClaimsPortfolioTransferName As String = "Claim_portfolio_transfer_sel"
    Public Const kGetClaimsPortfolioTransferSQL As String = "spu_Claim_portfolio_transfer_sel"
    Public Const kGetClaimsPortfolioTransferStored As Boolean = True

    Public Const kGetMaxPolicyVersionStored As Boolean = True
    Public Const kGetMaxPolicyVersionName As String = "GetMaxPolicyVersion"
    Public Const kGetMaxPolicyVersionSQL As String = "spu_get_max_policy_version_no"

    Public Const kDelPerilForDeletedRiskStored As Boolean = False
    Public Const kDelPerilForDeletedRiskName As String = "DeletePerilForDeletedRisk"
    Public Const kDelPerilForDeletedRiskSQL As String = " delete from peril where risk_cnt={risk_cnt}"

    Public Const kCheckAndCancelPolicyStored As Boolean = True
    Public Const kCheckAndCancelPolicyName As String = "CheckAndCancelPolicy"
    Public Const kCheckAndCancelPolicySQL As String = "spu_RI_PTCheckAndCancelPolicy"

    Public Const kUpdatePortfolioRenewalStatusStored As Boolean = True
    Public Const kUpdatePortfolioRenewalStatusName As String = "UpdatePortfolioRenewalStatus"
    Public Const kUpdatePortfolioRenewalStatusSQL As String = "spu_Update_Portfolio_Renewal_Status"

    ' Check Renewals SQL
    Public Const kCheckRenewalsStored As Boolean = True
    Public Const kCheckRenewalsName As String = "CheckRenewals"
    Public Const kCheckRenewalsSQL As String = "spu_check_in_renewal"

    Public Const kGetUnderRenewalPoliciesCountStored As Boolean = True
    Public Const kGetUnderRenewalPoliciesCountName As String = "GetPoliciesPortfolioTransfer"
    Public Const kGetUnderRenewalPoliciesCountSQL As String = "spu_count_policies_in_renewal"

    Public Const kDelRatingSectionForDeletedRiskStored As Boolean = False
    Public Const kDelRatingSectionForDeletedRiskName As String = "DeleteRatingSectionForDeletedRisk"
    Public Const kDelRatingSectionForDeletedRiskSQL As String = "Delete From Rating_section where risk_cnt = {risk_cnt}"

    Public Const kUpdateRiskPremiumSQLStored As Boolean = False
    Public Const kUpdateRiskPremiumSQLName As String = "UpdateRiskPremium"
    Public Const kUpdateRiskPremiumSQL As String = "Update Risk Set total_this_premium = 0.00 Where risk_cnt = {risk_cnt}"


End Module