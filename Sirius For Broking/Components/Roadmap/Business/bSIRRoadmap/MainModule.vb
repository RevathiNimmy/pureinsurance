Option Strict Off
Option Explicit On
Imports SSP.Shared.StringsHelper
Module MainModule

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  07/05/1999
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRRoadmap"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Username.
    Public g_sUsername As New FixedLengthString(12)

    ' Password.
    Public g_sPassword As New FixedLengthString(30)

    ' User ID
    'Public g_iUserID As Integer

    ' Calling Application
    'Public g_sCallingAppName As String
    ' Source ID
    'Public g_iSourceID As Integer
    ' Language ID
    'Public g_iLanguageID As Integer
    ' Currency ID
    'Public g_iCurrencyID As Integer
    ' LogLevel
    'Public g_iLogLevel As Integer
    ' Public instance of the object manager.
    'developer guide no. 107
    'Commented by sweta to remove bObjectManager dependency.
    '<ThreadStatic()>
    'Public g_oObjectManager As bObjectManager.ObjectManager
End Module