Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module FindInsuranceSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Class Name: FindInsuranceSQL
    '
    ' Date: 23rd October 1996
    '
    ' Description: Contains the SQL Statements to (Stored Procedures
    '              and Enbedded SQL) manipulate an FindInsurance
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas
    ' Public Const ACSelectStored = False
    ' Public Const ACSelectName = "SelectEvent"
    ' Public Const ACSelectSQL = "SELECT * FROM Event WHERE event_id = {event_id}"
    'Start(Sriram P)PN 56443
    Public Const ACGetAllTransactionDates As String = "GetAllTransactionDates"
    Public Const GetAllTransactionDatesStored As Boolean = True
    Public Const ACGetAllTransactionDatesSQL As String = "spu_Get_All_Policy_Transaction_Dates"
    'End(Sriram P)PN 56443
    ' Select FindInsurance SQL
    Public Const ACInsLikeRefStored As Boolean = True
    Public Const ACInsLikeRefName As String = "FindInsuranceLikeRef"
    'developer guide no 39. 
    Public Const ACInsLikeRefSQL As String = "spu_findins_like_ref"

    ' Select FindInsurance SQL
    Public Const ACInsLikeIndexStored As Boolean = True
    Public Const ACInsLikeIndexName As String = "FindInsuranceLikeIndex"
    'developer guide no 39. 
    Public Const ACInsLikeIndexSQL As String = "spu_findins_like_index"

    ' Select FindInsurance SQL
    Public Const ACInsLikeRefHolderStored As Boolean = True
    Public Const ACInsLikeRefHolderName As String = "FindInsuranceLikeRefHolder"
    'developer guide no. 39
    Public Const ACInsLikeRefHolderSQL As String = "spu_findins_like_ref_and_holder"

    ' Find Insurance from built query.
    Public Const ACInsFileFromQueryStored As Boolean = False
    Public Const ACInsFileFromQueryName As String = "FindInsuranceQuery"
    'developer guide no 39. 
    Public Const ACInsFileFromQuerySQL As String = ""

    ' Select Insurance From Vehicle Reg. SQL
    Public Const ACInsLikeRegistrationStored As Boolean = False
    Public Const ACInsLikeRegistrationName As String = "FindInsuranceLikeRegistration"
    'developer guide no 39. 
    Public Const ACInsLikeRegistrationSQL As String = "spu_findins_like_vehicle"

    ' Select FindInsurance SQL
    Public Const ACInsRiskDetailsStored As Boolean = True
    Public Const ACInsRiskDetailsName As String = "FindInsuranceRiskDetails"
    'developer guide no 39. 
    Public Const ACInsRiskDetailsSQL As String = "spu_gii_findins_risk_details"

    ' Select GetVehicle SQL
    Public Const ACGetVehicleDetailsStored As Boolean = True
    Public Const ACGetVehicleDetailsName As String = "GetVehicleDetails"
    'developer guide no 39.
    Public Const ACGetVehicleDetailsSQL As String = "spu_gii_get_vehicle_details"

    ' Select FindQuote SQL
    Public Const ACFindQuoteStored As Boolean = False
    Public Const ACFindQuoteName As String = "FindQuote"

    ' Ram 05-01-2000
    ' Find Insurance from Search Index (Using GIS Stored Procedure)
    Public Const ACInsLikeIndexGISSearchStored As Boolean = True
    Public Const ACInsLikeIndexGISSearchName As String = "FindInsuranceLikeIndexGISSearch"
    'Start (Prakash C Varghese) - (Tech Spec - WRQBENCF04-Association of multiple agents)-(5.2.1)
    Public Const ACInsLikeIndexGISSearchSQL As String = "spu_gis_search_property_find_two"
    'End (Prakash C Varghese) - (Tech Spec - WRQBENCF04-Association of multiple agents)-(5.2.1)

    ' Check Renewals SQL
    Public Const ACCheckRenewalsStored As Boolean = True
    Public Const ACCheckRenewalsName As String = "CheckRenewals"
    'developer guide no 39. 
    Public Const ACCheckRenewalsSQL As String = "spu_check_in_renewal"

    ' Update Renewal Statuss SQL
    Public Const ACUpdateRenewalStatusStored As Boolean = True
    Public Const ACUpdateRenewalStatusName As String = "UpdateRenewalStatus"
    'developer guide no 39. 
    Public Const ACUpdateRenewalStatusSQL As String = "spu_update_renewal_status"

    Public Const ACGetAllPolicyVersionStored As Boolean = True
    Public Const ACGetAllPolicyVersionName As String = "GetAllVersionOfPolicy"
    Public Const ACGetAllPolicyVersionSQL As String = "spu_Get_All_Policy_Version"

    ' Copy the coinsurance details
    Public Const ACCopyCoinsuranceStored As Boolean = True
    Public Const ACCopyCoinsuranceName As String = "ACCopyCoinsurance"
    'developer guide no 39. 
    Public Const ACCopyCoinsuranceSQL As String = "spu_copy_coinsurance"

    ' Copy the sub agent details
    Public Const ACCopySubAgentStored As Boolean = True
    Public Const ACCopySubAgentName As String = "ACCopySubAgent"
    'developer guide no 39. 
    Public Const ACCopySubAgentSQL As String = "spu_copy_sub_agent"

    ' Copy the policy standard wordings details
    Public Const ACCopyPolicyStandardWordingsStored As Boolean = True
    Public Const ACCopyPolicyStandardWordingsName As String = "ACCopyPolicyStandardWordings"
    'developer guide no 39. 
    Public Const ACCopyPolicyStandardWordingsSQL As String = "spu_copy_policy_standard_wordings"

    ' CTAF 080801 SearchAll queries
    'sj 03/07/2002
    'developer guide no 39.
    Public Const ACPolListAgencyPartySQL As String = "spu_sforb_policy_list_agency_party"

    'developer guide no 39. 
    Public Const ACPolListAgencySQL As String = "spu_sforb_policy_list_agency"

    'DJM 04/03/2004 : Add two extra parameters.
    'developer guide no 39.
    Public Const ACPolListUWPartySQL As String = "spu_sforb_policy_list_underwriting_party"

    Public Const ACPolListGetOtherPoliciesStored As Boolean = True
    Public Const ACPolListGetOtherPoliciesName As String = "OtherPolicyList"
    'developer guide no 39.
    Public Const ACPolListGetOtherPolicies As String = "spu_SIR_Policy_List_GetOtherPolicies"

    'DJM 04/03/2004 : Add two extra parameters.
    'developer guide no 39.
    Public Const ACPolListUWSQL As String = "spu_sforb_policy_list_underwriting"
    Public Const ACPolListStored As Boolean = True
    Public Const ACPolListName As String = "PolicyList"

    'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup START
    'KNCMGRISK Start
    Public Const ACSelPolicyBinderStored As Boolean = False
    Public Const ACSelPolicyBinderName As String = "GetPolicyBinder"
    Public Const ACSelPolicyBinderSQL As String = "SELECT rsa_policy_binder_id FROM RSA_Policy_Binder" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "WHERE gis_policy_link_id = {gis_policy_link_id}"

    Public Const ACSelGisPolicyLinkStored As Boolean = True
    Public Const ACSelGisPolicyLinkName As String = "GetGisPolicyLink"
    'developer guide no 39.
    Public Const ACSelGisPolicyLinkSQL As String = "sp_gis_policy_link_sel"

    ' Insert Insurance File Risk Link Details SQL
    Public Const ACInsertInsuranceFileRiskLinkDetailsStored As Boolean = True
    Public Const ACInsertInsuranceFileRiskLinkDetailsName As String = "InsertInsuranceFileRiskLinkDetails"
    'developer guide no 39.
    Public Const ACInsertInsuranceFileRiskLinkDetailsSQL As String = "spe_Insurance_File_Risk_Li_add"

    ' Select Risk Details SQL
    Public Const ACGetRiskDetailsStored As Boolean = True
    Public Const ACGetRiskDetailsName As String = "SelectRiskDetails"
    'developer guide no 39.
    Public Const ACGetRiskDetailsSQL As String = "spe_Risk_sel"


    ' Select Risk Group Details SQL
    Public Const ACGetRiskFolderDetailsStored As Boolean = True
    Public Const ACGetRiskFolderDetailsName As String = "SelectRiskFolderDetails"
    'developer guide no 39.
    Public Const ACGetRiskFolderDetailsSQL As String = "spe_Risk_Folder_sel"

    ' Insert Risk Details SQL
    Public Const ACInsertRiskDetailsStored As Boolean = True
    Public Const ACInsertRiskDetailsName As String = "InsertRiskDetails"
    'developer guide no 39.
    Public Const ACInsertRiskDetailsSQL As String = "spe_Risk_add"

    Public Const ACSaaRiskStored As Boolean = True
    Public Const ACSaaRiskName As String = "GetAllRisks"
    'developer guide no 39.
    Public Const ACSaaRiskSQL As String = "spe_Risk_saa"

    ' Insert Risk Folder Details SQL
    Public Const ACInsertRiskFolderDetailsStored As Boolean = True
    Public Const ACInsertRiskFolderDetailsName As String = "InsertRiskFolderDetails"
    'developer guide no 39.
    Public Const ACInsertRiskFolderDetailsSQL As String = "spe_Risk_Folder_add"
    'KNCMGRISK End
    'MKW060603 PN2400 1.6.9 to 1.8.6 Catchup END

    'MKW250703 PN4271 1.8.5 to 1.8.6 Catchup START
    'ISS1497 JAS 11/03/03 stored procedure for finding non temporary policy versions.
    Public Const ACGetAllNonTempPolicyVersionStored As Boolean = True
    Public Const ACGetAllNonTempPolicyVersionName As String = "GetAllNonTempVersionOfPolicy"
    Public Const ACGetAllNonTempPolicyVersionSQL As String = "spu_Get_All_Non_Temp_Policy_Version"
    'MKW250703 PN4271 1.8.5 to 1.8.6 Catchup END

    ' Select Risk Group Details SQL
    Public Const ACGetOption92Stored As Boolean = True
    Public Const ACGetOption92Name As String = "SelectRiskFolderDetails"
    'developer guide no 39.
    Public Const ACGetOption92SQL As String = "spu_Get_Option92_sel"

    ' Get Pary Type SQL
    Public Const ACGetPartyTypeStored As Boolean = True
    Public Const ACGetPartyTypeName As String = "GetPartyType"
    'developer guide no 39.
    Public Const ACGetPartyTypeSQL As String = "spu_Get_Party_Type_For_Party"


    ' RAW 29/04/2004 : CQ5139 : added
    ' Select InsuranceFile SQL
    Public Const ACSelectInsuranceFileStored As Boolean = True
    Public Const ACSelectInsuranceFileName As String = "SelectSingleInsuranceFile"
    'developer guide no 39.
    Public Const ACSelectInsuranceFileSQL As String = "spe_Insurance_File_sel"

    'check claim
    Public Const ACCheckClaimName As String = "Check Claim"
    Public Const ACCheckClaimSQL As String = "Select * from claim where policy_number = {Ins_ref} and loss_from_date > {Loss_date}"

    'GetBasePolicyCntForBackDateMTA
    Public Const ACGetBasePolicyCntForBackDateMTAName As String = "GetBasePolicyCntForBackDateMTA"
    'developer guide no 39.
    Public Const ACGetBasePolicyCntForBackDateMTASQL As String = "spu_GetBasePolicyCntForBackDateMTA"

    'Get CoverFromDate
    Public Const ACGetCoverFromDateName As String = "GetCoverFromDateName"
    Public Const ACGetCoverFromDateSQL As String = "Select min(cover_start_date) from insurance_file  " & Strings.ChrW(13) & Strings.ChrW(10) &
                "where insurance_folder_cnt={InsFolderCnt} " & Strings.ChrW(13) & Strings.ChrW(10) &
                "And expiry_date > {MTAEffectiveDt} " & Strings.ChrW(13) & Strings.ChrW(10) &
                "and insurance_file_type_id in (5,2)"
    'WPR 33-75 ADDED  
    'Get CoverEndDate
    Public Const ACGetCoverEndDateName As String = "GetCoverEndDateName"
    Public Const ACGetCoverEndDateSQL As String = "Select expiry_date from insurance_file  " & Strings.ChrW(13) & Strings.ChrW(10) &
                    "where insurance_file_cnt={InsFileCnt} "
    'WPR 33-75 END
    'Get Insurance_file_type
    Public Const ACGetInsFileTypeName As String = "Get Insurance_file Type"
    Public Const ACGetInsFileTypeSQL As String = "Select ift.code from Insurance_file Ifile " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "Inner Join Insurance_file_type IFT ON Ifile.Insurance_file_type_id=IFT.Insurance_file_type_id " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "And Ifile.insurance_file_cnt={InsFileCnt}"

    Public Const ACRebuildArrayFromLinkedPolicyVersionsStored As Boolean = True
    Public Const ACRebuildArrayFromLinkedPolicyVersionsName As String = "GetAllVersionOfPolicy"
    'developer guide no 39. 
    Public Const ACRebuildArrayFromLinkedPolicyVersionsSQL As String = "spu_SIR_MTA_Link_Sel"

    Public Const ACGetCancellationDateName As String = "GetCancellationDate"
    Public Const ACGetCancellationDateSQL As String = "Spu_GetPolicyCancellationDate"

    Public Const ACCopyRiskStandardWordingsStored As Boolean = True
    Public Const ACCopyRiskStandardWordingsName As String = "CopyRiskStandardWordings"
    'developer guide no 39. 
    Public Const ACCopyRiskStandardWordingsSQL As String = "spu_Copy_RISK_Standard_Wording"

    Public Const ACGetdataModelCodesName As String = "GetDataModelCodes"
    Public Const ACGetdataModelCodesSQL As String = "Spu_GetDataModelCodes"

    Public Const ACGetInsuranceFileStatusStored As Boolean = True
    Public Const ACGetInsuranceFileStatusName As String = "GetInsuranceFileStatus"
    Public Const ACGetInsuranceFileStatusSQL As String = "spu_SIR_Get_InsuranceFileStatus"

    'WPR12- Enhancement Quote Collection Process
    Public Const ACIsMarkedForCollectionName As String = "IsMarkedForCollection"
    Public Const ACIsMarkedForCollectionSQL As String = "spu_SIR_IsMarkedForCollection"

    Public Const ACUpdateMarkedForCollectionStatusName As String = "UpdateMarkedForCollection"
    Public Const ACUpdateMarkedForCollectionStatusSQL As String = "spu_SIR_Update_Marked_For_Collection"
    Public Const ACUpdateMarkedForCollectionStatusStored As Boolean = True

    Public Const ACGetQuotesMarkedForCollectionName As String = "GetQuotesMarkedForCollection"
    Public Const ACGetQuotesMarkedForCollectionSQL As String = "spu_Get_Quotes_Marked_ForCollection"

    Public Const ACCheckIsMarketplacePolicyName As String = "CheckIsMarketplacePolicy"
    Public Const ACCheckIsMarketplacePolicySQL As String = "spu_CheckIsMarketplacePolicy"
    Public Const ACCheckIsMarketplacePolicyStored As Boolean = True

    Public Const kCopyRiskLinksStored As Boolean = True
    Public Const kCopyRiskLinksName As String = "ACCopyRiskLinks"
    Public Const kCopyRiskLinksSQL As String = "spu_copy_insurance_file_risk_link"


    Public Const ACUpdateMarketplacePolicyStatusName As String = "UpdateMarketplacePolicyStatus"
    Public Const ACUpdateMarketplacePolicyStatusSQL As String = "spu_Update_MarketplacePolicy_Status"
    Public Const ACUpdateMarketplacePolicyStatusStored As Boolean = True

    Public Const ACGetInsuranceFileTypeIdFromCodeName As String = "GetInsuranceFileTypeIdFromCode"
    Public Const ACGetInsuranceFileTypeIdFromCodeSQL As String = "spu_Get_InsuranceFileTypeId_From_Code"
    Public Const ACGetInsuranceFileTypeIdFromCodeStored As Boolean = True

    Public Const kCopyPolicyAssociateStored As Boolean = True
    Public Const kCopyPolicyAssociateName As String = "ACCopyPolicyAssociates"
    Public Const kCopyPolicyAssociatesSQL As String = "spu_SIR_copy_insurance_file_associates"


    Public Const GetUnderwritingVersionByDateName As String = "GetUnderwritingVersionByDate"
    Public Const GetUnderwritingVersionByDateSQL As String = "spu_Get_Underwriting_Version_By_Date"


    'CheckDataModelCompatibility - WPR12
    Public Const ACCheckDataModelCompatibilityStored As Boolean = True
    Public Const ACCheckDataModelCompatibilityName As String = "ACCheckDataModelCompatibility"
    Public Const ACCheckDataModelCompatibilitySQL As String = "spu_Get_DataModel_Code_For_Risk"

    'CopyPolicyToQuoteWithoutVersioning - WPR12
    Public Const ACCopyPolicyToQuoteWithoutVersioningStored As Boolean = True
    Public Const ACCopyPolicyToQuoteWithoutVersioningName As String = "ACCopyPolicyToQuoteWithoutVersioning"
    Public Const ACCopyPolicyToQuoteWithoutVersioningSQL As String = "spu_SAM_Copy_Quote_Without_Versioning"

    'UpdateRiskStatus - WPR12
    Public Const ACUpdateRiskStatusStored As Boolean = True
    Public Const ACUpdateRiskStatusName As String = "ACUpdateRiskStatus"
    Public Const ACUpdateRiskStatusSQL As String = "spu_SIR_Update_Risk_Status"

    'UpdateRiskType - WPR12
    Public Const ACUpdateRiskTypeStored As Boolean = True
    Public Const ACUpdateRiskTypeName As String = "ACUpdateRiskType"
    Public Const ACUpdateRiskTypeSQL As String = "spu_SIR_Update_Risk_Type"

    'UpdateIsRiskSelected - WPR12
    Public Const ACUpdateIsRiskSelectedStored As Boolean = True
    Public Const ACUpdateIsRiskSelectedName As String = "ACUpdateIsRiskSelected"
    Public Const ACUpdateIsRiskSelectedSQL As String = "spe_Is_Risk_Selected_upd"

    Public Const ACUpdateEventlogUserStored As Boolean = False
    Public Const ACUpdateEventlogUserName As String = "UpdateEventLog"
    Public Const ACUpdateEventlogUserSQL As String = "UPDATE event_log SET user_id = {user_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "WHERE insurance_file_cnt = {insurance_file_cnt}"
End Module
