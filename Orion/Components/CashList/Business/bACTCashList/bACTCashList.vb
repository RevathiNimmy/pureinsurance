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
    ' Date:  03/09/1997
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bACTCashList"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"





    Public Const ACDebit As Integer = 1
    Public Const ACCredit As Integer = -1



    Public Sub Main()


    End Sub
    ' Signs Variant Decimals Correctly
    Function ACTSign(ByVal v_vAmount As Double, ByVal v_lDebitOrCredit As Integer) As Decimal
        Return Math.Abs(v_vAmount) * Math.Sign(v_lDebitOrCredit)
    End Function
    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module