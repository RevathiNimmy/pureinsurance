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
    ' Date:  08/10/1998
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRPartyOT"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"





    'SD 17/09/2002 Add Supplier party type changes
    Public Const ACActiveIndicator As Integer = 0
    Public Const ACAfterHoursIndicator As Integer = 1
    Public Const ACPriorityIndicator As Integer = 2
    Public Const ACTPASettleDirectly As Integer = 3
End Module