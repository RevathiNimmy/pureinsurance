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
    ' Date: 26/09/00
    '
    ' Description: Contains the SQL Statements required by the
    '              bSIRRenewal.Business class.
    '
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Example select using embedded SQL
    ' Encode parameter names using PMStartDelimiter and PMEndDelimiter defined in general.bas

    'CT 26/09/00
    ' Insert accumulation SQL'
    'Public Const ACInsertAccumulationStored = False
    'Public Const ACInsertAccumulationName = "InsertAccumulation"
    'Public Const ACInsertAccumulationSQL = "insert into accumulation (accumulation_id, code, caption_id, description, is_deleted, effective_date,quick_code, caption, parent_id, accumulation_class_id) VALUES ({accumulation_id},{code},{caption_id},{description},{is_deleted},{effective_date},{quick_code},{caption},{parent_id},{accumulation_class_id})"

    Public Const ACSelRiskCntStored As Boolean = True
    Public Const ACSelRiskCntName As String = "GetRiskTypeCnt"
    'developer guide no. 39
    Public Const ACSelRiskCntSQL As String = "spu_risks_for_policy_saa(?)"

    Public Const ACDeleteCreditControlItemStored As Boolean = True
    Public Const ACDeleteCreditControlItemName As String = "Delete Credit Control Item"
    Public Const ACDeleteCreditControlItemSQL As String = "spu_ACT_Del_Credit_Control_Item_InsFile"

    'TN20010709 - start
    Public Const ACUpdRenewalCountStored As Boolean = True
    Public Const ACUpdRenewalCountName As String = "Update Renewal Count"
    Public Const ACUpdRenewalCountSQL As String = "spu_Update_Renewal_Count"

    Public Const ACIsQuotedStored As Boolean = True
    Public Const ACIsQuotedName As String = "Is Quoted"
    Public Const ACIsQuotedSQL As String = "spu_Is_Quoted"
    'TN20010709 - end

    ' PW060203 - retrieve deposit amount
    ' PS209
    Public Const ACGetDepositAmountStored As Boolean = True
    Public Const ACGetDepositAmountName As String = "Get Deposit Amount"
    Public Const ACGetDepositAmountSQL As String = "spu_Get_Deposit_Amount"

    'Thinh Nguyen 26/04/2002 (start)
    Public Const ACSelIsInstalmentStored As Boolean = True
    Public Const ACSelIsInstalmentName As String = "Is Instalments"
    'PSL 10/09/2003 Issue 6756
    Public Const ACSelIsInstalmentSQL As String = "spu_Is_Instalment"

    Public Const ACSelPaymentMethodStored As Boolean = True
    Public Const ACSelPaymentMethodName As String = "Get Payment Method"
    Public Const ACSelPaymentMethodSQL As String = "spu_GetPaymentMethod"
    'Thinh Nguyen 26/04/2002 (end)

    'Thinh Nguyen 01/10/2002 - start
    Public Const ACSelDocTypeIDStored As Boolean = False
    Public Const ACSelDocTypeIDName As String = "GetDocumentTypeID"
    Public Const ACSelDocTypeIDSQL As String = "SELECT document_type_id FROM document_type" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "WHERE code = {code}" & Strings.ChrW(13) & Strings.ChrW(10)
    'Thinh Nguyen 01/10/2002 - end

    Public Const ACSelIsMidNightRenewalStored As Boolean = False
    Public Const ACSelIsMidNightRenewalName As String = "IsMidnightRenewal"
    Public Const ACSelIsMidNightRenewalSQL As String = "SELECT p.is_midnight_renewal FROM Insurance_File ifi JOIN Product p ON ifi.product_id = p.product_id WHERE ifi.insurance_file_cnt = {InsuranceFileCnt}"

    Public Const kGetAllUserBranchesName As String = "Returns all branches for the specified user"
    Public Const kGetAllUserBranchesSQL As String = "spu_SIR_GetAllUserBranches"

    Public Const ACUpdateRenewalStatusStored As Boolean = True
    Public Const ACUpdateRenewalStatusName As String = "Update Renewal Status"
    Public Const ACUpdateRenewalStatusSQL As String = "spu_UpdateRenewalStatus"

    Public Const ACUpdateTransferBrokerStored As Boolean = True
    Public Const ACUpdateTransferBrokerName As String = "Transfer Broker"
    Public Const ACUpdateTransferBrokerSQL As String = "spu_TransferBroker"

    Public Const ACUpdLapsedPolicyStored As Boolean = False
    Public Const ACUpdLapsedPolicyName As String = "Lapsed Policy"
    Public Const ACUpdLapsedPolicySQL As String = "UPDATE Insurance_File SET insurance_file_status_id = 2 WHERE insurance_folder_cnt = {InsuranceFolderCnt} AND (insurance_file_type_id IN (2, 3, 5, 6, 8, 9) OR insurance_file_status_id IS Null)"

    Public Const kValidateAcceptTMPIsValidActionName As String = "returns the insurance file cnt of the latest version of the policy that is also the final version of the previous true monthly policy cycle"
    Public Const kValidateAcceptTMPIsValidActionSQL As String = "spu_SIR_Get_Is_Accept_TMP_Valid_Action"

    Public Const ACDelAgentCommissionStored As Boolean = False
    Public Const ACDelAgentCommissionName As String = "Delete AgentCommission"
    Public Const ACDelAgentCommissionSQL As String = "Delete Agent_Commission" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "FROM Insurance_File ifi LEFT JOIN Agent_Commission ac on ifi.insurance_file_cnt = ac.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "WHERE ifi.insurance_file_cnt = {InsuranceFileCnt}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "AND IsNull(ifi.lead_agent_cnt,0) = 0"


    Public Const ACGetProductPrintOptionsName As String = "GetProductPrintOptions"
    Public Const ACGetProductPrintOptionsSQL As String = "select produce_schedule,produce_certificate,produce_debit_note from product where product_id={Product_id}"

    'Get Branch Agents
    Public Const ACGetBranchAgentsStored As Boolean = True
    Public Const ACGetBranchAgentsName As String = "Get Branch Agents"
    Public Const ACGetBranchAgentsSQL As String = "spu_Get_BranchAgents"

    Public Const ACSelectAccountIDStored As Boolean = False
    Public Const ACSelectAccountIDName As String = "GetAccountID"
    Public Const ACSelectAccountIDSQL As String = "Select Account_id from Account " &
                                                  " JOIN Party ON Account.Account_key = party.party_cnt" &
                                                  " WHERE party.party_cnt={Party_cnt}"

    Public Const ACSelectPolicyGrossTotalStored As Boolean = True
    Public Const ACSelectPolicyGrossTotalName As String = "GetPolicyGrossTotal"
    Public Const ACSelectPolicyGrossTotalSQL As String = "spu_Get_GrossTotal"


    Public Const ACSelCurrencyandAgentTypeStored As Boolean = True
    Public Const ACSelCurrencyandAgentTypeName As String = "Get Currency and Agent Type"
    Public Const ACSelCurrencyandAgentTypeSQL As String = "spu_GetCurrencyAndAgentType"


    Public Const ACGetPolicyNumberChangeOnRenewalName As String = "Policy Number To Change"
    Public Const ACGetPolicyNumberChangeOnRenewalSQL As String = "Select change_policy_number_at_renewal from product pr " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                 "LEFT JOIN insurance_file ifi on pr.product_id = ifi.product_id " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                 "WHERE ifi.insurance_file_cnt = {InsuranceFileCnt}"

    Public Const ACGetRenewalInstallmentPlanName As String = "Renewal Installment Plan"

    Public Const ACGetRenewalInstallmentPlanSQL As String = "SELECT pfs.bank_name_mandatory,pfs.bank_address_mandatory,pfs.branch_name_mandatory,pfs.branch_code_mandatory, " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "pfp.bankname,pfp.bankaddr1,pfp.bankpcode,pfp.bankbranch,pfp.banksortcode from pfscheme pfs " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                            "LEFT JOIN pfpremiumfinance pfp on pfs.schemeno = pfp.schemeno and pfp.TransType = 'REN' where pfp.insurance_file_cnt = {InsuranceFileCnt}"

    Public Const ACGetPolicyPaymentMethodName As String = "Policy Payment Method"
    Public Const ACGetPolicyPaymentMethodSQL As String = "Select payment_method from insurance_file WHERE insurance_file_cnt = {InsuranceFileCnt}"


    'Email Customer
    Public Const ACSelRenewalCustomerEmailDocumentTemplateStored As Boolean = True
    Public Const ACSelRenewalCustomerEmailDocumentTemplateName As String = "Get Renewal Customer Email Document Template"
    Public Const ACSelRenewalCustomerEmailDocumentTemplateSQL As String = "spu_renewal_customer_email_document_template_sel"
    '*****

    Public Const ACSelRenewalAgentEmailDocumentTemplateStored As Boolean = True
    Public Const ACSelRenewalAgentEmailDocumentTemplateName As String = "Get Renewal Agent Email Document Template"
    Public Const ACSelRenewalAgentEmailDocumentTemplateSQL As String = "spu_renewal_agent_email_document_template_sel"

    Public Const ACSelEmailContactStored As Boolean = True
    Public Const ACSelEmailContactName As String = "Get Main Email ContactAddress"
    Public Const ACSelEmailContactSQL As String = "spu_email_contact_select"

    Public Const ACUpdEmailSentStatusStored As Boolean = True
    Public Const ACUpdEmailSentStatusName As String = "Update the Email Sent Status"
    Public Const ACUpdEmailSentStatusSQL As String = "spu_Renewal_Status_Email_Sent_upd"

    Public Const ACGetProductTradeRNLOnlinOptionStored As Boolean = True
    Public Const ACGetProductTradeRNLOnlinOptionName As String = "Get Product Trade Rnl Online Option"
    Public Const ACGetProductTradeRNLOnlinOptionSQL As String = "spu_get_product_trade_rnl_online_option"

    'Task WR9 Batch Renewal - Multi Threaded Controller
    Public Const ACGetRenewalAcceptanceDetailsStored As Boolean = True
    Public Const ACGetRenewalAcceptanceDetailsName As String = "GetRenewalAcceptanceDetails"
    Public Const ACGetRenewalAcceptanceDetailsSQL As String = "spu_SIR_Get_Renewal_Acceptance_Details"

    Public Const ACGetRenewalPolicyDetailsStored As Boolean = True
    Public Const ACGetRenewalPolicyDetailsName As String = "GetRenewalPolicyDetails"
    Public Const ACGetRenewalPolicyDetailsSQL As String = "spu_SIR_Get_Renewal_Policy_Details"

    Public Const ACGetRenewalDetailsStored As Boolean = True
    Public Const ACGetRenewalDetailsName As String = "GetRenewalDetails"
    Public Const ACGetRenewalDetailsSQL As String = "spu_get_renewal_details"

    Public Const ACUpdRenewalPolicyDetailsStored As Boolean = True
    Public Const ACUpdRenewalPolicyDetailsName As String = "LapsePolicy"
    Public Const ACUpdRenewalPolicyDetailsSQL As String = "spu_insurance_file_update"

    Public Const ACAddBatchRenewalJobRunsStored As Boolean = True
    Public Const ACAddBatchRenewalJobRunsName As String = "AddBatchRenewalJobRuns"
    Public Const ACAddBatchRenewalJobRunsSQL As String = "spu_SIR_Add_Batch_Renewal_Job_Runs"

    Public Const ACGetBatchJobPrintingOptionsStored As Boolean = True
    Public Const ACGetBatchJobPrintingOptionsName As String = "GetBatchJobPrintingOptions"
    Public Const ACGetBatchJobPrintingOptionsSQL As String = "spu_SIRRen_Get_Batch_Job_Printing_Options"

    'Start-(Arul Stephen)-(PN Fixing-PN 59278)
    Public Const ACGetRenewalPaymentMethodStored As Boolean = True
    Public Const ACGetRenewalPaymentMethodName As String = "GetRenewalAcceptanceDetails"
    Public Const ACGetRenewalPaymentMethodSQL As String = "spu_Renewal_Get_Payment_Method_Del_PF"

    Public Const ACCheckSingleInstalmentPolicyName As String = "CheckSingleInstalmentPolicy"
    Public Const ACCheckSingleInstalmentPolicySQL As String = "spu_Check_Single_Instalment_policy"

    Public Const kRenewalGetPreviousVersionName As String = "Returns the previou version insurance file cnt for the specified insurance_file_cnt"
    Public Const kRenewalGetPreviousVersionSQL As String = "spu_Renewal_GetPreviousVersion"

    Public Const kGetAgentDetailsName As String = "Returns the Lead Agent details for the specified insurance_file_cnt"
    Public Const kGetAgentDetailsSQL As String = "spu_Get_Agent"

    Public Const ACCheckJobBatchRenewalInProcessStored As Boolean = True
    Public Const ACCheckJobBatchRenewalInProcessName As String = "CheckJobBatchRenewalInProcess"
    Public Const ACCheckJobBatchRenewalInProcessSQL As String = "spu_SIRRen_CheckJobBatchRenewalInProcess"

    Public Const ACUpdateCommonRenewalDateStored As Boolean = True
    Public Const ACUpdateCommonRenewalDateName As String = "Update Common Renewal Date"
    Public Const ACUpdateCommonRenewalDateSQL As String = "spu_update_agent_common_renewal_date"

    Public Const ACUpdateRenewalExceptionStored As Boolean = True
    Public Const ACUpdateRenewalExceptionName As String = "Update Renewal Exception Reason"
    Public Const ACUpdateRenewalExceptionSQL As String = "spu_Update_Renewal_Exception"

    Public Const ACGetandValidateSubAgentDetailStored As Boolean = True
    Public Const ACGetandValidateSubAgentDetailName As String = "Get Sub Agent Detail"
    Public Const ACGetandValidateSubAgentDetailSQL As String = "spu_Get_Sub_Agent_Detail"

    Public Const ACGetSubAgentDetailsStored As Boolean = True
    Public Const ACGetSubAgentDetailsName As String = "Get Sub Agent Detail Via Insurance File"
    Public Const ACGetSubAgentDetailsSQL As String = "spu_Select_SubAgents"

    Public Const kGetMTAQuotePolicyVersionsName As String = "GETMTAQuotePolicyVersions"
    Public Const kGetMTAQuotePolicyVersionsSQL As String = "spu_SIR_Get_MTAQuotePolicyVersions"

    Public Const ACAddChaseCycleItemInsuranceFileStored As Boolean = True
    Public Const ACAddChaseCycleItemInsuranceFileName As String = "Add_Chase_Cycle_Item_InsFile"
    Public Const ACAddChaseCycleItemInsuranceFileSQL As String = "spu_SIR_Add_Chase_Cycle_Item_InsFile"
    'Ashwani - (RFC_Enable_PrePayment_functionality)
    Public Const kGetPrepaymentOPtionVal As String = "Get Pre Payment Value"
    Public Const kGetPrepaymentOPtionValSQL As String = "spu_Get_PrePaymentOptionValue"

    Public Const ACSelDoctTemplateTypeIDStored As Boolean = False
    Public Const ACSelDoctTemplateTypeIDName As String = "GetDocumentTemplateTypeID"
    Public Const ACSelDoctTemplateTypeIDSQL As String = "SELECT document_template_id, document_type_id FROM document_template" & Strings.ChrW(13) & Strings.ChrW(10) &
                                               "WHERE code = {code}" & Strings.ChrW(13) & Strings.ChrW(10)

    Public Const ACSelBranchEmailContactStored As Boolean = False
    Public Const ACSelBranchEmailContactName As String = "Get Branch Email Address"
    Public Const ACSelBranchEmailContactSQL As String = "select email  from Source where source_id = {source_id}"

    Public Const KSelectSinglePFForInsStored As Boolean = True
    Public Const KSelectSinglePFForInsName As String = "PFSelectSingle"
    Public Const KSelectSinglePFForInsSQL As String = "spe_PFPremiumFinance_sel_single"

    Public Const kSelCreditCardDetailsStored As Boolean = True
    Public Const kSelCreditCardDetailsName As String = "SelCreditCardDetails"
    Public Const kSelCreditCardDetailsSQL As String = "spu_GetCreditCardDetails"

    Public Const kGetPartyCorrospondanceAddressCntStored As Boolean = True
    Public Const kGetPartyCorrospondanceAddressCntName As String = "Get_party_corrospondance_address_cnt"
    Public Const kGetPartyCorrospondanceAddressCntSQL As String = "spu_Get_party_corrospondance_address_cnt"

    Public Const kAddressSelStored As Boolean = True
    Public Const kAddressSelName As String = "Address_sel"
    Public Const kAddressSelSQL As String = "spe_Address_sel"

    Public Const ACGetUnderRenewalPolicyVersionsStored As Boolean = True
    Public Const ACGetUnderRenewalPolicyVersionsName As String = "Get Under Renewal Policy Versions"
    Public Const ACGetUnderRenewalPolicyVersionsSQL As String = "spu_get_underrenewal_policy_versions"

End Module