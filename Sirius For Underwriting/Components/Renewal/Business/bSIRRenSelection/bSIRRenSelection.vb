Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  02/09/2000
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRRenSelection"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'RWH(16/11/2000) Renewal criteria descriptions for Renewal_report.
    Public Const PMAutoRenewalDesc As String = "Auto-renewal flag not set"
    Public Const PMPartyRenewalStopDesc As String = "Party renewal stop code"
    Public Const PMPolicyRenewalStopDesc As String = "Policy renewal stop code"
    Public Const PMReferredAtRenewalDesc As String = "Referred at renewal"
    Public Const PMClaimsMadeDesc As String = "Failed claims criteria"
    Public Const PMFailedIndexLinkDesc As String = "Failed index-linking"
    Public Const PMFailedReRateDesc As String = "Failed re-rating"
    Public Const PMAgentRenewalStopDesc As String = "Agent renewal stop code"
    Public Const PMClosedBranchDesc As String = "Policy is for closed branch"
    Public Const PMAgentTransferred As String = "Agent is subject to a transfer"

    'TN20010719
    Public Const PMIsQuotedDesc As String = "Not Quoted"

    'RWH(16/11/2000) Renewal List Position constants.
    Public Const PMFieldPosInsuranceFileCnt As Integer = 0
    Public Const PMFieldPosInsuranceHolderCnt As Integer = 1
    Public Const PMFieldPosProductID As Integer = 2
    Public Const PMFieldPosLeadAgentCnt As Integer = 3
    Public Const PMFieldPosInsuranceRef As Integer = 4
    Public Const PMFieldPosCoverStartDate As Integer = 5
    Public Const PMFieldPosCoverEndDate As Integer = 6
    Public Const PMFieldPosClientName As Integer = 7
    Public Const PMFieldPosAgentName As Integer = 8
    Public Const PMFieldPosIsAutoRenewable As Integer = 9
    Public Const PMFieldPosProductCode As Integer = 10
    Public Const PMFieldPosPolicyStopReason As Integer = 11
    Public Const PMFieldPosClientStopReason As Integer = 12
    Public Const PMFieldPosReferredAtRenewal As Integer = 13
    Public Const PMFieldPosInsuranceFolderCnt As Integer = 14
    Public Const PMFieldPosRenewalDate As Integer = 15
    Public Const PMFieldPosHolderName As Integer = 16
    Public Const PMFieldPosAgentStopReason As Integer = 17
    Public Const PMFieldPosClosedBranch As Integer = 18
    Public Const PMFieldPosAgentInTransfer As Integer = 19
    Public Const PMFieldPosIsTrueMonthlyPolicy As Integer = 20
    Public Const PMFieldPosAnniversaryCopy As Integer = 21
    Public Const PMFieldPosRenewalDayNumber As Integer = 22
    Public Const PMFieldPosAnniversaryDate As Integer = 23
    Public Const PMFieldPosAnniversaryRenewalWeeks As Integer = 24
    Public Const PMFieldPosPutOnNextInstalmentRenewal As Integer = 25
    Public Const PMFieldPosLatestInstalmentPlanInsuranceFileCnt As Integer = 26
    Public Const PMFieldPosLeadAllowConsolidatedCommission As Integer = 27
    Public Const PMFieldPosSubAllowConsolidatedCommission As Integer = 28
    Public Const PMFieldPosRenewalCount As Integer = 29
    '1.12 WR25
    Public Const PMFieldPosRenewalProductId As Integer = 30
    Public Const PMFieldPosOriginalProductId As Integer = 31
    Public Const PMFieldPosTMPAutoRenFAC As Integer = 32
    Public Const PMFieldAlternateReference As Integer = 33
    'RWH(23/11/2000) Risk Array position constants.
    Public Const ACRiskPosCnt As Integer = 0
    Public Const ACRiskPosStatusId As Integer = 1
    Public Const ACRiskPosFolder As Integer = 2
    Public Const ACRiskPosAccumId As Integer = 3
    Public Const ACRiskPosTypeId As Integer = 4
    Public Const ACRiskPosDescription As Integer = 5
    Public Const ACRiskPosSequenceNo As Integer = 6
    Public Const ACRiskPosSumInsReq As Integer = 7
    Public Const ACRiskPosInceptionDate As Integer = 8
    Public Const ACRiskPosExpiryDate As Integer = 9
    Public Const ACRiskPosIsNotIndexLinked As Integer = 10
    Public Const ACRiskPosIsAccum As Integer = 11
    Public Const ACRiskPosLapsedReasonId As Integer = 12
    Public Const ACRiskPosLapsedDate As Integer = 13
    Public Const ACRiskPosLapsedDesc As Integer = 14
    Public Const ACRiskPosVarDataRef As Integer = 15
    Public Const ACRiskPosTotalSumIns As Integer = 16
    Public Const ACRiskPosTotalAnnualPrem As Integer = 17
    Public Const ACRiskPosTotalThisPrem As Integer = 18
    Public Const ACRiskPosIsRiAtRiskLevel As Integer = 19
    Public Const ACRiskPosIsAutoReinsured As Integer = 20
    Public Const ACRiskPosGisScreenId As Integer = 21
    Public Const ACRiskPosIsMandatoryRisk As Integer = 36
    Public Const kRiskRenRule As Integer = 38
    Public Const kRiskUALRule As Integer = 39
    ' policy discount constants
    Public Const kDiscountRecurringTypeIdTransaction As Integer = 1
    Public Const kDiscountRecurringTypeIdTerm As Integer = 2
    Public Const kDiscountRecurringTypeIdPolicy As Integer = 3

    'UPGRADE_NOTE: (1053) g_sProductFamily was changed from a Constant to a Variable. More Information: http://www.vbtonet.com/ewis/ewi1053.aspx
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Public Const kSystemOptionBlackListClientInForce As Integer = 5011

    Public Const kSystemOptionNumberofThreadsforRiskProcessing As Integer = 5147
    Public Enum ACBatchJobStartColumns
        BatchId = 0
        InsuranceFolderCnt = 1
        BatchRenewalJobId = 2
        BatchRenewalJobRunStatusId = 3
        RecalculateCommission = 4
        RecalculateFees = 5
        RecalculateTaxes = 6
        OldInsuranceFileCnt = 7
        NewInsuranceFileCnt = 8
        BatchRef = 9
    End Enum

    Public Enum ACRiskProcessingParamatersColumns
        RiskFolderCnt = 0
        Rerate = 1
        RecalculateReinsurance = 2
        RecalculateFees = 3
        RecalculateTaxes = 4
    End Enum

    Public Enum BatchRunStatus
        ReadyForProcessing = 1
        ProcessingInProgress = 2
        CompletedSuccess = 3
        CompletedFailed = 4
    End Enum
    Sub Main_Renamed()


    End Sub
End Module