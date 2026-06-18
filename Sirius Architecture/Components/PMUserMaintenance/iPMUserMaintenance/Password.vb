Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Imports Artinsoft.VB6.Utils
Partial Public Class frmPassword
    Inherits System.Windows.Forms.Form

    Private m_bPasswordChanged As Boolean

    Public m_ofrmUserMaintenance As frmUserMaintenance
    Public ReadOnly Property PasswordChanged() As Boolean
        Get
            Return m_bPasswordChanged
        End Get
    End Property
    Public WriteOnly Property PasswordChnaged() As Boolean
        Set(ByVal Value As Boolean)
            m_bPasswordChanged = Value
        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_bPasswordChanged = False
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        If txtNewPassword.Text.Trim().Length = 0 And Not (g_iSystemSecurityModel <> 0 And g_bValidDomainAccount) Then
            MessageBox.Show("Please enter a valid Password. Blank passwords are " & "not allowed", "Password")
            txtNewPassword.Focus()
            Exit Sub
        End If

        If txtNewPassword.Text.Trim().Length < 6 Or txtNewPassword.Text.Trim().Length > 30 Then
            MessageBox.Show("Password should be between 6 to 30 characters ", "Password")
            txtNewPassword.Focus()
            Exit Sub
        End If

        If txtNewPassword.Text.Trim() = txtConfirmPassword.Text.Trim() Then
            ReflectionHelper.SetMember(Me.Owner, "Password", txtNewPassword.Text)
            DirectCast(Me.Owner.Controls.Find("SSTab1", True)(0).Controls("_SSTab1_TabPage1").Controls(0).Controls(4).Controls(1), Panel).Name = DateTime.Now
            DirectCast(Me.Owner.Controls.Find("SSTab1", True)(0).Controls("_SSTab1_TabPage1").Controls(0).Controls(4).Controls(1).Controls(0), Label).Text = DateTime.Now
            Me.Hide()
        Else
            MessageBox.Show("Confirmed Password Does Not Match " & "New Password Entered", "Password")
            txtConfirmPassword.Focus()
            Exit Sub
        End If

        m_bPasswordChanged = True

    End Sub
End Class