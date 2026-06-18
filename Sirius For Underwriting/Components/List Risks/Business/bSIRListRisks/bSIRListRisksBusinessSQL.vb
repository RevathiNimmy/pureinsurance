Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
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
    ' Date: 30/10/2002
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRListRisks.Business class.
    '
    ' ***************************************************************** '

    'SQL Statements

    ' Get next risk no
    Public Const ACGetNextRiskNoStored As Boolean = True
    Public Const ACGetNextRiskNoName As String = "GetNextRiskNumber"
    'Developer Guide No 39. 
    Public Const ACGetNextRiskNoSQL As String = "spu_get_next_risk_no"

    ' Update risk no
    Public Const ACUpdateRiskNoStored As Boolean = True
    Public Const ACUpdateRiskNoName As String = "UpdateRiskNumber"
    'Developer Guide No 39.
    Public Const ACUpdateRiskNoSQL As String = "spu_update_risk_no"

    ' Get next risk variation no
    Public Const ACGetNextRiskVarStored As Boolean = True
    Public Const ACGetNextRiskVarName As String = "GetNextRiskVariationNumber"
    'Developer Guide No 39.
    Public Const ACGetNextRiskVarSQL As String = "spu_get_next_risk_var_no"

    ' Get current risk no
    Public Const ACGetCurRiskNoStored As Boolean = True
    Public Const ACGetCurRiskNoName As String = "GetCurrentRiskNumber"
    Public Const ACGetCurRiskNoSQL As String = "spu_Get_risk_cnt_for_original_risk_cnt"

    ' Update risk no
    Public Const ACUpdateRiskVariationStored As Boolean = True
    Public Const ACUpdateRiskVariationName As String = "UpdateRiskVariationNumber"
    'Developer Guide No 39.
    Public Const ACUpdateRiskVariationSQL As String = "spu_update_risk_var_no"

    ' Update risk selection status
    Public Const ACUpdateRiskSelectionStatusStored As Boolean = True
    Public Const ACUpdateRiskSelectionStatusName As String = "UpdateRiskSelectionStatus"
    'Developer Guide No 39.
    Public Const ACUpdateRiskSelectionStatusSQL As String = "spu_update_risk_sel_status"

    ' Update flagged quote
    Public Const ACUpdateFlaggedQuoteStored As Boolean = True
    Public Const ACUpdateFlaggedQuoteName As String = "UpdateFlaggedQuote"
    'Developer Guide No 39.
    Public Const ACUpdateFlaggedQuoteSQL As String = "spu_update_flagged_quote"

    ' Delete flagged quote
    Public Const ACDeleteFlaggedQuoteStored As Boolean = True
    Public Const ACDeleteFlaggedQuoteName As String = "UpdateFlaggedQuoteDel"
    'Developer Guide No 39.
    Public Const ACDeleteFlaggedQuoteSQL As String = "spu_update_flagged_quote"

    ' Get follow up time frame
    Public Const ACGetFollowUpTimeFrameStored As Boolean = True
    Public Const ACGetFollowUpTimeFrameName As String = "GetFollowUpTimeFrameNumber"
    'Developer Guide No 39.
    Public Const ACGetFollowUpTimeFrameSQL As String = "spu_get_follow_up_time_frame"

    'Update is_risk_edited field in insurance_file_risk_link table
    Public Const UpdateIFRLinkRiskName As String = "UpdateIFRLinkRiskEdited"
    Public Const UpdateIFRLinkRiskSQL As String = "spu_Update_IFRLink_Risk_Edited"
    Public Const UpdateIFRLinkRiskStored As Boolean = True

    Public Const kGetInsuranceFileDetailsName As String = "Returns the required insurance file details for the specified insurance file id"
    Public Const kGetInsuranceFileDetailsSQL As String = "spu_SIR_Get_Insurance_File_Details"

    Public Const kUpdateRiskSelectionName As String = "Update the selection status of the specified risk"
    Public Const kUpdateRiskSelectionSQL As String = "spu_SIR_Risk_Selection_Status_Update"

    Public Const kUpdatePolicyDetailsName As String = "Update the specified policy details"
    Public Const kUpdatePolicyDetailsSQL As String = "spu_SIR_Update_Policy_Details"

    ' policy discount
    Public Const kGetPolicyDiscountRisksName As String = "returns the risks for the specified policy"
    Public Const kGetPolicyDiscountRisksSQL As String = "spu_SIR_Policy_Discount_Get_Risks"

    Public Const kRecalculateRiskFeesName As String = "recalculates the fees for the risks associated with this policy"
    Public Const kRecalculateRiskFeesSQL As String = "spu_SIR_Policy_Discount_Recalculate_Risk_Fees"

    Public Const kGetPolicyDiscountTotalPremiumName As String = "recalculates the fees for the risks associated with this policy"
    Public Const kGetPolicyDiscountTotalPremiumSQL As String = "spu_SIR_Policy_Discount_Get_Total_Premium"

    Public Const kApplyPolicyDiscountName As String = "apply the discount percentage to the this_premium field on the associated rating sections"
    Public Const kApplyPolicyDiscountSQL As String = "spu_SIR_Policy_Discount_Apply"

    Public Const kAdjustPolicyDiscountName As String = "adjust the discount applied to the last risk to bring the current total premium in line with the specified discounted premium"
    Public Const kAdjustPolicyDiscountSQL As String = "spu_SIR_Policy_Discount_Adjust"

    Public Const kUpdatePolicyPremiumName As String = "Update Policy Premium from values discounted risks"
    Public Const kUpdatePolicyPremiumSQL As String = "spu_Upd_Policy_Premium"

    Public Const kUpdateRiskPremiumName As String = "Update risk premiums from rating section for risks associated with the specified policy"
    Public Const kUpdateRiskPremiumSQL As String = "spu_SIR_Policy_Discount_Update_Policy_Risk_Values"

    Public Const kAdjustValuesFeesName As String = "discount the value based fees by the discounted percentage"
    Public Const kAdjustValuesFeesSQL As String = "spu_SIR_Policy_Discount_Adjust_Values_Fees"

    Public Const kAdjustValuesTaxesName As String = "discount the value based taxes by the discounted percentage"
    Public Const kAdjustValuesTaxesSQL As String = "spu_SIR_Policy_Discount_Adjust_Values_Taxes"

    Public Const kGetPolicyDiscountRequiredInfoName As String = "returns all details required by policy discount processing"
    Public Const kGetPolicyDiscountRequiredInfoSQL As String = "spu_SIR_Policy_Discount_Get_Required_Info"

    Public Const kUpdateRiskDetailsName As String = "update risks is_discounted indicator"
    Public Const kUpdateRiskDetailsSQL As String = "spu_SIR_Policy_Discount_Update_Risk_Details"

    Public Const kRollbackPolicyDiscountName As String = "reset rating sections this premium to original premiums"
    Public Const kRollbackPolicyDiscountSQL As String = "spu_SIR_Policy_Discount_Rollback"

    Public Const kClearRatingsDiscountRelatedDetailsName As String = "reset original premium discounts to "
    Public Const kClearRatingsDiscountRelatedDetailsSQL As String = "spu_SIR_Policy_Discount_Process_Make_Live_Ratings"

    Public Const kProcessPolicyMakeLiveRisksName As String = "save premium_this_year details and reset is_discounted indicator"
    Public Const kProcessPolicyMakeLiveRisksSQL As String = "spu_SIR_Policy_Discount_Process_Make_Live_Risks"

    Public Const kAddPerilsName As String = "deletes and recreates the perils"
    Public Const kAddPerilsSQL As String = "spu_SIR_Policy_Discount_Recreate_Perils"

    Public Const kGetInvalidPolicyDiscountRisksName As String = "returns infomation for invalid risks"
    Public Const kGetInvalidPolicyDiscountRisksSQL As String = "spu_SIR_Policy_Discount_Get_Invalid_Risks"

    Public Const kUpdateRiskStatusName As String = "update the specified risks with the specified risk status"
    Public Const kUpdateRiskStatusSQL As String = "spu_SIR_Update_Risk_Status"

    Public Const kUpdateRiskTaxPremiumName As String = "updates the risks tax premiums based on newly discounted / loaded premiums"
    Public Const kUpdateRiskTaxPremiumSQL As String = "spu_SIR_Policy_Discount_Update_Risk_Tax_Premium"

    Public Const kUpdatePolicyTaxPremiumName As String = "updates the policy tax premiums based on newly discounted / loaded premiums"
    Public Const kUpdatePolicyTaxPremiumSQL As String = "spu_SIR_Policy_Discount_Update_Policy_Tax_Premium"

    Public Const kGetLookupsByEffectiveDateName As String = "Returns lookups by effective date"
    Public Const kGetLookupsByEffectiveDateSQL As String = "spu_SIR_Get_Lookup_Values_By_Effective_Date"

    Public Const ACGetRiskForPolicyIDStored As Boolean = False
    Public Const ACGetRiskForPolicyIDName As String = "Get risks status for policy id"
    Public Const ACGetRiskForPolicyIDSQL As String = "SELECT r.risk_cnt, r.risk_status_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "FROM Insurance_File_Risk_Link ifrl JOIN Risk r ON ifrl.risk_cnt = r.risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "WHERE ifrl.insurance_file_cnt = {InsuranceFileCnt}"

    Public Const ACUpdRiskStatusStored As Boolean = False
    Public Const ACUpdRiskStatusName As String = "Update risk status id"
    Public Const ACUpdRiskStatusSQL As String = "UPDATE Risk" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "SET risk_status_id = {RiskStatusID}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "WHERE risk_cnt = {RiskID}"

    Public Const ACUpdResetRiskStatusForPolicyIDStored As Boolean = True
    Public Const ACUpdResetRiskStatusForPolicyIDName As String = "Reset risk status for policy id"
    'Developer Guide No 39.
    Public Const ACUpdResetRiskStatusForPolicyIDSQL As String = "spu_update_risk_status_unquoted_mtc"

    Public Const ACUpdRiskSelectedStored As Boolean = False
    Public Const ACUpdRiskSelectedName As String = "Update selected flag"
    Public Const ACUpdRiskSelectedSQL As String = "UPDATE Risk SET is_risk_selected = {IsSelect} WHERE risk_cnt = {RiskCnt}"

    '***
    Public Const ACCopyRiskRIDetailsStored As Boolean = True
    Public Const ACCopyRiskRIDetailsName As String = "Copy Risk Reinsurance"
    Public Const ACCopyRiskRIDetailsSQL As String = "spu_RI_arrangement_duplicate"

    Public Const ACDelRatingSectionStored As Boolean = False
    Public Const ACDelRatingSectionName As String = "DeleteRatingSection"
    Public Const ACDelRatingSectionSQL As String = "Delete From Rating_section where risk_cnt = {risk_cnt}"

    'Select the Rating sections for the given Insurance file and the risk
    Public Const ACSelectRatingSectionStored As Boolean = True
    Public Const ACSelectRatingSectionName As String = "SelectRatingSection"
    'Developer Guide No 39.
    Public Const ACSelectRatingSectionSQL As String = "spu_sir_rating_section_sel_original"

    'Add Section and Perils SQL
    Public Const ACAddSectionAndPerilsStored As Boolean = True
    Public Const ACAddSectionAndPerilsName As String = "AddSectionAndPerils"
    Public Const ACAddSectionAndPerilsSQL As String = "spu_sir_peril_allocation"

    'Get Prorata Rate
    Public Const ACGetProrataRateStored As Boolean = True
    Public Const ACGetProrataRateName As String = "Get ProRata Rate"
    Public Const ACGetProrataRateSQL As String = "spu_get_pro_rata_rate"

    'Get Prorata Flag
    Public Const ACGetProrataFlagStored As Boolean = True
    Public Const ACGetProrataFlagName As String = "Get ProRata Flag"
    Public Const ACGetProrataFlagSQL As String = "spu_get_prorata_flag"

    'Get ProrataRate For unedited Risk from original risk cnt
    Public Const ACGetProrataRateForUneditedRiskStored As Boolean = True
    Public Const ACGetProrataRateForUneditedRiskName As String = "Get ProRata Rate For Unedited Risk"
    Public Const ACGetProrataRateForUneditedRiskSQL As String = "spu_get_ProrataRate_for_UneditedRisk"

    'Update the risk with the values from the perils
    Public Const ACUpdateRiskStored As Boolean = True
    Public Const ACUpdateRiskName As String = "UpdateRisk"
    Public Const ACUpdateRiskSQL As String = "spu_update_risk_values"

    'Select the data model code for the given risk
    Public Const ACSelectGISDataModelStored As Boolean = False
    Public Const ACSelectGISDataModelName As String = "SelectGISDataModel"
    Public Const ACSelectGISDataModelSQL As String = "select code,GPL.* from gis_data_model GDM ,gis_policy_link GPL " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "Where GDM.Gis_data_model_id=GPL.Gis_data_model_id " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "AND GPL.risk_id= {Risk_cnt}"

    'Get Insurance Folder Details
    Public Const ACGetInsFolderCntStored As Boolean = False
    Public Const ACGetInsFolderCntName As String = "GetInsuranceFolder"
    Public Const ACGetInsFolderCntSQL As String = "select insurance_folder_cnt from insurance_file where insurance_file_cnt = {insurance_file_cnt} "

    'Get Gis Policy LinkID
    Public Const ACSelectGISPolicyLinkIDStored As Boolean = False
    Public Const ACSelectGISPolicyLinkIDName As String = "SelectGISPolicyLinkID"
    Public Const ACSelectGISPolicyLinkIDSQL As String = "select gis_policy_link_id from Gis_policy_link where risk_id = {risk_cnt}"

    'Update Ri Details
    Public Const ACUpdateRIDetailsStored As Boolean = True
    Public Const ACUpdateRIDetailsName As String = "UpdateRIDetails"
    Public Const ACUpdateRIDetailsSQL As String = "spu_RI_Arrangement_Details_upd"
    '***

    'Float Balance and Pre-Payment (RC)
    Public Const ACGetPaymentTermsName As String = "gets PaymentTerms"
    Public Const ACGetPaymentTermsSQL As String = "spu_SIR_Get_Payment_Terms"

    'Get MTA Payment Term
    Public Const ACGetMTAPaymentTermsName As String = "gets PaymentTerms"
    Public Const ACGetMTAPaymentTermsSQL As String = "spu_SIR_Get_MTA_Payment_Terms"

    'Get Payment Method
    Public Const ACGetPaymentMethodStored As Boolean = True
    Public Const ACGetPaymentMethodName As String = "Get Payment Method"
    Public Const ACGetPaymentMethodSQL As String = "spu_Get_Payment_Method"

    'Check for the existence of Instalment Scheme for the Product
    Public Const ACGetInstalmentSchemeStored As Boolean = False
    Public Const ACGetInstalmentSchemeName As String = "Get Instalment Scheme for Product"
    Public Const ACGetInstalmentSchemeSQL As String = "SELECT count(*) FROM PFSchemeProducts WHERE product_id = (Select Product_id from insurance_file where insurance_file_cnt= {insurance_file_cnt})"

    Public Const kUpdatePolicyDiscountName As String = "Update Policy Discount from values discounted risks"
    Public Const kUpdatePolicyDiscountSQL As String = "spu_Update_Insurance_File_Discount"

    Public Const kIsDiscountAppliedName As String = "Is Discount Applied"
    Public Const kIsDiscountAppliedSQL As String = "spu_IsDiscountApplied"

    Public Const ACSelectAgentCodeForInsuranceFileCntStored As Boolean = False
    Public Const ACSelectAgentCodeForInsuranceFileCntName As String = "SelectAgentCode"
    Public Const ACSelectAgentCodeForInsuranceFileCntSQL As String = "Select pat.Code , " &
                                                                     "is_float_balance_account,is_overdraft_account, " &
                                                                     "float_balance_limit , overDraft_limit ,pa.Overdraft_expiry FROM " &
                                                                     "Insurance_file ifl LEFT JOIN party_agent pa ON " &
                                                                     "ifl.lead_agent_cnt=pa.party_cnt LEFT JOIN " &
                                                                     "Party_agent_type pat ON pa.party_agent_type_id=pat.party_agent_type_id " &
                                                                     "Where insurance_file_cnt = {InsuranceFileCnt}"

    '(RC) IH-UDPP
    Public Const ACGetOpenPostingPeriodsName As String = "gets Open Posting Periods"
    'Developer Guide No 39.
    Public Const ACGetOpenPostingPeriodsSQL As String = "spu_SIR_Select_Open_Posting_Periods"

    Public Const ACUpdatePolicyPostingPeriodName As String = "Updates Policy Posting Period"
    'Developer Guide No 39.
    Public Const ACUpdatePolicyPostingPeriodSQL As String = "spu_SIR_Update_Policy_Posting_Period"

    Public Const ACGetUserCanOverridePostingPeriodName As String = "Gets UserCanOverride PostingPeriod"
    'Developer Guide No 39.
    Public Const ACGetUserCanOverridePostingPeriodSQL As String = "spu_SIR_Get_UserCanOverride_PostingPeriod"

    Public Const kGetPolicyVersionDetailsName As String = "Returns the policy version details for the specified insurance_file_cnt"
    Public Const kGetPolicyVersionDetailsSQL As String = "spu_SIR_Select_Policy_Version"

    Public Const kGetPolicyCoverDatesAndProductIDFromRiskName As String = "GetPolicyCoverDatesAndProductIDFromRisk"
    Public Const kGetPolicyCoverDatesAndProductIDFromRiskSQL As String = "Spu_Get_PolicyCoverDatesAndProductIDFromRisk"

    'START (SUR) PN - 43045
    Public Const ACGetStatsFolderCount As String = "gets Stats Folder Count"
    Public Const ACGetStatsFolderCountSQL As String = "spu_get_stats_folder_count"
    'END PN - 43045

    Public Const ACAutoRenFlagName As String = "AutoRenFlag"
    Public Const ACAutoRenFlagSQL As String = "spu_SIR_GetAutoRenewalFlag"
    Public Const ACAutoRenFlagStored As Boolean = True

    Public Const kGetAgentDetailsName As String = "Returns the Lead Agent details for the specified insurance_file_cnt"
    Public Const kGetAgentDetailsSQL As String = "spu_Get_Agent"

    'PN52000
    Public Const kGetMTAQuotePolicyVersionsName As String = "GETMTAQuotePolicyVersions"
    Public Const kGetMTAQuotePolicyVersionsSQL As String = "spu_SIR_Get_MTAQuotePolicyVersions"

    ''Start(Saurabh Agrawal) Out of sequence MTA bug fixing
    Public Const ACGetPMwrkTaskId As String = "GetPMwrkTaskId"
    Public Const ACGetPMwrkTaskIdStored As Boolean = True
    Public Const ACGetPMwrkTaskIdSQL As String = "spu_get_PMWrk_task_ID"
    ''End(Saurabh Agrawal) Out of sequence MTA bug fixing

    Public Const kGetPremiumDetailsForAllPolicyVersionsName As String = "GetPremiumDetailsForAllPolicyVersions"
    Public Const kGetPremiumDetailsForAllPolicyVersionsSQL As String = "spu_Get_Premium_Details_For_All_Policy_Versions"

    'Start - Sankar - (WPRvb64 Media Type Status) - Paralleling
    Public Const kCheckPolicyReceiptMediaTypeStatusStored As Boolean = True
    Public Const kCheckPolicyReceiptMediaTypeStatusName As String = "Check Policy Receipt MediaTypeStatus For Refund"
    Public Const kCheckPolicyReceiptMediaTypeStatusSQL As String = "spu_SIR_Check_Policy_Receipt_MediaType_Status"
    'End - Sankar - (WPRvb64 Media Type Status) - Paralleling

    ' Select Risk Details SQL
    Public Const kGetRiskDetailsStored As Boolean = True
    Public Const kGetRiskDetailsName As String = "SelectRiskDetails"
    Public Const kGetRiskDetailsSQL As String = "spe_Risk_sel"

    ' Select Risk Group Details SQL
    Public Const kGetRiskFolderDetailsStored As Boolean = True
    Public Const kGetRiskFolderDetailsName As String = "SelectRiskFolderDetails"
    Public Const kGetRiskFolderDetailsSQL As String = "spe_Risk_Folder_sel"

    Public Const kInsertRiskFolderDetailsStored As Boolean = True
    Public Const kInsertRiskFolderDetailsName As String = "InsertRiskFolderDetails"
    Public Const kInsertRiskFolderDetailsSQL As String = "spe_Risk_Folder_add"

    Public Const kGetUpdateRiskFolderDetailsStored As Boolean = True
    Public Const kGetUpdateRiskFolderDetailsName As String = "Update_risk_folder_for_risk"
    Public Const kGetUpdateRiskFolderDetailsSQL As String = "spu_update_risk_folder_for_risk"

    Public Const ACGetandValidateSubAgentDetailStored As Boolean = True
    Public Const ACGetandValidateSubAgentDetailName As String = "Get Sub Agent Detail"
    Public Const ACGetandValidateSubAgentDetailSQL As String = "spu_Get_Sub_Agent_Detail"

    Public Const ACGetSubAgentDetailsStored As Boolean = True
    Public Const ACGetSubAgentDetailsName As String = "Get Sub Agent Detail Via Insurance File"
    Public Const ACGetSubAgentDetailsSQL As String = "spu_Select_SubAgents"

    Public Const kGetNoOfPoliciesForSingleInsAgentName As String = "Returns the No of Policies for a single instalment agent"
    Public Const kGetNoOfPoliciesForSingleInsAgentSQL As String = "spu_Get_No_Of_Policies_On_Agent"

    Public Const ACGetTransNBAccountIdForInsuranceFileCntName As String = "GetTransNBAccountId"
    Public Const ACGetTransNBAccountIdForInsuranceFileCntSQL As String = "SELECT top 1 intermediary_agent_account_id from Insurance_File where insurance_folder_cnt={InsuranceFolderCnt} and insurance_file_type_id=2 order by insurance_file_cnt"

    Public Const kGetPrepaymentOPtionVal As String = "Get Pre Payment Value"
    Public Const kGetPrepaymentOPtionValSQL As String = "spu_Get_PrePaymentOptionValue"


    ' WPR53
    Public Const kGetRiskTypeDetailsStored As Boolean = True
    Public Const kGetRiskTypeDetailsName As String = "Get RiskType Details"
    Public Const kGetRiskTypeDetailsSQL As String = "spe_Risk_Type_sel"

    Public Const kGetMandatoryRiskTypeDetailsStored As Boolean = True
    Public Const kGetMandatoryRiskTypeDetailsName As String = "Get Mandatory RiskType Details"
    Public Const kGetMandatoryRiskTypeDetailsSQL As String = "spu_SIR_Mandatory_Risk_Sel"

    Public Const kUpdateGISPolicyLinkStored As Boolean = True
    Public Const kUpdateGISPolicyLinkName As String = "Update GIS Policy Link"
    Public Const kUpdateGISPolicyLinkSQL As String = "spu_SIR_Update_GIS_Policy_Link"

    Public Const kUpdateMandatoryRiskStored As Boolean = True
    Public Const kUpdateMandatoryRiskName As String = "Update Mandatory Risk"
    Public Const kUpdateMandatoryRiskSQL As String = "spu_SIR_Update_Mandatory_Risk"

    Public Const kUpdateMandatoryRiskDetailsStored As Boolean = True
    Public Const kUpdateMandatoryRiskDetailsName As String = "Update Mandatory Risk Details"
    Public Const kUpdateMandatoryRiskDetailsSQL As String = "spu_SIR_Update_Mandatory_Risk_Details"

    Public Const kGetMandatoryRiskStored As Boolean = True
    Public Const kGetMandatoryRiskName As String = "Get Mandatory Risk"
    Public Const kGetMandatoryRiskSQL As String = "spu_SIR_Get_Mandatory_Risk"

    Public Const kCheckClaimOnRiskStored As Boolean = True
    Public Const kCheckClaimOnRiskName As String = "Check Claim On Risk"
    Public Const kCheckClaimOnRiskSQL As String = "spu_get_claims_cnt_on_risk"

    Public Const kUnquoteRisksForwardStored As Boolean = True
    Public Const kUnquoteRisksForwardName As String = "Unquote risks for all future versions"
    Public Const kUnquoteRisksForwardSQL As String = "spu_Unquote_Risks_Forward"

    Public Const kGetUserGroupId As Boolean = True
    Public Const kGetUserGroupIdName As String = "Get User group id by description"
    Public Const kGetUserGroupIdSQL As String = "spu_Get_User_Group_Id_For_WrkTask_Instance"

    Public Const kGetPolicyRisksForNoChangeStored As Boolean = True
    Public Const kGetPolicyRisksForNoChangeName As String = "Get Policy Risks For No Change"
    Public Const kGetPolicyRisksForNoChangeSQL As String = "spu_Get_Policy_Risks_For_No_Change"

    Public Const kGetUserAuthorityDisplayReinsuranceStored As Boolean = True
    Public Const kGetUserAuthorityDisplayReinsuranceName As String = "Get User Authority Display Reinsurance"
    Public Const kGetUserAuthorityDisplayReinsuranceSQL As String = "spe_User_Authorities_sel"



    Public Const ACGetPolicyLivePlansStored As Boolean = True
    Public Const ACGetPolicyLivePlansName As String = "Get Live plan on Policy"
    Public Const ACGetPolicyLivePlansSQL As String = "spu_get_policy_live_plans"

    Public Const ACGetTotalPremiumForAllVersionsStored As Boolean = True
    Public Const ACGetTotalPremiumForAllVersionsName As String = "Get Total Premium Amount For All Policy Versions"
    Public Const ACGetTotalPremiumForAllVersionsSQL As String = "spu_SAM_Get_Total_Premium_Amount_For_All_Policy_Versions"

    Public Const kGetPolicyRisksForAutoQuoteStored As Boolean = True
    Public Const kGetPolicyRisksForAutoQuoteName As String = "Get Policy Risks For Auto Quote"
    Public Const kGetPolicyRisksForAutoQuoteSQL As String = "spu_Get_Policy_Risks_For_AutoQuote"

    Public Const kDeletePFPremiumFinanceStored As Boolean = True
    Public Const kDeletePFPremiumFinanceName As String = "Delete PFPremiumFinance"
    Public Const kDeletePFPremiumFinanceSQL As String = "spu_Delete_PFPremiumFinance"

    'Start - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enhancement)
    Public Const ACGetPolicyRisksForAutoQuoteName = "Get Policy Risks For Auto Quote"
    Public Const ACGetPolicyRisksForAutoQuoteSQL = "spu_Get_Policy_Risks_For_AutoQuote"
    'End - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enhancement)
End Module