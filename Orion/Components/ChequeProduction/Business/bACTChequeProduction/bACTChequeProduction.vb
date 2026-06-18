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
    ' Date:  07/02/2001
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bACTChequeProduction"


    ' Constants for data array indexes.
    Public Const ACIBankCode As Integer = 0
    Public Const ACIChequeID As Integer = 1
    Public Const ACITransactionID As Integer = 2
    Public Const ACITransactionDate As Integer = 3
    Public Const ACIReference As Integer = 4
    Public Const ACIAmount As Integer = 5
    Public Const ACICurrencyID As Integer = 6
    Public Const ACIChequeNumber As Integer = 7
    Public Const ACIAccountID As Integer = 8
    Public Const ACIAccountCode As Integer = 9
    Public Const ACIAccountName As Integer = 10
    Public Const ACIAddress1 As Integer = 11
    Public Const ACIAddress2 As Integer = 12
    Public Const ACIAddress3 As Integer = 13
    Public Const ACIAddress4 As Integer = 14
    Public Const ACIPostalCode As Integer = 15
    Public Const ACIDocumentRef As Integer = 16
    Public Const ACIPartyCnt As Integer = 17
    Public Const ACIOurRef As Integer = 18
    Public Const ACISourceID As Integer = 19
    Public Const ACISourceDescription As Integer = 20
    Public Const ACIBankID As Integer = 21

    ' ***************************************************************** '
    ' Caption Array Column Position Constants
    ' ***************************************************************** '
    Public Enum ChequeSequenceArrColPosition
        ACLastSeqNumber = 0
        ACNextSeqNumber = 1
        ACFirstAvailable = 2
        ACLastAvailable = 3
        ACNumbersAvailable = 4
    End Enum

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'BankWise Starting Cheque Number
    Public Const ACBankID As Integer = 0
    Public Const ACBankCode As Integer = 1
    Public Const ACStartChequeNumber As Integer = 2


    Public Sub Main()

    End Sub
    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module