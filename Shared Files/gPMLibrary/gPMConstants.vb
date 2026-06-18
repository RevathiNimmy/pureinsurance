Option Strict Off
Option Explicit On
Imports System
Imports System.Globalization
<System.Runtime.InteropServices.ProgId("gPMConstants_NET.gPMConstants")> _
Public Module gPMConstants

    ' ***************************************************************** '
    ' Module Name: gPMConstants
    '
    ' Date: 14th January 1998
    '
    ' Description: This class maps ALL of the constants currently
    '              provided by PMConst.bas
    '
    '              Numeric constants are implemented by Enums.
    '
    '              Note: To maintain backwards compatibility the enum
    '              members will still be prefixed 'PM', NOT 'pme'. The
    '              enum itself will be prefixed PME however as this will
    '              not have been used in any previous code.
    '
    '              String constants are implemented by a Property Get
    '
    ' Edit History: 14/01/98    Original created                     RFC
    '
    ' BB030298 - Added DisplayMode enum for Navigator style variation
    ' BB040298 - Added Control Type constants for FormControl
    ' RFC270298 - Navigator Key Names moved to gSirLibs as they are
    '             Sirius specific.
    ' RFC270298 - Type of Business removed as it is not needed.
    ' RFC040398 - DSN's Added for Mercury, Documaster, DocumasterV2
    ' RFC040398   and DocumasterScan.
    ' RFC250398 - DSN's added for Sirius Architecture DB
    ' RFC070498 - Broking Link Constants Added
    ' RFC200498 - Architecture In Debug Reg Key constant added.
    ' RFC210498 - Architecture Local Enabled Reg Key constant added.
    ' RFC050698 - Sirius Broking DSN added.
    ' RFC080598 - LogMessage Default Log File changed to "C:\Sirius.Log"
    ' RFC170698 - Architecture Server Enabled Reg Key constant added.
    ' RFC190698 - Format Month & Day Long, Medium and Short added
    ' RFC190898 - QueryTimeoutSeconds Reg Key Added.
    ' RFC190898 - SiriusSolutions & Nirvana Product Family and DSN's added.
    '             Note: These are all set to use the Sirius DSN at present.
    ' RFC161098 - Added IncorrectDateFormat & SystemDate return codes.
    ' RFC140199 - Added SplashBitMap registry key and Abort/Complete Nav Actions.
    ' RFC180299 New Constants Added for SA1.4
    ' RFC060799 - Added GeminiII Product Family, DSN etc etc
    ' DAK280999 - New Process Mode constants
    ' DAK071099 - Work Manager constants moved here.
    ' DAK121099 - New responses for Licencing
    ' DAK051199 - Privilege levels for amending lookup tables
    ' DAK011299 - More lookup table privilege levels
    ' DAK141299 - Add is_visible column to task instance
    ' DAK241299 - More registry settings for Work Manager
    ' DAK110100 - More registry settings for Work Manager
    ' DAK250100 - Registry setting to allow error retry in Navigator
    ' DAK190600 - Registry setting to determine "PM News" tab name on Work Manager
    ' DAK110700 - Registry setting for Work Manager Main Form Caption
    '             Registry setting for PM Support web address
    ' RDC12062002 - changed to BAS module
    ' RDC12062002 - Constants and PMConstants merged
    ' ME29112002  - Added Constants for field manager additional sproc params
    ' AMB 21/01/2003 - Added workflow_information field - IAG Spec 229
    ' RAW 15/07/2003 : CQ258 : added PMLookupIsDeleted
    ' RAW 25/07/2003 : CQ258 : added PMLookupAllWithDeleted
    ' CLG 01/09/2004 : RFC71 Added product option to allow spool and archive of documents
    ' VB  14/02/2005 : PN-18426 : Added Enum in PMEControlType (PMAccountLookup=8) for UserControl 'AccountLookup'
    ' ***************************************************************** '

    ' ******************************************************************
    ' Moved from gSirLibraries Start
    ' ******************************************************************

    '   Enumeration For Hidden Options
    '   Please also add to populateArray function and
    '   redefine the Array
    Public Enum SIRHiddenOptions
        SIROPTUnderwriting = 1
        SIROPTIsNRMA = 2
        SIROPTValidateAlternativeIdentifier = 3
        SIROPTChaserLettersEnabled = 4
        SIROPTAONAffinity = 5
        SIROPTRestrictedInsurerAccess = 6
        SIROPTClientPolicyLinkage = 7
        SIROPTAsynchronousPosting = 8
        SIROPTClientSummary = 9
        SIROPTEnhancedOrionSecurity = 10
        SIROPTUseRetailLogicLink = 11
        SIROPTClaimsBuilder = 12
        SIROPTFutureDateAddressChanges = 13
        SIROPTFishInstalled = 14
        SIROPTAllowRegSearch = 15
        SIROPTMultiTreeAccounting = 16
        SIROPTLossSchedule = 17
        SIROPTRiskVariations = 18
        SIROPTAutomaticCreditProcessing = 19
        ' Allows restriction of account information to current user's companies
        SIROPTAccountSegregation = 20
        ' Show or hide the public/private notes
        SIROPTHidePublicPrivateNotes = 21
        ' Allows restriction of account information to current user's companies
        SIROPTMultiBranchCoreAccounts = 22
        SIROPTAllowChangeOfNCDAtRenewal = 23
        SIROPTIncludeInstalmentsOnRoadmap = 24
        SIROPTHidePolicyDatesOnSchemeSelectScreen = 25
        SIROPTRemoveNone = 26
        SIROPTSpoolAsHTML = 27
        ' Hide Letter Question for roadmaps
        SIROptHideRoadmapLetterQuestion = 28
        SIROPTAdmiralForceRisks = 29
        SIROPTLimitPersonalClientEditFields = 30
        SIROPTSetToQuoteAfterOverride = 31
        ' Interest Rate and Commissions Rate in Instalments.
        SIROPTAllowInstalmentsOverride = 32
        ' Manage salvage within claims
        SIROPTManageSalvage = 33
        SIROPTEnableRiskScreenEditing = 34
        SIROPTEnableClaimVersions = 35
        SIROPTShareDisclosures = 36
        SIROPTEnableBranchSelectAtLogon = 37
        ' Switch on AUA specific code
        SIROPTIsAUA = 38
        ' Force the Client Code to be numeric
        SIROPTForceNumericClientCode = 39
        ' Linking Account Executives To Commission Accounts.
        SIROPTLinkCommACCToACCExec = 40
        ' TPU requirement to cache the screen details
        SIROPTCacheTransDetail = 41
        ' TPU requirement to show Credit/Debit details on FindTransactions
        SIROPTDisplayDebitCredit = 42
        ' Control whether account handler parties can be assigned to a branch other than HO
        SIROPTAccountHandlerIsMultiBranch = 43
        ' Remove non-printing graphics from docs
        SIROPTNonPrintingGraphics = 44
        ' Allow Sirius View functionality in Documaster
        ' (Document Issuance changes)
        SIROPTSiriusViewInDocumaster = 45
        ' User Definable (party) Fields
        SIROPTUserDefinablePartyFields = 46
        ' Enable option to force a different user to pay a Binder Payment to the user who created it
        SIROPTInsurerPaymentPayUserCheck = 47
        ' Show renewal frequency on Schemes Select to allow alternative renewal dates for schemes new business policy.
        SIROPTShowRenewalFrequencyForSchemesNB = 48
        ' MEvans : 27-05-2003 : 223 - Show Recovery button on Claims Risk Screen
        SIROPTThirdPartyRecovery = 49
        ' Generate Credit Control Items for each Instalment
        ' This is used for sending out Invoices for instalments in advance.
        SIROPTGenerateAdvanceCreditControlForInstalments = 50
        ' Run Claims Authorisation Script
        ' for now used by TPU /PRU but can be sold as independent feature.
        'SIROPTRunClaimsAuthorisationScript = 51
        'For ability to select default subbranch against each branch
        SIROPTDefaultSubBranch = 52
        'for limiting policies to client source
        SIROPTLimitPolicySource = 53
        'for by default no branch access to users
        SIROPTNoBranchAccess = 54
        'for User group / branch relationship
        SIROPTUserGroupBranchLink = 55
        'for branch specific risks
        SIROPTBranchSpecificRisks = 56
        SIROPTBusinessFieldOnClientIsMandatory = 57
        SIROPTSubBranchShowingForBroking = 58
        SIROPTAONPRClientScreenChanges = 59
        ' Instalment Display Style, Value = 1 = list style
        SIROPTInstalmentDisplayStyle = 60
        SIROPTEnableFSACompliance = 61
        SIROPTWMAutoRefreshEnabled = 62
        SIROPTAlternativeLogon = 63
        SIROPTAllowElectronicPayment = 64
        ' Multi-step approval for Payments
        SIROPTMultiStepApproval = 65
        SIROOPTNoDeletedTempates = 66
        'Allow one or more branches to act as underwriters/insurers
        SIROPTUnderwritingBranchEnabled = 67
        SIROPTUnderwritingYear = 68
        SIROPTHoldCoverExpiryDate = 69
        SIROPTAllowSpoolAndArchive = 70
        SIROPTAllowPartialAllocationOnInsurer = 71
        SIROPTEnhancedChequeProduction = 72
        SIROPTMtaPolicyNumberLock = 73
        SIROPTShowRiskIndexSearchOnFindInsuranceFile = 74
        SIROPTPolicyPackagesEnabled = 75
        SIROPTrueMonthlyPoliciesEnabled = 76
        SIROPTM9PartyCodeFormat = 77
        SIROPTS4IMotorDataModelEnabled = 78
        SIROPTS4IHouseholdDataModelEnabled = 79
        SIROPTS4ICommercialCombinedDataModelEnabled = 80
        SIROPTS4IMarineDataModelEnabled = 81
        SIROPTS4IMedicalMalpracticeDataModelEnabled = 82
        'AON Insurer Payment Import
        SIROPTAONInsurerPaymentImport = 83
        SIROPTAllowDataModelSAMAccess = 84
        'S4BDAT003 DATASURE
        SIROPTEnhancedAccountingBasis = 85
        'Set this to the last entry and you won't have to remember to change the array size
        'S4BDAT002 DATASURE
        SIROPTNewZealandConfiguration = 86
        'Float Balance and Pre-Payment
        SIROPTEnablePayNowOptions = 87
        SIROPTEnableRI2007 = 88
        SIROPTMultiCoRestrictClientView = 89
        SIROPTMultiCoWorkManagerTaskRestriction = 90
        SIROPTSiriusQuoteBranchLevelPolicyNumbers = 91
        SIROPTEnableTaxPostingsRollup = 92
        SIROPTSchemeMtaPolicyNumberLock = 93
        SIROPTFindInsuranceFileRiskIndexSearch = 94
        SIROPTHcPbChanges = 95
        'Nexus Change - Add the SSP Sub-agent as a sub agent on every policy processed via SAM.
        SIROPTAddSSPSubAgentViaSAM = 96
        SIROPTAutoAllocateduringClaimPaymentWorkflow = 98
        'Add Documaster folder per insurance-ref
        SIROPTAddDocumasterFolderPerInsuranceRef = 99

        SIROPTEnableUniqueDocumentReference = 100
        SIROPTDisablePDFenhancement = 101
        SIROPTSubAgentCertificateYears = 102
        SIROTPRestrictAllocationReversal = 103
        SIROPTMultipleRetainedTreaty = 104

        SIROPTEnableRIRegeneration = 105

        SIROPTEnable6DPGISPercentage = 106


        SIROPTEnableDebitOrder = 107

        SIROPTOverridePortfolioTransferValidations = 108
        SIROPTEnableRIPaymentsRecoveries = 109
        SIRROPTEnableExternalWorkflowSystem = 110
        SIROPTEnableClaimRIEditing = 111
        SIROPTEnableDecimalsSuppression = 112
        SIROPTEnableGeospatialFunctionality = 113
        SIRROPTPostInstalmentWriteOffAsFullDocument = 114
        SIROPTEnableCloudHosting = 115
        SIROPTCopyRiskInMTA = 116

        'Set this to the last entry and you won't have to remember to change the array size
        SIROPTMasterOptionsLastEntry = SIRHiddenOptions.SIROPTCopyRiskInMTA
    End Enum

    Public Enum PMDebitAgainst
        PMDebitAgainstCashListItem
        PMDebitAgainstOverDraft
        PMDebitAgainstFloatBalance
        PMDebitAgainstUnallocatedCredit
        PMDebitAgainstCashDeposit 'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
    End Enum


    '   Head Office value
    Public Const SIRBCHHeadOffice As Integer = 1
    ' ******************************************************************
    ' Moved from gSirLibraries End
    ' ******************************************************************

    ' InsuranceFileRiskLinkType Constants
    Public Const kInsFileRiskLinkTypeORIGINAL As Integer = 1
    Public Const kInsFileRiskLinkTypeRENEWED As Integer = 2

    '****************************************
    ' ME : 29-11-2002 : 202 Added constants for field manager sproc params
    Public Const ACParamName As Integer = 0
    Public Const ACParamValue As Integer = 1
    Public Const ACParamType As Integer = 2
    Public Const ACParamNamePurchaseOrderId As String = "purchase_order_id"
    ' End ME : 29-11-2002 : 202
    '****************************************

    Public Const HKEY_CLASSES_ROOT As Integer = &H80000000
    Public Const HKEY_CURRENT_USER As Integer = &H80000001
    Public Const HKEY_LOCAL_MACHINE As PMERegSettingRoot = &H80000002
    Public Const HKEY_USERS As Integer = &H80000003
    Public Const HKEY_PERFORMANCE_DATA As Integer = &H80000004
    Public Const HKEY_CURRENT_CONFIG As Integer = &H80000005
    Public Const HKEY_DYN_DATA As Integer = &H80000006

    Public Const ACRegRoot As String = "SOFTWARE\Pure"
    Public Const ACRegRoot64 As String = "SOFTWARE\WOW6432Node\Pure" 'WOW6432Node\
    Public Const ACRegSiriusArchitecture As String = "\Architecture"
    Public Const ACRegSiriusUnderwriting As String = "\SiriusUnderwriting"
    Public Const ACRegSiriusBroking As String = "\SiriusBroking"
    Public Const ACRegOrion As String = "\Orion"
    Public Const ACRegGemini As String = "\Gemini"
    Public Const ACRegVoyager As String = "\Voyager"
    Public Const ACRegMercury As String = "\Mercury"
    Public Const ACRegDocumaster As String = "\Documaster"
    Public Const ACRegSiriusSolutions As String = "\PureInstallation"
    Public Const ACRegNirvana As String = "\Nirvana"
    Public Const ACRegGeminiII As String = "\GeminiII"
    Public Const ACRegClaims As String = "\Claims"
    Public Const ACRegMediquote As String = "\Mediquote"
    Public Const ACRegStargate As String = "\Stargate"
    Public Const ACRegSwift As String = "\Swift"
    Public Const ACRegClient As String = "\Client"
    Public Const ACRegServer As String = "\Server"
    Public Const ACRegCommon As String = "\Common"
    Public Const ACRegSetup As String = "\Setup"

    'Sirius setup registry settings
    Public Const ACRegSiriusSetupVersion As String = "PureReleaseVersion"
    Public Const ACRegSiriusSetupRelease As String = "Service release"
    Public Const ACRegSiriusSetupSiriusType As String = "Sirius type"
    Public Const ACRegSiriusSetupInstallDate As String = "Date installed"

    'SA setup registry settings
    Public Const ACRegArchitectureSetupVersion As String = "Version"
    Public Const ACRegArchitectureSetupRelease As String = "Version build"
    Public Const ACRegArchitectureSetupInstallDate As String = "Install date"

    'Work Manager workflow constants
    Public Const PMWrkWorkflowFirstStepCount As Integer = 1

    'Event logging constants
    'Any changes in these below two constants must also be apply simultaneously in APP.CONFIG file
    Public Const EVENT_LOG_APP_NAME As String = "PUREInsurance"
    Public Const EVENT_LOG_FILE_NAME As String = "PUREApplicationLog"
    'Public Const DEFAULT_EVENT_ID As Integer = &H60000001
    Public Const DEFAULT_EVENT_ID As Integer = 1
    Public Const ERR_REG_EVENT_SOURCE As Integer = 9001
    Public Const NERR_BASE As Integer = 2100
    Public Const MAX_NERR As Integer = NERR_BASE + 899
    Public Const FORMAT_MESSAGE_FROM_SYSTEM As Integer = &H1000
    Public Const FORMAT_MESSAGE_IGNORE_INSERTS As Integer = &H200
    Public Const FORMAT_MESSAGE_FROM_HMODULE As Integer = &H800
    Public Const INTERNET_ERROR_BASE As Integer = 12000
    Public Const INTERNET_ERROR_LAST As Integer = INTERNET_ERROR_BASE + 171
    Public Const LOAD_LIBRARY_AS_DATAFILE As Integer = 2
    Public Const HEAP_ZERO_MEMORY As Integer = &H8S
    Public Const HEAP_GENERATE_EXCEPTIONS As Integer = &H4S
    Public Const TOKEN_QUERY As Integer = &H8
    Public Const REG_SOURCENAME As String = "WriteEvents.WriteEventsLog."


    Public Const REG_NONE As Integer = 0
    Public Const REG_SZ As Integer = 1
    Public Const REG_EXPAND_SZ As Integer = 2
    Public Const REG_BINARY As Integer = 3
    Public Const REG_DWORD As Integer = 4
    Public Const REG_LINK As Integer = 6
    Public Const REG_MULTI_SZ As Integer = 7
    Public Const REG_RESOURCE_LIST As Integer = 8

    Public Const ERROR_NONE As Integer = 0
    Public Const ERROR_BADDB As Integer = 1
    Public Const ERROR_BADKEY As Integer = 2
    Public Const ERROR_CANTOPEN As Integer = 3
    Public Const ERROR_CANTREAD As Integer = 4
    Public Const ERROR_CANTWRITE As Integer = 5
    Public Const ERROR_OUTOFMEMORY As Integer = 6
    Public Const ERROR_INVALID_PARAMETER As Integer = 7
    Public Const ERROR_ACCESS_DENIED As Integer = 8
    Public Const ERROR_INVALID_PARAMETERS As Integer = 87
    Public Const ERROR_NO_MORE_ITEMS As Integer = 259

    Public Const EL_KEY_ALL_ACCESS As Integer = &H3FS

    Public Const REG_KEY_QUERY_VALUE As Integer = &H1S
    Public Const REG_KEY_ENUMERATE_SUB_KEYS As Integer = &H8S
    Public Const REG_KEY_NOTIFY As Integer = &O10S
    Public Const REG_KEY_READ As Integer = REG_KEY_QUERY_VALUE Or REG_KEY_ENUMERATE_SUB_KEYS Or REG_KEY_NOTIFY

    Public Const REG_OPTION_NON_VOLATILE As Integer = 0

    ' Type Of Business (Navigator)
    Public Const PMTypeOfBusinessGeneric As String = ""
    Public Const PMTypeOfBusinessNB As String = "NB"
    Public Const PMTypeOfBusinessRN As String = "RN"
    Public Const PMTypeOfBusinessENN As String = "ENN"
    Public Const PMTypeOfBusinessENR As String = "ENR"

    Public Const PM_ALLOW As Integer = 0
    Public Const PM_DENY As Integer = 1
    Public Const PM_PROMPT As Integer = 2

    Public Const PMKeyNameMsgBoxCaption As String = "message_box_caption"
    Public Const PMKeyNameMsgBoxText As String = "message_box_text"

    Public Const PMKeyNameDecisionTitle As String = "decision_title"
    Public Const PMKeyNameDecisionText As String = "decision_text"

    Public Const PMKeyNameDebitCredit As String = "debit_credit"

    ' Renewal modes
    Public Const ACRenewalModeIAG As Integer = 1
    Public Const ACRenewalModeTransfer As Integer = 2
    Public Const ACRenewalModeAmend As Integer = 3
    Public Const ACRenewalModeAccept As Integer = 4

    'Renewal Frequency
    Public Const ACRenewalFrequencyMonthly As Integer = 1

    'renewal status type
    Public Const PMBRenewalStatusTypeManualReview As Integer = 1
    Public Const PMBRenewalStatusTypeAutoRated As Integer = 2
    Public Const PMBRenewalStatusTypeAutoRatedFailed As Integer = 3
    Public Const PMBRenewalStatusTypePolicyChanged As Integer = 4
    Public Const PMBRenewalStatusTypeAwaitUpdate As Integer = 5
    Public Const PMBRenewalStatusTypeAwaitManualRating As Integer = 6
    Public Const PMBRenewalStatusTypeAwaitBrokerTransfer As Integer = 7
    Public Const PMBRenewalStatusTypeWrittenAwaitUpdate = 8

    'RFC180299 New Constants Added for SA1.4
    Private Const ACSiriusArchitecture As String = "Sirius"
    Private Const ACSiriusUnderwriting As String = "SirUnd"
    Private Const ACOrion As String = "Orion"
    Private Const ACGemini As String = "Gemini"
    Private Const ACVoyager As String = "Voyager"
    Private Const ACMercury As String = "Mercury"
    Private Const ACDocumaster As String = "Documaster"
    Private Const ACSiriusBroking As String = "SirBroking"
    Private Const ACSiriusSolutions As String = "SirSol"
    Private Const ACNirvana As String = "Nirvana"
    Private Const ACGeminiII As String = "GeminiII"
    Private Const ACClaims As String = "Claims"
    Private Const ACDocumasterScan As String = "DocumasterScan"
    Private Const ACMediquote As String = "Mediquote"
    Private Const ACStargate As String = "Stargate"

    Private Const ACSwift As String = "Swift"

    ' RDC 19072002 - for Windows Terminal Services functions in gPMFunctions
    Private Const WTS_CURRENT_SERVER_HANDLE As Integer = 0
    Private Const WTS_CURRENT_SESSION_HANDLE As Integer = -1

    ' transaction sub type codes - policy fees
    Public Const kTransSubTypeNB As Integer = 0
    Public Const kTransSubTypeAdditionMTA As Integer = 1
    Public Const kTransSubTypeReturnMTA As Integer = 2
    Public Const kTransSubTypeCancellation As Integer = 3
    Public Const kTransSubTypeRenewal As Integer = 4
    Public Const kTransSubTypeReInstatement As Integer = 5
    Public Const kTransSubTypeAll = 6

    ' work claim link type constants
    Public Const kWorkClaimLinkTypeEvent As Integer = 1
    Public Const kWorkClaimLinkTypeTask As Integer = 2

    'Constants for temporary vs. permanents MTAs
    Public Const kMTATypeTemporary As Integer = 0
    Public Const kMTATypePermanent As Integer = 1
    Public Const kMTATypePermanentAndTemporary As Integer = 2
    Public Const kSystemOptionAutoInstalment As Integer = 1024
    '-(Arul Stephen)-( WPR2 - Reinsurance Obligatory)-(PN66155)
    Public Const kRINetLineObligatory As Integer = 2
    ' ***************************************************************** '
    ' Return Codes
    '
    ' The return codes below are grouped as follows :
    '
    ' General   - Commonly used return codes.
    ' System    - Return Codes used by system functions.
    ' Interface - Interface Layer return codes.
    ' Business  - Business Layer return codes.
    ' Database  - Database Layer return codes.
    '
    ' Seporate each group by a 200 offset.
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' * Return Codes                                                  * '
    ' ***************************************************************** '
    Public Enum PMEReturnCode
        ' General
        '********
        PMFalse = 0
        PMTrue = 1
        PMFail = 10
        PMError = 11
        PMSucceed = 12
        PMOK = 20
        PMCancel = 21
        PMNavigate = 30
        ' Broking Link Set 1
        ' RFC070498
        '*************
        PMMNoAuthority = 51
        PMMAlreadyInUse = 52
        PMMInvalidPassword = 53
        PMMNoAccess = 54
        ' System
        '*******
        PMIncorrectUsername = 200
        PMIncorrectPassword = 201
        PMLoggedOnElsewhere = 202
        PMLogError1 = 210
        PMLogError2 = 211
        PMMixedModeIncorrectUserName = 212
        PMMixedModeUserLoggedOnElsewhere = 213
        PMUnifiedModeIncorrectUserName = 214
        PMUnifiedModeUserLoggedOnElsewhere = 215
        PMUserAccountLocked = 216
        PMUserPasswordExpired = 217
        PMUserTemporaryPassword = 218
        PMUserWeakPassword = 219
        PMUserForceChangePassword = 220
        PMReusedPassword = 221
        PMNewBuildUpgrade = 222
        ' Interface
        '**********
        PMMoveStatusBack = 400
        PMMoveStatusNext = 401
        PMMoveStatusCancel = 402
        PMMoveStatusFinish = 403
        ' Broking Link Set 2
        ' RFC070498
        '*************
        PMError_argcount = 500
        PMError_protocol = 501
        PMError_notconnected = 502
        PMError_timeout = 503
        PMError_usage = 504
        ' Business
        '*********
        PMLogonExceeded = 600
        PMLicenceExceeded = 601
        PMInvalidLicenceKey = 602
        PMDataChanged = 610
        PMMandatoryMissing = 611
        PMDataNotChanged = 612
        PMInvalidRequest = 620
        PMIncorrectDateFormat = 621
        PMIncorrectSystemDate = 622
        PMEarlier = 623
        PMLater = 624
        PMInstallStarted = 625
        PMBlockLicenceExceeded = 626
        PMWarnLicenceExceeded = 627
        PMInvalidRiskStatus = 628
        ' Navigator Return Codes
        '***********************
        PMNavStartNewProcess = 700
        PMNavCallComponent = 701
        PMNavBuildMap = 702
        PMNavRepeatMap = 703
        PMNavEndMap = 704
        PMNavNavigate = 705
        PMNavEndProcess = 706
        ' Database
        '*********
        PMRecordChanged = 800
        PMRecordDeleted = 801
        PMRecordInUse = 810
        PMNotFound = 811
        PMBOF = 820
        PMEOF = 821
        PMQueryTimeout = 822
        PMNonRaisedError = 823
        PMDeadlock = 888
        ' Broking Link Set 3
        ' RFC070498
        '*************
        PMNoHostRegistry = 1002
        PMNoPortRegistry = 1003
        PMNoConnection = 1004
        PMNoPMLink = 1005
        PMNoCompanies = 1006
        ' CTAF 20030722 start
        ' Agents/Customers Online
        ' ***********************
        PMNoEmailAddress = 1100
        PMFailedEmail = 1101
        PMUpdateUserFailed = 1102
        PMUserNotExist = 1103
        PMUserNotLinkedAgent = 1104 ' When a user isn't linked to an agent
        PMGISOutDated = 1105 ' When the GIS doesn't support a method that the STS requires
        ' Documaster Errors
        PMDocumasterError = 1200
        ' RVH 07/06/2004
        ' XML Serialize/Deserialize errors
        '*********************************
        PMXMLTooManyDimensions = 2101
        PMXMLNotEnoughRows = 2102
        PMXMLNotEnoughColumns = 2103
        PMXMLParseError = 2104
        PMBackOfficeError = 3000
        PMBusinessRuleError = 3001
        ' DD 04/08/2005
        ' Additional optional actions in Roadmap
        '***************************************
        PMNavAction1 = 2200
        PMNavAction2 = 2201
        MandatoryInputMissing = 5001
        PMNBQuoteReferred = 9999901
        PMNBQuoteDeclined = 9999902

        ' Additional optional actions in PRE
        '***************************************
        PMPREFailed = 6000
    End Enum

    ' ***************************************************************** '
    ' * Constants for Message Logging
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' * Log Levels
    ' ***************************************************************** '
    Public Enum PMELogLevel
        PMLogFatal = 1
        PMLogError = 2
        PMLogWarning = 3
        PMLogOnError = 4
        PMLogInfo = 5
        PMLogDebug1 = 6
        PMLogDebug2 = 7
        PMLogDebug3 = 8
        PMLogDebug4 = 9
        PMLogFeedback = 10
    End Enum

    ' ***************************************************************** '
    ' Column positions for PMLock table
    ' ***************************************************************** '
    Public Enum PMEPMLockColumnPosition
        PMLockFormAllName = 0
        PMLockFormAllValue = 1
        PMLockFormAllUser = 2
        PMLockFormAllTime = 3
        PMLockFormAllUserID = 4
        PMLockFormAllValue2 = 5
    End Enum

    ' ***************************************************************** '
    ' Resource file data types.
    ' ***************************************************************** '
    Public Enum PMEResourseFileDataType
        PMResString = 0
        PMResBitmap = 1
        PMResIcon = 2
        PMResCursor = 3
    End Enum

    ' ***************************************************************** '
    ' Period constants.
    ' ***************************************************************** '
    Public Enum PMETimePeriod
        PMDay = 0
        PMWeek = 1
        PMMonth = 2
        PMYear = 3
    End Enum

    ' ***************************************************************** '
    ' AutoNumbering Number Types
    ' ***************************************************************** '
    Public Enum PMEAutoNumberType
        PMAutoNumInsFile = 1
        PMAutoNumInsFolder = 2
        PMAutoNumRiskFolder = 3
        PMAutoNumParty = 4
        PMAutoNumContact = 5
        PMAutoNumAddress = 6
    End Enum

    ' ***************************************************************** '
    ' Caption Array Column Position Constants
    ' ***************************************************************** '
    Public Enum PMENavCaptionArrayColPosition
        PMNavCaptionStepKey = 0
        PMNavCaptionCaption = 1
        PMNavCaptionIsSubMap = 2
        PMNavCaptionComponentType = 3
    End Enum

    ' ***************************************************************** '
    ' Key Let/Get Column Position Constants
    ' ***************************************************************** '
    Public Enum PMENavLetGetKeyColPosition
        PMKeyName = 0
        PMKeyValue = 1
    End Enum

    ' ***************************************************************** '
    ' Summary Detail Type Constants
    ' ***************************************************************** '
    Public Enum PMENavSummaryLevel
        PMNavSummProcessSummary = 0
        PMNavSummMapInstances = 1
        PMNavSummMapSummary = 2
    End Enum

    ' ***************************************************************** '
    ' Summary Detail Array Column Position Constants
    ' ***************************************************************** '
    Public Enum PMENavSummaryArrayColPosition
        PMNavSummLevel = 0
        PMNavSummHeading = 1
        PMNavSummValue = 2
    End Enum

    ' ***************************************************************** '
    ' Status type values.
    ' ***************************************************************** '
    Public Enum PMEComponentAction
        PMView = 0
        PMAdd = 1
        PMEdit = 2
        PMDelete = 3
        PMDummyDelete = 4
        PMAdded = 10
        PMReverse = 11
        PMReplace = 12
        PMCopy = 20
        PMDeleteFromDB = 21
    End Enum

    ' ***************************************************************** '
    ' Status type values.
    '
    ' Process Mode values  (Navigator)
    '
    ' Only use the following values for these constants :
    ' 0,1,2,4,8,16,32,64,128,256,512,1024,2048,4096,8192,16384
    ' ***************************************************************** '
    Public Enum PMEProcessMode
        PMProcessModeGeneric = 0
        PMProcessModeEnquiry = 1
        PMProcessModeNBQuote = 2
        PMProcessModeNBLive = 4
        PMProcessModeRNQuote = 8
        PMProcessModeRNLive = 16
        PMProcessModeMTAQuote = 32
        PMProcessModeMTALive = 64
        PMProcessModeMTRQuote = 128
        PMProcessModeMTRLive = 256
        'DAK280999
        PMProcessModeFull = 101
        PMProcessModePostQuote = 102
        PMProcessModeSpecific = 103
        PMProcessModeStartAtQuote = 104
        PMProcessModeDefault = 105
        PMProcessModeReview = 106
        PMProcessModeCancellations = 107
        PMProcessModeClaims = 108
        PMProcessModeOverride = 109
    End Enum

    ' ***************************************************************** '
    ' Navigator button status.
    ' ***************************************************************** '
    Public Enum PMENavigateButtonStatus
        PMNavigateNotRequired = 0
        PMNavigateEnabled = 1
        PMNavigateDisabled = 2
    End Enum

    ' ***************************************************************** '
    ' Mouse pointer states.
    ' ***************************************************************** '
    Public Enum PMEMousePointerStatus
        PMMouseBusy = 0
        PMMouseNormal = 1
        PMMouseReset = 2
    End Enum

    ' ***************************************************************** '
    ' Formatting values.
    ' ***************************************************************** '
    Public Enum PMEFormatStyle
        PMFormatString = 0
        PMFormatStringCase = 1
        PMFormatDateShort = 2
        PMFormatDateMedium = 3
        PMFormatDateLong = 4
        PMFormatTimeShort = 5
        PMFormatTimeMedium = 6
        PMFormatTimeLong = 7
        PMFormatDateTimeShort = 8
        PMFormatDateTimeMedium = 9
        PMFormatDateTimeLong = 10
        PMFormatCurrency = 11
        PMFormatInteger = 12
        PMFormatBoolean = 13
        PMFormatStringUpper = 14
        PMFormatDateYearOnly = 15
        PMFormatPercent = 16
        PMFormatDouble = 17
        PMFormatLong = 18
        ' This format uses local currency symbol and drops decimal amounts
        PMFormatWholeMoney = 19
        ' This format uses local currency symbol
        PMFormatMoney = 20
        PMFormatDecimal = 21 'JY120298
        ' Month & Day Long, Medium and Short added
        PMFormatMonthOnlyLong = 22
        PMFormatMonthOnlyMedium = 23
        PMFormatMonthOnlyShort = 24
        PMFormatDayOnlyLong = 25
        PMFormatDayOnlyMedium = 26
        PMFormatDayOnlyShort = 27
        PMFormatStringMultiLine = 28
        ' Lookup list refresh value.
        PMListRefreshValue = 30
        PMFormatPercentFourDecimal = 31
    End Enum

    ' ***************************************************************** '
    ' Business Object Action Values.
    ' ***************************************************************** '
    Public Enum PMEBusinessObjectAction
        PMActionView = 0
        PMActionEditAdd = 1
        PMActionEditUpdate = 2
        PMActionEditDelete = 3
        PMActionGetDefault = 4
        PMActionGetMandatory = 5
    End Enum

    ' ***************************************************************** '
    ' Business Object Mandatory constants
    ' ***************************************************************** '
    Public Enum PMEMandatoryStatus
        PMNonMandatory = 0
        PMMandatory = 1
        PMNonVisible = 2
    End Enum

    ' ***************************************************************** '
    ' PMLookup constants
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' Column positions for input array.
    ' ***************************************************************** '
    Public Enum PMELookupInArrayColPos
        PMLookupTableName = 0
        PMLookupKey = 1
        PMLookupStartPos = 2
        PMLookupNumOfItems = 3
        PMLookupWhereClause = 4
    End Enum
    ' ***************************************************************** '
    ' Privilege levels for amending the lookup tables
    ' ***************************************************************** '
    Public Enum PMELookupEditPrivlegeLevel
        PMLookupNoEdit = 0
        PMLookupViewOnly = 1
        PMLookupAmendCaptions = 2
        PMLookupFullPrivileges = 3
        PMLookupAdminViewUserNone = 4
        PMLookupAdminCaptionsUserNone = 5
        PMLookupAdminCaptionsUserView = 6
        PMLookupAdminFullUserNone = 7
        PMLookupAdminFullUserView = 8
        PMLookupAdminFullUserCaptions = 9
    End Enum

    ' ***************************************************************** '
    ' Column positions for output array.
    ' ***************************************************************** '
    Public Enum PMELookupOutArrayColPos
        PMLookupID = 0
        PMLookupCaption = 1
        PMLookupCode = 2
        ' RAW 15/07/2003 : CQ258 : added
        PMLookupIsDeleted = 3
    End Enum

    ' ***************************************************************** '
    ' Type of Lookup Required
    ' ***************************************************************** '
    Public Enum PMELookupType
        PMLookupAll = 0
        PMLookupSingle = 1
        PMLookupAllEffective = 2
        PMLookupAllWithDeleted = 3
    End Enum

    ' ***************************************************************** '
    ' Variable Data Array Column Position Constants
    '
    ' Note: For the Full List of Variable Data Record/Fields name
    '       etc see VarDataConst.bas
    ' ***************************************************************** '
    Public Enum PMEVarDataArrayColPos
        PMVarRecordID = 0
        PMVarRecordName = 1
        PMVarFieldName = 2
        PMVarFieldType = 3
        PMVarDefaultFormat = 4
        PMVarLength = 5
        PMVarValidationID = 6
        PMVarLocatorType = 7
        PMVarCoreTable = 8
        PMVarCoreVariable = 9
        PMVarMandatoryLevel = 10
        PMVarValue = 11
    End Enum

    ' ***************************************************************** '
    ' Variable Data Valid Value Column Position Constants
    ' ***************************************************************** '
    Public Enum PMEVarValidValueArrayColPos
        PMVarValValidationID = 0
        PMVarValValidValueID = 1
        PMVarValIsDefault = 2
        PMVarValCode = 3
        PMVarValNumericValue = 4
        PMVarValCaption = 5
    End Enum

    ' ***************************************************************** '
    ' Variable Data Core Variable Array Constants
    ' ***************************************************************** '
    Public Enum PMEVarCoreVariableArrayColPos
        PMVarCoreVariableName = 0
        PMVarCoreVariableValue = 1
    End Enum

    ' ***************************************************************** '
    ' Variable Data Locator Array constants
    ' ***************************************************************** '
    Public Enum PMEVarLocatorArrayColPos
        PMVarLocatorTypeID = 0
        PMVarLocatorTypeValue = 1
    End Enum

    ' ***************************************************************** '
    ' True/False in the Variable Data World
    ' ***************************************************************** '
    Public Enum PMEVarTrueFalse
        PMVarFalse = 0
        PMVarTrue = 1
    End Enum

    ' ***************************************************************** '
    ' Lock Mode Constants
    ' ***************************************************************** '
    Public Enum PMELockMode
        PMNoLock = 0
    End Enum

    Public Enum PMEStringCompareType
        PMBinaryCompare = 0
        PMStringCompare = 1
    End Enum

    ' ***************************************************************** '
    ' PMDAO Parameter Direction
    ' ***************************************************************** '
    Public Enum PMEParameterDirection
        PMParamInput = 0
        PMParamInputOutput = 1
        PMParamOutput = 2
        PMParamReturnValue = 3
        PMParamDefault = 4
    End Enum

    ' ***************************************************************** '
    ' Data Types
    ' ***************************************************************** '
    Public Enum PMEDataType
        PMString = 0
        PMInteger = 1
        PMLong = 2
        PMDouble = 3
        PMDate = 4
        PMBoolean = 5
        PMCurrency = 6
        PMBinary = 7
        ' These two are used by PMLookup
        PMTableName = 8
        PMFieldName = 9
        ' These two are used by variable data
        PMUniqueKey = 10
        PMCode = 11
        ' RFC 18/09/1997 Added for VB Decimal Support
        PMDecimal = 12
        ' BB 06/10/1997 Added for PMFormControl as a Field Type
        PMLookup = 13
    End Enum

    ' ***************************************************************** '
    ' BB030298
    ' Display Mode used to vary the style of form displayed
    ' ***************************************************************** '
    Public Enum PMEDisplayMode
        PMDisplayStandard = 0
        PMDisplaySimple = 1
    End Enum

    ' ***************************************************************** '
    ' BB040298
    ' Display Mode used to vary the style of form displayed
    ' ***************************************************************** '
    Public Enum PMEControlType
        PMUnknownCtlType = 0
        PMTextBox = 1
        PMCombo = 2
        PMCheckBox = 3
        PMListBox = 4
        PMGrid = 5
        PMSpread = 6
        PMOptionButton = 7
        PMAccountLookup = 8
    End Enum

    'Work Manager Constants
    ' Type of Task
    Public Enum PMEWrkManTaskType
        pmeWMTTMemo = 0
        pmeWMTTSingleComponent = 1
        pmeWMTTNavigatorProcess = 2
    End Enum

    ' Task Status
    Public Enum PMEWrkManTaskStatus
        pmeWMTSNew = 0
        pmeWMTSInProgress = 1
        pmeWMTSIncomplete = 2
        pmeWMTSComplete = 3
        pmeWMTSDeleted = 4
    End Enum


    ' ***************************************************************** '
    ' Shared Between iPMWrkManager& bPMWrkManager

    ' Column Positions for Available Task Array
    Public Enum PMEACAvailTaskCol
        ACAvailTaskGroupIDCol = 0
        ACAvailTaskIDCol = 1
        ACAvailTaskCaptionCol = 2
        ACAvailTaskIsSupervisorCol = 3
        ACAvailTaskIsSystemTaskCol = 4
        ACAvailTaskTypeOfTaskCol = 5
        ACAvailTaskPMNavProcessIDCol = 6
        ACAvailTaskObjectNameCol = 7
        ACAvailTaskClassNameCol = 8
        ACAvailTaskDeleteAfterDaysCol = 9
        ACAvailTaskDisplayIconCol = 10
        ACAvailTaskIsViewOnlyTaskCol = 11
        ACAvailTaskLinkedObjectNameCol = 12
        ACAvailTaskLinkedClassNameCol = 13
        ACAvailTaskLinkedCaptionCol = 14
        ' RDC 02122002
        ACAvailTaskNavXMLFileCol = 15
    End Enum

    Public Enum PMEACSchedTaskCol
        ACSchedTaskInstanceCntCol = 0
        ACSchedTaskUrgentCol = 1
        ACSchedTaskStatusCol = 2
        ACSchedTaskTypeCol = 3
        ACSchedTaskIsSystemCol = 4
        ACSchedTaskDueDateCol = 5
        ACSchedTaskCustomerCol = 6
        ACSchedTaskDescriptionCol = 7
        ACSchedTaskUserGroupIDCol = 8
        ACSchedTaskUserIDCol = 9
        ACSchedTaskNavProcessIDCol = 10
        ACSchedTaskObjectNameCol = 11
        ACSchedTaskClassNameCol = 12
        ACSchedTaskDisplayIconCol = 13
        ACSchedTaskIsViewOnlyTaskCol = 14
        ACSchedTaskLinkedObjectNameCol = 15
        ACSchedTaskLinkedClassNameCol = 16
        ACSchedTaskLinkedCaptionCol = 17
        ACSchedTaskIsVisibleCol = 18
        ACSchedTaskNavXMLfile = 19
        ACSchedTaskClientCode = 20
        ACSchedTaskReadOnly = 21
        ACSchedTaskPartyCnt = 22
        ACSchedTaskPartyName = 23
    End Enum
    Public Enum PMEACBatchTaskCol
        ACBatchTaskProcessCol = 0
        ACBatchTaskDescriptionCol = 1
        ACBatchTaskStartDateTimeCol = 2
        ACBatchTaskEndDateTimeCol = 3
        ACBatchTaskTotalRecordsCountCol = 4
        ACBatchTaskFileNameCol = 5
        ACBatchTaskPassedRecordsCountCol = 6
        ACBatchTaskFailedRecordsCountCol = 7
        ACBatchTaskStatusCol = 8
        ACBatchTaskBatchIdCol = 9
        ACBatchTaskStatusDescCol = 10
    End Enum

    Public Enum PMEACQuickStartCol
        ACQSTaskGroupIDCol = 0
        ACQSTaskIDCol = 1
    End Enum

    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Shared Between iPMWrkTaskInstLog & bPMWrkTaskInstLog

    ' Log Entries Array Select Column Positions
    ' Note: If these vales change then the SQL Statement must also change..
    ' spe_PMWrk_Task_Inst_Log_sad
    Public Enum PMELogEntriesArrayColPos
        pmeLEACPTaskInstCnt = 0
        pmeLEACPDateCreated = 1
        pmeLEACPText = 2
        pmeLEACPCreatedById = 3
    End Enum
    ' Enumerator for PMAutoNumber component, InsuranceFile reference
    Public Enum PMEAutoNumInsFileRefType
        pmeRefTypeNBQuotation = 1
        pmeRefTypeNBMakeLive = 2
        pmeRefTypeRenewalNotice = 3
        pmeRefTypeRenewalUpdate = 4
    End Enum

    ' Enumerator for number of decimal places allowed for currency fields
    Public Enum PMECurrencyNoOfDP
        pmeCurDPZero = 0
        pmeCurDPOne = 1
        pmeCurDPTwo = 2
        pmeCurDPThree = 3
        pmeCurDPFour = 4
    End Enum

    ' Enumerator for number of decimal places allowed for vdecimal fields
    Public Enum PMEVDecimalNoOfDP
        pmeVDecimalDPZero = 0
        pmeVDecimalDPOne = 1
        pmeVDecimalDPTwo = 2
        pmeVDecimalDPThree = 3
        pmeVDecimalDPFour = 4
        pmeVDecimalDPFive = 5
        pmeVDecimalDPSix = 6
    End Enum

    ' Enumerator for the roundup factor
    Public Enum PMERoundupFactor
        pmeRFactor00Up = 0
        pmeRFactor49Up = 51
        pmeRFactor50Up = 50
        pmeRFactor51Up = 49
        pmeRFactor55Up = 45
        pmeRFactor99Up = 1
    End Enum

    ' ***************************************************************** '
    ' RFC040398
    ' Database Type
    ' ***************************************************************** '
    Public Enum PMEDatabaseType
        pmeDBTypeUnknown = -1
        pmeDBTypeMSAccess = 0
        pmeDBTypeMSSQLServer = 1
        pmeDBTypeSybaseSQLAnywhere = 2
    End Enum

    ' ***************************************************************** '
    ' RFC130398
    ' Policy Master Product Families
    ' ***************************************************************** '
    ' Note: If you are adding a New Product Family
    '       then you need to amend the following :
    '
    ' gPMLibraries.Constants.PMProductCode
    ' gPMLibraries.Constants.PMProductFamilyByCode
    ' gPMLibraries.PMFunctions.BuildKeyString
    ' gPMLibraries.PMConstants - Add associated DSN/Database.
    ' sPMServerCS.PMServerBusinessCS.GetDSN
    ' dPMDAO.Database.CheckDSN
    Public Enum PMEProductFamily
        pmePFSiriusArchitecture = 1
        pmePFSiriusUnderwriting = 2
        pmePFOrion = 3
        pmePFGemini = 4
        pmePFVoyager = 5
        pmePFMercury = 6
        pmePFDocumaster = 7
        pmePFSiriusBroking = 8
        pmePFSiriusSolutions = 9
        pmePFNirvana = 10
        pmePFGeminiII = 11
        pmePFClaims = 12
        pmePFDocumasterScan = 13
        pmePFMediquote = 14
        pmePFStargate = 15
        pmePFSwift = 16
    End Enum

    ' ***************************************************************** '
    ' RFC200498
    ' MAPI Recipient Types = MSMAPI.RecipTypeConstants
    ' ***************************************************************** '
    Public Enum PMEMapiRecipientTypes
        pmeMapiOrigList = 0
        pmeMapiToList = 1
        pmeMapiCcList = 2
        pmeMapiBccList = 3
    End Enum

    ' ***************************************************************** '
    ' RFC200498
    ' MAPI Attachment Types = MSMAPI. AttachTypeConstants
    ' ***************************************************************** '
    Public Enum PMEMapiAttachmentTypes
        pmeMapiData = 0
        pmeMapiEOLE = 1
        pmeMapiSOLE = 2
    End Enum

    ' ***************************************************************** '
    ' RFC050698
    ' Policy Master Reg Setting Root
    ' ***************************************************************** '
    Public Enum PMERegSettingRoot
        pmeRSRLocalMachine = 1
        pmeRSRCurrentUser = 2
    End Enum

    ' ***************************************************************** '
    ' RFC050698
    ' Policy Master Reg Setting Level
    ' ***************************************************************** '
    Public Enum PMERegSettingLevel
        pmeRSLClient = 1
        pmeRSLServer = 2
        pmeRSLCommon = 3
        pmeRSLSetup = 4
        pmeRSLBase = 5
    End Enum

    ' ***************************************************************** '
    ' RFC201198
    ' Authority Level
    ' ***************************************************************** '
    Public Enum PMEAuthorityLevel
        pmeALUser = 0
        pmeALSupervisor = 1
        pmeALSysAdmin = 2
    End Enum
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' These constants are used by iPMTaskGroupMaintenance and PMWorkManager
    ' to display the correct Icon for a Task Group.
    Public Enum PMEACTaskGroupIcon
        ACTaskGroupIconIndexClient = 1
        ACTaskGroupIconIndexPolicy = 2
        ACTaskGroupIconIndexQuote = 3
        ACTaskGroupIconIndexClaim = 4
        ACTaskGroupIconIndexAccount = 5
        ACTaskGroupIconIndexReport = 6
        ACTaskGroupIconIndexAgent = 7
        ACTaskGroupIconIndexAdmin = 8
        ACTaskGroupIconIndexRenewals = 9
        ACTaskGroupIconIndexStatistics = 10
        ACTaskGroupIconIndexGeneral = 11
    End Enum

    ' RDC 22052003
    ' ***************************************************************** '
    ' For FileSystemObject file modes
    ' ***************************************************************** '
    Public Enum PMEFSOFileMode
        PMFSOFileModeRead = 1
        PMFSOFileModeWrite = 2
        PMFSOFileModeAppend = 8
    End Enum

    ' ***************************************************************** '
    ' Bank Type
    ' FSA Phase 3.1
    ' ***************************************************************** '
    Public Enum PMBankType
        PMBankTypeStatutory = 1
        PMBankTypeNonStatutory = 2
        PMBankTypeRiskTransfer = 3
    End Enum

    ' ***************************************************************** '
    ' WR5 - Claim Workflow
    ' ***************************************************************** '
    Public Enum EClaimWorkflowId
        EClaim_Process_Type_Id = 0
        ECheck_Unpaid_Status = 1
        EReinsurance_Recovery = 2
        ESalvage_Recovery = 3
        EThird_Party_Recovery = 4
        EExternal_Claim_Handling = 5
        EDescription_for_Change_in_Reserve = 6
        EClaim_Notification_Doc_Message = 7
        EGenerate_Claim_Notification_Doc = 8
        EClaim_Payment_Process = 9
        ECheck_Deferred_Reinsurance = 10
        EFast_Track_Claims = 11
        EReinsurance_Payment = 12
        EDescription_for_Change_in_Payment = 13
        ECash_Payment_process = 14
        EClaim_Payment_Doc_Message = 15
        EGenerate_Claim_Payment_doc = 16
        EMake_Further_Payments = 17
    End Enum

    Public Const PMWorkflowOpenClaim As Integer = 1
    Public Const PMWorkflowMaintainClaim As Integer = 2
    Public Const PMWorkflowPayClaim As Integer = 3

    ' ***************************************************************** '
    ' * Log Level Descriptions
    ' ***************************************************************** '
    Public Const PMFatalText As String = "Fatal"
    Public Const PMErrorText As String = "Error"
    Public Const PMWarningText As String = "Warning"
    Public Const PMInfoText As String = "Information"
    Public Const PMOnErrorText As String = "Recoverable Error"
    Public Const PMDebug1Text As String = "Debug 1"
    Public Const PMDebug2Text As String = "Debug 2"
    Public Const PMDebug3Text As String = "Debug 3"
    Public Const PMDebug4Text As String = "Debug 4"
    Public Const PMFeedbackText As String = "Application Feedback"

    ' ***************************************************************** '
    ' * Log Level Descriptions
    ' ***************************************************************** '
    Public Const PMDefaultLogFile As String = "C:\Sirius.Log"

    ' ***************************************************************** '
    ' * System/Product Details
    ' ***************************************************************** '
    Public Const PMProduct As String = "SIRIUS"
    Public Const PMCustomer As String = "AIG"

    ' ***************************************************************** '
    ' ClientManager/LicenceManager Timeout settings
    ' ***************************************************************** '
    Public Const PMPollEverySeconds As Integer = 30
    Public Const PMTimeOutSeconds As Integer = 90

    ' ***************************************************************** '
    ' Constants for the logon attempts.
    ' ***************************************************************** '
    Public Const PMLogonAttempts As Integer = 3

    ' ***************************************************************** '
    ' Resource file language offset value.
    ' ***************************************************************** '
    Public Const PMLangOffSetValue As Integer = 1000

    ' ***************************************************************** '
    ' Registry constants
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' Application
    ' ***************************************************************** '
    Public Const PMRegAppName As String = "Pure"

    ' Work Manager Constants
    Public Const ACWrkManRegSubKey As String = "WorkManager"
    Public Const ACWrkManRegWebAddress As String = "PMNewsWebAddress"

    'DAK190600
    Public Const ACWrkManRegWebTabCaption As String = "WebTabCaption"

    'DAK110700
    Public Const ACWrkManRegFormCaption As String = "FormCaption"
    Public Const ACWrkManRegSupportWebAddress As String = "PMSupportWebAddress"
    Public Const ACWrkManRegViewSplash As String = "ViewSplashScreen"
    Public Const ACWrkManRegViewQuickStart As String = "ViewQuickStart"
    Public Const ACWrkManRegViewAvailableTasks As String = "ViewAvailableTasks"

    'DAK241299
    Public Const ACWrkManRegViewToolbar As String = "ViewToolbar"
    Public Const ACWrkManRegViewStatusBar As String = "ViewStatusBar"
    Public Const ACWrkManRegViewGridLines As String = "ViewGridLines"
    Public Const ACWrkManRegViewGraphics As String = "ViewGraphics"

    'DAK110100
    Public Const ACWrkManRegIsAutoRefresh As String = "IsAutoRefresh"
    Public Const ACWrkManRegRefreshRate As String = "RefreshRate"

    ' ***************************************************************** '
    ' Sections
    ' ***************************************************************** '
    Public Const PMRegSecSystem As String = "System"
    Public Const PMRegSecLicence As String = "LicenceManager"
    ' ***************************************************************** '
    ' Installation Folder
    ' ***************************************************************** '
    Public Const PMRegInstallationFolder As String = "Installation folder"
    ' ***************************************************************** '
    ' Keys
    ' ***************************************************************** '
    Public Const PMRegKeyPoolSize As String = "PoolSize"
    Public Const PMRegKeyLogFile As String = "LogFileName"
    ' RDC 30082002
    Public Const PMRegKeyUseEventLog As String = "EventLogMessaging"
    Public Const PMRegKeyLogLevel As String = "UserLogLevel"
    ' RFC200498
    Public Const PMRegKeyArchitectureInDebug As String = "ArchitectureInDebug"
    ' RFC210498
    Public Const PMRegKeyArchitectureLocalEnabled As String = "ArchitectureLocalEnabled"
    ' RFC170698
    Public Const PMRegKeyArchitectureServerEnabled As String = "ArchitectureServerEnabled"
    ' RDC 11072002
    Public Const PMRegKeyArchitectureUnifiedLogon As String = "ArchitectureUnifiedLogon"
    ' RFC19/08/1998
    Public Const PMRegKeyQueryTimeoutSeconds As String = "QueryTimeoutSeconds"

    Public Const PMRegKeyCachePath As String = "CachePath"
    Public Const PMRegKeySystemOptionCacheFileName As String = "SysOptionsCacheFile"

    'RFC140199
    Public Const PMRegKeySplashBitMap As String = "SplashBitMap"

    ' RFC180299 New Constants Added for SA1.4
    Public Const PMRegKeyVersion As String = "Version"

    'DAK130100
    Public Const ACRegKeyViewGraphics As String = "ViewGraphics"

    'DAK250100
    'Navigator registry settings
    Public Const ACNavRegSubKey As String = "Navigator"

    Public Const ACRegKeyAllowErrorRetry As String = "AllowErrorRetry"

    ' RDC 09042002
    ' ***************************************************************** '
    ' COM+ registry settings
    ' ***************************************************************** '
    ' COM+ client manager string name. Location: PM\SiriusArchitecture\Server\ClientManagerCOMPlus
    Public Const ACRegKeyClientManagerCOMPlus As String = "ClientManagerCOMPlus"
    Public Const kRegKeyLicenseKeyPath As String = "LicenceKeyPath"

    ' ***************************************************************** '
    ' Reference Fields
    ' ***************************************************************** '
    Public Const PMRefFieldCoverStartDate As String = "CSD"
    Public Const PMRefFieldCoverExpiryDate As String = "CED"
    Public Const PMRefFieldBranchCode As String = "BC"
    Public Const PMRefFieldProductCode As String = "PC"
    Public Const PMRefFieldProductAnalysisCode As String = "PAC"
    Public Const PMRefFieldTransactionTypeCode As String = "TTC"
    Public Const PMRefFieldTransactionBasis As String = "TB"
    Public Const PMRefFieldSourceCode As String = "SC"


    ' ***************************************************************** '
    ' Navigator
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Constants used for the collection keys
    ' ***************************************************************** '
    Public Const PMProcessKeyPrefix As String = "P"
    Public Const PMMapKeyPrefix As String = "M"
    Public Const PMStepKeyPrefix As String = "S"
    Public Const PMComponentKeyPrefix As String = "C"
    Public Const PMProcInstanceKeyPrefix As String = "PI"
    Public Const PMMapInstanceKeyPrefix As String = "MI"
    Public Const PMStepInstanceKeyPrefix As String = "SI"

    ' ***************************************************************** '
    ' Status settings for Process, Map and Step.
    ' ***************************************************************** '
    Public Const PMNavStatusUnknown As String = ""
    Public Const PMNavStatusNotActive As String = "NA"
    Public Const PMNavStatusComplete As String = "CP"
    Public Const PMNavStatusIncomplete As String = "IP"

    ' ***************************************************************** '
    ' Incomplete Effects
    ' ***************************************************************** '
    Public Const PMNavIncompleteNone As String = "NA"
    Public Const PMNavIncompleteCurrentProcess As String = "CP"
    Public Const PMNavIncompleteCurrentMap As String = "CM"

    ' ***************************************************************** '
    ' Action Constants
    ' ***************************************************************** '
    Public Const PMNavActionBackOne As String = "B1"
    Public Const PMNavActionBackX As String = "BX"
    Public Const PMNavActionExitMap As String = "EM"
    Public Const PMNavActionForwardOne As String = "F1"
    Public Const PMNavActionForwardX As String = "FX"
    Public Const PMNavActionRepeatMap As String = "RM"
    Public Const PMNavActionStartProcess As String = "SP"
    'RFC140199
    Public Const PMNavActionCompleteProcess As String = "CP"
    'RFC140199
    Public Const PMNavActionAbortProcess As String = "AP"

    ' ***************************************************************** '
    ' Component Type Constants
    ' ***************************************************************** '
    Public Const PMNavComponentDataForm As String = "DF"
    Public Const PMNavComponentBusinessObject As String = "BO"
    Public Const PMNavComponentFindForm As String = "FF"
    Public Const PMNavComponentDecisionForm As String = "QF"

    'RDC 12062003 For NavXM user-defined secondary steps
    Public Const PMNavComponentDiary As String = "XMD"
    Public Const PMNavComponentEditText As String = "XMET"
    Public Const PMNavComponentRaiseEvent As String = "XMRE"
    Public Const PMNavComponentStandardLetter As String = "XMSL"
    Public Const PMNavComponentLaunchEXE As String = "XMLE"
    Public Const PMNavComponentUserComponent As String = "XMUC"

    ' ***************************************************************** '
    ' Transaction_Type_Basis constants
    ' ***************************************************************** '
    Public Const PMTransTypeBasisAdditional As String = "A"
    Public Const PMTransTypeBasisRefund As String = "F"
    Public Const PMTransTypeBasisPrimary As String = "P"
    Public Const PMTransTypeBasisReversePrimary As String = "R"

    ' ***************************************************************** '
    ' Table names (PM Wide Lookup Tables)
    ' ***************************************************************** '
    Public Const PMLookupLanguage As String = "language"
    Public Const PMLookupCurrency As String = "currency"
    Public Const PMLookupCountry As String = "country"


    ' ***************************************************************** '
    ' Constants used by PMDAO (Data Access Object)
    ' ***************************************************************** '
    ' Database Name Constants
    ' PM Data Source / DB Name
    Public Const PMSiriusDSN As String = "Sirius"
    Public Const PMSiriusDatabase As String = "Sirius"

    ' Orion Data Source /DB Name
    Public Const PMOrionDSN As String = "Orion"
    Public Const PMOrionDatabase As String = "Orion"

    ' Gemini Data Source /DB Name
    Public Const PMGeminiDSN As String = "Gemini"
    Public Const PMGeminiDatabase As String = "Gemini"

    ' BB201097 - Constants for Voyager DB
    Public Const PMVoyagerDSN As String = "Voyager"
    Public Const PMVoyagerDatabase As String = "Voyager"

    ' RFC 04/03/1998 - Constants for Mercury DB
    Public Const PMMercuryDSN As String = "Mercury"
    Public Const PMMercuryDatabase As String = "Mercury"

    ' RFC 04/03/1998 - Constants for Documaster DB
    Public Const PMDocumasterDSN As String = "Documaster"
    Public Const PMDocumasterDatabase As String = "Documaster"

    ' RFC 04/03/1998 - Constants for DocumasterV2 DB
    Public Const PMDocumasterV2DSN As String = "DocumasterV2"
    Public Const PMDocumasterV2Database As String = "DocumasterV2"

    ' RFC 04/03/1998 - Constants for DocumasterScan DB
    Public Const PMDocumasterScanDSN As String = "DocumasterScan"
    Public Const PMDocumasterScanDatabase As String = "DocumasterScan"

    ' RFC 25/03/1998 - Constants for Sirius Architecture DB
    Public Const PMSiriusArchitectureDSN As String = "SiriusArchitecture"
    Public Const PMSiriusArchitectureDatabase As String = "SiriusArchitecture"

    ' RFC 05/06/1998 - Sirius Broking DSN
    Public Const PMSiriusBrokingDSN As String = "SiriusBroking"
    Public Const PMSiriusBrokingDatabase As String = "SiriusBroking"

    ' RFC 19/08/1998 - Sirius Underwriting DSN
    Public Const PMSiriusUnderwritingDSN As String = "SiriusUnderwriting"
    Public Const PMSiriusUnderwritingDatabase As String = "SiriusUnderwriting"

    ' RFC 19/08/1998 - Sirius Solutions DSN
    Public Const PMSiriusSolutionsDSN As String = "Pure"
    Public Const PMSiriusSolutionsDatabase As String = "Pure"

    ' RFC 19/08/1998 - Nirvana DSN
    Public Const PMNirvanaDSN As String = "Nirvana"
    Public Const PMNirvanaDatabase As String = "Nirvana"

    'RFC060799 - Added GeminiII Product Family, DSN etc etc
    Public Const PMGeminiIIDSN As String = "GeminiII"
    Public Const PMGeminiIIDatabase As String = "GeminiII"

    ' RDC 07082000 - New product family: Claims
    Public Const PMClaimsDSN As String = "Claims"
    Public Const PMClaimsDatabase As String = "Claims"

    'JSB 09/09/03 - Mediquote product family
    Public Const PMMediquoteDSN As String = "Mediquote"
    Public Const PMMediquoteDatabase As String = "Mediquote"
    'added for SQL Password Security
    Public Const PMSQLLoginId As String = "SQLLogin"
    Public Const PMSQLLoginPassword As String = "SecureKey"
    Public Const PMEncryptionEntropy As String = "SiriusArchitecture"

    ' RAM20041229 - Added for Swift
    Public Const PMSwiftDSN As String = "Swift"
    Public Const PMSwiftDatabase As String = "Swift"
   
    ' ***************************************************************** '
    ' Constants required for string manipulation
    ' ***************************************************************** '
    Public Const PMStartDelimiter As String = "{"
    Public Const PMEndDelimiter As String = "}"

    ' ***************************************************************** '
    ' Database Parameter Prefix
    ' Currently set to @ for SQLServer
    ' ***************************************************************** '
    Public Const PMDBParamPrefix As String = "@"
    ' Length of Prefix (In Characters)
    Public Const PMDBParamPrefixLen As Integer = 1

    ' ***************************************************************** '
    ' Database Hex Prefix
    ' Currently Set to 0x for SQLServer
    ' ***************************************************************** '
    Public Const PMDBHexPrefix As String = "0x"

    ' ***************************************************************** '
    ' SP 23/09/1997 - Tell PMDAO not to restrict number of records returned in SQLSelect
    ' ***************************************************************** '
    Public Const PMAllRecords As Integer = -1
    'DAK071099
    ' Work Manager Constants
    Public Const ACTaskGroupIconDescClient As String = "Client"
    Public Const ACTaskGroupIconDescPolicy As String = "Policy"
    Public Const ACTaskGroupIconDescQuote As String = "Quotes"
    Public Const ACTaskGroupIconDescClaim As String = "Claims"
    Public Const ACTaskGroupIconDescAccount As String = "Accounts"
    Public Const ACTaskGroupIconDescReport As String = "Reports"
    Public Const ACTaskGroupIconDescAgent As String = "Agent"
    Public Const ACTaskGroupIconDescAdmin As String = "Administration"
    Public Const ACTaskGroupIconDescRenewals As String = "Renewals"
    Public Const ACTaskGroupIconDescStatistics As String = "Statistics"
    Public Const ACTaskGroupIconDescGeneral As String = "Other Tasks"
    Public Const ACTaskCategoryNonLicence As String = "NONLICENCE"

    'Make Live Options
    Public Const kMakeLiveOptionsINVOICE As String = "INVOICE"
    Public Const kMakeLiveOptionsINST As String = "INST"
    Public Const kMakeLiveOptionsBG As String = "BG"
    Public Const kMakeLiveOptionsPAYNOW As String = "PAYNOW"
    Public Const kMakeLiveOptionsCD As String = "CD"
    Public Const kMakeLiveOptionsMARKED As String = "MARKED"



    ' *******************************************************
    ' Transaction Types
    ' RFC020398
    ' *******************************************************
    ' Generic
    Public Const PMTransactionTypeGeneric As String = ""

    'DAK280999
    'New Business
    Public Const PMTransactionTypeNB As String = "G_NB"

    'MTA
    Public Const PMTransactionTypeMTA As String = "G_MTA"

    'Renewals
    Public Const PMTransactionTypeRenewals As String = "G_RENEW"

    ' *******************************************************
    ' Object Manager GetInstance options.
    ' RFC120698
    ' *******************************************************
    ' Get instance via Client Manager i.e. Server Side
    Public Const PMGetViaClientManager As String = "CLIENTMANAGER"
    ' Get a local business object
    Public Const PMGetLocalBusiness As String = "LOCALBUSINESS"
    ' Get a local interface object
    Public Const PMGetLocalInterface As String = "LOCALINTERFACE"

    'Doc Link
    Public Const PMDocLinkPolicy As Integer = 1
    'Public Const PMDocLinkClaims As Integer = 2
    Public Const PMDocLinkOpenMaintainClaims As Integer = 2
    Public Const PMDocLinkAccounts As Integer = 3
    Public Const PMDocLinkPayClaims As Integer = 4
    Public Const PMSysOptionRestricteduserbranchOption As Integer = 5152

    ' RDC 25072002 event logging types ###################################################
    Public Structure SID_AND_ATTRIBUTES
        Dim PSID As Integer
        Dim hAttributes As Integer
    End Structure

    Public Structure TOKEN_USER
        Dim uSid As SID_AND_ATTRIBUTES
        Public Shared Function CreateInstance() As TOKEN_USER
            Dim result As New TOKEN_USER
            Return result
        End Function
    End Structure

    ' RDC 25072002 for event logging
    Public Structure FmtMsgArrayType
        Dim s1 As String
        Dim s2 As String
        Dim s3 As String
        Dim s4 As String
        Dim s5 As String
        Dim s6 As String
        Dim s7 As String
        Dim s8 As String
        Dim s9 As String
        Dim s10 As String
        Dim s11 As String
        Dim s12 As String
        Dim s13 As String
        Dim s14 As String
        Dim s15 As String
        Dim s16 As String
        Dim s17 As String
        Dim s18 As String
        Dim s19 As String
        Dim s20 As String
        Dim s21 As String
        Dim s22 As String
        Dim s23 As String
        Dim s24 As String
        Dim s25 As String
        Dim s26 As String
        Dim s27 As String
        Dim s28 As String
        Dim s29 As String
        Dim s30 As String
        Dim s31 As String
        Dim s32 As String
        Dim s33 As String
        Dim s34 As String
        Dim s35 As String
        Dim s36 As String
        Dim s37 As String
        Dim s38 As String
        Dim s39 As String
        Dim s40 As String
        Dim s41 As String
        Dim s42 As String
        Dim s43 As String
        Dim s44 As String
        Dim s45 As String
        Dim s46 As String
        Dim s47 As String
        Dim s48 As String
        Dim s49 As String
        Dim s50 As String
        Dim s51 As String
        Dim s52 As String
        Dim s53 As String
        Dim s54 As String
        Dim s55 As String
        Dim s56 As String
        Dim s57 As String
        Dim s58 As String
        Dim s59 As String
        Dim s60 As String
        Dim s61 As String
        Dim s62 As String
        Dim s63 As String
        Dim s64 As String
        Dim s65 As String
        Dim s66 As String
        Dim s67 As String
        Dim s68 As String
        Dim s69 As String
        Dim s70 As String
        Dim s71 As String
        Dim s72 As String
        Dim s73 As String
        Dim s74 As String
        Dim s75 As String
        Dim s76 As String
        Dim s77 As String
        Dim s78 As String
        Dim s79 As String
        Dim s80 As String
        Dim s81 As String
        Dim s82 As String
        Dim s83 As String
        Dim s84 As String
        Dim s85 As String
        Dim s86 As String
        Dim s87 As String
        Dim s88 As String
        Dim s89 As String
        Dim s90 As String
        Dim s91 As String
        Dim s92 As String
        Dim s93 As String
        Dim s94 As String
        Dim s95 As String
        Dim s96 As String
        Dim s97 As String
        Dim s98 As String
        Dim s99 As String
        Public Shared Function CreateInstance() As FmtMsgArrayType
            Dim result As New FmtMsgArrayType
            result.s1 = String.Empty
            result.s2 = String.Empty
            result.s3 = String.Empty
            result.s4 = String.Empty
            result.s5 = String.Empty
            result.s6 = String.Empty
            result.s7 = String.Empty
            result.s8 = String.Empty
            result.s9 = String.Empty
            result.s10 = String.Empty
            result.s11 = String.Empty
            result.s12 = String.Empty
            result.s13 = String.Empty
            result.s14 = String.Empty
            result.s15 = String.Empty
            result.s16 = String.Empty
            result.s17 = String.Empty
            result.s18 = String.Empty
            result.s19 = String.Empty
            result.s20 = String.Empty
            result.s21 = String.Empty
            result.s22 = String.Empty
            result.s23 = String.Empty
            result.s24 = String.Empty
            result.s25 = String.Empty
            result.s26 = String.Empty
            result.s27 = String.Empty
            result.s28 = String.Empty
            result.s29 = String.Empty
            result.s30 = String.Empty
            result.s31 = String.Empty
            result.s32 = String.Empty
            result.s33 = String.Empty
            result.s34 = String.Empty
            result.s35 = String.Empty
            result.s36 = String.Empty
            result.s37 = String.Empty
            result.s38 = String.Empty
            result.s39 = String.Empty
            result.s40 = String.Empty
            result.s41 = String.Empty
            result.s42 = String.Empty
            result.s43 = String.Empty
            result.s44 = String.Empty
            result.s45 = String.Empty
            result.s46 = String.Empty
            result.s47 = String.Empty
            result.s48 = String.Empty
            result.s49 = String.Empty
            result.s50 = String.Empty
            result.s51 = String.Empty
            result.s52 = String.Empty
            result.s53 = String.Empty
            result.s54 = String.Empty
            result.s55 = String.Empty
            result.s56 = String.Empty
            result.s57 = String.Empty
            result.s58 = String.Empty
            result.s59 = String.Empty
            result.s60 = String.Empty
            result.s61 = String.Empty
            result.s62 = String.Empty
            result.s63 = String.Empty
            result.s64 = String.Empty
            result.s65 = String.Empty
            result.s66 = String.Empty
            result.s67 = String.Empty
            result.s68 = String.Empty
            result.s69 = String.Empty
            result.s70 = String.Empty
            result.s71 = String.Empty
            result.s72 = String.Empty
            result.s73 = String.Empty
            result.s74 = String.Empty
            result.s75 = String.Empty
            result.s76 = String.Empty
            result.s77 = String.Empty
            result.s78 = String.Empty
            result.s79 = String.Empty
            result.s80 = String.Empty
            result.s81 = String.Empty
            result.s82 = String.Empty
            result.s83 = String.Empty
            result.s84 = String.Empty
            result.s85 = String.Empty
            result.s86 = String.Empty
            result.s87 = String.Empty
            result.s88 = String.Empty
            result.s89 = String.Empty
            result.s90 = String.Empty
            result.s91 = String.Empty
            result.s92 = String.Empty
            result.s93 = String.Empty
            result.s94 = String.Empty
            result.s95 = String.Empty
            result.s96 = String.Empty
            result.s97 = String.Empty
            result.s98 = String.Empty
            result.s99 = String.Empty
            Return result
        End Function
    End Structure

    ' RDC 25072002 Used for info on SIDs
    Public Enum TOKEN_INFORMATION_CLASS
        TokenUser = 1
        TokenGroups = 2
        TokenPrivileges = 3
        TokenOwner = 4
        TokenPrimaryGroup = 5
        TokenDefaultDacl = 6
        TokenSource = 7
        TokenType = 8
        TokenImpersonationLevel = 9
        TokenStatistics = 10
        TokenRestrictedSids = 11
        TokenSessionId = 12
    End Enum

    ' RDC 25072002 for event log
    Public Enum enmLogType
        PMEventLogError = 1
        PMEventLogWarning = 2
        PMEventLogInfo = 4
    End Enum

    ' RDT 07/12/2006
    ' Values for the import and export batch jobs
    Public Enum PMEImportInterface
        pmeIEIPaymentImport = 0
        pmeIEIReferenceImport = 1
        pmeIEIBankReconciliationImport = 2
    End Enum

    Public Enum PMEExportInterface
        pmeIEIGLExport = 0
        pmeIEIInstalmentExport = 1
        pmeIEIClaimExport = 2
        pmeIEIReceiptExport = 3
        pmeIEIPaymentExport = 4
        pmeIEIInstalmentPlanExport = 5
        pmeIEIPolicyExport = 6
        pmeIEIMessageExport = 7
        pmeIEIDocumentExport = 8
        pmeIEIPolicyBatchExport = 9
        'WPR14-MID
        pmeIEIMIDExport = 10
        'END WPR14-MID
        pmeIEIMID2Export = 11
        pmeIEICommissionExport = 12
    End Enum
    ' RDT 07/12/2006 - End

    Public Enum PMEEventType
        pmeIEISMS = 0
    End Enum

    Public Const PMSystemLowDate As Date = #1/1/1900#
    Public Const PMSystemHighDate As Date = #12/31/9998#

    ' PN24093.
    Public Const PMFileOperationRetryAttempts As Byte = 6
    Public Const PMFileOperationRetryWait As Byte = 10

    ' RDC 30/09/2005 for HTML-to-PDF conversion library
    Public Const PDF_CREATOR_PILOT_EMAIL As String = "pamela.wingate@siriusfs.com"
    Public Const PDF_CREATOR_PILOT_PWORD As String = "PN97H-3WQB6-AVTF6-BC7FY-5KR2C"

    Public Const PDF_HTML2PDF_USER As String = "Pamela Wingate"
    Public Const PDF_HTML2PDF_PWORD As String = "3PAUJ-XZ3DM-JPWP7-4LVZH"

    '***************************************************************************
    'Event Type Code
    '***************************************************************************
    Public Const PMDocumentEventType As String = "DOCUMENT"
    Public Const PMEmailSentEventType As String = "EMAILSENT"
    Public Const PMExternalDocUploadEventType As String = "EXTDOC"

    'Renewal Document Destination - WR9 Batch Renewal
    Public Const PMRenewalDocDestination_Print As Integer = 1
    Public Const PMRenewalDocDestination_Spool As Integer = 2

    Public Const kClonedDocumentTypeID As Integer = 59
    Public Const kClaimPaymentDocumentTypeID As Integer = 28
    Public Const kClaimReceiptDocumentTypeID As Integer = 29
    Public Const kClonedReversedDocumentTypeId As Integer = 58

    ' *******************************************************
    ' Unix Link Timeout Settings
    ' RFC070498
    ' RFC131098
    ' *******************************************************
    Public ReadOnly Property PMUnixLinkSendTimeout() As Integer
        Get

            Dim sRegSendTimeout As String = ""
            ' RDC 12062002 classes changed to BAS modules
            Dim lReturn As Integer

            Try

                ' Get the Unix Link Send Timoeut setting from
                ' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Server\UnixLink
                lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer, v_sSettingName:="SendTimeout", r_sSettingValue:=sRegSendTimeout, v_sSubKey:="UnixLink")

                ' Use setting from Registry OR Default if it doesnt exist/invalid.
                Dim dbNumericTemp As Double

                If Double.TryParse(sRegSendTimeout, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    Return CInt(sRegSendTimeout)
                Else
                    Return 60
                End If

            Catch
            End Try



            Return 60

        End Get
    End Property

    Public ReadOnly Property PMUnixLinkReadTimeout() As Integer
        Get

            Dim sRegReadTimeout As String = ""
            ' RDC 12062002 classes changed to BAS modules
            Dim lReturn As Integer

            Try

                ' Get the Unix Link Read Timoeut setting from
                ' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Server\UnixLink
                lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ReadTimeout", r_sSettingValue:=sRegReadTimeout, v_sSubKey:="UnixLink")

                ' Use setting from Registry OR Default if it doesnt exist/invalid.
                Dim dbNumericTemp As Double

                If Double.TryParse(sRegReadTimeout, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    Return CInt(sRegReadTimeout)
                Else
                    Return 60
                End If

            Catch
            End Try



            Return 60

        End Get
    End Property

    Public ReadOnly Property PMUnixLinkConnectTimeout() As Integer
        Get

            Dim sRegConnectTimeout As String = ""
            ' RDC 12062002 classes changed to BAS modules
            Dim lReturn As Integer

            Try

                ' Get the Unix Link Connect Timoeut setting from
                ' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Server\UnixLink
                lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer, v_sSettingName:="ConnectTimeout", r_sSettingValue:=sRegConnectTimeout, v_sSubKey:="UnixLink")

                ' Use setting from Registry OR Default if it doesnt exist/invalid.
                Dim dbNumericTemp As Double

                If Double.TryParse(sRegConnectTimeout, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    Return CInt(sRegConnectTimeout)
                Else
                    Return 20
                End If

            Catch
            End Try



            Return 20

        End Get
    End Property

    'RFC 16/10/1998
    Public ReadOnly Property PMProductCode(ByVal v_lPMProductFamily As Integer) As String
        Get

            Try

                'RFC180299 Changed to use Constants

                Select Case v_lPMProductFamily
                    Case PMEProductFamily.pmePFSiriusArchitecture
                        Return ACSiriusArchitecture
                    Case PMEProductFamily.pmePFSiriusUnderwriting
                        Return ACSiriusUnderwriting
                    Case PMEProductFamily.pmePFOrion
                        Return ACOrion
                    Case PMEProductFamily.pmePFGemini
                        Return ACGemini
                    Case PMEProductFamily.pmePFVoyager
                        Return ACVoyager
                    Case PMEProductFamily.pmePFMercury
                        Return ACMercury
                    Case PMEProductFamily.pmePFDocumaster
                        Return ACDocumaster
                    Case PMEProductFamily.pmePFSiriusBroking
                        Return ACSiriusBroking
                    Case PMEProductFamily.pmePFSiriusSolutions
                        Return ACSiriusSolutions
                    Case PMEProductFamily.pmePFNirvana
                        Return ACNirvana
                        'RFC060799 - Added GeminiII Product Family, DSN etc etc
                    Case PMEProductFamily.pmePFGeminiII
                        Return ACGeminiII
                        ' RDC 13092002
                    Case PMEProductFamily.pmePFDocumasterScan
                        Return ACDocumasterScan
                        'JSB 09/09/03
                    Case PMEProductFamily.pmePFMediquote
                        Return ACMediquote
                        'DC041203
                    Case PMEProductFamily.pmePFStargate
                        Return ACStargate
                        ' RAM20041229 - Added for Swift
                    Case PMEProductFamily.pmePFSwift
                        Return ACSwift
                    Case Else
                        Return ""
                End Select

            Catch
            End Try



            Return ""

        End Get
    End Property

    'RFC180299 New Constants Added for SA1.4
    Public ReadOnly Property PMProductFamilyByCode(ByVal v_sPMProductCode As String) As Integer
        Get

            Try


                Select Case v_sPMProductCode
                    Case ACSiriusArchitecture
                        Return PMEProductFamily.pmePFSiriusArchitecture
                    Case ACSiriusUnderwriting
                        Return PMEProductFamily.pmePFSiriusUnderwriting
                    Case ACOrion
                        Return PMEProductFamily.pmePFOrion
                    Case ACGemini
                        Return PMEProductFamily.pmePFGemini
                    Case ACVoyager
                        Return PMEProductFamily.pmePFVoyager
                    Case ACMercury
                        Return PMEProductFamily.pmePFMercury
                    Case ACDocumaster
                        Return PMEProductFamily.pmePFDocumaster
                    Case ACSiriusBroking
                        Return PMEProductFamily.pmePFSiriusBroking
                    Case ACSiriusSolutions
                        Return PMEProductFamily.pmePFSiriusSolutions
                    Case ACNirvana
                        Return PMEProductFamily.pmePFNirvana
                        'RFC060799 - Added GeminiII Product Family, DSN etc etc
                    Case ACGeminiII
                        Return PMEProductFamily.pmePFGeminiII
                        ' RDC 07082000 - new product family: Claims
                    Case ACClaims
                        Return PMEProductFamily.pmePFClaims
                    Case ACDocumasterScan
                        Return PMEProductFamily.pmePFDocumasterScan
                        'JSB 09/09/03
                    Case ACMediquote
                        Return PMEProductFamily.pmePFMediquote
                        'DC041203
                    Case ACStargate
                        Return PMEProductFamily.pmePFStargate
                        ' RAM20041229 - Added for Swift
                    Case ACSwift
                        Return PMEProductFamily.pmePFSwift
                    Case Else
                        Return 0
                End Select

            Catch
            End Try



            Return 0

        End Get
    End Property

    Public Const ERROR_NO_LENGTH As Integer = 10
    Public Const ERROR_LABEL As String = "SSP - ERROR REF : "
    'Class that contains the list of valid suffix for fields
    Public Class LOG_FIELDS_LIST
        Public Const Field1 As String = "id"
        Public Const Field2 As String = "cnt"
        Public Const Field3 As String = "ref"
        Public Const Field4 As String = "code"
        Public Const Field5 As String = "date"
    End Class

End Module