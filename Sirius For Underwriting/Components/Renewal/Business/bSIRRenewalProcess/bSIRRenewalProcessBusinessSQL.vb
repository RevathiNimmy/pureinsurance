Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Module BusinessSQL

    Public Const ACUpdateRenewalStatusTypeIDStored As Boolean = True
    Public Const ACUpdateRenewalStatusTypeIDName As String = "Update Renewal Status"
    Public Const ACUpdateRenewalStatusTypeIDSQL As String = "spu_SetRenewalStatusTypeID"

    Public Const ACIsQuotedStored As Boolean = True
    Public Const ACIsQuotedName As String = "Is Quoted"
    Public Const ACIsQuotedSQL As String = "spu_Is_Quoted"

    Public Const ACSelIsInstalmentStored As Boolean = True
    Public Const ACSelIsInstalmentName As String = "Is Instalments"
    Public Const ACSelIsInstalmentSQL As String = "spu_Is_Instalment"

    Public Const ACUpdRenewalCountStored As Boolean = True
    Public Const ACUpdRenewalCountName As String = "Update Renewal Count"
    Public Const ACUpdRenewalCountSQL As String = "spu_Update_Renewal_Count"

    Public Const ACRollBackPolicyToPreviousStateStored As Boolean = True
    Public Const ACRollBackPolicyToPreviousStateName As String = "Update Policy to Previous State"
    Public Const ACRollBackPolicyToPreviousStateSQL As String = "spu_Rollback_Policy"

    Public Const ACSelRenewalStored As Boolean = True
    Public Const ACSelRenewalName As String = "get renewals"
    Public Const ACSelRenewalSQL As String = "spu_GetRenewalPolicy"

    Public Const ACSelRenewalInviteListStored As Boolean = True
    Public Const ACSelRenewalInviteListName As String = "Get renewal invite list"
    Public Const ACSelRenewalInviteListSQL As String = "spu_GetRenewalInviteList"

    Public Const ACAddLastPrintRunStored As Boolean = True
    Public Const ACAddLastPrintRunName As String = "AddLastPrintRun"
    Public Const ACAddLastPrintRunSQL As String = "spe_Last_Print_Run_add"

    Public Const ACGetPolicyRenewalStatusStored As Boolean = True
    Public Const ACGetPolicyRenewalStatusName As String = "Get Policy Renewal Status"
    Public Const ACGetPolicyRenewalStatusSQL As String = "spu_GetPolicyRenewalStatus"

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

    Public Const ACUpdateLapsePolicyStored As Boolean = False
    Public Const ACUpdateLapsePolicyName As String = "Lapse policy"
    Public Const ACUpdateLapsePolicySQL As String = "UPDATE Insurance_File SET insurance_file_status_id = 2 WHERE insurance_folder_cnt = {InsuranceFolderCnt} AND (insurance_file_type_id IN (2, 3, 5, 6, 8, 9) OR insurance_file_status_id IS Null)"

    Public Const ACDelAgentCommissionStored As Boolean = False
    Public Const ACDelAgentCommissionName As String = "Delete AgentCommission"
    Public Const ACDelAgentCommissionSQL As String = "Delete Agent_Commission" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "FROM Insurance_File ifi LEFT JOIN Agent_Commission ac on ifi.insurance_file_cnt = ac.insurance_file_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "WHERE ifi.insurance_file_cnt = {InsuranceFileCnt}" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                     "AND IsNull(ifi.lead_agent_cnt,0) = 0"

    Public Const kValidateAcceptTMPIsValidActionName As String = "returns the insurance file cnt of the latest version of the policy that is also the final version of the previous true monthly policy cycle"
    Public Const kValidateAcceptTMPIsValidActionSQL As String = "spu_SIR_Get_Is_Accept_TMP_Valid_Action"


    'Get Branch Agents
    Public Const ACGetBranchAgentsStored As Boolean = True
    Public Const ACGetBranchAgentsName As String = "Get Branch Agents"
    'developer guide no. 39
    Public Const ACGetBranchAgentsSQL As String = "spu_Get_BranchAgents"

    Public Const ACSelectPolicyGrossTotalStored As Boolean = True
    Public Const ACSelectPolicyGrossTotalName As String = "GetPolicyGrossTotal"
    Public Const ACSelectPolicyGrossTotalSQL As String = "spu_Get_GrossTotal"


    'SQL Statements

    Public Const ACSelectAgentCodeForInsuranceFileCntStored As Boolean = False
    Public Const ACSelectAgentCodeForInsuranceFileCntName As String = "SelectAgentCode"
    Public Const ACSelectAgentCodeForInsuranceFileCntSQL As String = "Select pat.Code , " &
                                                                     "is_float_balance_account,is_overdraft_account, " &
                                                                     "float_balance_limit , overDraft_limit ,pa.Overdraft_expiry FROM " &
                                                                     "Insurance_file ifl LEFT JOIN party_agent pa ON " &
                                                                     "ifl.lead_agent_cnt=pa.party_cnt LEFT JOIN " &
                                                                     "Party_agent_type pat ON pa.party_agent_type_id=pat.party_agent_type_id " &
                                                                     "Where insurance_file_cnt = {InsuranceFileCnt}"

    Public Const ACGetProductPrintOptionsName As String = "GetProductPrintOptions"
    Public Const ACGetProductPrintOptionsSQL As String = "select produce_schedule,produce_certificate,produce_debit_note from product where product_id={Product_id}"

    Public Const ACDeleteCreditControlItemStored As Boolean = True
    Public Const ACDeleteCreditControlItemName As String = "DeleteCreditControlItem"
    Public Const ACDeleteCreditControlItemSQL As String = "spu_ACT_Del_Credit_Control_Item_InsFile"

    Public Const ACAddCreditControlItemInsFileStored As Boolean = True
    Public Const ACAddCreditControlItemInsFileName As String = "AddCreditControlItemInsFile"
    Public Const ACAddCreditControlItemInsFileSQL As String = "spu_ACT_Add_Credit_Control_Item_InsFile"
    'StartWritten Status.doc
    Public Const ACGetWrittenStatusUsedStored As Boolean = True
    Public Const ACGetWrittenStatusUsedName As String = "GetWrittenStatusUsed"
    Public Const ACGetWrittenStatusUsedSQL As String = "spu_Get_Written_Status_Used"
    'End - Written Status.doc
    'PN4538-Start
    Public Const ACUpdateCommonRenewalDateStored As Boolean = True
    Public Const ACUpdateCommonRenewalDateName As String = "Update Common Renewal Date"
    Public Const ACUpdateCommonRenewalDateSQL As String = "spu_update_agent_common_renewal_date"
    'PN4538-End
    Public Const ACAddChaseCycleItemInsuranceFileStored As Boolean = True
    Public Const ACAddChaseCycleItemInsuranceFileName As String = "Add_Chase_Cycle_Item_InsFile"
    Public Const ACAddChaseCycleItemInsuranceFileSQL As String = "spu_SIR_Add_Chase_Cycle_Item_InsFile"
    Public Const kDeletepolicyAssociatesStored As Boolean = True
    Public Const kDeletepolicyAssociatesFileName As String = "DeletePolicyAssociates"
    Public Const kDeletepolicyAssociatesSQL As String = "spu_DeleteRenewalPolicyAssociates"



    Public Const ACGetTransAccountIdForInsuranceFileCntName As String = "GetTransAccountId"
    Public Const ACGetTransAccountIdForInsuranceFileCntSQL As String = "Select account_id from TransDetail where Insurance_ref in " &
                                                                     "( select Insurance_ref from Insurance_File where insurance_file_cnt={InsuranceFileCnt}) and spare='GROSS'"


    'Ashwani - (RFC_Enable_PrePayment_functionality)
    Public Const kGetPrepaymentOPtionVal As String = "Get Pre Payment Value"
    Public Const kGetPrepaymentOPtionValSQL As String = "spu_Get_PrePaymentOptionValue"

    Public Const kGetAnnivPriorVersionInsFileCntName As String = "GetAnnivPriorVersionInsFileCnt"
    Public Const kGetAnnivPriorVersionInsFileCntSQL As String = "spu_SIR_GetAnnivPriorVersionInsFileCnt"

    Public Const ACGetRenewalPolicyDetailsStored As Boolean = True
    Public Const ACGetRenewalPolicyDetailsName As String = "GetRenewalPolicyDetails"
    Public Const ACGetRenewalPolicyDetailsSQL As String = "spu_SIR_Get_Renewal_Policy_Details"

    Public Const ACUpdRenewalPolicyDetailsStored As Boolean = True
    Public Const ACUpdRenewalPolicyDetailsName As String = "LapsePolicy"
    Public Const ACUpdRenewalPolicyDetailsSQL As String = "spu_insurance_file_update"

    Public Const KSelectSinglePFForInsStored As Boolean = True
    Public Const KSelectSinglePFForInsName As String = "PFSelectSingle"
    Public Const KSelectSinglePFForInsSQL As String = "spe_PFPremiumFinance_sel_single"
End Module