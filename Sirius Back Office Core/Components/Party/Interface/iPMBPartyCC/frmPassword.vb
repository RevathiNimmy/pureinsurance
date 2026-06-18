Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles.gPMConstants

Public Class frmPassword

    Private nStatus As PMEReturnCode
    Public ReadOnly Property Status() As PMEReturnCode
        Get
            Return nStatus
        End Get
    End Property

    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        Dim sPassword As String = txtPassword.Text.Trim()
        If sPassword.Length = 0 Then
            MessageBox.Show("Please enter password", "Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        ElseIf sPassword.Length > 15 Then
            MessageBox.Show("Password length cannot be greater than 15 digits", "Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            nStatus = PMEReturnCode.PMOK
            Me.Hide()
        End If
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        nStatus = PMEReturnCode.PMCancel
        txtPassword.Text = String.Empty
        Me.Hide()
    End Sub

    Private Sub frmPassword_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Dim nCancel As Integer = IIf(e.Cancel, 1, 0)
        Dim nUnloadMode As Integer = Convert.ToInt32(e.CloseReason)
        'User cancel
        If nUnloadMode <> 0 Then
            nStatus = PMEReturnCode.PMCancel
            txtPassword.Text = String.Empty
            Me.Hide()
            e.Cancel = nCancel <> 0
        End If
    End Sub
End Class