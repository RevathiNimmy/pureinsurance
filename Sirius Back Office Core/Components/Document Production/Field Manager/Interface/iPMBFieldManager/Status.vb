Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class frmStatus
	Inherits System.Windows.Forms.Form
	
	Public Sub ShowStatus(ByRef sMessage As String)
		
		lblStatus.Text = sMessage
		
		Me.Show()
		Me.Refresh()
        
        'TopMost.SetTopmost(Me)
        SetTopmost(Me)
		
	End Sub
	
	Public Sub ClearStatus()
		
        'TopMost.ClearTopmost(Me)
        ClearTopmost(Me)
		
		Me.Hide()
		
	End Sub
End Class