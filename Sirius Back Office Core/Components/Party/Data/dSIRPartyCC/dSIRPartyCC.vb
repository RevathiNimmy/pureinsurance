Option Strict Off
Option Explicit On
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")>
Public Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  04/09/1998
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "dSIRPartyCC"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    '***********************************************
    'TODO: GLOBAL VARIABLE IN BAS MODULE
    ''Global reference to ListManager
    '***********************************************
    '***********************************************
    'TODO: GLOBAL VARIABLE IN BAS MODULE
    'Global g_oListManager As Object
    '***********************************************

    Sub Main_Renamed()


    End Sub
End Module