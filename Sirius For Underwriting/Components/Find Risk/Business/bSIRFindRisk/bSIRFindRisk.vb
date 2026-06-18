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
    ' Date:  27th September 1996
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRFindRisk"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    'Start (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
    Public Const ACColCNRiskLinkId As Integer = 0
    Public Const ACColCNRiskId As Integer = 1
    Public Const ACColCNRef As Integer = 2
    Public Const ACColCNFrom As Integer = 3
    Public Const ACColCNTo As Integer = 4
    Public Const ACColCNRow As Integer = 5
    Public Const ACColCNAttach As Integer = 6

    'End (Venkatesh Raman)Tech Spec - WR19 - Cover Note Functionality section()
End Module