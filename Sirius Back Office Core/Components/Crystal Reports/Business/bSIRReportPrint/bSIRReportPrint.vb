Option Strict Off
Option Explicit On
'developer guide no.129
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
    ' Date:  07/01/1999
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRReportPrint"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"


    ' Constants to define print operations
    Public Const AC_VIEW_ONLY As Integer = 0
    Public Const AC_PRINT_ONLY As Integer = 1
    Public Const AC_PRINT_AND_VIEW As Integer = 2
    Public Const AC_EXPORT_TO_HTML As Integer = 3
    Public Const AC_EXPORT_TO_PDF As Integer = 4

    'JK010499
    ' Get Financial End Date from built query.
    Public Const ACFinancialYearEndQueryStored As Boolean = False
    Public Const ACFinancialYearEndQueryName As String = "FinancialYearEndQuery"
    Public Const ACFinancialYearEndQuerySQL As String = "{}"

    ' TB161002 specific checks on some reports
    Public Const ACRptName_UWPolicyListLong As String = "UNDERWRITING\POLICY_LISTING_LONG"
    Public Const ACRptName_AgencyDebitingBordereau As String = "AGENCY\SUBAGENT_DEBITING_BORDEREAU_DETAILED"
    Public Const ACRptName_AgencyPaidBordereau As String = "AGENCY\SUBAGENT_PAID_BORDEREAU_DETAILED"
    Public Const ACRptName_AccountsEarnedPremium As String = "ACCOUNTS\EARNED_PREMIUM"
    Public Const ACRptName_AccountsUnearnedPremium As String = "ACCOUNTS\UNEARNED_PREMIUM"
    Public Const ACRptName_ClaimsOSClaims As String = "CLAIMS\OUTSTANDING_CLAIMS"
    Public Const ACRptName_ClaimsOSClaimsGrossToNet As String = "CLAIMS\OUTSTANDING_CLAIMS_GROSS_TO_NET"
    'MKR 25/10/2004 PN 15730 -- specific checks on some reports added
    Public Const ACRptName_AccountsProfitAndLossAll As String = "ACCOUNTS\PROFIT_&_LOSS_ALL"
    Public Const ACRptName_AccountsProfitAndLossYear As String = "ACCOUNTS\PROFIT_&_LOSS_YEAR"
    Public Const ACRptName_AccountsProfitAndLossBudget As String = "ACCOUNTS\PROFIT_&_LOSS_BUDGET"
    Public Const ACRptName_TrialBalance As String = "ACCOUNTS\TRIAL_BALANCE_U"
    Public Const ACRptName_TrialBalanceSummary As String = "ACCOUNTS\TRIAL_BALANCE_SUMMARY"
    ' TB281002 Dropdown list limits
    Public Const ACDropDownLimit As Integer = 32766

    Public Const ACRptName_AgentCommissionStatement As String = "AGENCY\COMMISSION_STATEMENT" 'PN# 68632
    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    'Public g_lDDLimit As Long
    '***********************************************


    Sub Main_Renamed()

    End Sub

    Public Function Strip(ByRef v_sTextString As String) As String

        Dim result As String = String.Empty
        result = CStr(gPMConstants.PMEReturnCode.PMTrue)

        If v_sTextString Is Nothing Then
            Return String.Empty
        End If
        Dim sTextString As String = v_sTextString.Trim()

        Dim iPos As Integer = (sTextString.IndexOf(Strings.ChrW(0).ToString()) + 1)

        If iPos > 0 Then
            sTextString = v_sTextString.Substring(0, iPos - 1)
        End If

        Return sTextString.Trim()

    End Function
End Module