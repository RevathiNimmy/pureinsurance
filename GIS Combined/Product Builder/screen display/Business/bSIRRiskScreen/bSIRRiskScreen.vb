Option Strict Off
Option Explicit On
'developer guide no. 129
Module MainModule
    '******************************************************************************
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    '******************************************************************************
    '

    '******************************************************************************
    ' Module Name: MainModule
    '
    ' Date:  07/05/1999
    '
    ' Description: Main Module.
    '
    ' Edit History:
    '******************************************************************************


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRRiskScreen"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'Array constants for the Screen Header
    Public Const ACHScreenId As Integer = 0
    Public Const ACHCaptionId As Integer = 1
    Public Const ACHCode As Integer = 2
    Public Const ACHDescription As Integer = 3
    Public Const ACHIsDeleted As Integer = 4
    Public Const ACHEffectiveDate As Integer = 5
    Public Const ACHParentId As Integer = 6
    Public Const ACHIsMaintainable As Integer = 7

    'Array constants for the Risk
    Public Const ACRRiskId As Integer = 0
    Public Const ACRRiskStatusId As Integer = 1
    Public Const ACRRiskFolderCnt As Integer = 2
    Public Const ACRAccumulationId As Integer = 3
    Public Const ACRRiskTypeId As Integer = 4
    Public Const ACRDescription As Integer = 5
    Public Const ACRSequenceNumber As Integer = 6
    Public Const ACRSumInsuredRequested As Integer = 7
    Public Const ACRInceptionDate As Integer = 8
    Public Const ACRExpiryDate As Integer = 9
    Public Const ACRIsNotIndexLinked As Integer = 10
    Public Const ACRIsAccumulated As Integer = 11
    Public Const ACRLapsedReasonId As Integer = 12
    Public Const ACRLapsedDate As Integer = 13
    Public Const ACRLapsedDescription As Integer = 14
    Public Const ACRVarDataRef As Integer = 15
    Public Const ACRTotalSumInsured As Integer = 16
    Public Const ACRTotalAnnualPremium As Integer = 17
    Public Const ACRTotalThisPremium As Integer = 18
    Public Const ACRIsRiAtRiskLevel As Integer = 19
    Public Const ACRIsAutoReinsured As Integer = 20
    Public Const ACRGISScreenId As Integer = 21
    Public Const ACREMLPercentage As Integer = 22
    Public Const ACRRiskNumber As Integer = 23
    Public Const ACRVariationNumber As Integer = 24
    Public Const ACRIsRiskSelected As Integer = 25
    Public Const ACRCoverage As Integer = 26
    Public Const ACRInsuredItem As Integer = 27
    Public Const ACRExtensions As Integer = 28

    'AC 18/09/2003 CQ17  Two new fields added to update array size by 2
    Public Const ACRMax As Integer = 31



    'Array constants for the Risk Type
    Public Const ACRTRiskTypeId As Integer = 0
    Public Const ACRTRiskFolderTypeId As Integer = 1
    Public Const ACRTCaptionId As Integer = 2
    Public Const ACRTCode As Integer = 3
    Public Const ACRTDescription As Integer = 4
    Public Const ACRTEffectiveDate As Integer = 5
    Public Const ACRTIsDeleted As Integer = 6
    Public Const ACRTVarDataStructureId As Integer = 7
    Public Const ACRTInterfaceObjectName As Integer = 8
    Public Const ACRTInterfaceClassName As Integer = 9
    Public Const ACRTOverridePerilRiBand As Integer = 10
    Public Const ACRTOverridePerilXlBand As Integer = 11
    Public Const ACRTNBPremiumProRateTypeId As Integer = 12
    Public Const ACRTMTAPremiumProRateTypeId As Integer = 13
    Public Const ACRTRNPremiumProRateTypeId As Integer = 14
    Public Const ACRTIsShareWithCoinsurers As Integer = 15
    Public Const ACRTIsShareWithReinsurers As Integer = 16
    Public Const ACRTIsSuppressPublicText As Integer = 17
    Public Const ACRTIsSuppressPrivateText As Integer = 18
    Public Const ACRTIsSuppressTaxes As Integer = 19
    Public Const ACRTReportPointer As Integer = 20
    Public Const ACRTSectionMask As Integer = 21
    Public Const ACRTStampDutyRate1 As Integer = 22
    Public Const ACRTStampDutyRate2 As Integer = 23
    Public Const ACRTPrimarySort As Integer = 24
    Public Const ACRTSecondarySort As Integer = 25
    Public Const ACRTHeaderClause As Integer = 26
    Public Const ACRTTrailerClause As Integer = 27
    Public Const ACRTIsRiAtRiskLevel As Integer = 28
    Public Const ACRTIsAutoReinsured As Integer = 29
    Public Const ACRTHeaderClauseId As Integer = 30
    Public Const ACRTTrailerClauseId As Integer = 31
    Public Const ACRTAccumulationLevel As Integer = 32
    Public Const ACRTGISScreenId As Integer = 33

    'Array constants for the Product
    Public Const ACPProductId As Integer = 0
    Public Const ACPCaptionId As Integer = 1
    Public Const ACPCode As Integer = 2
    Public Const ACPDescription As Integer = 3
    Public Const ACPEffectiveDate As Integer = 4
    Public Const ACPIsDeleted As Integer = 5
    Public Const ACPSchemeAgencyRef As Integer = 6
    Public Const ACPBlockNo As Integer = 7
    Public Const ACPIsTaxSuppressed As Integer = 8
    Public Const ACPQuoteAutoNumberingId As Integer = 9
    Public Const ACPIsShortPeriodRated As Integer = 10
    Public Const ACPIsMidnightRenewal As Integer = 11
    Public Const ACPIsAutoRenewable As Integer = 12
    Public Const ACPRenewalWeeks As Integer = 13
    Public Const ACPPolicyAutoNumberingId As Integer = 14
    Public Const ACPProvClaimAutoNumberingId As Integer = 15
    Public Const ACPFullClaimAutoNumberingId As Integer = 16
    Public Const ACPIsAccumulation As Integer = 17
    Public Const ACPRIPointer As Integer = 18
    Public Const ACPReportPointer As Integer = 19

    'Array constants for the Risk Folder
    Public Const ACRFRiskFolderCnt As Integer = 0
    Public Const ACRFRiskFolderId As Integer = 1
    Public Const ACRFSourceId As Integer = 2
    Public Const ACRFRiskFolderTypeId As Integer = 3
    Public Const ACRFCode As Integer = 4
    Public Const ACRFDescription As Integer = 5
    Public Const ACRFInsuranceFolderCnt As Integer = 6

    'Array constants for the Insurance File Risk Link
    Public Const ACIFRLInsuranceFileCnt As Integer = 0
    Public Const ACIFRLRiskCnt As Integer = 1
    Public Const ACIFRLStatusFlag As Integer = 2
    Public Const ACIFRLOriginalRiskCnt As Integer = 3
    Public Const ACIFRLIsManuallyChanged As Integer = 4




    Sub Main_Renamed()


    End Sub
End Module