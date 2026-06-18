Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule

    '================
    'Public Constants
    '================
    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "uctSIRInstalmentsControl"

    ' ***************************************************************** '
    ' TR071102 - Added for TS23 (start)
    ' Not Used by Stored Procedures. Only used to create the Quotes array
    ' manually, which is passed externally
    ' ***************************************************************** '
    Public Const k_PFQuoteCompanyNo As Integer = 0
    Public Const k_PFQuoteCompanyName As Integer = 1
    Public Const k_PFQuoteSchemeNo As Integer = 2
    Public Const k_PFQuoteSchemeVersion As Integer = 3
    Public Const k_PFQuoteSchemeName As Integer = 4
    Public Const k_PFQuoteFrequencyID As Integer = 5
    Public Const k_PFQuoteFrequencyDescription As Integer = 6
    Public Const k_PFQuoteMediaTypeID As Integer = 7
    Public Const k_PFQuoteMediaTypeDescription As Integer = 8
    Public Const k_PFQuoteProductClass As Integer = 9
    Public Const k_PFQuoteProductCode As Integer = 10
    Public Const k_PFQuoteTotalAmountInput As Integer = 11
    Public Const k_PFQuoteInstalmentsToPay As Integer = 12
    Public Const k_PFQuoteFirstInstalmentDate As Integer = 13
    Public Const k_PFQuoteNextInstalmentDate As Integer = 14
    Public Const k_PFQuoteLastInstalmentDate As Integer = 15
    Public Const k_PFQuoteFirstInstalmentAmount As Integer = 16
    Public Const k_PFQuoteOtherInstalmentAmount As Integer = 17
    Public Const k_PFQuoteTotalInstalmentsAmount As Integer = 18
    Public Const k_PFQuoteAprRate As Integer = 19
    Public Const k_PFQuoteInterestRate As Integer = 20
    Public Const k_PFQuoteDaysDelay As Integer = 21
    Public Const k_PFQuoteDepositAmount As Integer = 22
    Public Const k_PFQuoteInterestAmount As Integer = 23
    Public Const k_PFQuoteTaxAmount As Integer = 24
    Public Const k_PFQuoteFinanceCharge As Integer = 25
    Public Const k_PFQuoteProtectionAmount As Integer = 26
    Public Const k_PFQuoteOriginalOtherInstalmentAmount As Integer = 27
    Public Const k_PFQuoteHighlightCell As Integer = 28
    Public Const k_PFQuoteSchemeTypeCode As Integer = 29
    Public Const k_PFQuoteMediaTypeValidation As Integer = 30
    Public Const k_PFQuoteFrequencyPerYear As Integer = 31
    Public Const k_PFQuotePFRF_ID As Integer = 32
    Public Const k_PFQuoteFrequencyPeriod As Integer = 33
    Public Const k_PFQuoteFrequencyAmount As Integer = 34
    Public Const k_PFQuoteOriginalAmount As Integer = 35
    Public Const k_PFQuoteClaimDebtID As Integer = 36
    Public Const k_PFQuoteUserID As Integer = 37
    Public Const k_PFQuoteAgentCnt As Integer = 38
    Public Const k_PFQuoteAgentRef As Integer = 39
    Public Const k_PFQuoteLastInstalmentAmount As Integer = 40
    Public Const k_PFSGUsername As Integer = 41
    Public Const k_PFSGPassword As Integer = 42
    Public Const k_PFSGBrokerID As Integer = 43
    Public Const k_PFSGBrokerURL As Integer = 44
    Public Const k_PFSGTimeout As Integer = 45
    Public Const k_PFSGProviderCode As Integer = 46
    Public Const k_PFSGTerms As Integer = 47
    Public Const k_PFSGRef As Integer = 48
    Public Const k_PFOriginalRate As Integer = 49
    Public Const k_PFQuoteRefundType As Integer = 50
    Public Const k_PFQuoteMinMTA As Integer = 51
    Public Const k_PFQuoteTaxGroupID As Integer = 52
    Public Const k_PFQuoteXSLCode As Integer = 53
    Public Const k_PFSGSchemeType As Integer = 54
    Public Const k_PFDepositAsInstalment As Integer = 55
    Public Const k_PFAlignTo As Integer = 56
    Public Const k_PFBranchCodeMandatory As Integer = 57
    Public Const k_PFBranchNameMandatory As Integer = 58
    Public Const k_PFBankNameMandatory As Integer = 59
    Public Const k_PFBankAddressMandatory As Integer = 60
    Public Const k_PFStartLimit As Integer = 61
    Public Const k_PFStartDate As Integer = 62
    Public Const k_PFDelayULimit As Integer = 63
    Public Const k_PFSingleInstalmentPerMonth As Integer = 64
    Public Const k_PFFirstInstalmentAlignWithMonthInDay As Integer = 65
    Public Const k_PFUseTransCurrncy As Integer = 66



    'The constant below should be set to the value of the last constant above
    'as this is used for dimensioning within the class module clsPremFinance.
    Public Const k_PFQuoteUBound As Integer = k_PFUseTransCurrncy

    'TR - BackDatedMTATypes
    Public Const k_PFNotBDMTA As Integer = 0
    Public Const k_PFIntermediateBDMTA As Integer = 1
    Public Const k_PFLastBDMTA As Integer = 2

    '================
    'Public Variables
    '================
    'Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    'Public source and language ID's from the Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_sUsername As String = ""


    ' RAW 05/11/2003 : CQ2912, 2976 : removed constants duplicated in bSIRPremFinConst

    'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.1.3)
    Public Const kUSLangId As Integer = 2
    Public Const kUKLangId As Integer = 1
    'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.1.3)


End Module