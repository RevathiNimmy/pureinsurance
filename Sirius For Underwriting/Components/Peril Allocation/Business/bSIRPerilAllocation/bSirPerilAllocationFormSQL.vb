Option Strict Off
Option Explicit On
Module BusinessSQL
    ' ***************************************************************** '
    ' Class Name: FormSQL
    '
    ' Date: 27/10/1997
    '
    ' Description: Contains the SQL Statements required by the
    '              bSirPerilAllocation.Form class.
    '
    ' Edit History: TF27101997 - Created
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    ' Add Section and Perils SQL
    Public Const ACAddSectionAndPerilsStored As Boolean = True
    Public Const ACAddSectionAndPerilsName As String = "AddSectionAndPerils"
    Public Const ACAddSectionAndPerilsSQL As String = "spu_sir_peril_allocation"

    'Calculate Premium for the Rating section type
    Public Const ACCalculatePremiumStored As Boolean = True
    Public Const ACCalculatePremiumName As String = "CalculatePremium"
    Public Const ACCalculatePremiumSQL As String = "spu_sir_Calc_Premium"

    'Select the Rating sections for the given Insurance file and the risk
    Public Const ACSelectRatingSectionStored As Boolean = True
    Public Const ACSelectRatingSectionName As String = "SelectRatingSection"
    Public Const ACSelectRatingSectionSQL As String = "spu_sir_rating_section_sel"

    'Delete all the Rating section for the given Insurance file and the risk
    Public Const ACDeleteRatingSectionStored As Boolean = True
    Public Const ACDeleteRatingSectionName As String = "DeleteRatingSection"
    Public Const ACDeleteRatingSectionSQL As String = "spu_sir_rating_section_del"

    'Delete all the perils for the given Insurance file and the risk
    Public Const ACDeletePerilsStored As Boolean = True
    Public Const ACDeletePerilsName As String = "DeleteRatingSection"
    Public Const ACDeletePerilsSQL As String = "spu_sir_peril_del"

    'Select All the Rating Section types
    Public Const ACSelectRatingSectionTypeStored As Boolean = True
    Public Const ACSelectRatingSectionTypeName As String = "SelectRatingSectionType"
    Public Const ACSelectRatingSectionTypeSQL As String = "spu_get_all_rating_section_types"

    'Select All the Rate Types
    Public Const ACSelectRateTypesStored As Boolean = False
    Public Const ACSelectRateTypesName As String = "SelectRateTypes"
    Public Const ACSelectRateTypesSQL As String = "Select Rate_Type_id, Code from Rate_Type Where Is_Deleted  = 0 "

    'Select the data model code for the given Insurance file and the risk
    Public Const ACSelectGISDataModelStored As Boolean = True
    Public Const ACSelectGISDataModelName As String = "SelectGISDataModel"
    Public Const ACSelectGISDataModelSQL As String = "spu_get_gis_data_model_from_risk"

    'Select the id for the given code
    Public Const ACSelectIdFromCodeStored As Boolean = True
    Public Const ACSelectIdFromCodeName As String = "SelectIdFromCode"
    Public Const ACSelectIdFromCodeSQL As String = "spu_pm_get_eff_id_from_code"

    'Update the risk status id for the risk
    Public Const ACUpdateRiskStatusStored As Boolean = False
    Public Const ACUpdateRiskStatusName As String = "UpdateRiskStatus"
    Public Const ACUpdateRiskStatusSQL As String = "UPDATE risk SET risk_status_id = {risk_status_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "WHERE risk_cnt = {risk_cnt}"

    'Update the risk with the values from the perils
    Public Const ACUpdateRiskStored As Boolean = True
    Public Const ACUpdateRiskName As String = "UpdateRisk"
    Public Const ACUpdateRiskSQL As String = "spu_update_risk_values"

    'Select the original risk cnt from the Insurance file and risk cnts
    Public Const ACSelectIFRLStored As Boolean = True
    Public Const ACSelectIFRLName As String = "SelectInsuranceFileRiskLink"
    Public Const ACSelectIFRLSQL As String = "spe_insurance_file_risk_li_sel"

    'Select the insurance file cnt for the risk record
    Public Const ACSelectInsuranceFileCntStored As Boolean = True
    Public Const ACSelectInsuranceFileCntName As String = "SelectInsuranceFileCnt" 'WPR 75 added 
    Public Const ACSelectInsuranceFileCntSQL As String = "spu_select_Insurance_FileCnt"

    Public Const kSelectInsuranceFileCntForPTStored As Boolean = False
    Public Const kSelectInsuranceFileCntForPTName As String = "SelectInsuranceFileCnt"

    Public Const kSelectInsuranceFileCntForPTSQL As String = "SELECT ifi.insurance_file_cnt, ifi.cover_start_date, ifi.expiry_date " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                         "FROM insurance_file_risk_link ifrl " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                         "JOIN insurance_file ifi ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                         "WHERE ifrl.risk_cnt = {risk_cnt}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "AND ISNULL(ifi.insurance_file_status_id, 3) in (1,2,3,4,5,6) " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "AND ifi.insurance_file_type_id IN (2,3,5,9,4,8,6)" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                         "ORDER BY 1 DESC"
    'End Girija - PN 55432

    Public Const ACSelectInsuranceFileCntMTRStored As Boolean = False
    Public Const ACSelectInsuranceFileCntMTRSQL As String = "SELECT ifi.insurance_file_cnt, ifi.cover_start_date, ifi.expiry_date" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "FROM insurance_file_risk_link ifrl" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "JOIN insurance_file ifi ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "WHERE ifrl.risk_cnt = {risk_cnt}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "AND ifi.insurance_file_type_id in(8,12,11)" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "ORDER BY 1 DESC"

    'Select the insurance file record
    Public Const ACSelectInsuranceFileStored As Boolean = True
    Public Const ACSelectInsuranceFileName As String = "SelectInsuranceFile"
    Public Const ACSelectInsuranceFileSQL As String = "spe_Insurance_File_sel"

    ' Gets product options for an underwriting product
    '   This should be a stored proc call, but hell, it should really be a link to
    '   the Product business object but we don't do object oriented 'round here!
    Public Const ACSelectUWProductOptionsStored As Boolean = False
    Public Const ACSelectUWProductOptionsName As String = "SelectUWProductOption"
    Public Const ACSelectUWProductOptionsSQL As String = "SELECT is_midnight_renewal, allow_positive_cancellation, ISNULL(unified_renewal_day, 0) " &
                                                         "FROM Product WHERE product_id = {product_id}"

    Public Const ACGetProrataFlagStored As Boolean = True
    Public Const ACGetProrataFlagName As String = "Get ProRata Flag"
    Public Const ACGetProrataFlagSQL As String = "spu_get_prorata_flag"

    Public Const ACApplyCoinsuranceStored As Boolean = True
    Public Const ACApplyCoinsuranceName As String = "Apply Coinsurance To Peril"
    Public Const ACApplyCoinsuranceSQL As String = "spu_apply_coinsurance_to_peril"

    Public Const ACGetRoundingInfoStored As Boolean = False
    Public Const ACGetRoundingInfoName As String = "GetRoundingInfo"

    'Modifying the inline query to make it compatible with SQL server 2005

    Public Const ACGetRoundingInfoSQL As String = "SELECT p.round_prem_to_nearest_unit," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "p.rounding_section_id," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "rst.code" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "FROM Product p LEFT OUTER JOIN rating_section_type rst" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "ON p.rounding_section_id = rst.rating_section_type_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "WHERE p.product_id = {product_id}"

    ' Select Transaction Type SQL
    Public Const ACGetTransactionTypeStored As Boolean = False
    Public Const ACGetTransactionTypeName As String = "GetTransactionType"
    Public Const ACGetTransactionTypeSQL As String = "select transaction_type_id from transaction_type where code = {code} and is_deleted = 0"


    Public Const ACApplyCoinsuranceToRiskStored As Boolean = True
    Public Const ACApplyCoinsuranceToRiskName As String = "Apply Coinsurance To Peril via Risk"
    Public Const ACApplyCoinsuranceToRiskSQL As String = "spu_apply_risk_coinsurance_to_peril"


    Public Const ACGetUnusedRatingSectionsStored As Boolean = True
    Public Const ACGetUnusedRatingSectionsName As String = "Get Unused Rating Sections"
    Public Const ACGetUnusedRatingSectionsSQL As String = "spu_get_unused_rating_sections"

    Public Const ACGetOriginalRatingSectionsStored As Boolean = True
    Public Const ACGetOriginalRatingSectionsName As String = "Get_Original_Rating_Sections"
    Public Const ACGetOriginalRatingSectionsSQL As String = "spu_Get_Original_Rating_Sections"

    Public Const ACGetRoundingSectionAmountsStored As Boolean = True
    Public Const ACGetRoundingSectionAmountsName As String = "GetRoundingSectionAmounts"
    Public Const ACGetRoundingSectionAmountsSQL As String = "spu_Get_Rounding_Section_Amounts"


    Public Const ACGetRatingSectionTypeTaxStored As Boolean = True
    Public Const ACGetRatingSectionTypeTaxName As String = "Get Rating Section Type Tax"
    'Developer Guide No. 39
    Public Const ACGetRatingSectionTypeTaxSQL As String = "spu_sir_get_rating_section_type_tax"

    Public Const kIsRiskACopyName As String = "Determine if the risk is a copy"
    Public Const kIsRiskACopySQL As String = "spu_sir_is_risk_a_copy"

    'TMP
    Public Const ACGetTMPStatusName As String = "Determine if its TMP"
    Public Const ACGetTMPStatusSQL As String = "Select is_true_monthly_policy from product where product_id= {prod_id}"


    'Select All the Rating Section types
    Public Const ACSelectRatingSectionTypeForRiskTypeStored As Boolean = True
    Public Const ACSelectRatingSectionTypeForRiskTypeName As String = "SelectRatingSectionType"
    Public Const ACSelectRatingSectionTypeForRiskTypeTypeSQL As String = "spu_SIR_Get_RatingSectionTypes"

    'Select Get Peril Allocation Security
    Public Const ACSelectPerilAllocationSecurityStored As Boolean = True
    Public Const ACSelectPerilAllocationSecurityName As String = "SelectPerilAllocationSecurity"
    Public Const ACSelectPerilAllocationSecuritySQL As String = "spu_SIR_GetPerilAllocationSecurity"

    'Check Mandatory Questions
    Public Const ACCheckMandatoryQueStored As Boolean = True
    Public Const ACCheckMandatoryQueName As String = "CheckMandatoryQuestions"
    Public Const ACCheckMandatoryQueSQL As String = "spu_SIR_Check_Mandatory_Que"

    Public Const kGetRisksBilledPremiumStored As Boolean = True
    Public Const kGetRisksBilledPremiumName As String = "GetRisksBilledPremium"
    Public Const kGetRisksBilledPremiumSQL As String = "spu_SIR_Get_Risks_Billed_Premium"

    Public Const kGetGISOutputStored As Boolean = True
    Public Const kGetGISOutputName As String = "GetGISOutput"
    Public Const kGetGISOutputSQL As String = "spu_SIR_Get_GIS_Output"

    Public Const ACCheckMTCRatingRulesStored As Boolean = True
    Public Const ACCheckMTCRatingRulesSQL As String = "spu_Check_MTC_Rating_Rules"
    Public Const ACCheckMTCRatingRulesName As String = "CheckMTCRatingRules"

    'Get The Risk inception Date
    Public Const ACGetRiskInceptionDateStored As Boolean = True
    Public Const ACGetRiskInceptionDateName As String = "GetRiskInceptionDate"
    Public Const ACGetRiskInceptionDateSQL As String = "spu_SIR_Get_RiskInceptionDate"

    'Wpr53
    Public Const ACCheckMandatoryRiskStored As Boolean = True
    Public Const ACCheckMandatoryRiskSQL As String = "Spu_SIR_Check_Mandatory_Risk"
    Public Const ACCheckMandatoryRiskName As String = "CheckMandatoryRisk"

    Public Const ACGETInsuranceFileTypeIDAndCodeName As String = "GetInsuranceFileTypeIdAndCode"
    Public Const ACGETInsuranceFileTypeIDAndCodeNameSQL As String = "spu_get_insurance_file_type_from_InsuranceFileCnt"
    Public Const ACGETInsuranceFileTypeIDAndCodeStored As Boolean = True

    Public Const kGetProRataForOriginalRiskStored As Boolean = True
    Public Const kGetProRataForOriginalRiskName As String = "GetProRataForOriginalRiskPremium"
    Public Const kGetProRataForOriginalRiskSQL As String = "spu_get_ProrataRate_for_UneditedRisk"

    Public Const kGetRiskBilledPremiumStored As Boolean = True
    Public Const kGetRiskBilledPremiumName As String = "GetRisksBilledPremium"
    Public Const kGetRiskBilledPremiumSQL As String = "spu_SIR_Get_Risks_Billed_Premium"

    Public Const kGetRiskRetunPremiumStored As Boolean = False
    Public Const kGetRiskReturnPremiumName As String = "GetRisksReturnPremium"
    Public Const kGetRiskReturnPremiumSQL As String = "SELECT SUM(ISNULL(this_premium,0)) FROM   Rating_Section WHERE  risk_cnt = {risk_cnt}"

    Public Const kUpdateRiskReturnPremiumStored As Boolean = True
    Public Const kUpdateRiskReturnPremiumName As String = "UpdateRiskReturnPremiumDuringCancellation"
    Public Const kUpdateRiskReturnPremiumSQL As String = "spu_Update_Risk_Return_Premium_During_Cancellation"

End Module