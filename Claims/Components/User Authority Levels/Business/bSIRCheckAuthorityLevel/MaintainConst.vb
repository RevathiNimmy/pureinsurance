Option Strict Off
Option Explicit On
Module BusinessConst
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: BusinessConst
    '
    ' Date: 15-11-2002
    '
    ' Description: Constants used by both business
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "BusinessConst"

    Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Module