Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module MainModule

    Public Const ACApp As String = "iPMUListRisks"


    Public Const ScreenHelpID As Integer = 4
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    ' Public source and language IDs from the
    <ThreadStatic()>
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iUserID As Integer

    ' Public instance of the object manager.
    <ThreadStatic()>
    Public g_oObjectManager As bObjectManager.ObjectManager

    ' PW311002 - Risk Array position constants.
    Public Const ACRiskPosCnt As Integer = 0

    Public Const ACTabTitle1 As Integer = 100
    Public Const ACCoverFromDate As Integer = 101
    Public Const ACExpiryDate As Integer = 102
    Public Const ACInstalments As Integer = 103

    Public Const kRecalculateModeRisks As Integer = 1
    Public Const kRecalculateModeFees As Integer = 2
    Public Const kRecalculateModeRITax As Integer = 3
    Public Const kRecalculateModeAgentCommission As Integer = 4
    Public Const kRecalculateModeInstalments As Integer = 5

    Public Const kTabRisk As Integer = 0
    Public Const kTabPoliyFees As Integer = 1
    Public Const kTabPolicyTaxes As Integer = 2
    Public Const kTabAgentCommission As Integer = 3
    Public Const kTabInstalments As Integer = 4

    Public Const kPaymentTermsInstalments As Integer = 1
    Public Const kPaymentTermsInvoice As Integer = 2
    Public Const kPaymentTermsPutMTAOnNextInstalmentRenewal As Integer = 3

    Public Const kRiskStatus As Integer = 0
    Public Const kRiskIsSelected As Integer = 1
    Public Const kRiskInsuanceFileCnt As Integer = 2
    Public Const kRiskCnt As Integer = 3
    Public Const kRiskMandatoryrisk As Integer = 6

    Public Const kValidationSuccessful As Integer = 1
    Public Const kValidationFailed As Integer = 2

    Public Const kInsFileDetailsProductIsTrueMonthlyPolicy As Integer = 15
    Public Const kInsFileDetailsPutOnNextInstalmentRenewal As Integer = 16
    Public Const kInsFileDetailsDiscountReasonId As Integer = 17
    Public Const kInsFileDetailsDiscountPercentage As Integer = 18
    Public Const kInsFileDetailsProductId As Integer = 19
    Public Const kInsFileDetailsDiscountedPremium As Integer = 20
    Public Const kInsFileDetailsMatchDiscountedPremium As Integer = 21
    Public Const kInsFileDetailsAnniversaryDate As Integer = 22
    Public Const kInsFileDetailsOnsFileTypeID As Integer = 35
    Public Const kInsFileDetailsWrittenStatusPermitted As Integer = 36
    Public Const kInsFileDetailsInsuranceFileStatusCode As Integer = 37
    Public Const kInsFileDetailsWrittenTaskManagerDays As Integer = 38
    Public Const kInsFileDetailsWrittenRemUserGroup As Integer = 39
    Public Const kInsFileDetailsWrittenRemTaskGroup As Integer = 40

    Public Const kInsFileDetailsInceptionTPI As Integer = 41

    Public Const kInsFileDetailsTransCurrencyID As Integer = 42
    Public Const kInsFileDetailsBaseCurrencyID As Integer = 43
    Public Const kInsFileDetailsTransISOCode As Integer = 44
    Public Const kInsFileDetailsBaeISOCode As Integer = 45

    Public Const kInsFileDetailsIsMarketPlacePolicy As Integer = 45

    Public Const kPolicyDiscountStatusNotApplicable As Integer = 0
    Public Const kPolicyDiscountStatusAllowRollback As Integer = 1
    Public Const kPolicyDiscountStatusAllowMakeLive As Integer = 1
    Public Const kPolicyDiscountStatusAllowDisableMakeLive As Integer = 2
    Public Const kPolicyDiscountStatusAllowApply As Integer = 2

    Public Const kPolicyDiscountRollbackReasonFeesChanged As Integer = 1
    Public Const kPolicyDiscountRollbackReasonTaxChanged As Integer = 2
    Public Const kPolicyDiscountRollbackReasonAddRisk As Integer = 3
    Public Const kPolicyDiscountRollbackReasonEditRisk As Integer = 4
    Public Const kPolicyDiscountRollbackReasonDeleteRisk As Integer = 5
    Public Const kPolicyDiscountRollbackReasonCopyRisk As Integer = 6
    Public Const kPolicyDiscountRollbackReasonSaveQuote As Integer = 7
    Public Const kPolicyDiscountRollbackReasonRequote As Integer = 8
    Public Const kPolicyDiscountRollbackReasonRiskSelectOrUnSelect As Integer = 9

    Public Const kPolicyDiscountActionApply As Integer = 1
    Public Const kPolicyDiscountActionRollback As Integer = 2

    Public Const kLookupTableNameDiscountReason As String = "discount_reason"
    Public Const kLookupTableNameDiscountRecurringType As String = "discount_recurring_type"

    Public m_vKeyArray As Object

    'Backdate MTA
    Public Const ACIReappliedIFileCnt As Integer = 0
    Public Const ACIPolicyType As Integer = 1
    Public Const ACICoverStartDate As Integer = 2
    Public Const ACICoverEndDate As Integer = 3
    Public Const ACIReappliedPremium As Integer = 4
    Public Const ACIReversedPremium As Integer = 5
    Public Const ACIStatus As Integer = 6
    Public Const ACIReversedIFileCnt As Integer = 7
    Public Const ACIQuoteStatus As Integer = 8
    Public Const ACIReversedComm As Integer = 9
    Public Const ACIReappliedComm As Integer = 10
    Public Const ACIReversedFee As Integer = 11
    Public Const ACIReappliedFee As Integer = 12

    Public Const kInsFileTransactionTypeNB As String = "NB"
    Public Const kInsFileTransactionTypeREN As String = "REN"
    Public Const kInsFileTransactionTypeMTA As String = "MTA"

    Public Const kInsFileTypeQuote As Integer = 1
    Public Const kInsFileTypeLive As Integer = 2
    Public Const kInsFileTypeUnderRen As Integer = 3
    Public Const kInsFileTypeMTAPermQuote As Integer = 4
    Public Const kInsFileTypeMTAPerm As Integer = 5
    Public Const kInsFileTypeMTATemp As Integer = 6
    Public Const kInsFileTypeMTATempQuote As Integer = 7

    Public Const kUSLangId As Integer = 2
    Public Const kUKLangId As Integer = 1

    Public Const kInsuranceFileTypeQUOTEID As Integer = 1
    Public Const kInsuranceFileTypeMTACANID As Integer = 8
    Public Const kInsuranceFileStatusCANID As Integer = 1
    Public Const kInsuranceFileStatusREPBDMTAID As Integer = 309
    Public Const kRiskID As Integer = 0
    Public Const kRiskFolderID As Integer = 1
    Public Const kRiskNumber As Integer = 2
    Public Const kInsuranceFileID As Integer = 3
    Public Const kInsuranceFolderID As Integer = 4
    Public Const kProductID As Integer = 5
    Public Const kScreenID As Integer = 6
    Public Const kRiskDescription As Integer = 7
    Public Const kRiskStatusID As Integer = 8
    Public Const kRiskStatusCode As Integer = 9
    Public Const kRiskStatusDescription As Integer = 10
    Public Const kRiskTypeID As Integer = 11
    Public Const kRiskTypeCode As Integer = 12
    Public Const kRiskTypeDescription As Integer = 13
    Public Const kIsAutoRated As Integer = 14
    Public Const kHasFACRI As Integer = 15
    Public Const kIsMandatoryRisk As Integer = 16
    'Adding constats to represent the policy make live status
    Public Enum EPolicyMakeLiveStatus
        PolicyQuoted = 0
        PolicyMadeLive = 1
    End Enum
    'Start - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enhancement)
    Public Const ACReRateRiskID = 0
    Public Const ACReRateRiskFolderID = 1
    Public Const ACReRateRiskNumber = 2
    Public Const ACReRateInsuranceFileID = 3
    Public Const ACReRateInsuranceFolderID = 4
    Public Const ACReRateProductID = 5
    Public Const ACReRateScreenID = 6
    Public Const ACReRateRiskDescription = 7
    Public Const ACReRateRiskStatusID = 8
    Public Const ACReRateRiskStatusCode = 9
    Public Const ACReRateRiskStatusDescription = 10
    Public Const ACReRateRiskTypeID = 11
    Public Const ACReRateRiskTypeCode = 12
    Public Const ACReRateRiskTypeDescription = 13
    Public Const ACReRateIsAutoRated = 14
    Public Const ACReRateHasFACRI = 15
    'End - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enhancement)

End Module
