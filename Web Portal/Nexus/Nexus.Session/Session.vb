Imports System.Web
Imports System.Web.HttpContext

Public Module Session

    'Everything session related. ALL CONSTANTS SHOULD START FROM "CN"
    Public Const CNInsuranceFileDetails As String = "InsuranceFileDetails"
    Public Const CNAgentDetails As String = "AGENT_DETAILS" 'Object returned from a SAM login, contains all the agents details
    Public Const CNLoginType As String = "LOGIN_TYPE" 'Holds the kind of user logged, agent or customer
    Public Const CNLoginName As String = "LoginName" 'Holds the current logged in username 
    Public Const CNIsAgent As String = "IsAgent" 'Hold the customer type for forgotten password functionality, if not agent user send forgotten password mail to the user.
    Public Const CNClientSearchCriteria As String = "CLIENT_SEARCH_CRITERIA" ' THE CURRENT SEARCH FIELDS LIKE CLIENT CODES ETC. (ONLY BE IN CASE OF AGENT)
    Public Const CNUserIdentityName As String = "UserIdentityName" 'This is the HttpContext.Current.User.Identity.Name
    Public Const CNQuote As String = "QUOTE" 'Holds the current quote being view/editted
    Public Const CNParty As String = "PARTY" 'Holds the current party being view/editted
    Public Const CNUserName As String = "UserName" 'Holds the current party username being view/editted
    Public Const CNIsNewClient As String = "IS_NEW_CLIENT" 'Holds the status whether client has been created recently or not
    Public Const CNEncryptedSessionKey As String = "EncryptedSession" 'Holds the hashkey to prevent Session Hijacking
    Public Const CNIsAnonymous As String = "IS_ANONYMOUS" ' To check whether the client is added as Anonymous at Quick Quote
    Public Const CNDataModelCode As String = "DATA_MODEL_CODE" 'Current data model code in use
    Public Const CNPaid As String = "PAID"
    Public Const CNStatementsAgreed As String = "STATEMENTS_AGREED" 'indicates that user has agreed to important statements
    Public Const CNBranchCode As String = "BRANCH_CODES"
    Public Const CNPartyDataModelCode As String = "PARTY_DATA_MODEL_CODE"
    Public Const CNTransBranchCode As String = "TRANS_BRANCH_CODE" ' holds current transaction branch code if user has multibranch access
    Public Const CNRedirectedFor As String = "ANONQUOTE_REDIRECTED_FOR" ' hold from "SaveQuote" or "BuyQuote" for anonymous
    Public Const CNIsTransferQuoteRequired As String = "IS_TRANSFER_QUOTE_REQUIRED" ' If we have added a new party for anonymous quote but quote is not transferred yet as AutoSave is false. Then value will be true for this boolean flag.
    Public Const CNPolicyAllTaxesColl As String = "PolicyAllTaxesColl" 'holds the ALL TAXES Records
    Public Const CNRefreshRI As String = "RefreshRI" 'To Refresh RI when edit Rating section details
    Public Const CNRequestedUrl As String = "RequestedUrl" 'To hold the requested url. before redirecting for Authentication.

    '---------Used to Store Modal controls (Personal) -----------
    Public Const CNPCAddresses As String = "PersonalClientAddresses" 'Holds the current address being view/editted
    Public Const CNPCContact As String = "PersonalClientContact" 'Holds the current Contacts being view/editted
    Public Const CNPCAssociate As String = "PersonalClientAssociate" 'Holds the current Associate being view/editted
    Public Const CNPCConviction As String = "PersonalClientConviction" 'Holds the current conviction being view/editted
    Public Const CNPCLifestyle As String = "PersonalClientLifestyle" 'Holds the current lifestyle being view/editted
    Public Const CNPCLoyalty As String = "PersonalClientLoyalty" 'Holds the current loyalty being view/editted
    Public Const CNPCProspectPolicy As String = "PersonalClientProspectPolicy" 'Holds the current prospect policy being view/editted


    '---------Used to Store Modal controls (Corporate) -----------
    Public Const CNCCAddresses As String = "CorporateClientAddresses" 'Holds the current address being view/editted
    Public Const CNCCContact As String = "CorporateClientContact" 'Holds the current Contacts being view/editted
    Public Const CNCCAssociate As String = "CorporateClientAssociate" 'Holds the current Associate being view/editted
    Public Const CNCCConviction As String = "CorporateClientConviction" 'Holds the current conviction being view/editted
    Public Const CNCCLifestyle As String = "CorporateClientLifestyle" 'Holds the current lifestyle being view/editted
    Public Const CNCCLoyalty As String = "CorporateClientLoyalty" 'Holds the current loyalty being view/editted
    Public Const CNCCProspectPolicy As String = "CorporateClientProspectPolicy" 'Holds the current prospect policy being view/editted

    '---------Used to Store Modal controls  -----------
    Public Const CNAddresses As String = "Addresses" 'Holds the current address being view/editted
    Public Const CNContact As String = "Contact" 'Holds the current Contacts being view/editted
    Public Const CNAssociate As String = "Associate" 'Holds the current Associate being view/editted
    Public Const CNConviction As String = "Conviction" 'Holds the current conviction being view/editted
    Public Const CNLifestyle As String = "Lifestyle" 'Holds the current lifestyle being view/editted
    Public Const CNLoyalty As String = "Loyalty" 'Holds the current loyalty being view/editted
    Public Const CNProspectPolicy As String = "ProspectPolicy" 'Holds the current prospect policy being view/editted
    Public Const CNClientType As String = "CLIENT_TYPE" ' storing the personal / corporate client type

    Public Const CNSubAgents As String = "POLICY_SUBAGENTS"

    '---------Used for Page Navigation (WorkManager) -----------


    Public Const CNWMMode As String = "WM_MODE" 'Holds the session of current page being viewed
    Public Const CNWMWorkManagerCollection As String = "WorkManagerCollection" 'Holds the collection of the Response type of the GetWorkMamagerSheduledtasks
    Public Const CNWMTaskLogCollection As String = "TaskLogCollection"
    Public Const CNWMTaskInstanceKey As String = "TaskInstanceKey"
    Public Const CNRequestType As String = "RequestType"

    '----Used to link risk screens together------------
    Public Const CNOI As String = "OI_LIST"
    Public Const CNTabState As String = "TAB_STATE"
    Public Const CNRiskProgress As String = "RISK_PROGRESS"
    Public Const CNNode As String = "NODE_LIST" 'Will be used to store the Current Node collection during child/grand child edit/add
    Public Const CNDeletedNode As String = "DELETED_NODE_LIST" 'Will be used to store the Deleted Current Node collection during child/grand child edit/add
    '----Used in FindParty to carry the result------------
    Public Const CNResultSet As String = "RESULT_SET"
    '--------------------------------------------------
    Public Const CNCurrentPageNumber As String = "CURRENT_PAGE_NUMBER"
    Public Const CNHighestPageNumber As String = "HIGHEST_PAGE_NUMBER"
    Public Const CNReserveDescriptions As String = "CL_CLAIM_RESERVE_DESCRIPTIONS"

    Public Const CNPFUseTransCurrency As String = "PF_USE_TRAN_CURRENCY"
    Public Const CNPFUserAuthorityValue As String = "PF_USER_AUTHORITY_VALUE"
    Public Const CNAmountToPay As String = "AMOUNT_TO_PAY"
    Public Const CNCashList As String = "CASHLIST"
    Public Const CNMode As String = "MODE" 'Storing the mode of the current policy, relates to the mode in the constants e.g add, edit, view
    Public Const CNRiskMode As String = "RISK_MODE" 'Storing the mode of the current risk, see enum RiskMode
    Public Const CNQuoteMode As String = "QUOTE_MODE" 'Storing the mode of the current quote, see enum QuoteMode
    Public Const CNClientMode As String = "CLIENT_MODE" ' Storing the mode of the current selected client i.e. add, edit, view
    Public Const CNReturnURL As String = "ReturnUrl" 'Storing the return URL of the current page.

    Public Const CNQuoteInSync As String = "QUOTE_IN_SYNC"

    Public Const CNFinalScreenCode As String = "FINAL_SCREEN_CODE"

    Public Const CNCardType As String = "CARD_TYPE"
    Public Const CNSelectedDocument As String = "SELECTED_DOCUMENT" ' THE DOCUMENT THAT HAS BEEN SELECTED BY THE CLIENT        
    Public Const CNSelectedPaymentIndex As String = "SELECTED_PAYMENT_INDEX"
    Public Const CNInsuranceFileKey As String = "INSURANCE_FILE_KEY"
    Public Const CNOriginalInsuranceFileKey As String = "ORIGINAL_INSURANCE_FILE_KEY"
    Public Const CNChargetoPay As String = "CHARGE"
    Public Const CNAnonymousUser As String = "ANONYMOUS"
    Public Const CNCollection As String = "POLICYCOLLECTION" '
    Public Const CNRiskDuplicateClientCheck As String = "RISK_DUPLICATECLIENTCHECK"
    Public Const CNMTAType As String = "MTA_TYPE" '  HOLDS THE KIND OF MTA Selected, Temporary,Permanent or Cancelation 
    Public Const CNOriginalMTAType As String = "MTA_ORIGINAL_TYPE"
    Public Const CNPayment As String = "PAYMENT_DETAILS" 'Holds the current payments details being view/editted
    Public Const CNPayNowReceipt As String = "PAYNOW_RECEIPT" 'Holds the current payments receipt details being view/editted
    Public Const CNCashListItem As String = "PAYNOW_CASHLIST" 'Holds the screen flow code 
    Public Const CNPolicy_Summary As String = "POLICY_SUMMARY"
    Public Const CNCurrenyCode As String = "CURRENYCODE" ' holds the information for the Selected Currency for the Product
    Public Const CNIsSummaryVisited As String = "IS_SUMMARYVISITED" ' holds the information true/false if SummaryofCoverPage has been visited 
    Public Const CNIsTransactionConfirmationVisited As String = "IS_TransactionConfirmation" ' holds the information true/false if TransactionConfirmationPage has been visited 
    Public Const CNMtaReasonSelected As String = "MTAREASON_SELECTED" ' holds the MTAReason selected value if MTAReasonPage has been visited 
    Public Const CNOldPremium As String = "OLD_PREMIUM" ' Holds the Old Premium in case of MTA 
    Public Const CNCurrentRiskKey As String = "CURRENT_RISK_INDEX" 'Holds the Risk Index for the selected Risk
    Public Const CNAgentType As String = "AGENT_TYPE" 'Holds the Type of the agent selected during NB
    Public Const CNAgentComm As String = "AGENT_COMM" 'Holds the Agent Commission to be deducted on later stage
    Public Const CNIsTrueMonthlyPolicy As String = "IS_TRUEMONTHLYPOLICY" 'Holds the information true/false as per product option
    Public Const CNIsUnifiedRenewalDayReadOnly As String = "IS_UNIFIEDRENEWALDAYREADONLY" 'Holds the information true/false as per Product Risk Option. It will only be true for Product Risks that are setup as True Monthly Policies.

    Public Const CNCashListItemWithTransDetailKey As String = "PAYNOW_CASHLIST_WITH_TRANSDETAILKEY"
    Public Const CNCashListItemAllocationStatus As String = "CASHLISTITEM_ALLOCATION_STATUS"
    Public Const CNMultipleAllocation As String = "CASHLISTITEM_MULTIPLE_ALLOCATION"
    '------------------------- Claims DataSet -------------------
    Public Const CNLossToDate As String = "CLAIM_LOSSTODATE"
    Public Const CNClaimStatus As String = "CLAIM_STATUS"
    Public Const CNClaimStatusDate As String = "CLAIM_STATUSDATE"
    Public Const CNEditClaim As String = "EDIT_CLAIM"
    Public Const CNSearchResults As String = "CL_CLAIM_SEARCH_RESULTS"
    Public Const CNSearchCriteria As String = "CL_CLAIM_SEARCH_CRITERIA"
    Public Const CNSearchPaymentAuthorization As String = "CL_PAYMENT_AUTHORIZATION_SEARCH_CRITERIA"
    Public Const CNPageSize As String = "CL_RESULTS_PAGE_SIZE"
    Public Const CNClaimVersion As String = "CLAIM_VERSION"
    Public Const CNClaimFlag As String = "CLAIM_FLAG"
    Public Const CNClaimShortName As String = "CLAIM_SHORT_NAME"
    Public Const CNFindClaimFlag As String = "CLAIM_FIND_CLAIM_FLAG"
    Public Const CNClaimPolicy As String = "CLAIM_POLICY_NUMBER"
    Public Const CNRecoveryDescriptions As String = "CLAIM_RECOVERY_ITEM"
    Public Const CNClaimQuote As String = "CLAIM_QUOTE" 'Holds the current quote being view/editted
    Public Const CNRecovery As String = "CLAIM_RECOVERY"
    Public Const CNAAAddresses As String = "AAAddresses"
    Public Const CNCAAddresses As String = "CAAddresses"
    Public Const CNRunClaimWorkFlow As String = "RunCalimWorkFlow"
    Public Const CNAuthorizeStatus As String = "AuthorizeStatus"
    Public Const CNEnablePayClaim As String = "EnablePayClaim"
    Public Const CNChangeReason As String = "ChangeReason"
    Public Const CNDirtyPeril As String = "DirtyPeril"
    Public Const CNInitialClaim As String = "InitialClaim"
    Public Const CNMaxClaimPaymentValue As String = "MaxClaimPaymentValue"
    Public Const CNLapseDate As String = "LapseDate"
    Public Const CNInsuranceFileStatus As String = "InsuranceFileStatus"
    Public Const CNInsuranceFolderKey As String = "InsuranceFolderKey"
    Public Const CNDisplayValidVersion As String = "DisplayValidVersion"
    Public Const CNMAXUNAUTHORISEDCLAIMVALUE As String = "MAX_UNAUTHORISED_CLAIM_VALUE"
    '------------------- Risks / Perils DataSet -----------------

    Public Const CNPolicyNumber As String = "POLICY_NUMBER"
    Public Const CNPartyKey As String = "PARTY_KEY"
    Public Const CNLossDate As String = "LOSS_DATE"
    Public Const CNClaimNumber As String = "CLAIM_NUMBER"
    Public Const CNClaimKey As String = "CLAIM_KEY"
    Public Const CNBaseClaimKey As String = "BASE_CLAIM_KEY"
    Public Const CNClaimPerilKey As String = "CLAIM_PERIL_KEY"
    Public Const CNClaimPerilIndex As String = "CLAIM_PERIL_INDEX"
    Public Const CNClaimMultiPerilIndex As String = "CLAIM_MULTI_PERILS_INDEX"
    Public Const CNRenewalDate As String = "RENEWAL_DATE"
    Public Const CNAddress As String = "POLICY_ADDRESS"
    Public Const CNAgentCommission As String = "AGENT_COMMISSION"
    Public Const CNClaim As String = "CLAIM"
    Public Const CNClaimDetails As String = "CLAIM_DETAILS"
    Public Const CNRisks As String = "POLICY_RISKS"
    Public Const CNClaimTimeStamp As String = "CLAIM_TIMESTAMP"
    Public Const CNClaimRiskTimeStamp As String = "CLAIM_RISK_TIMESTAMP"
    Public Const CNPointOfNoReturn As String = "CLAIM_POINT_OF_NO_RETURN"
    Public Const CNReserveInitialValues As String = "RESERVE_INITIAL_VALUE"
    Public Const CNReserveRevised As String = "RESERVE_REVISED"
    Public Const CNOriginalClaim As String = "ORIGINAL_CLAIM"
    Public Const CNScreenCode As String = "CL_ClAIM_SCREEN_CODE"
    Public Const CNRiskType As String = "RISK_TYPE"
    Public Const CNPartyType As String = "PartyType"
    Public Const CNTransDeatilsKeys As String = "TRANSDETAIL_KEY" ' For getting Transactions
    Public Const CNTransdetailKeyfromCashList As String = "TRANSDETAIL_KEY_Cash_List" ' For getting Transactions Cashlist
    Public Const CNPayClaim As String = "CLAIM_PAYMENT"
    Public Const CNPayClaimError As String = "CLAIM_PAYMENT_ERROR" ' Hold the Error return by PayClaim for XOL
    Public Const CNSalvageClaim As String = "CLAIM_SALVAGE"
    Public Const CNProductCode As String = "PRODUCT_CODE"
    Public Const CNClaimBuilder As String = "CLAIM_BUILDER" 'Check the hidden product option of the Claim Builder
    Public Const CNPerilReturnURL As String = "PERIL_RETURN_URL" 'holds the page address from where peril page invoked
    Public Const CNLockPaymentGrid As String = "LOCK_PAYMENT_GRID" 'hold the status whether Payment is locked or not
    Public Const CNCaseKey As String = "CASE_KEY" 'hold the case key to attach with open claim
    Public Const CNBaseCaseKey As String = "BASE_CASE_KEY" 'holds the base case key
    Public Const CNIsClaimLocked As String = "IS_CLAIM_LOCKED" ' holds the information true/false if claim locked by another User
    Public Const CNClaimCallsTimeStamp As String = "CLAIM_Calls_TIMESTAMP"
    Public Const CNCommissionWarning As String = "COMMISSION_WARNING"
    Public Const CNPayee As String = "PAYEE_OPTION"
    '------------------ DataSet Container ------------------------
    ' Use CNDataSet session object to hold both Claim & Quote Risk object.
    Public Const CNDataSet As String = "DATASET"
    Public Const CNTempClaimDataSet As String = "TempClaimDataSet"
    '--------------- Header --------------

    Public Const CNStatus As String = "CL_STATUS"
    Public Const CNCurrency As String = "CL_CURRENCY"
    Public Const CNDate_Header As String = "CL_HEADER_DATE"
    Public Const CNOrigin_Header As String = "CL_HEADER_ORIGIN"
    Public Const CNInsurer_Header As String = "CL_HEADER_INSURER"
    Public Const CNAgentName_Header As String = "CL_HEADER_AGENTNAME"
    Public Const CNAgentContact_Header As String = "CL_HEADER_AGENTCONTACT"

    '----------------------  Search criteria ------------------------------
    Public Const CNSearchType As String = "SEARCHTYPE"
    Public Const CNSearchOtherPartyTypeCode As String = "SEARCH_OTHERPARTYTYPECODE"
    Public Const CNSearchAgentType As String = "SEARCH_AGENTTYPE"
    Public Const CNSearchData As String = "SEARCHDATA"
    Public Const CNClaimsSearchData As String = "CLAIMSSEARCHDATA"
    Public Const CNSearchSubAgentData As String = "SEARCHSADATA"
    Public Const CNCoverNoteSheetData As String = "CoverNoteSheetData"
    Public Const CNFindPolicyResults As String = "FindPolicyResults" 'Holds the results of a call to FindPolicy

    '----------------------  Collections ------------------------------
    Public Const CNUnallocatedCreditsForAgents As String = "UNALLOCATEDCREDITSFORAGENTS" ' holds the UnallocatedCreditsForAgents records in the PrePayment.aspx page to avoid the multiple SAM calls
    Public Const CNUnallocatedCreditsForClients As String = "UNALLOCATEDCREDITSFORCLIENTS" ' holds the UnallocatedCreditsForClients records in the PrePayment.aspx page to avoid the multiple SAM calls
    Public Const CNUnAllocatedClaimPayment As String = "UnAllocatedClaimPayment"
    Public Const CNProduceDocument As String = "ProduceDocument"
    Public Const CNPaymentMode As String = "PaymentMode"
    Public Const CNReceiptMode As String = "ReceiptMode"
    Public Const CNRatingSections As String = "RatingSections"



    '----------------Renewal-----------------------------
    Public Const CNRenewal As String = "RENEWAL_PROCESS" 'HOLDS THE PROCESS OF RENEWAL IN THE SESSION
    Public Const CNRenewalShowPremium As String = "RENEWAL_SHOW_PREMIUM" 'HOLDS THE STATUS OF RENEWAL IN THE SESSION"
    Public Const CNInstalmentsPlan As String = "CNInstalmentsPlan" 'HOLDS THE STATUS OF RENEWAL IN THE SESSION"
    Public Const CNMtaRenSelResponse As String = "MTA_REN_SEL_RESPONSE"

    '---------Used for Page Navigation (WorkManager) -----------
    Public Const CNAddEvent As String = "ADDEVENT" ' Holds the Newly Added Event in the session.
    Public Const CNEvent As String = "Events" '
    Public Const CNAuditTrail As String = "AuditTrailEvents"
    Public Const CNCoverMode As String = "Mode" ' Holds the value in ViewState for CoverNote processing
    Public Const CNTimeStamp As String = "TimeStamp"    ' Holds the value of time stamp for Covernote processing
    Public Const CNCoverNoteCollection As String = "CoverNoteCollection"

    Public Const CNSearchAccountResult As String = "SearchAccountResult" ' Holds the Search Result of Account 
    Public Const CNSearchBankResult As String = "SearchBankResult" ' Holds the search result of Bank 

    ' Added variables for Start and End number for CoverNote
    Public Const CNStartNumber As String = "StartNumber" 'Hold the start number of the Cover Note
    Public Const CNEndNumber As String = "EndNumber" 'Hold the start number of the Cover Note
    ' Added variables for Start and End number for CoverNote
    Public Const CNCoInsurancePage As String = "COINSURANCE_PAGE_VISITED" 'Hold the status whether user has visited the Coinsurance page


    ' Session name constants for insurer payments
    Public Const CNInsSearchCriteria As String = "InsSearchCriteria"
    Public Const CNDocumentRef As String = "DocumentRef"
    Public Const CNAccountSearchResults As String = "AccountSearchResults"
    Public Const CNManupulatedGridResult As String = "ManupulatedGridResult"
    Public Const CNOutStandingGridResult As String = "OutstandingGridResult"
    ' Stores the list of TransdetailID which are marked
    Public Const CNMarkedTransDetailList As String = "MarkedTransDetailList"
    ' Stores the account key from Insurer payment page
    Public Const CNAccountkey As String = "AccountKey"
    ' Stores the value of total marked amount
    Public Const CNTotalAmount As String = "TotalMarkedAmount"
    ' Stores the Value of total write off amount
    Public Const CNTotalWriteOffAmount As String = "TotalWriteOffAmount"
    'Stores the value for Marked Amount '
    Public Const CNMarkedAmountSignForCashList As String = "MarkedAmountSignForCashList"
    'stores the value for write off amount
    Public Const CNWriteOffAmount As String = "WriteOffAmount"
    ' Stores Account Shortname 
    Public Const CNAccountName As String = "AccountName"
    ' Stores Parent Page
    Public Const CNParentPage As String = "ParentPage"
    Public Const CNCashListReceipt As String = "MEDIA_TYPE_STATUS"
    Public Const CNCurrentMode As String = "CurrentMode"
    Public Const CNCurrentOI As String = "CurrentOI"
    Public Const CNCurrentOIItem As String = "CurrentOIItem"
    Public Const CNCommissionGreaterThanPremium As String = "Commission_Greater_Premium"
    Public Const CNReciptAmountEntered As String = "ReciptAmountEntered"

    'Quote collection session variables
    Public Const CNQuoteCollectionSearchResults As String = "QuoteCollectionSearchResults" 'stores search results on Quote Collection page
    Public Const CNQuoteCollectionFiles As String = "QuoteCollectionFiles" 'stores an array list of insurance file keys corresponding to the quotes selected on quotecollection.aspx
    Public Const CNTotalForQuoteCollection As String = "TotalForQuoteCollection" 'stores total value of quotes selected for quote collection
    Public Const CNPolicySummaryCollection As String = "PolicySummaryCollection in QuoteCollection" 'stores total Policies made live during quote collection

    'Session constants for Manual journalTransactionList
    Public Const CNSearchManualJournalTransactions As String = "MJ_TRANSACTIONS"
    Public Const CNAuthoriseManualJournalTransactions As String = "AUTH_MJTRANSACTIONS"

    'Session constants for ManualJournalItem Page
    Public Const CNAmount As String = "Amount"
    Public Const CNAltReference As String = "AltReference"
    Public Const CNUnderwritingYearCode As String = "UnderwritingYearCode"
    Public Const CNUnderwritingYearDescription As String = "UnderwritingYearDescription"
    Public Const CNCurrencyRate As String = "CurrencyRate"
    Public Const CNBaseAmount As String = "BaseAmount"
    Public Const CNComment As String = "Comment"
    Public Const CNCostCentreCode As String = "CostCentreCode"
    Public Const CNCostCentreDescription As String = "CostCentreDescription"
    Public Const CNInsuranceRef As String = "InsuranceRef"
    Public Const CNPurchaseOrderNumber As String = "PurchaseOrderNumber"
    Public Const CNPurchaseInvoiceNumber As String = "PurchaseInvoiceNumber"
    Public Const CNManualJournalItemCollection As String = "ManualJournalItemCollection"
    Public Const CNManualJournalAuthItemCollection As String = "ManualJournalItemAuthCollection"
    Public Const CNManualJournal As String = "ManualJournal"
    Public Const CNIsBackDatedMTA = "IsBackDatedMTA" 'to store a flag for back dated mta
    Public Const CNBackDatedVersions = "BackDatedVersions" 'to store backdated versions
    Public Const CNEditStandardWordingsTemplate As String = "EditStandardWordingsTemplate"
    Public Const CNCheckSalvageRecovery As String = "CheckSalvageRecovery" 'store the salvage recovery flag set at product level on claim workflow tab
    Public Const CNCheckTPRecovery As String = "CheckTPRecovery" 'store the third party recovery flag set at product level on claim workflow tab
    Public Const CNClaimPaymentKey As String = "ClaimPaymentKey" 'store the claimpaymentkey 
    Public Const CNCurrentDocumentCollection As String = "CurrentDocumentCollection"  'stores the document collection which may contain documents to generate, editted documents and uploaded documents
    Public Const CNRiskStandardWordingsTemplate As String = "EditRiskStandardWordingsTemplate"
    Public Const CNPolicyStandardWordingsTemplate As String = "PolicyStandardWordingsTemplate"
    Public Const CNFreshPolicySW As String = "FreshPolicyStandardWording"
    Public Const CNTempOI As String = "TempOI" 'To save temprary OI created by Nexus for new element or edit child
    Public Const CNBaseInsuranceFileKey = "BaseInsuranceFileKeyForIntractiveMTA" ' To save baseinsurance file key for backdated MTA
    Public Const CNRenewalStatus As String = "CNRenewalStatus" 'To save the renewal status 
    Public Const CNIsInteractiveBackdatedMTA = "IsInteractiveBackdatedMTA" 'To Save a flag that MTA is in interactive mode
    Public Const CNTransCurr As String = "TransCurr" 'To save the currency selected on Insurer Payment page
    Public Const CNTransInMultiCurr As String = "TransInMultiCurr" 'To save "Yes" if transactions which are selected for payment are in defferent currencies o.w "No"
    Public Const CNDocCollection As String = "CNDocCollection" 'stores collection of documents stored in sharepoint 
    Public Const CNSecureGuid As String = "SecureGuid" 'stores a guid which must be passed back on any AJAX service call inorder to verify that call is coming from valid session
    Public Const CNViewType As String = "VIEW_TYPE" '  HOLDS THE KIND OF View Selected, Temporary,Permanent or Cancelation 
    Public Const CNRIXMLData As String = "ReinsuranceXMLData"
    Public Const CNRIXMLDataOriginal As String = "ReinsuranceXMLDataOriginal"
    Public Const CNRIFACXol As String = "ReinsuranceFACXol"
    Public Const CNRIFACXolTemp As String = "ReinsuranceFACXolFinal"
    Public Const CNRIArrangementkey As String = "ReinsuranceArrangementkey"
    Public Const CNRIBandKey As String = "ReinsuranceBandKey"
    Public Const CNRIModel As String = "ReinsuranceModel"
    Public Const CNRIFACProp As String = "ReinsuranceFACProp"
    Public Const CNRITransactionType As String = "Reinsurance Transaction details"
    Public Const CNJavaScript As String = "JS"
    Public Const CNHasPremiumUpdated As String = "HasPremiumUpdated"
    'Public Const CNIsUnifiedRenewalDayReadOnly As String = "IsUnifiedRenewalDayReadOnly" 'A flag to identify if Unified renewal day for this product is readonly
    Public Const CNClaimPaymentSummary As String = "ClaimPaymentSummary" ' holds claim payment summary after processing claim payments through settle all functionality

    Public Const CNShowPasswordExpiryWarning As String = "ShowPasswordExpiryWarning"
    Public Const CNAgentCommissions As String = "AgentCommissions"


    Public Const CNAgentSettings As String = "AgentSettings" ' holds the settings for an agent linked with quote / policy
    Public Const CNAnonymousClientType As String = "AnonymousClientType" 'stores the Client Type like PC for Personal Client and CC for Corporate Client
    Public Const CNInstalmentPlanMode As String = "InstalmentPlanMode" 'Storing the mode of the current Instalment Plan, relates to the mode in the constants e.g add or edit
    Public Const CNFinancePlanDetails As String = "FinancePlanDetails" 'Holds GetHeaderandSummariesPFPlanByKey SAM response
    Public Const CNFinancePlan As String = "FinancePlan" 'Hold GetFinancePlanDetails SAM response
    Public Const CNTransactionDetails As String = "TransactionDetails" 'Holds GetTransactionDetails SAM response
    Public Const CNFinancePlansCollection As String = "FinancePlans" 'Holds Finance Plans details
    Public Const CNPolicyAssociateCollection As String = "PolicyAssociateCollection" 'Holds Policy Associate Details
    Public Const CNMTAPlanType As String = "MTAPlanType"
    Public Const CNTaxGroupForClaims As String = "TAX_GROUP_FOR_CLAIMS"
    Public Const CNTaxOverridden As String = "TAX_Overridden"
    Public Const CNDebitTransDetailkey As String = "DebitTransDetailkey" ''Holds Trans Detail Key post settlement of a finance plan.
    Public Const CNRiskViewStartPoint As String = "RiskStartPoint" ''Holds the start point of view risk functionality
    Public Const CNDocumentToDownload As String = "DocumentToDownload"
    Public Const CNInstalmentDatesUpdated As String = "InstalmentDatesUpdated"
    Public Const CNInfoOnly As String = "InfoOnly" 'store for check claim which we want to edit is an provisional claim or not



    Public Const CNIsMarketPlacePolicy As String = "IsMarketPlacePolicy" 'hold the value for quote or policy created through market place 


    Public Const CNOnlyOriginalRating As String = "OnlyOriginalRating" 'For Add NEW Risk in MTA only Original rating details should be visible
    Public Const CNSelectedAccount As String = "SelectedAccount"

    Public Const CNRiskIndex As String = "CNRiskIndex"

    Public Const CNClaimPaymentCreatedBy As String = "ClaimPaymentCreatedBy" 'store the user name who created the payment
    Public Const CNBackDatedReinstatement As String = "BackDatedReinstatement" 'Key holding the backdated reinstatement indicator.
    Public Const CNTempOriginalMTAQuote As String = "TempOriginalMTAQuote"
    Public Const CNAgentCancelled As String = "AgentCancelled"
    Public Const CNIsLapsed As String = "CNIsLapsed" 'Check If the Policy Lapse
    Public Const CNReverseReceipt As String = "CNReverseReceipt"

    Public Const CNTemplateCode As String = "TemplateCode" 'Holds the template code for WPR10 Letter Writing
    Public Const CNNoTrans As String = "NoTrans" 'Holds value "NB", "CLAIM" And "MTA" for manual transfer
    Public Const CNIsCancelMTA As String = "IsCancelMTA" 'Holds boolean value to check whether cancelMTA button is clicked on premium display

    Public Const CNPartyBankDetail As String = "PartyBankDetail" 'Holds Party bank details for renewal installment

    Public Const CNSelectedSchemeNo As String = "SelectedSchemeNo" 'Holds scheme number selected at NB
    Public Const CNRIData As String = "ReinsuranceData"
    Public Const CNRIBands As String = "RiBands"
    Public Const CNFinancePlanStatus As String = "FinancePlanStatus"
    Public Const CNPaymentHubDetails As String = "CNPaymentHubDetails"
    Public Const CNCardDetails As String = "CREDITCARDDETAILS"
    Public Const CNInstalmentMediaType As String = "InstalmentMediaType"
    ' Stores Payment HUb config 
    Public Const CNPaymentHubConfig As String = "PaymentHubConfig"
    Public Const CNCashListCurrencyRates As String = "CashListCurrencyRates"
    Public Const CNCashListItemPending As String = "CNCashListItemPending"
    Public Const CNCurrencyConversion As String = "CurrencyConversion"

    <Serializable()>
    Public Class CashListCurrencyRates
        Public Property CurrencyBaseDate As DateTime
        Public Property CurrencyBaseXrate As Double
        Public Property AccountBaseDate As DateTime
        Public Property AccountBaseXrate As Double
        Public Property SystemBaseDate As DateTime
        Public Property SystemBaseXrate As Double
        Public Property OverrideReason As Integer
        Public Property BaseAmount As Decimal
    End Class

    <Serializable()>
    Public Class CurrencyConversion
        Public Property CurrencyBaseDate As DateTime
        Public Property CurrencyBaseXrate As Double
        Public Property AccountBaseDate As DateTime
        Public Property AccountBaseXrate As Double
        Public Property SystemBaseDate As DateTime
        Public Property SystemBaseXrate As Double
        Public Property OverrideReason As Integer
        Public Property BaseAmount As Decimal
    End Class

    'WorkManager Page - CR24 
    Public Const kWorkManagerFilter As String = "WorkManagerFilter"
    'Mode of ChildNode
    Public Const CNChildMode As String = "CNChildMode"
    Public Const CNDuplicateClaimPaymentReason As String = "DuplicateClaimPaymentReason"
    Public Const CNDuplicateClaimPayment As String = "DuplicateClaimPayment"

    Public Const CNQuoteAllProcessComplete As String = "QuoteAllProcessComplete"
    Public Const CNNEXTBUTTON As String = "NextButtonClicked"

    'Manage exiting form details for cliams and policy 
    Public Const CNFormFilds As String = "FormFilds"
    Public Const CNPolicyBackButton As String = "PolicyBackButton"
    Public Const CNPolicyBackFlag As String = "CNPolicyBackFlag"
    Public Const CNRiskBackFlag As String = "CNRiskBackFlag"
    Public Const CNFindPolicySearchData As String = "FindPolicySearchData"
    Public Const CNClaimBackButton As String = "ClaimBackButton"
    Public Const CNFindClaimSearchData As String = "FindClaimSearchData"
    Public Const CNDoNotClearSession = "DoNotClearSession"
    Public Const CNClaimOI As String = "CLAIMOI"


    Public Const CNFileUploadError As String = "FileUploadError"
    Public Const CNUserId As String = "UserId"

    ' Policy Discount
    Public Const CNPolicyDiscountEnabled As String = "POLICY_DISCOUNT_ENABLED"
    Public Const CNPolicyDiscountReasonId As String = "POLICY_DISCOUNT_REASON_ID"
    Public Const CNPolicyDiscountPercentage As String = "POLICY_DISCOUNT_PERCENTAGE"
    Public Const CNPolicyDiscountedPremium As String = "POLICY_DISCOUNTED_PREMIUM"
    Public Const CNPolicyDiscountTotalPremium As String = "POLICY_DISCOUNT_TOTAL_PREMIUM"
    Public Const CNPolicyDiscountRecurringTypeId As String = "POLICY_DISCOUNT_RECURRING_TYPE_ID"
    Public Const CNPolicyDiscountApplied As String = "POLICY_DISCOUNT_APPLIED"

    Public Sub ClearQuote()

        With HttpContext.Current.Session
            .Remove(CNDataModelCode)
            .Remove(CNQuoteInSync)
            .Remove(CNTabState)
            .Remove(CNRiskProgress)
            .Remove(CNQuote)
            .Remove(CNFinalScreenCode)
            .Remove(CNMode)
            .Remove(CNQuoteMode)
            .Remove(CNOI)
            .Remove(CNMTAType)
            .Remove(CNOriginalMTAType)
            .Remove(CNViewType)
            .Remove(CNIsSummaryVisited)
            .Remove(CNMtaReasonSelected)
            .Remove(CNViewType)
            ' .Remove(CNClaimsCollection)
            .Remove(CNClientSearchCriteria)
            .Remove(CNCurrenyCode)
            .Remove(CNOldPremium)
            .Remove(CNPaid)
            .Remove(CNPolicy_Summary)
            ' .Remove(CNShowChangePolicyBtn)
            .Remove(CNRenewal)
            .Remove(CNSalvageClaim)
            .Remove(CNSubAgents)
            .Remove(CNTransBranchCode)
            .Remove(CNCurrentMode)
            .Remove(CNProduceDocument)
            .Remove(CNPaymentMode)
            .Remove(CNReceiptMode)
            'Remove the Quote Collection Session Variables
            .Remove(CNQuoteCollectionFiles)
            .Remove(CNTotalForQuoteCollection)
            .Remove(CNPolicySummaryCollection)
            .Remove(CNAgentType)
            .Remove(CNAgentComm)
            .Remove(CNPayment)
            .Remove(CNPolicyNumber)
            .Remove(CNNode)
            .Remove(CNDeletedNode)
            .Remove(CNEditStandardWordingsTemplate)
            .Remove(CNIsNewClient)
            .Remove(CNIsBackDatedMTA)
            .Remove(CNBackDatedVersions)
            '.Remove(CNRiskType)
            .Remove(CNRiskStandardWordingsTemplate)
            .Remove(CNPolicyStandardWordingsTemplate)
            .Remove(CNFreshPolicySW)
            .Remove(CNTempOI)
            .Remove(CNIsInteractiveBackdatedMTA)
            .Remove(CNRenewalStatus)
            .Remove(CNIsTransferQuoteRequired)
            .Remove(CNClaimCallsTimeStamp)
            .Remove(CNIsTransferQuoteRequired)
            .Remove(CNIsAnonymous)
            .Remove(CNRiskDuplicateClientCheck)
            .Remove(CNPolicyAllTaxesColl)
            .Remove(CNRatingSections)
            .Remove(CNRIXMLData)
            .Remove(CNRIXMLDataOriginal)
            .Remove(CNRIData)
            .Remove(CNRIBands)
            .Remove(CNRiskMode)
            .Remove(CNIsTrueMonthlyPolicy)
            .Remove(CNHasPremiumUpdated)
            .Remove(CNIsUnifiedRenewalDayReadOnly)
            .Remove(CNRefreshRI)
            .Remove(CNTempOriginalMTAQuote)

            .Remove(CNBaseInsuranceFileKey)
            .Remove(CNBackDatedReinstatement)
            .Remove(CNCommissionWarning)
            .Remove(CNAgentCommissions)
            .Remove(CNIsMarketPlacePolicy)
            .Remove(CNCoInsurancePage)
            .Remove(CNInstalmentsPlan)
            .Remove(CNFinancePlan)
            .Remove(CNCurrentRiskKey)
            .Remove(CNRiskIndex)
            .Remove(CNInstalmentsPlan)
            .Remove(CNSelectedAccount)
            .Remove(CNAgentCancelled)
            .Remove(CNAnonymous)
            .Remove(CNStatementsAgreed)
            .Remove(CNInsuranceFolderKey)
            .Remove(CNPolicyAssociateCollection)
            .Remove(CNCardDetails)
            .Remove(CNInstalmentMediaType)
            .Remove(CNChildMode)

            ' Policy Discount
            .Remove(CNPolicyDiscountEnabled)
            .Remove(CNPolicyDiscountReasonId)
            .Remove(CNPolicyDiscountPercentage)
            .Remove(CNPolicyDiscountedPremium)
            .Remove(CNPolicyDiscountTotalPremium)
            .Remove(CNPolicyDiscountRecurringTypeId)
            .Remove(CNPolicyDiscountApplied)
            .Remove(CNPolicyDiscountApplied)
            .Remove("POLICY_DISCOUNT_APPLIED_TO_RISKS")
            .Remove("POLICY_DISCOUNT_ROLLED_BACK_FOR_NEW_RISK")
            .Remove("POLICY_DISCOUNT_ROLLED_BACK_FOR_EDIT_RISK")
            .Remove("POLICY_DISCOUNT_ORIGINAL_TOTAL")

            .Remove(CNTabState & "_ctrlTabIndex")
            .Remove(CNNEXTBUTTON)
        End With
        ClearRiskStandardWordingsSessionValues()

    End Sub

    Public Sub ClearClaims()

        Dim bClearPartyBuilderSession As Boolean = True

        If HttpContext.Current.Session(CNClientMode) IsNot Nothing Then
            If HttpContext.Current.Session(CNClientMode) = Mode.Edit Then
                bClearPartyBuilderSession = False
            End If
        End If

        With HttpContext.Current.Session
            .Remove(CNPartyKey)
            .Remove(CNLossDate)
            .Remove(CNClaimNumber)
            .Remove(CNClaimKey)
            .Remove(CNBaseClaimKey)
            .Remove(CNClaimPerilKey)
            .Remove(CNRenewalDate)
            .Remove(CNAddress)
            .Remove(CNClaim)
            .Remove(CNRisks)
            .Remove(CNClaimTimeStamp)
            .Remove(CNReserveDescriptions)
            .Remove(CNPointOfNoReturn)
            .Remove(CNOriginalClaim)
            .Remove(CNReserveRevised)
            .Remove(CNEditClaim)
            .Remove(CNClaimFlag)
            .Remove(CNClaimShortName)
            .Remove(CNClaimStatusDate)
            .Remove(CNClaimPolicy)
            .Remove(CNFindClaimFlag)
            .Remove(CNDataModelCode)
            If bClearPartyBuilderSession Then
                .Remove(CNOI)
            End If
            .Remove(CNClaimRiskTimeStamp)
            .Remove(CNClaimDetails)
            .Remove(CNClaimPerilIndex)
            .Remove(CNClaimMultiPerilIndex)
            .Remove(CNClaimVersion)
            .Remove(CNSalvageClaim)
            .Remove(CNDataSet)
            .Remove(CNAuthorizeStatus)
            .Remove(CNCurrentMode)
            .Remove(CNProduceDocument)
            .Remove(CNPaymentMode)
            .Remove(CNReceiptMode)
            .Remove(CNPayment)
            .Remove(CNEnablePayClaim)
            .Remove(CNPayClaim)
            .Remove(CNDirtyPeril)
            .Remove(CNInitialClaim)
            .Remove(CNIsNewClient)
            .Remove(CNCheckSalvageRecovery)
            .Remove(CNCheckTPRecovery)
            .Remove(CNClaimPaymentKey)
            .Remove(CNPayClaimError)
            .Remove(CNLockPaymentGrid)
            .Remove(CNClaimBuilder)
            .Remove(CNPerilReturnURL)
            .Remove(CNIsClaimLocked)
            .Remove(CNClaimCallsTimeStamp)
            .Remove(CNRIXMLData)
            .Remove(CNRIXMLDataOriginal)
            .Remove(CNRIData)
            .Remove(CNRIBands)
            .Remove(CNParentPage)
            .Remove(CNClaimPaymentSummary)
            .Remove("InsuranceFileDetailspageCacheID")
            .Remove(CNTabState & "_TabIndex")
            .Remove(CNClaimPaymentSummary)
            .Remove(CNTaxGroupForClaims)
            .Remove(CNTaxOverridden)
            .Remove(CNRiskProgress)
            .Remove(CNDuplicateClaimPaymentReason)
            .Remove(CNDuplicateClaimPayment)
        End With

    End Sub

    Public Sub ClearHeader()

        With HttpContext.Current.Session
            .Remove(CNStatus)
            .Remove(CNCurrency)
            .Remove(CNDate_Header)
            .Remove(CNOrigin_Header)
            .Remove(CNInsurer_Header)
            .Remove(CNAgentName_Header)
            .Remove(CNAgentContact_Header)
            .Remove(CNReturnURL)
            .Remove(CNCurrentMode)
            .Remove(CNProduceDocument)
            .Remove(CNPaymentMode)
            .Remove(CNReceiptMode)
            .Remove(CNPolicyAssociateCollection)
        End With

    End Sub

    Public Sub ClearSearch()

        With Current.Session
            .Remove(CNSearchResults)
            .Remove(CNSearchCriteria)

        End With

    End Sub

    Public Sub ClearCase()
        With HttpContext.Current.Session
            .Remove(CNCaseKey)
            .Remove(CNBaseCaseKey)
        End With
    End Sub
    Public Sub ClearWorkManager()
        With HttpContext.Current.Session
            .Remove(CNWMMode)
            .Remove(CNWMTaskLogCollection)
            .Remove(CNWMWorkManagerCollection)
            .Remove(CNWMTaskInstanceKey)
        End With
    End Sub
    Public Sub ClearInstalment()
        With HttpContext.Current.Session
            .Remove(CNInstalmentPlanMode)
            .Remove(CNFinancePlanDetails)
            .Remove(CNTransactionDetails)
            .Remove(CNFinancePlansCollection)
            .Remove(CNMTAPlanType)
        End With

    End Sub
#Region " CLEAR QuoteCollection SESSION VALUE "

    ''' <summary>
    ''' Clear QuoteCollection SessionValues .
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ClearQuoteCollectionSessionValues()
        With HttpContext.Current.Session
            .Remove(CNQuoteCollectionFiles)
            .Remove(CNTotalForQuoteCollection)
            .Remove(CNPolicySummaryCollection)
        End With
    End Sub

    Public Sub ClearRiskStandardWordingsSessionValues()
        With HttpContext.Current.Session
            Dim lstRiskStandardWordingSessions As New List(Of String)

            For Each sSessionVariable As String In .Keys
                If sSessionVariable.StartsWith(CNRiskStandardWordingsTemplate) Then
                    lstRiskStandardWordingSessions.Add(sSessionVariable)
                End If
            Next

            For Each sSession As String In lstRiskStandardWordingSessions
                .Remove(sSession)
            Next
            lstRiskStandardWordingSessions = Nothing
        End With
    End Sub

    Public Sub ClearTemporarySessionValues()
        With HttpContext.Current.Session
            .Remove(CNClaimOI)
            .Remove(CNDoNotClearSession)
        End With
    End Sub
#End Region
End Module



