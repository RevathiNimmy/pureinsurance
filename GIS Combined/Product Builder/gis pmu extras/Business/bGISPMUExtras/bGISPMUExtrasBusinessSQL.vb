Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Imports SSP.Shared
Module BusinessSQL
    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    '
    ' Date: 05/05/1999
    '
    ' Description: Contains the SQL Statements required by the
    '              bGISUserDefHeader.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectRisk"
    ' Public Const ACSelectSQL = "SELECT * FROM Risk WHERE Risk_id = {Risk_id}"

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20040608 : Added the following constant. Ref. CQ5367 - START
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'developer guide no 39. 
    Public Const ACGetCodeAndIndicatorSQL As String = "spu_GIS_Get_CodeAndIndicator"
    Public Const ACGetCodeAndIndicatorName As String = "GetCodeAndIndicator"
    Public Const ACGetCodeAndIndicatorStored As Boolean = True
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20040608 : Added the following constant. Ref. CQ5367 - END
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    ' Select Address SQL
    Public Const ACGetAddressStored As Boolean = True
    Public Const ACGetAddressName As String = "SelectAddress"
    Public Const ACGetAddressSQL As String = "spe_Address_sel"

    ' get claim info
    Public Const ACGetClaimInfoStored As Boolean = True
    Public Const ACGetClaimInfoName As String = "Get Claim Info"
    Public Const ACGetClaimInfoSQL As String = "spu_get_claim_info"

    ' get insurance file count using gis_policy_link_id
    Public Const ACGetPolicyIDStored As Boolean = False
    Public Const ACGetPolicyIDName As String = "Get Insurance File Count"
    'sj 10/12/2002 - start
    'Public Const ACGetPolicyIDSQL = "SELECT ifrl.insurance_file_cnt" & vbCrLf & _
    ''                                "FROM gis_policy_link gpl," & vbCrLf & _
    ''                                "insurance_file_risk_link ifrl" & vbCrLf & _
    ''                                "WHERE gpl.gis_policy_link_id = {gis_policy_link_id}" & vbCrLf & _
    ''                                "AND gpl.risk_id = ifrl.risk_cnt"
    Public Const ACGetPolicyIDSQL As String = "SELECT ifrl.insurance_file_cnt, i.insured_cnt,i.insurance_folder_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "FROM gis_policy_link gpl," & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "insurance_file i," & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "insurance_file_risk_link ifrl" & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "WHERE gpl.gis_policy_link_id = {gis_policy_link_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "AND gpl.risk_id = ifrl.risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                              "AND i.insurance_file_cnt = ifrl.insurance_file_cnt"
    'sj 10/12/2002 - end

    ' get accumulation used elsewhere
    Public Const ACAccumulationUsedElsewhereStored As Boolean = True
    Public Const ACAccumulationUsedElsewhereName As String = "Get Accumulation Info"
    Public Const ACAccumulationUsedElsewhereSQL As String = "spu_accumulation_used_elsewhere"

    'Thinh Nguyen 20/03/2002 (start)
    Public Const ACGetPrevRateStored As Boolean = True
    Public Const ACGetPrevRateName As String = "Get Previous risk's rate"
    Public Const ACGetPrevRateSQL As String = "spu_Get_Prev_Rate"
    'Thinh Nguyen 20/03/2002 (end)

    ' Get Code From ID - TF200198
    Public Const ACGetCodeFromIDStored As Boolean = True
    Public Const ACGetCodeFromIDName As String = "GetCodeFromID"
    Public Const ACGetCodeFromIDSQL As String = "spu_pm_get_code_from_id"

    ' Ed 25072002 - Scheme Id Comparison changes
    Public Const ACIsSameSchemeSQL As String = "spu_PMB_Is_Same_Scheme"
    Public Const ACIsSameSchemeName As String = "IsSameScheme"
    Public Const ACIsSameSchemeStored As Boolean = True

    Public Const ACTaskAssignmentSubDetailsSQL As String = "spu_get_task_assignment_sub_details"
    Public Const ACTaskAssignmentSubDetailsName As String = "TaskAssignmentSubDetails"
    Public Const ACTaskAssignmentSubDetailsStored As Boolean = True

    '''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20021122 : NRMA Project Process No 204 - Start
    '''''''''''''''''''''''''''''''''''''''''''''''''''
    ' get claim info Detail
    Public Const ACGetClaimInfoDetailStored As Boolean = True
    Public Const ACGetClaimInfoDetailName As String = "Get Claim Info Detail"
    Public Const ACGetClaimInfoDetailSQL As String = "spu_get_claim_info_detail"

    ' Check Renewals SQL
    Public Const ACCheckRenewalsStored As Boolean = True
    Public Const ACCheckRenewalsName As String = "CheckRenewals"
    Public Const ACCheckRenewalsSQL As String = "spu_check_in_renewal"

    ' To fetch the Insurance File Cnt (used by Claims Builder)
    Public Const ACGetInsuranceFileCntForClaimDMStored As Boolean = True
    Public Const ACGetInsuranceFileCntForClaimDMName As String = "GetInsuranceFileCntForClaimDM"
    Public Const ACGetInsuranceFileCntForClaimDMSQL As String = "spu_get_insurance_file_cnt_for_claim_DM"

    '''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20021122 : NRMA Project Process No 204 - End
    '''''''''''''''''''''''''''''''''''''''''''''''''''

    'GetPartyAdditionalDetails
    Public Const ACGetRenewalStopCodeCaptionIdStored As Boolean = False
    Public Const ACGetRenewalStopCodeCaptionIdName As String = gSIRLibrary.SIRLookupRenewalStopCode & "CaptionId"
    Public Const ACGetRenewalStopCodeCaptionIdSQL As String = "select caption_id from " & gSIRLibrary.SIRLookupRenewalStopCode

    Public Const ACGetSubBranchDetailsSQL As String = "select caption_id,source_id from sub_branch"
    Public Const ACGetSubBranchDetailsName As String = "GetSubBranchDetails"
    Public Const ACGetSubBranchDetailsStored As Boolean = False

    Public Const ACGetBranchCaptionIdSQL As String = "select caption_id from source"
    Public Const ACGetBranchCaptionIdName As String = "GetSubBranchDetails"
    Public Const ACGetBranchCaptionIdStored As Boolean = False

    Public Const ACGetCaptionDescStored As Boolean = True
    Public Const ACGetCaptionDescName As String = gSIRLibrary.SIRLookupRenewalStopCode & "CaptionDesc"
    Public Const ACGetCaptionDescSQL As String = "spu_pm_caption_desc"

    '''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20030205 : NRMA Project Process No 426 - Start
    '''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Const ACGetNoofClaimsLodgedSQL As String = "spu_check_claims_lodged"
    Public Const ACGetNoofClaimsLodgedName As String = "GetNoofClaimsLodged"
    Public Const ACGetNoofClaimsLodgedStored As Boolean = True
    '''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20030205 : NRMA Project Process No 426 - End
    '''''''''''''''''''''''''''''''''''''''''''''''''''

    'sj 04/03/2003 - start
    'ISS2644
    Public Const ACCheckIfWorkManagerTaskRequiredSQL As String = "spu_SIR_Check_If_WMTask_Required"
    Public Const ACCheckIfWorkManagerTaskRequiredName As String = "CheckIfWorkManagerTaskRequired"
    Public Const ACCheckIfWorkManagerTaskRequiredStored As Boolean = True
    'sj 04/03/2003 - end

    'HG02062003 - Retrieve Renewal Referal/Declined reasons for all previous risks (underwriting only)
    Public Const ACGetRenewalResonsSQL As String = "spu_SIR_Return_Previous_Fail_Reasons"
    Public Const ACGetRenewalResonsName As String = "ReturnPreviousRenewalReasons"
    Public Const ACGetRenewalResonsStored As Boolean = True


    Public Const ACRiskTypeSQL As String = "spu_Get_gis_policy_link_RiskType"
    Public Const ACRiskTypeName As String = "RiskType"
    Public Const ACRiskTypeStored As Boolean = True

    ' Create a new claim peril record
    Public Const ACAddClaimPerilGetMoreDataSQL As String = "spu_CLM_Claim_Peril_Get_SumInsured_And_RIBand"
    Public Const ACAddClaimPerilGetMoreDataName As String = "Gets SumInsured and RI_Band for new claim peril creation"
    Public Const ACAddClaimPerilSQL As String = "spu_CLM_Claim_Peril_Add"
    Public Const ACAddClaimPerilName As String = "Creates a new claim peril record"

    ' Update a claim peril
    Public Const ACEditClaimPerilSQL As String = "spu_CLM_Claim_Peril_Upd"
    Public Const ACEditClaimPerilName As String = "Updates a claim peril record"

    ' Delete a claim peril
    Public Const ACDeleteClaimPerilCheckSQL As String = "spu_check_deletion_for_peril"
    Public Const ACDeleteClaimPerilCheckName As String = "Checks whether a claim peril record can be deleted"
    Public Const ACDeleteClaimPerilSQL As String = "spu_CLM_Claim_Peril_Del"
    Public Const ACDeleteClaimPerilName As String = "Deletes a claim peril record"

    ' Get claim peril (and associated) data
    Public Const ACGetClaimPerilDataSQL As String = "spu_CLM_Get_Claim_Peril"
    Public Const ACGetClaimPerilDataName As String = "Selects a claim peril record plus associated data"

    ' Get claim peril (and associated) data
    Public Const ACLinkPerilClaimSQL As String = "spu_CLM_Claim_Peril_Link"
    Public Const ACLinkPerilClaimName As String = "Links a claim peril record to peril party"

    Public Const kGetProductDetailsName As String = "Returns product details for the specified insurance file"
    Public Const kGetProductDetailsSQL As String = "spu_GIS_Get_Insurance_File_Product_Details"

    Public Const ACInsertStandardWordingStored As Boolean = False
    Public Const ACInsertStandardWordingName As String = "InsertStandardWording"
    Public Const ACInsertStandardWordingSQL As String = "INSERT INTO {data_model}_standard_wording " &
                                                        "({data_model}_Policy_binder_id, sequence_id, document_template_id, gis_property_id, gis_object_id) " &
                                                        "VALUES ({policy_binder_id}, {sequence_id}, {document_template_id}, {gis_property_id}, {gis_object_id})"

    Public Const ACGetSchemeDetails As String = "spu_GIS_Scheme_sel"
    Public Const ACGetSchemeDetailsName As String = "GetSchemeDetails"
    Public Const ACGetSchemeDetailsStored As Boolean = True

    Public Const ACGetReserveAgainstPerilSQL As String = "Spu_CLM_Get_Claim_Reserve_details"
    Public Const ACGetReserveAgainstPerilName As String = "GetReserveAgainstPeril"
    Public Const ACGetReserveAgainstPerilStored As Boolean = True

    Public Const ACGetClaimInfoExSQL As String = "spu_get_claim_info_ex"
    Public Const ACGetClaimInfoExName As String = "GetClaimInfo"
    Public Const ACGetClaimInfoExStored As Boolean = True

    Public Const ACGetUserCanChangeReservesSQL As String = "spu_SIR_Get_UserCanChange_Reserves"
    Public Const ACGetUserCanChangeReservesName As String = "GetUserCanChangeReserves"
    Public Const ACGetUserCanChangeReservesStored As Boolean = True

    Public Const ACGetClaimCntSQL As String = "Spu_GIS_GetClaimCnt"
    Public Const ACGetClaimCntName As String = "GetClaimCnt"
    Public Const ACGetClaimCntStored As Boolean = True

    Public Const ACUpdateReserveDetailsSQL As String = "spu_update_reserve_details"
    Public Const ACUpdateReserveDetailsName As String = "UpdateReserveDetails"
    Public Const ACUpdateReserveDetailsStored As Boolean = True

    Public Const ACGetClaimHeaderDetailsSQL As String = "Spu_GIS_GetClaimHeaderDetails"
    Public Const ACGetClaimHeaderDetailsName As String = "GetClaimHeaderDetails"
    Public Const ACGetClaimHeaderDetailsStored As Boolean = True

    Public Const ACGetCaseIncurredTotalsSQL As String = "spu_Get_Case_Claim_Link"
    Public Const ACGetCaseIncurredTotalsName As String = "GetCaseIncurredTotals"

    Public Const ACInsertEventLogSQL As String = "spe_event_log_add"
    Public Const ACInsertEventLogName As String = "InsertEventLog"

    Public Const ACGetEventTypeGroupSQL As String = "spu_GetEventTypeGroupCode"
    Public Const ACGetEventTypeGroupName As String = "GetEventTypeGroupCode"

    Public Const ACGetRiskForGisPolicyLinkSQL As String = "spu_get_riskforgispolicylink"
    Public Const ACGetRiskForGisPolicyLinksName As String = "GetRiskForClaim"

    Public Const ACGetPartyCodeSQL As String = "Spu_get_party_code"
    Public Const ACGetPartyCodeName As String = "GetPartyCode"
    Public Const ACGetPartyCodeStored As Boolean = True

    Public Const ACGetPartySQL As String = "spe_party_sel"
    Public Const ACGetPartyName As String = "GetParty"
    Public Const ACGetPartyStored As Boolean = True

    '1.12 Wr25
    Public Const ACSetRenewalProductCodeSQL As String = "Spu_Sir_insurance_file_UpdateRenewalProduct"
    Public Const ACSetRenewalProductCodeName As String = "SetRenewalProductCode"

    Public Const ACGetRenewalProductCodeSQL As String = "Spu_Sir_insurance_file_SelectRenewalProduct"
    Public Const ACGetRenewalProductCodeName As String = "GetRenewalProductCode"

    'WR5
    Public Const ACGetPaymentAgainstPerilSQL As String = "spu_CLM_Get_Claim_Payment_AgainstReserveLevel"
    Public Const ACGetPaymentAgainstPerilName As String = "GetPaymentAgainstPeril"
    Public Const ACGetPaymentAgainstPerilStored As Boolean = True

    'WR 22
    Public Const ACUpdateClaimPaymentDetailsSQL As String = "spu_update_ClaimPayment_details"
    Public Const ACUpdateClaimPaymentDetailsName As String = "UpdateClaimPaymentDetails"
    Public Const ACUpdateClaimPaymentDetailsStored As Boolean = True

    Public Const ACUpdateNumberOfFleetVehiclesSQL As String = "spu_SIR_Update_Number_Of_Fleet_Vehicles" ' 2819
    Public Const ACUpdateNumberOfFleetVehiclesName As String = "UpdateNumberOfFleetVehicles"
    Public Const ACUpdateNumberOfFleetVehiclesStored As Boolean = True


    'SGH WPR 8-11   Rahul Jaiswal   Start
    Public Const ACGetPolicySubAgentsSQL As String = "spu_Select_SubAgents_CommissionOutput"
    Public Const ACGetPolicySubAgentsName As String = "GetPolicySubAgents"
    Public Const ACGetPolicySubAgentsStored As Boolean = True
    'SGH WPR 8-11   Rahul Jaiswal   End

    Public Const ACGetUserAuthorityLevelSelSQL As String = "spe_PMUser_Authority_Level_sel"
    Public Const ACGetUserAuthorityLevelSelName As String = "GetUserAuthorityLevel"
    Public Const ACGetUserAuthorityLevelSelStored As Boolean = True

    Public Const ACGetLoggedInUserDetailsSQL As String = "spu_get_pmuser_group_info"
    Public Const ACGetLoggedInUserDetailsName As String = "GetLoggedInUserDetails"
    Public Const ACGetLoggedInUserDetailsStored As Boolean = True

    Public Const ACGetGISUDLDetailSQL As String = "spu_GIS_List_TypeUDL_Exists"
    Public Const ACGetGISUDLDetailName As String = "spu_GIS_List_TypeUDL_Exists"
    Public Const ACGetGISUDLDetailProc As Boolean = True

    Public Const kGetClaimRecoveriesName As String = "Returns required recoveries details for specified peril id"
    Public Const kGetClaimRecoveriesSQL As String = "spu_get_recoveries_by_peril"

    Public Const kSaveClaimRecoveryName As String = "Save Claim recovery"
    Public Const kSaveClaimRecoverySQL As String = "spu_CLM_save_recovery"

    Public Const kGetClaimPaymentDeclineDetailsName As String = "Returns claim payment decline details"
    Public Const kGetClaimPaymentDeclineDetailsSQL As String = "spu_Get_Claim_Payment_Decline_Details"

    Public Const kGetPreviousClaimKeyName As String = "Returns Previous valid Claim_ID"
    Public Const kGetPreviousClaimKeySQL As String = "spu_Get_Previous_ClaimKey"

    Public Const kGetPreviousInsuranceFileName As String = "Returns Previous valid Insurance_File_Cnt"
    Public Const kGetPreviousInsuranceFileKeySQL As String = "spu_Get_Previous_InsuranceFileKey"

    Public Const kGetPreviousRiskDataName As String = "Returns Previous Risk Data"
    Public Const kGetPreviousRiskDataSQL As String = "spu_Get_Previous_RiskData"

    Public Const kGetNewClaimPaymentItemName As String = "Returns an array with values required for new payment"
    Public Const kGetNewClaimPaymentItemSQL As String = "spu_Get_New_Claim_Payment_item"

    Public Const kGetClassOfBusinessName As String = "get class of business for peril"
    Public Const kGetClassOfBusinessSQL As String = "spu_CLM_Get_Claim_Peril_Class_Of_Business"

    Public Const kGetGetTableDataName As String = "Get Data From Table"
    Public Const kGetGetTableDataSQL As String = "spu_Get_Table_Data"

    Public Const kRetrieveCurrenciesForClaimBranchName As String = "Returns the currencies available on the branch associated with the specified claim"
    Public Const kRetrieveCurrenciesForClaimBranchSQL As String = "spu_Get_Claim_Branch_Currency"

    Public Const kGetClaimCurrencyName As String = "GetClaimCurrency"
    Public Const kGetClaimCurrencySQL As String = "spu_Get_Claim_Currency"

    Public Const kCalculateTaxAmountsName As String = "Returns Calculated Tax Amounts"
    Public Const kCalculateTaxAmountsSQL As String = "spu_CLM_Calculate_Tax_Amounts"

    Public Const kGetAdvancedTaxScriptName As String = "Returns Advanced Tax Script information of a tax group"
    Public Const kGetAdvancedTaxScriptSQL As String = "spu_SAM_Get_Advanced_Script_Option"

    Public Const kGetCurrencyIdFromCurrencyCodeName As String = "Get CurrencyId From Currency Code"
    Public Const kGetCurrencyIdFromCurrencyCodeSQL As String = "spu_SAM_Get_CurrencyId_From_CurrencyCode"

    Public Const kGetBasicClaimInformationName As String = "Get Basic Claim Information"
    Public Const kGetBasicClaimInformationSQL As String = "spu_CLM_Get_Basic_Claim_Information"

    Public Const kClearStatsDetailName As String = "Clear Previous Stats Details"
    Public Const kClearStatsDetailSQL As String = "spu_CLM_Clear_Stats_Detail"


    Public Const kClearTaxBandInfoName As String = "Clear Records from  TaxBandInfo"
    Public Const kClearTaxBandInfoSQL As String = "spu_CLM_Clear_TaxBandInfo"

    Public Const kAddTaxBandInfoName As String = "Add Record to TaxBandInfo"
    Public Const kAddTaxBandInfoSQL As String = "spu_CLM_Add_TaxBandInfo"

    Public Const kGetPreviousReserveName As String = "Get Previous Reserve"
    Public Const kGetPreviousReserveSQL As String = "spu_get_previous_reserve"

    Public Const ACGetPartyServiceLevelSQL As String = "spu_SIR_Get_Party_Service_Level"
    Public Const ACGetPartyServiceLevelName As String = "GetPartyServiceLevelID"
    Public Const ACGetPartyServiceLevelStored As Boolean = True

End Module
