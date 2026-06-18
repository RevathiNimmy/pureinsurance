Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports System
Imports System.Windows.Forms
Friend Partial Class frmMessage
	Inherits System.Windows.Forms.Form
	Private Sub frmMessage_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	
	Public Sub GetMessage(ByRef sMessage As String, ByRef lSeconds As Integer)
		lblProgress.Text = sMessage
		If lSeconds * 1000 = 0 Then
			tmrTimer.Enabled = False
		Else
			tmrTimer.Interval = lSeconds * 1000
			tmrTimer.Enabled = True
		End If
		If (IIf(Not tmrTimer.Enabled, 0, tmrTimer.Interval)) > 0 Then
			tmrTimer.Enabled = True
			Me.Visible = True
		End If
	End Sub
	
	Private Sub tmrTimer_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrTimer.Tick
		tmrTimer.Enabled = False
		Me.Hide()
		Me.Close()
	End Sub
End Class