Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 07/01/1999
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMBReportPrint"
    Public m_sCallingAppName As String = ""
    Public m_sReportOutputLocation As String = ""
    Public m_sUserReportName As String = ""
    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACPreviewButton As Integer = 204
    Public Const ACExportButton As Integer = 205
    Public Const ACPrintButton As Integer = 206

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    ' Menus

    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    '31/10/2002 - PWC - Added new Key Names
    Public Const PMKeyNameFilterReports As String = "filter_reports"
    Public Const PMKeyNameSaveParams As String = "save_params"

    ' Public source and language ID's from the
    ' Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'JMK 16/06/2001
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' TF271100 - API calls for Report Preview
    Declare Function GetActiveWindow Lib "user32" () As Integer
    Declare Function IsWindow Lib "user32" (ByVal hwnd As Integer) As Integer
    Declare Sub Sleep Lib "kernel32" (ByVal dwMilliseconds As Integer) 'MKW160204 PN10450

    ' KB 030801 Required for help text link
    ' Value here is base value to allow for underwriting or agency
    ' decision later

    Public Const ScreenHelpID As Integer = 4013
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    ' TB161002 Special report
    Public Const ACRptName_UWPolicyListLong As String = "UNDERWRITING\POLICY_LISTING_LONG"
    Public Const ACRptName_AgencyDebitingBordereau As String = "AGENCY\SUBAGENT_DEBITING_BORDEREAU_DETAILED"
    Public Const ACRptName_AgencyPaidBordereau As String = "AGENCY\SUBAGENT_PAID_BORDEREAU_DETAILED"

    Public Const ACRptName_BRPolicyListLong As String = "COMPLIANCE ASSISTANCE\RISK_TRANSFER"
    ' TB281002 Dropdown list limits
    Public Const ACDropDownLimit As Integer = 32766
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_lDDLimit As Integer
    'DC270303 -ISS1911
    Public Const ACDateColumn As Integer = 10

    Public Const ACLabelTypeOfCurrency As String = "Type Of Currency:"
    Public Const ACLabelGroupBy As String = "Group By:"
    Public Const ACLabelBranch As String = "Branch:"
    Public Const ACLabelLookUpTable As String = "Lookup Table:"

    Public Const ACTypeOfCurrencyCodeAccount As String = "A"
    Public Const ACTypeOfCurrencyCodeBase As String = "B"
    Public Const ACTypeOfCurrencyCodeSystem As String = "S"
    Public Const ACTypeOfCurrencyCodeTransaction As String = "T"

    'If the following option is in the default value then the Type Of Currency in the
    'default value will only be shown when a branch is selected (not ALL branches).
    Public Const ACRestrictionCodeBranchSelectedOnly As String = "SB"

    Public Const ACTypeOfCurrencyAccount As String = "Account"
    Public Const ACTypeOfCurrencyBase As String = "Base"
    Public Const ACTypeOfCurrencySystem As String = "System"
    Public Const ACTypeOfCurrencyTransaction As String = "Transaction"

    Public Const ACGroupByCodeNothing As String = "GX"
    Public Const ACGroupByCodeBranch As String = "GB"
    Public Const ACGroupByCodeCurrency As String = "GC"
    Public Const ACGroupByCodeBranchCurrency As String = "GD"
    Public Const ACGroupByCodeCurrencyBranch As String = "GE"

    Public Const ACGroupByNothing As String = "No Grouping"
    Public Const ACGroupByBranch As String = "Branch"
    Public Const ACGroupByCurrency As String = "Currency"
    Public Const ACGroupByBranchCurrency As String = "Branch and Currency"
    Public Const ACGroupByCurrencyBranch As String = "Currency and Branch"

    Sub Main_Renamed()

    End Sub

    Public Function Strip(ByRef v_sTextString As String) As String

        Dim result As String = String.Empty
        result = CStr(gPMConstants.PMEReturnCode.PMTrue)

        Dim sTextString As String = v_sTextString.Trim()

        Dim iPos As Integer = (sTextString.IndexOf(Strings.Chr(0).ToString()) + 1)

        If iPos > 0 Then
            sTextString = v_sTextString.Substring(0, iPos - 1)
        End If

        Return sTextString.Trim()

    End Function
End Module