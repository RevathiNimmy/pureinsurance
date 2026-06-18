Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Friend Partial Class frmDialogMsg
	Inherits System.Windows.Forms.Form
	
	
	Private m_iClickAction As Integer
	Public ReadOnly Property ClickAction() As Integer
		Get
			Return m_iClickAction
		End Get
	End Property
	
	Private Sub CancelButton_Renamed_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CancelButton_Renamed.Click
		m_iClickAction = 0
		Me.Hide()
	End Sub
	
	Private Sub OKButton_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles OKButton.Click
		m_iClickAction = 1
		Me.Hide()
	End Sub
End Class