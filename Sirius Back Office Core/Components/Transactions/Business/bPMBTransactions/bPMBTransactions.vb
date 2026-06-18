Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  09/11/1998
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bPMBTransactions"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'Constant for Credit Control option
    Public Const kSystemOptionCreditControlEnabled As Integer = 4

    Public Sub Main()

        ' Main entry point for the component

    End Sub
    'UPGRADE_NOTE: (7013) Constructor is just executed once. Please review if Component contains SingleUse classes because they have a different behaviour. More Information: http://www.vbtonet.com/ewis/ewi7013.aspx
    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module