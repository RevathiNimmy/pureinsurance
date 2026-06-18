Imports System.Configuration

''' <summary>
''' Constants used for identifiying data within cache or user session
''' </summary>
Public Module Constants

    ''' <summary>
    ''' Cache key for the Dataset Defintion
    ''' </summary>
    ''' <remarks>Key is suffixed with the DataModelCode</remarks>
    Public Const PROVIDER_DATASETDEFINITION = "DataSetDefinition_"

    ''' <summary>
    ''' Cache key for a data list
    ''' </summary>
    ''' <remarks>Key is suffixed with code / key of the list being retrieved</remarks>
    Public Const PROVIDER_LOOKUPLIST = "LookupList_"

    ''' <summary>
    ''' Cache key of the search results for the FindControl
    ''' </summary>
    ''' <remarks>Key is suffixed with a HashCode of the search criteria</remarks>
    Public Const PROVIDER_FINDCONTROLSEARCH = "FindControlSearch_"

    ''' <summary>
    ''' The length of time in hours that any data cached within the provider will be held before expirying
    ''' </summary>
    ''' <remarks></remarks>
    Public Const PROVIDER_CACHE_HOURS = 2

    Public Const ERROR_NO_LENGTH As Integer = 10
    Public Const ERROR_LABEL As String = "SSP - ERROR REF : "

    ''' <summary>
    ''' Session key of the current users UserName
    ''' </summary>
    Public Const PROVIDER_USERNAME = "ProviderUserName"

    ''' <summary>
    ''' Session key of the current users password hash
    ''' </summary>
    ''' <remarks>It's not ideal to place the password hash in session but it's npossible
    ''' to serialize the Wse3 usertoken with the credentials, even though it's serialized for
    ''' use by the web service</remarks>
    Public Const PROVIDER_USERHASH = "ProviderUserHash"
End Module

Public Enum ProductRiskOptions

    '''Remarks
    CaptionId = 0

    '''Remarks
    Code = 1

    '''Remarks
    Description = 2

    '''Remarks
    ProductEffectiveDate = 3

    '''Remarks
    IsDeleted = 4

    '''Remarks
    SchemeAgencyRef = 5

    '''Remarks
    BlockNo = 6

    '''Remarks
    IsTaxSuppressed = 7

    '''Remarks
    QuoteAutoNumberingID = 8

    '''Remarks
    IsShortPeriodRated = 9

    '''Remarks
    IsMidnightRenewal = 10

    '''Remarks
    IsAutoRenewable = 11

    '''Remarks
    RenewalWeeks = 12

    '''Remarks
    PolicyAutoNumberingID = 13

    '''Remarks
    ProvClaimAutoNumberingID = 14

    '''Remarks
    FullClaimAutoNumberingID = 15

    '''Remarks
    Accumulation = 16

    '''Remarks
    RIPointer = 17

    '''Remarks
    ReportPointer = 18

    '''Remarks
    ClaimYearToCheck = 19

    '''Remarks
    MaxSingleClaimValue = 20

    '''Remarks
    MaxNumberOfClaim = 21

    '''Remarks
    MaxTotalClaimValue = 22

    '''Remarks
    NBProrata = 23

    '''Remarks
    MTAProrata = 24

    '''Remarks
    RoundPremium = 25

    '''Remarks
    RoundingSection = 26

    '''Remarks
    PolicyNumberAtQuote = 27

    '''Remarks
    FollowUpTimeFrame = 28

    '''Remarks
    GracePeriod = 29

    '''Remarks
    PreventCancelledAgents = 30

    '''Remarks
    AllowPositiveValues = 31

    '''Remarks
    MediaTypeMandatory = 32

    '''Remarks
    PolicyStyleID = 33

    '''Remarks
    PolicyStyleMandatory = 34

    '''Remarks
    CurrencyChange = 35

    '''Remarks
    LossCurrencyChange = 36

    '''Remarks
    ChangePolicyNumberAtRenewal = 37

    '''Remarks
    AllowStandardWordingEdit = 38

    '''Remarks
    HideSummaryAtRenewalAcceptance = 39

    '''Remarks
    ProductSuppressClaimTransactionsReserves = 40

    '''Remarks
    ProductSuppressClaimTransactionsPayments = 41

    '''Remarks
    ProductSuppressClaimTransactionsRecoveries = 42

    '''Remarks
    CanMakeLiveInvoice = 43

    '''Remarks
    CanMakeLiveInstalments = 44

    '''Remarks
    CanMakeLivePaynow = 45

    '''Remarks
    ProduceSchedule = 46

    '''Remarks
    ProduceCertificate = 47

    '''Remarks
    ProduceDebitNote = 48

    '''Remarks
    ClaimsTypeBasis = 49

    '''Remarks
    ClaimsCoverBasis = 50

    '''Remarks
    TradeNbOnline = 51

    '''Remarks
    TradeMtaOnline = 52

    '''Remarks
    TradeRnlOnline = 53

    '''Remarks
    OnlineTradingCommencedOn = 54

    '''Remarks
    MTCRatingRulesEnabled = 55

    '''Remarks
    CanMakeBankGuarantee = 56

    '''Remarks
    CoverNotedocTemplate = 57

    '''Remarks
    IsTrueMonthlyPolicy = 58

    '''Remarks
    AnniversaryRenewalWeeks = 59

    '''Remarks
    UnifiedRenewalDay = 60

    '''Remarks
    LeadAllowConsolidatedCommission = 61

    '''Remarks
    LeadMonthInCycle = 62

    '''Remarks
    LeadSuspenseAccountId = 63

    '''Remarks
    SubAllowConsolidatedCommission = 64

    '''Remarks
    SubMonthInCycle = 65

    '''Remarks
    SubSuspenseAccountId = 66

    '''Remarks
    DefaultPaymentMethod = 67

    '''Remarks
    IsRenewable = 68

    '''Remarks
    IsRenewalSelectionEnabled = 69

    '''Remarks
    TrueMonthlyPolicyRenewalCommunication = 70

    '''Remarks
    RenewalSelectionManReviewTemplateId = 71

    '''Remarks
    RenewalSelectionManReviewAttachmentTemplateId = 72

    '''Remarks
    RenewalSelectionInviteTemplateId = 73

    '''Remarks
    RenewalSelectionInviteAttachmentTemplateId = 74

    '''Remarks
    RenewalSelectionUpdateTemplateId = 75

    '''Remarks
    RenewalSelectionUpdateAttachmentTemplateId = 76

    '''Remarks
    IsRenewalInviteEnabled = 77

    '''Remarks
    RenewalInviteManReviewTemplateId = 78

    '''Remarks
    RenewalInviteManReviewAttachmentTemplateId = 79

    '''Remarks
    RenewalInviteInviteTemplateId = 80

    '''Remarks
    RenewalInviteInviteAttachmentTemplateId = 81

    '''Remarks
    RenewalInviteUpdateTemplateId = 82

    '''Remarks
    RenewalInviteUpdateAttachmentTemplateId = 83

    '''Remarks
    IsRenewalUpdateEnabled = 84

    '''Remarks
    RenewalUpdateManReviewTemplateId = 85

    '''Remarks
    RenewalUpdateManReviewAttachmentTemplateId = 86

    '''Remarks
    RenewalUpdateInviteTemplateId = 87

    '''Remarks
    RenewalUpdateInviteAttachmentTemplateId = 88

    '''Remarks
    RenewalUpdateUpdateTemplateId = 89

    '''Remarks
    RenewalUpdateUpdateAttachmentTemplateId = 90

    '''Remarks
    IsAgentRenewalSelectionEnabled = 91

    '''Remarks
    IsAgentRenewalInviteEnabled = 92

    '''Remarks
    IsAgentRenewalUpdateEnabled = 93

    '''Remarks
    AgentRenewalManReviewTemplateId = 94

    '''Remarks
    AgentRenewalManReviewReportId = 95

    '''Remarks
    AgentRenewalInviteTemplateId = 96

    '''Remarks
    AgentRenewalInviteReportId = 97

    '''Remarks
    AgentRenewalUpdateTemplateId = 98

    '''Remarks
    AgentRenewalUpdateReportId = 99

    '''Remarks
    MultipleClaimsPayments = 100

    '''Remarks
    MaxUnauthorisedClaimValue = 101

    '''Remarks
    MaxUnauthorisedNoClaimPayments = 102

    '''Remarks
    RunAuthorisationScriptsClaimPayments = 103

    '''Remarks
    BankAccountId = 104

    '''Remarks
    ClaimValueForLargeLossAdvice = 105

    '''Remarks
    InclusionOfCoInsurersOnClaims = 106

    '''Remarks
    AllowNegativeReserve = 107

    '''Remarks
    ExtClmHandlerAcknowledgedTaskAllowedTime = 108

    '''Remarks
    ExtClmHandlerSupplyPreReportTaskAllowedTime = 109

    '''Remarks
    ValidPolicyVersionAtLossDate = 110

    '''Remarks
    IsGrossClaimPaymentAmount = 111

    '''Remarks
    ClaimTaskGroup = 112

    '''Remarks
    ClaimUserGroup = 113

    '''Remarks
    ClaimsUDTA = 114

    '''Remarks
    ClaimsUDTB = 115

    '''Remarks
    ClaimsUDTC = 116

    '''Remarks
    ClaimsUDTD = 117

    '''Remarks
    ClaimsUDTE = 118

    '''Remarks
    IsDuplicateClaimCheckEnabled = 119

    '''Remarks
    IsAdvancedTaxScriptEnabled = 120

    '''Remarks
    IsPaymentRefCheckEnabled = 121

    '''Remarks
    IsRecommendClaimPayments = 122

    '''Remarks
    OutOfSequenceMTAAllocation = 123

    '''Remarks
    OutOfSequenceMTADates = 124

    '''Remarks
    DefaultRenewalMonths = 125

    '''Remarks
    PaymentCannotExceedReserve = 126

    '''Remarks
    AllowBackdatedMTAs = 127

    '''Remarks
    CoverNoteNumberingId = 128

    '''Remarks
    CoverNoteDefaultPeriod = 129

    '''Remarks
    CoverNoteReusedUpTo = 130

    '''Remarks
    CheckMediatypeStatusAtClaimPayment = 131

    '''Remarks
    CheckMediatypeStatusAtPolicyRefund = 132

    '''Remarks
    RoundOffToZero = 133

    '''Remarks
    CanMakeLiveCashDeposit = 134

    '''Remarks
    AllowBackdatedMTCs = 135

    '''Remarks
    BackdatedMTAUserGroup = 136

    '''Remarks
    BackdatedMTATaskGroup = 137

    '''Remarks
    TMPAutorenfac = 138

    '''Remarks
    IsPrepaymentOptionEnabled = 139

    '''Remarks
    CanMakeLiveInvoiceTMP = 140

    '''Remarks
    CanMakeLiveInstalmentTMP = 141

    '''Remarks
    AllowWrittenStatus = 142

    '''Remarks
    WrittenTaskManagerDays = 143

    '''Remarks
    WrittenReminderUserGroup = 144

    '''Remarks
    WrittenReminderTaskGroup = 145

    '''Remarks
    ChangePolicyNumberAtRenewalAutomatically = 146

    '''Remarks
    BindRenewalWithoutInvitation = 147

    '''Remarks
    DoNotDeleteRenQuoteOnMTA = 148

    '''Remarks
    DefaultCoverToDateToLastDay = 149

    '''Remarks
    UnifiedRenewalDateIsReadOnly = 150

    '''Remarks
    IsReservesReadOnly = 151

    '''Remarks
    IsRecoveriesReadOnly = 152

    '''Remarks
    IsPaymentsReadOnly = 153

    '''Remarks
    RiManualPremiumAdjustment = 154

    '''Remarks
    QuoteAllRiskNB = 155

    '''Remarks
    QuoteAllRiskMTC = 156

    '''Remarks
    QuoteAllRiskMTA = 157

    '''Remarks
    AutoRenewBDMPolicy = 158

    '''Remarks
    AllowReRateAllCancRein = 159

    '''Remarks
    AllowReRateAllNBQuotation = 160

    '''Remarks
    DeleteRenQuoteReRunRenewal = 161

    '''Remarks
    QuoteAllRiskRenewal = 162

    '''Remarks
    RetainPolicyNumberonCopy = 163

    AnniversaryDateEditable = 164

    DisableCoverStartDateOnREN = 165

    IsQuoteVersioning = 166

    RecoveryInstalmentsEnabled = 167

End Enum

Public Enum SystemOptions

    DoNotDefaultMTADate = 1026
    '''<remarks/>
    SingleCashListItemPerAllocation = 5087

    '''<remarks/>
    DisableTempMTAs = 5116

    ExclusiveLock = 5174
    EnableCCMDatasetLogging = 5181
    AllowPolicyClientAssociations = 5182
    RefundPremiumThroughInvoice = 5197
    SystemOptionPaymentHubEnabled = 5200
    ClaimsReserveForGross = 5239
    TaxGroupForClaimsReserve = 5240
    EnhancedCashSearch = 5099
    EmailRegex = 5245
    WriteOffInterMediateAccount = 5028
    PolicyDiscount = 5004
    ClientSearchIndependentToBranchAccess = 5265
End Enum

Public Enum ProductOptions
    SuppressDecimalValues = 112
    EnableSpatialAddressFields = 113
    MultiStepApproval = 65
End Enum

Public Enum ClaimProcessType

    None
    OpenClaim
    MaintainClaim
    ClaimPayment
End Enum
Public Enum RiskTypeOptions
    '''<remarks/>
    Code

    '''<remarks/>
    Description

    '''<remarks/>
    EffectiveDate

    '''<remarks/>
    AccumulationLevel

    '''<remarks/>
    GisScreenId

    '''<remarks/>
    PrimarySort

    '''<remarks/>
    SecondarySort

    '''<remarks/>
    StampDutyRate1

    '''<remarks/>
    StampDutyRate2

    '''<remarks/>
    HeaderClauseDescription

    '''<remarks/>
    TrailerClauseDescription

    '''<remarks/>
    IsShareWithCoInsurers

    '''<remarks/>
    IsShareWithReInsurers

    '''<remarks/>
    IsSuppressPublicText

    '''<remarks/>
    IsSuppressPrivateText

    '''<remarks/>
    IsSuppressTaxes

    '''<remarks/>
    ClaimsIsPostTaxes

    '''<remarks/>
    VsectionMask

    '''<remarks/>
    IsAutoReinsured

    '''<remarks/>
    IsDeferredRiPermitted

    '''<remarks/>
    DisplayReinsurance

    '''<remarks/>
    DisplayClaimReinsurance

    '''<remarks/>
    AllowRatingSectionAdd

    '''<remarks/>
    AllowRatingSectionEdit

    '''<remarks/>
    AllowRatingSectionDelete

    '''<remarks/>
    AllowEditRatingSectionRateType

    '''<remarks/>
    AllowEditRatingSectionRate

    '''<remarks/>
    AllowEditRatingSectionSumInsured

    '''<remarks/>
    AllowEditRatingSectionThisPremium
    '''<remarks/>
    ClaimsTypeBasis

    '''<remarks/>
    ClaimsCoverBasis

    ''' <remarks/>
    AttachClaimOutsideOfPolicyPeriod

    ''' <remarks/>
    None
End Enum
Public Enum ProductConfigActionType
    ProductRiskMaintenance
    RiskTypeMaintenance

End Enum

Public Enum CopyRiskTypes

    '''<remarks/>
    Comparative

    '''<remarks/>
    Duplicate
End Enum

''' <summary>
''' Document types
''' </summary>
Public Enum DocumentType

    ''' <summary>
    ''' None
    ''' </summary>
    None = 0

    ''' <summary>
    ''' HTML
    ''' </summary>
    HTML = 1

    ''' <summary>
    ''' PDF
    ''' </summary>
    PDF = 2

    ''' <summary>
    ''' XML format
    ''' </summary>
    ''' <remarks></remarks>
    DOCX = 3

    ''' <summary>
    ''' XML format
    ''' </summary>
    ''' <remarks></remarks>
    XML = 4

    ''' <summary>
    ''' XML format
    ''' </summary>
    ''' <remarks></remarks>
    TXT = 5
End Enum
Public Enum InsuranceFileTypes
    '''<remarks/>
    QUOTE = 1

    '''<remarks/>
    MTAQUOTE = 2

    '''<remarks/>
    POLICY = 3

    '''<remarks/>
    RENEWAL = 4

    ALL = 0 ' Not to pass this parameter
End Enum
Public Enum InsuranceQuoteType

    '''<remarks/>
    QUOTE

    '''<remarks/>
    POLICY

    '''<remarks/>
    ALL
End Enum
Public Enum OptionType
    ProductOption = 0
    SystemOption = 1
End Enum
Public Enum DocumentTypeTypes

    '''<remarks/>
    JN
End Enum
Public Enum BGGetPoliciesActionTypeType

    '''<remarks/>
    All = 0

    '''<remarks/>
    OutStandingPremium = 1
End Enum

Public Enum BGPartyTypeType

    '''<remarks/>
    Client

    '''<remarks/>
    Agent
End Enum


'Added for Party type in GetClaimReceipttaxes
Public Enum ClaimReceiptPartyTypeType
    CLMRECEIVABLE = 0
    PARTY = 1
    AGENT = 2
    CLIENT = 3
End Enum
'WPR 29
''' <summary>
''' DMEDoc types
''' </summary>
Public Enum DMEDocType

    ''' <summary>
    ''' Unknown
    ''' </summary>
    Unknown = 0

    ''' <summary>
    ''' Tif
    ''' </summary>
    Tif = 1

    ''' <summary>
    ''' PlainText
    ''' </summary>
    PlainText = 2

    ''' <summary>
    ''' RTF
    ''' </summary>
    RTF = 3

    ''' <summary>
    ''' Word
    ''' </summary>
    Word = 4

    ''' <summary>
    ''' Excel
    ''' </summary>
    Excel = 5

    ''' <summary>
    ''' PowerPoint
    ''' </summary>
    PowerPoint = 6

    ''' <summary>
    ''' Access
    ''' </summary>
    Access = 7

    ''' <summary>
    ''' HTML
    ''' </summary>
    HTML = 8

    ''' <summary>
    ''' GIF
    ''' </summary>
    GIF = 9

    ''' <summary>
    ''' JPEG
    ''' </summary>
    JPEG = 10

    ''' <summary>
    ''' Email
    ''' </summary>
    Email = 11

    ''' <summary>
    ''' PDF
    ''' </summary>
    PDF = 12

    ''' <summary>
    ''' HelpFile
    ''' </summary>
    HelpFile = 13

    ''' <summary>
    ''' ZIP
    ''' </summary>
    ZIP = 14

    ''' <summary>
    ''' MSG
    ''' </summary>
    ''' <remarks></remarks>
    MSG = 15
End Enum

Public Enum FinancePlanStatus

    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("None")>
    Item000

    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("Saved")>
    Item010

    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("Updated")>
    Item011

    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("QuotePrinted")>
    Item012

    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("Live")>
    Item040

    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("OnHold")>
    Item140

    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("Completed")>
    Item900

    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("Superceded")>
    Item990

    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("Cancelled")>
    Item999
End Enum

Public Enum CancelPFPlanType
    DeletePlan
    SettlePlan
    CancelPlan
End Enum

Public Enum ProcessPFPlanType
    NewPlan
    PlanMTA
End Enum
#Region "Get Report"
''' <summary>
''' Will be used for Reports - document format type
''' </summary>
''' <remarks></remarks>
Public Enum DocumentFormatType

    ''' <summary>
    ''' None
    ''' </summary>
    None = 0

    ''' <summary>
    ''' HTML
    ''' </summary>
    HTML = 1

    ''' <summary>
    ''' PDF
    ''' </summary>
    PDF = 2

    ''' <summary>
    ''' XML format
    ''' </summary>
    ''' <remarks></remarks>
    DOCX = 3

End Enum
#End Region

Public Enum RiskStatusType

    '''<remarks/>
    REFERRED

    '''<remarks/>
    DECLINED

    '''<remarks/>
    QUOTED

    '''<remarks/>
    UNQUOTED

    '''<remarks/>
    PURCHASE

    '''<remarks/>
    POSTQUOTE

    '''<remarks/>
    PREQUOTE

    '''<remarks/>
    PENDINGRI

    '''<remarks/>
    RIDEFERRED

    '''<remarks/>
    PENDINGRIP

    '''<remarks/>
    DISCOUNT

    '''<remarks/>
    REVIEWFAC
End Enum

Public Enum InsuranceFilterQuotesType

    All


    ExcludeExpiredQuotes


    ExpiredQuotesOnly


    ExcludeCancelledQuotes


    CancelledQuotesOnly


    ExcludeCancelledandExpiredQuotes


    NBQuotesOnly


    MTAQuotesOnly

    RenewalQuotesOnly
End Enum

Public Enum InsuranceFilterPoliciesType

    All

    ExcludeLapsedPolicies


    LapsedPoliciesOnly


    ExcludeCancelledPolicies


    CancelledPoliciesOnly


    ExcludeCancelledandLapsedPolicies


End Enum
Public MustInherit Class ProviderBase : Inherits Provider.ProviderBase

    ' ''' <summary>
    ' ''' Sets the credentials to be used when accessing the provider, if no credentials are set
    ' ''' when a method is called the credentials will default to those provided in the web.config
    ' ''' </summary>
    ' ''' <param name="v_sUserName">UserName</param>
    ' ''' <param name="v_sPassword">Password (ClearText)</param>
    ' ''' <remarks></remarks>
    'Public MustOverride Sub Credentials(ByVal v_sUserName As String, ByVal v_sPassword As String)

    ' ''' <summary>
    ' ''' Clears any credentials in use i.e wse3 usertoken
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public MustOverride Sub ClearCredentials()

#Region "WPR 12"
    ''' <summary>
    ''' GetQuotesMarkedForCollection returns the Quote Collection
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_iPartyKey"></param>
    ''' <param name="v_iInsuranceFilekey"></param>
    ''' <param name="v_iAgentKey"></param>
    ''' <param name="v_oProduct"></param>
    ''' <param name="v_dSearchDateFrom"></param>
    ''' <param name="v_dSearchDateTo"></param>
    ''' <param name="v_bDirectBusinessonly"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetQuotesMarkedForCollection(Optional ByVal v_sBranchCode As String = Nothing,
                                         Optional ByVal v_iPartyKey As Integer = 0,
                                         Optional ByVal v_iInsuranceFilekey As Integer = 0,
                                         Optional ByVal v_iAgentKey As Integer = 0,
                                         Optional ByVal v_oProductCollection As ProductCollection = Nothing,
                                         Optional ByVal v_dSearchDateFrom As Date = Nothing,
                                         Optional ByVal v_dSearchDateTo As Date = Nothing,
                                         Optional ByVal v_bDirectBusinessonly As Boolean = False) As QuoteCollection
#End Region

#Region "Get PaymentHub System Option"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_dOutstandingAmount"></param>
    ''' <param name="v_nInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    Public MustOverride Function GetPaymentHubSystemOptions(Optional ByVal v_sBranchCode As String = Nothing) As PaymentHubConfig
#End Region

#Region "WPR 29"
    ''' <summary>
    ''' AddDocumentToDocumaster returns the Integer
    ''' </summary>
    ''' <param name="oAddDocumentToDocumasterType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function AddDocumentToDocumaster(ByVal oAddDocumentToDocumasterType As AddDocumentToDocumasterType,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As Integer

    ''' <summary>
    ''' GetDMEFolder returns the DME Record (Collection of DocumentList and SubFolder)
    ''' </summary>
    ''' <param name="v_folderNumField"></param>
    ''' <param name="v_folderPathField"></param>
    ''' <param name="v_bincludeFilesField"></param>
    ''' <remarks></remarks>
    Public MustOverride Function GetDMEFolder(ByVal v_folderNumField As Integer,
                                                          ByVal v_folderPathField As String,
                                                          ByVal v_bincludeFilesField As Boolean,
                                                          Optional ByVal v_sBranchCode As String = Nothing,
                                                          Optional ByVal v_iFilterCategoryId As Integer = 0,
                                                          Optional ByVal v_iFilterSubCategoryId As Integer = 0,
                                                          Optional ByVal v_sFilterDocName As String = Nothing) As DME
    ''' <summary>
    ''' FindDMEDocuments returns the DME Record (Collection of DocumentList and SubFolder)
    ''' </summary>
    ''' <param name="v_oDMESearchCriteria"></param>
    ''' <remarks></remarks>
    Public MustOverride Function FindDMEDocuments(ByVal v_oDMESearchCriteria As DMESearchCriteria,
                                        Optional ByVal v_sBranchCode As String = Nothing) As DME


#End Region

#Region "SAGICOR - 1.13.8 Broker View & CATLIN - 1.13.5 Anonymous Quote"
    ''' <summary>
    ''' To Transfer an anonymous quote to real party - 
    ''' </summary>
    ''' <param name="v_iPartyFrom"></param>
    ''' <param name="v_iPartyTo"></param>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks>WPR 12 - To transfer an anonymous quote to real party</remarks>
    Public MustOverride Sub TransferQuoteToRealParty(ByVal v_iPartyFrom As Integer, ByVal v_iPartyTo As Integer,
       ByVal v_iInsuranceFileKey As Integer, Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' Agent user logged in to S4i NEXUS can retrieve quotes/policies - 
    ''' </summary>
    ''' <param name="v_oQuoteType"></param>
    ''' <param name="v_sProductCode"></param>
    ''' <param name="v_sResultsType"></param>
    ''' <param name="v_sInsuranceRef"></param>
    ''' <param name="v_sInsuredName"></param>
    ''' <param name="v_iMaxRowsToFetch"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="CoverStartDate"></param>
    ''' <param name="QuoteORLiveDate"></param>
    ''' <param name="Agentkey"></param>
    ''' <remarks>WPR 13 - Agent user logged in to S4i NEXUS can retrieve quotes/policies | WPR 63 -  3 parameters added </remarks>
    '''
    Public MustOverride Function GetBrokerSummary(ByVal v_oQuoteType As InsuranceQuoteType, ByVal v_sProductCode As String, ByVal v_sResultsType As Integer, ByVal v_sInsuranceRef As String, ByVal v_sInsuredName As String, ByVal v_iMaxRowsToFetch As Integer,
                                                  Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal CoverStartDate As Date = Nothing, Optional ByVal QuoteORLiveDate As Date = Nothing, Optional ByVal Agentkey As Integer = Nothing, Optional ByVal v_sRiskIndex As String = Nothing, Optional ByVal v_iFilterQuotes As InsuranceFilterQuotesType = Nothing, Optional ByVal v_iFilterPolicies As InsuranceFilterPoliciesType = Nothing) As PartySummary
#End Region

#Region "Old Method"

    '    ''' <summary>
    '    ''' Retrieves the user details of the user whose credentials are in use
    '    ''' </summary>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetUserDetails() As UserDetails

    '    ''' <summary>
    '    ''' Find a party by the given criteria
    '    ''' </summary>
    '    ''' <param name="v_oPartySearchCriteria">Party Search Criteria</param>
    '    ''' <param name="v_sBranchCode">Branch Code, will default to the web.config branch code if not provided</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function FindParty(ByVal v_oPartySearchCriteria As PartySearchCriteria, _
    '                                        Optional ByVal v_sBranchCode As String = Nothing) As PartyCollection

    '    ''' <summary>
    '    ''' Adds a new quote and risk(s) (if provided in the quote object) to the data source
    '    ''' </summary>
    '    ''' <param name="r_oQuote">Quote</param>
    '    ''' <param name="v_sBranchCode">Branch Code, will default to the web.config branch code if not provided</param>
    '    ''' <param name="v_sSubBranchCode">Sub Branch Code</param>
    '    ''' <param name="v_iAgentKey">Agent Key</param>
    '    ''' <param name="v_sUserName">UserName</param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub AddQuote(ByRef r_oQuote As Quote, _
    '                                Optional ByVal v_sBranchCode As String = Nothing, _
    '                                Optional ByVal v_sSubBranchCode As String = Nothing, _
    '                                Optional ByVal v_iAgentKey As Integer = 0, _
    '                                Optional ByVal v_sUserName As String = Nothing)

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="r_oQuote"></param>
    '    ''' <param name="v_iQuoteRiskIndex"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_sSubBranchCode"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub AddRisk(ByRef r_oQuote As Quote, _
    '                                ByVal v_iQuoteRiskIndex As Integer, _
    '                                Optional ByVal v_sBranchCode As String = Nothing, _
    '                                Optional ByVal v_sSubBranchCode As String = Nothing)

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="r_oQuote"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub UpdateQuote(ByRef r_oQuote As Quote, _
    '                                    Optional ByVal v_sBranchCode As String = Nothing, _
    '                                    Optional ByVal v_iAgentKey As Integer = 0, _
    '                                    Optional ByVal v_sUserName As String = Nothing)

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="r_oQuote"></param>
    '    ''' <param name="v_iQuoteRiskIndex"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_sSubBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <param name="v_sTransactionType"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub UpdateRisk(ByRef r_oQuote As Quote, _
    '                                    ByVal v_iQuoteRiskIndex As Integer, _
    '                                    Optional ByVal v_sBranchCode As String = Nothing, _
    '                                    Optional ByVal v_sSubBranchCode As String = Nothing, _
    '                                    Optional ByVal v_iAgentKey As Integer = 0, _
    '                                    Optional ByVal v_sUserName As String = Nothing, _
    '                                    Optional ByVal v_sTransactionType As String = Nothing)

    '   ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_iInsuranceFileKey"></param>
    '    ''' <param name="v_oPayment"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_sTransactionType"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function BindQuote(ByVal v_iInsuranceFileKey As Integer, _
    '                                        ByVal v_oPayment As Payment, _
    '                                        Optional ByVal v_sBranchCode As String = Nothing, _
    '                                        Optional ByVal v_sTransactionType As String = Nothing) As PolicySummary


    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_sDataModelCode"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <param name="v_sDatasetDefinitionFileName"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetDatasetDefinition(ByVal v_sDataModelCode As String, _
    '                                                    Optional ByVal v_sBranchCode As String = Nothing, _
    '                                                    Optional ByVal v_iAgentKey As Integer = 0, _
    '                                                    Optional ByVal v_sUserName As Integer = Nothing, _
    '                                                    Optional ByVal v_sDatasetDefinitionFileName As String = Nothing) As String

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_sScreenCode"></param>
    '    ''' <param name="v_sXMLDataset"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function RunDefaultRulesAdd(ByVal v_sScreenCode As String, _
    '                                                ByVal v_sXMLDataset As String, _
    '                                                Optional ByVal v_sBranchCode As String = Nothing, _
    '                                                Optional ByVal v_iAgentKey As Integer = 0, _
    '                                                Optional ByVal v_sUserName As Integer = Nothing) As String

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_sScreenCode"></param>
    '    ''' <param name="v_sXMLDataset"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function RunDefaultRulesEdit(ByVal v_sScreenCode As String, _
    '                                                ByVal v_sXMLDataset As String, _
    '                                                Optional ByVal v_sBranchCode As String = Nothing, _
    '                                                Optional ByVal v_iAgentKey As Integer = 0, _
    '                                                Optional ByVal v_sUserName As Integer = Nothing) As String

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_oListType"></param>
    '    ''' <param name="v_sListCode"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetList(ByVal v_oListType As ListType, ByVal v_sListCode As String, _
    '                                    Optional ByVal v_sBranchCode As String = Nothing, _
    '                                    Optional ByVal v_iAgentKey As Integer = 0, _
    '                                    Optional ByVal v_sUserName As String = Nothing) As LookupListCollection

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_iRiskKey"></param>
    '    ''' <param name="r_oQuote"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub GetRisk(ByVal v_iRiskKey As Integer, _
    '                                ByRef r_oQuote As Quote, _
    '                                Optional ByVal v_sBranchCode As String = Nothing, _
    '                                Optional ByVal v_iAgentKey As Integer = 0, _
    '                                Optional ByVal v_sUserName As String = Nothing)

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="r_oAddress"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub AddAddress(ByRef r_oAddress As Address, _
    '                                    Optional ByVal v_iAgentKey As Integer = 0, _
    '                                    Optional ByVal v_sUserName As String = Nothing)

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_iAddressKey"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetAddress(ByVal v_iAddressKey As Integer, _
    '                                        Optional ByVal v_sBranchCode As String = Nothing, _
    '                                        Optional ByVal v_iAgentKey As Integer = 0, _
    '                                        Optional ByVal v_sUserName As String = Nothing) As Address

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="oSearchCriteria"></param>
    '    ''' <param name="v_iFindControlKey"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function FindControlSearch(ByVal oSearchCriteria As FindControlCriteriaCollection, _
    '                                            ByVal v_iFindControlKey As Integer, _
    '                                            Optional ByVal v_sBranchCode As String = Nothing) As Xml.XmlElement

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_iPartyKey"></param>
    '    ''' <param name="v_iInsuranceFileKey"></param>
    '    ''' <param name="v_iInsuranceFolderKey"></param>
    '    ''' <param name="v_sDocumentCode"></param>
    '    ''' <param name="v_oDocumentType"></param>
    '    ''' <param name="v_sDocumentExtractionDirectory"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GenerateDocument(ByVal v_iPartyKey As Integer, _
    '                                        ByVal v_iInsuranceFileKey As Integer, _
    '                                        ByVal v_iInsuranceFolderKey As Integer, _
    '                                        ByVal v_sDocumentCode As String, _
    '                                        ByVal v_oDocumentType As DocumentType, _
    '                                        ByVal v_sDocumentExtractionDirectory As String, _
    '                                        Optional ByVal v_sBranchCode As String = Nothing, _
    '                                        Optional ByVal v_iAgentKey As Integer = 0, _
    '                                        Optional ByVal v_sUserName As String = Nothing) As String

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_dAmountToFinance"></param>
    '    ''' <param name="v_dtStartDate"></param>
    '    ''' <param name="v_dtEndDate"></param>
    '    ''' <param name="v_dtPreferredDate"></param>
    '    ''' <param name="v_dtQuoteDate"></param>
    '    ''' <param name="v_iWeekDay"></param>
    '    ''' <param name="v_iMonthDay"></param>
    '    ''' <param name="v_iInsuranceFileKey"></param>
    '    ''' <param name="v_dOverrideInterestRate"></param>
    '    ''' <param name="v_dOverrideRate"></param>
    '    ''' <param name="v_bpaymentProtection"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetInstalmentQuotes(ByVal v_dAmountToFinance As Double, _
    '                                                ByVal v_dtStartDate As DateTime, _
    '                                                ByVal v_dtEndDate As DateTime, _
    '                                                ByVal v_dtPreferredDate As DateTime, _
    '                                                ByVal v_dtQuoteDate As DateTime, _
    '                                                ByVal v_iWeekDay As Integer, _
    '                                                ByVal v_iMonthDay As Integer, _
    '                                                ByVal v_iInsuranceFileKey As Integer, _
    '                                                ByVal v_dOverrideInterestRate As Double, _
    '                                                ByVal v_dOverrideRate As Double, _
    '                                                ByVal v_bPaymentProtection As Boolean, _
    '                                                Optional ByVal v_sBranchCode As String = Nothing) As InstallmentQuoteCollection

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="r_oParty"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub AddParty(ByRef r_oParty As BaseParty, _
    '                                Optional ByVal v_sBranchCode As String = Nothing, _
    '                                Optional ByVal v_iAgentKey As Integer = 0, _
    '                                Optional ByVal v_sUserName As String = Nothing)

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_iPartyKey"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetParty(ByVal v_iPartyKey As Integer, _
    '                                    Optional ByVal v_sBranchCode As String = Nothing) As BaseParty

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="r_oParty"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub UpdateParty(ByRef r_oParty As BaseParty, _
    '                                    Optional ByVal v_sBranchCode As String = Nothing, _
    '                                    Optional ByVal v_sSubBranchCode As String = Nothing, _
    '                                    Optional ByVal v_iAgentKey As Integer = 0, _
    '                                    Optional ByVal v_sUserName As String = Nothing)

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_iPartyKey"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetPartySummary(ByVal v_iPartyKey As Integer, _
    '                                            Optional ByVal v_sBranchCode As String = Nothing) As PartySummary

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_iInsuranceFolderKey"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetAllPolicyVersions(ByVal v_iInsuranceFolderKey As Integer, _
    '                                                    Optional ByVal v_sBranchCode As String = Nothing, _
    '                                                    Optional ByVal v_iAgentKey As Integer = 0, _
    '                                                    Optional ByVal v_sUserName As String = Nothing) As PolicyCollection

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_iInsuranceFileKey"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetHeaderAndSummariesByKey(ByVal v_iInsuranceFileKey As Integer, _
    '                                                    Optional ByVal v_sBranchCode As String = Nothing, _
    '                                                    Optional ByVal v_iAgentKey As Integer = 0, _
    '                                                    Optional ByVal v_sUserName As String = Nothing) As Quote

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_sInsuranceRef"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetHeaderAndSummariesByRef(ByVal v_sInsuranceRef As String, _
    '                                                    Optional ByVal v_sBranchCode As String = Nothing, _
    '                                                    Optional ByVal v_iAgentKey As Integer = 0, _
    '                                                    Optional ByVal v_sUserName As String = Nothing) As Quote

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_sOldPassword"></param>
    '    ''' <param name="v_sNewPassword"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub ChangePassword(ByVal v_sOldPassword As String, _
    '                                                ByVal v_sNewPassword As String, _
    '                                                Optional ByVal v_sBranchCode As String = Nothing)

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub ForgottenPassword(ByVal v_sUserName As String, _
    '                                            Optional ByVal v_sBranchCode As String = Nothing)

    '#Region "Sachin"

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="r_oMta"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_sSubBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub AddMtaQuote(ByRef r_oMta As MTA, _
    '                                Optional ByVal v_sBranchCode As String = Nothing, _
    '                                Optional ByVal v_sSubBranchCode As String = Nothing, _
    '                                Optional ByVal v_iAgentKey As Integer = 0, _
    '                                Optional ByVal v_sUserName As String = Nothing)

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_iInsuranceFileKey"></param>
    '    ''' <param name="v_oPayment"></param>
    '    ''' <param name="r_bTimeStamp"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_sTransactionType"></param>
    '    ''' <param name="r_oPolicy"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub BindMtaQuote(ByVal v_iInsuranceFileKey As Integer, _
    '                                    ByVal v_oPayment As Payment, _
    '                                    ByRef r_bTimeStamp() As Byte, _
    '                                    Optional ByVal v_sBranchCode As String = Nothing, _
    '                                    Optional ByVal v_sTransactionType As String = Nothing, _
    '                                    Optional ByRef r_oPolicy As PolicySummary = Nothing)


    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_iInsuranceFolderKey"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetMtaQuotes(ByVal v_iInsuranceFolderKey As Integer, _
    '                                Optional ByVal v_sBranchCode As String = Nothing) As MTACollection


    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="i_InsuranceFileKey"></param>
    '    ''' <param name="v_iInsuranceFolderKey"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_sSubBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function IsMtaQuoteValid(ByVal i_InsuranceFileKey As Integer, _
    '                                    Optional ByVal v_iInsuranceFolderKey As Integer = 0, _
    '                                    Optional ByVal v_sBranchCode As String = Nothing, _
    '                                    Optional ByVal v_sSubBranchCode As String = Nothing, _
    '                                    Optional ByVal v_iAgentKey As Integer = 0, _
    '                                    Optional ByVal v_sUserName As String = Nothing) As Boolean


    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="r_oQuote"></param>
    '    ''' <param name="v_iQuoteRiskIndex"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <param name="v_sSubBranchCode"></param>
    '    ''' <param name="v_iAgentKey"></param>
    '    ''' <param name="v_sUserName"></param>
    '    ''' <param name="v_sTransactionType"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub UpdateMtaRisk(ByRef r_oQuote As Quote, _
    '                                      ByVal v_iQuoteRiskIndex As Integer, _
    '                                      Optional ByVal v_sBranchCode As String = Nothing, _
    '                                      Optional ByVal v_sSubBranchCode As String = Nothing, _
    '                                      Optional ByVal v_iAgentKey As Integer = 0, _
    '                                      Optional ByVal v_sUserName As String = Nothing, _
    '                                      Optional ByVal v_sTransactionType As String = Nothing)


    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="r_oDocument"></param>
    '    ''' <param name="v_iDocNum"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub GetDocument(ByRef r_oDocument As Document, _
    '                                      ByVal v_iDocNum As Integer, _
    '                                      Optional ByVal v_sBranchCode As String = Nothing)


    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_iInsuranceFolderKey"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetDocumentList(ByVal v_iInsuranceFolderKey As Integer, _
    '                                      Optional ByVal v_sBranchCode As String = Nothing) As DocumentCollection

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetCurrenciesByBranch(Optional ByVal v_sBranchCode As String = Nothing) As CurrencyCollection


    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_sProductCode"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetRiskByProduct(ByVal v_sProductCode As String, _
    '                                      Optional ByVal v_sBranchCode As String = Nothing) As RiskCollection

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_ipartyKeyField"></param>
    '    ''' <param name="v_sxMLDataSetField"></param>
    '    ''' <param name="v_btimeStampField"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <remarks></remarks>
    '    Public MustOverride Sub UpdatePartyRisk(ByVal v_ipartyKeyField As Integer, _
    '                                     ByRef v_sxMLDataSetField As String, _
    '                                     ByRef v_btimeStampField() As Byte, Optional ByVal v_sBranchCode As String = Nothing)

    '    ''' <summary>
    '    ''' 
    '    ''' </summary>
    '    ''' <param name="v_iriskKeyField"></param>
    '    ''' <param name="i_InsuranceFileKey"></param>
    '    ''' <param name="v_sBranchCode"></param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public MustOverride Function GetRatingDetails(ByVal v_iriskKeyField As Integer, ByVal i_InsuranceFileKey As Integer, _
    '                                                  Optional ByVal v_sBranchCode As String = Nothing) As RatingCollection

    '#End Region

#End Region

#Region "New Method"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iAddressKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetAddress(ByVal v_iAddressKey As Integer,
                                        Optional ByVal v_iPartyKey As Integer = Nothing,
                                        Optional ByVal v_sBranchCode As String = Nothing) As Address


    Public MustOverride Function GetAllPolicyVersions(ByVal v_iInsuranceFolderKey As Integer,
                                         Optional ByVal v_sBranchCode As String = Nothing) As PolicyCollection



    Public MustOverride Function GetList(ByVal v_oListType As ListType,
                                ByVal v_sListCode As String,
                                ByVal v_bExcludeDeletedRecords As Boolean,
                                ByVal v_bExcludeEffectiveDate As Boolean,
                                Optional ByVal v_sParentFieldName As String = Nothing,
                                Optional ByVal v_iParentFieldValue As Integer = -1,
                                Optional ByVal v_sBranchCode As String = Nothing,
                                Optional ByRef v_sXmlElement As System.Xml.XmlElement = Nothing,
                                 Optional ByVal v_dEffectiveDate As Date = Nothing,
                                Optional ByVal v_sWhereClause As List(Of ListFilterOptions) = Nothing
                                ) As LookupListCollection
    'Optional ByVal v_sWhereColumn As List(Of String) = Nothing,
    'Optional ByVal v_sWhereOperator As List(Of String) = Nothing,
    'Optional ByVal v_sWhereValue As List(Of String) = Nothing

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iPartyKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public MustOverride Function GetParty(ByVal v_iPartyKey As Integer,
                                   Optional ByVal v_sBranchCode As String = Nothing) As BaseParty


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iPartyKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetPartySummary(ByVal v_iPartyKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As PartySummary


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iriskKeyField"></param>
    ''' <param name="i_InsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetRatingDetails(ByVal v_iriskKeyField As Integer,
                                  ByVal i_InsuranceFileKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As RatingCollection


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetCurrenciesByBranch(Optional ByVal v_sBranchCode As String = Nothing) As CurrencyCollection


    Public MustOverride Function GetOptionSetting(ByVal v_oOptionType As NexusProvider.OptionType,
                                  ByVal v_iOptionNumber As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As OptionTypeSetting
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetProductByAgent(Optional ByVal v_sBranchCode As String = Nothing) As ProductCollection


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFIleKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetRatingSectionTypes(ByVal v_iInsuranceFIleKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As RatingSectionTypesCollection

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sDataModelCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sDatasetDefinitionFileName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetDatasetDefinition(ByVal v_sDataModelCode As String,
                                  Optional ByVal v_sBranchCode As String = Nothing,
                                  Optional ByVal v_sDatasetDefinitionFileName As String = Nothing) As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oDocument"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>

    Public MustOverride Sub GetDocument(ByRef r_oDocument As Document,
                                           Optional ByVal v_sBranchCode As String = Nothing)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetAudittrailUser(ByRef r_oAudittrailUser As AuditTrail,
                                           Optional ByVal v_sBranchCode As String = Nothing) As AuditTrailCollection

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetAuditTrails(ByRef r_oAuditTrails As AuditTrail,
                                           Optional ByVal v_sBranchCode As String = Nothing) As AuditTrailCollection


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetAuditTrailModule(ByRef r_oAuditTrailModule As AuditTrail,
                                           Optional ByVal v_sBranchCode As String = Nothing) As AuditTrailCollection
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetUserDetails(ByVal sUserName As String, Optional ByVal bIsSSO As Boolean = False) As UserDetails
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFolderKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetDocumentList(ByVal v_iInsuranceFolderKey As Integer,
                                      Optional ByVal v_sBranchCode As String = Nothing) As DocumentCollection
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetHeaderAndSummariesByKey(ByVal v_iInsuranceFileKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing,
                                            Optional ByVal v_bIncludeGISRetoractiveDate As Boolean = False,
                                            Optional ByVal v_nPreChangeInsuranceFileKey As Integer = 0,
                                            Optional ByVal bExclusiveLock As Boolean = False) As Quote
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sInsuranceRef"></param>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetHeaderAndSummariesByRef(ByVal v_sInsuranceRef As String,
                                            ByVal v_iInsuranceFileKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As Quote
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oCurrency"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_iClaimKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetCurrencyExchangeRates(ByVal v_oCurrency As Currency,
                                            Optional ByVal v_sBranchCode As String = Nothing,
                                            Optional ByVal v_iClaimKey As Integer = Nothing) As Currency

    Public MustOverride Function ConvertCurrencyToBase(ByVal v_oParameters As ConvertCurrencytoBaseParameters,
                                                     Optional ByVal v_sBranchCode As String = Nothing) As ConvertCurrencytoBaseResponseParameters

    Public MustOverride Function GetCurrencyToCurrencyExchangeRate(ByVal sCurrencyCodeFrom As String,
                                                     ByVal sCurrencyCodeTo As String,
                                                     ByVal dCurrencyAmountUnRounded As Decimal,
                                        Optional ByVal v_sBranchCode As String = Nothing) As Currency


    Public MustOverride Function GetEventDetails(ByRef r_oEventDetails As EventDetails,
                                           Optional ByVal v_sBranchCode As String = Nothing) As EventDetailsCollection

    Public MustOverride Sub GetUserAuthorityValue(ByRef r_oUserAuthority As UserAuthority,
                                                             Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oPartySearchCriteria"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function FindParty(ByVal v_oPartySearchCriteria As PartySearchCriteria,
                                     Optional ByVal v_sBranchCode As String = Nothing,
                                     Optional ByVal sSearchType As String = "") As PartyCollection

    ''' <summary>
    '''  This method is used to Add a Address
    ''' </summary>
    ''' <param name="r_oAddress"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub AddAddress(ByRef r_oAddress As Address,
                            Optional ByVal v_sBranchCode As String = Nothing)
    ''' <summary>
    ''' This method is used to add a new party.
    ''' </summary>
    ''' <param name="r_oParty"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sSubBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub AddParty(ByRef r_oParty As BaseParty,
                                    Optional ByVal v_sBranchCode As String = Nothing,
                                    Optional ByVal v_sSubBranchCode As String = Nothing)
    ''' <summary>
    ''' This method is used to change the Password.
    ''' </summary>
    ''' <param name="v_sNewPassword">New password with Old logic</param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Function ChangePassword(ByVal v_sNewPassword As String, Optional ByVal v_sBranchCode As String = Nothing) As Boolean

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iPartyKey"></param>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_iInsuranceFolderKey"></param>
    ''' <param name="v_sDocumentCode"></param>
    ''' <param name="v_oDocumentType"></param>
    ''' <param name="v_sDocumentExtractionDirectory"></param>
    ''' <param name="v_iClaimKey"></param>
    ''' <param name="v_bSpoolDocumentOnly"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GenerateDocument(ByVal v_iPartyKey As Integer,
                                            ByVal v_iInsuranceFileKey As Integer,
                                            ByVal v_iInsuranceFolderKey As Integer,
                                            ByVal v_sDocumentCode As String,
                                            ByVal v_oDocumentType As DocumentType,
                                            ByVal v_sDocumentExtractionDirectory As String,
                                            Optional ByVal v_iClaimKey As Integer = 0,
                                            Optional ByVal v_bSpoolDocumentOnly As Boolean = False,
                                            Optional ByVal v_sBranchCode As String = Nothing,
                                            Optional ByVal v_sDocumentRef As String = Nothing,
                                            Optional ByVal v_bSkipArchiveonEdit As Boolean = False,
                                            Optional ByVal v_iMode As Integer = 4,
                                            Optional ByVal bIsSuppressArchive As Boolean = False,
                                            Optional ByVal sDocumentName As String = Nothing) As String

    Public MustOverride Sub AddEvent(ByRef r_oEventDetails As EventDetails,
                                  Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub AddClientDataExtractAuditTrail(ByVal v_iPartyKey As Integer,
                                                ByVal v_sClientCode As String, 
                                                Optional ByVal v_sBranchCode As String = Nothing)                                  

    Public MustOverride Sub AddEventNote(ByRef r_oEventDetails As EventDetails,
                                  Optional ByVal v_sBranchCode As String = Nothing)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function FindControlSearch(ByVal oSearchCriteria As FindControlCriteriaCollection,
                                            ByVal v_iFindControlKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As System.Xml.XmlElement
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oDocumentTemplates"></param>
    ''' <param name="v_sRiskTypeCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function FindDocumentTemplates(ByVal v_oDocumentTemplates As DocumentTemplate,
                                            ByVal v_sRiskTypeCode As String,
                                            Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_bViaClientManager As Boolean = Nothing) As DocumentTemplateCollection


    Public MustOverride Sub GetDatasetSchema(ByRef r_oRisk As Risk,
                                           Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iPartyKey"></param>
    ''' <param name="v_oContactCollection"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>

    Public MustOverride Sub ReplacePartyContact(ByVal v_iPartyKey As Integer,
                                             ByVal v_oContactCollection As ContactCollection,
                                             Optional ByVal v_sBranchCode As String = Nothing)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sScreenCode"></param>
    ''' <param name="v_sXMLDataset"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public MustOverride Function RunDefaultRulesAdd(ByVal v_sScreenCode As String,
                                                    ByVal v_sXMLDataset As String,
                                                    Optional ByVal v_sBranchCode As String = Nothing,
                                                    Optional ByVal v_bSkipDaveToDB As Boolean = True) As String


    Public MustOverride Function RunDefaultRulesEdit(ByVal v_sScreenCode As String,
                                                ByVal v_sXMLDataset As String,
                                                Optional ByVal v_oPerilSummary As PerilSummary = Nothing,
                                                Optional ByVal v_sBranchCode As String = Nothing,
                                                Optional ByVal v_sTransactionType As String = Nothing,
                                                Optional ByVal v_bSkipSaveToDB As Boolean = False,
                                                Optional ByVal nClaimPerilKey As Integer = 0,
                                                Optional ByVal v_dtInceptionTPI As Date = Nothing) As String


    Public MustOverride Sub UpdateParty(ByRef r_oParty As BaseParty,
                                Optional ByVal v_sBranchCode As String = Nothing,
                                Optional ByVal v_sSubBranchCode As String = Nothing)




    Public MustOverride Sub UpdatePartyRisk(ByVal v_iPartyKey As Integer,
                                         ByRef r_sXMLDataSet As String,
                                         ByRef r_bTimeStamp() As Byte,
                                         Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oClaimSearchCriteria"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function FindClaim(ByVal v_oClaimSearchCriteria As ClaimSearchCriteria,
                                        Optional ByVal v_sBranchCode As String = Nothing) As ClaimCollection
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oInsuranceFileForClaims"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function FindInsuranceFileForClaims(ByRef v_oInsuranceFileForClaims As InsuranceFileDetails,
                                         Optional ByVal v_sBranchCode As String = Nothing) As InsuranceFileDetailsCollection
    ''' <summary>
    ''' GetClaimDetails
    ''' </summary>
    ''' <param name="v_iClaimKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_iFetchAllVersionAmounts"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetClaimDetails(ByVal v_iClaimKey As Integer,
                                         Optional ByVal v_sBranchCode As String = Nothing,
                                         Optional ByVal v_iFetchAllVersionAmounts As Integer = 0,
                                         Optional ByVal bExclusiveLock As Boolean = False) As ClaimDetails
    ''' <summary>
    ''' To get claim insurers if the policy was a coinsurance policy.
    ''' </summary>
    ''' <param name="v_sClaimKey"></param>
    ''' <remarks></remarks>
    Public MustOverride Function GetClaimCoinsurer(ByVal v_sClaimKey As String,
                                         Optional ByVal v_sBranchCode As String = Nothing) As Claim

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oClaimReceipt"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Sub ClaimReceipt(ByRef r_oClaimReceipt As ClaimReceipt,
                                         Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal ClaimReceiptCollection As NexusProvider.ClaimReceiptCollection = Nothing)


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oClaimsDocument"></param>
    ''' <param name="v_oDocumentType"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public MustOverride Function GenerateClaimsDocuments(ByVal v_oClaimsDocument As ClaimDocument,
                                         ByVal v_oDocumentType As DocumentType,
                                         Optional ByVal v_sBranchCode As String = Nothing) As ClaimDocumentCollection


    ''' <summary>
    ''' To search the insurance file depending upon the entered criteria.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustOverride Function AddClaimRisk(ByVal v_iBaseClaimKey As Integer,
                                              ByVal v_bTimeStamp As Byte(),
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ClaimRisk

    ''' <summary>
    ''' To search the insurance file depending upon the entered criteria.
    ''' </summary>
    ''' <param name="v_iClaimPaymentKey"></param>
    ''' <param name="v_sComments"></param>
    ''' <param name="v_bDeclined"></param>
    ''' <param name="v_bTimeStamp"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_PaymentCashList"></param>
    ''' <param name="v_nAccountKey"></param>
    ''' <param name="v_dtPaymentDate"></param>
    ''' <param name="v_dtPaymentDateTo"></param>
    ''' <param name="v_sSourceIds"></param>
    ''' <param name="r_bIsUpdated"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub AuthoriseClaimPayment(ByVal v_iClaimPaymentKey As Integer,
                                                 ByVal v_sComments As String,
                                                 ByVal v_bDeclined As Boolean,
                                                 ByRef v_bTimeStamp() As Byte,
                                                 Optional ByVal v_sBranchCode As String = Nothing,
                                                 Optional ByVal v_PaymentCashList As NexusProvider.PaymentCashListItemType = Nothing,
                                                 Optional ByVal v_nAccountKey As Integer = 0,
                                                 Optional ByVal v_dtPaymentDate As DateTime = Nothing,
                                                 Optional ByVal v_dtPaymentDateTo As DateTime = Nothing,
                                                 Optional ByVal v_sSourceIds As String = "",
                                                 Optional ByRef r_bIsUpdated As Boolean = False,
                                                 Optional ByRef r_sFailureReason As String = "",
                                                Optional ByVal bExclusiveLock As Boolean = False)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oClaimPaymentTaxes"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public MustOverride Function GetClaimPaymentTaxes(ByVal r_oClaimPaymentTaxes As ClaimPayment,
                                                    Optional ByVal v_sBranchCode As String = Nothing,
                                                    Optional ByVal nGetSavedTaxOfPeril As Integer = 0) As ClaimPayment
    ''' <summary>
    ''' To get the ClaimPerilSummary
    ''' </summary>
    ''' <param name="r_oPeril"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns>Summary of Peril</returns>

    Public MustOverride Function GetClaimPerilSummary(ByRef r_oPeril As PerilSummary,
                                                Optional ByVal v_sBranchCode As String = Nothing) As PerilSummary

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sTypeCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetClaimReceiptTaxGroups(ByVal v_sTypeCode As String,
                                                Optional ByVal v_sBranchCode As String = Nothing) As TaxCollection

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oClaimReceipt"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub GetClaimReceiptTaxes(ByRef r_oClaimReceipt As ClaimReceipt,
                                                    Optional ByVal v_sBranchCode As String = Nothing,
                                                    Optional ByVal nGetSavedTaxOfPeril As Integer = 0)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iBaseClaimKey"></param>
    ''' <param name="v_iClaimKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public MustOverride Function GetClaimRisk(ByVal v_iBaseClaimKey As Integer,
                                              Optional ByVal v_iClaimKey As Integer = 0,
                                              Optional ByVal v_sBranchCode As String = Nothing) As ClaimRisk

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_iRiskKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public MustOverride Function GetClaimRiskLinks(ByVal v_iInsuranceFileKey As Integer,
                                                        ByVal v_iRiskKey As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ClaimRiskLinkCollection

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iBaseClaimKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public MustOverride Function GetClaimRiskReadOnly(ByVal v_iBaseClaimKey As Integer,
                                                     Optional ByVal v_sBranchCode As String = Nothing) As ClaimRisk

    ''' <summary>
    ''' To get ReinsurranceArrangementLines
    ''' </summary>
    ''' <param name="v_iClaimID"></param>
    ''' <param name="v_iArrangementID"></param>
    ''' <param name="v_iMode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_iAgentKey"></param>
    ''' <param name="v_sUserName"></param>
    ''' <returns>ReinsurranceArrangementLines</returns>

    Public MustOverride Function GetClaimReinsuranceArrangementLines(ByVal v_iClaimID As Integer,
                                                        ByVal v_iArrangementID As Integer,
                                                        ByVal v_iMode As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ArrangementLinesTypeCollection

    ''' <summary>
    ''' To get ReinsurranceArrangements
    ''' </summary>
    ''' <param name="v_iClaimID"></param>
    ''' <param name="v_iMode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_iAgentKey"></param>
    ''' <param name="v_sUserName"></param>
    ''' <returns>ReinsurranceArrangements</returns>

    Public MustOverride Function GetClaimReinsuranceArrangements(ByVal v_iClaimID As Integer,
                                                        ByVal v_iMode As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ReinsuranceArrangementLineCollection


    ''' <summary>
    ''' To get Reinsurance bands
    ''' </summary>
    ''' <param name="v_iClaimID"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_iAgentKey"></param>
    ''' <param name="v_sUserName"></param>
    ''' <returns>Reinsurancebands</returns>

    Public MustOverride Function GetClaimReinsurancebands(ByVal v_iClaimID As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ReinsuranceBandsCollection



    Public MustOverride Function GetClaimPaymentTaxGroups(ByVal v_iPartyKey As Integer,
                                                        ByVal v_oPartyType As NexusProvider.ClaimReceiptPartyTypeType,
                                                        ByVal v_oAdvancedTax As ClaimReceiptAdvancedTaxDetails,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As TaxCollection

    ''' <summary>
    ''' Adds a new quote and risk(s) (if provided in the quote object) to the data source
    ''' </summary>
    ''' <param name="r_oQuote">Quote</param>
    ''' <param name="v_sBranchCode">Branch Code, will default to the web.config branch code if not provided</param>
    ''' <param name="v_sSubBranchCode">Sub Branch Code</param>
    ''' <param name="v_iAgentKey">Agent Key</param>
    ''' <param name="v_sUserName">UserName</param>
    ''' <remarks></remarks>
    Public MustOverride Sub AddQuote(ByRef r_oQuote As Quote,
                                        Optional ByVal v_sBranchCode As String = Nothing,
                                        Optional ByVal v_sSubBranchCode As String = Nothing,
                                        Optional ByVal v_iAgentKey As Integer = 0)
    ''' <summary>
    ''' Adds a new quote and risk(s) (if provided in the quote object) to the data source
    ''' </summary>
    ''' <param name="r_oQuote">Quote</param>
    ''' <param name="v_sBranchCode">Branch Code, will default to the web.config branch code if not provided</param>
    ''' <param name="v_sSubBranchCode">Sub Branch Code</param>
    ''' <param name="v_iAgentKey">Agent Key</param>
    ''' <param name="v_sUserName">UserName</param>
    ''' <remarks></remarks>
    Public MustOverride Sub AddQuoteV2(ByRef r_oQuoteV2 As Quote,
                                Optional ByVal v_sBranchCode As String = Nothing,
                                Optional ByVal v_sSubBranchCode As String = Nothing,
                                Optional ByVal v_iAgentKey As Integer = 0)





    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_iQuoteRiskIndex"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sSubBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub AddRisk(ByRef r_oQuote As Quote,
                                ByVal v_iQuoteRiskIndex As Integer,
                               Optional ByVal v_sBranchCode As String = Nothing,
                                Optional ByVal v_sSubBranchCode As String = Nothing)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_oPayment"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function BindQuote(ByVal v_iInsuranceFileKey As Integer,
                                            ByVal v_oPayment As Payment,
                                            ByVal v_bTimeStamp As Byte(),
                                            Optional ByVal v_bAcceptRenewal As Boolean = False,
                                            Optional ByVal v_sBranchCode As String = Nothing,
                                            Optional ByVal v_sTransactionType As String = Nothing,
                                            Optional ByVal v_bIsBackDatedMTA As Boolean = False, Optional ByVal v_bWritePolicy As Boolean = False,
                                            Optional ByVal v_sOverriddenPolicyNumber As String = Nothing,
                                            Optional ByVal v_bPayNegativePremiumMTABalance As Boolean = False) As PolicySummary

    Public MustOverride Sub GetRisk(ByVal v_iRiskKey As Integer,
                            ByVal v_iQuoteRiskIndex As Integer,
                            ByRef r_oQuote As Quote,
                            Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_bIgnoreLocking As Boolean = False, Optional ByVal sRiskLinkStatusFlag As String = Nothing,
                            Optional ByVal bIsForEdit As Boolean = False)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sProductCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetRiskByProduct(Optional ByVal v_sProductCode As String = Nothing,
                                               Optional ByVal v_sBranchCode As String = Nothing) As RiskCollection

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sInsuranceRef"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function CheckUnpaidPremium(ByVal v_sInsuranceRef As String, Optional ByVal sClaimNumber As String = "",
                                                  Optional ByVal v_sBranchCode As String = Nothing) As CheckUnPaidDetails

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oQoute"></param>
    ''' <param name="v_iRiskIndex"></param>
    ''' <param name="v_sCopyType"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub CopyRisk(ByRef r_oQoute As Quote, ByVal v_iRiskNumber As Integer,
                                  ByVal v_iRiskIndex As Integer,
                                  ByVal v_sCopyType As CopyRiskTypes,
                                  Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_iRiskKey As Integer = 0)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_iRiskIndex"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub DeleteRisk(ByRef r_oQuote As Quote,
                                    ByVal v_iRiskIndex As Integer,
                                    Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_sTransactionType As String = Nothing,
                                    Optional ByVal v_iRiskKey As Integer = 0)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetCoInsuranceValues(ByVal v_iInsuranceFileKey As Integer,
                                                   Optional ByVal v_sBranchCode As String = Nothing) As CoinsuranceDefaults
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sUserName"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub ForgottenPassword(ByVal v_sUserName As String,
                                                Optional ByVal v_sBranchCode As String = Nothing)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateQuote(ByRef r_oQuote As Quote,
                                    Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_iQuoteRiskIndex"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sSubBranchCode"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateRisk(ByRef r_oQuote As Quote,
                                  ByVal v_iQuoteRiskIndex As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing,
                                  Optional ByVal v_sSubBranchCode As String = Nothing,
                                  Optional ByVal v_sTransactionType As String = Nothing,
                                  Optional ByVal v_bIgnoreErrorMessage As Boolean = False)


    'Public MustOverride Sub UpdateRiskStatus(ByVal v_iInsuranceFileKey As Integer, _
    '                                ByVal v_iRiskKey As Integer, ByVal v_RiskStatusType As NexusProvider.RiskStatusType, _
    '                                Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oQuoteV2"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sSubBranchCode"></param>
    ''' <param name="v_iAgentKey"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateQuotev2(ByRef r_oQuoteV2 As Quote,
                                  Optional ByVal v_sBranchCode As String = Nothing,
                                  Optional ByVal v_sSubBranchCode As String = Nothing,
                                  Optional ByVal v_iAgentKey As Integer = 0)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oQuoteV2"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sSubBranchCode"></param>
    ''' <param name="v_iAgentKey"></param>
    ''' <param name="v_sUserName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function UpdateRiskSelection(ByRef r_oQuote As Quote,
                                ByVal v_iQuoteRiskIndex As Integer,
                                Optional ByVal v_sBranchCode As String = Nothing) As Quote
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetCoinsuranceDefaults(Optional ByVal v_sBranchCode As String = Nothing) As CoinsuranceDefaultsCollection

    Public MustOverride Sub GetHeaderAndRisksByKey(ByRef v_oQuote As NexusProvider.Quote,
                                                         Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_iRiskKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetHeaderAndRiskTaxByKey(ByVal v_iInsuranceFileKey As Integer,
                                                          ByVal v_iRiskKey As Integer,
                                                          Optional ByVal v_sBranchCode As String = Nothing) As Quote

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetHeaderAndPolicyFeesByKey(ByVal v_iInsuranceFileKey As Integer,
                                                            Optional ByVal v_sBranchCode As String = Nothing) As Quote
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetBalancesAndUnallocatedCredits(ByVal v_iInsuranceFileKey As Integer,
                                                                 Optional ByVal v_sBranchCode As String = Nothing) As BalancesAndUnallocatedCredits

    ''' <summary>
    ''' To get instalment quotes for given parameters
    ''' </summary>
    ''' <param name="v_dAmountToFinance"></param>
    ''' <param name="v_dtStartDate"></param>
    ''' <param name="v_dtEndDate"></param>
    ''' <param name="v_dtPreferredDate"></param>
    ''' <param name="v_dtQuoteDate"></param>
    ''' <param name="v_iWeekDay"></param>
    ''' <param name="v_iMonthDay"></param>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_dOverrideInterestRate"></param>
    ''' <param name="v_dOverrideRate"></param>
    ''' <param name="v_bPaymentProtection"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_iInstallmentType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetInstalmentQuotes(ByVal v_dAmountToFinance As Double,
                                                    ByVal v_dtStartDate As DateTime,
                                                    ByVal v_dtEndDate As DateTime,
                                                    ByVal v_dtPreferredDate As DateTime,
                                                    ByVal v_dtQuoteDate As DateTime,
                                                    ByVal v_iWeekDay As Integer,
                                                    ByVal v_iMonthDay As Integer,
                                                    ByVal v_iInsuranceFileKey As Integer,
                                                    ByVal v_dOverrideInterestRate As Double,
                                                    ByVal v_dOverrideRate As Double,
                                                    ByVal v_bPaymentProtection As Boolean,
                                                    Optional ByVal v_sBranchCode As String = Nothing,
                                                    Optional ByVal v_iInstallmentType As InstalmentType = Nothing,
                                                    Optional ByRef r_nPartyBankId As Integer = 0,
                                                    Optional ByVal bIsUseTransactionCurrency As Boolean = False,
                                                    Optional ByVal sProcessPFMode As String = Nothing,
                                                    Optional ByVal v_OverrideDepositAmount As Double = -1.0,
                                                    Optional ByVal bOverrideCommission As Boolean = False,
                                                    Optional ByVal oPFTransactionCollection As FinancePlanTransactionsCollection = Nothing,
                                                    Optional ByVal iPremiumFinancekey As Integer = Nothing,
                                                    Optional ByVal iPremiumFinanceVersionKey As Integer = Nothing,
                                                    Optional ByVal bPreferredInstalmentDueDateonly As Boolean = False) As InstallmentQuoteCollection


    Public MustOverride Function GetHeaderAndAgentCommissionByKey(ByVal v_iInsuranceFileKey As Integer,
                                                                  Optional ByVal v_sBranchCode As String = Nothing) As HeaderAndAgentCommission
    Public MustOverride Function GetPaymentCashListDetails(ByVal v_iCashListKey As Integer,
                                     Optional ByVal v_sBranchCode As String = Nothing) As PaymentCashList

    Public MustOverride Function GetPaymentCashListItemDetails(ByVal v_iCashListItemKey As Integer,
                                 Optional ByVal v_sBranchCode As String = Nothing) As PaymentCashListItemType
    Public MustOverride Function GetPaymentCashListItems(ByVal v_iCashListKey As Integer,
                                Optional ByVal v_sBranchCode As String = Nothing) As PaymentCashListItemTypeCollection

    Public MustOverride Function GetPoliciesOnBankGuaranteeByKey(ByVal v_iBGKey As Integer,
                                 Optional ByVal v_sBranchCode As String = Nothing) As PolicyCollection

    Public MustOverride Function GetPoliciesOnBankGuaranteeForReceipt(ByVal v_iAccountKey As Integer,
                                    ByVal v_oBGGetPoliciesActionType As BGGetPoliciesActionTypeType,
                                    ByVal v_iPartyKey As Integer,
                                    Optional ByVal v_sBranchCode As String = Nothing) As BankGuaranteePolicy
    Public MustOverride Function GetPolicyBankGuarantee(ByVal v_iInsuranceFileKey As Integer,
                                    ByVal v_oBGPartyType As BGPartyTypeType,
                                    Optional ByVal v_sBranchCode As String = Nothing) As BankGuaranteeCollection

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetAccountBalance(Optional ByVal v_sBranchCode As String = Nothing) As AccountBalancecollection
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_BankGuarantee"></param>
    ''' <param name="v_iKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>   
    Public MustOverride Sub GetBankGuarantee(ByRef r_BankGuarantee As BankGuarantee,
                                ByVal v_iKey As Integer, Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetExistingInstalmentPlanPaymentDetails(ByVal v_iInsuranceFileKey As Integer,
                        Optional ByVal v_sBranchCode As String = Nothing) As Payment

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_iRiskKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetHeaderAndRiskFeesByKey(ByVal v_iInsuranceFileKey As Integer,
                                    ByVal v_iRiskKey As Integer,
                                    Optional ByVal v_sBranchCode As String = Nothing) As HeaderAndRisk

    Public MustOverride Sub AddAgentReceipt(ByVal v_PartyKey As Integer,
                                                ByRef r_oReceiptType As ReceiptType,
                                                Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function FindBank(ByVal v_oBankSearchCriteria As BankSearchCriteria,
                                       Optional ByVal v_sBranchCode As String = Nothing) As BankCollection

    Public MustOverride Sub ApproveCashListItem(ByRef r_CashListItemKey As PaymentCashListItemType,
                                                  Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function GetRecoveryCoinsurance(ByVal v_iClaimPerilKey As Integer,
                                                        ByVal v_bIsSalvage As Boolean,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As CoInsurersCollections

    Public MustOverride Function GetRecoveryReinsurance(ByVal v_iClaimPerilKey As Integer,
                                                        ByVal v_bIsSalvage As Boolean,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ReinsurancesCollection

    Public MustOverride Function GetReferredPayments(Optional ByVal v_oCashListItem As NexusProvider.CashListItems = Nothing,
                                                  Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_sReferredPaymentsBranchCode As String = Nothing) As CashListItemsCollection

    Public MustOverride Function GetAccountDetails(ByVal v_oAccountDetails As AccountDetails,
                                     Optional ByVal v_sBranchCode As String = Nothing) As AccountDetailsDefaults


    Public MustOverride Function GetAccountShortCodeFromParty(ByVal v_iPartyCnt As Integer,
                                     Optional ByVal v_sBranchCode As String = Nothing) As String


    Public MustOverride Function GetInsurerPayments(ByVal v_oAccountDetails As AccountDetails,
                                    Optional ByVal v_sBranchCode As String = Nothing) As AccountDetailsCollection

    Public MustOverride Function GetReceiptCashListDetails(ByVal v_iCashListKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As ReceiptCashListCollection

    Public MustOverride Function GetReceiptCashListItemDetails(ByVal v_iCashListItemKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As ReceiptCashList

    Public MustOverride Function GetReceiptCashListItems(ByVal v_iCashListKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As ReceiptCashListItemsCollection

    Public MustOverride Function GetTaxGroupsForClaims(ByVal v_bIs_withholding_tax As Boolean,
                                  Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_sTransactionTypeCode As String = Nothing) As TaxGroupCollection


    Public MustOverride Function GetTransactionDetails(ByVal v_iAccountKey As Integer,
                                  ByVal v_oAllocationDetailsCollections As AllocationDetailsCollections,
                                  Optional ByVal sShortCode As String = Nothing,
                                  Optional ByVal dtToDate As String = Nothing,
                                  Optional ByVal sInsuranceRef As String = Nothing,
                                  Optional ByVal bIsNewPF As Boolean = False,
                                  Optional ByVal v_sBranchCode As String = Nothing) As AllocationDetailsCollections

    ''' <summary>
    ''' Get Claim Recovery (CLR) transactions eligible for instalment plan creation.
    ''' </summary>
    ''' <param name="sShortCode">Client short code.</param>
    ''' <param name="sClaimNumber">Claim number filter.</param>
    ''' <param name="v_sBranchCode">Optional branch code.</param>
    ''' <returns>Collection of CLR transactions as FinancePlanTransactionsCollection.</returns>
    Public MustOverride Function GetClaimRecoveryTransactions(
                                  Optional ByVal sShortCode As String = Nothing,
                                  Optional ByVal sClaimNumber As String = Nothing,
                                  Optional ByVal v_sBranchCode As String = Nothing) As FinancePlanTransactionsCollection

    Public MustOverride Function GetUnallocatedClaimPayment(ByVal v_iAccountKey As Integer,
                                 ByVal v_dtPaymentDate As DateTime,
                                 ByVal v_dtPaymentDateTo As DateTime,
                                 Optional ByVal v_sShortCode As String = Nothing,
                                 Optional ByVal v_sBranchCode As String = Nothing) As UnallocatedClaimPaymentsCollection


    Public MustOverride Sub MarkUnmarkTransaction(ByRef v_oMarkUnmarkTransaction As MarkUnmarkTransaction,
                                 Optional ByVal v_sBranchCode As String = Nothing)


    Public MustOverride Sub AddMtaQuote(ByRef r_oMta As MTA,
                             Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub UpdateStandardPolicyWordings(ByRef oQuote As NexusProvider.Quote,
                                                    Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function AddBankGuarantee(ByVal r_oBankCollection As BankGuaranteeCollection,
                                     Optional ByVal v_sBranchCode As String = Nothing) As BankGuaranteeCollection

    Public MustOverride Function UpdateBankGuarantee(ByVal oBankCollection As BankGuaranteeCollection,
                    ByVal oBranchCollection As BranchCollection,
                    ByRef oProductCollection As ProductCollection,
                    Optional ByVal v_sBranchCode As String = Nothing) As BankGuaranteeCollection

    Public MustOverride Sub UpdateBankGuaranteeConditionally(ByVal oBankCollection As BankGuaranteeCollection,
                                                   Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function FindBankGuarantee(ByVal v_oBankGuarantee As BankGuarantee,
                                     Optional ByVal v_sBranchCode As String = Nothing) As BankGuaranteeCollection

    Public MustOverride Sub UpdateTaskGroups(ByRef oTaskGroup As TaskGroup,
                                              Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub UpdateTaskGroupTasks(ByRef oTaskGroup As TaskGroup,
                                              Optional ByVal v_sBranchCode As String = Nothing)


    Public MustOverride Sub UpdateUserGroup(ByRef r_oWorkManager As WorkManager,
                                        Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function GetWmTask(ByVal r_oWorkManager As WorkManager,
                                                  Optional ByVal v_sBranchCode As String = Nothing) As WorkManager

    Public MustOverride Function GetWmTaskLog(ByVal r_oWorkManager As WorkManager,
                                                 Optional ByVal v_sBranchCode As String = Nothing) As TaskLogCollection


    Public MustOverride Function GetWorkManagerScheduledTasks(ByVal oWorkManager As WorkManager,
                                           Optional ByVal v_sBranchCode As String = Nothing) As WorkManagerCollection

    Public MustOverride Sub UpdateWmTask(ByRef r_oWorkManager As WorkManager,
                                          Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub ReAssignMultipleWMTasks(ByVal v_oWorkManagerCollection As WorkManagerCollection,
                                                           Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub AddTaskGroup(ByRef v_oAddTaskGroup As TaskGroup,
                    Optional ByVal v_sBranchCode As String = Nothing)


    Public MustOverride Sub AddUserGroup(ByRef v_oAddUserGroup As UserGroup,
                Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub AddWmTaskLog(ByVal v_iTaskInstanceKey As Integer,
                ByVal v_sLogText As String,
                                              Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub DeleteUndeleteUserGroup(ByVal v_bDeleted As Boolean,
                                              ByVal v_sUserGroupCode As String,
                                              Optional ByVal v_sBranchCode As String = Nothing)


    Public MustOverride Sub DeleteWmTask(ByRef r_oWorkManager As WorkManager,
                             Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function FindUsers(ByVal v_oFindUserSearchCriteria As FindUsersSearchCriteria,
                    Optional ByVal v_sBranchCode As String = Nothing) As UserCollection

    Public MustOverride Function GetSubAgents(ByVal v_iInsuranceFileKey As Integer,
                            Optional ByVal v_sBranchCode As String = Nothing) As SubAgentCollection

    Public MustOverride Function GetTaskGroupTasks(ByVal v_iTaskGroupKey As Integer,
                                                ByVal v_dtEffectiveDate As Date,
                                                Optional ByVal v_sBranchCode As String = Nothing) As TaskGroupCollection

    Public MustOverride Function GetTaskGroups(Optional ByVal v_sBranchCode As String = Nothing) As TaskGroupCollection

    Public MustOverride Function GetUserGroupTaskGroups(ByVal v_sUserGroupCode As String,
                                            ByVal v_dtEffectiveDate As Date,
                                            Optional ByVal v_sBranchCode As String = Nothing) As TaskGroup

    Public MustOverride Function GetUserGroupUsers(ByVal v_sUserGroupCode As String,
                    ByVal v_dtEffectiveDate As Date, ByVal v_bRestrictUserList As Boolean,
                    ByVal v_bRestrictUserListSpecified As Boolean,
                    Optional ByVal v_sBranchCode As String = Nothing) As UserCollection

    Public MustOverride Function GetUserGroups(Optional ByVal v_sBranchCode As String = Nothing) As UserGroupCollection

    Public MustOverride Function GetUserGroupsbyTask(ByVal v_sTaskGroupCode As String,
                Optional ByVal v_sBranchCode As String = Nothing) As UserGroupCollection

    Public MustOverride Function FindCoverNoteBooks(ByRef v_oCoverNote As CoverNote,
                                       Optional ByVal v_sBranchCode As String = Nothing) As CoverNoteCollection

    Public MustOverride Sub AddCoverNoteBook(ByRef r_oCoverNote As CoverNote,
                                               Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub GetCoverNoteBook(ByRef r_oCoverNote As CoverNote,
                                            Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub UpdateCoverNoteBook(ByRef r_oCoverNote As CoverNote,
                                               Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub AddCoverNoteSheet(ByRef r_oCoverNote As CoverNote,
                                               Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub DeleteCoverNoteSheet(ByRef r_oCoverNote As CoverNote,
                                                      Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub GetCoverNoteSheet(ByRef r_oCoverNote As CoverNote,
                                                Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub UpdateCoverNoteSheet(ByRef r_oCoverNote As CoverNote,
                                                Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function GetEventNote(ByVal v_iEventKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As EventDetailsCollection

    Public MustOverride Sub DeleteRenewal(ByVal v_oQuote As Quote,
                                       Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub LapseRenewal(ByRef r_oQuote As Quote,
                                    ByVal v_sLapseReasonCode As String,
                                    Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub UpdateRenewalStatus(ByRef r_oQuote As Quote,
                                            ByVal v_sRenewalStatusCode As String,
                                            Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function GenerateInvite(ByVal v_oDocumentType As DocumentType,
                                            ByVal v_bSpoolDocumentOnly As Boolean,
                                            ByRef r_oQuote As Quote,
                                            ByVal v_sDocumentExtractionDirectory As String,
                                            Optional ByVal v_sBranchCode As String = Nothing) As String

    Public MustOverride Function GetPoliciesInRenewal(ByVal v_iPartyKey As Integer,
                                                ByVal v_iAgentKey As Integer,
                                                ByVal v_sProductCode As String,
                                                ByVal v_dtRenewalDate As DateTime,
                                                ByVal v_bForAccept As Boolean,
                                                ByVal v_bDirectOnly As Boolean,
                                                Optional ByVal v_sBranchCode As String = Nothing,
                                                Optional ByVal v_sInsuranceRef As String = Nothing,
                                                Optional ByVal v_bShowUserOnly As Boolean = False) As PolicyCollection

    Public MustOverride Function FindPolicy(ByVal v_sInsuranceRef As String,
                                      ByVal v_sRiskIndex As String,
                                      ByVal v_sClientShortName As String,
                                      ByVal v_oQuoteType As InsuranceFileTypes,
                                      ByVal v_bShowLapsedOnly As Boolean,
                                      Optional ByVal v_iMaxRowsToFetch As Integer = 0,
                                      Optional ByVal v_sBranchCode As String = Nothing,
                                      Optional ByVal v_bShowCancelledForEvents As Boolean = False) As InsuranceFileDetailsCollection

    Public MustOverride Function GetAgentDetailsForPolicy(ByVal v_iInsuranceFileKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As AgentDetailsForPolicy


    Public MustOverride Function GetAllocationDetails(ByVal v_iTransDetailKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As AllocationDetailsCollections

    Public MustOverride Function GetPoliciesForRenewalSelection(ByVal v_sProductCode As String,
                                                ByVal v_dtCompareDate As Date,
                                                ByVal v_dtStartDate As Date,
                                                Optional ByVal v_sBranchCode As String = Nothing) As PolicyCollection

    Public MustOverride Function GetRenewalStatus(ByVal v_iInsuranceFileKey As Integer,
                                                Optional ByVal v_sBranchCode As String = Nothing) As RenewalStatus

    Public MustOverride Sub RunRenewalAccept(ByVal v_iInsuranceFileKey As Integer,
                                            ByVal v_iBatchRenewalJobKey As Integer,
                                            ByVal v_iRecordsCount As Integer,
                                            ByVal v_sGUID As String,
                                            Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub RunRenewalSelection(ByVal v_iInsuranceFileKey As Integer,
                                            ByVal v_iBatchRenewalJobKey As Integer,
                                            ByVal v_iRecordsCount As Integer,
                                            ByVal v_sGUID As String,
                                            Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub RunRenewalInvite(ByVal v_iInsuranceFileKey As Integer,
                                        ByVal v_iBatchRenewalJobKey As Integer,
                                        ByVal v_iRecordsCount As Integer,
                                        ByVal v_sGUID As String,
                                        Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub RunRenewalSelectionByPolicy(ByRef r_oQuote As Quote,
                                                    Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function RunValidationRules(ByVal v_sScreenCode As String,
                                            ByRef v_sXMLDataSet As String,
                                            Optional ByVal v_iClaimKey As Integer = 0,
                                            Optional ByVal v_iClaimPerilKey As Integer = 0,
                                            Optional ByVal v_sBranchCode As String = Nothing,
                                            Optional ByVal v_sTransactionType As String = Nothing,
                                            Optional ByVal v_bSkipDaveToDB As Boolean = True) As String

    Public MustOverride Sub UpdateCoInsuranceValues(ByVal v_iInsuranceFileKey As Integer,
                                                ByVal v_bIsRecovered As Boolean,
                                                ByVal v_bIsSurcharged As Boolean,
                                                ByRef v_bTimeStampField() As Byte,
                                                ByVal v_oCoInsurers As CoInsurersCollections,
                                                Optional ByVal v_iDefaultId As Integer = Nothing,
                                                Optional ByVal v_sBranchCode As String = Nothing)



    Public MustOverride Function GetValidPrimaryCauses(ByVal v_iInsuranceFileKey As Integer,
                                                       Optional ByVal v_iMode As Integer = 0,
                                                       Optional ByVal v_bModeSpecified As Boolean = False,
                                        Optional ByVal v_sBranchCode As String = Nothing,
                                                    Optional ByVal v_bIncludeDeleted As Boolean = False) As PrimaryCausesCollections

    Public MustOverride Function GetVersionsForClaim(ByVal v_sClaimNumber As String,
                                            Optional ByVal v_sBranchCode As String = Nothing) As VersionsCollections

    Public MustOverride Sub PostDocument(ByVal v_oDocumentTypeType As DocumentTypeTypes,
                                           ByVal v_oTransactions As TransactionCollection,
                                           ByVal v_sComment As String,
                                           ByVal v_sDocumentTypeCode As String,
                                           ByRef v_sDocumentReference As String,
                                           ByVal v_iSAMStagingPolicyKey As Integer,
                                           Optional ByVal v_sBranchCode As String = Nothing)


    Public MustOverride Function GetRiskReinsuranceArrangementLines(ByVal v_iArrangementId As Integer,
                                                                        Optional ByVal v_sBranchCode As String = Nothing) As ArrangementLinesTypeCollection
    Public MustOverride Function GetRiskReinsuranceArrangements(ByVal v_iRiskKey As Integer,
                                                                Optional ByVal v_nVersionId As Integer = 0,
                                                                    Optional ByVal v_sBranchCode As String = Nothing) As ArrangementsTypeCollection
    Public MustOverride Function GetRiskReinsuranceBands(ByVal v_iRiskKey As Integer,
                                                           Optional ByVal v_nVersionId As Integer = 1,
                                                            Optional ByVal v_sBranchCode As String = Nothing) As ReinsuranceBandsCollection

    Public MustOverride Sub SaveRisk(ByRef r_oQuote As Quote, ByVal v_iQuoteRiskIndex As Integer,
                                      Optional ByVal v_sBranchCode As String = Nothing)


    Public MustOverride Function GetStandardPolicyWordings(ByVal v_iInsuranceFileKey As Integer,
                                                       Optional ByVal v_bGetFreshPolicyStandardWording As Boolean = False,
                                                       Optional ByVal v_sBranchCode As String = Nothing) As DocumentTemplateCollection


    Public MustOverride Sub UpdateSubAgents(ByRef v_oQuote As NexusProvider.Quote,
                                              ByVal v_oSubAgents As NexusProvider.SubAgentCollection,
                                              Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function UpdateUsersGroupUsers(ByVal v_oWorkManager As WorkManager,
                                                Optional ByVal v_sBranchCode As String = Nothing) As WorkManager

    Public MustOverride Function Updatestandardpolicywordings(ByRef v_oDocumentTemplate As DocumentTemplate,
                                                  Optional ByVal v_sBranchCode As String = Nothing) As DocumentTemplate
    Public MustOverride Sub AddPayNowReceipt(ByVal v_oAddPayNowReceipt As AddPayNowReceipt,
                                        Optional ByVal v_sBranchCode As String = Nothing)


    Public MustOverride Function AddWriteoff(ByVal v_oWriteoff As Writeoff,
                                        ByVal v_iTransactionKey As Integer,
                                        Optional ByVal v_sBranchCode As String = Nothing) As Integer

    Public MustOverride Function GetPolicyBankGuarantee(ByVal v_oBankGuarantee As BankGuarantee,
                                     Optional ByVal v_sBranchCode As String = Nothing) As BankGuaranteeCollection

    Public MustOverride Sub CreatePaymentCashListItem(ByVal v_iCashListKey As Integer,
                                                        ByRef v_oPaymentCashListCollection As PaymentCashListItemTypeCollection,
                                                        Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub CreatePaymentCashListWithItems(ByRef v_oPaymentCashListItem As PaymentCashListItemType,
                                                                 Optional ByVal v_sBranchCode As String = Nothing,
                                                                 Optional ByVal v_alTransDetailCollection As ArrayList = Nothing)


    Public MustOverride Function CreateReceiptCashListItem(ByRef r_oReceiptCashList As PaymentCashListItemType,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As PaymentCashListItemType

    Public MustOverride Function CreateReceiptcashListWithItem(ByRef r_oReceiptCashListItem As ReceiptCashListItemType,
                                                    Optional ByVal v_sBranchCode As String = Nothing) As ReceiptCashListCollection



    Public MustOverride Function OpenClaim(ByVal v_oClaimOpen As ClaimOpen,
                                            Optional ByVal v_sBranchCode As String = Nothing) As ClaimResponse

    Public MustOverride Function GetHeaderAndPolicyTaxByKey(ByVal v_iInsuranceFileKey As Integer,
                                                            Optional ByVal v_sBranchCode As String = Nothing) As Quote

    Public MustOverride Function FindAccounts(ByVal v_oAccountSearchCriteria As AccountSearchCriteria,
                                        Optional ByVal v_sBranchCode As String = Nothing) As AccountSearchResultCollection


    Public MustOverride Function MaintainClaim(ByVal v_oClaimMaintain As ClaimOpen,
                                            ByVal v_bTimeStamp As Byte(),
                                            Optional ByVal v_sBranchCode As String = Nothing) As ClaimResponse


    Public MustOverride Function UpdateClaimRisk(ByVal v_oClaimRisk As ClaimRisk,
                                     Optional ByVal v_sBranchCode As String = Nothing) As Byte()

    Public MustOverride Function UpdateAllocation(ByVal v_oAllocationDetails As AllocationDetails,
                                               Optional ByVal v_sBranchCode As String = Nothing) As Boolean

    Public MustOverride Function PayClaim(ByVal v_oClaimPayment As ClaimPayment,
                                    ByVal v_bTimeStamp As Byte(),
                                    Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal oClaimPaymentCollection As ClaimPaymentCollection = Nothing) As PayClaimResponse

    Public MustOverride Function GetProductRiskOptionValue(ByVal ActionType As NexusProvider.ProductConfigActionType,
                                                        ByVal ProductRiskOption As NexusProvider.ProductRiskOptions,
                                                        ByVal RiskTypeOption As NexusProvider.RiskTypeOptions,
                                                        ByVal ProductCode As String,
                                                        ByVal RiskTypeCode As String,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As String

    Public MustOverride Function GetClaimPartyDetails(ByVal v_iInsuranceFileKey As Integer,
                                                    Optional ByVal v_sBranchCode As String = Nothing) As InsuranceFileDetails

    Public MustOverride Sub CalculateTaxForClaims(ByRef v_oTaxForClaims As TaxForClaims,
                                                  Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function GetProductRiskEvents(ByVal v_iInsuranceFileKey As Integer,
                                                        ByVal v_sProductCode As String,
                                                        ByVal v_sEventType As String,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As LookupListCollection

    Public MustOverride Function GetBankAccounts(Optional ByVal v_iBankAccountKey As Integer = 0,
                                      Optional ByVal v_sBranchCode As String = Nothing) As AccountDetailsCollection

    Public MustOverride Function GetPartyPolicies(ByVal v_sPartyCode As String,
                                                  Optional ByVal v_sBranchCode As String = Nothing,
                                                  Optional ByVal v_nAgentKeyFilter As Integer = 0,
                                                  Optional ByVal v_sAgentTypeFilter As String = Nothing) As PartySummary

    Public MustOverride Function GetProductClaimsWorkflowOptions(ByVal ProcessType As NexusProvider.ClaimProcessType,
                                                         ByVal ProductCode As String,
                                                         Optional ByVal v_sBranchCode As String = Nothing) As ProductClaimsWorkflowOptionsValue

    Public MustOverride Function GetAgentSettings(ByVal v_iAgentKey As Integer,
                                                                Optional ByVal v_sBranchCode As String = Nothing) As AgentSettings

    Public MustOverride Function GetAccountingPeriod(Optional ByVal v_sBranchCode As String = Nothing) As LookupListCollection

    ' Begin WPR 64 - Commission Maintenance 
    Public MustOverride Function GetAgentCommission(ByVal v_iInsuranceFileKey As Integer,
                                                           Optional ByVal v_sBranchCode As String = Nothing,
                                                           Optional ByVal v_sRiskType As String = Nothing,
                                                           Optional ByVal v_sCommissionBand As String = Nothing) As EditAgentCommission

    Public MustOverride Function UpdateAgentCommission(ByVal oUpdateAgentCommission As EditAgentCommission,
                                          Optional ByVal v_sBranchCode As String = Nothing) As EditAgentCommission

    ' End WPR 64 - Commission Maintenance 
    ' Begin WPR - VB 64 - Media Type Status

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCashListReceipt"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function FindCashListReceipts(ByVal oCashListReceipt As CashListReceipt,
                                              Optional ByVal v_sBranchCode As String = Nothing) As CashListReceipts
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCashListReceipts"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateReceiptMediaTypeStatus(ByVal oCashListReceipts As CashListReceipts,
                                              Optional ByVal v_sBranchCode As String = Nothing)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oMediaTypeStatus"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub GetPolicyStatusForMediaTypeStatus(ByRef oMediaTypeStatus As MediaTypeStatus,
                                              Optional ByVal v_sBranchCode As String = Nothing)
    ' End WPR - VB 64 - Media Type Status 

    Public MustOverride Function UpdateClaimReservesOrPayments(ByVal v_oClaimOpen As Claim,
                                                          ByVal v_oClaimPayment As ClaimPayment,
                                                          ByVal v_bTimeStamp As Byte(),
                                                          ByVal v_iProcessType As Integer,
                                                          Optional ByVal v_sBranchCode As String = Nothing) As ClaimResponse

    Public MustOverride Function BindClaim(ByVal v_oClaimOpen As Claim,
                                               ByVal v_bTimeStamp As Byte(),
                                               ByVal v_iProcessType As Integer,
                                               ByVal v_oClaimPayment As ClaimPayment,
                                               Optional ByVal v_sBranchCode As String = Nothing,
                                               Optional ByVal bSkipSaveTransaction As Boolean = False, Optional ByVal ClaimPaymentCollection As NexusProvider.ClaimPaymentCollection = Nothing) As ClaimResponse

#Region "WPR85 & WPR12"

    'begin the methods used in the process of WPR85 & WPR12 

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sCashDepositRef"></param>
    ''' <param name="v_sPartyCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetCashDeposit(ByVal v_sPartyCode As String,
                                        ByVal v_sCashDepositRef As String,
                                        Optional ByVal v_sBranchCode As String = Nothing) As CashDeposit
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sPartyCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetNextCashDepositRef(ByVal v_sPartyCode As String,
                                                   Optional ByVal v_sBranchCode As String = Nothing) As CashDeposit
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCDColl"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function AddCashDeposit(ByVal oCDColl As NexusProvider.CashDepositCollection,
                                            Optional ByVal v_sBranchCode As String = Nothing) As CashDepositCollection
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCDColl"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function UpdateCashDeposit(ByVal oCDColl As NexusProvider.CashDepositCollection,
                                         Optional ByVal v_sBranchCode As String = Nothing) As CashDepositCollection
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sBankCode"></param>
    ''' <param name="v_sCashDepositRef"></param>
    ''' <param name="v_sPartyCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function FindCashDeposit(ByVal v_sPartyCode As String,
                                           ByVal v_sCashDepositRef As String,
                                           ByVal v_sBankCode As String,
                                           Optional ByVal v_iMaxRowsToFetch As Integer = 0,
                                           Optional ByVal v_sBranchCode As String = Nothing) As CashDepositCollection
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sPartyCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetLinkedCashDeposits(ByVal v_sPartyCode As String,
                                                 Optional ByVal v_sBranchCode As String = Nothing) As CashDepositCollection


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="InsuranceFileKey"></param>
    ''' <param name="GetCDsOf"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Function GetCashDepositsForPolicy(ByVal v_iInsuranceFileKey As Integer,
                                            ByVal v_oGetCDsOf As String,
                                           Optional ByVal v_sBranchCode As String = Nothing) As CashDepositsForPolicy

    'End the methods used for the process of WPR85 & WPR12 
#End Region


#End Region
    'UIICWR96 - Manual Journal
    Public MustOverride Sub AddJournal(ByVal v_oManualJournal As NexusProvider.ManualJournal, Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function ValidateAuthorizationSteps(ByVal v_oManualJournal As NexusProvider.ManualJournal, Optional ByVal v_sBranchCode As String = Nothing) As ManualJournalCollection

    Public MustOverride Function GetAgentCommissionTax(ByVal v_iInsuranceFileKey As Integer,
                                                ByVal v_dAgentCommissionAmount As Double,
                                                ByVal v_sCurrencyCode As String,
                                                ByVal v_sTaxGroupCode As String,
                                                Optional ByVal v_sBranchCode As String = Nothing) As TaxForClaims

    Public MustOverride Function GetRenewalAmountToFinance(ByVal v_iInsuranceFileKey As Integer,
                                                Optional ByVal v_sBranchCode As String = Nothing) As Decimal

    ''' <summary>
    ''' This SAM method return the Document Template
    ''' </summary>
    ''' <param name="v_sDocumentTemplateCode"></param>
    ''' <param name="v_iDocumentTemplateKey"></param>
    ''' <param name="v_sTemplateExtractionDirectory"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetStandardWordingsTemplate(ByVal v_sDocumentTemplateCode As String,
                                                         ByVal v_iDocumentTemplateKey As Integer,
                                                         ByVal v_sTemplateExtractionDirectory As String,
                                                         ByVal v_oDocumentFormatType As NexusProvider.DocumentFormatType,
                                                         Optional ByVal v_sBranchCode As String = Nothing,
                                                         Optional ByVal v_IsTXTextControlEnabled As Boolean = False) As String
    ''' <summary>
    ''' This SAM Method update the template
    ''' </summary>
    ''' <param name="v_sDocumentTemplateCode"></param>
    ''' <param name="v_iDocumentTemplateKey"></param>
    ''' <param name="v_sUpdatedTemplateLocation"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function UpdateStandardWordingsTemplate(ByVal v_sDocumentTemplateCode As String,
                                                          ByVal v_iDocumentTemplateKey As Integer,
                                                          ByVal v_sUpdatedTemplateLocation As String,
                                                          ByVal v_sTempFileLocation As String,
                                                            ByVal v_oDocumentFormatType As NexusProvider.DocumentFormatType,
                                                          Optional ByVal v_sBranchCode As String = Nothing,
                                                           Optional ByVal v_IsTxTextControlEnabled As Boolean = False) As DocumentTemplate


    ''' <summary>
    ''' This SAM Method Create Background Job
    ''' </summary>
    ''' <param name="v_sDescription"></param>
    ''' <param name="v_sJobXML"></param>
    ''' <param name="v_dJobWhenToStart"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Function CreateBackgroundJob(ByVal v_sDescription As String,
                                                     ByVal v_sJobXML As String,
                                                     ByVal v_dJobWhenToStart As Date,
                                                     Optional ByVal v_sBranchCode As String = Nothing) As Integer
    ''' <summary>
    ''' Returns the defaults for a given document template
    ''' </summary>
    ''' <param name="DocumentTemplateCodes">Comma separated list of document template codes</param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetDocumentDefaults(ByVal DocumentTemplateCodes As List(Of String),
                                        Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal DocumentTemplateKeys As List(Of String) = Nothing) As DocumentDefaultsCollection

    ''' <summary>
    ''' This Method is created to Get the SharePointFile List
    ''' </summary>
    ''' <param name="v_sPartyShortName"></param>
    ''' <param name="v_sPolicyNumber"></param>
    ''' <param name="v_sClaimNumber"></param>
    ''' <param name="v_sFolderPath"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_bCreateFolder"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetSharePointFileList(ByVal v_sPartyShortName As String,
                                                   Optional ByVal v_sPolicyNumber As String = Nothing,
                                                   Optional ByVal v_sClaimNumber As String = Nothing,
                                                   Optional ByVal v_sFolderPath As String = Nothing,
                                                   Optional ByVal v_sBranchCode As String = Nothing,
                                                   Optional ByVal v_bCreateFolder As Boolean = False) As SharepointFileList

    'WPR63 New Methods Declaration [Start]
#Region "Get Report"
    ''' <summary>
    ''' This method is used to generate report
    ''' </summary>
    ''' <param name="v_sReportName"></param>
    ''' <param name="v_oDocumentFormatType"></param>
    ''' <param name="v_oParameters"></param>
    ''' <param name="v_sDocumentExtractionDirectory"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Function GetReport(ByVal v_sReportName As String,
                               ByVal v_oDocumentFormatType As NexusProvider.DocumentFormatType,
                               ByVal v_oParameters As ParametersCollection,
                               ByVal v_sDocumentExtractionDirectory As String,
                               Optional ByVal v_sBranchCode As String = Nothing) As String
#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iriskKeyField"></param>
    ''' <param name="i_InsuranceFileKey"></param>
    ''' <param name="r_bTimeStamp"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateRatingSections(ByVal v_iriskKeyField As Integer,
                                  ByVal i_InsuranceFileKey As Integer,
                                   ByRef r_bTimeStamp() As Byte,
                                  ByVal oRatingCollection As RatingCollection,
                                  Optional ByVal v_sBranchCode As String = Nothing)

    ' Begin WPR - 15 

    ''' <summary>
    ''' This method is used to Get Taxes
    ''' </summary>
    ''' <param name="i_InsuranceFileKey"></param> 
    ''' <param name="v_sBranchCode"></param>
    ''' <returns>AllTaxesCollection</returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetTaxes(ByVal i_InsuranceFileKey As Integer,
                                 Optional ByVal v_sBranchCode As String = Nothing) As AllTaxes

    ''' <summary>
    ''' This method is used to Update Taxes
    ''' </summary>
    ''' <param name="v_oAllTaxesCollection"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateTaxes(ByVal v_oAllTaxesCollection As AllTaxesCollection,
                                    Optional ByVal v_sBranchCode As String = Nothing)

    ' End WPR - 15 

    Public MustOverride Sub CopyQuote(ByRef v_iInsuranceFileKey As Integer,
                               Optional ByRef v_iInsuranceFolderKey As Integer = 0,
                               Optional ByVal v_sBranchCode As String = Nothing,
                               Optional ByVal v_bCalledFromClonePolicy As Boolean = False,
                               Optional ByVal v_bIsQuoteVersioning As Boolean = False) 'CopyQuote

    Public MustOverride Sub CloneQuoteFromLivePolicy(ByRef v_iInsuranceFileKey As Integer,
                               Optional ByRef v_iInsuranceFolderKey As Integer = 0,
                               Optional ByVal v_sBranchCode As String = Nothing) 'CloneQuoteFromLivePolicy

    Public MustOverride Sub UpdateQuoteStatus(ByRef r_oQuote As Quote,
                                            Optional ByVal v_sBranchCode As String = Nothing) 'UpdateQuoteStatus

    Public MustOverride Sub DeletePolicy(ByRef r_oQuote As Quote,
                                            Optional ByVal v_sBranchCode As String = Nothing) 'DeletePolicy
    'WPR63 New Methods Declaration [End]
    'WPR48 New Method Declaration
    Public MustOverride Function GetPeriod(Optional ByVal v_bGetPaymentsAllocated As Boolean = False,
                                   Optional ByVal v_sBranchCode As String = Nothing) As PeriodCollection

    ''' <summary>
    ''' This method update the Policy/Risk fee 
    ''' </summary>
    ''' <param name="v_iFeeKey"></param>
    ''' <param name="v_bIsValue"></param>
    ''' <param name="v_dRate"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateFee(ByVal v_iFeeKey As Integer,
                                   ByVal v_bIsValue As Boolean,
                                   ByVal v_dRate As Decimal,
                                   Optional ByVal v_sBranchCode As String = Nothing)
#Region "PartyBank WPR12"

    Public MustOverride Sub AddPartyBankDetails(ByVal v_iPartyKey As Integer,
                                   ByRef v_PartyBankDetails As BankCollection,
                                   Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub UpdatePartyBankDetails(ByVal v_iPartyKey As Integer,
                                   ByVal vPartyBankDetails As BankCollection,
                                   Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub DeletePartyBankDetails(ByVal v_iPartyKey As Integer,
                                     ByVal vPartyBankDetails As BankCollection,
                                     Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function GetPartyBankDetails(ByVal v_iPartyKey As Integer,
                                   Optional ByVal v_sBranchCode As String = Nothing) As BankCollection

    Public MustOverride Function ValidateBankAccountNumber(ByVal v_iBankMediaID As Integer,
                                                           ByVal v_iBankCountryID As Integer,
                                                           Optional ByVal v_sAccountNumber As String = Nothing,
                                                           Optional ByVal v_sBankMediaCode As String = Nothing,
                                                           Optional ByVal v_sBranchCode As String = Nothing,
                                                           Optional ByVal sBIC As String = Nothing,
                                                           Optional ByVal sIBAN As String = Nothing,
                                                           Optional ByVal sBankName As String = Nothing) As ValidationDetailsCollection

    Public MustOverride Sub ManageBankDetails(ByVal v_iPartyKey As Integer,
                                   ByVal vPartyBankDetails As BankCollection,
                                   Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Sub ActivatePartyBankDetails(ByVal v_iPartyBankKey As Integer,
                                    ByVal vPartyBankDetails As BankCollection,
                                    Optional ByVal v_sBranchCode As String = Nothing)

#End Region  'PartyBank WPR12


#Region "WPR32 - PostCode and Address Validation"
    ''' <summary>
    ''' this method will fetch the address details on the basis of below mentioned parameters and will return address collection
    ''' </summary>
    ''' <param name="v_oAddress"></param>
    ''' <remarks></remarks>
    Public MustOverride Function FindAddress(ByVal v_oAddress As Address,
                                     Optional ByVal v_sBranchCode As String = Nothing) As AddressCollection

#End Region

#Region "WPR33-Backdated MTA"
    ''' <summary>
    ''' If MTAEffectiveDate is out of sync then this method need to be called.
    ''' This will replace the backdated versions and will create a new version.
    ''' This method will return backdated versions as collection in response.
    ''' If any validation becomes fail in SAM then validation message will be returned in FailureReason
    ''' property and BackDatedVersions collection will be nothing.
    ''' </summary>
    ''' <param name="oMta">MTA Details</param>
    ''' <param name="sFailureReason">By Ref parameter so value for this parameter will be return if any failure occurs</param>
    ''' <param name="v_sBranchCode">branch code</param>
    ''' <returns>backdated versions</returns>
    ''' <remarks></remarks>
    Public MustOverride Function AddBackdatedMtaQuote(ByVal oMta As MTA,
                                                ByRef sFailureReason As String,
                                                Optional ByVal v_sBranchCode As String = Nothing) As PolicyCollection


#End Region


    Public MustOverride Function GetRatingSectionByRiskType(ByVal RiskTypeCode As String, Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.RatingSectionTypesCollection

    'WPR14-MID
    Public MustOverride Function GetMidFiles(ByVal v_dtStartDate As Date,
                                            ByVal v_dtEndDate As Date,
                                            ByVal v_bFailuresOnly As Boolean,
                                            ByVal v_iMIDFileKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As MidFileCollection
    Public MustOverride Function GetMidFileDetails(ByVal v_bFailuresOnly As Boolean,
                                                   ByVal v_iMIDFileKey As Integer,
                                                   Optional ByVal v_sBranchCode As String = Nothing) As MidFileDetails
    'END WPR14-MID

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetFinancePlanDetails(ByVal v_iInsuranceFileKey As Integer,
                        Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal nPremiumFinanceCnt As Integer = 0,
                                                    Optional ByVal nPremiumFinanceVersion As Integer = 0) As FinancePlan

#Region "WPR34-Reversal of Allocations"
    ''' <summary>
    ''' This method is used to reverse the transaction allocation.
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_bIgnoreWarnings" ></param>
    ''' <param name="v_iAllocationKey" ></param>
    ''' <param name="v_iTransDetailKey" ></param>
    ''' <returns>WarningCollection</returns>
    ''' <remarks></remarks>
    Public MustOverride Function ReverseAllocation(ByVal v_iTransDetailKey As Integer,
                                            ByVal v_iAllocationKey As Integer,
                                            Optional ByVal v_bIgnoreWarnings As Boolean = True,
                                            Optional ByVal v_sBranchCode As String = Nothing) As String

#End Region


    'WPR 28
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oBaseCaseKey"></param>
    ''' <param name="v_oClaimKey"></param>
    ''' <param name="v_oIsLinked"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function CaseLinkUnlink(Optional ByVal v_oBaseCaseKey As Integer = 0,
                                    Optional ByVal v_oClaimKey As Integer = 0,
                                    Optional ByVal v_oIsLinked As Boolean = False,
                                    Optional ByVal v_sBranchCode As String = Nothing) As WarningCollection
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oCaseKey"></param>
    ''' <param name="v_oCaseNumber"></param>
    ''' <param name="v_oCaseOpenDate"></param>
    ''' <param name="v_oAssistant"></param>
    ''' <param name="v_oAnalyst"></param>
    ''' <param name="v_oProgressStatusCode"></param>
    ''' <param name="v_oEventDescription"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetCaseDetails(Optional ByVal v_oCaseKey As Integer = 0,
                                Optional ByVal v_oCaseNumber As String = Nothing,
                                Optional ByVal v_oCaseOpenDate As Date = Nothing,
                                Optional ByVal v_oAssistant As String = Nothing,
                                Optional ByVal v_oAnalyst As String = Nothing,
                                Optional ByVal v_oProgressStatusCode As String = Nothing,
                                Optional ByVal v_oEventDescription As String = Nothing,
                                Optional ByVal v_sBranchCode As String = Nothing) As CaseDetails

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oBaseCaseKey"></param>
    ''' <param name="v_oCaseKey"></param>
    ''' <param name="v_oEventDescription"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub CloseCase(Optional ByVal v_oBaseCaseKey As Integer = 0,
                                    Optional ByVal v_oCaseKey As Integer = 0,
                                    Optional ByVal v_oEventDescription As String = Nothing,
                                    Optional ByVal v_sBranchCode As String = Nothing)
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oCaseSearchCriteria"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function FindCase(ByVal v_oCaseSearchCriteria As CaseDetails,
                                        Optional ByVal v_sBranchCode As String = Nothing) As CaseCollection
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oCaseKey"></param>
    ''' <param name="v_oCaseNumber"></param>
    ''' <param name="v_oCaseOpenDate"></param>
    ''' <param name="v_oAssistant"></param>
    ''' <param name="v_oAnalyst"></param>
    ''' <param name="v_oProgressStatusCode"></param>
    ''' <param name="v_oEventDescription"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function SaveCase(Optional ByVal v_oCaseKey As Integer = 0,
                                        Optional ByVal v_oCaseNumber As String = Nothing,
                                        Optional ByVal v_oCaseOpenDate As Date = Nothing,
                                        Optional ByVal v_oAssistant As String = Nothing,
                                        Optional ByVal v_oAnalyst As String = Nothing,
                                        Optional ByVal v_oProgressStatusCode As String = Nothing,
                                        Optional ByVal v_oEventDescription As String = Nothing,
                                        Optional ByVal v_sBranchCode As String = Nothing) As CaseDetails


#Region "GetCashClaimLink"
    ''' <summary>
    ''' To GetCashClaimLink.
    ''' </summary>
    ''' <param name="v_iClaimPaymentKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetCashClaimLink(ByVal v_iClaimPaymentKey As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As CashClaimLink
#End Region

    ''' <summary>
    ''' To GetCashClaimLink.
    ''' </summary>
    ''' <param name="v_iTaxGroupId"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetTaxTypesAndBands(ByVal v_iTaxGroupId As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As TaxTypesAndBandsCollection


#Region "AddCashClaimLink"
    ''' <summary>
    ''' To AddCashClaimLink.
    ''' </summary>
    ''' <param name="v_iClaimPaymentKey"></param>
    ''' <param name="v_iCashListItemKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub AddCashClaimLink(ByVal v_iClaimPaymentKey As Integer,
                                               ByVal v_iCashListItemKey As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing)
#End Region

#Region "Reinsurance2007 WPR05"
    'Etana WPR 05
    ''' <summary>
    ''' GetReinsurance
    ''' </summary>
    ''' <param name="iRiskKey"></param>
    ''' <param name="bIsRI2007"></param>
    ''' <param name="sReturnXML"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetReinsurance(ByVal iRiskKey As Integer,
                                                Optional ByVal nVersionId As Integer = 0,
                                                Optional ByVal bIsRI2007 As Boolean = False,
                                                Optional ByRef sReturnXML As String = Nothing,
                                                Optional ByVal oReinsuranceBandsCollection As NexusProvider.ReinsuranceBandsCollection = Nothing) As DataSet
    ''' <summary>
    ''' GetReinsurance2007
    ''' </summary>
    ''' <param name="iRiskKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetReinsurance2007(ByVal iRiskKey As Integer,
                                                    Optional ByVal nVersionId As Integer = 0,
                                                    Optional ByVal oReinsuranceBandsCollection As NexusProvider.ReinsuranceBandsCollection = Nothing) As String

    ''' <summary>
    ''' GetClaimReinsurance
    ''' </summary>
    ''' <param name="iClaimKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetClaimReinsurance(ByVal iClaimKey As Integer) As DataSet

    ''' <summary>
    ''' GetClaimReinsurance2007
    ''' </summary>
    ''' <param name="iClaimKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetClaimReinsurance2007(ByVal iClaimKey As Integer) As String

    ''' <summary>
    ''' GetRIModelDetails
    ''' </summary>
    ''' <param name="v_oReinsurance2007"></param>
    ''' <param name="v_sRIModelCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub GetRIModelDetails(ByRef v_oReinsurance2007 As Reinsurances,
                                 ByVal v_sRIModelCode As String,
                                 Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' GetRIModelLineDetails
    ''' </summary>
    ''' <param name="v_oReinsurance2007"></param>
    ''' <param name="v_sRIModelCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub GetRIModelLineDetails(ByRef v_oReinsurance2007 As Reinsurances,
                                      ByVal v_sRIModelCode As String,
                                Optional ByVal v_sBranchCode As String = Nothing,
                                Optional ByVal v_bIsXOL As Boolean = False,
                                Optional ByVal v_iFilterType As Integer = 0,
                                    Optional ByVal v_lRIArrangementID As Long = 0)

    ''' <summary>
    ''' GetRITreatyPartyDetails
    ''' </summary>
    ''' <param name="v_oReinsurance2007"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Function GetRITreatyPartyDetails(ByRef v_oReinsurance2007 As Reinsurances,
                                            Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.RITreatyParty
    ''' <summary>
    ''' RIModelDetails
    ''' </summary>
    ''' <param name="v_sRIModelCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function RIModelDetails(ByVal v_sRIModelCode As String,
                                             Optional ByVal v_sBranchCode As String = Nothing,
                                Optional ByVal v_bIsXOL As Boolean = False,
                                Optional ByVal v_iFilterType As Integer = 0,
                                    Optional ByVal v_lRIArrangementID As Long = 0) As Reinsurances
    ''' <summary>
    ''' 'GetClaimReinsuranceArrangementLinesRI2007
    ''' </summary>
    ''' <param name="v_iClaimID"></param>
    ''' <param name="v_iArrangementID"></param>
    ''' <param name="v_iMode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetClaimRIArrangementLinesRI2007(ByVal v_iClaimID As Integer,
                                                        ByVal v_iArrangementID As Integer,
                                                        ByVal v_iMode As Integer,
                                                        ByVal v_bIsRecovery As Boolean,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ArrangementLinesTypeCollection
    ''' <summary>
    ''' GetRiskReinsuranceArrangementLinesRI2007
    ''' </summary>
    ''' <param name="v_iArrangementKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetRiskReinsuranceArrangementLinesRI2007(ByVal v_iArrangementKey As Integer,
                                                                        Optional ByVal v_sBranchCode As String = Nothing) As ArrangementLinesTypeCollection
    ''' <summary>
    ''' FindReinsurer
    ''' </summary>
    ''' <param name="v_oReinsurerSearchCriteria"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function FindReinsurer(ByVal v_oReinsurerSearchCriteria As ReinsurerSearchCriteria,
                                    Optional ByVal v_sBranchCode As String = Nothing) As ReinsurerCollection
    ''' <summary>
    ''' UpdateArrangementLinesRI2007
    ''' </summary>
    ''' <param name="v_oRIArrangementLines"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateArrangementLinesRI2007(ByVal v_oRIArrangementLines As ArrangementLinesTypeCollection,
                                  Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_dTotalSI As Double = 0)

    ''' <summary>
    ''' Update Arrangement Lines
    ''' </summary>
    ''' <param name="v_oRIArrangementLines"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateArrangementLines(ByVal v_oRIArrangementLines As ArrangementLinesTypeCollection, Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' UpdateClaimRIArrangementLinesRI2007
    ''' </summary>
    ''' <param name="v_oRIArrangementLines"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateClaimRIArrangementLinesRI2007(ByVal v_oRIArrangementLines As ArrangementLinesTypeCollection,
                                  Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' GetRIArrangementLineFromXML
    ''' </summary>
    ''' <param name="v_iRIArrangementLineKey"></param>
    ''' <param name="v_sXML"></param>
    ''' <param name="v_bIsClaim"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetRIArrangementLineFromXML(ByVal v_iRIArrangementLineKey As Integer,
                                                          ByVal v_sXML As String,
                                                          Optional ByVal v_bIsClaim As Boolean = False,
                                                          Optional ByVal v_sRIBAND As String = "",
                                                          Optional ByVal v_sTreatyCode As String = "") As ArrangementLinesType


    ''' <summary>
    ''' CheckFACXOLLimit
    ''' </summary>
    ''' <param name="v_sXML"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function CheckFACXOLLimit(ByVal v_sXML As String) As Boolean

    Public MustOverride Sub UpdateRIArrangementOverrideReason(ByVal v_iRIArrangementId As Integer, ByVal v_iOverrideReasonId As Integer)

    Public MustOverride Sub CalculateRITax(ByVal v_iRiskKey As Integer,
                                            ByVal v_iPartyKey As Integer,
                                            ByVal v_dPremium As Double,
                                            ByVal v_iInsuranceFileKey As Integer,
                                            ByRef r_dTaxPercentage As Double,
                                            Optional ByVal v_sBranchCode As String = Nothing)


#End Region
    Public Enum RowAction

        '''<remarks/>
        AddRow

        '''<remarks/>
        EditRow

        '''<remarks/>
        DeleteRow
    End Enum

    Public MustOverride Sub UpdateRiskStatus(ByVal v_iInsuranceFileKey As Integer,
                                     ByVal v_iRiskKey As Integer, ByVal v_RiskStatusType As NexusProvider.RiskStatusType,
                                      Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_dtRiskInceptionDate As DateTime = Nothing,
                                          Optional ByRef o_nTimeStamp As Byte() = Nothing)
    ''' <summary>
    ''' To settle multiple claim payments in one go
    ''' </summary>
    ''' <param name="v_oUnallocatedClaimPayments"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function SettleAllClaimPayments(ByVal v_oUnallocatedClaimPayments As UnallocatedClaimPaymentsCollection,
                                        Optional ByVal v_sBranchCode As String = Nothing) As SettleAllClaimPaymentsResults

    ''' <summary>
    ''' This method will return userpassword and password history for a valid user
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function ValidateUser(Optional ByVal v_sBranchCode As String = Nothing) As ValidateUserResponse

    ''' <summary>
    ''' Function to create work manager task
    ''' </summary>
    ''' <param name="v_oCreateWmTask"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function CreateWmTask(ByVal v_oCreateWmTask As WorkManager,
                                              Optional ByVal v_sBranchCode As String = Nothing) As WorkManager
    ''' <summary>
    ''' Function to Get Task on keys
    ''' </summary>
    ''' <param name="oKeyData"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetTaskOnKeys(ByVal oKeyDataCollection As KeyDataCollection,
                                              Optional ByVal v_sBranchCode As String = Nothing) As TaskCollection
    ''' <summary>
    ''' Functiona to update the task status
    ''' </summary>
    ''' <param name="oUpdateTaskStatusRequest"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateTaskStatus(ByVal oUpdateTaskStatusRequest As WorkManager,
                                             Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' This procedure update the market place policy status
    ''' </summary>
    ''' <param name="v_nInsuranceFileKey"></param>
    ''' <param name="v_bIsMarketPlacePolicy"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateMarketplacePolicyStatus(ByVal v_nInsuranceFileKey As Integer, ByVal v_bIsMarketPlacePolicy As Boolean, Optional ByVal v_sBranchCode As String = Nothing)



#Region "WPR 85 - Automatic cashlsit creation for salvage and TP Recovery"

    ''' <summary>
    ''' For generating a cashlist for given claim key
    ''' </summary>
    ''' <param name="v_iClaimId"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub GenerateCashList(ByVal v_iClaimId As Integer, Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' Get Default bank account details for given parameters
    ''' </summary>
    ''' <param name="v_sProductCode"></param>
    ''' <param name="v_iMediaTypeId"></param>
    ''' <param name="v_iCashListTypeId"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetDefaultBankAccountWithCurrency(ByVal v_sProductCode As String, ByVal v_iMediaTypeId As Integer,
                                     ByVal v_iCashListTypeId As Integer, Optional ByVal v_sBranchCode As String = Nothing) As BankAccountDefaults

#End Region

#Region "WPR 100B - Portfolio Transfer"
    ''' <summary>
    ''' To get policies in portfolio amendment
    ''' </summary>
    ''' <param name="v_sPolicyRef"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetPTPoliciesForAmend(Optional ByVal v_sPolicyRef As String = Nothing,
                                                       Optional ByVal v_sBranchCode As String = Nothing) As PolicyCollection
    ''' <summary>
    ''' To get policies in clone amendment
    ''' </summary>
    ''' <param name="v_sPolicyRef"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetClonePoliciesForAmend(Optional ByVal v_sPolicyRef As String = Nothing,
                                                          Optional ByVal v_sBranchCode As String = Nothing) As PolicyCollection
    ''' <summary>
    ''' To create financial postings for portfolio and clone amendments
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sProcessType"></param>
    ''' <param name="v_bTimeStamp"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub CreatePostingsForReinsurance(ByVal v_iInsuranceFileKey As Integer, ByVal v_sProcessType As String,
                                                         ByVal v_bTimeStamp As Byte(), Optional ByVal v_sBranchCode As String = Nothing)
    ''' <summary>
    ''' To calculate RI for clone process
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub RecalculateRIForCloneTransfer(ByRef r_oQuote As Quote, Optional ByVal v_sBranchCode As String = Nothing)
    ''' <summary>
    ''' To calculate RI for portfolio process
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub RecalculateRIForPortfolioTransfer(ByRef r_oQuote As Quote, Optional ByVal v_sBranchCode As String = Nothing)
    ''' <summary>
    ''' To update RI amendments status for portfolio and clone process
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_sProcessType"></param>
    ''' <param name="v_sStatus"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateRIAmendmentStatus(ByRef r_oQuote As Quote, ByVal v_sProcessType As String,
                                                     ByVal v_sStatus As String, Optional ByVal v_sBranchCode As String = Nothing)
    ''' <summary>
    ''' To check whether policy is pending for portfolio transfer
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="r_bIsPendingCloneTransfer"></param>
    ''' <param name="r_bIsPendingPortfolioTransfer"></param>
    ''' <param name="v_sInsuranceFileRef"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub IsPendingTransfer(ByVal v_iInsuranceFileKey As Integer, ByRef r_bIsPendingCloneTransfer As Boolean,
                                              ByRef r_bIsPendingPortfolioTransfer As Boolean, Optional ByVal v_sInsuranceFileRef As String = Nothing,
                                              Optional ByVal v_sBranchCode As String = Nothing)
    ''' <summary>
    ''' To calculate RI for portfolio process
    ''' </summary>
    ''' <param name="v_iRiskKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetRIVersion(ByVal v_iRiskKey As Integer,
                                              Optional ByVal v_sBranchCode As String = Nothing) As LookupListCollection

    ''' <summary>
    ''' To enable/disable anniversary date field for monthly policy.
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="r_bIsAnniversaryFieldEditable"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub IsAnniversaryDateEditable(ByVal v_iInsuranceFileKey As Integer, ByRef r_bIsAnniversaryFieldEditable As Boolean, Optional ByVal v_sBranchCode As String = Nothing)

#End Region
#Region "WPR18/18A - Recommender Functionality"
    ''' <summary>
    ''' To update the recommanded status
    ''' </summary>
    ''' <param name="v_nClaimKey"></param>
    ''' <param name="tsTimeStamp"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateRecommendStatus(ByVal v_nClaimKey As Integer,
                                                  ByRef tsTimeStamp() As Byte,
                                                  Optional ByVal v_sBranchCode As String = Nothing)


    ''' <summary>
    ''' This method is used to get the cash list item details for payment.
    ''' </summary>
    ''' <param name="v_nCashListItemKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetPaymentTypeCashListItem(ByVal v_nCashListItemKey As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.PaymentCashListItemTypeCollection


    ''' <summary>
    ''' To GetProductDocuments configured.
    ''' </summary>
    ''' <param name="v_sProductCode"></param>
    ''' <param name="v_nFunctionalArea"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetProductDocuments(ByVal v_sProductCode As String,
                                                  ByVal v_nFunctionalArea As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ProductDocumentsCollection
#End Region

    ''' <summary>
    ''' To get backdated MTA policy versions for a policy
    ''' </summary>
    ''' <param name="v_nInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetBackdatedMTAPolicyVersions(ByVal v_nInsuranceFileKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As PolicyCollection
    ''' <summary>
    ''' To cancel/delete a MTA quote
    ''' </summary>
    ''' <param name="oQuote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub CancelQuote(ByVal oQuote As Quote,
                                            Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function GenerateUniqueSSPExceptionRef() As String
    ''' <summary>
    ''' To delete the previously generated back dated versions
    ''' </summary>
    ''' <param name="v_nInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function DeleteBackDatedVersions(ByVal v_nInsuranceFileKey As Integer,
                                     Optional ByVal v_sBranchCode As String = Nothing) As PolicyCollection

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_nPartyKey"></param>
    ''' <param name="v_sRiskIndex"></param>
    ''' <param name="r_sInsuranceFileKeys"></param>
    ''' <param name="r_sRiskKeys"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub FindPoliciesByRiskIndex(ByVal v_nPartyKey As Integer, ByVal v_sRiskIndex As String, ByRef r_sInsuranceFolderKeys As String,
                                                  ByRef r_sInsuranceFileKeys As String,
                                                  ByRef r_sRiskKeys As String,
                                                  Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' CheckReserveRecovery
    ''' </summary>
    ''' <param name="nClaimKey"></param>
    ''' <param name="r_nClaimStatus"></param>
    ''' <param name="r_dCurrentReserve"></param>
    ''' <param name="r_dCurrentRecovery"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub CheckReserveRecovery(ByVal nClaimKey As Integer, ByRef r_nClaimStatus As Integer, ByRef r_dCurrentReserve As Decimal, ByRef r_dCurrentRecovery As Decimal, Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' Validates no active instalment plan exists for the given CLR recovery transaction.
    ''' </summary>
    Public MustOverride Function ValidateNoExistingInstalmentPlan(ByVal v_iClrTransactionId As Integer, Optional ByVal v_sBranchCode As String = Nothing) As Boolean

    ''' <summary>
    ''' This Method will Check Pending OOS Versions on policy
    ''' </summary>
    ''' <param name="v_nInsuranceFolderKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function CheckPendingOOSVersions(ByVal v_nInsuranceFileKey As Integer, ByVal v_nInsuranceFolderKey As Integer, Optional ByVal v_sBranchCode As String = Nothing) As BaseInsuranceFileKeyCollection

    ''' <summary>
    ''' To get latest policy version for given search criteria
    ''' </summary>
    ''' <param name="v_sInsuranceRef"></param>
    ''' <param name="v_sProductCode"></param>
    ''' <param name="v_sRecordType"></param>
    ''' <param name="v_nAgentId"></param>
    ''' <param name="v_sInsuredName"></param>
    ''' <param name="v_nMaxRowsToFetch"></param>
    ''' <param name="v_dtCoverStartDate"></param>
    ''' <param name="v_dtQuoteOrLiveDate"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function FindLatestPolicyVersions(ByVal v_sInsuranceRef As String, ByVal v_sProductCode As String,
                                            ByVal v_sRecordType As InsuranceQuoteType, ByVal v_nAgentId As Integer,
                                            ByVal v_sInsuredName As String, ByVal v_nMaxRowsToFetch As Integer, Optional ByVal v_dtCoverStartDate As Date = Nothing,
                                            Optional ByVal v_dtQuoteOrLiveDate As Date = Nothing, Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal bIncludeAssociates As Boolean = False, Optional ByVal v_sRiskIndex As String = Nothing) As InsuranceFileDetailsCollection

    Public MustOverride Sub ReverseCashListItem(ByVal v_TransdetailKey As Integer,
                                          ByVal v_CashListItemKey As Integer, ByVal v_ReverseReasonCode As String, Optional ByVal v_sBranchCode As String = Nothing)



    ''' <summary>
    ''' Check if document Template exists
    ''' </summary>
    ''' <param name="v_nPartyKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Function CheckDocumentTemplateExists(ByVal v_nPartyKey As Integer, Optional ByVal v_sBranchCode As String = Nothing) As Boolean
#Region "CancelMTAQuote"
    ''' <summary>
    ''' This is MustOverridable Function which  need override implementaion
    ''' </summary>
    ''' <param name="nInsuranceFileKey"></param>
    ''' <param name="sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function CancelMTAQuote(ByVal nInsuranceFileKey As Integer,
                                                Optional ByVal sBranchCode As String = Nothing, Optional ByVal v_bTimeStamp As Byte() = Nothing)
#End Region

    ''' <summary>
    ''' Get Lock Details
    ''' </summary>
    ''' <param name="sBranchCode"></param>
    ''' <returns>Lock Collection</returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetLockDetails(ByVal sBranchCode As String) As LockCollection

    ''' <summary>
    ''' Maintain Lock Details
    ''' </summary>
    ''' <param name="sBranchCode"></param>
    ''' <returns>Success failure code for unlocking the lock</returns>
    ''' <remarks></remarks>
    Public MustOverride Function MaintainLock(ByVal oClaimLocks As LockCollection,
                                              Optional ByVal bClearAll As Boolean = False,
                                              Optional ByVal bLogout As Boolean = False,
                                              Optional ByVal sBranchCode As String = Nothing) As Boolean



    ''' <summary>
    '''  Get premium finance plan collection
    ''' </summary>
    ''' <param name="nPartyKey"></param>
    ''' <param name="sStatusKey"></param>
    ''' <param name="sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetFinancePlan(ByVal nPartyKey As Integer,
                                            ByVal sStatusKey As String,
                                            Optional ByVal sBranchCode As String = Nothing) As FinancePlanCollection

    ''' <summary>
    ''' Get premium finance plans filtered by claim number.
    ''' </summary>
    ''' <param name="nPartyKey">Party key.</param>
    ''' <param name="sStatusKey">Status filter.</param>
    ''' <param name="sClaimNumber">Claim number filter.</param>
    ''' <returns>FinancePlanCollection matching the criteria.</returns>
    Public MustOverride Function GetFinancePlanByClaimNumber(ByVal nPartyKey As Integer,
                                            ByVal sStatusKey As String,
                                            ByVal sClaimNumber As String,
                                            Optional ByVal sBranchCode As String = Nothing) As FinancePlanCollection

    ''' <summary>
    ''' Get premium finance plan details by key
    ''' </summary>
    ''' <param name="sDocumentRef"></param>
    ''' <param name="nPFPremiumFinanceKey"></param>
    ''' <param name="nPFPremiumFinanceVersionKey"></param>
    ''' <param name="sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetHeaderAndSummariesPFPlanByKey(ByVal sDocumentRef As String,
                                                               ByVal nPFPremiumFinanceKey As Integer,
                                                                ByVal nPFPremiumFinanceVersionKey As Integer,
                                            Optional ByVal sBranchCode As String = Nothing,
                                                                  Optional ByVal bExclusiveLock As Boolean = False
                                                                  ) As PremiumFinancePlan

    ''' <summary>
    ''' This method is used for Processing Premium Finance Plan
    ''' </summary>
    ''' <param name="oProcessPfPlan"></param>
    ''' <param name="sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function ProcessPfPlan(ByRef oProcessPfPlan As PremiumFinancePlan,
                                                Optional ByVal sBranchCode As String = Nothing) As PremiumFinancePlan

    ''' <summary>
    ''' This method is used for Canceling Premium Finance Policies
    ''' </summary>
    ''' <param name="nPFPremiumFinanceKey"></param>
    ''' <param name="nPFPremiumFinanceVersionKey"></param>
    ''' <param name="sLapsedReasonCode"></param>
    ''' <param name="bSpoolDoc"></param>
    ''' <param name="bWriteOff"></param>
    ''' <param name="dtPolicyLapsedDate"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub CancelPFPolicies(ByVal nPFPremiumFinanceKey As Integer,
                                                ByVal nPFPremiumFinanceVersionKey As Integer,
                                                ByVal sLapsedReasonCode As String,
                                                ByVal bSpoolDoc As Boolean,
                                                ByVal bWriteOff As Boolean,
                                                ByVal dtPolicyLapsedDate As Date)

    ''' <summary>
    ''' This method is used for cancelling/settling/deleting Premium Finance Plans
    ''' </summary>
    ''' <param name="nPFPremiumFinanceKey"></param>
    ''' <param name="nPFPremiumFinanceVersionKey"></param>
    ''' <param name="sReasonCode"></param>
    ''' <param name="sRequestType"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub CancelPremiumFinancePlan(ByVal nPFPremiumFinanceKey As Integer,
                                                        ByVal nPFPremiumFinanceVersionKey As Integer,
                                                        ByVal sReasonCode As String,
                                                        ByVal sRequestType As String,
                                                        ByRef r_nDebitTransdetailKey As Integer)

    ''' <summary>
    ''' This method is used for updating the status of instalments
    ''' </summary>
    ''' <param name="nPFInstalmentKey"></param>
    ''' <param name="sStatusCode"></param>
    ''' <param name="sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateInstalmentStatus(ByVal nPFInstalmentKey As Integer,
                                                        ByVal sStatusCode As String,
                                                        ByVal sBranchCode As String)

    ''' <summary>
    ''' Finance plan info
    ''' </summary>
    ''' <param name="nInsuranceFileKey"></param>
    ''' <param name="sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function GetFinancePlanInformation(ByVal nInsuranceFileKey As Integer,
                                                    Optional ByVal sBranchCode As String = Nothing) As FinancePlanInformation

    ''' <summary>
    ''' Instalment settlement amount
    ''' </summary>
    ''' <param name="iInsuranceFileKey"></param>
    ''' <param name="InstalmentSettlementAmount"></param>
    ''' <param name="TransactionType"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub GetInstalmentSettlementAmount(ByVal iInsuranceFileKey As Integer, ByRef InstalmentSettlementAmount As Decimal, ByVal TransactionType As String, Optional ByVal v_sBranchCode As String = Nothing)




    ''' <summary>
    ''' This procedure return outstanding amount
    ''' </summary>
    ''' <param name="r_dOutstandingAmount"></param>
    ''' <param name="v_nInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    Public MustOverride Function GetPolicyOutstandingAmount(ByRef r_dOutstandingAmount As Decimal,
                                                           ByVal v_nInsuranceFileKey As Integer,
                                                            Optional ByVal v_sBranchCode As String = Nothing) As Decimal
    ''' <summary>
    ''' Delete Abandon Claims
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <param name="v_sBranchCode"></param>
    Public MustOverride Function DeletAbandonClaim(ByVal nClaimId As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As Boolean



    Public MustOverride Function GetClientDataExtract(ByVal nPartyCnt As Integer, ByVal sFilePassword As String) As Byte()

    Public MustOverride Sub ExecutePRERuleset(ByRef r_oQuote As Quote,
                                 ByVal v_iQuoteRiskIndex As Integer,
                                 Optional ByVal v_sBranchCode As String = Nothing,
                                 Optional ByVal v_sSubBranchCode As String = Nothing,
                                 Optional ByVal v_sTransactionType As String = Nothing,
                                 Optional ByVal v_bIgnoreErrorMessage As Boolean = False,
                                 Optional ByVal sPRERuleAssemblyName As String = "",
                                 Optional ByVal bRunPrePRERule As Boolean = False,
                                 Optional ByVal bRunPostPRERule As Boolean = False)




    ''' <summary>
    ''' Save Premium finance details on renewal quote
    ''' </summary>
    ''' <param name="oPayment"></param>
    ''' <param name="nInsuranceFileKey"></param>
    ''' <param name="sTransactionType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function SavePremiumFinanceDetails(ByVal oPayment As NexusProvider.Payment,
                                                           ByVal nInsuranceFileKey As Integer,
                                                           ByVal sTransactionType As String)


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nPFInstalmentKey"></param>
    ''' <param name="nPFInstalmentVersion"></param>
    ''' <param name="nInstalmentNo"></param>
    ''' <param name="dtDueDate"></param>
    ''' <param name="sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub UpdateInstalmentDetails(ByVal nPFInstalmentKey As Integer,
                                                        ByVal nPFInstalmentVersion As Integer,
                                                        ByVal nInstalmentNo As Integer,
                                                        ByVal dtDueDate As DateTime,
                                                        ByVal sBranchCode As String)

    ''' <summary>
    ''' Update Policy Payment Method
    ''' </summary>
    ''' <param name="v_sPaymentMethod"></param>
    ''' <param name="v_nInsuranceFileKey"></param>
    ''' <param name="r_oQuoteTimeStamp"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public MustOverride Function UpdatePolicyPaymentMethod(ByVal v_sPaymentMethod As String,
                                                       ByVal v_nInsuranceFileKey As Integer,
                                                       ByRef r_oQuoteTimeStamp As Byte())


    Public MustOverride Function GetPolicyAssociates(ByVal v_iInsuranceFileKey As Integer,
                               Optional ByVal v_iInsuranceFolderCnt As Integer = 0,
                               Optional ByVal v_sBranchCode As String = Nothing) As PolicyAssociateCollection


    Public MustOverride Function UpdatePolicyAssociates(ByVal v_oPolicyAssociate As PolicyAssociateCollection,
                            ByVal v_bTimeStamp As Byte(),
                           Optional ByVal v_sBranchCode As String = Nothing) As PolicyAssociate




    Public MustOverride Function ReverseCollectedInstalment(ByVal nPFInstalmentsID As Integer, ByVal sPFPlanStatusInd As String) As Integer

    ''' <summary>
    ''' GetRITreatyPartyDetailsWithTax
    ''' </summary>
    ''' <param name="r_oRITreatyPartyWithTax"></param>
    ''' <param name="v_sBranchCode"></param>
    Public MustOverride Sub GetRITreatyPartyDetailsWithTax(ByRef r_oRITreatyPartyWithTax As RITreatyPartyWithTax,
                                        Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' UpdateRiOverrideReasonInRiArrangement
    ''' </summary>
    ''' <param name="v_nRiArrangementId"></param>
    ''' <param name="v_nRiOverrideReasonId"></param>
    ''' <param name="v_sBranchCode"></param>
    Public MustOverride Sub UpdateRiOverrideReasonInRiArrangement(ByVal v_nRiArrangementId As Integer, ByVal v_nRiOverrideReasonId As Integer,
                                        Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' GetRIPropTreaties
    ''' </summary>
    ''' <param name="v_oRIPropTreaties"></param>
    ''' <param name="v_nRIArrangementKey"></param>
    ''' <param name="v_sTreatyType"></param>
    ''' <param name="v_sBranchCode"></param>
    Public MustOverride Sub GetRIPropTreaties(ByRef v_oRIPropTreaties As RIPropTreatiesCollection,
                                       Optional ByVal v_nRIArrangementKey As Integer? = Nothing,
                                       Optional ByVal v_sTreatyType As String = Nothing,
                                       Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' CheckRetainedCoInsurerExists
    ''' </summary>
    ''' <param name="v_CoInsurerKeys"></param>
    ''' <param name="v_sBranchCode"></param>
    Public MustOverride Function CheckRetainedCoInsurerExists(ByVal v_CoInsurerKeys As CoInsurersCollections,
                                       Optional ByVal v_sBranchCode As String = Nothing) As Boolean


    ''' <summary>
    '''  oGetUserPreferredColumnList
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    Public MustOverride Function GetUserPreferredColumnList(Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_sInterfaceName As String = Nothing) As UserPreferredColumnList

    ''' <summary>
    '''  UpdateSearchTransactionSelectedColumn
    ''' </summary>
    ''' <param name="oUserPreferredColumns"></param>
    ''' <param name="v_sBranchCode"></param>
    Public MustOverride Sub UpdateUserPreferredColumnList(ByRef oUserPreferredColumns As UserPreferredColumnList, Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function GetListofUnapprovedPayment(Optional ByVal v_oAuthorisedPaymentList As NexusProvider.AutorisedPaymentRequestType = Nothing) As NexusProvider.AuthorisedPaymentCollection
    ''' <summary>
    ''' UpdateAuthorizationComment
    ''' </summary>
    ''' <param name="oUpdateAuthorizationComment"></param>
    ''' <param name="v_sBranchCode"></param>
    Public MustOverride Sub UpdateAuthorizationComment(ByRef oUpdateAuthorizationComment As UpdateAuthorizationComment, Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' GetAuthorizationComment
    ''' </summary>
    ''' <param name="CashListItem_Id"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    Public MustOverride Function GetAuthorizationComment(ByVal CashListItem_Id As Integer, Optional ByVal v_sBranchCode As String = Nothing) As GetAuthorizationComment

    ''' <summary>
    '''  GetListofUnapprovedManualJournalTransactions
    ''' </summary>
    ''' <param name="v_oManualJournalTransactionRequestType"></param>

    Public MustOverride Function GetListofManualJournalTransactions(Optional ByVal v_oManualJournalTransactionRequestType As NexusProvider.AuthorisedManualJournalTransactionRequestType = Nothing) As NexusProvider.ManualJournalTransactionsCollection

    Public MustOverride Function GetListOfManualJournalTransactionApprovalMaster(ByVal manualJournalId As Integer, Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.ManualJournalTransactionApprovalMasterCollection

    Public MustOverride Function GetListOfManualJournalTransactionApprovalDetails(ByVal manualJournalId As Integer, Optional ByVal v_sBranchCode As String = Nothing) As NexusProvider.ManualJournalItemCollection

    ''' <summary>
    ''' UpdateAuthorizationComment
    ''' </summary>
    ''' <param name="oUpdateManualJournalApproversComment"></param>
    ''' <param name="v_sBranchCode"></param>
    Public MustOverride Sub UpdateManualJournalApproversComment(ByRef oUpdateManualJournalApproversComment As UpdateManualJournalApproversComment, Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function CallNamedStoredProcedure(ByVal oBaseParamter() As StoredProcedureParameterType, ByVal sStoredProcedureName As String, Optional ByVal v_sBranchCode As String = Nothing) As StoredProcedureResponseType
    ''' <summary>
    ''' CallNamedStoredProcedure
    ''' </summary>
    ''' <param name="v_sProcedureName"></param>
    ''' <param name="v_oParameters"></param>
    ''' <param name="v_bReport"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    Public MustOverride Function CallNamedStoredProcedure(ByVal v_sProcedureName As String,
                                  ByVal v_oParameters As NexusProvider.ParametersCollection,
                                  ByVal v_bReport As Boolean,
                                  Optional ByVal v_sBranchCode As String = Nothing) As System.Data.DataSet
    
    ''' <summary>
    ''' This method check if policy version can be voided
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="r_bIsValidVoidPolicyVersion"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub IsVoidPolicyVersion(ByVal v_iInsuranceFileKey As Integer, ByRef r_bIsValidVoidPolicyVersion As Boolean, ByRef r_bIsInstalmentExists As Boolean, ByRef r_bIsQuoteExists As Boolean, Optional ByVal v_sBranchCode As String = Nothing)

    ''' <summary>
    ''' This method will make the latest live version void 
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="r_bIsCreatedVoidPolicyVersion"></param>
    ''' <param name="r_sFailureMessage"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public MustOverride Sub CreateVoidPolicyVersion(ByVal v_iInsuranceFileKey As Integer, ByVal v_iInsuranceFolderKey As Integer, ByRef r_bIsCreatedVoidPolicyVersion As Boolean, ByRef r_sFailureMessage As String, Optional ByVal v_sBranchCode As String = Nothing)

    Public MustOverride Function GetCurrencyOverride(Optional ByVal v_sBranchCode As String = Nothing) As CurrencyOverride

    Public MustOverride Function GetInsuranceFileInformation(ByVal v_iInsuranceFileCnt As Integer,
                                                          Optional ByVal v_sBranchCode As String = Nothing) As GetInsuranceFileInformation

    Public MustOverride Function GetAccountIdFromPartyCnt(ByVal v_iPartyCnt As Integer,
                                                       ByVal v_iCompanyId As Integer,
                                                       Optional ByVal v_sBranchCode As String = Nothing) As Integer

    Public MustOverride Function DoCurrencyConversion(ByVal v_iAccountId As Integer,
                                                   ByVal v_iCompanyId As Integer,
                                                   ByVal v_iCurrencyId As Integer,
                                                   ByVal v_dCurrencyAmountUnrounded As Decimal,
                                                   ByVal v_iBaseCurrencyID As Integer,
                                                   ByVal v_cBaseAmount As Decimal,
                                                   ByVal v_iAccountCurrencyID As Integer,
                                                   ByVal v_cAccountAmount As Decimal,
                                                   ByVal v_iSystemCurrencyID As Integer,
                                                   ByVal v_cSystemAmount As Decimal,
                                                   ByVal v_dCurrencyBaseXrate As Double,
                                                   ByVal v_dtCurrencyBaseDate As Date,
                                                   ByVal v_dAccountBaseXrate As Double,
                                                   ByVal v_dtAccountBaseDate As Date,
                                                   ByVal v_dSystemBaseXrate As Double,
                                                   ByVal v_dtSystemBaseDate As Date,
                                                   Optional ByVal v_sBranchCode As String = Nothing) As DoCurrencyConversion

    Public MustOverride Function UpdateInsuranceFile(ByVal v_iInsuranceFileCnt As Integer,
                                             Optional ByVal v_dCurrencyBaseRate As Double? = Nothing,
                                             Optional ByVal v_dtCurrencyBaseDate As DateTime? = Nothing,
                                             Optional ByVal v_dAccountBaseRate As Double? = Nothing,
                                             Optional ByVal v_dtAccountBaseDate As DateTime? = Nothing,
                                             Optional ByVal v_dSystemBaseRate As Double? = Nothing,
                                             Optional ByVal v_dtSystemBaseDate As DateTime? = Nothing,
                                             Optional ByVal v_iRateOverrideReasonID As Integer? = Nothing,
                                             Optional ByVal v_iBaseCurrencyID As Integer? = Nothing,
                                             Optional ByVal v_iAccountCurrencyID As Integer? = Nothing,
                                             Optional ByVal v_sBranchCode As String = Nothing) As Boolean



    ' Policy Discount
    Public MustOverride Function ApplyPolicyDiscount(ByVal v_iInsuranceFileKey As Integer, ByVal v_iProductId As Integer, ByVal v_sTransactionType As String, ByVal v_iTask As Integer, ByRef r_sFailureReason As String, Optional ByVal v_crAppliedDiscountPremium As Decimal = 0, Optional ByVal v_dAppliedDiscountPercentage As Double = 0, Optional ByVal v_iAppliedMatchDiscountPremium As Integer = 0, Optional ByVal v_iAppliedDiscountReasonId As Integer = 0, Optional ByVal v_iAppliedDiscountRecurringTypeId As Integer = 0) As Boolean

    Public MustOverride Function RollbackPolicyDiscount(ByVal v_iInsuranceFileKey As Integer, ByVal v_iProductId As Integer, ByVal v_sTransactionType As String, ByVal v_iTask As Integer) As Boolean

    Public MustOverride Function GetPolicyDiscountInfo(ByVal v_iInsuranceFileKey As Integer) As PolicyDiscount

    Public MustOverride Function GetPolicyDiscountTotalPremium(ByVal v_iInsuranceFileKey As Integer) As Decimal

    Public MustOverride Function GetInvalidPolicyDiscountRisks(ByVal v_iInsuranceFileKey As Integer) As Object(,)

End Class