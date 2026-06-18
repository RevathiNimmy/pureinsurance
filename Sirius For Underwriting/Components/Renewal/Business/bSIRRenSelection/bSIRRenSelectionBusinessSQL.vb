Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module BusinessSQL
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES

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
    '              bSIRRenSelection.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements
    Public Const ACSelRenewalPolicyStored As Boolean = True
    Public Const ACSelRenewalPolicyName As String = "SelRenewalPolicies"
    'developer guide no. 39
    Public Const ACSelRenewalPolicySQL As String = "spu_Sel_Renewal_Policies"

    Public Const ACDelRenewalStatusStored As Boolean = True
    Public Const ACDelRenewalStatusName As String = "DelRenewalStatus"
    Public Const ACDelRenewalStatusSQL As String = "spu_Del_Renewal_Status"

    Public Const ACDelLastPrintRunStored As Boolean = True
    Public Const ACDelLastPrintRunName As String = "DelLastPrintRun"
    Public Const ACDelLastPrintRunSQL As String = "spu_Del_Last_Print_Run"

    Public Const ACSelRenewalListStored As Boolean = True
    Public Const ACSelRenewalListName As String = "GetRenewalList"
    Public Const ACSelRenewalListSQL As String = "spu_Get_Renewal_PreList"

    Public Const ACUpdRenewalPolicyStored As Boolean = True
    Public Const ACUpdRenewalPolicyName As String = "UpdateRenewalPolicies"
    Public Const ACUpdRenewalPolicySQL As String = "spu_upd_Renewal_Policies"

    Public Const ACAddRenewalStatusStored As Boolean = True
    Public Const ACAddRenewalStatusName As String = "AddRenewalStatus"
    Public Const ACAddRenewalStatusSQL As String = "spe_Renewal_Status_add"

    Public Const ACAddRenewalReportStored As Boolean = True
    Public Const ACAddRenewalReportName As String = "AddRenewalReport"
    Public Const ACAddRenewalReportSQL As String = "spu_Add_RenewalReport"

    Public Const ACDelRenewalReportStored As Boolean = True
    Public Const ACDelRenewalReportName As String = "DelRenewalReport"
    Public Const ACDelRenewalReportSQL As String = "spu_Del_RenewalReport"

    'Public Const ACSelRiskIDStored = False
    'Public Const ACSelRiskIDName = "GetRiskID"
    'Public Const ACSelRiskIDSQL = "SELECT risk_id, gis_screen_id FROM Risk" & vbCrLf & _
    ''                                            "WHERE insurance_file_cnt = {insurance_file_cnt}" & vbCrLf & _
    ''                                            "AND is_not_index_linked <> 1"

    'RWH(20/11/2000) New Risk select query using Insurance_file_risk_link table.
    Public Const ACSelRiskCntStored As Boolean = False
    Public Const ACSelRiskCntName As String = "GetRiskCnt"
    Public Const ACSelRiskCntSQL As String = "SELECT risk_cnt, gis_screen_id FROM Risk" & Strings.ChrW(13) & Strings.ChrW(10) &
                                             "WHERE risk_cnt IN" & Strings.ChrW(13) & Strings.ChrW(10) &
                                             "(SELECT risk_cnt FROM Insurance_file_risk_link " & Strings.ChrW(13) & Strings.ChrW(10) &
                                             "WHERE insurance_file_cnt = {insurance_file_cnt})" & Strings.ChrW(13) & Strings.ChrW(10) &
                                             "AND is_not_index_linked <> 1"

    Public Const ACSelIndexLinkStored As Boolean = False
    Public Const ACSelIndexLinkName As String = "GetIndexLink"
    Public Const ACSelIndexLinkSQL As String = "SELECT SI.RSA_policy_binder_id, SI.sum_insured_type_id, " & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "SI.sequence_id, SI.sum_insured, SIT.index_linking_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "FROM GIS_Policy_Link PL," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "RSA_Policy_Binder PB," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "RSA_Sum_Insured SI," & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "Sum_Insured_Type SIT" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "Where PL.insurance_file_cnt = {insurance_file_cnt}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "AND PL.risk_id = {risk_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "AND PL.gis_policy_link_id = PB.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "AND PB.RSA_policy_binder_id = SI.RSA_policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "AND SI.sum_insured_type_id = SIT.sum_insured_type_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "AND SIT.index_linking_id is not null"

    Public Const ACSelIndexLinkDetailStored As Boolean = True
    Public Const ACSelIndexLinkDetailName As String = "GetIndexLinkDetail"
    Public Const ACSelIndexLinkDetailSQL As String = "spu_index_linking_detail_sel"

    Public Const ACUpdRSASumInsuredStored As Boolean = False
    Public Const ACUpdRSASumInsuredName As String = "UpdRSASumInsured"
    Public Const ACUpdRSASumInsuredSQL As String = "UPDATE RSA_Sum_Insured" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "SET sum_insured = {sum_insured}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "WHERE RSA_policy_binder_id = {rsa_policy_binder_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "AND sum_insured_type_id = {sum_insured_type_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                   "AND sequence_id = {sequence_id}"

    Public Const ACSelGISIndexLinkStored As Boolean = False
    Public Const ACSelGISIndexLinkName As String = "GetGISIndexLink"
    Public Const ACSelGISIndexLinkSQL As String = "SELECT  GDM.code, GO.object_name, " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "GP.Property_name, GP.index_linking_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "FROM Gis_Screen GS," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "Gis_Data_Model GDM," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "Gis_Object GO," & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "Gis_Property GP" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "Where GS.gis_screen_id = {gis_screen_id}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "AND GS.gis_data_model_id = GDM.gis_data_model_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "AND GDM.gis_data_model_id = GO.gis_data_model_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "AND GO.gis_object_id = GP.gis_object_id" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "AND ISNULL(GP.index_linking_id,0) <> 0" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "ORDER BY GO.object_name"

    'JMK 08/08/2001 - prod.claim_year_to_check: default null to 1
    Public Const ACSelCheckForClaimStored As Boolean = True
    Public Const ACSelCheckForClaimName As String = "CheckForClaim"
    Public Const ACSelCheckForClaimSQL As String = "spu_CLM_Check_For_Claim"



    'RWH(17/11/2000) Extra proc to check details of non-allowable claims.
    Public Const ACSelCheckForClaimValueStored As Boolean = True
    Public Const ACSelCheckForClaimValueName As String = "CheckForClaimValue"
    Public Const ACSelCheckForClaimValueSQL As String = "spu_Report_Ren_Check_For_Claim"


    Public Const ACSelOriginalPolicyStored As Boolean = False
    Public Const ACSelOriginalPolicyName As String = "GetOriginalVersionPolicy"
    Public Const ACSelOriginalPolicySQL As String = "SELECT insurance_file_cnt FROM renewal_status" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "WHERE renewal_insurance_file_cnt = {renewal_insurance_file_cnt}" & Strings.ChrW(13) & Strings.ChrW(10)

    Public Const ACUpdPolicyStatusStored As Boolean = False
    Public Const ACUpdPolicyStatusName As String = "UpdatePolicyStatus"
    Public Const ACUpdPolicyStatusSQL As String = "UPDATE Insurance_File SET insurance_file_status_id = null" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                  "WHERE insurance_file_cnt = {insurance_file_cnt}"

    Public Const ACUpdRiskStatusStored As Boolean = False
    Public Const ACUpdRiskStatusName As String = "UpdateRiskStatus"
    Public Const ACUpdRiskStatusSQL As String = "UPDATE Risk SET risk_status_id = 3" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                "WHERE risk_cnt = {risk_cnt}"

    Public Const ACDelRenewalStatusRecordStored As Boolean = False
    Public Const ACDelRenewalStatusRecordName As String = "DeleteRenewalStatus"
    Public Const ACDelRenewalStatusRecordSQL As String = "DELETE Renewal_Status WHERE renewal_insurance_file_cnt = {renewal_insurance_file_cnt}"

    Public Const ACSelRenewalVersionStored As Boolean = False
    Public Const ACSelRenewalVersionName As String = "GetAllRenewalVersionOfPolicy"
    Public Const ACSelRenewalVersionSQL As String = "SELECT  renewal_insurance_file_cnt,cover_start_date,insurance_folder_cnt  FROM renewal_status rs join insurance_file ifi ON rs.renewal_insurance_file_cnt=ifi.insurance_file_cnt " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "WHERE ifi.insurance_file_cnt in" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "(SELECT insurance_file_cnt FROM insurance_file" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "WHERE ifi.insurance_folder_cnt = {insurance_folder_cnt})"

    Public Const ACSelRenewalQuoteRefStored As Boolean = False
    Public Const ACSelRenewalQuoteRefName As String = "GetAllRenewalQuoteRefOfPolicy"
    Public Const ACSelRenewalQuoteRefSQL As String = "SELECT  insurance_ref FROM insurance_file" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "WHERE insurance_file_cnt = {insurance_file_cnt}"

    Public Const ACSelPolicyForRenewalStored As Boolean = True
    Public Const ACSelPolicyForRenewalName As String = "GetCorrectVersionOfPolicyForRenewal"
    Public Const ACSelPolicyForRenewalSQL As String = "spu_Get_Policy_For_Renewal"

    Public Const ACDelPolicyDependantStored As Boolean = True
    Public Const ACDelPolicyDependantName As String = "DeletePolicyDependant"
    Public Const ACDelPolicyDependantSQL As String = "spu_Del_Policy_Dependant"

    ' Delete SIRInsuranceFileSystem SQL
    Public Const ACDelInsFileSystemStored As Boolean = True
    Public Const ACDelInsFileSystemName As String = "DeleteSIRInsuranceFileSystem"
    Public Const ACDelInsFileSystemSQL As String = "spe_Insurance_File_System_del"

    ' Delete SIRInsuranceFile SQL
    Public Const ACDelInsFileStored As Boolean = True
    Public Const ACDelInsFileName As String = "DeleteSIRInsuranceFile"
    Public Const ACDelInsFileSQL As String = "spe_Insurance_File_del"


    'RWH(23/02/2001) Insert Risk Standard Wordings.
    Public Const ACInsertRiskWordingStored As Boolean = False
    Public Const ACInsertRiskWordingName As String = "InsertRiskWording"
    Public Const ACInsertRiskWordingSQL As String = "INSERT INTO {table_name}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                    "VALUES ({policy_binder_id}, {sequence_id}, {document_template_id})"


    'RWH(23/02/2001) Insert Policy Standard Wordings.
    Public Const ACInsertPolicyWordingStored As Boolean = False
    Public Const ACInsertPolicyWordingName As String = "InsertPolicyWording"
    Public Const ACInsertPolicyWordingSQL As String = "INSERT INTO policy_standard_wording" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "VALUES ({insurance_file_cnt}, {policy_standard_wording_id}, {document_template_id},{do_not_merge})"

    'Midnight Renewal
    Public Const ACSelectIsMidnightRenewalStored As Boolean = False
    Public Const ACSelectIsMidnightRenewalName As String = "GetIsMidnightRenewal"
    Public Const ACSelectIsMidnightRenewalSQL As String = "SELECT is_midnight_renewal FROM Product" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                          "WHERE product_id = {product_id}"


    'RWH(24/05/2001) Renewal report checks.
    'Public Const ACSelRenewalReportManualStored = True
    'Public Const ACSelRenewalReportManualName = "GetRenewalReportManual"
    'Public Const ACSelRenewalReportManualSQL = "{call spu_Report_Manual_Renewal}"
    '
    'Public Const ACSelRenewalReportAutoStored = True
    'Public Const ACSelRenewalReportAutoName = "GetRenewalReportAuto"
    'Public Const ACSelRenewalReportAutoSQL = "{call spu_Report_Automatic_Renewal}"


    Public Const ACRenewalReportExistsStored As Boolean = True
    Public Const ACRenewalReportExistsName As String = "GetRenewalReportExists"
    Public Const ACRenewalReportExistsSQL As String = "spu_Renewal_Report_Exists"

    'RWH(13/06/01) Copy coinsurance for renewal.
    Public Const ACRenewalCopyCoinsStored As Boolean = True
    Public Const ACRenewalCopyCoinsName As String = "CopyCoinsurance"
    Public Const ACRenewalCopyCoinsSQL As String = "spu_copy_coinsurance"

    'TN20010719 - start
    Public Const ACIsPolicyRICompleteStored As Boolean = True
    Public Const ACIsPolicyRICompleteName As String = "IsPolicyRIComplete"
    Public Const ACIsPolicyRICompleteSQL As String = "spu_Policy_RI_Value"

    Public Const ACIsRiskRICompleteStored As Boolean = True
    Public Const ACIsRiskRICompleteName As String = "IsRiskRIComplete"
    Public Const ACIsRiskRICompleteSQL As String = "spu_Risk_RI_Value"

    Public Const ACIsQuotedStored As Boolean = True
    Public Const ACIsQuotedName As String = "Is Policy Quoted"
    Public Const ACIsQuotedSQL As String = "spu_Is_Quoted"
    'TN20010719 - end

    Public Const ACRenewalCopyAgentCommissionStored As Boolean = True
    Public Const ACRenewalCopyAgentCommissionName As String = "CopyAgentCommission"
    Public Const ACRenewalCopyAgentCommissionSQL As String = "spu_copy_agent_commission"

    Public Const ACRenewalCopyInsuranceFileAgentStored As Boolean = True
    Public Const ACRenewalCopyInsuranceFileAgentName As String = "CopyInsuranceFileAgent"
    Public Const ACRenewalCopyInsuranceFileAgentSQL As String = "spu_copy_insurance_file_agent"

    ' Select Transaction Type SQL
    Public Const ACGetTransactionTypeStored As Boolean = False
    Public Const ACGetTransactionTypeName As String = "GetTransactionType"
    Public Const ACGetTransactionTypeSQL As String = "select transaction_type_id from transaction_type where code = {code} and is_deleted = 0"

    'Is Premium Zero
    Public Const ACSelectIsPremiumZeroStored As Boolean = False
    Public Const ACSelectIsPremiumZeroName As String = "IsPremiumZero"
    Public Const ACSelectIsPremiumZeroSQL As String = "SELECT this_premium FROM Insurance_File" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "WHERE insurance_file_cnt = {insurance_file_cnt}"

    'Thinh Nguyen 15/08/2003
    Public Const ACSelIsAgentCancelledStored As Boolean = True
    Public Const ACSelIsAgentCancelledName As String = "IsAgentCancelled"
    Public Const ACSelIsAgentCancelledSQL As String = "spu_IsAgentCancelled"

    Public Const ACSelIsInstalmentStored As Boolean = True
    Public Const ACSelIsInstalmentName As String = "Is Instalments"
    Public Const ACSelIsInstalmentSQL As String = "spu_Is_Instalment"

    Public Const ACGetBrokerTransferPortfolioDetailStored As Boolean = False
    Public Const ACGetBrokerTransferPortfolioDetailName As String = "Get Broker Transfer Portfolio detail"
    Public Const ACGetBrokerTransferPortfolioDetailSQL As String = "SELECT  pa.party_cnt, pa.is_in_transfer_mode, pa.transfer_to_party_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                   "FROM    Party_Agent pa JOIN Insurance_File ifi ON pa.party_cnt = ifi.lead_agent_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                   "WHERE   ifi.insurance_file_cnt = {InsuranceFileCnt}"

    Public Const kGetBlacklistedClientDetailsForInsuranceFileName As String = "returns the blacklist reason id / desc for the insured on the specified insuance file"
    Public Const kGetBlacklistedClientDetailsForInsuranceFileSQL As String = "spu_SIR_Get_Insured_Blacklist_Reason"

    Public Const ACGetRIModelTypeStored As Boolean = False
    Public Const ACGetRIModelTypeName As String = "Get RI Model Type"
    Public Const ACGetRIModelTypeSQL As String = "SELECT ra.ri_model_id, rm.ri_model_type FROM RI_Arrangement ra JOIN RI_Model rm ON ra.ri_model_id = rm.ri_model_id " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "WHERE ra.risk_cnt = {RiskCnt}"

    Public Const kFindAnniversaryCopyName As String = "returns the number of anniversary copies found for the specified insurance ref on or after the specified cover start date"
    Public Const kFindAnniversaryCopySQL As String = "spu_SIR_Find_Anniversary_Copy"


    Public Const kDeleteRenewalsLastPrintRunName As String = "deletes any associated last print run record for a specified renewal insurance file cnt"
    Public Const kDeleteRenewalsLastPrintRunSQL As String = "spu_SIR_Delete_Last_Print_Run"

    Public Const kGetAllVersionsStored As Boolean = True
    Public Const kGetAllVersionsName As String = "getAllAnnualLiveVersions"
    Public Const kGetAllVersionsSQL As String = "spu_get_all_version_info"

    Public Const ACCopyRiskStandardWordingsStored As Boolean = True
    Public Const ACCopyRiskStandardWordingsName As String = "CopyRiskStandardWordings"
    Public Const ACCopyRiskStandardWordingsSQL As String = "spu_Copy_RISK_Standard_Wording"

    'WR25 1.12
    Public Const ACCheckRenewalProductName As String = "CheckRenewalProduct"
    Public Const ACCheckRenewalProductSQL As String = "spu_SIR_Renewal_Check_Renewal_Product"

    Public Const ACSelectRenewalProductName As String = "Select Renewal Product"
    Public Const ACSelectRenewalProductSQL As String = "Spu_Sir_insurance_file_SelectRenewalProduct"

    Public Const ACUpdateRenewalProductName As String = "UpdateRenewalProduct"
    Public Const ACUpdateRenewalProductSQL As String = "Spu_Sir_insurance_file_UpdateRenewalProduct"

    Public Const ACIsTMPProductName As String = "IsTrueMonthlyPolicyProduct"
    Public Const ACIsTMPProductSQL As String = "Spu_Sir_IsTMPProduct"

    Public Const ACGetInstalmentFrequencyName As String = "GetInstalmentFrequency"
    Public Const ACGetInstalmentFrequencySQL As String = "spu_Get_installment_frequency"

    'Task WR9 Batch Renewal - Multi Threaded Controller
    Public Const ACGetRenewalSelectionDetailsStored As Boolean = True
    Public Const ACGetRenewalSelectionDetailsName As String = "GetRenewalSelectionDetails"
    Public Const ACGetRenewalSelectionDetailsSQL As String = "spu_SIR_Get_Renewal_Selection_Details"

    Public Const ACAddBatchRenewalJobRunsStored As Boolean = True
    Public Const ACAddBatchRenewalJobRunsName As String = "AddBatchRenewalJobRuns"
    Public Const ACAddBatchRenewalJobRunsSQL As String = "spu_SIR_Add_Batch_Renewal_Job_Runs"

    Public Const ACGetBatchJobPrintingOptionsStored As Boolean = True
    Public Const ACGetBatchJobPrintingOptionsName As String = "GetBatchJobPrintingOptions"
    Public Const ACGetBatchJobPrintingOptionsSQL As String = "spu_SIRRen_Get_Batch_Job_Printing_Options"


    Public Const ACGetMaxPolicyVersionNoStored As Boolean = True
    Public Const ACGetMaxPolicyVersionNoName As String = "GetMaxPolicyVersionNo"
    Public Const ACGetMaxPolicyVersionNoSQL As String = "spu_get_max_policy_version_no"

    Public Const ACCopyPolicyClientsStored As Boolean = True
    Public Const ACCopyPolicyClientsName As String = "ACCopyPolicyClients"
    Public Const ACCopyPolicyClientsSQL As String = "spu_copy_policy_clients"

    Public Const ACGetPolicyDetailsName As String = "GetPolicyDetails"
    Public Const ACGetPolicyDetailsSQL As String = "spu_get_REN_policy_details"

    Public Const ACBatchRenewalJobSelName As String = "BatchRenewalJobSel"
    Public Const ACBatchRenewalJobSelSQL As String = "spu_SIR_Batch_Renewal_Job_sel"

    Public Const ACSelIsInstalmentAndActivePartyBankName As String = "Is Instalments AND Active Party Bank"
    Public Const ACSelIsInstalmentAndActivePartyBankSQL As String = "spu_Is_Instalment_and_Active_PartyBank"

    Public Const ACUpdateRENStatusName As String = "Update Renewal Status"
    Public Const ACUpdateRENStatusSQL As String = "spu_REN_update_renewal_status"

    Public Const ACGetInitialPolicyDetailsName As String = "GetInitialPolicyDetails"
    Public Const ACGetInitialPolicyDetailsSQL As String = "spu_Get_Initial_Policy_Payment_Terms"

    Public Const ACReorderLaterPolicyVersionsName As String = "Reorder Later Policy Versions"
    Public Const ACReorderLaterPolicyVersionsSQL As String = "spu_reorder_later_policy_versions"

    Public Const ACGetPriorTermSchemeInsuranceFileName As String = "GetPriorTermSchemeInsuranceFile"
    Public Const ACGetPriorTermSchemeInsuranceFileSQL As String = "spu_Get_Prior_Term_Scheme_Insurance_file_cnt"

    Public Const ACGetInsFolderFromInsRefName As String = "GetInsFolderFromInsRef"
    Public Const ACGetInsFolderFromInsRefSQL As String = "spu_Get_InsFolderFromInsRef"


    Public Const kStartBatchForInsuranceFolderName As String = "Batch Renewal Start Insurance Folder"
    Public Const kStartBatchForInsuranceFolderSQL As String = "spu_SIR_Batch_Renewal_Job_Start"

    Public Const kGetRiskProcessingParamatersName As String = "Batch Renewal Risk Paramaters"
    Public Const kGetRiskProcessingParamatersSQL As String = "spu_SIR_Batch_Renewal_Job_Risk_Paramaters"
    '
    Public Const kCompleteBatchForInsuranceFolderName As String = "Batch Renewal Completed Insurance Folder"
    Public Const kCompleteBatchForInsuranceFolderSQL As String = "spu_SIR_Batch_Renewal_Job_Completed"

    Public Const kGetBatchInsuranceFileName As String = "Get the insurance file cnt "
    Public Const kGetBatchInsuranceFileSQL As String = "spu_SIRRen_Get_Renewal_Selection_Policy_List"

    Public Const kGetPaymentTermsName As String = "gets PaymentTerms"
    Public Const kGetPaymentTermsSQL As String = "spu_SIR_Get_Payment_Terms"

    'Check for the existence of Instalment Scheme for the Product
    Public Const kGetInstalmentSchemeStored As Boolean = False
    Public Const kGetInstalmentSchemeName As String = "Get Instalment Scheme for Product"
    Public Const kGetInstalmentSchemeSQL As String = "SELECT count(*) FROM PFSchemeProducts WHERE product_id = (Select Product_id from insurance_file where insurance_file_cnt= {insurance_file_cnt})"

    Public Const kUpdateInsuranceFileCntForAnniversaryVersionSQL As String = "spu_Update_InsuranceFileCnt_For_Anniversary_Version"
    Public Const kUpdateInsuranceFileCntForAnniversaryVersionName As String = "UpdateInsuranceFileCntForAnniversaryVersion"

    Public Const kSelLatestPolicyVersionStored As Boolean = True
    Public Const kSelLatestPolicyVersionName As String = "GetLatestPolicyVersion"
    Public Const kSelLatestPolicyVersionSQL As String = "spu_Get_Latest_Policy_Version"

    Public Const ACGetLatestVersionOfDocumentTemplateName As String = "GetLatestVersionOfDocumentTemplate"
    Public Const ACGetLatestVersionOfDocumentTemplateSQL As String = "spu_get_latest_document_version_template_id"

    Public Const kHasInstalmentPlanOnCurrentTermName As String = "HasInstalmentPlanOnCurrentTerm"
    Public Const kHasInstalmentPlanOnCurrentTermSQL As String = "spu_HasInstalment_Plan_CurrentTerm"

    Public Const ACUpdatePaymentMethodStored As Boolean = True
    Public Const ACpdatePaymentMethodName As String = "Update Payment Method"
    Public Const ACpdatePaymentMethodSQL As String = "spu_PFPaymentMethod_Upd"
    Public Const ACCopyPolicyAssociatesName As String = "CopyPolicyAssociates"
    Public Const ACCopyPolicyAssociatesSQL As String = "spu_SIR_copy_insurance_file_associates"

    Public Const kGetDefaultPaymentTermsName As String = "Get Default Payment Terms"
    Public Const kGetDefaultPaymentTermsSQL As String = "spu_SAM_Get_Default_Payment_Terms"

    Public Const kGetMaxPolicyVersionStored As Boolean = True
    Public Const kGetMaxPolicyVersionName As String = "Get Max Policy Version"
    Public Const kGetMaxPolicyVersionSQL As String = "spu_Get_Maximum_PolicyVersion"

    Public Const ACIsClonedPolicyName As String = "IsClonedPolicyNumber"
    Public Const ACIsClonedPolicySQL As String = "spu_Skip_New_Policy_Number"

End Module