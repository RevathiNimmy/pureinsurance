Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class frmAnnotation
	Inherits System.Windows.Forms.Form
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		txtAnnotation.Text = ""
		Me.Hide()
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Me.Hide()
		
	End Sub
	
	Private Sub frmAnnotation_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		Cancel = (Not bAppClosing)
		
		eventArgs.Cancel = Cancel <> 0
	End Sub
End Class