Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class FrmVerify
	Inherits System.Windows.Forms.Form
	Private Sub CmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdCancel.Click
		
		iVerify = gPMConstants.PMEReturnCode.PMCancel
		
        Me.Hide()
		
	End Sub
	Private Sub CmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdOK.Click
		
		'declare variables
		Dim sEncryptedPassword, sCorrectPassword As String
		
		'store password
		Dim sPassword As String = TxtPassword.Text
		
		'encrypt password
		Dim m_lreturn As Object = m_oDOCPassword.EncryptPassword(sPassword:=sPassword, sEncryptedPassword:=sEncryptedPassword)
		
		'call GetNodePassword

		m_lreturn = m_oDOCPassword.GetNodePassword(lNodeNum:=lCurrentNum, iNodeLevel:=iCurrentLevel, sPassword:=sCorrectPassword)
		
		'Check see password is valid (check stored password)
		If sEncryptedPassword <> sCorrectPassword Then
			'Display error message box if incorrect, reset text boxes
			'else enter document/folder
			Interaction.Beep()
			MessageBox.Show("Incorrect Password, try again", "Password", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			TxtPassword.Text = ""
			TxtPassword.Focus()
		Else
			Me.Cursor = Cursors.WaitCursor
			iVerify = gPMConstants.PMEReturnCode.PMOK
            Me.Hide()
		End If
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub TxtPassword_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TxtPassword.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		CmdOK.Enabled = Strings.Len(TxtPassword.Text) > 0
		
	End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub FrmVerify_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        TxtPassword.Text = ""
    End Sub
End Class