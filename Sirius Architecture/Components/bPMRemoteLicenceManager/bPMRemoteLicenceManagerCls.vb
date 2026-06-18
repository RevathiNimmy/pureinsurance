Option Strict Off
Option Explicit On
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("LicenceManager_NET.LicenceManager")>
Public NotInheritable Class LicenceManager
    Implements bPMLicenceManager.LicenceManager
    ' ***************************************************************** '
    ' Class Name: LicenceManager
    '
    ' Date: 21 April 1998
    '
    ' Description: This class implements bPMLicenceManager.LicenceManager
    '
    ' Edit History:
    ' RFC 21/04/1998 Original
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Implement ALL Licence Manager methods

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "LicenceManager"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    Private m_oLicenceManager As bPMLicenceManager.LicenceManager
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    Public Property WarningMessage As String Implements bPMLicenceManager.LicenceManager.WarningMessage

    Public Property ErrorMessage As String Implements bPMLicenceManager.LicenceManager.ErrorMessage

    ' ***************************************************************** '
    ' Name: LicenceManager_Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '              Checks the licence limit.
    ' ***************************************************************** '
    Public Function LicenceManager_Initialise() As Integer Implements bPMLicenceManager.LicenceManager.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Instance of LicenceManager
            m_oLicenceManager = New bPMLicenceManager.LicenceManager_CoClass

            ' Call the Initialise Method of LicenceManager

            Return m_oLicenceManager.Initialise()

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise object", vApp:=ACApp, vClass:=ACClass, vMethod:="LicenceManager_Initialise", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LicenceManager_Logon
    '
    ' Description: Logs a user onto the system.
    '              Checks Licence Limit and Password details first.
    ' ***************************************************************** '
    Public Function LicenceManager_Logon(ByVal v_sUsername As String, ByVal v_sPassword As String, ByRef r_sClientSystemName As String, ByRef r_bPMBLinkRequired As Boolean, ByRef r_oClientManager As BCLIENTMANAGER.ClientManager) As Integer Implements bPMLicenceManager.LicenceManager.Logon

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the Logon method of LicenceManager

            Return m_oLicenceManager.Logon(v_sUsername:=v_sUsername, v_sPassword:=v_sPassword, r_sClientSystemName:=r_sClientSystemName, r_bPMBLinkRequired:=r_bPMBLinkRequired, r_oClientManager:=r_oClientManager)

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Set the return object to nothing.

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to LicenceManager_Logon object", vApp:=ACApp, vClass:=ACClass, vMethod:="LicenceManager_Logon", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements bPMLicenceManager.LicenceManager.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If Not (m_oLicenceManager Is Nothing) Then
                    ' Call the Licence Manager Terminate Method
                    m_oLicenceManager.Dispose()
                    m_oLicenceManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub
    ' ***************************************************************** '
    ' Name: LicenceManager_Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error Section.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try


    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ' RDC 19102000 used by LogonManager to determine status of temporary licensing.
    ' Changes here must be reflected in LocalLicenceManager.GetTemporaryLicenceDetails
    Public Function GetTemporaryLicenceDetails(ByRef sEncryptedStatus As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMFalse

            lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="InstallationConfig", r_sSettingValue:=sEncryptedStatus)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sEncryptedStatus = "" Then
                ' registry setting is missing
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTemporaryLicenceDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTemporaryLicenceDetails", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
