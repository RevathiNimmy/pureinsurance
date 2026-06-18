Option Strict On
Option Explicit On

Imports System
Imports SharedFiles
Imports System.Data.SqlClient

Public Class frmSQLCredentials
    Inherits System.Windows.Forms.Form

    '===================================================
    'Module Name        : frmSQLCredentials
    'Project            : ILOGONMANAGER
    'Copyright          : © SSP Limited 2013. All rights reserved.

    'Description: Change SQL Login and Password for SQL Server Authentication

    'This code adhers to the SSP Coding Standards Document V2013-01

    'THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
    'EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
    'WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
    '===================================================

#Region "Private Constants"
    Private Const ACClass As String = "frmSQLCredentials"
#End Region

#Region "Private Variables"
    Private m_nReturnStatus As PMEReturnCode
#End Region

#Region "Public Properties"
    Public ReadOnly Property ReturnStatus() As PMEReturnCode
        Get
            Return m_nReturnStatus
        End Get

    End Property

#End Region

#Region "Public Methods"

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Set Interface Defaults
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInterfaceDefaults() As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMFalse

        Try

            ' Center the interface.
            iPMFunc.CenterForm(Me)
            Dim sDatabase As String = String.Empty
            txtDataBase.Enabled = False
            txtDataBaseServer.Enabled = False

            'Right now only supporting for SQL Server Authentication. 
            ' Switching to Windows Based Authentication is not allowed from here
            optSQLServerAuthentication.Checked = True
            optWindowsAuthentication.Enabled = False
            optSQLServerAuthentication.Enabled = False
            'Get the Server and Database Name from registry
            ' Get the database registry setting
            nResult = GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, _
                                        v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture, _
                                        v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer, _
                                        v_sSettingName:="Database", _
                                        r_sSettingValue:=sDatabase, _
                                        v_sSubKey:="Databases\Pure")
            If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                txtDataBase.Text = sDatabase
            End If
            Dim sServer As String = String.Empty
            ' Get the server setting
            nResult = GetPMRegSetting(v_lPMERegSettingRoot:=PMERegSettingRoot.pmeRSRLocalMachine, _
                                        v_lPMEProductFamily:=PMEProductFamily.pmePFSiriusArchitecture, _
                                        v_lPMERegSettingLevel:=PMERegSettingLevel.pmeRSLServer, _
                                        v_sSettingName:="Server", _
                                        r_sSettingValue:=sServer, _
                                        v_sSubKey:="Databases\Pure")

            If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                txtDataBaseServer.Text = sServer
            End If
            DisplayPureVersion()
            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            Throw New Exception("Failed to set the interface defaults", excep)
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Check the Database Connection to validate the LoginId and Password.
    ''' </summary>
    ''' <param name="sServer"></param>
    ''' <param name="sDataBase"></param>
    ''' <param name="sLoginId"></param>
    ''' <param name="sPassword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckDBConnection(ByVal sServer As String, _
                                       ByVal sDataBase As String, _
                                       ByVal sLoginId As String, _
                                       ByVal sPassword As String) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kConnectionStringFrame As String = "Server={server};Database={database};Integrated Security=False; User ID={loginid}; Password={loginpassword}"
        Dim sConnectionString As String = String.Empty
        Dim con As SqlConnection = Nothing
        Try

            ' Replace the placeholders with the correct values
            sConnectionString = Replace(kConnectionStringFrame, "{server}", sServer)
            sConnectionString = Replace(sConnectionString, "{database}", sDataBase)
            sConnectionString = Replace(sConnectionString, "{loginid}", sLoginId)
            sConnectionString = Replace(sConnectionString, "{loginpassword}", sPassword)

            ' Is there a Current Connection
            If con Is Nothing Then
                ' Create a New Connection
                con = New SqlConnection()
            End If

            If con.State = ConnectionState.Closed Then
                con.ConnectionString = sConnectionString
                con.Open()
            End If
            con.Close()
        Catch excep As System.Exception
            ' Error Section
            nResult = gPMConstants.PMEReturnCode.PMError
            Throw New ApplicationException("CheckDBConnection Failed. Failed to Establish a Connection to the Database", excep)
        Finally
            con = Nothing
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Display the Pure Version
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DisplayPureVersion() As Integer
        Dim nReturn As Integer = PMEReturnCode.PMFalse
        Dim sVersion As String = String.Empty
        Dim sRelease As String = String.Empty
        Dim sSiriusType As String = String.Empty
        Try

            ' get sirius version details
            nReturn = GetSiriusVersion(sVersion, sRelease, sSiriusType)

            Dim sSR As String = ""
            Dim sBuild As String = ""
            Dim vStringArray() As String

            sBuild = " Build " & sRelease.Substring(sRelease.LastIndexOf("."c) + 1)
            vStringArray = sRelease.Split("."c)
            sSR = " SR" & vStringArray(0)

            'Do Not Show Build Numbers.
            sVersion = sVersion & sSR & sBuild

            ' Set the version number and date
            lblVersion.Text = "Version " & sVersion
            nReturn = PMEReturnCode.PMTrue

        Catch excep As Exception
            nReturn = PMEReturnCode.PMFalse
            Throw New ApplicationException("DisplayPureVersion Failed", excep)
        End Try

        Return nReturn
    End Function

#End Region

#Region "Event Handlers"


    Private Sub frmSQLCredentials_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            SetInterfaceDefaults()

        Catch excep As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,sMsg:="Failed to Load",vApp:=ACApp, vClass:=ACClass, vMethod:="frmSQLCredentials_Load", excep:=excep)
        End Try
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        m_nReturnStatus = PMEReturnCode.PMCancel
        Me.Hide()
    End Sub
    ''' <summary>
    ''' check the DB connection and IF valid update the LoginId and Password in Registry
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOk.Click
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMFalse
        Try

            'Check the DataBase Connection
            nResult = CheckDBConnection(sServer:=txtDataBaseServer.Text, sDataBase:=txtDataBase.Text, sLoginId:=txtSQLloginId.Text, sPassword:=txtSQLPassword.Text)
            If nResult <> PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to Connect " & Strings.Chr(10).ToString() & "Please check the LoginID and Password.", _
                                "Failed to Connect", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            'Update the Registry if Correct user Name and Password is Provided. 


            Dim sApplicationPath As String = String.Empty
            sApplicationPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            If Not sApplicationPath.EndsWith("\") Then
                sApplicationPath += "\"
            End If

            'Call the PasswordRehasher to update the Registry  With LoginId and Password
            Dim oProcessStartInfo As New ProcessStartInfo
            Dim oProcess As New Process
            With oProcessStartInfo
                .UseShellExecute = True
                .FileName = sApplicationPath & "PasswordRehasher.exe"
                .Arguments = txtSQLloginId.Text.Trim & " " & txtSQLPassword.Text
                .WindowStyle = ProcessWindowStyle.Hidden
                .Verb = "runas" 'add this to prompt for elevation
            End With

            oProcess = Process.Start(oProcessStartInfo)
            oProcess.WaitForExit()
            m_nReturnStatus = PMEReturnCode.PMTrue
            Me.Hide()

        Catch excep As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,sMsg:="Failed to cmdOK_Click",vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", excep:=excep)
        End Try


    End Sub

    Private Sub txtSQLloginId_TextChanged(sender As Object, e As EventArgs) Handles txtSQLloginId.TextChanged
        If LTrim(RTrim(txtSQLloginId.Text)) = "" Then
            cmdOk.Enabled = False
        Else
            cmdOk.Enabled = True
        End If
    End Sub
#End Region

End Class
