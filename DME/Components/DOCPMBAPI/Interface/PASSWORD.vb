Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Friend Partial Class frmPassword
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Me.Tag = CStr(False)
		txtPassword.Text = ""
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		If VB6.PixelsToTwipsY(pan3FormBox.Height) = 1995 Then
			If Not (txtPassword.Text = txtRePassword.Text) Then
				MessageBox.Show("Passwords Do Not Match", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				
				fra3RePassBox.Visible = False
				Me.Height = VB6.TwipsToPixelsY(1560)
				pan3FormBox.Height = VB6.TwipsToPixelsY(1155)
				txtPassword.Text = ""
				txtRePassword.Text = ""
				txtPassword.Focus()
				
				Exit Sub
			End If
			
			Me.Hide()
			
		Else
			
			'    If Len(txtPassword) < 4 Then
			'        MsgBox "Password to Short. 4 Characters or more", 48
			
			'        Exit Sub
			'    End If
			
			Me.Tag = CStr(True)
			Me.Height = VB6.TwipsToPixelsY(2385)
			pan3FormBox.Height = VB6.TwipsToPixelsY(1995)
			fra3RePassBox.Visible = True
			txtRePassword.Text = ""
			txtRePassword.Focus()
			
		End If
		
	End Sub
	

	Private Sub frmPassword_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'TODO
        'CenterForm(Me)
		
		Me.Tag = CStr(True)
		Me.fra3RePassBox.Visible = False
		Me.pan3FormBox.Height = VB6.TwipsToPixelsY(1155)
		Me.Height = VB6.TwipsToPixelsY(1560)
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub txtPassword_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPassword.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		'    If Len(txtPassword.Text) > 12 Then
		'            cmdOK_Click
		'    End If
		
	End Sub
	
	Private Sub txtPassword_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtPassword.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		
		If KeyAscii = 9 Then
			cmdOK_Click(cmdOK, New EventArgs())
		End If
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	Private Sub txtPassword_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles txtPassword.KeyUp
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		
		If KeyCode = KEY_TAB Then
			cmdOK_Click(cmdOK, New EventArgs())
		End If
		
	End Sub
	
	Private Sub txtRePassword_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRePassword.TextChanged
		If isInitializingComponent Then
			Exit Sub
		End If
		
		If Strings.Len(txtRePassword.Text) > 12 Then
			cmdOK_Click(cmdOK, New EventArgs())
		End If
		
	End Sub
	
	Private Sub txtRePassword_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRePassword.Enter
		
		If VB6.PixelsToTwipsY(pan3FormBox.Height) = 1155 Then
			cmdOK_Click(cmdOK, New EventArgs())
		End If
		
	End Sub
End Class