Option Strict Off
Option Explicit On

Module SQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '

    Public Const ACGetObjectsStored As Boolean = True
    Public Const ACGetObjectsName As String = "GetObjects"
    Public Const ACGetObjectsSQL As String = "spu_gis_model_objects_sel"

    Public Const ACGetPropertiesStored As Boolean = True
    Public Const ACGetPropertiesName As String = "GetProperties"
    Public Const ACGetPropertiesSQL As String = "spu_gis_model_properties_sel"

    Public Const ACGetPolicyLinkStored As Boolean = True
    Public Const ACGetPolicyLinkName As String = "GetPolicyLink"
    Public Const ACGetPolicyLinkSQL As String = "spu_gis_policy_link_sel"

    Public Const ACGetPolicyLinkSchIdStored As Boolean = True
    Public Const ACGetPolicyLinkSchIdName As String = "GetPolicyLinkSchId"
    Public Const ACGetPolicyLinkSchIdSQL As String = "spu_gis_policy_link_sel_schid"

    Public Const ACAddPolicyLinkStored As Boolean = True
    Public Const ACAddPolicyLinkName As String = "AddPolicyLink"
    Public Const ACAddPolicyLinkSQL As String = "spu_gis_policy_link_add"

    Public Const ACUpdateSchIdStored As Boolean = True
    Public Const ACUpdateSchIdName As String = "UpdateSchId"
    Public Const ACUpdateSchIdSQL As String = "spu_gis_policy_link_upd_schid" ' CL010200

    Public Const ACUpdateSchIdViaInsFileCntStored As Boolean = True
    Public Const ACUpdateSchIdViaInsFileCntName As String = "UpdateSchIdViaInsFileCnt"
    Public Const ACUpdateSchIdViaInsFileCntSQL As String = "spu_gis_policy_link_upd_schemeid"

    Public Const ACUpdateQteRefStored As Boolean = True
    Public Const ACUpdateQteRefName As String = "UpdateQuoteRef"
    Public Const ACUpdateQteRefSQL As String = "spu_gis_policy_link_qte_ref_upd"

    Public Const ACGetCurrentSchemeIdStored As Boolean = True
    Public Const ACGetCurrentSchemeIdName As String = "GetCurrentSchemeId"
    Public Const ACGetCurrentSchemeIdSQL As String = "spu_gis_current_scheme_id_sel"

    Public Const ACCopyQuoteStored As Boolean = True
    Public Const ACCopyQuoteName As String = "CopyQuote"
    Public Const ACCopyQuoteSQL As String = "spu_SirRen_Copy_Quote_Ins"

    Public Const ACSchemeSelUpdStored As Boolean = True
    Public Const ACSchemeSelUpdName As String = "SchemeSelUpdate"
    Public Const ACSchemeSelUpdSQL As String = "spu_GIS_PolicySchemesSel_add"

    Public Const ACCopyAgentsStored As Boolean = True
    Public Const ACCopyAgentsName As String = "CopyAgents"
    Public Const ACCopyAgentsSQL As String = "spu_SIRRen_copy_agents"

    Public Const ACRollbackEDISQL As String = "spu_EDI_ClearUp"
    Public Const ACRollbackEDIName As String = "EDIClearUp"
    Public Const ACRollbackEDIStored As Boolean = True

    Public Const ACUpdateRiskWithGisScreenIdSQL As String = "spu_STS_risk_gis_screen_id_upd"
    Public Const ACUpdateRiskWithGisScreenIdName As String = "UpdateRiskWithGisScreenId"
    Public Const ACUpdateRiskWithGisScreenIdStored As Boolean = True

    Public Const ACSQLEDIGetUserDetailsSQL As String = "spu_edi_get_pmuser_details"
    Public Const ACSQLEDIGetUserDetailsStored As Boolean = True

    Public Const kGetDataModelDetailsName As String = "returns details for the specified gis data model code"
    Public Const kGetDataModelDetailsSQL As String = "spu_GIS_Get_Data_Model_Details_For_Code"

    Public Const ACCheckIfNexusRiskSQL As String = "spu_SAM_CheckNexusPolicy"
    Public Const ACCheckIfNexusRiskName As String = "CheckIfNexusPolicy"
    Public Const ACCheckIfNexusRiskStored As Boolean = True
    Public Const ACDeleteExistingReinsuranceStored As Boolean = True
    Public Const ACDeleteExistingReinsuranceName As String = "DeleteRiskReinsurance"
    Public Const ACDeleteExistingReinsuranceSQL As String = "spu_SAM_Delete_Risk_Reinsurance"

    Public Const ACGetProrataFlagStored As Boolean = True
    Public Const ACGetProrataFlagName As String = "Get ProRata Flag"
    Public Const ACGetProrataFlagSQL As String = "spu_get_prorata_flag"

    Public Const ACSelectIFRLStored As Boolean = True
    Public Const ACSelectIFRLName As String = "SelectInsuranceFileRiskLink"
    Public Const ACSelectIFRLSQL As String = "spe_insurance_file_risk_li_sel"

    Public Const ACSelectGISDataModelStored As Boolean = True
    Public Const ACSelectGISDataModelName As String = "SelectGISDataModel"
    Public Const ACSelectGISDataModelSQL As String = "spu_get_gis_data_model_from_risk"

    Public Const ACSelectInsuranceFileCntStored As Boolean = False
    Public Const ACSelectInsuranceFileCntName As String = "SelectInsuranceFileCnt"
    Public Const ACSelectInsuranceFileCntSQL As String = "SELECT ifi.insurance_file_cnt, ifi.cover_start_date, ifi.expiry_date " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                         "FROM insurance_file_risk_link ifrl " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                         "JOIN insurance_file ifi ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                         "WHERE ifrl.risk_cnt = {risk_cnt}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                         "AND ISNULL(ifi.insurance_file_status_id, 3) in (2,3,4,5,6) " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                         "AND ifi.insurance_file_type_id IN (2,5,9,4)" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                         "ORDER BY 1 DESC"

    Public Const ACSelectInsuranceFileCntMTRSQL As String = "SELECT ifi.insurance_file_cnt, ifi.cover_start_date, ifi.expiry_date" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "FROM insurance_file_risk_link ifrl" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "JOIN insurance_file ifi ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "WHERE ifrl.risk_cnt = {risk_cnt}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "AND ifi.insurance_file_status_id = 1" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "AND ifi.insurance_file_type_id = 8" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "ORDER BY 1 DESC"

    Public Const ACSelectInsuranceFileStored As Boolean = True
    Public Const ACSelectInsuranceFileName As String = "SelectInsuranceFile"
    Public Const ACSelectInsuranceFileSQL As String = "spe_Insurance_File_sel"

    Public Const ACGetTMPStatusName As String = "Determine if its TMP"
    Public Const ACGetTMPStatusSQL As String = "Select is_true_monthly_policy from product where product_id= {prod_id}"

    Public Const ACGetRoundingInfoStored As Boolean = False
    Public Const ACGetRoundingInfoName As String = "GetRoundingInfo"

    'Modifying the inline query to make it compatible with SQL server 2005

    Public Const ACGetRoundingInfoSQL As String = "SELECT p.round_prem_to_nearest_unit," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "p.rounding_section_id," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "rst.code" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "FROM Product p LEFT OUTER JOIN rating_section_type rst" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "ON p.rounding_section_id = rst.rating_section_type_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "WHERE p.product_id = {product_id}" & Strings.ChrW(13) & Strings.ChrW(10)

    Public Const ACSelectUWProductOptionsStored As Boolean = False
    Public Const ACSelectUWProductOptionsName As String = "SelectUWProductOption"
    Public Const ACSelectUWProductOptionsSQL As String = "SELECT is_midnight_renewal, allow_positive_cancellation, ISNULL(unified_renewal_day, 0) " &
                                                         "FROM Product WHERE product_id = {product_id}"

    'Update the risk with the values from the perils
    Public Const ACUpdateRiskStored As Boolean = True
    Public Const ACUpdateRiskName As String = "UpdateRisk"
    Public Const ACUpdateRiskSQL As String = "spu_update_risk_values"

    Public Const ACUpdateInsuranceFileRiskLinkDetailsStatusStored As Boolean = True
    Public Const ACUpdateInsuranceFileRiskLinkDetailsStatusName As String = "UpdateInsuranceFileRiskLinkDetailsStatus"
    Public Const ACUpdateInsuranceFileRiskLinkDetailsStatusSQL As String = "spu_delete_insurance_file_risk_link"

    Public Const ACAgentBranchAddSQL As String = "spu_Party_agent_Branch_add"
    Public Const ACAgentBranchAddName As String = "Agent Branch Add"
    Public Const ACAgentBranchAddStored As Boolean = True
    ' Select Insurance File Risk Link Details SQL
    Public Const ACSelectInsuranceFileRiskLinkDetailsStored As Boolean = True
    Public Const ACSelectInsuranceFileRiskLinkDetailsName As String = "SelectInsuranceFileRiskLinkDetails"
    Public Const ACSelectInsuranceFileRiskLinkDetailsSQL As String = "spe_Insurance_File_Risk_Li_sel"

    Public Const ACGetRuleFileSQL As String = "spu_get_rule_file_name"
    Public Const ACGetRuleFileName As String = "Get Rule File Name"
    Public Const ACGetRuleFileStored As Boolean = True

    Public Const ACCopyRiskStandardWordingsSQL As String = "spu_Copy_RISK_Standard_Wording"
    Public Const ACCopyRiskStandardWordingsName As String = "CopyRiskStandardWordings"
    Public Const ACCopyRiskStandardWordingsStored As Boolean = True

    Public Const kGisGetLookupDataSQL As String = "spu_gis_get_lookup_data"
    Public Const kGisGetLookupDataName As String = "GisGetLookupData"
    Public Const kGisGetLookupDataStored As Boolean = True

    Public Const kGetClaimVersionDetailsSQL As String = "spu_Claim_sel"
    Public Const kGetClaimVersionDetailsName As String = "GetClaimVersionDetails"
    Public Const kGetClaimVersionDetailsStored As Boolean = True

    Public Const kGetClaimVersionReserveDetailsSQL As String = "spu_CLM_Get_Claim_Version_Details"
    Public Const kGetClaimVersionReserveDetailsName As String = "GetClaimVersionReserveDetails"
    Public Const kGetClaimVersionReserveDetailsStored As Boolean = True


    Public Const kGetPolicyVersionDetailsSQL As String = "spu_SAM_insurance_file_sel"
    Public Const kGetPolicyVersionDetailsName As String = "GetPolicyVersionDetails"
    Public Const kGetPolicyVersionDetailsStored As Boolean = True


    Public Const kGetContactUserDetailsSQL As String = "spu_SAM_Get_User_Details"
    Public Const kGetContactUserDetailsName As String = "GetContactUserDetails"
    Public Const kGetContactUserDetailsStored As Boolean = True

    Public Const kGetPartyDetailsSQL As String = "spu_get_party_dataset"
    Public Const kGetPartyDetailsName As String = "GetPartyDetails"
    Public Const kGetPartyDetailsStored As Boolean = True


    Public Const ACGetEffectiveDateSQL As String = "spu_get_EffectiveDate_PRE"
    Public Const ACGetEffectiveDate As String = "Get EffectiveDate for PRE"
    Public Const ACEffectiveDateStored As Boolean = True
End Module