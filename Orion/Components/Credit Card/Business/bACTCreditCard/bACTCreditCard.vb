Option Strict Off
Option Explicit On
Module MainModule

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  21nd December 2004
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '
    'developer guide no. 39
    Public Const ACSelMediaTypeIssuerAndConnectorDataSQL As String = "spu_ACT_Select_MediaTypeIssuerAndConnectorData"
    Public Const ACSelMediaTypeIssuerAndConnectorDataName As String = "SelectMediaTypeIssuerAndConnectorData"
    'developer guide no. 39
    Public Const ACSelPreviosulyUsedCCDataSQL As String = "spu_ACT_Select_PreviouslyUsedCCNumbers"
    Public Const ACSelPreviosulyUsedCCDataName As String = "SelectPreviosulyUsedCCData"

    Public Const kGetDefaultCreditcardByAccountSQL As String = "spu_GetDefaultCreditcardByAccount"
    Public Const kGetDefaultCreditcardByAccountName As String = "GetDefaultCreditcardByAccount"

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bACTCreditCard"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    Public Enum eSolveSEMessage
        eTestProbe = 0
        eAuthorisationRequest = 10
        eSettlement = 11
        eCancelTransaction = 13
        eAuthorisationAndSettle = 14
        eCompleteTransaction = 15
        eValidateOnly = 16
        eReload = 99
    End Enum

    Public Const k_eValidated As String = "validated"
    Public Const k_eTerminal As String = "terminal"
    Public Const k_eOnline As String = "on-line"
    Public Const k_eManual As String = "manual"
    Public Const k_eDeclined As String = "declined"
    Public Const k_eCancelled As String = "cancelled"
    Public Const k_eUnable_to_cancel As String = "unable_to_cancel"
    Public Const k_eGet_manual_auth As String = "get_manual_auth"
    Public Const k_eGet_sig_auth As String = "get_sig_auth"
    Public Const k_eTelephone As String = "telephone"
    Public Const k_eGet_manual_auth_resubmit As String = "get_manual_auth_re-submit"
    Public Const k_eHot_card_declined As String = "hot_card_declined"

    Public Enum eSolveSEState
        eUnknown
        eFormLoaded
    End Enum

    Public Enum eSolveSEExtendedError
        eAuthorisationFailed = 0
        eOkay = 50
        eNotInitialised = 51
        eAlreadyInitialised = 52
        eWinsockNotConnected = 53
        eNoHostPortDefined = 54
        eNoHostDestinationDefined = 55
        eUnknownTransactionId = 56
        eParseError = 57
        eConnectionTimeout = 58
        eConnectionError = 59
    End Enum

    Sub Main_Renamed()

    End Sub
End Module