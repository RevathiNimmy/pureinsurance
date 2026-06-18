Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  07/04/1998
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '



    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bACTCashlistitem"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Username.

    ' Password.

    ' User ID

    ' Calling Application

    Public Enum ENBankGuarantee
        BGId
        BankNameId
        BankName
        BGRef
        DueDate
        InsuranceFileCnt
        InsuranceRef
        Amount
        OutstandingAmount
        SourceID
        SourceDescription
        ProductId
        ProductDescription
        SourceCode
        CoverStartDate
        ExpiryDate
        ProductCode
        AmtTobePosted
        LastItem = ENBankGuarantee.AmtTobePosted
    End Enum


    Public Sub Main()


    End Sub
    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module