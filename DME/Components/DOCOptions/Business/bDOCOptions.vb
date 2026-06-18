Option Strict Off
Option Explicit On
Imports SSP.Shared

Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  03/12/97
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bDOCOptions"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Username.
    Public g_sUsername As String = ""

    ' Password.
    Public g_sPassword As New StringsHelper.FixedLengthString(30)

    ' User ID
    Public g_iUserID As Integer

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

    Public Const ACGetOptionStored As Boolean = True
    Public Const ACGetOptionName As String = "GetDMEOption"
    'developer's guide no. 39
    Public Const ACGetOptionSQL As String = "spu_DOC_get_option"

    Public Const ACSetOptionStored As Boolean = True
    Public Const ACSetOptionName As String = "SetDMEOption"
    'developer's guide no. 39
    Public Const ACSetOptionSQL As String = "spu_DOC_set_option"

    Sub Main_Renamed()


    End Sub
End Module