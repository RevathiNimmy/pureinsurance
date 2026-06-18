Option Strict Off
Option Explicit On
Imports SSP.Shared.StringsHelper
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
    Public Const ACApp As String = "bDOCViewBatch"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Username.
    Public g_sUsername As String = ""

    ' Password.
    Public g_sPassword As New FixedLengthString(30)

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

    Public g_iAdminLevel As Integer

    Public g_iAccessLevel As Integer

#If PD_EARLYBOUND = 1 Then

	Global m_oDatabase As dPMDAO.Database
	Global m_oMainDatabase As dPMDAO.Database
#Else
    Public m_oDatabase As Object
    Public m_oMainDatabase As dPMDAO.Database
#End If

    ' Reference to the miscellaneous class
    Public m_oMisc As bDOCViewBatch.Miscellaneous

    ' Current user and log level
    Public sCurrentUserName As String = ""
    Public iCurrentLogLevel As Integer


    Sub Main_Renamed()


    End Sub
End Module