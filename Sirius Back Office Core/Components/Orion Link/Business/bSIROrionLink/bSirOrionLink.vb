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
    ' Date:  31/03/1998
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSirOrionLink"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"
    'AK 010802 - these constants are part of gSIRLibrary now
    ''EK 24/11/99
    ''Constants for Ledger Names
    'Public Const SIRACTInsurerLedgerShortName = "I"
    'Public Const SIRACTAgentLedgerShortName = "A"
    'Public Const SIRACTFeeLedgerShortName = "F"
    'Public Const SIRACTDiscountLedgerShortName = "D"
    'Public Const SIRACTCommissionLedgerShortName = "C"
    'Public Const SIRACTPremiumFinanceLedgerShortName = "R"
    'Public Const SIRACTSubAgentLedgerShortName = "U"
    'Public Const SIRACTNominalLedgerShortName = "N"
    'RWH(24/07/01) Other Party Types
    'Public Const SIRACTOtherPartySingleCharCode = "O"
    ''Tomo300701 - Would have been nice if these had been here.  How did this compile?
    'Public Const SIRACTOtherPartyRecLedgerShortName = "OR"
    'Public Const SIRACTOtherPartyPayLedgerShortName = "OP"
    '
    'Public Const SIRACTClientLedgerMappingCode = "Client Ledger"
    'Public Const SIRACTInsurerLedgerMappingCode = "Insurer Ledger"
    'Public Const SIRACTAgentLedgerMappingCode = "Agent Ledger"
    'Public Const SIRACTFeeLedgerMappingCode = "Fees Receivable"
    'Public Const SIRACTDiscountLedgerMappingCode = "Discounts"
    'Public Const SIRACTCommissionLedgerMappingCode = "Commission"
    'Public Const SIRACTPremiumFinanceLedgerMappingCode = "Premium Finance"
    'Public Const SIRACTSubAgentLedgerMappingCode = "Sub Agent Ledger"
    'Public Const SIRACTNominalLedgerMappingCode = "Nominal Ledger"

    Public Sub Main()

    End Sub
    'UPGRADE_NOTE: (7013) Constructor is just executed once. Please review if Component contains SingleUse classes because they have a different behaviour. More Information: http://www.vbtonet.com/ewis/ewi7013.aspx
    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module