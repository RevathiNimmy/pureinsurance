Option Strict Off
Option Explicit On
Module BusinessSQL
    ' ***************************************************************** '
    ' Class Name: BusinessSQL
    ' Description: Contains the SQL Statements required by the
    '              bSIRPremiumFinance.Business class.
    ' Edit History:
    ' ***************************************************************** '

    'SQL Statements

    ' Select All PFPRemiumFinanceDetails SQL
    Public Const ACGetPFPremiumFinanceDetailsStored As Boolean = True
    Public Const ACGetPFPremiumFinanceDetailsName As String = "SelectPFPremiumFinance"
    Public Const ACGetPFPremiumFinanceDetailsSQL As String = "spu_PFPremiumFinance_Find"

    ' Select All PFFinancePlanTransactions SQL
    Public Const ACGetPFFinancePlanTransactionsStored As Boolean = True
    Public Const ACGetPFFinancePlanTransactionsName As String = "SelectPFTransactions"
    Public Const ACGetPFFinancePlanTransactionsSQL As String = "spe_PFTransaction_Id_sel"

    ' Select All PFFinancePlanTransactions SQL
    Public Const ACGetSGPFFinancePlanTransactionsStored As Boolean = True
    Public Const ACGetSGPFFinancePlanTransactionsName As String = "SelectPFTransactions"
    Public Const ACGetSGPFFinancePlanTransactionsSQL As String = "spu_pf_getsgtransactions"

    ' Select All PFPRemiumFinanceVersions SQL
    Public Const ACGetPFPremiumFinanceVersionsStored As Boolean = True
    Public Const ACGetPFPremiumFinanceVersionsName As String = "SelectPFPremiumFinanceVersions"
    Public Const ACGetPFPremiumFinanceVersionsSQL As String = "spe_PFPremiumFinanceversions_select"

    ' Check Transactions against PlansSQL
    Public Const ACCheckFinancedTransactionsStored As Boolean = True
    Public Const ACCheckFinancedTransactionsName As String = "CheckPFTransactions"
    Public Const ACCheckFinancedTransactionsSQL As String = "spe_PFTransaction_check"

    ' Check Scheme for in-house or 3rd party status
    Public Const ACCheckIsInHouseStored As Boolean = True
    Public Const ACCheckIsInHouseName As String = "CheckInHouse"
    Public Const ACCheckIsInHouseSQL As String = "spe_PFPremiumFinance_inhouse"

    'CheckSchemeType replaces CheckIsInHouse
    Public Const ACCheckSchemeTypeStored As Boolean = True
    Public Const ACCheckSchemeTypeName As String = "CheckSchemeType"
    Public Const ACCheckSchemeTypeSQL As String = "spu_PFCheckSchemeType"

    'Start (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.3.1)
    Public Const ACGetAgentCommissionTypeStored As Boolean = True
    Public Const ACGetAgentCommissionTypeName As String = "GetAgentCommission"

    Public Const ACGetAgentCommissionTypeSQL As String = "spu_sir_agent_commission_sel"
    'End (Sriram P)Tech Spec - Valiant - P16 - Broker Instalments.doc section(5.3.1)

    ' Add new premium finance record
    'PN12594 Add extra parameter
    Public Const ACAddNewPFPremiumFinanceStored As Boolean = True
    Public Const ACAddNewPFPremiumFinanceName As String = "AddNewPFPremiumFinance"
    Public Const ACAddNewPFPremiumFinanceSQL As String = "spu_PFPremiumFinance_addnew"

    'PN12977 Get Maximum Plan Version
    Public Const ACGetPFMaxVersionStored As Boolean = True
    Public Const ACGetPFMaxVersionName As String = "GetPFMaxVesion"
    Public Const ACGetPFMaxVersionSQL As String = "spu_PFGetMaxVersion"

    Public Const ACAddNewPFPremiumFinanceVersionStored As Boolean = True
    Public Const ACAddNewPFPremiumFinanceVersionName As String = "AddNewPFPremiumFinance"
    Public Const ACAddNewPFPremiumFinanceversionSQL As String = "spu_PFPremiumFinance_addnewversion"

    Public Const ACPFPremiumFinanceUpdateStatusStored As Boolean = True
    Public Const ACPFPremiumFinanceUpdateStatusName As String = "PFPremiumFinanceUpdateStatus"
    Public Const ACPFPremiumFinanceUpdateStatusSQL As String = "spu_PFPremiumFinance_UpdateStatus"

    ' Update premium finance record
    'PN12594 Add extra parameter
    Public Const ACUpdatePFPremiumFinanceStored As Boolean = True
    Public Const ACUpdatePFPremiumFinanceName As String = "UpdatePFPremiumFinance"
    Public Const ACUpdatePFPremiumFinanceSQL As String = "spu_PFPremiumFinance_update"

    '(RC) PLICO 9-10
    ' Update the batch_id on the PFPremiumFinance record
    Public Const ACUpdatePFPremiumFinanceBatchIDStored As Boolean = True
    Public Const ACUpdatePFPremiumFinanceBatchIDName As String = "UpdatePFPremiumFinanceBatchID"
    Public Const ACUpdatePFPremiumFinanceBatchIDSQL As String = "spu_PFPremiumFinance_UpdateBatchID"

    ' Select All PFPRemiumFinanceDetails SQL
    Public Const ACDeletePFPremiumFinanceStored As Boolean = True
    Public Const ACDeletePFPremiumFinanceName As String = "DeletePFPremiumFinance"
    Public Const ACDeletePFPremiumFinanceSQL As String = "spe_PFPremiumFinance_delete"

    'get Scheme Details
    Public Const ACGetSchemeDetailsStored As Boolean = True
    Public Const ACGetSchemeDetailsName As String = "Get Scheme Details"
    Public Const ACGetSchemeDetailsSQL As String = "spe_GetSchemeDetails"

    'Get merge fields for word template
    Public Const ACGetMergeFieldsStored As Boolean = True
    Public Const ACGetMergeFieldsName As String = "Get Merge Fields"
    Public Const ACGetMergeFieldsSQL As String = "spu_GetMergeFields"

    'Check PFScheme if valid for source
    Public Const ACCheckPFSchemeSourceStored As Boolean = True
    Public Const ACCheckPFSchemeSourceName As String = "Check PFScheme Source"
    Public Const ACCheckPFSchemeSourceSQL As String = "spe_PFScheme_checksource"

    'Check PFScheme if valid for product
    Public Const ACCheckPFSchemeProductStored As Boolean = True
    Public Const ACCheckPFSchemeProductName As String = "Check PFScheme Product"
    Public Const ACCheckPFSchemeProductSQL As String = "spe_PFScheme_checkproduct"

    'Select Plans for Merging
    Public Const ACGetPFPremiumFinanceMergesStored As Boolean = True
    Public Const ACGetPFPremiumFinanceMergesName As String = "Select Plans for Merging"
    Public Const ACGetPFPremiumFinanceMergesSQL As String = "spe_PFPremiumFinance_merges"

    'Select Settlement
    Public Const ACGetPFPremiumFinanceSettleStored As Boolean = True
    Public Const ACGetPFPremiumFinanceSettleName As String = "Select Plan settlement"
    'PSL 18/03/2003 Issue 2993 Added Parameter
    ' RAW 05/11/2003 : CQ2912, 2976 : added 3 extra params
    ' RAW 13/11/2003 : CQ1765 : added extra param
    Public Const ACGetPFPremiumFinanceSettleSQL As String = "spu_PFPremiumFinance_settlement"

    'Thinh Nguyen 03/12/2001 (start)
    Public Const ACGetInsuranceFileCntStored As Boolean = True
    Public Const ACGetInsuranceFileCntName As String = "Get Insurance File Count"
    Public Const ACGetInsuranceFileCntSQL As String = "spu_GetInsuranceFileCnt"

    Public Const ACGetPolicyIDStored As Boolean = True
    Public Const ACGetPolicyIDName As String = "Get Policy IDs"
    Public Const ACGetPolicyIDSQL As String = "spu_GetPolicyID"
    'Thinh Nguyen 03/12/2001 (end)

    'Thinh Nguyen 18/02/2002 (start)
    'Get lead agent
    Public Const ACGetAgentAndClientStored As Boolean = True
    Public Const ACGetAgentAndClientName As String = "Get lead agent and client"
    Public Const ACGetAgentAndClientSQL As String = "spu_GetAgentAndClient"

    Public Const ACGetHiddenOptionStored As Boolean = True
    Public Const ACGetHiddenOptionName As String = "Get Hidden Option"
    Public Const ACGetHiddenOptionSQL As String = "spu_GetHiddenOption"

    Public Const ACGetLeadAgentStored As Boolean = True
    Public Const ACGetLeadAgentName As String = "get lead agent for policy"
    Public Const ACGetLeadAgentSQL As String = "spu_GetLeadAgentForPolicy"

    Public Const ACUpdatePlanTransIDStored As Boolean = True
    Public Const ACUpdatePlanTransIDName As String = "Update Plan Transaction ID"
    Public Const ACUpdatePlanTransIDSQL As String = "spu_SetPlanTransID"
    'Thinh Nguyen 18/02/2002 (end)

    'Thinh Nguyen 01/05/2002 (start)
    Public Const ACGetIPTValueStored As Boolean = True
    Public Const ACGetIPTValueName As String = "Get IPT Value"
    Public Const ACGetIPTValueSQL As String = "spu_GetIPTValue"
    'Thinh Nguyen 01/05/2002 (end)

    ' PN 74070 Tariq Rashid
    Public Const ACGetPreviousSchemeFromInsuranceFileCntStored As Boolean = True
    Public Const ACGetPreviousSchemeFromInsuranceFileCntName As String = "Get Previous Scheme From Insurance File Cnt"
    Public Const ACGetPreviousSchemeFromInsuranceFileCntSQL As String = "spu_Get_Previous_Scheme_Version_From_InsuranceFile"

    'PN 74894
    Public Const ACUpdateInstalmentStatusForPlanDuringCancellationStored As Boolean = True
    Public Const ACUpdateInstalmentStatusForPlanDuringCancellationSQL As String = "spu_ACT_PFInstalments_Status_Update_For_Plan_On_Cancellation"
    Public Const ACUpdateInstalmentStatusForPlanDuringCancellationName As String = "updates the status of instalments to Failed to the specified pfinstalments_status during cancellation"

    '=========================================================================
    'TR - TS23 Using Existing Stored Procedure
    '=========================================================================
    'TR - Constants for Stored Procedure spu_PFPremiumFinance_sel_single (was spe_)
    Public Const ACGetPFPremiumFinanceSingleStored As Boolean = True
    Public Const ACGetPFPremiumFinanceSingleName As String = "SelectPFPremiumFinanceSingle"
    Public Const ACGetPFPremiumFinanceSingleSQL As String = "spu_PFPremiumFinance_Sel_Single"

    'TR - Constants for Stored Procedure spu_PFInstalments_saa
    Public Const ACGetPFInstalmentsStored As Boolean = True
    Public Const ACGetPFInstalmentsName As String = "SelectAllPfInstalments"
    Public Const ACGetPFInstalmentsSQL As String = "spu_PFInstalments_saa"

    '=========================================================================
    'TR - TS23 Added Support for New Stored Procedures 06/11/02 (start)
    '=========================================================================
    'TR - Constants for Stored Procedure spu_PFRFValidDates_Sel
    'Selects all PFRF records for ProductCode/QuoteDate combination
    Public Const ACGetValidPFRateFilesStored As Boolean = True
    Public Const ACGetValidPFRateFilesName As String = "Get Valid Rate Files"
    Public Const ACGetValidPFRateFilesSQL As String = "spu_PFRFValidDates_Sel_All"

    'TR - Constants for Stored Procedure spu_PFSchemeProducts_Sel
    'Selects all Scheme Product records for a PFRF
    Public Const ACGetSchemeProductsStored As Boolean = True
    Public Const ACGetSchemeProductsName As String = "Get Scheme Products"
    Public Const ACGetSchemeProductsSQL As String = "spu_PFSchemeProducts_Sel"

    'TR - Constants for Stored Procedure spu_PFSchemeSource_Sel
    'Selects all Scheme Source records for a PFRF
    Public Const ACGetSchemeSourcesStored As Boolean = True
    Public Const ACGetSchemeSourcesName As String = "Get Scheme Sources"
    Public Const ACGetSchemeSourcesSQL As String = "spu_PFSchemeSource_Sel"

    'TR - Constants for Stored Procedure spu_PFMediaTypes_Sel
    'Selects all Media Types
    Public Const ACGetMediaTypesStored As Boolean = True
    Public Const ACGetMediaTypesName As String = "Get Media Types"
    Public Const ACGetMediaTypesSQL As String = "spu_PFMediaTypes_Sel"

    'TR - Constants for Stored Procedure spu_PFRFs_Sel
    'Selects single PFRF
    Public Const ACGetPFRFSingleStored As Boolean = True
    Public Const ACGetPFRFSingleName As String = "Get Single PFRF"
    Public Const ACGetPFRFSingleSQL As String = "spu_PFRF_Sel_sin"

    ' Check Scheme for in-house or 3rd party status
    Public Const ACInstalmentsSettleStored As Boolean = True
    Public Const ACInstalmentsSettleName As String = "PFInstalments_Settle"
    Public Const ACInstalmentsSettleSQL As String = "spu_PFInstalments_Settle"

    ' Select All Transactions for a given PF key
    Public Const ACGetPFGetTransactionsStored As Boolean = True
    Public Const ACGetPFGetTransactionsName As String = "SelectPFGetTransactions"
    Public Const ACGetPFGetTransactionsSQL As String = "spu_PFGetTransactions"

    ' Select PFScheme SQL
    Public Const ACSelectSingleSchemeStored As Boolean = True
    Public Const ACSelectSingleSchemeName As String = "SelectSinglePFScheme"
    Public Const ACSelectSingleSchemeSQL As String = "spu_PFScheme_sel"

    'TR - Constants for Stored Procedure spu_PFPremiumFinance_sel_single using InsuranceFileCnt
    Public Const ACGetPFGetPfFromInsuranceSingleStored As Boolean = True
    Public Const ACGetPFGetPfFromInsuranceSingleName As String = "SelectPFPremiumFinanceSingle"
    Public Const ACGetPFGetPfFromInsuranceSingleSQL As String = "spu_PFPremiumFinance_Sel_SingleFromInsuranceFileCount"

    'TR - Constants for Stored Procedure spe_Insurance_File_sel to get an Insurance File Record
    Public Const ACGetInsuranceFileStored As Boolean = True
    Public Const ACGetInsuranceFileName As String = "Get Insurance File"
    Public Const ACGetInsuranceFileSQL As String = "spe_Insurance_File_sel"

    ' Alix Bergeret - 24/03/2003 - Delete all finance plans attached to one insurance file
    Public Const ACDeletePlanForOneInsFileStored As Boolean = True
    Public Const ACDeletePlanForOneInsFileName As String = "Delete Plans for one insurance file"
    Public Const ACDeletePlanForOneInsFileSQL As String = "spu_DeletePlanForOneInsFile"

    'TR - Constants for Stored Procedure spu_MarkPlanAsDeleted to mark an instalment plan as deleted
    Public Const KMarkPlanAsDeletedFileStored As Boolean = True
    Public Const KMarkPlanAsDeletedFileName As String = "Mark Plan for insurance file as deleted"
    Public Const KMarkPlanAsDeletedFileSQL As String = "spu_MarkPlanAsDeleted"

    'TR - Constants for Stored Procedure spu_MarkDeletedPlanAsSaved to mark an instalment plan as saved
    Public Const KMarkPlanAsSavedFileStored As Boolean = True
    Public Const KMarkPlanAsSavedFileName As String = "Mark Plan for insurance file as saved"
    Public Const KMarkPlanAsSavedFileSQL As String = "spu_MarkPlanAsSaved"

    'TR - Constants for Stored Procedure spe_Insurance_File_sel to get an Insurance File Record
    Public Const ACGetAllFrequenciesStored As Boolean = True
    Public Const ACGetAllFrequenciesName As String = "Get All Frequencies"
    Public Const ACGetAllFrequenciesSQL As String = "spu_PFFrequency_Sel_All"

    'TR - Constants for Stored Procedure spe_Insurance_File_sel to get an Insurance File Record
    Public Const ACGetPartyTypeCodeStored As Boolean = True
    Public Const ACGetPartyTypeCodeName As String = "Get Paty Type Code"
    Public Const ACGetPartyTypeCodeSQL As String = "spu_party_type_code_sel"

    ' Alix Bergeret - 02/04/2003 - Get a valid rate for the given parameters
    Public Const ACGetValidRateStored As Boolean = True
    Public Const ACGetValidRateName As String = "Get a valid rate"
    Public Const ACGetValidRateSQL As String = "spu_get_valid_pfrf"

    ' Alix Bergeret - 10/04/2003 - Get arrears for a specific plan
    Public Const ACGetArrearsStored As Boolean = True
    Public Const ACGetArrearsName As String = "Get a arrears for a plan"
    Public Const ACGetArrearsSQL As String = "spu_PFPremiumFinance_Arrears"

    ' Alix Bergeret - 15/04/2003 - Returns all media type and for each of them, the available frequencies.
    Public Const ACGetMediaTypeFrequStored As Boolean = True
    Public Const ACGetMediaTypeFrequName As String = "Get media types and frequencies"
    Public Const ACGetMediaTypeFrequSQL As String = "spu_get_mediatype_frequencies"

    ' Alix Bergeret - 15/04/2003 - Load a finance plan from its claim debt ID
    Public Const ACGetPfFromClaimDebtIDStored As Boolean = True
    Public Const ACGetPfFromClaimDebtIDName As String = "Load a plan from claim debt ID"
    Public Const ACGetPfFromClaimDebtIDSQL As String = "spu_PFPremiumFinance_sel_single_rec"

    'TR - Constants for Stored Procedure spu_GetPlanForInsuranceFolderVersion
    Public Const ACGetPFPlanForInsFolderAndVersionStored As Boolean = True
    Public Const ACGetPFPlanForInsFolderAndVersionName As String = "SelectPFPremiumFinanceForInsFolderAndVer"
    Public Const ACGetPFPlanForInsFolderAndVersionSQL As String = "spu_GetPlanForPreviousPolicyVersion"

    'AAB - 03/24/2003 - To support 1.9 Instalment integration
    ' Add ExportFolder SQL
    Public Const ACAddExportFolderStored As Boolean = True
    Public Const ACAddExportFolderName As String = "AddExportFolder"
    Public Const ACAddExportFolderSQL As String = "spu_add_trans_export_folder"

    ''PN 71095
    Public Const ACUpdatePaymentMethodStored As Boolean = False
    Public Const ACUpdatePaymentMethodName As String = "Update Payment Method to Invoice if Plan Status is set to UPDATED"
    Public Const ACUpdatePaymentMethodSQL As String = "UPDATE insurance_file SET payment_method='Invoice' " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                      "WHERE insurance_file_cnt = {InsuranceFileCnt}"
    ''PN 71095

    ' Add TransDetailDeposit SQL
    Public Const ACAddTransDetailsDepositStored As Boolean = True
    Public Const ACAddTransDetailsDepositName As String = "Add_trans_Details_Deposit"
    Public Const ACAddTransDetailsDepositSQL As String = "spu_Add_trans_Details_Deposit"

    ' Add MakeLiveAfterStats SQL
    Public Const ACMakeLiveAfterStatsStored As Boolean = True
    Public Const ACMakeLiveAfterStatsName As String = "spu_PFPremiumFinance_MakeLiveAfterStats"
    Public Const ACMakeLiveAfterStatsSQL As String = "spu_PFPremiumFinance_MakeLiveAfterStats"

    Public Const ACGetExportFolderCntStored As Boolean = True
    Public Const ACGetExportFolderCntName As String = "Get_Exprot_Folder_Cnt"
    Public Const ACGetExportFolderCntSQL As String = "spu_Get_Export_Folder_Cnt"

    Public Const ACGetDocumentRefStored As Boolean = True
    Public Const ACGetDocumentRefName As String = "Get_Document_Ref"
    Public Const ACGetDocumentRefSQL As String = "spu_Get_Document_Ref"

    Public Const ACAddCreditControlItemInstalmentsInsuranceFileStored As Boolean = True
    Public Const ACAddCreditControlItemInstalmentsInsuranceFileName As String = "spu_ACT_Add_Credit_Control_Item_Instalments_InsFile"
    Public Const ACAddCreditControlItemInstalmentsInsuranceFileSQL As String = "spu_ACT_Add_Credit_Control_Item_Instalments_InsFile"

    Public Const ACDelCreditControlItemInsuranceFileStored As Boolean = True
    Public Const ACDelCreditControlItemInsuranceFileName As String = "Del_Credit_Control_Item_InsFile"
    Public Const ACDelCreditControlItemInsuranceFileSQL As String = "spu_ACT_Del_Credit_Control_Item_InsFile"

    Public Const ACGetNextOrionDocRefStored As Boolean = True
    Public Const ACGetNextOrionDocRefName As String = "ACGetNextOrionDocRef"
    Public Const ACGetNextOrionDocRefSQL As String = "spu_SIR_Get_NextOrionDocRef"

    Public Const ACCheckIfInstalmentDepositRequiredStored As Boolean = True
    Public Const ACCheckIfInstalmentDepositRequiredName As String = "ACGetNextOrionDocRef"
    Public Const ACCheckIfInstalmentDepositRequiredSQL As String = "spu_SIR_CheckInstalmentDepositRequired"
    'End changes - AAB = 03/24/2003

    'Kevin Renshaw (CMG) 11/04/2003 (start)
    Public Const ACGetInstalmentsRemainingStored As Boolean = True
    Public Const ACGetInstalmentsRemainingName As String = "Get Instalments Remaining"
    Public Const ACGetInstalmentsRemainingSQL As String = "spu_Get_Instalments_Remaining"
    'Kevin Renshaw (CMG) 11/04/2003 (end)

    Public Const ACGetUnCollectedInstalmentsStored As Boolean = True
    Public Const ACGetUnCollectedInstalmentsName As String = "Get UnCollected Instalments"
    Public Const ACGetUnCollectedInstalmentsSQL As String = "spu_Get_UnCollected_Instalments"

    'DD 06/06/2003: Added for Premium Finance
    Public Const ACGetAccountIDFromPartyCntStored As Boolean = True
    Public Const ACGetAccountIDFromPartyCntName As String = "Get Account_id from PartyCnt"
    'PN6169 Pass new company_id parameter
    Public Const ACGetAccountIDFromPartyCntSQL As String = "spu_ACT_GetAccountFromPartyCnt"

    'Sw 01/07/2003 Added this for Accounting For ISF spec (1.9 IAG)
    Public Const ACGetStatsFolderCntName As String = "GetStatsFolderCnt"
    Public Const ACGetStatsFolderCntSQL As String = "spu_ACT_Get_StatsFolderCnt"

    'Tracy Richards 15/07/03 - Added support for Broker Agent Instalments
    Public Const ACGetAgentAndTypeStored As Boolean = True
    Public Const ACGetAgentAndTypeName As String = "Get lead agent and type"
    Public Const ACGetAgentAndTypeSQL As String = "spu_GetAgentAndType"

    'Tracy Richards 17/07/03 - Changes status of Instalments from Hold to New
    Public Const ACReleasePFInstalmentsStored As Boolean = True
    Public Const ACReleasePFInstalmentsName As String = "Change Instalment Status from Hold to New"
    Public Const ACReleasePFInstalmentsSQL As String = "spu_PFReleaseInstalments"

    ' HG22072003 - Find Document Tempate
    Public Const ACGetDocumentTemplateStored As Boolean = True
    Public Const ACGetDocumentTemplateName As String = "Get Document Template"
    'SJ 29/06/2004 - start
    'Public Const ACGetDocumentTemplateSQL = "{Call spu_get_document_template_saa (?,?,?,?,?)}"
    Public Const ACGetDocumentTemplateSQL As String = "spu_get_document_template_saa"
    'SJ 29/06/2004 - end

    ' SMJB 02/09/03 - Get Insurance Media Ref
    Public Const ACGetMediaRefStored As Boolean = True
    Public Const ACGetMediaRefName As String = "Get Insurance Media ref"
    Public Const ACGetMediaRefSQL As String = "spu_Get_Insurance_Ref"

    ' Get ICCS
    Public Const ACGetICCSStored As Boolean = True
    Public Const ACGetICCSName As String = "GetICCS"
    Public Const ACGETICCSSQL As String = "spu_pm_iccs"

    'SMJB CQ906 30/09/03 - Load existing plan details
    Public Const ACGetExisitingPlanDetailsSQL As String = "spu_PFPremiumFinance_Get_Exisiting_Plan_Details"
    Public Const ACGetExisitingPlanDetailsName As String = "Get Existing Plan Details"
    Public Const ACGetExisitingPlanDetailsStored As Boolean = True

    'AAB-23-Oct-2003 09:22 - added to support check deposit
    Public Const ACCheckDepositStored As Boolean = True
    Public Const ACCheckDepositName As String = "Get the deposit amount and deposit as instalment flag for given PFCnt & Version"
    Public Const ACCheckDepositSQL As String = "spu_PFGetDepositInfo"

    '*********
    ' MEvans : 30-10-2003 : 227
    Public Const ACGetAssociatedClaimDetailsName As String = "Returns the claim debt id that is associated with this finance plan "
    Public Const ACGetAssociatedClaimDetailsSQL As String = "spu_PF_Get_Claim_Details"

    Public Const ACUpdateTaskOutcomeName As String = "Updates the outcome of the specified pmwrk_work_task_instance"
    Public Const ACUpdateTaskOutcomeSQL As String = "spu_pmwrk_task_inst_outcome_update"

    Public Const ACGetEventTaskName As String = "Returns the event task for the specified "
    Public Const ACGetEventTaskSQL As String = "spu_PF_Get_Event_Task"

    Public Const ACGetCreateEventKeysName As String = "Returns the keys required to create event task for the specified claim debt"
    Public Const ACGetCreateEventKeysSQL As String = "spu_PF_Get_Event_Task_Keys"
    '*********

    'Thinh Nguyen 01/02/2004
    Public Const ACGetPlanInsuranceFolderCntStored As Boolean = True
    Public Const ACGetPlanInsuranceFolderCntName As String = "Get plan insurance folder count"
    Public Const ACGetPlanInsuranceFolderCntSQL As String = "spu_GetInsuranceFolderCnt"

    'SET 16/08/2004 ISS14208 - PolicyFees SQL
    Public Const ACGetPolicyFeesSQL As String = "spu_PMB_Policy_Fees_sel"
    Public Const ACGetPolicyFeesName As String = "GetPolicyFees"
    Public Const ACGetPolicyFeesStored As Boolean = True

    Public Const ACSpreadFeesAmongInstalmentsSQL As String = "spu_SIR_Policy_Fees_Select"
    Public Const ACSpreadFeesAmongInstalmentsName As String = "SpreadFeeAcrossInstalments"

    Public Const ACStampDutySQL As String = "spu_get_StampDuty_info"
    Public Const ACStampDutyName As String = "GetStampDutyInfo"

    Public Const ACSpreadTaxAmongInstalmentsSQL As String = "SELECT include_tax_in_instalments,spread_tax_across_instalments,value,is_not_applied_to_client FROM Tax_Calculation WHERE insurance_file_cnt = {insurance_file_cnt} "
    Public Const ACSpreadTaxAmongInstalmentsName As String = "SpreadTaxAcrossInstalments"

    Public Const ACGetPolicyFeeTotalSQL As String = "SELECT" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "    ISNULL(SUM(CAST(ROUND(pf.fee_amount + ((ISNULL(ie.rate,0) * fee_amount) / 100), 2) AS MONEY)),0)" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "FROM policy_fee pf" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "LEFT JOIN ipt_extras ie" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "    ON ie.party_cnt = pf.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "    AND ie.effective_date =" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "        (" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "            SELECT" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "                MAX(effective_date)" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "            FROM ipt_extras" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "            WHERE party_cnt = ie.party_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "            AND effective_date <= GETDATE()" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "        )" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                 "WHERE pf.insurance_file_cnt = {insurance_file_cnt}"
    Public Const ACGetPolicyFeeTotalName As String = "GetPolicyFeeTotal"
    Public Const ACGetPolicyFeeTotalStored As Boolean = False

    'SET 16/08/2004 ISS14208 - IPT rates on Extras defined in Rule files
    Public Const ACGetIPTRateForAccountSQL As String = "spu_Extra_IPT_Rate_sel"
    Public Const ACGetIPTRateForAccountName As String = "GetIPTRatesForExtrasAccount"
    Public Const ACGetIPTRateForAccountStored As Boolean = True

    Public Const kDeleteCreditControlItemName As String = "Deletes the credit control item for the specified insurance file cnt"
    Public Const kDeleteCreditControlItemSQL As String = "spu_ACT_Del_Credit_Control_Item_InsFile"

    Public Const kAddCreditControlItemName As String = "Adds a Credit Control Item for the specified Insurance file cnt and business type"
    Public Const kAddCreditControlItemSQL As String = "spu_ACT_Add_CCItem_For_Cancelled_InstPlan"

    Public Const kGetRenewalAmountToFinanceName As String = "get the renewal amount to finance via instalments for underwriting"
    Public Const kGetRenewalAmountToFinanceSQL As String = "spu_SIR_Get_Renewal_Amount_To_Finance"

    'DC030106 PN26057 update bacs agreement with new bank details
    Public Const ACUpdateBACSInstalmentUpdateStatusSQL As String = "spu_PFInstalments_update_bacs_status"
    Public Const ACUpdateBACSInstalmentUpdateStatusName As String = "UpdateBACSInstalmentUpdateStatus"
    Public Const ACUpdateBACSInstalmentUpdateStatusStored As Boolean = True

    'DC030106 PN26049 to default to instalments if theres a plan
    Public Const ACGetLatestValidFinancePlanStored As Boolean = True
    Public Const ACGetLatestValidFinancePlanName As String = "GetLatestValidFinancePlan"
    Public Const ACGetLatestValidFinancePlanSQL As String = "spu_PFPremiumFinance_Sel_Latest_Valid_Plan"

    'DC190106 PN26849 update instalment as on hold
    Public Const ACUpdateInstalmentStatusToOnHoldSQL As String = "spu_PFInstalments_update_status_to_onhold"
    Public Const ACUpdateInstalmentStatusToOnHoldName As String = "UpdateBACSInstalmentUpdateStatus"
    Public Const ACUpdateInstalmentStatusToOnHoldStored As Boolean = True

    Public Const kGetMTAAdjustmentAmountName As String = "Returns the total MTA adjustment amount of the transactions allocated against the specififed finance plan / version "
    Public Const kGetMTAAdjustmentAmountSQL As String = "spu_PF_Get_Total_MTA_Amount"

    'DC210206 PN26057
    Public Const ACCheckFirstInstalmentCatersForBankChangesStored As Boolean = True
    Public Const ACCheckFirstInstalmentCatersForBankChangesName As String = "UpdatePFPremiumFinancePaymentDate"
    Public Const ACCheckFirstInstalmentCatersForBankChangesSQL As String = "spu_PFInstalments_update_new_instalment_date"

    'ACR 060606 Added for on screen policy details (SG plans only)
    Public Const ACGetSGPolicyDetailsSQL As String = "spu_PF_GetSGPolicyDetails"
    Public Const ACGetSGPolicyDetailsName As String = "GetSGPolicyDetails"

    Public Const ACInsertPFTransIDStored As Boolean = True
    Public Const ACInsertPFTransIDName As String = "InsertPFTransID"
    Public Const ACInsertPFTransIDSQL As String = "spu_PFTransaction_id_ins"

    Public Const ACCheckAllocationAgainstPolicyStored As Boolean = False
    Public Const ACCheckAllocationAgainstPolicyName As String = "CheckAllocationAgainstPolicy"
    Public Const ACCheckAllocationAgainstPolicySQL As String = "SELECT orig_Base_Amount , Alloc_Base_Amount  ,fully_matched FROM AllocationDetail " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                " WHERE transdetail_id IN " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                "    ( " &
                                                                "      SELECT transdetail_id FROM transdetail " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                "      WHERE document_id in (" & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                "        ( " &
                                                                "         SELECT document_id FROM document " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                "         WHERE insurance_file_cnt={insurance_file_cnt}) " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                "         ) " & Strings.ChrW(13) & Strings.ChrW(10) &
                                                                "    ) AND alloc_base_amount<>0 and cashlistitem_id is not Null"

    Public Const ACCheckIsTrueMonthlyPolicyStored As Boolean = True
    Public Const ACCheckIsTrueMonthlyPolicyName As String = "CheckIsTrueMonthlyPolicy"
    Public Const ACCheckIsTrueMonthlyPolicySQL As String = "spu_check_is_true_monthly_policy"

    Public Const ACGetTaxesNotIncludedInInstalmentStored As Boolean = True
    Public Const ACGetTaxesNotIncludedInInstalmentName As String = "GetTaxesNotIncludedInInstalment"
    Public Const ACGetTaxesNotIncludedInInstalmentSQL As String = "spu_SIR_Get_TaxNotIncludedInInstalment"
    Public Const kUpdateInstalmentStatusSQL As String = "spu_ACT_PFInstalment_Status_Update"
    Public Const kUpdateInstalmentStatusName As String = "update the pfinstalments_status_id on the pfinstalments record"

    Public Const kUpdateInstalmentDuedateSQL As String = "spu_ACT_PFInstalment_Details_Update"
    Public Const kUpdateInstalmentDuedateName As String = "update the pfinstalments_Duedate on the pfinstalments record"

    Public Const kGetPFMediaTypeHistorySQL As String = "spu_PFMediaTypeHistory_sel"
    Public Const kGetPFMediaTypeHistoryName As String = "return the history items for the specified plan and mediatype validation code"

    Public Const kUpdateInstalmentStatusForPlanSQL As String = "spu_ACT_PFInstalments_Status_Update_For_Plan"
    Public Const kUpdateInstalmentStatusForPlanName As String = "updates the status of all instalments on the specified plan to the specified pfinstalments_status"

    Public Const kSaveInstalmentsPlanMediaTypeDetailsSQL As String = "spu_PFMediaTypeHistory_add"
    Public Const kSaveInstalmentsPlanMediaTypeDetailsName As String = "store the current media type details on the instalment plan in the pfmediatypehistory table"

    Public Const kGetInstalmentHistorySQL As String = "spu_ACT_PFInstalments_History_Select"

    Public Const kCreateInstalmentNotificationSQL As String = "spu_ACT_Import_Create_Instalment_Notification"

    Public Const kGetPremiumFinanceCntSQL As String = "spu_Get_Previous_Plan_Cnt"
    Public Const kGetPremiumFinanceCntName As String = "Get the old Premium Finance Cnt with Insurance File"

    Public Const kGetPFPremiumFinanceForPlanRefSQL As String = "spu_PFPremiumFinance_Sel_For_PlanRef"
    Public Const kGetPFPremiumFinanceForPlanRefName As String = "GetPFPremiumFinanceForPlanRef"

    Public Const ksGetPFPremiumFinanceGenerateUniquePlanRefSQL As String = "spu_PFPremiumFinance_Generate_Unique_PlanRef"
    Public Const ksGetPFPremiumFinanceGenerateUniquePlanRefName As String = "GetPFPremiumFinanceGenerateUniquePlanRef"

    Public Const kGetMediaHistoryIdSQL As String = "spu_get_mediahistory_id"
    Public Const kGetMediaHistoryIdName As String = "Get Media History Id"

    Public Const kUpdateSuspendedAccountsTransactionsName As String = "Update Suspended_Accounts_Transactions  "
    Public Const kUpdateSuspendedAccountsTransactionsSQL As String = "spu_Suspended_Accounts_Transactions_Upd"

    Public Const kGetAgentSingleInstalmentPlanSQL As String = "spu_Get_Agent_Single_Instalment_Plan"
    Public Const kGetAgentSingleInstalmentPlanName As String = "GetAgentSingleInstalmentPlan"

    Public Const kPFPremiumFinanceGetPolicyListSQL As String = "spu_PFPremiumFinance_GetPolicyList"
    Public Const kPFPremiumFinanceGetPolicyListName As String = "spuPFPremiumFinanceGetPolicyList"

    Public Const kPFPremiumFinanceCancellationTransactionAddSQL As String = "spu_PFPremiumFinance_CancellationTransactions_Add"
    Public Const kPFPremiumFinanceCancellationTransactionAddName As String = "spuPFPremiumFinanceCancellationTransactionAdd"

    Public Const kPFPremiumFinanceCancellationTransactionsGetSQL As String = "spu_PFPremiumFinance_CancellationTransactions_Get"
    Public Const kPFPremiumFinanceCancellationTransactionsGetName As String = "spuPFPremiumFinanceCancellationTransactionsGet"

    Public Const kPFPremiumFinanceGetVersionSQL As String = "spu_PFPremiumFinance_Get_Version"
    Public Const kPFPremiumFinanceGetVersionName As String = "spuPFPremiumFinanceGetVersion"

    Public Const kGetLeadAgentSQL As String = "spu_Get_LeadAgent"
    Public Const kGetLeadAgentName As String = "spuGetLeadAgent"

    Public Const kPFPremiumFinanceAgentUpdSQL As String = "spu_PFPremiumFinance_Agent_Upd"
    Public Const kPFPremiumFinanceAgentUpdName As String = "PFPremiumFinanceAgentUpd"

    Public Const kGetPolicyPaidToDateSQL As String = "spu_ACT_Get_Instalments_Paid_To_Date"
    Public Const kGetPolicyPaidToDateName As String = "GetPolicyPaidToDate"

    Public Const kGetPolicyCountSQL As String = "spu_Get_Policy_Count"
    Public Const kGetPolicyCountName As String = "spuGetPolicyCount"

    Public Const kUpdateLapseReasonIdSQL As String = "spu_update_lapse_reason_id"
    Public Const kUpdateLapseReasonIdName As String = "Updatelapsereasonid"

    Public Const kACTGetOutstandingTransactionsForInsuranceFolderSQL As String = "spu_ACT_Get_Outstanding_Transactions_For_Insurance_Folder"
    Public Const kACTGetOutstandingTransactionsForInsuranceFolderName As String = "spuACTGetOutstandingTransactionsForInsuranceFolder"

    Public Const kTransdetailAddSQL As String = "spu_Transdetail_Add"
    Public Const kTransdetailAddName As String = "spuTransdetailAdd"

    Public Const kGetDocumentTypeSQL As String = "spu_Get_DocumentType"
    Public Const kGetDocumentTypeName As String = "spuGetDocumentType"

    Public Const kGetAccountIdFromShortCodeSQL As String = "spu_Get_AccountIdFromShortCode"
    Public Const kGetAccountIdFromShortCodeName As String = "spuGetAccountIdFromShortCode"

    Public Const kGetAccountSubBranchSQL As String = "spu_Get_AccountSubBranch"
    Public Const kGetAccountSubBranchName As String = "spuGetAccountSubBranch"

    Public Const kGetPeriodSQL As String = "spu_Get_Period"
    Public Const kGetPeriodName As String = "spuGetPeriod"

    Public Const kCreditControlItemAddForTransSQL As String = "spu_CreditControlItemAddForTrans"
    Public Const kCreditControlItemAddForTransName As String = "spuCreditControlItemAddForTrans"

    Public Const kGetPremiumFinancePolicySQL As String = "spu_Get_PremiumFinance_Policy"
    Public Const kGetPremiumFinancePolicyName As String = "GetPremiumFinancePolicy"

    Public Const kUpdateTransactionCodeSQL As String = "spu_updateTransaction_code"
    Public Const kUpdateTransactionCodeName As String = "Update Transaction Code"

    'PN 50319
    Public Const kGetAgentPartyBrokerSQL As String = "spu_get_agent_party_broker"
    Public Const kGetAgentPartyBrokerName As String = "Get Broker Agent Cnt"

    Public Const kGetTransDetailsForPlanName As String = "Get trans details for plan"
    Public Const kGetTransDetailsForPlanSQL As String = "spu_ACT_Get_TransDetails_For_Plan"

    Public Const kGetInsuranceFileDetailsSQL As String = "spu_CLM_Get_Insurance_File_Details"
    Public Const kGetInsuranceFileDetailsName As String = "spu_CLM_Get_Insurance_File_Details"

    Public Const ACGetPreviousInstalmentsStored As Boolean = True
    Public Const ACGetPreviousInstalmentsName As String = "Get Previous Instalments"
    Public Const ACGetPreviousInstalmentsSQL As String = "spu_Get_Previous_Instalments"

    Public Const ACGetPreviousInstalmentsAmountStored As Boolean = True
    Public Const ACGetPreviousInstalmentsAmountName As String = "Get Previous Instalments Amount"
    Public Const ACGetPreviousInstalmentsAmountSQL As String = "spu_Get_Previous_Instalments_Amount"

    Public Const kUpdateInstalmentsSQL As String = "spu_PFInstalments_Update"
    Public Const kUpdateInstalmentsName As String = "updates the instalments on the specified plan"

    Public Const kUpdateCreditCardDetailSQL As String = "spu_PFPremiumfinance_credit_card_update"
    Public Const kUpdateCreditCardDetailName As String = "updates the instalments on the specified plan"

    Public Const kIsCollectedInstalmentsSQL As String = "spu_PFInstalments_IsCollected"
    Public Const kIsCollectedInstalmentsName As String = "Is Collected Instalments"

    Public Const ACGetSchemeNosForOriginalAndRenewalPoliciesStored As Boolean = True
    Public Const ACGetSchemeNosForOriginalAndRenewalPoliciesName As String = "Get Scheme Nos For Original And Renewal Policies"
    Public Const ACGetSchemeNosForOriginalAndRenewalPoliciesSQL As String = "spu_GetSchemeNosForOriginalAndRenewalPolicies"

    Public Const ACCheckSingleInstalmentPolicyName As String = "CheckSingleInstalmentPolicy"
    Public Const ACCheckSingleInstalmentPolicySQL As String = "spu_Check_Single_Instalment_policy"

    Public Const kRenewalPlanExitsForSingleInstalmentPolicyName As String = "Renewal plan exists for single instalment policy for the specified insurance_file_cnt"
    Public Const kRenewalPlanExitsForSingleInstalmentPolicySQL As String = "spu_Renewal_PlanExitsForSingleInstalmentPolicy"

    Public Const kGetAgentDetails As String = "Get Agent Details for the specified Agent_cnt"
    Public Const kGetAgentDetailsSQL As String = "spu_Get_PartyAgentByAgentCnt"

    Public Const kGetTransDetails As String = "Get trans details for Single instalment plan"
    Public Const kGetTransDetailsSQL As String = "spu_ACT_Get_TransDetails_For_SingleInstalementPlan"

    Public Const kUpdateTransactionID As String = "Update trans details id"
    Public Const kUpdateTransactionIDSQL As String = "spu_Act_UpdateTransactionID"

    Public Const kPFTransactionidUpdSQL As String = "spu_PFTransaction_id_upd"
    Public Const kPFTransactionidUpdName As String = "Update PFTransaction"

    Public Const kUpdatePFTransactionForSingleInstalmentPlanSQL As String = "spu_PFTransaction_Id_UpdateForSingleInstalmentPlan"
    Public Const kUpdatePFTransactionForSingleInstalmentPlanName As String = "Update PFTransaction for single instalment plan"

    Public Const kRenewalGetPreviousVersionName As String = "Returns the previou version insurance file cnt for the specified insurance_file_cnt"
    Public Const kRenewalGetPreviousVersionSQL As String = "spu_Renewal_GetPreviousVersion"

    Public Const ACGetStatusIndName As String = "Get Status Ind"
    Public Const ACGetStatusIndSQL As String = "spu_PFGet_PremFinance_StatusInd"

    Public Const ksGetAgentSinglePlanCheckedSQL As String = "spu_Get_Agent_Single_Plan_Checked"
    Public Const ksGetAgentSinglePlanCheckedName As String = "GetAgentSinglePlanChecked"

    'EH011300-To get Latest instalments plan for MTA
    Public Const ACGetLatestValidFinancePlanStored2 As Boolean = True
    Public Const ACGetLatestValidFinancePlanName2 As String = "GetLatestValidFinancePlan_2"
    Public Const ACGetLatestValidFinancePlanSQL2 As String = "spu_PFPremiumFinance_Sel_Latest_Valid_Plan_2"

    Public Const ACGetFeesNotIncludedInInstalmentsName As String = "GetFeesNotIncludedInInstalments"
    Public Const ACGetFeesNotIncludedInInstalmentsSQL As String = "spu_SIR_Get_FeeNotIncludedInInstalments"

    Public Const kGetDefaultPaymentTermsName As String = "GetDefaultPaymentTerms"
    Public Const kGetDefaultPaymentTermsSQL As String = "spu_SAM_Get_Default_Payment_Terms"

    Public Const ACUpdatePFInstalmenStatusToCollectedStored As Boolean = True
    Public Const ACUpdatePFInstalmenStatusToCollectedName As String = "Update_Instalments_status_to_collected"
    Public Const ACUpdatePFInstalmenStatusToCollectedSQL As String = "spu_PFAllInstalments_UpdateStatusToCollected"

    Public Const ACGetSubAgentStored = True
    Public Const ACGetSubAgentName = "Get SubAgent"
    Public Const ACGetSubAgentSQL = "spu_getSubAgents"


    Public Const kProductRiskOptionSQLName As String = "Get ProductRisk Option"
    Public Const kProductRiskOptionSQL As String = "spu_SAM_Product_sel"

    Public Const kGetRenewalStatusFromRenInsFileCntSQL As String = "spu_Get_Renewal_Status_from_RenInsFileCnt"
    Public Const kGetRenewalStatusFromRenInsFileCntName As String = "GetRenewalStatusFromRenInsFileCnt"
    Public Const kGetRenewalStatusFromRenInsFileCntStored As Boolean = True

    Public Const kbACGetSubAgentStored As Boolean = True
    Public Const ksACGetSubAgentName As String = "Get SubAgent"
    Public Const ksACGetSubAgentSQL As String = "spu_getSubAgents"

    'Select All Transactions ID for a given PF key
    Public Const kbACGetPFGetTransactionIDStored As Boolean = True
    Public Const ksACGetPFGetTransactionIDName As String = "SelectPFGetTransactionID"
    Public Const ksACGetPFGetTransactionIDSQL As String = "spu_PFGetTransactionID"

    Public Const kbACGetPFPremiumFinanceSingleNextDateStored As Boolean = True
    Public Const ksACGetPFPremiumFinanceSingleNextDateName As String = "SelectPFPremiumFinanceSingleNextDate"
    Public Const ksACGetPFPremiumFinanceSingleNextDateSQL As String = "spu_PFPremiumFinance_Sel_Single_NextDate"

    Public Const ACGetTransDetailStored As Boolean = True
    Public Const ACGetTransDetailName As String = "SelectTransdetail"
    Public Const ACGetTransDetailSQL As String = "spu_ACT_Select_TransDetail"

    Public Const ACGetInsuranceFileDetailsFromPlanStored As Boolean = True
    Public Const ACGetInsuranceFileDetailsFromPlanName As String = "GetInsuranceFileDetailsFromFinancialplan"
    Public Const ACGetInsuranceFileDetailsFromPlanSQL As String = "spu_Get_InsuranceFileDetails_From_Financialplan"

End Module
