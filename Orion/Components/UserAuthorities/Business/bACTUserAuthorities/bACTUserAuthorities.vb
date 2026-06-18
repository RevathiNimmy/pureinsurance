Option Strict Off
Option Explicit On
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
    ' Date:  14/02/2000
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bACTUserAuthorities"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'Start(Saurabh Agrawal)- Tech Spec WR3 User Level RI Display Restriction(5.2)
    'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling - Modified 12 to 13
    'Public Const ACParamsCount As Integer = 13 'Initiallly Public Const ACParamsCount = 7
    'Start PN:75358(Sumit Singla) Change: Set ACParamsCount 13 to 15
    Public Const kParamsCount As Integer = 28 'Initiallly Public Const ACParamsCount = 23
    'End (Sumit Singla0)


    'End(Saurabh Agrawal)- Tech Spec WR3 User Level RI Display Restriction(5.2)

    Public Const ACIsRecommenderArrPos As Integer = 0
    Public Const ACRecommendationCurrencyArrPos As Integer = 1
    Public Const ACRecommendationAmountArrPos As Integer = 2

    'Payment Maintenance
    Public Const ACCanReverseAllocationArrPos As Integer = 3
    Public Const ACTimePeriodForReversalArrPos As Integer = 4

    'For Electra M3 Find Transaction Changes
    Public Const ACCanReverseReplaceArrPos As Integer = 5
    Public Const ACMTAAuthorityArrPos As Integer = 6
    Public Const ACChequeNumberArrPos As Integer = 7

    'Start(Saurabh Agrawal)- Tech Spec WR3 User Level RI Display Restriction(5.2)
    Public Const ACDisplayReinsuranceScreen As Integer = 8
    Public Const ACDisplayClaimReinsurance As Integer = 9
    'End(Saurabh Agrawal)- Tech Spec WR3 User Level RI Display Restriction(5.2)
    'Start(Gaurav Arora)- Tech Spec WR6 Bank Guarantee
    Public Const ACMakeLiveBankGuarantee As Integer = 10
    'End(Gaurav Arora)- Tech Spec WR6 Bank Guarantee

    Public Const ACCanBackdateCollectionDate As Integer = 11
    Public Const ACEditDefaultCommission As Integer = 12

    Public Const ACMakeLiveCashDeposit As Integer = 13 ' Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
    'Start PN: 75358(Sumit Singla)
    Public Const ACUserCanDebugDynamicLogicScripts As Integer = 14
    Public Const ACUserServerScriptsRunInDebug As Integer = 15
    Public Const ACUserCanChangeInstalmentDefaultCurrency As Integer = 23 'WPR IH 10
    Public Const ACInstalmentStatus As Integer = 24

    'End (Sumit Singla)
    Public Const ACCanEditInstalmentDueDate As Integer = 25
    Public Const ACEditInstalmentDateByNoOfDays As Integer = 26

    Public Const kEditDefaultCommissionNBRN As Integer = 16
    Public Const kEditDefaultCommissionMTA As Integer = 17
    Public Const kEditDefaultCommissionMTR As Integer = 18
    Public Const kEditDefaultCommissionMTC As Integer = 19
    Public Const kEditAgentDuringMTAMTC As Integer = 20
    Public Const ACCanReverseReceiptArrPos As Integer = 21
    Public Const kACCViewBatchProcessStatus As Integer = 22
    Public Const kACCCanExtractClientData As Integer = 27

    Sub Main_Renamed()


    End Sub
End Module