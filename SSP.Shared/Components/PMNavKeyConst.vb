Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("PMNavKeyConst_NET.PMNavKeyConst")>
Public Module PMNavKeyConst
    ' ***************************************************************** '
    ' Class Name: Navigator Key Constants
    '
    ' Date: 08/12/1998
    '
    ' Description: Navigator Key Name constants for ALL Apps.
    '
    ' Edit History: 08/12/1998  Original created                    RFC
    '               08/12/1998  Added Orion keys                   CTAF
    '               SP010299 - Orion Constants for reversing and recurring
    '                          transactions
    '               17/09/1999  Cash List roadmap key              CTAF
    '               TF190600 - Lead Insurer added
    '               TF190600 - Nav key change keys added
    '               TF210600 - Risk Register mode key
    '               Ram20042001 - Added Marsh Specific keys
    '               JSB 06/06/2001 - added nav process maps constants, for use when call map's directly from Client manager
    '               PF011001 - Added GNet/I4M keys from branched file
    '               ECK220102 - Add Cash ListItem Amount'
    '               SJ190602 - Moved nav keys from gSirLibraries
    '               DD 15/07/2002: Added Orion Enhanced Security Keys
    '               RVH 15/07/2002 - Added Claims keys
    '               MEvans 16-12-2002 - Added Additional FindParty KeyName Constants
    '               AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
    '               JT  01-10-2004  -  Added Key for IncludeClosed Branch checked or not
    ' ***************************************************************** '

    '*****************************************************************************
    ' Navigator Key Name Constants
    '*****************************************************************************
    Public Const PMKeyNameClientKey As String = "client_key"
    Public Const PMKeyNameClientName As String = "client_name"
    Public Const PMKeyNamePolicyKey As String = "policy_key"
    Public Const PMKeyNamePolicyNo As String = "policy_no"
    Public Const PMKeyNameListManager As String = "ListManager"
    Public Const PMKeyNameScreenId As String = "SCREEN_ID"
    Public Const PMKeyNameScreenDocument As String = "SCREEN_DOCUMENT"
    Public Const PMKeyNameSchemeProperties As String = "PROFILE"
    Public Const PMKeyNameScreenType As String = "SCREEN_TYPE"
    Public Const PMKeyNameGis As String = "GIS"
    Public Const PMKeyNameBusinessTypeId As String = "business_type_id"
    Public Const PMKeyNameBusinessTypeCode As String = "business_type_code"
    Public Const PMKeyNameDefaultRequired As String = "default_required"
    Public Const PMKeyNameDefaultPolicyId As String = "default_policy_id"
    Public Const PMKeyNamePartyCnt As String = "party_cnt"
    Public Const PMKeyNameShortName As String = "shortname"
    Public Const PMKeyNameSpecifiedTab As String = "specifiedtab"
    Public Const PMKeyNameInsuranceFolderCnt As String = "insurance_folder_cnt"
    Public Const PMKeyNameInsuranceFileCnt As String = "insurance_file_cnt"
    Public Const PMKeyNameOrigInsuranceFileCnt As String = "orig_insurance_file_cnt" ' RAW 13/11/2003 : CQ1765 : added
    Public Const PMKeyNameDiaryActionCodeId As String = "diary_action_code_id"
    Public Const PMKeyNameActionNo As String = "action_no"
    Public Const PMKeyNameGISSchemeId As String = "scheme_id"
    Public Const PMKeyNameTransactionType As String = "transaction_type"
    Public Const PMKeyNameDescription As String = "description"
    Public Const PMKeyNameTransactionTypeCode As String = "transaction_type_code"
    'sj 09/07/2001 - start
    Public Const PMKeyNameBatchComponent As String = "batch_component"
    Public Const PMKeyNameEffectiveDate As String = "effective_date"
    Public Const PMKeyNameCoverNoteIssueReason As String = "cover_note_issue_reason"
    Public Const PMKeyNamePolicyStartDate As String = "policy_start_date"
    'sj 09/07/2001 - end
    Public Const PMKeyNameChooseStepPolicyStatus As String = "choose_step_policy_status"
    Public Const PMKeyNameChooseStepKeyName As String = "choose_step_key_name"
    Public Const PMKeyNameChooseStepKeyValue As String = "choose_step_key_value"
    Public Const PMKeyNamePartyOther As String = "party_other" 'RWH(04/07/2000) RSAIB Process 007
    Public Const PMKeyNameQuickQuoteInProgress As String = "quick_quote_in_progress" 'sj 7/7/2000
    Public Const PMKeyNameRunMode As String = "Run_Mode" ' JSB 2/1/1
    ' 07/08/2000 PSA
    Public Const PMKeyNameSourceId As String = "SourceId"
    ' 07/08/2000 PSA
    Public Const PMKeyNamePartySourceId As String = "party_source_id" ' RAW 08/10/2003 : PS246 : added

    ' Start - Sankar - PN 56728
    Public Const PMKeyPartyBankId As String = "party_bank_id"
    ' End - Sankar - PN 56728

    'MKW 150606
    Public Const PMKeyNameCountryId As String = "Country_id"

    'MKW080104 PN9424 Include Complaint in FSA reasons
    Public Const PMKeyNameIncludeComplaints As String = "include_complaints"

    ' SET 08/06/2004 ISS11882 - Data Protection Act questions
    Public Const PMKeyNameAskDPAQuestions As String = "ask_dpa_questions"

    Public Const PMKeyNameTaskInstanceCnt As String = "TaskInstanceCnt"
    Public Const PMKeyNameTaskGroupCode As String = "TaskGroupCode"
    Public Const PMKeyNameTaskGroupID As String = "TaskGroupID"
    Public Const PMKeyNameTaskCode As String = "TaskCode"
    Public Const PMKeyNameTaskID As String = "TaskID"
    Public Const PMKeyNameTaskDescription As String = "TaskDescription"
    Public Const PMKeyNameTaskCustomer As String = "TaskCustomer"
    Public Const PMKeyNameTaskDueDate As String = "TaskDueDate"
    Public Const PMKeyNameTaskIsUrgent As String = "TaskIsUrgent"
    Public Const PMKeyNameTaskDaysDue As String = "TaskDaysDue"
    ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
    Public Const PMKeyNameTaskWorkflowInformation As String = "TaskWorkflowInformation"

    'WPR12
    Public Const PMKeyNameTaskQuote As String = "TaskQuote"
    Public Const PMKeyNameTaskClient As String = "TaskClient"
    Public Const PMKeyNameTaskAgent As String = "TaskAgent"
    Public Const PMKeyNameTaskProduct As String = "TaskProduct"
    Public Const PMKeyNameTaskRiskIndex As String = "TaskRiskIndex"
    Public Const PMKeyNameTaskFromDate As String = "TaskFromDate"
    Public Const PMKeyNameTaskToDate As String = "TaskToDate"
    Public Const PMKeyNameTaskDirectBusiness As String = "TaskDirectBusiness"
    Public Const PMKeyNameTaskVia As String = "TaskVia"
    Public Const PMKeyNameTaskProductText As String = "TaskProdutText"


    Public Const PMKeyNameUserGroupCode As String = "UserGroupCode"
    Public Const PMKeyNameUserGroupID As String = "UserGroupID"
    Public Const PMKeyNameUserName As String = "UserName"
    Public Const PMKeyNameUserID As String = "UserID"

    Public Const PMKeyNameBatchID As String = "batch_set_id"

    ' TF081298 - Find Claim/Party constants
    Public Const PMKeyNameClaimReference As String = "claim_ref"
    Public Const PMKeyNameAgentCnt As String = "agent_cnt"
    Public Const PMKeyNameAgent As String = "lead_agent"
    Public Const PMKeyNamePolicyHolderCnt As String = "insurance_holder_cnt"
    Public Const PMKeyNamePolicyHolder As String = "insurance_holder"
    ' TF190600
    Public Const PMKeyNameLeadInsurerCnt As String = "lead_insurer_cnt"
    Public Const PMKeyNameLeadInsurer As String = "lead_insurer"

    Public Const PMKeyNameFindClaimMode As String = "claim_mode"
    Public Const PMKeyNameClaimMode As String = "claim_mode"

    ' Extra Party keys for Gemini maps
    Public Const PMKeyNameClientCnt As String = "client_cnt"
    Public Const PMKeyNameClientUIK As String = "client_key"
    'Already got from GII
    'Public Const PMKeyNameClientName = "client_name"
    Public Const PMKeyNameClientCode As String = "client_code"
    ' TF211298 - Extra Policy keys for Gemini maps
    Public Const PMKeyNamePolicyUIK As String = "policy_key"
    'Already got from GII
    'Public Const PMKeyNamePolicyNo = "policy_no"

    ' TF311298 - Replaces NavProcessCode for FindInsurance
    Public Const PMKeyNameInsFileType As String = "insurance_file_type"
    ' ED 02082002 - Scalability Changes, now in gPMConstants
    ' RFC140199 - Used by iPMDecision
    'Public Const PMKeyNameDecisionTitle = "decision_title"
    'Public Const PMKeyNameDecisionText = "decision_text"

    ' ECK 091000
    Public Const PMKeyNameFinancePlanCnt As String = "pfprem_finance_cnt"
    Public Const PMKeyNameFinancePlanVersion As String = "pfprem_finance_version"
    Public Const PMKeyNameFinancePlanTransactions As String = "pfprem_finance_transactions"

    Public Const PMKeyNameCancelOriginalPFPlan As String = "cancel_original_pf_plan" ' RAW 13/11/2003 : CQ1765 : added

    'ED 2810200 -
    Public Const PMKeyNameInstalmentsPaidByPDC As String = "InstalmentsPaidByPDC"
    Public Const PMKeyNameShortCode As String = "short_code"

    'PN61609
    Public Const PMKeyNameFinanceAmountNetPremium As String = "is_finance_amount_net_premium"

    ' CTAF 081298 - Orion Constants
    Public Const ACTKeyNameAccountID As String = "account_id"
    Public Const ACTKeyNameLedgerID As String = "ledger_id"
    Public Const ACTKeyNameLedgerTypeID As String = "ledger_type_id"
    Public Const ACTKeyNameShortCode As String = "short_code"
    Public Const ACTKeyNameFullKey As String = "full_key"
    Public Const ACTKeyNameAccountName As String = "account_name"
    Public Const ACTKeyNameDocumentRef As String = "document_ref"
    Public Const ACTKeyNameDocumentID As String = "document_id"
    Public Const ACTKeyNameDocumenttypeID As String = "documenttype_id"
    Public Const ACTKeyNameTransDetailID As String = "trans_detail_id"
    Public Const ACTKeyNameMappingId As String = "mapping_id"
    Public Const ACTKeyNameCashListTypeId As String = "cashlisttype_id"
    Public Const ACTKeyNameCashListId As String = "cashlist_id"
    Public Const ACTKeyNameCashListItemId As String = "cashlistitem_id"
    'sw payment maintenance 06-11-2002
    Public Const ACTKeyNameActionKey As String = "actionkey"
    'sw
    '19/11/2002 - PWC - Added for Front Office Receipting
    Public Const ACTKeyNameCashListDrawerId As String = "cashlist_drawer_id"
    '30/04/2003 - PWC - ENDVR00000850
    Public Const ACTKeyNameCashListStatusId As String = "cashliststatus_id"
    'SMJB 10/07/2003
    Public Const ACTKeyNameFindCashList As String = "findcashlist"
    'eck220102
    Public Const ACTKeyNameCashListItemAmount As String = "cashlistitem_amount"
    'AR20050310 - PN19332
    Public Const ACTKeyNameCashListProcessAbort As String = "cashlist_process_abort"
    Public Const ACTKeyNamePaymentMethod As String = "payment_method"
    'AR20070222 - PN33413
    Public Const ACTKeyNameLedgerCode As String = "ledger_code"

    ' AMB 24/02/2003: PS220 - added for Manage Debtors 'refunds' development
    Public Const ACTKeyNameCashListItemPaymentTypeID As String = "cashlistitem_payment_type_id"
    Public Const ACTKeyNameCashListItemPaymentStatusId As String = "cashlistitem_payment_status_id"
    Public Const ACTKeyNameCashListItemMode As String = "cash_list_item_mode"
    Public Const ACTKeyNameAllocationId As String = "allocation_id"
    Public Const ACTKeyNameAllocationDetailId As String = "allocationdetail_id"

    Public Const ACTKeyNameCurrencyID As String = "currency_id"
    Public Const ACTKeyNameCompanyCurrencyID As String = "companycurrency_id"
    Public Const ACTKeyNameTransactionCurrencyID As String = "transactioncurrency_id"

    Public Const ACTKeyNameTransDetailIDs As String = "trans_detail_ids"
    Public Const ACTKeyNameAccountingDate As String = "accounting_date"
    Public Const ACTKeyNamePrimaryTransDetailID As String = "primary_trans_detail_id"
    Public Const ACTKeyNameAllocationTransType As String = "allocation_trans_type"
    Public Const ACTKeyNameExplorerReadOnly As String = "explorer_read_only"
    Public Const ACTKeyNameBudgetID As String = "budget_id"
    Public Const ACTKeyNamePeriodYearName As String = "period_year_name"
    Public Const ACTKeyNameRevisesBudgetId As String = "revises_budget_id"
    Public Const ACTKeyNameBasedOnBudgetId As String = "based_on_budget_id"
    Public Const ACTKeyNameInvoiceID As String = "invoice_id"
    Public Const ACTKeyNameNominalAccountID As String = "nominal_account_id"
    'eck090500
    Public Const ACTKeyNameBranchID As String = "branch_id"
    ' KG 23/06/03
    Public Const ACTKeyNameSubBranchID As String = "Sub_branch_id"

    'eck280401
    Public Const ACTKeyNamePaymentReceipt As String = "payment_receipt"
    Public Const ACTKeyNameDebitCredit As String = "debit_credit"

    ' CF110399 - If this is TRUE then stopped accounts are allowed to be selected
    Public Const ACTKeyAllowStoppedAccounts As String = "allow_stopped_accounts"
    'eck200600
    Public Const ACTKeyOnlyInsurerAndAgentAccounts As String = "allow_insurer_and_agent_accounts"

    Public Const ACTKeyDisallowInsurerAndAgentAccounts As String = "disallow_insurer_and_agent_accounts"

    ' CF060399
    Public Const ACTKeyAllowCashListButton As String = "allow_cash_list_button"
    Public Const ACTKeyAllowAllocateButton As String = "allow_allocate_button"
    'eck130904
    Public Const ACTKeyAllocationCompanyID As String = "allocation_company_ID"
    Public Const ACTKeyAllocationAccountID As String = "allocation_account_ID"
    Public Const ACTKeyAllocationCashListTypeID As String = "allocation_cashlisttype_ID"
    Public Const ACTKeyAllocationArray As String = "allocation_array"
    Public Const ACTKeyAllocationCallingAppName As String = "allocation_calling_app_name"
    'eck130904End
    ' SP010299 - Orion Constants for reversing and recurring transactions
    Public Const ACTKeyNameReversingDocumentID As String = "reversing_document_id"
    Public Const ACTKeyNameReversingDocumentDate As String = "reversing_document_date"
    Public Const ACTKeyNameDocumentDate As String = "document_date"
    Public Const ACTKeyNameReversingDocument As String = "reversing_document"

    Public Const ACTKeyNameRecurringDocumentIDs As String = "recurring_document_ids"
    Public Const ACTKeyNameRecurringDocumentDates As String = "recurring_document_dates"
    Public Const ACTKeyNameOccurrences As String = "occurrences"
    Public Const ACTKeyNameRecurringDocument As String = "recurring_document"

    ' SP250399 - If this is TRUE then stopped agents are allowed to be selected
    Public Const PMKeyNameAllowStoppedAgents As String = "allow_stopped_agents"

    ' CF200499 - Orion constant used for posting multiple allocations
    Public Const ACTKeyNameAllocationIDs As String = "allocation_ids"

    'sj220699 - Sirius solutions insurance file count
    Public Const PMKeyNameInsFileCnt As String = "insurance_file_cnt"
    Public Const PMKeyNamePaymentAccountID As String = "Payment Account ID"
    Public Const PMKeyNameDebitAgainst As String = "Debit Against"
    Public Const PMKeyNameCreditTransactions As String = "Credit Transactions"
    Public Const PMKeyNameDebitTransactions As String = "Debit Transactions"
    Public Const PMKeyNameCashListID As String = "Cash List ID"
    Public Const PMKeyNameCashListItemID As String = "Cash ListItem ID"
    Public Const PMKeyNameTransactionID As String = "TransactionID"
    Public Const PMKeyNameTransactionAmount As String = "TransactionAmount"
    Public Const PMKeyNameInsFolder As String = "insurance_folder_cnt" 'PN35753 --RC
    Public Const PMKeyNameSettleTransactions As String = "Settle Transactions"

    ' CF170999 - Orion constant used for specifying the roadmap to use
    '            in cash list item
    Public Const ACTKeyNameCashListAllocationRoadmap As String = "cash_list_allocation_roadmap"

    ' EK 151299 -
    Public Const ACTKeyNameInsurerPayment As String = "insurer_payment"
    'eck240102 New For Write Offs
    Public Const ACTKeyNameWriteOffReasonId As String = "writeoff_reason_id"
    Public Const ACTKeyNameWriteOffAmount As String = "writeoff_amount"
    ''Round Off
    Public Const ACTKeyNameRoundOffAmount = "roundoff_amount"
    Public Const ACTKeyNameRoundOffTransDetailId = "Roundoff_TransDetail_Id"

    'KB PN 6747 For currency differences
    Public Const ACTKeyNameCurrencyDifference As String = "currency_difference"
    Public Const ACTKeyNameAllocatingSuspense As String = "allocating_suspense" 'FSA Phase 3.2
    'EK 20/12/99
    Public Const ACTInsurerPaymentRoadMap As String = "ACTINSPAY2"

    ' SD 21/01/2003 - Orion Constant for finding bank account
    Public Const ACTKeyNameBankAccountID As String = "bankaccount_id"

    'sw 29/01/2003
    Public Const ACTKeyNameReceiptPolicyRef As String = "receipt_policy_ref"

    ' AMB 21/02/2003: PS220 - added keys for Manage Debtors development
    Public Const ACTKeyNamePaymentName As String = "payment_name"
    Public Const ACTKeyNameMediaTypeID As String = "media_type_id"
    Public Const ACTKeyNameTotalPremium As String = "total_premium"
    Public Const ACTKeyPartyBankID As String = "party_bank_id"
    Public Const ACTKeyNameAllocationBatchId As String = "allocationbatch_id"
    Public Const kTKeyNameTransDetailExID As String = "trans_detail_ex_id"
    Public Const ACTKeyNameTransactionDate As String = "transaction_date"
    Public Const kTKeyNameWriteOffAllocationId As String = "WriteOffAllocation_Id"
    Public Const kTKeyNameWriteOffTransDetailId As String = "WriteOffTransDetail_Id"
    ' CF 261099 - For iPMBFindParty. Display find or just the Parties screen
    Public Const PMKeyNamePartiesOnly As String = "parties_only" ' YES/NO String
    Public Const PMKeyNamePartyType As String = "party_type"

    'RKS 141004 PN13238 & PN14838
    Public Const PMKeyNameIncludeClosedBranches As String = "include_closed_branches" 'YES/NO String

    ' CF 261099 - Why wasn't this in here before?!?!?
    'Already got from GII
    'Public Const PMKeyNamePartyCnt As String = "party_cnt"

    ' CF 271099 -
    Public Const PMKeyNameInsFolderCnt As String = "insurance_folder_cnt"

    ' CF 271099 -
    Public Const PMKeyNameRiskGroupID As String = "risk_group_id"
    ' CTAF 190600
    Public Const PMKeyNameRiskCodeID As String = "risk_code_id"
    ' TF301100
    Public Const PMKeyNameRiskScreenID As String = "risk_screen_id"

    ' eck080301
    Public Const PMKeyNameRiskCnt As String = "risk_cnt"
    'eck230301
    Public Const PMKeyNameQuoteId As String = "quote_id"
    'Tomo021199
    Public Const PMKeyNameEventCnt As String = "event_cnt"
    Public Const PMKeyNameDocumentTemplateId As String = "document_template_id"
    Public Const PMKeyNameDocumentTypeId As String = "document_type_id"

    'jmf 22/4/2003
    Public Const PMKeyNameDocumentTemplateVersionId As String = "document_template_version_id"
    Public Const PMKeyNameDocumentTemplateEffectiveDate As String = "document_template_effective_date"


    ' TF140901 - GNet application indicators
    Public Const PMKeyNameFromGNet As String = "is_from_gnet"
    Public Const PMKeyNameFromIts4Me As String = "is_from_its4me"
    'eck050402
    Public Const PMKeyNameFromBatchTrans As String = "is_from_batchtrans"

    ' CTAF 240200
    Public Const ACTKeyNameCashListRoadmap As String = "cash_list_roadmap"

    ' CTAF 020300
    Public Const ACTKeyNameAutoAllocate As String = "auto_allocate"


    ' CTAF 310500
    Public Const PMKeyNameNavStep As String = "nav_step"
    Public Const PMKeyNameWMTask As String = "wm_task"
    Public Const PMKeyNameWMDescription As String = "wm_description"
    ' CTAF 200600
    Public Const PMKeyNameWMStep As String = "wm_step"

    ' CTAF 130600
    Public Const PMKeyNameDocTemplateMode As String = "doc_template_mode"
    Public Const PMKeyNameDocName As String = "roadmap_document"

    ' TF150600
    Public Const PMKeyNameInsFileStatus As String = "ins_file_status"

    ' TF190600 - for changing Nav Key value
    Public Const PMKeyNameNavSourceKey As String = "nav_source_key"
    Public Const PMKeyNameNavTargetKey As String = "nav_target_key"

    ' TF210600 - for Risk Register mode settings
    Public Const PMKeyNameRiskRegMode As String = "risk_register_mode"

    ' CTAF 310800 - Misc. Data - Just use this for passing in default
    '                            values - Dont use for GetKeys!!!
    Public Const PMKeyNameMiscDefault As String = "misc_default"

    'RWH(26/09/2000) RSAIB Process 28. Process/Document Type (e.g 'Quotation', 'Proposal form', 'Schedule')
    Public Const PMKeyNameProcessType As String = "process_type"
    ' EK 041200 -
    Public Const ACTKeyNameFinanceDeposit As String = "finance_deposit"
    ' DC 030702
    Public Const ACTKeyNameDefaultProduct As String = "default_product"

    ' TF071100 - Premium Finance
    Public Const PMKeyCompanyNo As String = "company_no"
    Public Const PMKeyNameSchemeNo As String = "scheme_no"
    Public Const PMKeyNameSchemeVersion As String = "scheme_version"

    'Thinh Nguyen 01/02/2004
    Public Const PMKeyNamePlanInsuranceFileCnt As String = "Plan_InsuranceFileCnt"
    Public Const PMKeyNamePlanInsuranceFolderCnt As String = "Plan_InsuranceFolderCnt"
    Public Const PMKeyNamePlanIsSingleInstalment As String = "Plan_IsSingleInstalment"

    ' DC091100 - Claims
    Public Const PMKeyNameOperateMode As String = "claim_mode"
    Public Const PMKeyNamePolicyID As String = "insurancefile_cnt"
    Public Const PMKeyNamePolicyNumber As String = "policy_number"
    Public Const PMKeyNameClaimNumber As String = "claim_ref"
    Public Const PMKeyNameClaimID As String = "claim_cnt"
    Public Const PMKeyNameRiskTypeID As String = "risk_type_id"
    Public Const PMKeyNameRiskTypeCode As String = "risk_type_code" 'DD10122001
    Public Const PMKeyNameClaimDate As String = "claim_date"
    Public Const PMKeyNameClientHolder As String = "policy_holder"
    Public Const PMKeyNameClaimPaymentId As String = "payment_id"

    Public Const PMKeyNameRiskIndex As String = "risk_index"
    Public Const PMKeyNameFromDate As String = "from_date"

    '-------------------------------------------------------------------------------------
    '   15/07/2002  RVH BEGIN
    '                   Add new constants for claim keys - for some reason the "claim_cnt"
    '                   has previously been set to the PMKeyNameClaimID...
    '-------------------------------------------------------------------------------------
    Public Const PMKeyNameRealClaimID As String = "claim_id"
    Public Const PMKeyNamePerilID As String = "peril_id"
    Public Const PMKeyNameClaimPerilID As String = "claim_peril_id"
    Public Const PMKeyNameClaimGISScreenID As String = "gis_screen_id"
    Public Const PMKeyNameClaimRiskID As String = "claim_risk_id"
    Public Const PMKeyNameWorkClaimID As String = "work_claim_id"
    Public Const PMKeyNameWorkClaimPerilID As String = "work_claim_peril_id"
    Public Const PMKeyNameClaimTransactionType As String = "claim_transaction_type"
    Public Const PMKeyNameClaimInsFileCnt As String = "claim_insurance_file"
    Public Const PMKeyNameNoTransaction As String = "no_transactions"
    'CMG/PB 12092002 Loss Schedule
    Public Const PMKeyNameLossSchedule As String = "LossSchedule"
    Public Const PMKeyNameLossScheduleTypeId As String = "LossScheduleTypeID"
    Public Const PMKeyNameItemDetailId As String = "ItemDetailID"
    Public Const PMKeyNamePerilTypeId As String = "PerilTypeID"

    ' RVH 29/08/2003 : CQ1943, 2089 - START - Add const for peril OIKEY
    Public Const PMKeyNameClaimPerilOIKey As String = "ChildOIKey"
    ' RVH 29/08/2003 : CQ1943, 2089 - END

    ' RVH 24/12/2004 - Added for screen caption override in iPMURisk
    Public Const PMKeyNameScreenCaption As String = "screen_caption"
    Public Const PMKeyNameIncludeClosedClaims As String = "include_closed_claims"

    Public Const PMKeyNameRecommendation As String = "RecommendClaimPayment"
    Public Const PMKeyNameClaimPaymentIDs As String = "Claim Payment IDs"
    Public Const PMKeyNameOurRef As String = "Our Reference"
    Public Const PMKeyNameTheirRef As String = "Their Reference"

    Public Const PMKeyNameClaimClosed As String = "claim_closed"


    '-------------------------------------------------------------------------------------
    '   15/07/2002  RVH END
    '-------------------------------------------------------------------------------------

    ' CTAF 040101 - Readded the following keys!
    ' CTAF 171100 - FindParty - Swift Requirements
    Public Const PMKeyNamePartyLongName As String = "party_long_name"
    Public Const PMKeyNamePartyResolvedName As String = "party_resolved_name"
    Public Const PMKeyNameDOB As String = "date_of_birth"
    Public Const PMKeyNameAddLine1 As String = "address_line_1"
    Public Const PMKeyNameAddLine2 As String = "address_line_2"
    Public Const PMKeyNameAddLine3 As String = "address_line_3"
    Public Const PMKeyNameAddLine4 As String = "address_line_4"
    Public Const PMKeyNamePostCode As String = "post_code"
    Public Const PMKeyNameStatus As String = "party_status"
    Public Const PMKeyNameFullAddress As String = "full_address"

    ' TF061200 - Reports
    Public Const AC_VIEW_ONLY As Integer = 0
    Public Const AC_PRINT_ONLY As Integer = 1
    Public Const AC_PRINT_AND_VIEW As Integer = 2
    Public Const AC_EXPORT_TO_HTML As Integer = 3
    Public Const PMKeyNameReportName As String = "report_name"
    Public Const PMKeyNamePrintReport As String = "report_print_options"
    'added to print the report in .doc format
    Public Const PMKeyNameReportType As String = "report_type"
    Public Const PMKeyNameParam1Name As String = "param_name1"
    Public Const PMKeyNameParam1Value As String = "param_value1"
    Public Const PMKeyNameParam2Name As String = "param_name2"
    Public Const PMKeyNameParam2Value As String = "param_value2"
    Public Const PMKeyNameParam3Name As String = "param_name3"
    Public Const PMKeyNameParam3Value As String = "param_value3"
    Public Const PMKeyNameParam4Name As String = "param_name4"
    Public Const PMKeyNameParam4Value As String = "param_value4"
    Public Const PMKeyNameParam5Name As String = "param_name5"
    Public Const PMKeyNameParam5Value As String = "param_value5"
    Public Const PMKeyNameParam6Name As String = "param_name6"
    Public Const PMKeyNameParam6Value As String = "param_value6"
    Public Const PMKeyNameParam7Name As String = "param_name7"
    Public Const PMKeyNameParam7Value As String = "param_value7"
    Public Const PMKeyNameParam8Name As String = "param_name8"
    Public Const PMKeyNameParam8Value As String = "param_value8"
    Public Const PMKeyNameParam9Name As String = "param_name9"
    Public Const PMKeyNameParam9Value As String = "param_value9"
    Public Const PMKeyNameParam10Name As String = "param_name10"
    Public Const PMKeyNameParam10Value As String = "param_value10"
    Public Const PMKeyNameParam11Name As String = "param_name11"
    Public Const PMKeyNameParam11Value As String = "param_value11"
    Public Const PMKeyNameParam12Name As String = "param_name12"
    Public Const PMKeyNameParam12Value As String = "param_value12"
    Public Const PMKeyNameParam13Name As String = "param_name13"
    Public Const PMKeyNameParam13Value As String = "param_value13"
    Public Const PMKeyNameParam14Name As String = "param_name14"
    Public Const PMKeyNameParam14Value As String = "param_value14"
    Public Const PMKeyNameParam15Name As String = "param_name15"
    Public Const PMKeyNameParam15Value As String = "param_value15"
    Public Const PMKeyNameParam16Name As String = "param_name16"
    Public Const PMKeyNameParam16Value As String = "param_value16"

    Public Const PMKeyNameKeyDefaults As String = "key_defaults"
    Public Const PMKeyNameKeyPrompts As String = "key_prompts"

    ' Ram20042001 - Added Marsh Specific keys
    ' These keys are used by iPMBrowserControl
    Public Const PMKeyNameWebServerURL As String = "web_server_url"
    Public Const PMKeyNameReferralURL As String = "referral_url"
    Public Const PMKeyNameDisplayMode As String = "display_mode"
    Public Const PMKeyNameSelectedInsurers As String = "selected_insurers"
    Public Const PMKeyNameGoToURL As String = "go_to_url"
    Public Const PMKeyNameInsurerID As String = "insurer_id"
    Public Const PMKeyNameQuoteRef As String = "quote_ref"
    Public Const PMKeyNameRiskID As String = "risk_id"

    'SSL 04/06/2001 -- Included renewal constant to identify
    '                   renewal or NB Transact
    Public Const PMKeyNameIsRenewal As String = "RENEWAL"
    'SSL - End

    'SSL 21/06/2001 -- Including status to know
    '                  whether Confirm or Lapse button was choosen
    Public Const PMKeyNameIsConfirmORLapse As String = "IsConfirmORLapse"
    'SSL - End

    'SSL 13/8/01 -- including status to know whether this component being
    ' called by Renewal - What If Quote
    Public Const PMKeyNameIsWhatIFQ As String = "IsWhatIFQ"
    Public Const PMKeyNameIsViewQuote As String = "BrokerLedVwQuote"
    'SSL - End
    Public Const PMKeyNameRenewalDate As String = "renewal_date"
    Public Const PMKeyNameOriginalRenewalQuote As String = "original_renewal_quote"
    Public Const PMKeyNameRenewalPolicyStatus As String = "renewal_policy_status"
    Public Const PMKeyNameActiveRenewalQuote As String = "active_renewal_quote" ' PN17680
    Public Const PMKeyNameRenewalConfirmationMode As String = "renewal_confirmation_mode" ' PN18750

    ' RDC 13062003 User configurable step keys
    ' DIARY
    Public Const PMKeyNameDiaryNXMStep As String = "DiaryNXMStep"
    Public Const PMKeyNameDiaryTask As String = "DiaryTask"
    Public Const PMKeyNameDiaryDescription As String = "DiaryDescription"
    Public Const PMKeyNameDiaryWMStep As String = "DiaryWMStep"
    Public Const PMKeyNameDiaryUserGroupID As String = "DiaryUserGroupId"
    Public Const PMKeyNameDiaryUserID As String = "DiaryUserId"
    Public Const PMKeyNameDiaryTaskDays As String = "DiaryTaskDays"
    Public Const PMKeyNameDiaryTaskGroupID As String = "DiaryTaskGroupId"
    Public Const PMKeyNameDiaryTaskID As String = "DiaryTaskId"
    'SCREENTEXT
    Public Const PMKeyNameMessageBoxCaption As String = "description"
    Public Const PMKeyNameMessageBoxText As String = "TEXT"
    ' QUESTION
    Public Const PMKeyNameQuestionTitle As String = "QuestionTitle"
    Public Const PMKeyNameQuestionText As String = "QuestionText"
    ' EDITTEXT
    Public Const PMKeyNameEditText As String = "EditText"
    ' RAISEEVENT
    Public Const PMKeyNameRaiseEvent As String = "RaiseEvent"
    ' STANDARDLETTER
    Public Const PMKeyNameStandardLetter As String = "StandardLetter"
    ' LAUNCHEXE
    Public Const PMKeyNameLaunchEXEFile As String = "LaunchExeFile"
    Public Const PMKeyNameLaunchEXECmd As String = "LaunchExeCmd"
    ' USERCOMPONENT
    Public Const PMKeyNameScriptFilename As String = "ScriptFilename"
    Public Const PMKeyNameScriptStartMethod As String = "ScriptStartMethod"

    '***eck Brough through from Combined were added to an unshared version
    ' SJP (CMG) 08042003 PS235
    Public Const PMKeyNameAccountExecutiveCnt As String = "account_executive_cnt"
    Public Const PMKeyNameAccountExecutive As String = "account_executive"
    ' AMB 29-Oct-03: 1.8.6 MMM True Monthly Policies - true monthly policy handling
    Public Const PMKeyNameTrueMonthlyPolicy As String = "true_monthly_policy"
    '*******

    'JSB 06/06/2001 - Process Map Constants
    Public Const PMNavProcMapGIIStartMotorNB As String = "G2_M_MS"
    Public Const PMNavProcMapGIIStartMotorMaintain As String = "G2_M_MPDS"
    Public Const PMNavProcMapGIIStartMotorMTA As String = "G2_M_MTA"
    Public Const PMNavProcMapGIIStartMotorRebroke As String = "G2_M_MS"
    Public Const PMNavProcMapGIIStartMotorReivew As String = "G2_M_MRS"
    Public Const PMNavProcMapGIIStartHouseholdNB As String = "G2_H_LD"
    Public Const PMNavProcMapGIIStartHouseholdMaintain As String = "G2_H_MPDS"
    Public Const PMNavProcMapGIIStartHouseholdMTA As String = "G2_H_MTA"
    Public Const PMNavProcMapGIIStartHouseholdRebroke As String = "G2_H_LD"
    Public Const PMNavProcMapGIIStartHouseholdReivew As String = "G2_H_HRS"
    Public Const PMNavProcMapGIIStartCVNB As String = "G2_T_MS"
    Public Const PMNavProcMapGIIStartCVMaintain As String = "G2_T_MPDS"
    'Public Const PMNavProcMapGIIStartCVMTA ="
    Public Const PMNavProcMapGIIStartCVRebroke As String = "G2_T_MS"
    'KB PN Issue 4895
    'For CV review - was calling the wrong step of the roadmap to start
    'Public Const PMNavProcMapGIIStartCVReivew = "G2_T_MR"
    Public Const PMNavProcMapGIIStartCVReivew As String = "G2_T_MRS"

    Public Const PMNavProcMapSQStartMotorNB As String = "SFO_NB_MOTOR.xml"
    Public Const PMNavProcMapSQStartMotorNBCurrentNavXMStep As Integer = 3
    Public Const PMNavProcMapSQStartMotorMaintain As String = "SFO_MAINTAIN_DETAILS_MOTOR.XML"
    Public Const PMNavProcMapSQStartMotorMaintainCurrentNavXMStep As Integer = 3
    Public Const PMNavProcMapSQStartMotorMTA As String = "SFO_MTA_MOTOR.XML"
    Public Const PMNavProcMapSQStartMotorMTACurrentNavXMStep As Integer = 3
    Public Const PMNavProcMapSQStartMotorRebroke As String = "SFO_REBROKE_MOTOR.XML"
    Public Const PMNavProcMapSQStartMotorRebrokeCurrentNavXMStep As Integer = 3
    Public Const PMNavProcMapSQStartMotorReview As String = "SFO_REVIEW_MOTOR.XML"
    Public Const PMNavProcMapSQStartMotorReviewCurrentNavXMStep As Integer = 4

    Public Const PMNavProcMapSQStartHouseholdNB As String = "SFO_NB_HOUSEHOLD.xml"
    Public Const PMNavProcMapSQStartHouseholdNBCurrentNavXMStep As Integer = 3
    Public Const PMNavProcMapSQStartHouseholdMaintain As String = "SFO_MAINTAIN_DETAILS_HOUSEHOLD.XML"
    Public Const PMNavProcMapSQStartHouseholdMaintainCurrentNavXMStep As Integer = 3
    Public Const PMNavProcMapSQStartHouseholdMTA As String = "SFO_MTA_HOUSEHOLD.XML"
    Public Const PMNavProcMapSQStartHouseholdMTACurrentNavXMStep As Integer = 3
    Public Const PMNavProcMapSQStartHouseholdRebroke As String = "SFO_REBROKE_HOUSEHOLD.XML"
    Public Const PMNavProcMapSQStartHouseholdRebrokeCurrentNavXMStep As Integer = 3
    Public Const PMNavProcMapSQStartHouseholdReview As String = "SFO_REVIEW_HOUSEHOLD.XML"
    Public Const PMNavProcMapSQStartHouseholdReviewCurrentNavXMStep As Integer = 4

    Public Const PMNavProcMapSQStartCVNB As String = "SFO_NB_CV.xml"
    Public Const PMNavProcMapSQStartCVNBCurrentNavXMStep As Integer = 3
    Public Const PMNavProcMapSQStartCVMaintain As String = "SFO_MAINTAIN_DETAILS_CV.XML"
    Public Const PMNavProcMapSQStartCVMaintainCurrentNavXMStep As Integer = 3
    Public Const PMNavProcMapSQStartCVMTA As String = "SFO_MTA_CV.XML"
    Public Const PMNavProcMapSQStartCVMTACurrentNavXMStep As Integer = 3
    Public Const PMNavProcMapSQStartCVRebroke As String = "SFO_REBROKE_CV.XML"
    Public Const PMNavProcMapSQStartCVRebrokeCurrentNavXMStep As Integer = 3
    Public Const PMNavProcMapSQStartCVReview As String = "SFO_REVIEW_CV.XML"
    Public Const PMNavProcMapSQStartCVReviewCurrentNavXMStep As Integer = 4

    'KB20010621 Marsh specific for quote Manager
    Public Const PMKeyNamePolicyStatus As String = "policy_status"

    'TF270901 - to enable Post Quote control
    Public Const PMKeyNamePostQuoteStatus As String = "post_quote_status"

    'CLG20011210 Copied from gSIRLibraries
    'Public Const PMKeyNameNavigatorTitle1 = "navigator_title_1"
    Public Const PMKeyNameProductID As String = "Product_id"
    Public Const PMKeyNameProductCode As String = "product_code"

    ' CTAF 120202
    Public Const PMKeyNameCurrentNashStep As String = "restart_step"
    ' CTAF 16102 - Duplicated above but different name for XM consistency
    Public Const PMKeyNameCurrentNavXMStep As String = "restart_step"

    'TF180202
    Public Const PMKeyNameDataModelCode As String = "data_model_code"

    'sj 19/06/2002 - start
    'Move all these from gSirLibraries
    '------------------------------------------
    Public Const PMKeyNameTextType As String = "TextType"
    Public Const PMKeynameTableName As String = "EntityName"
    Public Const PMKeyNameNavigatorTitle1 As String = "navigator_title_1"
    Public Const PMKeyNamePartyTypeID As String = "party_type_id"
    Public Const PMKeyNameInsFileID As String = "insurance_file_id"
    Public Const PMKeyNameInsReference As String = "insurance_ref"
    Public Const PMKeyNameLongName As String = "long_name"
    Public Const PMKeyNameRenProcessCode As String = "renewal_process_code"
    Public Const PMKeyNameExtraTypeCode As String = "code"
    Public Const PMKeyNameRenewToDate As String = "renew_to_date"
    Public Const PMKeyNameBatchSetID As String = "batch_set_id"
    Public Const PMKeyNameNavProcessCode As String = "nav_process_code"
    Public Const PMKeyNameMsgBoxCaption As String = "message_box_caption"
    Public Const PMKeyNameMsgBoxText As String = "message_box_text"
    Public Const PMKeyNameInitialNoticeFromDate As String = "initial_fromdate"
    Public Const PMKeyNameInitialNoticeToDate As String = "initial_todate"
    Public Const PMKeyNameMainEventTypeCode As String = "main_event_type_code"
    Public Const PMKeyNameSubEventTypeCode As String = "sub_event_type_code"
    Public Const PMKeyNameMainEventID As String = "main_event_id"
    Public Const PMKeyNameSubEventID As String = "sub_event_id"
    Public Const PMKeyNameLapsedReasonID As String = "lapsed_reason_id"
    Public Const PMKeyNameClaimCnt As String = "claim_cnt"

    ' PH130298 - Key names for iSirMTADataControl.NavigatorV2
    Public Const PMKeyNameLiveInsFileCnt As String = "live_insurance_file_cnt"
    Public Const PMKeyNameMTAInsFileCnt As String = "mta_insurance_file_cnt"
    Public Const PMKeyNameIsPermanent As String = "is_permanent"
    Public Const PMKeyNameIsOutOfSequence As String = "is_out_of_sequence"
    Public Const PMKeyNameOutOfSequenceEditing As String = "is_out_of_sequence_editing"
    ' PH260298 - Key names for iSirMTADataControl.NavigatorV2
    Public Const PMKeyNameMTACnt As String = "mta_cnt"
    Public Const PMKeyNameCoverStartDate As String = "cover_start_date"
    ' TF260398
    Public Const PMKeyNameExpiryDate As String = "expiry_date"
    Public Const PMKeyNameArchiveFolderId As String = "archive_folder_id"

    'sj 19/06/2002 - end

    'DD 15/07/2002: Key names for Orion Enhance Security Product Option
    Public Const ACTKeyNameOnlyUpdatableAccounts As String = "only_updatable"
    Public Const ACTKeyNameAccountUpdatable As String = "account_updatable"

    'sj 06/09/2002 - start
    Public Const PMKeyNameRefundAmount As String = "refund_amount"
    'sj 06/09/2002 - end

    '06/11/2002 - PWC - added for setting start up form
    Public Const PMKeyNameStartForm As String = "start_form"

    '****************************
    ' MEvans 16-12-2002 : 202
    ' New constants for IPMFindParty - used in relation to multiple addresses.
    Public Const PMKeyNameAllowAddressSelection As String = "allow_address_selection"
    Public Const PMKeyNameAddressCnt As String = "address_cnt"
    '****************************

    'KR 03/02/03 - start
    Public Const PMKeyNameCMClaimNumber As String = "claim_number"
    Public Const PMKeyNameCMRiskIndex As String = "risk_index"
    Public Const PMKeyNameClaimPolicyHolder As String = "policy_holder"
    'KR 03/02/03 - end

    'RAM20030203 - Start
    Public Const PMKeyNameReason As String = "reason"
    'RAM20030203 - End

    'SW 13/02/2003 Added key to identify different Hub Interface Type Codes
    Public Const PMKeyNameInterfaceTypeCode As String = "interface_type_code"



    'sj 10/03/2003 - start
    'ISS2463
    Public Const PMKeyNameRequoteFlag As String = "requote_flag"
    'sj 10/03/2003 - end

    '***************
    ' MEvans : 14-05-2003 : CQ  709
    Public Const PMKeyNameApprovalType As String = "ApprovalType"
    '***************

    '27/05/2003 - PWC - 186 - Debt Roll-up
    Public Const PMKeyNameFindTransRollUp As String = "find_trans_rollup"
    Public Const PMKeyNameFindTransSearchParams As String = "find_trans_search_params"


    'AAB - 06/05/2003 Added MTAType for 1.9 integration
    Public Const ACTKeyNameMTAType As String = "mta_type'"

    ' SJP (CMG) 07/05/2003
    Public Const PMKeyNameRenewalFrequencyId As String = "renewal_frequency_id"
    ' SJP - END

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20030612 - Start. Ref. PN Issue 4132
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Const PMKeyNameAnalysisCodeID As String = "analysis_code_id"
    Public Const PMKeyNameDataModelID As String = "data_model_id"
    Public Const PMKeyNameGISPolicyLinkID As String = "gis_policy_link_id"
    Public Const PMKeyNameQuoteExpiryDaysAllowed As String = "quote_expiry_days_allowed"
    Public Const PMKeyNamePolicyQuoteDate As String = "Policy_Quote_Date"
    Public Const PMKeyNameInceptionDate As String = "inception_date"
    ' SET 08/07/2003 ISS4132
    Public Const PMKeyNameBranchId As String = "branch_id"
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20030612 - End
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    'TF200603
    Public Const PMKeyNameRenewalProcess As String = "renewal_process"

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' RAM20030814 - START. Ref. PN Issue 5746
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Const PMKeyNamePMWrkTaskInstanceCnt As String = "task_instance_cnt"
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'RAM20030814 - End
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'sj 10/09/2003 - Start
    Public Const PMKeyNameFSAComplaintFolderCnt As String = "fsa_complaint_folder_cnt"
    Public Const PMKeyNameFSAComplaintFileCnt As String = "fsa_complaint_file_cnt"
    'sj 10/09/2003 - End
    Public Const PMKeyNameFSAComplaintReference As String = "fsa_complaint_folder_reference" 'FSA Phase 3.2
    ' CJB 060904 Added as part of Complaints module MH requested rework
    ' Determines if complaint is related to a policy or claim or General complaint
    Public Const PMKeyNameFSAComplaintType As String = "fsa_complaint_type"

    ' SET 10/10/20003 PS261 - archive document after printing it.
    Public Const PMKeyNameArchiveAfterPrinting As String = "ArchiveAfterPrinting"
    'DC151003 -PN7449
    Public Const PMKeyNameHandlerType As String = "handler_type"

    ' CTAF 20040302 - Start
    Public Const PMKeyNameAlternateReference As String = "alternate_reference"
    Public Const PMKeyNamebrokerABIID As String = "broker_abi_id"
    ' CTAF 20040302 - End
    'SJ 02/03/2004 - start
    Public Const PMKeyNameExternalSchemeNo As String = "external_scheme_no"
    Public Const PMKeyNameThisPremium As String = "this_premium"
    Public Const PMKeyNameNetPremium As String = "net_premium"
    Public Const PMKeyNameTaxAmount As String = "tax_amount"
    Public Const PMKeyNameTaxRate As String = "tax_rate"
    Public Const PMKeyNameCalledFromSTS As String = "CalledFromSTS"
    Public Const PMKeyNamePolicyVersion As String = "policy_version"
    Public Const PMKeyNameUnderwritingBranchEnabled As String = "underwriting_branch_enabled"
    'SJ 02/03/2004 - end

    'SJ 15/04/2004 - start
    Public Const PMKeyNameIsInsurerMode As String = "is_insurer_mode"
    Public Const PMKeyNameRenewalTransferReason As String = "renewal_transfer_reason"
    Public Const PMKeyNameRenewalTransferNotes As String = "renewal_transfer_notes"
    'SJ 15/04/2004 - end

    'JT PN-13238
    Public Const PMKeyNameIsIncludeClosedBrancheChecked As String = "is_include_closed_branch_Chkd" 'True/False

    'DJM 02/11/2004
    Public Const PMKeyNameClaimPaymentSequence As String = "sequenceno"
    Public Const PMKeyNameUseExtraKeys As String = "use_extra_keys"

    Public Const PMKeyNameClaimPaymentAuthoriseMode As String = "authorise_mode"

    Public Const PMKeyNamePayeeName As String = "payee_name"
    Public Const PMKeyNamePayeeAccountCode As String = "payee_account_code"
    Public Const PMKeyNamePayeeSortCode As String = "payee_sort_code"
    Public Const PMKeyNamePayeeComments As String = "payee_comments"

    Public Const PMKeyNameClaimPayment As String = "claim_payment"
    Public Const PMKeyNameCurrencyCode As String = "currency_code"

    Public Const PMKeyNameOptionNumber As String = "option_number"

    'JAS 01122004 PN17134
    Public Const PMKeyNamePremiumFeesTax As String = "premium_fees_tax"

    Public Const PMKeyNameCalledFromSwift As String = "called_from_swift" ' RAM20050104 - Added for SWIFT

    Public Const PMKeyNameAccountHandlerCnt As String = "account_handler_cnt"
    Public Const PMKeyNameRegarding As String = "regarding"

    Public Const PMKeyNamePaymentMethod As String = "payment_method"

    ' Indicate if the recovery component should run in salvage mode
    Public Const PMKeyNameRecoveryIsSalvage As String = "recovery_is_salvage"

    Public Const PMKeyNameOriginalInsuranceFileCnt As String = "original_insurance_file_cnt" ' RAM20050825 - PN 23018
    Public Const PMKeyNameNewBusinessNoTrans As String = "no_transactions" ' PN 43045 --SUR

    ' Indicate if the maintain claim roadmap is running in "balance and close" mode
    Public Const PMKeyNameBalanceAndCloseClaim As String = "balance_and_close_claim"

    Public Const PMKeyNameIsTrueMonthlyPolicy As String = "is_true_monthly_policy"

    Public Const PMKeyNameRenewalHoldingInsurerGISSchemeId As String = "ren_holding_ins_scheme_id"
    Public Const PMKeyNameMTAOriginalRiskXML As String = "MTAOriginalRiskXML"
    Public Const PMKeyNameRenewalStatusType As String = "renewal_status_type"

    ' R.Griffiths 2006-10-16 (Plus One)
    Public Const PMKeyNamePartyTelephonePrefix As String = "party_telephone_prefix"
    Public Const PMKeyNamePartyTelephoneNumber As String = "party_telephone_number"
    Public Const PMKeyNamePartyAutoSearch As String = "party_auto_search"

    Public Const PMKeyNameKeepWindowOnTop As String = "keep_window_on_top"

    ' Hall & Clarke Changes
    Public Const PMKeyNameDefaultBranchCode As String = "default_branch_code"
    Public Const PMKeyNameDefaultAnalysisCode As String = "default_analysis_code"
    Public Const PMKeyNameBatchrun As String = "batch_run"
    Public Const PMKeyAgentDocRun As String = "agent_doc"

    'Display Claim Reinsurance
    Public Const PMKeyNameDisplayClaimReinsurance As String = "display_claim_reinsurance"

    'PN38836
    'Public Const PMKeyNameFinancePlanEditAuthority = "FinancePlanEditAuthority"
    'PYV DM
    Public Const PMKeyBureauAccount As String = "bureau_account"
    Public Const PMKeyNameBankingAmount As String = "banking_amount"

    'WR5 - Claims Workflow
    Public Const PMKeyNameClaimWorkflowId As String = "claim_workflow_id"
    Public Const PMKeyNameDisplayCheckUnpaidStatus As String = "display_check_unpaid_status"
    Public Const PMKeyNameDisplaySalvageRecovery As String = "display_salvage_recovery"
    Public Const PMKeyNameDisplayThirdPartyRecovery As String = "display_third_party_recovery"
    Public Const PMKeyNameDisplayExternalClaimHandling As String = "display_external_claim_handling"
    Public Const PMKeyNameDisplayDescriptionForChange As String = "display_description_for_change"
    Public Const PMKeyNameDisplayClaimDocMessage As String = "display_claim_doc_message"
    Public Const PMKeyNameGenerateClaimDocument As String = "generate_claim_document"
    Public Const PMKeyNameDisplayClaimPaymentProcess As String = "display_claim_payment_process"
    Public Const PMKeyNameCheckDeferredReinsurance As String = "check_deferred_reinsurance"
    Public Const PMKeyNameDisplayCashPaymentProcess As String = "display_cash_payment_process"
    Public Const PMKeyNameDisplayMakeFurtherPayments As String = "display_make_further_payments"
    Public Const PMKeyNameFastTrackClaimPayment As String = "Fast_track_claim_payment"
    Public Const PMKeyNameUserAuthRunCashPayment As String = "UserAuthorityToRunCashPaymentTask"
    Public Const PMKeyNameUserAuthRunClaimPayment As String = "UserAuthRunClaimPayment"

    Public Const PMKeyNameClaimPaymentValid As String = "claim_payment_valid"
    Public Const PMKeyNameDecisionResult As String = "decision_result"



    Public Const PMFromClientCnt As String = "FromClientCnt"
    Public Const PMToClientCnt As String = "ToClientCnt"
    Public Const PMFromClientCode As String = "FromClientCode"
    Public Const PMToClientCode As String = "ToClientCode"

    Public Const PMKeyNameRoundOffAmount As String = "round_off_amount"

    'Adding constants for passing and getting values from renewal process
    Public Const PMKeyNamePolicyRenewalStatus As String = "PolicyRenewalStatus"
    Public Const PMKeyNamePolicyMakeLiveStaus As String = "PolicyMakeLiveStaus"

    Public Const PMKeyNameRenewalProcessMode As String = "RenewalProcessMode"

    Public Const PMKeyNameCurrencyKey As String = "CurrencyKey"
    Public Const PMKeyNameMultiCurrencyFlag As String = "MultiCurrencyFlag"
    Public Const PMKeynameFormlessInterface As String = "FormlessInterface"

    Public Const PMKeyNameCaseNumber = "Case Number"
    Public Const ACTKeyNameCaseNumber As String = "Case Number"
    Public Const PMKeyNameShowAllClaimVersionEvents As String = "Show All Claim Versions"

    Public Const kPMKeyAllocationCallingAppName As String = "AllocationCallingAppName"
    Public Const kPMKeyNameAllocationViewMode As String = "AllocationViewMode"
    Public Const kACTKeyNameBatchID As String = "Batch_Id"
    Public Const kPMKeyNameInsurerPaymentRoadMap = " Insurer Payment Road Map"
    Public Const kACTKeyNameTaxBandId = "tax_band_id"
    Public Const kACTKeyNameTaxAmount = "tax_amount"

    Public Const PMKeyNameCashlistRef = "cashlist_ref"
    Public Const ACTKeyNameAgentCnt As String = "Agent_Cnt"
    Public Const kACTKeyNameCallingComponent As String ="Calling_Component"
End Module
