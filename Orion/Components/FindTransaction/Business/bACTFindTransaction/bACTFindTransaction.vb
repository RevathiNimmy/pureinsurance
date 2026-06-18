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
    ' Date:  28 August 1997
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '



    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bFindTrans"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"






    ' Log Level




    Public Sub Main()

        ' Main entry point for the component

    End Sub
    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module