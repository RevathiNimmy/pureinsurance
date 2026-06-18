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
    ' Date:  16/01/1998
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bDOCAnnotation"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"




    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    'Public m_lreturn As Long
    '***********************************************
    Sub Main_Renamed()

    End Sub
End Module