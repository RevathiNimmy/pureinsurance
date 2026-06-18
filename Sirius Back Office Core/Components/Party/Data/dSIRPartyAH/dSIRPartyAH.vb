Option Strict Off
Option Explicit On
Imports SSP.Shared
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  11/08/1999
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "dSIRPartyAH"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Username.
    Public g_sUsername As New StringsHelper.FixedLengthString(12)

    ' Password.
    Public g_sPassword As New StringsHelper.FixedLengthString(30)

    ' User ID
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iUserID As Integer

    ' Calling Application
    'developer guide no. 107
    <ThreadStatic()>
    Public g_sCallingAppName As String = ""
    ' Source ID
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iSourceID As Integer
    ' Language ID
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iLanguageID As Integer
    ' Currency ID
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iCurrencyID As Integer
    ' LogLevel
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iLogLevel As Integer

    Sub Main_Renamed()


    End Sub
End Module