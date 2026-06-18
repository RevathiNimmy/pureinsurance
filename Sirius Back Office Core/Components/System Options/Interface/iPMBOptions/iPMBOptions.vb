Option Strict Off
Option Explicit On
Imports System
Module MainModule
    Public Const ACApp As String = "IPMBOptions"
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iLogLevel As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_sUserName As String = ""
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iUserId As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iSourceId As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_sPassword As String = ""
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iLanguageId As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iCurrencyId As Integer
    'developer guide no. 107
    <ThreadStatic()>
    Public g_sCallingAppName As String = ""
    'developer guide no. 107
    <ThreadStatic()>
    Public g_oObjectManager As bObjectManager.ObjectManager

    Public Const ACChequeProductionNotInstalled As String = "Cheque Production not installed"
    Public Const ACChequeProductionInstalled As String = "Cheque Production installed"

    Public Const ACEnhChequeProductionCM As String = "ChequeMaster"
    Public Const ACEnhChequeProductionInHouse As String = "In-House Production"
    Public Const ACEnhChequeProductionExport As String = "Export"


    Public Const ACQASNotInstalled As String = "No Address Lookup installed"
    Public Const ACQASRapid As String = "UK QAS Rapid installed"
    Public Const ACQASPro As String = "UK QAS Pro installed"
    Public Const ACQASNames As String = "UK QAS Names installed"
    Public Const ACPAFWrapper As String = "UK PAF Wrapper installed"
    Public Const ACQASProRegistry As String = "QASPro_Path"
    Public Const ACQASRapidRegistry As String = "QASRapid_Path"
    'Modified,added helpcontextid as per vbcode
    Public Const HelpContextID As Integer = 5015
    ' CTAF 020600
    Public Const ACNoTemplate As String = "(none)"

    Public Const ScreenHelpID As Integer = 15
    'Start (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.2)
    Public Const ACEnhancedPersonalClientResolvedName As String = "Enhanced Personal Client Resolved Name"
    Public Const ACUpdateExistingClients As String = "Update Existing Clients"
    Public Const ACDisableAllWildcardSearches As String = "Disable All Wildcard Searches"
    Public Const ACEnableWildcardSearchesEndingWith As String = "Enable Wildcard Searches Ending With %"
    'End (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.2)

    Public Declare Function LockWindowUpdate Lib "user32" (ByVal hWndLock As Integer) As Integer

    Public Const ACAgentCommissionSuspendedPosting As String = "Agent Commission Suspended Postings:"
    Public Const kExclusiveLocking As String = "Enable Exclusive Locking:"
    Public Const kPartyHistoryLoggingEnabled As String = "Party History Logging Enabled"
    Public Const kClaimsReservesGross As String = "Claims Reserves are Gross"
End Module