Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129
Module mainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '



    Public Const ACApp As String = "bSIRAutomaticRenewalsSel"


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
    Public Const kIsTMP As Integer = 20


    Public Const PMIsQuotedDesc As String = "Not Quoted"
    Public Const PMFailedReRateDesc As String = "Failed re-rating"

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


    Public Const ACRiskPosCnt As Integer = 0


    Public Const ACIInsFileCnt As Integer = 0
    Public Const ACIRiskId As Integer = 1
    Public Const ACIRiskDescription As Integer = 2
    Public Const ACIRiskTypeDescription As Integer = 3
    Public Const ACIRiskInceptionDate As Integer = 4
    Public Const ACIRiskExpiryDate As Integer = 5
    ' AM 061200 Add new column for risk status
    Public Const ACIRiskStatus As Integer = 6
    Public Const ACIRiskTotalSumInsured As Integer = 7
    Public Const ACIRiskTotalAnnualPremium As Integer = 8
    Public Const ACIRiskGisScreen As Integer = 9
    Public Const ACIRiskTypeId As Integer = 10
    Public Const ACIInsuranceFolderCnt As Integer = 11
    Public Const ACIRiskStatusFlag As Integer = 12
    ' PW311002 - add new columns for Risk Variations / Quote management
    Public Const ACIRiskNo As Integer = 13
    Public Const ACIVariationNo As Integer = 14
    Public Const ACIIsSelected As Integer = 15
    Public Const ACICoverage As Integer = 16
    Public Const ACIInsuredItem As Integer = 17
    Public Const ACIExtensions As Integer = 18
    ' PW221102 - add risk tax
    ' PS411
    Public Const ACIRiskTax As Integer = 19
    Public Const ACIRiskFolderCnt As Integer = 24
End Module