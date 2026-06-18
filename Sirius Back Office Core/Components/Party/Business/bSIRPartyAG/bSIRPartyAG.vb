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
    ' Date:  12/10/1998
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRPartyAG"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'Float Balance and Pre-Payment (RC)
    Public Const AC_PARTYAG_MakeLiveInvoice As Integer = 0
    Public Const AC_PARTYAG_MakeLiveInstallments As Integer = 1
    Public Const AC_PARTYAG_MakeLivePayNow As Integer = 2
    Public Const AC_PARTYAG_IsStandardAccount As Integer = 3
    Public Const AC_PARTYAG_IsFloatBalanceAccount As Integer = 4
    Public Const AC_PARTYAG_IsPrepaymentAccount As Integer = 5
    Public Const AC_PARTYAG_IsOverdraftAccount As Integer = 6
    Public Const AC_PARTYAG_FloatBalanceLimit As Integer = 7
    Public Const AC_PARTYAG_ExpectedDailyPremium As Integer = 8
    Public Const AC_PARTYAG_OverdraftLimit As Integer = 9
    Public Const AC_PARTYAG_DaysAllowed As Integer = 10
    Public Const AC_PARTYAG_OverdraftExpiry As Integer = 11
    '(RC) QBENZ014
    Public Const AC_PARTYAG_AltRefMandatory As Integer = 12
    Public Const AC_PARTYAG_AltRefRequiredForEachTrans As Integer = 13
    '(RC) PLICO 9-10
    Public Const AC_PARTYAG_CommissionPostingType As Integer = 14
    Public Const AC_PARTYAG_IsSingleInstalmentPlanOnly As Integer = 15
    Public Const AC_PARTYAG_CommonRenewalDate As Integer = 16
    'Batch Renewal
    Public Const AC_PARTYAG_IsProduceAgentRenewalList As Integer = 17
    Public Const AC_PARTYAG_MakeLiveBankGuarantee As Integer = 18
    'Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.13)
    Public Const AC_PARTYAG_MakeLiveCashDeposit As Integer = 19
    Public Const AC_PARTYAG_MakeLiveIsGrossAgent = 20

    Sub Main_Renamed()


    End Sub
End Module