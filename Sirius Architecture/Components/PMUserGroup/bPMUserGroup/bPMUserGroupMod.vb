Option Strict Off
Option Explicit On
Imports SSP.Shared
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  17 October 1996
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bPMUserGroup"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Username.
    Public g_sUsername As String = ""

    ' Password.
    Public g_sPassword As New StringsHelper.FixedLengthString(30)

    ' Calling Application
    Public g_sCallingAppName As String = ""
    ' Source ID
    Public g_iSourceID As Integer
    ' Language ID
    Public g_iLanguageID As Integer
    ' Currency ID
    Public g_iCurrencyID As Integer
    ' LogLevel
    Public g_iLogLevel As Integer
    ' UserID
    Public g_iUserID As Integer

    Sub Main_Renamed()

        ' Main entry point for the component

    End Sub
End Module