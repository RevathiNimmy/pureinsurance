Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("PMServerRegistry_NET.PMServerRegistry")> _
Public NotInheritable Class PMServerRegistry
    Implements IDisposable
    'Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: PMServerRegistry
    '
    ' Date: 12/07/2000
    '
    ' Description: Reads/Updates Server registry settings
    '
    ' Edit History:
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


    Private Const ACClass As String = "PMServerRegistry"
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description: Standard function to initialise the object
    '
    ' History: 12/07/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(Optional ByVal sUsername As String = "", Optional ByVal sPassword As String = "", Optional ByVal iUserID As Integer = 0, Optional ByVal iSourceID As Integer = 0, Optional ByVal iLanguageID As Integer = 0, Optional ByVal iCurrencyID As Integer = 0, Optional ByVal iLogLevel As Integer = 0, Optional ByVal sCallingAppName As String = "", Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long 'Implements SSP.S4I.Interfaces.IBusiness.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            If Not False Then
            Else
                m_sUsername = "sirius"
            End If

            If Not False Then
            Else
                m_sPassword = "sirius"
            End If

            If Not False Then
            Else
                m_iUserID = 1
            End If

            If Not False Then
            Else
                m_iSourceID = 1
            End If

            If Not False Then
            Else
                m_iLanguageID = 1
            End If

            If Not False Then
            Else
                iCurrencyID = 1
            End If

            If Not False Then
            Else
                iLogLevel = 4
            End If

            If Not False Then
            Else
                sCallingAppName = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description: Standard function to terminate the object
    '
    ' History: 12/07/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetServerRegSetting
    '
    ' Description: Function to get registry settings on Server from the
    '              interface object.
    '
    ' History: 11/07/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetServerRegSetting(ByVal v_lPMERegSettingRoot As Integer, ByVal v_lPMEProductFamily As Integer, ByVal v_lPMERegSettingLevel As Integer, ByVal v_sSettingName As String, ByRef r_sSettingValue As String, Optional ByVal v_sSubKey As String = "") As Integer

        Dim result As Integer = 0
        Try


            Return gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=v_lPMERegSettingRoot, v_lPMEProductFamily:=v_lPMEProductFamily, v_lPMERegSettingLevel:=v_lPMERegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=r_sSettingValue, v_sSubKey:=v_sSubKey)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetServerRegSetting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServerRegSetting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetServerRegSetting
    '
    ' Description: Function to set registry settings on Server from the
    '              interface object.
    '
    ' History: 11/07/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function SetServerRegSetting(ByVal v_lPMERegSettingRoot As Integer, ByVal v_lPMEProductFamily As Integer, ByVal v_lPMERegSettingLevel As Integer, ByVal v_sSettingName As String, ByVal v_sSettingValue As String, Optional ByVal v_sSubKey As String = "") As Integer

        Dim result As Integer = 0
        Try


            Return gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=v_lPMERegSettingRoot, v_lPMEProductFamily:=v_lPMEProductFamily, v_lPMERegSettingLevel:=v_lPMERegSettingLevel, v_sSettingName:=v_sSettingName, v_sSettingValue:=v_sSettingValue, v_sSubKey:=v_sSubKey)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetServerRegSetting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetServerRegSetting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Class_Initialize
    '
    ' Description: Standard class initialize
    '
    ' History: 12/07/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try


    End Sub

    ' ***************************************************************** '
    '
    ' Name: Class_Terminate
    '
    ' Description:
    '
    ' History: 12/07/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
