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
    ' Created: PW301002
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bPMUSourceDefaults"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"





    ' Defaults array position constants
    Public Const ACDPAgentID As Integer = 0
    Public Const ACDPDirectBusiness As Integer = 1

    Sub Main_Renamed()


    End Sub
End Module