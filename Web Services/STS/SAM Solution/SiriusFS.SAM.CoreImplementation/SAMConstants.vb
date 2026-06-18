'Namespace SiriusFS.SAM.CoreImplementation

Option Strict On

Public Enum InstalmentsMode
    NewBusinessOrRenewals = 3
    MidTermAdjustment = 4
    ThirdParty = 5
End Enum

Public Enum SAMGetAndValidateListItemFromCodeReturnValues
    ListNotFound = -2
    ListItemNotFound = -1
End Enum

Public NotInheritable Class RenewalEmailType
    Public Const Invite As String = "invitation"
    Public Const Selection As String = "selection"
    Public Const Update As String = "acceptance"
End Class

Public NotInheritable Class InstalmentHistoryActionType
    Public Const Setup As String = "Setup"
    Public Const Amendment As String = "Amendment"
    Public Const Cancellation As String = "Cancellation"
End Class

Public NotInheritable Class RenewalStatusTypeCodes
    Public Const AwaitingManualReview As String = "ManReview"
    Public Const AwaitingRenewalNoticePrint As String = "AutoReview"
    Public Const AwaitingManualRatingDueToFailure As String = "AutoFailed"
    Public Const PolicyDetailsChanged As String = "PolicyChan"
    Public Const AwaitingUpdate As String = "Update"
    Public Const AwaitingManualRating As String = "ManRating"
    Public Const AwaitingBrokerTransfer As String = "BROKERXFER"
    'Start - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.10)
    Public Const AwaitingUpdateWritten As String = "Written"
    'End -  (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.10)
End Class

Public NotInheritable Class PMLookupTable
    Public Const AddressUsageType As String = "Address_Usage_Type"
    Public Const AllocationStatus As String = "AllocationStatus"
    Public Const AnalysisCode As String = "Analysis_Code"
    Public Const Area As String = "Area"
    Public Const Bank As String = "Bank"
    Public Const Bank_Guarantee As String = "Bank_Guarantee"
    Public Const BankAccount As String = "BankAccount"
    Public Const BankGuarantee As String = "bank_guarantee"
    Public Const BankPaymentType As String = "Bank_Payment_Type"
    Public Const BG_Status As String = "BG_Status"
    Public Const CashList As String = "CashList"
    Public Const CashListItem As String = "CashListItem"
    Public Const CashListItemBank As String = "CashListItem_Bank"
    Public Const CashListItemPaymentStatus As String = "CashListItem_Payment_Status"
    Public Const CashListItemPaymentType As String = "CashListItem_Payment_Type"
    Public Const CashListItemReceiptStatus As String = "CashListItem_Receipt_Status"
    Public Const CashListItemReceiptType As String = "CashListItem_Receipt_Type"
    Public Const CashListStatus As String = "CashListStatus"
    Public Const CashListType As String = "CashListType"
    Public Const CatastropheCode As String = "Catastrophe_Code"
    Public Const ChequeClearingType As String = "Cheque_Clearing_Type"
    Public Const ChequeType As String = "ChequeType"
    Public Const ClaimStatus As String = "Claim_Status"
    Public Const ClassOfBusiness As String = "Class_of_Business"
    Public Const CommissionBand As String = "Commission_band"
    Public Const ContactType As String = "Contact_Type"
    Public Const CostCentre As String = "CostCentre"
    Public Const Country As String = "Country"
    Public Const Currency As String = "Currency"
    Public Const DocumentTemplate As String = "Document_Template"
    Public Const DocumentType As String = "document_type"
    Public Const DocumentTypes As String = "Documenttype"
    Public Const DriverStatus As String = "Driver_Status"
    Public Const EarningPattern As String = "Earning_Pattern"
    Public Const Exchange_Rate_Override_Reason As String = "Exchange_Rate_Override_Reason"
    Public Const GisDataModel As String = "Gis_Data_Model"
    Public Const Handler As String = "Handler"
    Public Const Insurance_file As String = "Insurance_file"
    Public Const LapsedReason As String = "lapsed_reason"
    Public Const ledger As String = "ledger"
    Public Const LicenseType As String = "License_Type"
    Public Const LifestyleCategory As String = "lifestyle_category"
    Public Const LoyaltyScheme As String = "Loyalty_Scheme"
    Public Const MediaType As String = "MediaType"
    Public Const MediaTypeIssuer As String = "MediaType_Issuer"
    Public Const MediaTypePayment As String = "MediaType_Payment"
    Public Const MediaTypeReceipt As String = "MediaType_Receipt"
    Public Const MediaTypeStatus As String = "MediaType_Status" 'Sankar - (WPRvb64 Media Type Status) - Paralleling
    Public Const MediaTypeValidationType As String = "MediaType_Validation"
    Public Const MTAEventDescription As String = "MTA_Event_Description"
    Public Const MTAReason As String = "mta_reason"
    Public Const Party As String = "Party"
    Public Const PartyType As String = "Party_Type"
    Public Const PerilType As String = "Peril_Type"
    Public Const PolicyStatus As String = "Policy_Status"
    Public Const PrimaryCause As String = "Primary_Cause"
    Public Const Product As String = "Product"
    Public Const ProgressStatus As String = "Progress_Status"
    Public Const RateType As String = "rate_type"
    Public Const RatingSectionType As String = "rating_section_type"
    Public Const RecoveryType As String = "Recovery_Type"
    Public Const RelationshipType As String = "Relationship_Type"
    Public Const ReminderType As String = "Reminder_Type"
    Public Const RenewalStatusType As String = "Renewal_Status_Type"
    Public Const RenewalStopCode As String = "Renewal_stop_code"
    Public Const RIBand As String = "RI_Band"
    Public Const RIModel As String = "RI_Model"
    Public Const RiskGroup As String = "Risk_group"
    Public Const RiskType As String = "Risk_Type"
    Public Const SafeHarbour As String = "Safe_Harbour"
    Public Const SeasonalGift As String = "Seasonal_gift"
    Public Const SecondaryCause As String = "Secondary_Cause"
    Public Const ServiceLevel As String = "Service_level"
    Public Const SICCode As String = "SIC_code"
    Public Const Source As String = "Source"
    Public Const State As String = "State"
    Public Const SupplierBusiness As String = "Supplier_Business"
    Public Const SupplierSpeciality As String = "Supplier_Speciality"
    Public Const Task As String = "PMWrk_Task"
    Public Const TaskGroup As String = "PMWrk_Task_Group"
    Public Const TaxBand As String = "Tax_Band"
    Public Const TaxGroup As String = "Tax_Group"
    Public Const Town As String = "Town"
    Public Const TransactionType As String = "Transaction_Type"
    Public Const Treaty As String = "Treaty"
    Public Const TurnoverBand As String = "TurnoverBand"
    Public Const TypeOfCard As String = "Type_of_Card"
    Public Const UnderwritingYear As String = "Underwriting_Year"
    Public Const UserGroup As String = "PMUser_Group"
    Public Const CaseProgressStatusCode As String = "Case_Progress"
    Public Const PFCancelReason As String = "PFPremiumFinance_Cancel_Reason"
    Public Const CorrespondenceType As String = "Correspondence_Type"
    Public Const CashListItemReverseReason As String = "CashListItem_Reverse_Reason"
    Public Const ClaimPaymentTo As String = "Claim_Payment_To"
    Public Const SubBranch As String = "Sub_Branch"
End Class

'''<summary>
''' This class defines the constants for MediaTypeValidationType
'''</summary>
Public NotInheritable Class MediaTypeValidationType
    Public Const Cheque As String = "CHEQUE"
    Public Const Cash As String = "CASH"
    Public Const CreditCard As String = "CC"
    Public Const Basic As String = "BASIC"
    Public Const Bank As String = "BANK"
End Class

'''<summary>
''' This class defines the constants for CashListType
'''</summary>
Public NotInheritable Class CashListType
    Public Const Payment As String = "P"
    Public Const Receipt As String = "R"
    Public Const ClaimPayment As String = "CP"
End Class

'''<summary>
''' This class defines the constants for CashListStatus
'''</summary>
Public NotInheritable Class CashListStatus
    Public Const Entered As String = "E"
    Public Const Opened As String = "O"
    Public Const Closed As String = "C"
End Class

'''<summary>
''' This class defines the constants for CashListItemPaymentStatus
'''</summary>
Public NotInheritable Class CashListItemPaymentStatus
    Public Const Issued As String = "ISS"
    Public Const Presented As String = "PRES"
    Public Const Cancelled As String = "CAN"
    Public Const Stale As String = "STALE"
    Public Const StopRequested As String = "STOPREQ"
    Public Const Stopped As String = "STOPPED"
    Public Const PendingApproval As String = "PENDING"
    Public Const Incomplete As String = "INCOMPLETE"
    Public Const Rejected As String = "REJECTED"
End Class

'''<summary>
''' This class defines the constants for AllocationStatus
'''</summary>
Public NotInheritable Class AllocationStatus
    Public Const Unallocated As String = "U"
    Public Const Posted As String = "P"
    Public Const Allocated As String = "A"
    Public Const PartialAllocation As String = "PARTIAL"

End Class

Public NotInheritable Class GIITables
    Public Const Gender As String = "131091"
    Public Const BusinessType As String = "2228228"
    Public Const Title As String = "131085"
End Class

Public NotInheritable Class PolicyPaymentMethod
    Public Const Instalments As String = "Instalments"
    Public Const PayNow As String = "PayNow"
    Public Const Invoice As String = "Invoice"
    Public Const CashDeposit As String = "CashDeposit"
End Class

Public NotInheritable Class AccountStatusCode
    Public Const Active As String = "ACTIVE"
    Public Const Stopped As String = "STOPPED"
End Class

Public NotInheritable Class RiskStatusCode
    Public Const PendingReinsurance As String = "PENDINGRI"
    Public Const Referred As String = "REFERRED"
    Public Const Declined As String = "DECLINED"
    Public Const Unquoted As String = "UNQUOTED"
    Public Const Quoted As String = "QUOTED"
    Public Const Deferred As String = "RIDEFERRED"
    Public Const QuotedPurchaseQuestionsOutstanding As String = "PURCHASE"
    Public Const QuotedPostQuoteQuestionsOutstanding As String = "POSTQUOTE"
    Public Const QuotedPreQuoteQuestionsOutstanding As String = "PREQUOTE"
End Class

Public NotInheritable Class NonPMLookupTable
    Public Const BankAccount As String = "BankAccount"
    Public Const Account As String = "Account"
    Public Const DocumentType As String = "DocumentType"
    Public Const DocumentTemplate As String = "DocumentType"
End Class

Public NotInheritable Class NonPMLookupTableKeyFields
    Public Const BankAccount As String = "BankAccount_Id"
    Public Const Account As String = "Account_Id"
    Public Const DocumentType As String = "DocumentType_Id"
    Public Const DocumentTemplateCode As String = "Code"
    Public Const DocumentTemplateId As String = "Document_Template_Id"
End Class

Public Enum SystemOption

    DMEInstalled = 10
    DuplicateClaimCheck = 5002
    InclusionOfCoinsuranceOnClaims = 1015
    AllowNegativeReserve = 1016
    ClaimHandlerAcknowlegedTaskAllowedTime = 1017
    ClaimHandlerPreliminaryReportTaskAllowedTime = 1018
    ClaimTaskGroup = 1019
    ClaimUserGroup = 1020
    PrintRenewalSchedule = 1036
    PrintRenewalCertificate = 1037
    PrintRenewalDebitNote = 1038
    ClaimPaymentAuthority = 2020
    ClaimPaymentIsGross = 5018
    AdvanceTaxScriptEnabled = 5007
    GenerateClaimDocumentRILimit = 1014
    DirectBusinessReturnPremiumUserGroup = 5051
    AgentBusinessReturnPremiumUserGroup = 5055
    ClaimPaymentWorkflow = 5017
    EnhancedResolvedName = 5148
    DisableWildcardSearch = 5065
    EnablePartialWildcardSearch = 5066
    SalvageAndTPRecoveryReservesExcludeTax = 5067
    CreditCardProcessingMethod = 5069
    CancelInstalmentPlanonPolicyCancellation = 5076
    EnableAgentProductLink = 5088
    AddressLookupInstallation = 13
    QuoteVersioning = 5089
    UserAttemptCount = 5107
    PasswordStrengthRegularExpression = 5101
    SingleCashListItemPerAllocation = 5087
    DisableTempMTAs = 5116
    EnableExclusiveLocking = 5174
    DocumentProductionInHouseOrCCM = 5163
    IsKCMApplicableForSelectedDocument = 5207
    IsSharepointOnline = 5177
    ChequeProductionEnabled = 60
    AllowClientPolicyAssociations = 5182
    DefaultReceiptDocument = 61
    DefaultPaymentDocument = 63
    OnlyAllowClaimClosureWhenReserveIsZero = 5073
    KCMApplicableForSelectedTemplate = 5207
    AutoAllocateCancelFromInception = 5246
    AutoAllocateCancelToleranceDays = 5247
    AutoAllocateCancelToleranceAmount = 5248
End Enum

Public NotInheritable Class EventTypeCode
    Public Const NewClaim As String = "NEWCLAIM"
    Public Const ClaimChanged As String = "CLACHANGE"
    Public Const ChangeOfPolicyDetails As String = "POLCHANGE"
    Public Const ProductionOfDocument As String = "DOCUMENT"
    Public Const NewCase As String = "NEWCASE"
    Public Const ClaimRecommend As String = "CLMRCOMD"
    Public Const ClaimAuthorise As String = "CLMAUTHO"
    Public Const ClaimDecline As String = "CLMDECLN"
End Class

Public Enum ProductOption
    Underwriting = 1
    ClaimsBuilderEnabled = 12
    RunClaimsAuthorisationScript = 51
    MultiStepApproval = 65
    RI2007 = 88
End Enum

Public NotInheritable Class PartyType
    Public Const Unknown As String = ""
    Public Const Agent As String = "AG"
    Public Const ThirdParty As String = "OT"
End Class

Public NotInheritable Class ContactType
    Public Const Telephone As String = "TELEPHONE"
    Public Const Homephone As String = "HOMEPHONE"
    Public Const Fax As String = "FAX"
    Public Const Email As String = "E-MAIL"
    Public Const Mobile As String = "MOBILE"
    Public Const Web As String = "WEB"
    Public Const Letter As String = "LETTER"
    Public Const Main As String = "MAIN"
    Public Const Other As String = "OTHER"
    Public Const MainEmailContact As String = "MEMAIL"
    Public Const WorkPhone As String = "WORKPHONE"
    Public Const MoreName As String = "MORENAME"
End Class

' Processing Status
Public Enum ProcessingStatus
    View = 0
    Insert = 1
    Update = 2
    Delete = 3
End Enum

Public NotInheritable Class OtherParty
    Public Const Supplier As String = "OTSUPPLIER"
    Public Const Driver As String = "OTDRIVER"
    Public Const Witness As String = "OTWITNESS"
    Public Const Repairer As String = "OTREPAIRER"
    Public Const ThirdParty As String = "OTTHIRD"
End Class

' Payment Party TO
Public Enum PaymentPartyTo
    None = 0
    CLMPayable = 1
    Party = 2
    Agent = 4
    Client = 8
End Enum

Public NotInheritable Class TransArray
    Public Const Account_id As Integer = 0
    Public Const Account As Integer = 1
    Public Const Currency_id As Integer = 2
    Public Const Currency As Integer = 3
    Public Const Amount As Integer = 4
    Public Const Currency_Rate As Integer = 5
    Public Const Base_Amount As Integer = 6
    Public Const AltRef As Integer = 7
    Public Const Comment As Integer = 8
    Public Const UnderwritingYear_id As Integer = 9
    Public Const UnderwritingYear As Integer = 10
    Public Const Department_id As Integer = 11
    Public Const Department As Integer = 12
    Public Const Insurance_Ref As Integer = 13
    Public Const Purchase_Order As Integer = 14
    Public Const Purchase_Invoice As Integer = 15
End Class

Public Enum ClaimTaxArray
    TaxGroupId = 0
    TaxBandId = 1
    CurrencyCode = 2
    Percentage = 3
    Value = 4
    IsValue = 5
    ClassOfBusinessId = 6
    Sequence = 7
    IsManuallyChanged = 8
    TaxGroupDescription = 9
    TaxBandDescription = 10
    TaxBandCode = 11
    ErrorMessage = 12
End Enum

Public Enum PMEEntityType
    ' General
    '********
    Party = 0
    InsuranceFile = 1
    Risk = 2
    Source = 3
End Enum

Public Enum TaxScriptMode
    Payment = 1
    Receipt = 2
End Enum

Public Enum WithholdingTaxSwitch
    IsOff = 0
    IsOn = 1
End Enum

Public Enum AutoReinsuranceFailure As Integer
    Reserve = 1
    Payment = 2
End Enum

Public Enum QuoteTypeRules
    Quote = 1
    Validate = 2
    Ual = 3
    [Default] = 4
    Renewal = 5
    PreScreen = 6
End Enum

Public NotInheritable Class TransactionTypeCode
    Public Const MaintainClaim As String = "C_CR"
    Public Const OpenClaim As String = "C_CO"
    Public Const PayClaim As String = "C_CP"
    Public Const NewBusiness As String = "NB"
    Public Const SalvageRecovery As String = "C_SA"
    Public Const ThirdPartyRecovery As String = "C_RV"
    Public Const CancelPolicy As String = "MTC"
    Public Const EditPolicy As String = "EDIT"
    Public Const MTA As String = "MTA"
    Public Const Renewals As String = "REN"
    Public Const PremiumFinanceCash As String = "PFCASH"
    Public Const PremiumFinanceMidTermAdjustment As String = "PFMTA"
    Public Const PremiumFinanceNewBusiness As String = "PFNB"
    Public Const PremiumFinanceRenewal As String = "PFREN"
    Public Const ReinstatePolicy As String = "MTR"
    Public Const DeferredReinsurance As String = "DRI"
    Public Const PortfolioTransfer As String = "PT"
End Class

Public NotInheritable Class ClaimStatus
    Public Const ProvisionalOpenClaim As String = "PRVOPENCLM"
    Public Const LiveOpenClaim As String = "LIVOPENCLM"
    Public Const Closed As String = "CLOSED"
End Class

Public NotInheritable Class InsuranceFileType
    Public Const Quote As String = "QUOTE"
    Public Const LivePolicy As String = "POLICY"
    Public Const Renewal As String = "RENEWAL"
    Public Const MTAPermanentQuotation As String = "MTAQUOTE"
    Public Const MTAPermanent As String = "MTA PERM"
    Public Const MTATemporary As String = "MTA TEMP"
    Public Const MTATemporaryQuotation As String = "MTAQTETEMP"
    Public Const MTACancellation As String = "MTACAN"
    Public Const MTAReinstated As String = "MTAREINS"
    Public Const MTAQuotationReinstatement As String = "MTAQREINS"
    Public Const Written As String = "WRITTEN"
    Public Const MTAQuotationCancellation As String = "MTAQCAN"
End Class

Public NotInheritable Class InstalmentPlanStatus
    Public Const Saved As String = "010"
    Public Const Updated As String = "011"
    Public Const QuotePrinted As String = "012"
    Public Const Live As String = "040"
    Public Const OnHold As String = "140"
    Public Const Completed As String = "900"
    Public Const Superceded As String = "990"
    Public Const Cancelled As String = "999"
End Class

Public NotInheritable Class SAMAgentType
    Public Const Agent As String = "3"
    Public Const SubAgent As String = "4"
    Public Const Introducer As String = "5"
End Class

Public NotInheritable Class SiriusUserDefaults
    Public Const Username As String = "sirius"
    Public Const Password As String = ""
    Public Const LanguageID As Int16 = 1
    Public Const CurrencyID As Int16 = 26
    Public Const SourceID As Int16 = 1
    Public Const UserID As Int32 = 1
    Public Const LogLevel As Int16 = 9
    Public Const AppName As String = "SiriusTransactionService"
End Class

Public Enum RenewalStatusType
    AwaitingManualReview = 1
    AwaitingRenewalNoticePrint = 2
    AwaitingManualRatingDueToFailure = 3
    PolicyDetailsChanged = 4
    AwaitingUpdate = 5
    AwaitingManualRating = 6
    AwaitingBrokerTransfer = 7
    'Start - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.10)
    AwaitingUpdateWritten = 8
    'End - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.10)
End Enum

Public NotInheritable Class Task
    Public Const Memo As String = "MEMO"
    'Start - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.9)
    Public Const UnderwritingNewBusiness As String = "UNDERNB"
    Public Const RenewalProcess As String = "RENPROCESS"
    'End  - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.9)
End Class

Public NotInheritable Class TaskGroup
    Public Const Common As String = "COMMON"
    'Start  - (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.9)
    Public Const UnderwritingMaintenance As String = "UNDER"
    Public Const UnderwritingRenewal As String = "UWRENEWAL"
    'End -  (Tech Spec - WPR35 - Written Status.doc) - (5.1.3.9)
End Class

Public NotInheritable Class UserGroup
    Public Const SystemAdministrator As String = "SYSADMIN"
End Class

Public Class InternalSAMConstants
    Public Const kCredit As String = "C"
    Public Const PMRenewalAlreadyAccepted As Integer = 60132
    ' business_type lookup
    Public Const kBusinessTypeDIRECT As String = "DIRECT"
    Public Const kBusinessTypeAGENCY As String = "AGENCY"
    Public Const sBroker As String = "Broker"
    Public Const sCommissionAccount As String = "Comm Acc"
    Public Const sIntermediary As String = "Intermed"
    Public Const kBusinessTypeAGENCYCOINLEAD As String = "COIN LEAD"
    Public Const kBusinessTypeAGENCYCOINFOLLOW As String = "COIN FOLL"
    Public Const kAccountShortnameCLMPAYABLE As String = "CLMPAYABLE"
    Public Const kAccountShortnameCLMRECEIVABLE As String = "CLMRECEIVABLE"
    Public Const kInsuranceFileRiskLinkValid As Integer = 1
    Public Const kDateInFuture As Date = #1/1/3000#
    Public Const ksRiskStatusReInsuranceDeferred As String = "RIDEFERRED"
    Public Const ksLapsedReasonVoided As String = "VOIDED"
    Public Const kProgressStatusClosed As String = "CLOSED"
    Public Const kAddressUsageTypeCodeCorrespondanceAddress As String = "3131 XCO"
    Public Const kDocumentTypeTransferredDebit As Integer = 35
    Public Const kStatsDetailTypeGross As String = "GRS"
    Public Const kTransactionSuppressed As Integer = 1
    Public Const kClaimProcessed As Integer = 0
    Public Const kUserId As Integer = 1
    Public Const ACTClaimAdminTaskGroupID As Integer = 10
    Public Const ACTBrokingTaskGroupID As Integer = 21
    Public Const ACAddPremiumDueNet As String = "PREMIUMDUENET"
    Public Const ACAddPremiumDueTax As String = "PREMIUMDUETAX"
    Public Const ACAddPremiumDueGross As String = "PREMIUMDUEGROSS"
    Public Const ACAddTotalAnnualTax As String = "TOTALANNUALTAX"
    Public Const ACAddCommission As String = "TOTALCOMMISSION"
    Public Const ACProductId As String = "PRODUCT_ID"
    Public Const ACPolicyTypeIdentifier As String = "PolicyType"
    Public Const ACPolicyTypeValueUW As String = "UNDERWRITE"
    Public Const ACConsolidatedLeadAgentCommission As String = "CLAC"
    Public Const ACConsolidatedSubAgentCommission As String = "CSAC"
    Public Const DebugFlag As Boolean = False
    Public Const GISLowDate As Date = #1/1/1900#
    Public Const ACPartyTypePC As String = "Personal Client"
    Public Const ACPartyTypeCodePC As String = "PC"
    Public Const CNClaimMode As String = "CLAIM_MODE"
    Public Const CNNewSchemeId As String = "CONTROL__NEW_SCHEME_ID"
    Public Const CNSchemeId As String = "CONTROL__SCHEME_ID"
    Public Const CNInsurerCnt As String = "CONTROL__INSURER_CNT"
    Public Const CNCoverFromDate As String = "CONTROL__COVER_FROM_DATE"
    Public Const CNCoverType As String = "CONTROL__COVER_TYPE"
    Public Const CNRiskCodeId As String = "CONTROL__RISK_CODE_ID"
    Public Const CNInsuranceFileCnt As String = "CONTROL__INSURANCE_FILE_CNT"
    Public Const CNEDISolution As String = "EDI_SOLUTION"
    Public Const CNDebitCredit As String = "NB_DebitCredit"
    Public Const CNPolicyLinkId As String = "POLICY_LINK_ID"
    Public Const CNLastEdiMessageCountSent As String = "LAST_EDI_MESSAGE_COUNT_SENT"
    Public Const CNLastTransType As String = "NB_LastTransType"
    Public Const CNNBReason As String = "NB_Reason"
    Public Const CNPartyCode As String = "PARTY_CODE"
    Public Const CNSourceOfBusiness As String = "SOB"
    Public Const CNAgentsOnline As String = "AOL"
    Public Const CNAddInfoSourceId As String = "SOURCE_ID"
    Public Const CNAddInfoCurrencies As String = "CURRENCIES"
    Public Const CNAddInfoFileCode As String = "FILECODE"
    Public Const CNAddressCountry As String = "AddressCountry"
    Public Const CNCurrentRiskCnt As String = "CURRENT_RISK_CNT"
    Public Const CNInsuranceFolderCnt As String = "CONTROL__INSURANCE_FOLDER_CNT"
    Public Const CNTransactionType As String = "TRANSACTION_TYPE"
    Public Const CNBusinessTypePremFinanceNB As String = "PFNB"
    Public Const CNBusinessTypeREN As String = "REN"
    Public Const CNDataModel As String = "CONTROL__XML_DATAMODEL"
    Public Const CNNBTransactMessage As String = "NBTRANSACT_MESSAGE"
    Public Const CNNewPolicyNumber As String = "NEW_POLICY_NUMBER"
    Public Const Header_BusinessType As String = "NB"
    Public Const CNPartyCnt As String = "CONTROL__PARTY_CNT"
    Public Const CNRiskCnt As String = "CONTROL__RISK_CNT"

    ' Postcode Visibility Code
    Public Const PMPostCodeVisibilityHidden As String = "H"
    Public Const PMPostCodeVisibilityVisible As String = "V"

    Public Const ACMailFormatText As String = "1"
    Public Const ACMailFormatHTML As String = "2"

    ' Merge Data Constants
    Public Const ACMergeXPath As String = "/DATA_SET/RISK_OBJECTS/{0}_POLICY_BINDER/"
    Public Const ACMergePolicyBinderId As String = "@{0}_POLICY_BINDER_ID"
    Public Const ACMergePolicyLinkId As String = "@GIS_POLICY_LINK_ID"
    Public Const ACMergeInsuranceFileCnt As String = "ASSOCIATED_CLIENT/@INSURANCE_FILE_CNT"
    Public Const ACMergeRiskCnt As String = "ASSOCIATED_CLIENT/@RISK_CNT"
    Public Const ACMergeAssociatedClient As String = "ASSOCIATED_CLIENT"
    Public Const ACMergePartyCnt As String = "ASSOCIATED_CLIENT/@PARTY_CNT"
    Public Const ACMergeIsInsured As String = "ASSOCIATED_CLIENT/@IS_INSURED"
    Public Const ACMergePartyTitleCode As String = "ASSOCIATED_CLIENT/@PARTY_TITLE_CODE"
    Public Const ACMergeForename As String = "ASSOCIATED_CLIENT/@FORENAME"
    Public Const ACMergeName As String = "ASSOCIATED_CLIENT/@NAME"
    Public Const ACMergeResolvedName As String = "ASSOCIATED_CLIENT/@RESOLVED_NAME"
    Public Const ACMergeInitials As String = "ASSOCIATED_CLIENT/@INITIALS"
    Public Const ACMergeDOB As String = "ASSOCIATED_CLIENT/@DATE_OF_BIRTH"
    Public Const ACMergeGender As String = "ASSOCIATED_CLIENT/@GENDER_CODE"
    Public Const ACMergePartyType As String = "ASSOCIATED_CLIENT/@PARTY_TYPE_CODE"
    Public Const ACMergePartyTypeDesc As String = "ASSOCIATED_CLIENT/@PARTY_TYPE_DESCRIPTION"
    Public Const ACMergeOutputReferMessage As String = "{0}_OUTPUT/@REFER_REASON"
    Public Const ACMergeOutputDeclineMessage As String = "{0}_OUTPUT/@DECLINE_REASON"
    Public Const ACMergeOutputStatus As String = "{0}_OUTPUT/@STATUS"

    ' Constants for the Tax Result Arrays
    Public Const ACRParentCnt As Integer = 0
    Public Const ACRTaxBandId As Integer = 1
    Public Const ACRPremium As Integer = 2
    Public Const ACRTaxRate As Integer = 3
    Public Const ACRTaxValue As Integer = 4
    Public Const ACRIsValue As Integer = 5
    Public Const ACRIsManuallyChanged As Integer = 6
    Public Const ACRDescription As Integer = 7
    Public Const ACRIsNotAppliedToClient As Integer = 8
    Public Const ACRIsDeleted As Integer = 9
    Public Const ACRBasisValue As Integer = 10
    Public Const ACRCalcBasis As Integer = 11
    Public Const ACRSumInsured As Integer = 12
    Public Const ACRIsSIRounded As Integer = 13
    Public Const ACRCurrencyID As Integer = 14
    Public Const ACRCurrencyName As Integer = 15
    Public Const ACRAllowTaxCredit As Integer = 16
    Public Const ACROriginalSumInsured As Integer = 17
    Public Const ACRSumInsuredChange As Integer = 18
    Public Const ACRTaxGroupID As Integer = 19
    Public Const ACRTaxGroup As Integer = 20
    Public Const ACRSequence As Integer = 21
    Public Const ACRCountryID As Integer = 22
    Public Const ACRCountry As Integer = 23
    Public Const ACRStateID As Integer = 24
    Public Const ACRState As Integer = 25
    Public Const ACRClassOfBusinessID As Integer = 26
    Public Const ACRClassOfBusiness As Integer = 27
    Public Const ACRRunningTotal As Integer = 28
    Public Const ACRPrimaryKeyTaxCnt As Integer = 29

    ' Constants for data array indexes.
    Public Const ACIDocumentRef As Integer = 0
    Public Const ACIAccountingDate As Integer = 1
    Public Const ACIPeriodName As Integer = 2
    Public Const ACICurrencyAmount As Integer = 3
    Public Const ACIPrimarySettled As Integer = 4
    Public Const ACIMatchedCurrencyAmount As Integer = 5
    Public Const ACIDocumentTypeId As Integer = 6
    Public Const ACIDocTypeGroupId As Integer = 7
    Public Const ACIInsuranceRef As Integer = 8
    Public Const ACIOperatorName As Integer = 9
    Public Const ACIPurchaseInvoiceNo As Integer = 10
    Public Const ACIPurchaseOrderNo As Integer = 11
    Public Const ACIDepartment As Integer = 12
    Public Const ACISpare As Integer = 13
    Public Const ACIAccountShortCode As Integer = 14
    Public Const ACIAccountId As Integer = 15
    Public Const ACICurrencyID As Integer = 16
    Public Const ACITransDetailId As Integer = 17
    Public Const ACIBaseAmount As Integer = 18
    Public Const ACIDocumentSequence As Integer = 19
    Public Const ACIDocumentDate As Integer = 20
    Public Const ACISourceID As Integer = 21
    Public Const ACIMatchAmount As Integer = 22
    Public Const ACIMatchDate As Integer = 23
    Public Const ACIReason As Integer = 24
    Public Const ACIInsuredName As Integer = 25
    Public Const ACIInsuredAccount As Integer = 26
    Public Const ACIFlag As Integer = 27
    Public Const ACIDocInsuranceFileCnt As Integer = 28
    Public Const ACIDocDocumentID As Integer = 29
    Public Const ACIAuditSetID As Integer = 30
    Public Const ACIAuditSetUserID As Integer = 31
    Public Const ACITransCurrencyBaseXRate As Integer = 32
    Public Const ACIPayeeName As Integer = 33
    Public Const ACIAlternateReference As Integer = 34
    Public Const ACIPolicyTypeId As Integer = 35
    Public Const ACIComment As Integer = 36
    Public Const ACINotReported As Integer = 37
    Public Const ACIUnderwritingYear As Integer = 38
    Public Const ACIMediaType As Integer = 39
    Public Const ACICurrencyText As Integer = 40
    Public Const ACIAmountCurrencyText As Integer = 41
    Public Const ACIAmountCurrencyID As Integer = 42
    Public Const ACIAccountCurrencyID As Integer = 43
    Public Const ACIAccountAmount As Integer = 44
    Public Const ACIOutstandingBaseAmount As Integer = 45
    Public Const ACIOutstandingAccountAmount As Integer = 46
    Public Const ACIAmountUpdated As Integer = 47
    Public Const ACIOutstandingTransAmount As Integer = 48
    Public Const ACIPeriodEndDate As Integer = 49
    Public Const ACIClaimReference As Integer = 50
    Public Const ACIPaymentDueDate As Integer = 51
    Public Const ACIRiskTransfer As Integer = 52
    Public Const ACITaxBandCode As Integer = 50
    Public Const ACITaxGroupCode As Integer = 51
    Public Const ACIAllocSequence As Integer = 52
    Public Const ACIAllocRule As Integer = 53
    Public Const ACIDetailTypeCode As Integer = 54
    Public Const ACIBalanceType As Integer = 55
    Public Const ACIPartyType As Integer = 56
    Public Const ACIIsFlaotBalanceAccount As Integer = 57
    Public Const ACIIsOverdraftAccount As Integer = 58
    Public Const ACITransReference As Integer = 59
    Public Const ACIAgentName As Integer = 60
    Public Const ACIBGRef As Integer = 61
    Public Const ACIDocumentTypeDescription As Integer = 62

    Public Const ACIEventCnt As Integer = 0
    Public Const ACIEventInsuranceFolderCnt As Integer = 1
    Public Const ACIEventInsuranceFolderDesc As Integer = 2
    Public Const ACIEventInsuranceFileCnt As Integer = 3
    Public Const ACIEventInsuranceFileStructureId As Integer = 4
    Public Const ACIEventClaimCnt As Integer = 5
    Public Const ACIEventClaimDesc As Integer = 6
    Public Const ACIEventDocumentCnt As Integer = 7
    Public Const ACIEventDocumentDesc As Integer = 8
    Public Const ACIEventOldAddressCnt As Integer = 9
    Public Const ACIEventNewAddressCnt As Integer = 10
    Public Const ACIEventCampaignDesc As Integer = 11
    Public Const ACIEventReportDesc As Integer = 12
    Public Const ACIEventEventType As Integer = 13
    Public Const ACIEventUserName As Integer = 14
    Public Const ACIEventDate As Integer = 15
    Public Const ACIEventDescription As Integer = 16
    Public Const ACIEventOldPartyType As Integer = 17
    Public Const ACIEventReason As Integer = 18
    Public Const ACIEventDocumentRef As Integer = 19
    Public Const ACIEventEventLogSubject As Integer = 20
    Public Const ACIEventEventTypeGroupId As Integer = 21
    Public Const ACIEventEventTypeGroupDescription As Integer = 22
    Public Const ACIEventEventLogSubjectId As Integer = 23
    Public Const ACIEventFSAComplaintFolderCnt As Integer = 24
    Public Const ACIEventAlternateReference As Integer = 25
    Public Const ACIEventSourceId As Integer = 26
    Public Const ACIEventPolicyTypeId As Integer = 27
    Public Const ACIEventUnderwritingBranchInd As Integer = 28
    Public Const ACIEventHasNotes As Integer = 29
    Public Const ACIEventPriorityCode As Integer = 30   '2005 StickyNotes
    Public Const ACIEventIsCompleted As Integer = 31     '2005 StickyNotes
    Public Const ACIEventRTFNotes As Integer = 32
    Public Const ACIEventBGId As Integer = 33
    Public Const ACIEventCaseNumber As Integer = 34
    Public Const ACIEventBaseClaimCnt As Integer = 35
End Class

Public NotInheritable Class PartyBankHistoryActionCode
    Public Const Setup As String = "Setup"
    Public Const Amendment As String = "Amendment"
    Public Const Activated As String = "Activated"
    Public Const Inactivated As String = "Inactivated"
End Class

Public Enum BusinessType
    Quote = 1
    Policy = 2
    ProvisionalClaim = 3
    FullClaim = 4
End Enum

Public NotInheritable Class TaxType
#Region "Constructors"
    Private Sub New()
        ' This class cannot be instantiated.
    End Sub
#End Region

    Public Const TREATYPREMIUMTAXTYPE As String = "TTRITP"
    Public Const TREATYCOMMISSIONTAXTYPE As String = "TTRITC"
    Public Const FACPREMIUMTAXTYPE As String = "TTRIFP"
    Public Const FACCOMMISSIONTAXTYPE As String = "TTRIFC"
End Class
''' <summary>
''' Defines the contant values for the risk link status flag
''' </summary>
''' <remarks>Done as a module as there in no need to instantiate an instance of this class</remarks>
Public Module RiskLinkStatusType

    ''' <summary>
    ''' Risk has been copied/changed status if this is set then there is a record for this insurance file in the risk table
    ''' </summary>
    Public Const Changed As String = "C"

    ''' <summary>
    ''' Risk is unchanged in this version and points to the old risk
    ''' </summary>
    Public Const Unchanged As String = "U"

    ''' <summary>
    ''' Risk has been renewed, still points to the old risk record but has financial data for this version
    ''' </summary>
    Public Const Renewed As String = "R"

    ''' <summary>
    ''' Risk has been deleted in this version, has a records in risk tables
    ''' </summary>
    Public Const Deleted As String = "D"
End Module

'End Namespace
