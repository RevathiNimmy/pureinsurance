Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports Microsoft.VisualBasic.Compatibility.VB6



Public Class frmInterface

    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 03 July 1996
    '
    ' Description: Main View Form.
    '
    ' Edit History:
    ' RFC 23/04/1998 - Amended to allow local or remote logon.
    ' RKS 13/12/2004 - Implementation of Unified Logon Process
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Form cancelled flag.
    Private m_bFormCancelled As Boolean

    ' Form error number.
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    ' Object parameter member.
    Private m_vObjectParam As Object

    ' Object event type.
    Private m_sEventType As String = ""

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iLogonManager.General

    'RDC07062001 SA client software version number from registry
    Private m_sVersion As String = ""
    Private m_sRelease As String = ""
    Private m_sSiriusType As String = ""
    ' PRIVATE Data Members (End)

    ' RDC 11072002 Unified logon status
    Private m_bUnifiedLogon As Boolean
    Private m_sUnifiedLogonUsername As String = ""
    Private Const vbFormCode As Integer = 0
    Private m_bPasswordChanged As Boolean
    ' PUBLIC Property Procedures (Begin)


    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    Public Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the form.
            Return m_lErrorNumber

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the current form's error number.
            m_lErrorNumber = Value

        End Set
    End Property


    Public Property FormCancelled() As Boolean
        Get

            ' Standard Property.

            ' Return if the form has been cancelled.
            Return m_bFormCancelled

        End Get
        Set(ByVal Value As Boolean)

            ' Standard Property.

            ' Set the form's cancelled flag.
            m_bFormCancelled = Value

        End Set
    End Property



    Public WriteOnly Property ObjectParam() As Object
        Set(ByVal Value As Object)

            ' Standard Property.

            ' Set the objects parameter value.


            m_vObjectParam = Value

        End Set
    End Property

    ' RDC 11072002
    ' RDC 11072002
    Public Property UnifiedLogon() As Boolean
        Get
            Return m_bUnifiedLogon
        End Get
        Set(ByVal Value As Boolean)
            m_bUnifiedLogon = Value
        End Set
    End Property

    ' RDC 11072002
    ' RDC 11072002
    Public Property UnifiedLogonUsername() As String
        Get
            Return m_sUnifiedLogonUsername
        End Get
        Set(ByVal Value As String)
            m_sUnifiedLogonUsername = Value
        End Set
    End Property

    ''' <summary>
    ''' To get and set Password changed by user
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PasswordChanged() As Boolean
        Get
            ' Standard Property.
            ' Return if the password has been changed.
            Return m_bPasswordChanged

        End Get
        Set(ByVal Value As Boolean)

            ' Standard Property.
            ' Set the password changed flag.
            m_bPasswordChanged = Value
        End Set
    End Property
    ' PRIVATE Property Procedures (End)

    ' RKS 131204 - DoUnifiedLogin
    ' ***************************************************************** '
    ' Name: DoUnifiedLogin
    '
    ' Description: Attempt to do UnifiedLogin
    '
    ' ***************************************************************** '
    Public Function DoUnifiedLogin() As Integer
        Dim result As Integer = 0
        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oGeneral.UnifiedLogon = m_bUnifiedLogon
            m_oGeneral.UnifiedLogonUsername = m_sUnifiedLogonUsername

            'Call the logon method to attempt to logon.
            lReturnValue = CType(m_oGeneral.Logon(), gPMConstants.PMEReturnCode)
            Select Case lReturnValue
                'Added - Login cancelled PN 18220, 18221 (During Client Install)
                Case gPMConstants.PMEReturnCode.PMTrue
                    FormCancelled = False
                    result = gPMConstants.PMEReturnCode.PMTrue
                Case gPMConstants.PMEReturnCode.PMUnifiedModeIncorrectUserName, gPMConstants.PMEReturnCode.PMUnifiedModeUserLoggedOnElsewhere, gPMConstants.PMEReturnCode.PMMixedModeUserLoggedOnElsewhere, gPMConstants.PMEReturnCode.PMCancel
                    FormCancelled = True
                    result = gPMConstants.PMEReturnCode.PMTrue
                Case Else
                    FormCancelled = False
                    result = gPMConstants.PMEReturnCode.PMFalse
            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "DoUnifiedLogin Failed", ACApp, ACClass, "DoUnifiedLogin", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function





    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: FormToPublic
    '
    ' Description: Updates all public variables from the form details.
    '
    ' ***************************************************************** '
    Public Function FormToPublic() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the public variables.

            g_bUnifiedLogon = m_bUnifiedLogon
            g_sUnifiedLogonUserName = m_sUnifiedLogonUsername

            If m_bUnifiedLogon Then
                g_sUserName = m_sUnifiedLogonUsername
                g_sPassword = ""
            Else
                g_sUserName = txtUserName.Text.Trim()
                g_sPassword = txtPassword.Text
            End If
            g_dLogonTime = DateTime.Now

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to update the public variables", ACApp, ACClass, "FormToPublic", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearForm
    '
    ' Description: Clears all of the forms details.
    '
    ' ***************************************************************** '
    Public Function ClearForm(ByRef lClearValue As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear form details

            If lClearValue = gPMConstants.PMEReturnCode.PMIncorrectUsername Then
                txtUserName.Text = ""
                txtPassword.Text = ""

                ' Set the focus.
                txtUserName.Focus()
            Else
                txtPassword.Text = ""

                ' Set the focus.
                txtPassword.Focus()
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to clear the forms details", ACApp, ACClass, "ClearForm", excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            Dim sVersion As String = ""
            Dim sSR As String = ""
            Dim sBuild As String = ""
            Dim vStringArray() As String
            Try

                sBuild = " Build " & m_sRelease.Substring(m_sRelease.LastIndexOf("."c) + 1)
                vStringArray = m_sRelease.Split("."c)
                sSR = " SR" & vStringArray(0)

                'Do Not Show Build Numbers.
                sVersion = m_sVersion & sSR & sBuild

                ' Set the version number and date
                lblVersion.Text = "Version " & sVersion
            Catch ex As Exception
                Exit Sub
            End Try
        End If
    End Sub

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)

    Private Sub Form_Initialize_Renamed()

        ' Forms Initialise Event.

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lErrorValue As Integer
        Dim bLocalEnabled, bServerEnabled As Boolean

        Try

            ' Load the instance of the general
            ' interface object into memory.
            m_oGeneral = New iLogonManager.General()

            ' Call the initialise method passing
            ' this form as the parameter.
            lErrorValue = m_oGeneral.Initialise(Me)

            ' Check for errors.
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to initialise the general interface object", ACApp, ACClass, "Initialise")

                Exit Sub
            End If

            ' Set the cancelled property to true
            FormCancelled = True

            ' Get the Local and Server Enabled settings
            bLocalEnabled = m_oGeneral.ArchitectureLocalEnabled()
            bServerEnabled = m_oGeneral.ArchitectureServerEnabled()

            ' If we can't logon locally OR to a Server we have an error
            If (Not bLocalEnabled) And (Not bServerEnabled) Then
                gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogError, "Unable to Logon - Neither Local or Server Logon is enabled.", ACApp, ACClass, "Initialise")
                ErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' get sirius version details
            lReturn = CType(gPMFunctions.GetSiriusVersion(m_sVersion, m_sRelease, m_sSiriusType), gPMConstants.PMEReturnCode)

            ' Initialise
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section

            ErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to initialise the form object", ACApp, ACClass, "Initialise", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        ' Click Event Of The OK Button.

        Dim lReturnValue As gPMConstants.PMEReturnCode

        Try

            ' Set the cancelled property to false
            FormCancelled = False

            ' Busy
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_oGeneral.UnifiedLogon = False
            m_oGeneral.ShowWarningessage = True
            ' Call the logon method to attempt to logon.
            lReturnValue = CType(m_oGeneral.Logon(), gPMConstants.PMEReturnCode)

            ' Normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Me.staStatus.Text = ""
            Me.staStatus.Refresh()

            ' Check logon return value.
            Select Case (lReturnValue)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Logon has been successful or the logon
                    ' attempts has been exceeded.

                    ' Hide me from the world.
                    Me.Hide()

                Case gPMConstants.PMEReturnCode.PMCancel
                    ' RFC18081998 - Some errors are returned as Cancel
                    ' RFC18091998 - e.g. LicenceLimitExceeded
                    ' RFC18081998 - Treat as a User Cancel
                    FormCancelled = True
                    Me.Hide()

                Case gPMConstants.PMEReturnCode.PMIncorrectUsername, gPMConstants.PMEReturnCode.PMIncorrectPassword, gPMConstants.PMEReturnCode.PMUserAccountLocked
                    ' Logon has failed with current username
                    ' or password.
                Case gPMConstants.PMEReturnCode.PMUserForceChangePassword
                    ' Make Change Password Frame Visible 	
                    pnlPasswordChange.Visible = True
                    pnlLogin.Visible = False
                    Text = "Change Password"
                    AcceptButton = cmdChangePassword
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Case Else
                    ' An error has occurred.

                    ErrorNumber = lReturnValue
                    Me.Hide()

            End Select

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the OK command button", ACApp, ACClass, "cmdOK_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click Event Of The OK Button.

        Try

            ' Set the cancelled property to true
            FormCancelled = True

            ' Destroy the instance of the client manager
            ' from memory.
            g_oClientManager = Nothing

            ' Hide me from the world.
            Me.Hide()

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the Cancel command button", ACApp, ACClass, "cmdCancel_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        ' Forms Terminate Event.

        Dim lErrorValue As Integer

        Try

            ' Call the terminate method
            m_oGeneral.Dispose()
            ' Destroy the instance of the general
            ' interface object from memory.
            m_oGeneral = Nothing

        Catch excep As System.Exception



            ' Error Section.

            ErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to terminate the form", ACApp, ACClass, "Form_Unload", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtPassword_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPassword.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        ' Password change event.

        cmdOK.Enabled = Not (txtUserName.Text.Trim().Length < 1 Or txtPassword.Text.Trim().Length < 1)

    End Sub

    Private Sub txtPassword_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPassword.Enter

        ' Password gotfocus event.

        txtPassword.SelectionStart = 0
        txtPassword.SelectionLength = Strings.Len(txtPassword.Text)

    End Sub

    Private Sub txtUserName_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtUserName.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        ' Username change event.

        cmdOK.Enabled = Not (txtUserName.Text.Trim().Length < 1 Or txtPassword.Text.Trim().Length < 1)

    End Sub

    Private Sub txtUserName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtUserName.Enter

        ' Username gotfocus event

        txtUserName.SelectionStart = 0
        txtUserName.SelectionLength = Strings.Len(txtUserName.Text)

    End Sub

    Private Sub frmInterface_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Dim UnloadMode As Integer = CInt(e.CloseReason)
        Try
            If UnloadMode <> vbFormCode Then
                ' Set the cancelled property to true
                FormCancelled = True

                ' Destroy the instance of the client manager
                ' from memory.
                g_oClientManager = Nothing

                ' Hide me from the world.
                Me.Hide()
            End If
        Catch excep As System.Exception
            ' Log Error.
            gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the Cancel command button", ACApp, ACClass, "cmdCancel_Click", excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdChangePassword_Click(sender As Object, e As EventArgs) Handles cmdChangePassword.Click

        ' Store the Password Details
        m_oGeneral.OldPassword = txtOldPassword.Text.Trim()
        m_oGeneral.NewPassword = txtNewPassword.Text.Trim()
        m_oGeneral.ConfirmPassword = txtConfirmPassword.Text.Trim()
        If txtOldPassword.Text = "" Then
            lblOldPassword.Visible = False
            lblNewPassword.Visible = False
            txtOldPassword.Visible = False
            txtNewPassword.Visible = False

            'Add Confirmation of Password
            lblConfirmPassword.Visible = False
            txtConfirmPassword.Visible = False
            txtOldPassword.Text = ""
            txtNewPassword.Text = ""
            txtConfirmPassword.Text = ""

            'disable the Change Password button
            cmdChangePassword.Enabled = False

            PasswordChanged = False
            Exit Sub
        End If
        ' Valide and Apply the new details.
        m_lErrorNumber = CType(m_oGeneral.VerifyAndUpdateProperties(bPasswordChanged:=m_bPasswordChanged, frmInterface:=Me), gPMConstants.PMEReturnCode)

        If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
            Exit Sub
        End If

        ' Store the new password
        g_sPassword = m_oGeneral.ConfirmPassword
        txtUserName.Text = g_sUserName
        txtPassword.Text = g_sPassword

        m_oGeneral.UnifiedLogon = False

        ' Call the logon method to attempt to logon.
        m_lErrorNumber = CType(m_oGeneral.Logon(), gPMConstants.PMEReturnCode)

        ' Normal
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        Me.staStatus.Text = ""
        Me.staStatus.Refresh()

        ' Check logon return value.
        Select Case (m_lErrorNumber)
            Case gPMConstants.PMEReturnCode.PMTrue
                ' Logon has been successful or the logon
                ' attempts has been exceeded.

                ' Hide me from the world.
                Me.Hide()

            Case gPMConstants.PMEReturnCode.PMCancel
                '  Some errors are returned as Cancel
                '  Treat as a User Cancel
                FormCancelled = True
                Me.Hide()

            Case gPMConstants.PMEReturnCode.PMIncorrectUsername, gPMConstants.PMEReturnCode.PMIncorrectPassword, gPMConstants.PMEReturnCode.PMUserAccountLocked
                ' Logon has failed with current username
                ' or password.
            Case gPMConstants.PMEReturnCode.PMUserForceChangePassword
                ' Make Change Password Frame Visible 	
                pnlPasswordChange.Visible = True
                pnlLogin.Visible = False
                Text = "Change Password"
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Case Else
                ' An error has occurred.

                ErrorNumber = m_lErrorNumber
                Me.Hide()

        End Select

        PasswordChanged = False
    End Sub


    Private Sub txtConfirmPassword_TextChanged(sender As Object, e As EventArgs) Handles txtConfirmPassword.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        ' Password change event.

        cmdChangePassword.Enabled = Not (txtOldPassword.Text.Trim().Length < 1 Or txtNewPassword.Text.Trim().Length < 1 Or txtConfirmPassword.Text.Trim().Length < 1)
        m_bPasswordChanged = True
    End Sub

    Private Sub cmdCancelChangePassword_Click(sender As Object, e As EventArgs) Handles cmdCancelChangePassword.Click

        ' Click Event Of The OK Button.

        Try

            ' Set the cancelled property to true
            FormCancelled = True

            ' Destroy the instance of the client manager
            ' from memory.
            g_oClientManager = Nothing

            ' Hide me from the world.
            Me.Hide()

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            gPMFunctions.LogMessagePopup(gPMConstants.PMELogLevel.PMLogOnError, "Failed to process the  Cancel ChangePassword", ACApp, ACClass, "cmdCancelChangePassword_Click", excep:=excep)

            Exit Sub

        End Try
    End Sub

End Class
