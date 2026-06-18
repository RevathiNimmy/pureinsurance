Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Friend Partial Class frmGetPassword
	Inherits System.Windows.Forms.Form
    'developer guide no.101
    Dim f_PrevMousePointer As Cursor
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		txtPassword.Text = ""
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Me.Hide()
		
	End Sub
	
	Private Sub frmGetPassword_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Enter
		
		txtPassword.Focus()
		
	End Sub
	

	Private Sub frmGetPassword_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'TODO
        'CenterForm(Me)
		
		f_PrevMousePointer = Me.Cursor
		Me.Cursor = Cursors.Default
		
	End Sub
	
	Private Sub frmGetPassword_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		

		Me.Cursor = f_PrevMousePointer
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub txtPassword_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPassword.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		cmdOK.Enabled = (Strings.Len(txtPassword.Text) > 0)
		
	End Sub
End Class